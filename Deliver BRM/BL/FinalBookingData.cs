using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using System.IO;
using OfficeOpenXml;
using System.Globalization;
using System.Security.Cryptography;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;


namespace SalesTrackerAPI.BL
{
    public class FinalBookingData
    {

      

        public enum DelTranTablesE
        {
            tempInvoiceHistory,
            tempInvoiceItemTaxDtls,
            tempInvoiceItemDetails,
            tempInvoiceAddress,
            tempInvoice,
            tempLoadingSlipExtHistory,
            tempLoadingSlipExt,
            tempLoadingSlipDtl,
            tempLoadingSlipAddress,
            tempLoadingSlip,
            tempWeighingMeasures,
            tempLoadingStatusHistory,
            tempLoading,
            tempInvoiceDocumentDetails, //Vijaymala added [04-06-2018]
            tempEInvoiceApiResponse //Dhananjay added [23-12-2020]
        }

        


        #region Insertion Method

        public static ResultMessage InsertFinalBookingData(int loadingId, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            TblLoadingTO loadingTO = new TblLoadingTO();
            List<TblLoadingSlipTO> loadingSlipTOList = new List<TblLoadingSlipTO>();
            List<TblLoadingSlipTO> bookingTOList = new List<TblLoadingSlipTO>();
            List<TblLoadingStatusHistoryTO> loadingStatusHistoryTOList = new List<TblLoadingStatusHistoryTO>();
            List<TblWeighingMeasuresTO> weighingMeasuresTOList = new List<TblWeighingMeasuresTO>();
            List<TblLoadingSlipExtTO> oldLoadingSlipExtTOList = new List<TblLoadingSlipExtTO>();

            // For keeping history of loadinSlipExt.
            var loadinSlipExthistoryList = new List<KeyValuePair<int, int>>();
            // For keeping history of invoiceItemDetails.
            var invoiceItemDetailshistoryList = new List<KeyValuePair<int, int>>();

            int oldLoadingSlipId = 0;
            double totalLoadingQty = 0;
            int result = 0;

            try
            {

                #region Selection

                // Select temp loading details.
                loadingTO = TblLoadingBL.SelectTblLoadingTO(loadingId, conn, tran);
                if (loadingTO == null)
                {
                    resultMessage.DefaultBehaviour("Error Booking loadingTO is null");
                    return resultMessage;
                }

                // Select temp loading slip details.
                loadingSlipTOList = TblLoadingSlipBL.SelectAllLoadingSlipListWithDetails(loadingId, conn, tran);
                // Select loading slip status history data.
                loadingStatusHistoryTOList = TblLoadingStatusHistoryBL.SelectAllTblLoadingStatusHistoryList(loadingId, conn, tran);
                // Select weighingMeasures details.
                weighingMeasuresTOList = TblWeighingMeasuresBL.SelectAllTblWeighingMeasuresListByLoadingId(loadingId, conn, tran);

                #endregion

                // Filter loading slip list with confirm flag.
                if (loadingSlipTOList != null && loadingSlipTOList.Count > 0)
                    bookingTOList = loadingSlipTOList.FindAll(ele => ele.IsConfirmed == 1).ToList();


                // Insert final loading slip details.
                if (bookingTOList != null && bookingTOList.Count > 0)
                {
                    // Insert final loading details.
                    // Set total loading qty to 0.
                    loadingTO.TotalLoadingQty = 0;
                    loadingTO.NoOfDeliveries = bookingTOList.Count;

                    result = InsertFinalLoading(loadingTO, conn, tran);
                    if (result <= 0)
                    {
                        resultMessage.DefaultBehaviour("Error while Booking InsertFinalLoading");
                        return resultMessage;
                    }

                    // Maintain history loadinSlipExtTO.
                    TblLoadingSlipExtTO oldLoadingSlipExtTO = new TblLoadingSlipExtTO();
                    int loadingSlipExtTOCnt = 1;

                    foreach (var loadingSlipTO in bookingTOList)
                    {
                        List<TblLoadingSlipExtTO> newLoadingSlipExtTOList = new List<TblLoadingSlipExtTO>();
                        // Set old loading slip id.
                        oldLoadingSlipId = loadingSlipTO.IdLoadingSlip;

                        // Select invoice details.
                        List<TblInvoiceTO> invoiceTOList = TblInvoiceBL.SelectTempInvoiceTOList(loadingSlipTO.IdLoadingSlip, conn, tran);


                        // Insert loading slip.
                        loadingSlipTO.LoadingId = loadingTO.IdLoading;
                        loadingSlipTO.NoOfDeliveries = bookingTOList.Count;
                        result = InsertFinalLoadingSlip(loadingSlipTO, conn, tran);

                        if (result <= 0)
                        {
                            resultMessage.DefaultBehaviour("Error while Booking InsertFinalLoadingSlip");
                            return resultMessage;
                        }

                        // Insert loading slip address details.
                        if (loadingSlipTO.DeliveryAddressTOList != null && loadingSlipTO.DeliveryAddressTOList.Count > 0)
                        {
                            foreach (var loadingSlipAddressTO in loadingSlipTO.DeliveryAddressTOList)
                            {
                                loadingSlipAddressTO.LoadingSlipId = loadingSlipTO.IdLoadingSlip;
                                result = InsertFinalLoadingSlipAddress(loadingSlipAddressTO, conn, tran);
                                if (result <= 0)
                                {
                                    resultMessage.DefaultBehaviour("Error while Booking InsertFinalLoadingSlipAddress");
                                    return resultMessage;
                                }
                            }
                        }

                        // Insert loading slip details data.
                        if (loadingSlipTO.TblLoadingSlipDtlTO != null)
                        {
                            loadingSlipTO.TblLoadingSlipDtlTO.LoadingSlipId = loadingSlipTO.IdLoadingSlip;
                            totalLoadingQty = totalLoadingQty + loadingSlipTO.TblLoadingSlipDtlTO.LoadingQty;

                            result = InsertFinalLoadingSlipDtl(loadingSlipTO.TblLoadingSlipDtlTO, conn, tran);
                            if (result <= 0)
                            {
                                resultMessage.DefaultBehaviour("Error while Booking InsertFinalLoadingSlipDtl");
                                return resultMessage;
                            }
                        }

                        // Insert loading slip ext details.
                        if (loadingSlipTO.LoadingSlipExtTOList != null && loadingSlipTO.LoadingSlipExtTOList.Count > 0)
                        {
                            //Sanjay Gunjal [03-July-2021] Added ordering of list by weighing sequence number as it was doing wrong tare weight calculation.
                            var extList = loadingSlipTO.LoadingSlipExtTOList.OrderBy(ws => ws.WeighingSequenceNumber).ToList();

                            foreach (var loadinSlipExtTO in extList)
                            {
                                // Insert to old loadingSlipExtTOList.
                                oldLoadingSlipExtTOList.Add(loadinSlipExtTO);

                                // Select loading slip ext history data.
                                List<TblLoadingSlipExtHistoryTO> loadingSlipExtHistoryTOList = TblLoadingSlipExtHistoryBL.SelectTempLoadingSlipExtHistoryTOList(loadinSlipExtTO.IdLoadingSlipExt, conn, tran);

                                loadinSlipExtTO.LoadingSlipId = loadingSlipTO.IdLoadingSlip;
                                int oldloadinSlipExtId = loadinSlipExtTO.IdLoadingSlipExt;

                                // Calculate calcTareWeight.                              
                                if (loadingSlipExtTOCnt > 1)
                                {
                                    loadinSlipExtTO.CalcTareWeight = oldLoadingSlipExtTO.CalcTareWeight + oldLoadingSlipExtTO.LoadedWeight;
                                }

                                // Make adjustment of CalcTareWeight.
                                if (loadingSlipExtTOCnt == 1 && loadinSlipExtTO.CalcTareWeight > loadinSlipExtTO.LoadedWeight)
                                {
                                    List<TblLoadingSlipExtTO> oldAllLoadingSlipExtTOList = TblLoadingSlipExtBL.SelectAllLoadingSlipExtListFromLoadingId(loadingId, conn, tran);

                                    if (oldAllLoadingSlipExtTOList != null && oldAllLoadingSlipExtTOList.Count > 0)
                                        //Sanjay Gunjal [03-July-2021] Added ordering of list by CalcTareWeight as it was doing wrong tare weight calculation.
                                        loadinSlipExtTO.CalcTareWeight = oldAllLoadingSlipExtTOList.OrderBy(tw => tw.CalcTareWeight).FirstOrDefault().CalcTareWeight;
                                }

                                result = InsertFinalLoadingSlipExt(loadinSlipExtTO, conn, tran);
                                if (result <= 0)
                                {
                                    resultMessage.DefaultBehaviour("Error while Booking InsertFinalLoadingSlipExt");
                                    return resultMessage;
                                }
                                else
                                {
                                    loadinSlipExthistoryList.Add(new KeyValuePair<int, int>(oldloadinSlipExtId, loadinSlipExtTO.IdLoadingSlipExt));

                                    // Assign old loadinSlipExtTO.
                                    oldLoadingSlipExtTO = loadinSlipExtTO;
                                    newLoadingSlipExtTOList.Add(loadinSlipExtTO);
                                }

                                // Update loadinSlipExtId in tblLoadingQuotaConsumption.
                                result = UpdateTblLoadingQuotaConsumption(oldloadinSlipExtId, loadinSlipExtTO.IdLoadingSlipExt, conn, tran);
                                if (result < 0)
                                {
                                    resultMessage.DefaultBehaviour("Error while Booking UpdateTblLoadingQuotaConsumption");
                                    return resultMessage;
                                }

                                result = UpdateTblStockConsumption(oldloadinSlipExtId, loadinSlipExtTO.IdLoadingSlipExt, conn, tran);
                                if (result < 0)
                                {
                                    resultMessage.DefaultBehaviour("Error while Booking UpdateTblStockConsumption");
                                    return resultMessage;
                                }

                                // Insert loading slip ext history details.
                                if (loadingSlipExtHistoryTOList != null && loadingSlipExtHistoryTOList.Count > 0)
                                {
                                    foreach (var loadinSlipExtHistoryTO in loadingSlipExtHistoryTOList)
                                    {
                                        loadinSlipExtHistoryTO.LoadingSlipExtId = loadinSlipExtTO.IdLoadingSlipExt;
                                        result = InsertFinalLoadingSlipExtHistory(loadinSlipExtHistoryTO, conn, tran);
                                        if (result <= 0)
                                        {
                                            resultMessage.DefaultBehaviour("Error while Booking InsertFinalLoadingSlipExtHistory");
                                            return resultMessage;
                                        }
                                    }
                                }
                                loadingSlipExtTOCnt++;
                            }
                        }

                        // Insert invoice details.
                        if (invoiceTOList != null && invoiceTOList.Count > 0)
                        {
                            foreach (var invoiceTO in invoiceTOList)
                            {
                                //Added Dhananjay [23-12-2020] Select invoice eInvoice API response.
                                List<TblEInvoiceApiResponseTO> eInvoiceApiResponseTOList = TblEInvoiceApiResponseBL.SelectTblEInvoiceApiResponseListForInvoiceId(invoiceTO.IdInvoice, conn, tran);
                                // Select invoice address details.
                                List<TblInvoiceAddressTO> invoiceAddressTOList = TblInvoiceAddressBL.SelectAllTblInvoiceAddressList(invoiceTO.IdInvoice, conn, tran);
                                // Select invoice item details.
                                List<TblInvoiceItemDetailsTO> invoiceItemDetailsTOList = TblInvoiceItemDetailsBL.SelectAllTblInvoiceItemDetailsList(invoiceTO.IdInvoice, conn, tran);
                                // Select invoice history details.
                                List<TblInvoiceHistoryTO> invoiceHistoryTOList = TblInvoiceHistoryBL.SelectTempInvoiceHistory(invoiceTO.IdInvoice, conn, tran);
                                invoiceTO.LoadingSlipId = loadingSlipTO.IdLoadingSlip;

                                //[28-05-2018]:Vijaymla added to get invoice document details
                                List<TempInvoiceDocumentDetailsTO> tempInvoiceDocumentDetailsTOList = TempInvoiceDocumentDetailsBL.SelectTempInvoiceDocumentDetailsByInvoiceId(invoiceTO.IdInvoice, conn, tran);

                                // Weighing calculation.
                                if (newLoadingSlipExtTOList != null)
                                {
                                    //invoiceTO.TareWeight = newLoadingSlipExtTOList.Select(ele => ele.CalcTareWeight).First();
                                    //invoiceTO.NetWeight = newLoadingSlipExtTOList.Sum(ele => ele.LoadedWeight);
                                    //invoiceTO.GrossWeight = newLoadingSlipExtTOList.Select(ele => ele.LoadedWeight).Last() + newLoadingSlipExtTOList.Select(ele => ele.CalcTareWeight).Last();


                                    invoiceTO.TareWeight = newLoadingSlipExtTOList.OrderBy(o => o.CalcTareWeight).FirstOrDefault().CalcTareWeight;
                                    invoiceTO.NetWeight = newLoadingSlipExtTOList.Sum(ele => ele.LoadedWeight);
                                    invoiceTO.GrossWeight = invoiceTO.TareWeight + invoiceTO.NetWeight;

                                }

                                result = InsertFinalInvoice(invoiceTO, conn, tran);
                                if (result <= 0)
                                {
                                    resultMessage.DefaultBehaviour("Error while Booking InsertFinalInvoice");
                                    return resultMessage;
                                }

                                //Added Dhananjay [23-12-2020] insert invoice eInvoice API response.
                                if (eInvoiceApiResponseTOList != null && eInvoiceApiResponseTOList.Count > 0)
                                {
                                    foreach (var eInvoiceApiResponseTO in eInvoiceApiResponseTOList)
                                    {
                                        eInvoiceApiResponseTO.InvoiceId = invoiceTO.IdInvoice;
                                        result = InsertFinalEInvoiceApiResponse(eInvoiceApiResponseTO, conn, tran);
                                        if (result <= 0)
                                        {
                                            resultMessage.DefaultBehaviour("Error while Booking InsertFinalEInvoiceApiResponse");
                                            return resultMessage;
                                        }
                                    }
                                }

                                // Insert invoice address details.
                                if (invoiceAddressTOList != null && invoiceAddressTOList.Count > 0)
                                {
                                    foreach (var invoiceAddressTO in invoiceAddressTOList)
                                    {
                                        invoiceAddressTO.InvoiceId = invoiceTO.IdInvoice;
                                        result = InsertFinalInvoiceAddress(invoiceAddressTO, conn, tran);
                                        if (result <= 0)
                                        {
                                            resultMessage.DefaultBehaviour("Error while Booking InsertFinalInvoiceAddress");
                                            return resultMessage;
                                        }
                                    }
                                }

                                //[28-05-2018]Vijaymala added to insert into final invoice document details

                                if (tempInvoiceDocumentDetailsTOList != null && tempInvoiceDocumentDetailsTOList.Count > 0)
                                {
                                    foreach (var tempInvoiceDocumentDetailsTO in tempInvoiceDocumentDetailsTOList)
                                    {
                                        tempInvoiceDocumentDetailsTO.InvoiceId = invoiceTO.IdInvoice;
                                        result = InsertFinalInvoiceDocumentDetails(tempInvoiceDocumentDetailsTO, conn, tran);
                                        if (result <= 0)
                                        {
                                            resultMessage.DefaultBehaviour("Error while Enquiry InsertFinalInvoiceDocumentDetails");
                                            return resultMessage;
                                        }
                                    }
                                }

                                if (invoiceItemDetailsTOList != null && invoiceItemDetailsTOList.Count > 0)
                                {
                                    foreach (var invoiceItemDetailsTO in invoiceItemDetailsTOList)
                                    {

                                        // Select invoice item tax details.
                                        List<TblInvoiceItemTaxDtlsTO> invoiceItemTaxDtlsTOList = TblInvoiceItemTaxDtlsBL.SelectAllTblInvoiceItemTaxDtlsList(invoiceItemDetailsTO.IdInvoiceItem, conn, tran);

                                        invoiceItemDetailsTO.InvoiceId = invoiceTO.IdInvoice;
                                        int oldInvoiceItemId = invoiceItemDetailsTO.IdInvoiceItem;

                                        if (loadinSlipExthistoryList != null && loadinSlipExthistoryList.Count > 0)
                                            invoiceItemDetailsTO.LoadingSlipExtId = loadinSlipExthistoryList.Find(ele => ele.Key == invoiceItemDetailsTO.LoadingSlipExtId).Value;

                                        result = InsertFinalInvoiceItemDetails(invoiceItemDetailsTO, conn, tran);
                                        if (result <= 0)
                                        {
                                            resultMessage.DefaultBehaviour("Error while Booking InsertFinalInvoiceItemDetails");
                                            return resultMessage;
                                        }
                                        else
                                        {
                                            invoiceItemDetailshistoryList.Add(new KeyValuePair<int, int>(oldInvoiceItemId, invoiceItemDetailsTO.IdInvoiceItem));
                                        }

                                        if (invoiceItemTaxDtlsTOList != null && invoiceItemTaxDtlsTOList.Count > 0)
                                        {
                                            foreach (var invoiceItemTaxDtlsTO in invoiceItemTaxDtlsTOList)
                                            {
                                                invoiceItemTaxDtlsTO.InvoiceItemId = invoiceItemDetailsTO.IdInvoiceItem;
                                                result = InsertFinalInvoiceItemTaxDtls(invoiceItemTaxDtlsTO, conn, tran);
                                                if (result <= 0)
                                                {
                                                    resultMessage.DefaultBehaviour("Error while Booking InsertFinalInvoiceItemTaxDtls");
                                                    return resultMessage;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (invoiceHistoryTOList != null && invoiceHistoryTOList.Count > 0)
                                {
                                    foreach (var invoiceHistoryTO in invoiceHistoryTOList)
                                    {
                                        invoiceHistoryTO.InvoiceId = invoiceTO.IdInvoice;

                                        if (invoiceItemDetailshistoryList != null && invoiceItemDetailshistoryList.Count > 0)
                                            invoiceHistoryTO.InvoiceItemId = invoiceItemDetailshistoryList.Find(ele => ele.Key == invoiceHistoryTO.InvoiceItemId).Value;

                                        result = InsertFinalInvoiceHistory(invoiceHistoryTO, conn, tran);
                                        if (result <= 0)
                                        {
                                            resultMessage.DefaultBehaviour("Error while Booking InsertFinalInvoiceHistory");
                                            return resultMessage;
                                        }
                                    }
                                }
                            }
                        }
                    }


                    // Insert loading weighingMeasures details.
                    if (weighingMeasuresTOList != null && weighingMeasuresTOList.Count > 0)
                    {
                        // Filter weighingMeasures list by weightMeasureId from loadingSlipExtTOList.
                        List<TblWeighingMeasuresTO> newWeighingMeasuresTOList = weighingMeasuresTOList.Where(item =>
                          oldLoadingSlipExtTOList.Any(ele => ele.WeightMeasureId == item.IdWeightMeasure)).ToList();

                        Double totalWeightMT = 0;


                        // First insert tare weight.
                        List<TblWeighingMeasuresTO> tareWeighingMeasuresTOList = weighingMeasuresTOList.FindAll(ele => ele.WeightMeasurTypeId == (int)Constants.TransMeasureTypeE.TARE_WEIGHT);

                        if (tareWeighingMeasuresTOList != null && tareWeighingMeasuresTOList.Count > 0)
                        {
                            foreach (var tareWeighingMeasuresTO in tareWeighingMeasuresTOList)
                            {
                                totalWeightMT = tareWeighingMeasuresTO.WeightMT;
                                tareWeighingMeasuresTO.LoadingId = loadingTO.IdLoading;

                                result = InsertFinalWeighingMeasures(tareWeighingMeasuresTO, conn, tran);

                                if (result <= 0)
                                {
                                    resultMessage.DefaultBehaviour("Error while Booking InsertFinalWeighingMeasures");
                                    return resultMessage;
                                }
                            }
                        }

                        foreach (var weighingMeasuresTO in newWeighingMeasuresTOList)
                        {
                            // Calculate only booking weighing.
                            int count = oldLoadingSlipExtTOList.Count(ele => ele.WeightMeasureId == weighingMeasuresTO.IdWeightMeasure);
                            if (count > 1)
                            {
                                Double loadedWeight = oldLoadingSlipExtTOList.Where(ele => ele.WeightMeasureId == weighingMeasuresTO.IdWeightMeasure).Sum(ele => ele.LoadedWeight);
                                totalWeightMT = totalWeightMT + loadedWeight;
                            }
                            else
                            {
                                Double loadedWeight = oldLoadingSlipExtTOList.Find(ele => ele.WeightMeasureId == weighingMeasuresTO.IdWeightMeasure).LoadedWeight;
                                totalWeightMT = totalWeightMT + loadedWeight;
                            }

                            weighingMeasuresTO.LoadingId = loadingTO.IdLoading;
                            weighingMeasuresTO.WeightMT = totalWeightMT;
                            int oldWeighingMeasuresId = weighingMeasuresTO.IdWeightMeasure;

                            result = InsertFinalWeighingMeasures(weighingMeasuresTO, conn, tran);
                            if (result <= 0)
                            {
                                resultMessage.DefaultBehaviour("Error while Booking InsertFinalWeighingMeasures");
                                return resultMessage;
                            }

                            // Update weighingMeasureid in finalLoadingSlipExt table.
                            result = UpdateFinalLoadingSlipExt(oldWeighingMeasuresId, weighingMeasuresTO.IdWeightMeasure, conn, tran);
                            if (result < 0)
                            {
                                resultMessage.DefaultBehaviour("Error while Booking UpdateFinalLoadingSlipExt");
                                return resultMessage;
                            }
                        }

                        // Insert gross weight.
                        List<TblWeighingMeasuresTO> grossWeighingMeasuresTOList = weighingMeasuresTOList.FindAll(ele => ele.WeightMeasurTypeId == (int)Constants.TransMeasureTypeE.GROSS_WEIGHT);

                        if (grossWeighingMeasuresTOList != null && grossWeighingMeasuresTOList.Count > 0)
                        {
                            int weightCnt = 0;
                            double grossWeighingMeasuresDiff = 0;

                            foreach (var grossWeighingMeasuresTO in grossWeighingMeasuresTOList)
                            {

                                // Gross weight calculation for 2 weighing machine only.
                                weightCnt++;
                                if (grossWeighingMeasuresTOList.Count > 1 && weightCnt == 1)
                                {
                                    grossWeighingMeasuresDiff = grossWeighingMeasuresTOList[weightCnt].WeightMT - grossWeighingMeasuresTOList[weightCnt - 1].WeightMT;
                                }

                                if (weightCnt > 1)
                                {
                                    totalWeightMT = totalWeightMT + grossWeighingMeasuresDiff;
                                }

                                grossWeighingMeasuresTO.WeightMT = totalWeightMT;
                                grossWeighingMeasuresTO.LoadingId = loadingTO.IdLoading;

                                result = InsertFinalWeighingMeasures(grossWeighingMeasuresTO, conn, tran);

                                if (result <= 0)
                                {
                                    resultMessage.DefaultBehaviour("Error while Booking InsertFinalWeighingMeasures");
                                    return resultMessage;
                                }
                            }
                        }
                    }

                    // Insert loading status history.
                    if (loadingStatusHistoryTOList != null && loadingStatusHistoryTOList.Count > 0)
                    {
                        foreach (var loadingStatusHistoryTO in loadingStatusHistoryTOList)
                        {
                            loadingStatusHistoryTO.LoadingId = loadingTO.IdLoading;
                            result = InsertFinalLoadingStatusHistory(loadingStatusHistoryTO, conn, tran);
                            if (result <= 0)
                            {
                                resultMessage.DefaultBehaviour("Error while Booking InsertFinalLoadingStatusHistory");
                                return resultMessage;
                            }
                        }
                    }


                    // Update total loading qty.
                    result = UpdateTotalLoadingQty(totalLoadingQty, loadingTO.IdLoading, conn, tran);
                    if (result < 0)
                    {
                        resultMessage.DefaultBehaviour("Error while Booking UpdateTotalLoadingQty");
                        return resultMessage;
                    }

                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertFinalBookingData");
                return resultMessage;
            }
            finally
            {
                oldLoadingSlipExtTOList = null;
                loadinSlipExthistoryList = null;
                invoiceItemDetailshistoryList = null;
            }
        }

        #endregion

        #region Insertion Commands 

        private static int InsertFinalLoading(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                tblLoadingTO.ModeId = Constants.getModeIdConfigTO();
                return ExecuteInsertionFinalLoadingCommand(tblLoadingTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        private static int ExecuteInsertionFinalLoadingCommand(TblLoadingTO tblLoadingTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [finalLoading]( " +
                                "  [isJointDelivery]" +
                                " ,[noOfDeliveries]" +
                                " ,[statusId]" +
                                " ,[createdBy]" +
                                " ,[updatedBy]" +
                                " ,[statusDate]" +
                                " ,[loadingDatetime]" +
                                " ,[createdOn]" +
                                " ,[updatedOn]" +
                                " ,[loadingSlipNo]" +
                                " ,[vehicleNo]" +
                                " ,[statusReason]" +
                                " ,[cnfOrgId]" +
                                " ,[totalLoadingQty]" +
                                " ,[statusReasonId]" +
                                " ,[transporterOrgId]" +
                                " ,[freightAmt]" +
                                " ,[superwisorId]" +
                                " ,[isFreightIncluded]" +
                                " ,[contactNo]" +
                                " ,[driverName]" +
                                " ,[parentLoadingId]" +
                                " ,[callFlag]" +
                                " ,[flagUpdatedOn]" +
                                " ,[isAllowNxtLoading]" +
                                " ,[loadingType]" +
                                " ,[currencyId]" +
                                " ,[currencyRate]" +
                                " ,[callFlagBy]" +
                                " ,[modeId]" +
                                " )" +
                    " VALUES (" +
                                "  @IsJointDelivery " +
                                " ,@NoOfDeliveries " +
                                " ,@StatusId " +
                                " ,@CreatedBy " +
                                " ,@UpdatedBy " +
                                " ,@StatusDate " +
                                " ,@LoadingDatetime " +
                                " ,@CreatedOn " +
                                " ,@UpdatedOn " +
                                " ,@LoadingSlipNo " +
                                " ,@VehicleNo " +
                                " ,@StatusReason " +
                                " ,@cnfOrgId " +
                                " ,@totalLoadingQty " +
                                " ,@statusReasonId " +
                                " ,@transporterOrgId " +
                                " ,@freightAmt " +
                                " ,@superwisorId " +
                                " ,@isFreightIncluded " +
                                " ,@contactNo " +
                                " ,@driverName " +
                                " ,@parentLoadingId " +
                                " ,@callFlag " +
                                " ,@flagUpdatedOn " +
                                " ,@isAllowNxtLoading " +
                                " ,@loadingType " +
                                " ,@currencyId " +
                                " ,@currencyRate " +
                                " ,@callFlagBy " +
                                " ,@ModeId " +
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdLoading", System.Data.SqlDbType.Int).Value = tblLoadingTO.IdLoading;
            cmdInsert.Parameters.Add("@IsJointDelivery", System.Data.SqlDbType.Int).Value = tblLoadingTO.IsJointDelivery;
            cmdInsert.Parameters.Add("@NoOfDeliveries", System.Data.SqlDbType.Int).Value = tblLoadingTO.NoOfDeliveries;
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblLoadingTO.StatusId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLoadingTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.UpdatedBy);
            cmdInsert.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblLoadingTO.StatusDate;
            cmdInsert.Parameters.Add("@LoadingDatetime", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.LoadingDatetime);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.UpdatedOn);
            cmdInsert.Parameters.Add("@LoadingSlipNo", System.Data.SqlDbType.VarChar).Value = tblLoadingTO.LoadingSlipNo;
            cmdInsert.Parameters.Add("@VehicleNo", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.VehicleNo);
            cmdInsert.Parameters.Add("@StatusReason", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.StatusReason);
            cmdInsert.Parameters.Add("@cnfOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.CnfOrgId);
            cmdInsert.Parameters.Add("@totalLoadingQty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.TotalLoadingQty);
            cmdInsert.Parameters.Add("@statusReasonId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.StatusReasonId);
            cmdInsert.Parameters.Add("@transporterOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.TransporterOrgId);
            cmdInsert.Parameters.Add("@freightAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.FreightAmt);
            cmdInsert.Parameters.Add("@superwisorId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.SuperwisorId);
            cmdInsert.Parameters.Add("@isFreightIncluded", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.IsFreightIncluded);
            cmdInsert.Parameters.Add("@contactNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.ContactNo);
            cmdInsert.Parameters.Add("@driverName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.DriverName);
            cmdInsert.Parameters.Add("@parentLoadingId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.ParentLoadingId);
            cmdInsert.Parameters.Add("@callFlag", System.Data.SqlDbType.Int).Value = tblLoadingTO.CallFlag;
            cmdInsert.Parameters.Add("@flagUpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.FlagUpdatedOn);
            cmdInsert.Parameters.Add("@isAllowNxtLoading", System.Data.SqlDbType.Int).Value = tblLoadingTO.IsAllowNxtLoading;
            cmdInsert.Parameters.Add("@loadingType", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.LoadingType);
            cmdInsert.Parameters.Add("@currencyId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.CurrencyId);
            cmdInsert.Parameters.Add("@currencyRate", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.CurrencyRate);
            cmdInsert.Parameters.Add("@callFlagBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.CallFlagBy);
            cmdInsert.Parameters.Add("@ModeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.ModeId);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblLoadingTO.IdLoading = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }


        private static int InsertFinalLoadingSlip(TblLoadingSlipTO tblLoadingSlipTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionFinalLoadingSlipCommand(tblLoadingSlipTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        private static int ExecuteInsertionFinalLoadingSlipCommand(TblLoadingSlipTO tblLoadingSlipTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [finalLoadingSlip]( " +
                            "  [dealerOrgId]" +
                            " ,[isJointDelivery]" +
                            " ,[noOfDeliveries]" +
                            " ,[statusId]" +
                            " ,[createdBy]" +
                            " ,[statusDate]" +
                            " ,[loadingDatetime]" +
                            " ,[createdOn]" +
                            " ,[cdStructure]" +
                            " ,[statusReason]" +
                            " ,[vehicleNo]" +
                            " ,[loadingId]" +
                            " ,[statusReasonId]" +
                            " ,[loadingSlipNo]" +
                            " ,[isConfirmed]" +
                            " ,[comment]" +
                            " ,[contactNo]" +
                            " ,[driverName]" +
                            " ,[cdStructureId]" +
                            " ,[orcAmt]" +              //Priyanka [07-05-2018]
                            " ,[orcMeasure]" +
                            " )" +
                " VALUES (" +
                            "  @DealerOrgId " +
                            " ,@IsJointDelivery " +
                            " ,@NoOfDeliveries " +
                            " ,@StatusId " +
                            " ,@CreatedBy " +
                            " ,@StatusDate " +
                            " ,@LoadingDatetime " +
                            " ,@CreatedOn " +
                            " ,@CdStructure " +
                            " ,@StatusReason " +
                            " ,@VehicleNo " +
                            " ,@LoadingId " +
                            " ,@statusReasonId " +
                            " ,@loadingSlipNo " +
                            " ,@isConfirmed " +
                            " ,@comment " +
                            " ,@contactNo " +
                            " ,@driverName " +
                            " ,@cdStructureId " +
                            " ,@orcAmt" +               //Priyanka [07-05-2018]
                            " ,@orcMeasure" +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";

            //cmdInsert.Parameters.Add("@IdLoadingSlip", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.IdLoadingSlip;
            cmdInsert.Parameters.Add("@DealerOrgId", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.DealerOrgId;
            cmdInsert.Parameters.Add("@IsJointDelivery", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.IsJointDelivery;
            cmdInsert.Parameters.Add("@NoOfDeliveries", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.NoOfDeliveries;
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.StatusId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.CreatedBy;
            cmdInsert.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblLoadingSlipTO.StatusDate;
            cmdInsert.Parameters.Add("@LoadingDatetime", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.LoadingDatetime);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingSlipTO.CreatedOn;
            cmdInsert.Parameters.Add("@CdStructure", System.Data.SqlDbType.NVarChar).Value = tblLoadingSlipTO.CdStructure;
            cmdInsert.Parameters.Add("@StatusReason", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.StatusReason);
            cmdInsert.Parameters.Add("@VehicleNo", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.VehicleNo);
            cmdInsert.Parameters.Add("@LoadingId", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.LoadingId;
            cmdInsert.Parameters.Add("@statusReasonId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.StatusReasonId);
            cmdInsert.Parameters.Add("@loadingSlipNo", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.LoadingSlipNo);
            cmdInsert.Parameters.Add("@isConfirmed", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.IsConfirmed;
            cmdInsert.Parameters.Add("@comment", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.Comment);
            cmdInsert.Parameters.Add("@contactNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.ContactNo);
            cmdInsert.Parameters.Add("@driverName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.DriverName);
            cmdInsert.Parameters.Add("@cdStructureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.CdStructureId);

            cmdInsert.Parameters.Add("@orcAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.OrcAmt);          //Priyanka[07-05-18]
            cmdInsert.Parameters.Add("@orcMeasure", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.OrcMeasure);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblLoadingSlipTO.IdLoadingSlip = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }

        private static int InsertFinalLoadingSlipAddress(TblLoadingSlipAddressTO tblLoadingSlipAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return InsertFinalLoadingSlipAddress(tblLoadingSlipAddressTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        private static int InsertFinalLoadingSlipAddress(TblLoadingSlipAddressTO tblLoadingSlipAddressTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [finalLoadingSlipAddress]( " +
                            "  [bookDelAddrId]" +
                            " ,[loadingSlipId]" +
                            " ,[loadingLayerId]" +
                            " ,[address]" +
                            " ,[village]" +
                            " ,[taluka]" +
                            " ,[district]" +
                            " ,[state]" +
                            " ,[country]" +
                            " ,[pincode]" +
                            " ,[comment]" +
                            " ,[billingName]" +
                            " ,[gstNo]" +
                            " ,[contactNo]" +
                            " ,[txnAddrTypeId]" +
                            " ,[stateId]" +
                            " ,[panNo]" +
                            " ,[aadharNo]" +
                            " ,[addrSourceTypeId]" +
                            " )" +
                " VALUES (" +
                            "  @BookDelAddrId " +
                            " ,@LoadingSlipId " +
                            " ,@LoadingLayerId " +
                            " ,@Address " +
                            " ,@Village " +
                            " ,@Taluka " +
                            " ,@District " +
                            " ,@State " +
                            " ,@Country " +
                            " ,@Pincode " +
                            " ,@Comment " +
                            " ,@billingName " +
                            " ,@gstNo " +
                            " ,@contactNo " +
                            " ,@txnAddrTypeId " +
                            " ,@stateId " +
                            " ,@panNo " +
                            " ,@aadharNo " +
                            " ,@addrSourceTypeId " +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdLoadSlipAddr", System.Data.SqlDbType.Int).Value = tblLoadingSlipAddressTO.IdLoadSlipAddr;
            cmdInsert.Parameters.Add("@BookDelAddrId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipAddressTO.BookDelAddrId);
            cmdInsert.Parameters.Add("@LoadingSlipId", System.Data.SqlDbType.Int).Value = tblLoadingSlipAddressTO.LoadingSlipId;
            cmdInsert.Parameters.Add("@LoadingLayerId", System.Data.SqlDbType.Int).Value = tblLoadingSlipAddressTO.LoadingLayerId;
            cmdInsert.Parameters.Add("@Address", System.Data.SqlDbType.VarChar, 256).Value = tblLoadingSlipAddressTO.Address;
            cmdInsert.Parameters.Add("@Village", System.Data.SqlDbType.VarChar, 128).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipAddressTO.VillageName);
            cmdInsert.Parameters.Add("@Taluka", System.Data.SqlDbType.VarChar, 128).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipAddressTO.TalukaName);
            cmdInsert.Parameters.Add("@District", System.Data.SqlDbType.VarChar, 128).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipAddressTO.DistrictName);
            cmdInsert.Parameters.Add("@State", System.Data.SqlDbType.VarChar, 128).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipAddressTO.State);
            cmdInsert.Parameters.Add("@Country", System.Data.SqlDbType.VarChar, 128).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipAddressTO.Country);
            cmdInsert.Parameters.Add("@Pincode", System.Data.SqlDbType.VarChar, 24).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipAddressTO.Pincode);
            cmdInsert.Parameters.Add("@Comment", System.Data.SqlDbType.VarChar, 256).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipAddressTO.Comment);
            cmdInsert.Parameters.Add("@billingName", System.Data.SqlDbType.NVarChar, 256).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipAddressTO.BillingName);
            cmdInsert.Parameters.Add("@gstNo", System.Data.SqlDbType.NVarChar, 25).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipAddressTO.GstNo);
            cmdInsert.Parameters.Add("@contactNo", System.Data.SqlDbType.NVarChar, 50).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipAddressTO.ContactNo);
            cmdInsert.Parameters.Add("@txnAddrTypeId", System.Data.SqlDbType.Int).Value = tblLoadingSlipAddressTO.TxnAddrTypeId;
            cmdInsert.Parameters.Add("@stateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipAddressTO.StateId);
            cmdInsert.Parameters.Add("@panNo", System.Data.SqlDbType.NVarChar, 25).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipAddressTO.PanNo);
            cmdInsert.Parameters.Add("@aadharNo", System.Data.SqlDbType.NVarChar, 25).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipAddressTO.AadharNo);
            cmdInsert.Parameters.Add("@addrSourceTypeId", System.Data.SqlDbType.Int).Value = tblLoadingSlipAddressTO.AddrSourceTypeId;

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblLoadingSlipAddressTO.IdLoadSlipAddr = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }


        private static int InsertFinalLoadingSlipDtl(TblLoadingSlipDtlTO tblLoadingSlipDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionFinalLoadingSlipDtlCommand(tblLoadingSlipDtlTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        private static int ExecuteInsertionFinalLoadingSlipDtlCommand(TblLoadingSlipDtlTO tblLoadingSlipDtlTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [finalLoadingSlipDtl]( " +
                            "  [loadingSlipId]" +
                            " ,[bookingId]" +
                            " ,[bookingExtId]" +
                            " ,[loadingQty]" +
                            " )" +
                " VALUES (" +
                            "  @LoadingSlipId " +
                            " ,@BookingId " +
                            " ,@BookingExtId " +
                            " ,@LoadingQty " +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdLoadSlipDtl", System.Data.SqlDbType.Int).Value = tblLoadingSlipDtlTO.IdLoadSlipDtl;
            cmdInsert.Parameters.Add("@LoadingSlipId", System.Data.SqlDbType.Int).Value = tblLoadingSlipDtlTO.LoadingSlipId;
            cmdInsert.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = tblLoadingSlipDtlTO.BookingId;
            cmdInsert.Parameters.Add("@BookingExtId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipDtlTO.BookingExtId);
            cmdInsert.Parameters.Add("@LoadingQty", System.Data.SqlDbType.NVarChar).Value = tblLoadingSlipDtlTO.LoadingQty;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblLoadingSlipDtlTO.IdLoadSlipDtl = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }


        private static int InsertFinalLoadingSlipExt(TblLoadingSlipExtTO tblLoadingSlipExtTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionFinalLoadingSlipExtCommand(tblLoadingSlipExtTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        private static int ExecuteInsertionFinalLoadingSlipExtCommand(TblLoadingSlipExtTO tblLoadingSlipExtTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [finalLoadingSlipExt]( " +
                            "  [bookingId]" +
                            " ,[loadingSlipId]" +
                            " ,[loadingLayerid]" +
                            " ,[materialId]" +
                            " ,[bookingExtId]" +
                            " ,[loadingQty]" +
                            " ,[prodCatId]" +
                            " ,[prodSpecId]" +
                            " ,[quotaBforeLoading]" +
                            " ,[quotaAfterLoading]" +
                            " ,[loadingQuotaId]" +
                            " ,[bundles]" +
                            " ,[parityDtlId]" +
                            " ,[ratePerMT]" +
                            " ,[rateCalcDesc]" +
                            " ,[loadedWeight]" +
                            " ,[loadedBundles]" +
                            " ,[calcTareWeight]" +
                            " ,[weightMeasureId]" +
                            " ,[updatedBy]" +
                            " ,[updatedOn]" +
                            " ,[cdStructureId]" +
                            " ,[cdStructure]" +
                            ", [isAllowWeighingMachine] " +
                            " ,[prodItemDesc]" +
                            " ,[prodItemId]" +
                            " ,[taxableRateMT]" +
                            " ,[freExpOtherAmt]" +
                            " ,[cdApplicableAmt]" +
                            " ,[weighingSequenceNumber]" +
                            " )" +
                " VALUES (" +
                            "  @BookingId " +
                            " ,@LoadingSlipId " +
                            " ,@LoadingLayerid " +
                            " ,@MaterialId " +
                            " ,@BookingExtId " +
                            " ,@LoadingQty " +
                            " ,@prodCatId " +
                            " ,@prodSpecId " +
                            " ,@quotaBforeLoading " +
                            " ,@quotaAfterLoading " +
                            " ,@loadingQuotaId " +
                            " ,@bundles " +
                            " ,@parityDtlId " +
                            " ,@ratePerMT " +
                            " ,@rateCalcDesc " +
                            " ,@loadedWeight " +
                            " ,@loadedBundles " +
                            " ,@calcTareWeight " +
                            " ,@weightMeasureId " +
                            " ,@updatedBy " +
                            " ,@updatedOn " +
                            " ,@cdStructureId " +
                            " ,@cdStructure " +
                            " ,@isAllowWeighingMachine" +
                            " ,@prodItemDesc " +
                            " ,@prodItemId " +
                            " ,@taxableRateMT " +
                            " ,@freExpOtherAmt " +
                            " ,@cdApplicableAmt " +
                            " ,@weighingSequenceNumber " +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            // cmdInsert.Parameters.Add("@IdLoadingSlipExt", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtTO.IdLoadingSlipExt;
            cmdInsert.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.BookingId);
            cmdInsert.Parameters.Add("@LoadingSlipId", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtTO.LoadingSlipId;
            cmdInsert.Parameters.Add("@LoadingLayerid", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtTO.LoadingLayerid;
            cmdInsert.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.MaterialId);
            cmdInsert.Parameters.Add("@BookingExtId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.BookingExtId);
            cmdInsert.Parameters.Add("@LoadingQty", System.Data.SqlDbType.NVarChar).Value = tblLoadingSlipExtTO.LoadingQty;
            cmdInsert.Parameters.Add("@prodCatId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.ProdCatId);
            cmdInsert.Parameters.Add("@prodSpecId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.ProdSpecId);
            cmdInsert.Parameters.Add("@quotaBforeLoading", System.Data.SqlDbType.NVarChar).Value = tblLoadingSlipExtTO.QuotaBforeLoading;
            cmdInsert.Parameters.Add("@quotaAfterLoading", System.Data.SqlDbType.NVarChar).Value = tblLoadingSlipExtTO.QuotaAfterLoading;
            cmdInsert.Parameters.Add("@loadingQuotaId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.LoadingQuotaId);
            cmdInsert.Parameters.Add("@bundles", System.Data.SqlDbType.NVarChar).Value = tblLoadingSlipExtTO.Bundles;
            cmdInsert.Parameters.Add("@parityDtlId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.ParityDtlId);
            cmdInsert.Parameters.Add("@ratePerMT", System.Data.SqlDbType.Decimal).Value = tblLoadingSlipExtTO.RatePerMT;
            cmdInsert.Parameters.Add("@rateCalcDesc", System.Data.SqlDbType.NVarChar, 256).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.RateCalcDesc);
            cmdInsert.Parameters.Add("@loadedWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.LoadedWeight);
            cmdInsert.Parameters.Add("@loadedBundles", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.LoadedBundles);
            cmdInsert.Parameters.Add("@calcTareWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.CalcTareWeight);
            cmdInsert.Parameters.Add("@weightMeasureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.WeightMeasureId);
            cmdInsert.Parameters.Add("@updatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.UpdatedBy);
            cmdInsert.Parameters.Add("@updatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.UpdatedOn);
            cmdInsert.Parameters.Add("@cdStructureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.CdStructureId);
            cmdInsert.Parameters.Add("@cdStructure", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.CdStructure);
            cmdInsert.Parameters.Add("@isAllowWeighingMachine", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.IsAllowWeighingMachine);
            cmdInsert.Parameters.Add("@prodItemDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.ProdItemDesc);
            cmdInsert.Parameters.Add("@prodItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.ProdItemId);
            cmdInsert.Parameters.Add("@taxableRateMT", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.TaxableRateMT);
            cmdInsert.Parameters.Add("@freExpOtherAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.FreExpOtherAmt);
            cmdInsert.Parameters.Add("@cdApplicableAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.CdApplicableAmt);
            cmdInsert.Parameters.Add("@weighingSequenceNumber", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.WeighingSequenceNumber);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblLoadingSlipExtTO.IdLoadingSlipExt = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }



        private static int InsertFinalLoadingSlipExtHistory(TblLoadingSlipExtHistoryTO tblLoadingSlipExtHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionFinalLoadingSlipExtHistoryCommand(tblLoadingSlipExtHistoryTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        private static int ExecuteInsertionFinalLoadingSlipExtHistoryCommand(TblLoadingSlipExtHistoryTO tblLoadingSlipExtHistoryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [finalLoadingSlipExtHistory]( " +
                                "  [loadingSlipExtId]" +
                                " ,[lastConfirmationStatus]" +
                                " ,[currentConfirmationStatus]" +
                                " ,[parityDtlId]" +
                                " ,[createdBy]" +
                                " ,[createdOn]" +
                                " ,[lastRatePerMT]" +
                                " ,[currentRatePerMT]" +
                                " ,[lastRateCalcDesc]" +
                                " ,[currentRateCalcDesc]" +
                                " ,[lastCdAplAmt]" +
    " ,[currentCdAplAmt]" +
                                " )" +
                    " VALUES (" +
                                "  @LoadingSlipExtId " +
                                " ,@LastConfirmationStatus " +
                                " ,@CurrentConfirmationStatus " +
                                " ,@ParityDtlId " +
                                " ,@CreatedBy " +
                                " ,@CreatedOn " +
                                " ,@LastRatePerMT " +
                                " ,@CurrentRatePerMT " +
                                " ,@LastRateCalcDesc " +
                                " ,@CurrentRateCalcDesc " +
                                 " ,@lastCdAplAmt " +
    " ,@currentCdAplAmt " +
                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdConfirmHistory", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtHistoryTO.IdConfirmHistory;
            cmdInsert.Parameters.Add("@LoadingSlipExtId", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtHistoryTO.LoadingSlipExtId;
            cmdInsert.Parameters.Add("@LastConfirmationStatus", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtHistoryTO.LastConfirmationStatus;
            cmdInsert.Parameters.Add("@CurrentConfirmationStatus", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtHistoryTO.CurrentConfirmationStatus;
            cmdInsert.Parameters.Add("@ParityDtlId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtHistoryTO.ParityDtlId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtHistoryTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingSlipExtHistoryTO.CreatedOn;
            cmdInsert.Parameters.Add("@LastRatePerMT", System.Data.SqlDbType.NVarChar).Value = tblLoadingSlipExtHistoryTO.LastRatePerMT;
            cmdInsert.Parameters.Add("@CurrentRatePerMT", System.Data.SqlDbType.NVarChar).Value = tblLoadingSlipExtHistoryTO.CurrentRatePerMT;
            cmdInsert.Parameters.Add("@LastRateCalcDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtHistoryTO.LastRateCalcDesc);
            cmdInsert.Parameters.Add("@CurrentRateCalcDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtHistoryTO.CurrentRateCalcDesc);
            cmdInsert.Parameters.Add("@lastCdAplAmt", System.Data.SqlDbType.Decimal).Value = tblLoadingSlipExtHistoryTO.LastCdAplAmt;
            cmdInsert.Parameters.Add("@currentCdAplAmt", System.Data.SqlDbType.Decimal).Value = tblLoadingSlipExtHistoryTO.CurrentCdAplAmt;

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblLoadingSlipExtHistoryTO.IdConfirmHistory = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }


        private static int InsertFinalWeighingMeasures(TblWeighingMeasuresTO tblWeighingMeasuresTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionFinalWeighingMeasuresCommand(tblWeighingMeasuresTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        private static int ExecuteInsertionFinalWeighingMeasuresCommand(TblWeighingMeasuresTO tblWeighingMeasuresTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [finalWeighingMeasures]( " +
                               " [weighingMachineId]" +
                                " ,[loadingId]" +
                                " ,[weightMeasurTypeId]" +
                                " ,[isConfirmed]" +
                                " ,[createdBy]" +
                                " ,[updatedBy]" +
                                " ,[createdOn]" +
                                " ,[updatedOn]" +
                                " ,[weightMT]" +
                                " ,[vehicleNo]" +
                                " ,[machineCalibrationId]" +
                                " ,[unLoadingId]" +
                                " )" +
                    " VALUES (" +
                               " @WeighingMachineId " +
                                " ,@LoadingId " +
                                " ,@WeightMeasurTypeId " +
                                " ,@IsConfirmed " +
                                " ,@CreatedBy " +
                                " ,@UpdatedBy " +
                                " ,@CreatedOn " +
                                " ,@UpdatedOn " +
                                " ,@WeightMT " +
                                " ,@VehicleNo " +
                                " ,@machineCalibrationId " +
                                " ,@unLoadingId " +
                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdWeightMeasure", System.Data.SqlDbType.Int).Value = tblWeighingMeasuresTO.IdWeightMeasure;
            cmdInsert.Parameters.Add("@WeighingMachineId", System.Data.SqlDbType.Int).Value = tblWeighingMeasuresTO.WeighingMachineId;
            cmdInsert.Parameters.Add("@LoadingId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblWeighingMeasuresTO.LoadingId);
            cmdInsert.Parameters.Add("@WeightMeasurTypeId", System.Data.SqlDbType.Int).Value = tblWeighingMeasuresTO.WeightMeasurTypeId;
            cmdInsert.Parameters.Add("@IsConfirmed", System.Data.SqlDbType.Int).Value = tblWeighingMeasuresTO.IsConfirmed;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblWeighingMeasuresTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblWeighingMeasuresTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblWeighingMeasuresTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblWeighingMeasuresTO.UpdatedOn);
            cmdInsert.Parameters.Add("@WeightMT", System.Data.SqlDbType.NVarChar).Value = tblWeighingMeasuresTO.WeightMT;
            cmdInsert.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = tblWeighingMeasuresTO.VehicleNo;
            cmdInsert.Parameters.Add("@machineCalibrationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblWeighingMeasuresTO.MachineCalibrationId);
            cmdInsert.Parameters.Add("@unLoadingId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblWeighingMeasuresTO.UnLoadingId);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblWeighingMeasuresTO.IdWeightMeasure = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }

            return 0;
        }


        private static int InsertFinalLoadingStatusHistory(TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionFinalLoadingStatusHistoryCommand(tblLoadingStatusHistoryTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        private static int ExecuteInsertionFinalLoadingStatusHistoryCommand(TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [finalLoadingStatusHistory]( " +
                                "  [loadingId]" +
                                " ,[statusId]" +
                                " ,[createdBy]" +
                                " ,[statusDate]" +
                                " ,[createdOn]" +
                                " ,[statusRemark]" +
                                " ,[statusReasonId]" +
                                " )" +
                    " VALUES (" +
                                "  @LoadingId " +
                                " ,@StatusId " +
                                " ,@CreatedBy " +
                                " ,@StatusDate " +
                                " ,@CreatedOn " +
                                " ,@StatusRemark " +
                                " ,@statusReasonId " +
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdLoadingHistory", System.Data.SqlDbType.Int).Value = tblLoadingStatusHistoryTO.IdLoadingHistory;
            cmdInsert.Parameters.Add("@LoadingId", System.Data.SqlDbType.Int).Value = tblLoadingStatusHistoryTO.LoadingId;
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblLoadingStatusHistoryTO.StatusId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLoadingStatusHistoryTO.CreatedBy;
            cmdInsert.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblLoadingStatusHistoryTO.StatusDate;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingStatusHistoryTO.CreatedOn;
            cmdInsert.Parameters.Add("@StatusRemark", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingStatusHistoryTO.StatusRemark);
            cmdInsert.Parameters.Add("@statusReasonId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingStatusHistoryTO.StatusReasonId);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblLoadingStatusHistoryTO.IdLoadingHistory = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }


        private static int InsertFinalInvoice(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionFinalInvoiceCommand(tblInvoiceTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        private static int ExecuteInsertionFinalInvoiceCommand(TblInvoiceTO tblInvoiceTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [finalInvoice]( " +
                                "  [invoiceTypeId]" +
                                " ,[transportOrgId]" +
                                " ,[transportModeId]" +
                                " ,[currencyId]" +
                                " ,[loadingSlipId]" +
                                " ,[distributorOrgId]" +
                                " ,[dealerOrgId]" +
                                " ,[finYearId]" +
                                " ,[statusId]" +
                                " ,[createdBy]" +
                                " ,[updatedBy]" +
                                " ,[invoiceDate]" +
                                " ,[lrDate]" +
                                " ,[statusDate]" +
                                " ,[createdOn]" +
                                " ,[updatedOn]" +
                                " ,[currencyRate]" +
                                " ,[basicAmt]" +
                                " ,[discountAmt]" +
                                " ,[taxableAmt]" +
                                " ,[cgstAmt]" +
                                " ,[sgstAmt]" +
                                " ,[igstAmt]" +
                                " ,[freightPct]" +
                                " ,[freightAmt]" +
                                " ,[roundOffAmt]" +
                                " ,[grandTotal]" +
                                " ,[invoiceNo]" +
                                " ,[electronicRefNo]" +
                                " ,[vehicleNo]" +
                                " ,[lrNumber]" +
                                " ,[roadPermitNo]" +
                                " ,[transportorForm]" +
                                " ,[airwayBillNo]" +
                                " ,[narration]" +
                                " ,[bankDetails]" +
                                " ,[invoiceModeId]" +
                                " ,[deliveryLocation]" +
                                  " ,[tareWeight]" +
                                " ,[netWeight]" +
                                " ,[grossWeight]" +
                                 " ,[changeIn]" +
                                " ,[expenseAmt]" +
                                " ,[otherAmt]" +
                                " ,[isConfirmed]" +
                                " ,[rcmFlag]" +
                                " ,[remark]" +
                                " ,[invFromOrgId]" +
                                " ,[deliveredOn]" +
                                " ,[IrnNo] " + //Dhananjay added [23-12-2020]
                                " ,[isEInvGenerated] " + //Dhananjay added [23-12-2020]
                                " ,[isEwayBillGenerated] " + //Dhananjay added [23-12-2020]
                                " ,[distanceInKM] " + //Dhananjay added [23-12-2020]
                                " ,[tdsAmt] " + //Saket added [23-07-2021]
                                " )" +
                                " VALUES (" +
                                "  @InvoiceTypeId " +
                                " ,@TransportOrgId " +
                                " ,@TransportModeId " +
                                " ,@CurrencyId " +
                                " ,@LoadingSlipId " +
                                " ,@DistributorOrgId " +
                                " ,@DealerOrgId " +
                                " ,@FinYearId " +
                                " ,@StatusId " +
                                " ,@CreatedBy " +
                                " ,@UpdatedBy " +
                                " ,@InvoiceDate " +
                                " ,@LrDate " +
                                " ,@StatusDate " +
                                " ,@CreatedOn " +
                                " ,@UpdatedOn " +
                                " ,@CurrencyRate " +
                                " ,@BasicAmt " +
                                " ,@DiscountAmt " +
                                " ,@TaxableAmt " +
                                " ,@CgstAmt " +
                                " ,@SgstAmt " +
                                " ,@IgstAmt " +
                                " ,@FreightPct " +
                                " ,@FreightAmt " +
                                " ,@RoundOffAmt " +
                                " ,@GrandTotal " +
                                " ,@InvoiceNo " +
                                " ,@ElectronicRefNo " +
                                " ,@VehicleNo " +
                                " ,@LrNumber " +
                                " ,@RoadPermitNo " +
                                " ,@TransportorForm " +
                                " ,@AirwayBillNo " +
                                " ,@Narration " +
                                " ,@BankDetails " +
                                " ,@invoiceModeId " +
                                " ,@deliveryLocation " +
                                 " ,@tareWeight " +
                                " ,@netWeight " +
                                " ,@grossWeight " +
                                 " ,@changeIn " +
                                " ,@expenseAmt " +
                                " ,@otherAmt " +
                                " ,@isConfirmed " +
                                " ,@rcmFlag " +
                                " ,@remark " +
                                " ,@invFromOrgId " +
                                " ,@deliveredOn " + //Vijaymala added [30-03-2018]
                                " ,@IrnNo " + //Dhananjay added [23-12-2020]
                                " ,@isEInvGenerated " + //Dhananjay added [23-12-2020]
                                " ,@isEwayBillGenerated " + //Dhananjay added [23-12-2020]
                                " ,@distanceInKM " + //Dhananjay added [23-12-2020]
                                " ,@TdsAmt " + 
                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdInvoice", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IdInvoice;
            cmdInsert.Parameters.Add("@InvoiceTypeId", System.Data.SqlDbType.Int).Value = tblInvoiceTO.InvoiceTypeId;
            cmdInsert.Parameters.Add("@TransportOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TransportOrgId);
            cmdInsert.Parameters.Add("@TransportModeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TransportModeId);
            cmdInsert.Parameters.Add("@CurrencyId", System.Data.SqlDbType.Int).Value = tblInvoiceTO.CurrencyId;
            cmdInsert.Parameters.Add("@LoadingSlipId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.LoadingSlipId);
            cmdInsert.Parameters.Add("@DistributorOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DistributorOrgId);
            cmdInsert.Parameters.Add("@DealerOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DealerOrgId);
            cmdInsert.Parameters.Add("@FinYearId", System.Data.SqlDbType.Int).Value = tblInvoiceTO.FinYearId;
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblInvoiceTO.StatusId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblInvoiceTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.UpdatedBy);
            cmdInsert.Parameters.Add("@InvoiceDate", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.InvoiceDate;
            cmdInsert.Parameters.Add("@LrDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.LrDate);
            cmdInsert.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.StatusDate;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.UpdatedOn);
            cmdInsert.Parameters.Add("@CurrencyRate", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.CurrencyRate;
            cmdInsert.Parameters.Add("@BasicAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.BasicAmt;
            cmdInsert.Parameters.Add("@DiscountAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.DiscountAmt;
            cmdInsert.Parameters.Add("@TaxableAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.TaxableAmt;
            cmdInsert.Parameters.Add("@CgstAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.CgstAmt;
            cmdInsert.Parameters.Add("@SgstAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.SgstAmt;
            cmdInsert.Parameters.Add("@IgstAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.IgstAmt;
            cmdInsert.Parameters.Add("@FreightPct", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.FreightPct;
            cmdInsert.Parameters.Add("@FreightAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.FreightAmt;
            cmdInsert.Parameters.Add("@RoundOffAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.RoundOffAmt;
            cmdInsert.Parameters.Add("@GrandTotal", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.GrandTotal;
            cmdInsert.Parameters.Add("@InvoiceNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.InvoiceNo);
            cmdInsert.Parameters.Add("@ElectronicRefNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.ElectronicRefNo);
            cmdInsert.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.VehicleNo);
            cmdInsert.Parameters.Add("@LrNumber", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.LrNumber);
            cmdInsert.Parameters.Add("@RoadPermitNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.RoadPermitNo);
            cmdInsert.Parameters.Add("@TransportorForm", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TransportorForm);
            cmdInsert.Parameters.Add("@AirwayBillNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.AirwayBillNo);
            cmdInsert.Parameters.Add("@Narration", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.Narration);
            cmdInsert.Parameters.Add("@BankDetails", System.Data.SqlDbType.NChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.BankDetails);
            cmdInsert.Parameters.Add("@invoiceModeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.InvoiceModeId);
            cmdInsert.Parameters.Add("@deliveryLocation", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DeliveryLocation);
            cmdInsert.Parameters.Add("@tareWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TareWeight);
            cmdInsert.Parameters.Add("@netWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.NetWeight);
            cmdInsert.Parameters.Add("@grossWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.GrossWeight);
            cmdInsert.Parameters.Add("@changeIn", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.ChangeIn);
            cmdInsert.Parameters.Add("@expenseAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.ExpenseAmt;
            cmdInsert.Parameters.Add("@otherAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.OtherAmt;
            cmdInsert.Parameters.Add("@isConfirmed", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IsConfirmed;
            cmdInsert.Parameters.Add("@rcmFlag", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.RcmFlag);
            cmdInsert.Parameters.Add("@remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.Remark);
            cmdInsert.Parameters.Add("@invFromOrgId", System.Data.SqlDbType.Int).Value = tblInvoiceTO.InvFromOrgId;
            cmdInsert.Parameters.Add("@deliveredOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DeliveredOn);
            cmdInsert.Parameters.Add("@IrnNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.IrnNo);//Dhananjay added [23-12-2020]
            cmdInsert.Parameters.Add("@isEInvGenerated", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IsEInvGenerated;//Dhananjay added [23-12-2020]
            cmdInsert.Parameters.Add("@isEwayBillGenerated", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IsEWayBillGenerated;//Dhananjay added [23-12-2020]
            cmdInsert.Parameters.Add("@distanceInKM", System.Data.SqlDbType.Decimal).Value = tblInvoiceTO.DistanceInKM;//Dhananjay added [23-12-2020]
            cmdInsert.Parameters.Add("@TdsAmt", System.Data.SqlDbType.Decimal).Value = tblInvoiceTO.TdsAmt;//Dhananjay added [23-12-2020]

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblInvoiceTO.IdInvoice = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }

            return 0;
        }

        //Added Dhananjay [23-12-2020] insert invoice eInvoice API response.
        private static int InsertFinalEInvoiceApiResponse(TblEInvoiceApiResponseTO tblEInvoiceApiResponseTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionFinalEInvoiceApiResponseCommand(tblEInvoiceApiResponseTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        private static int InsertFinalInvoiceAddress(TblInvoiceAddressTO tblInvoiceAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionFinalInvoiceAddressCommand(tblInvoiceAddressTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        //Added Dhananjay [23-12-2020] insert invoice eInvoice API response.
        private static int ExecuteInsertionFinalEInvoiceApiResponseCommand(TblEInvoiceApiResponseTO TblEInvoiceApiResponseTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [FinalEInvoiceApiResponse]( " +
                                "  [apiId]" +
                                " ,[invoiceId]" +
                                " ,[responseStatus]" +
                                " ,[response]" +
                                " ,[createdBy]" +
                                " ,[createdOn]" +
                                " )" +
                    " VALUES (" +
                                "  @ApiId " +
                                " ,@InvoiceId " +
                                " ,@ResponseStatus " +
                                " ,@Response " +
                                " ,@CreatedBy " +
                                " ,@CreatedOn " +
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdResponse", System.Data.SqlDbType.Int).Value = TblEInvoiceApiResponseTO.IdResponse;
            cmdInsert.Parameters.Add("@ApiId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(TblEInvoiceApiResponseTO.ApiId);
            cmdInsert.Parameters.Add("@InvoiceId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(TblEInvoiceApiResponseTO.InvoiceId);
            cmdInsert.Parameters.Add("@ResponseStatus", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(TblEInvoiceApiResponseTO.ResponseStatus);
            cmdInsert.Parameters.Add("@Response", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(TblEInvoiceApiResponseTO.Response);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = TblEInvoiceApiResponseTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = TblEInvoiceApiResponseTO.CreatedOn;

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                TblEInvoiceApiResponseTO.IdResponse = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        private static int ExecuteInsertionFinalInvoiceAddressCommand(TblInvoiceAddressTO tblInvoiceAddressTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [finalInvoiceAddress]( " +
                                "  [invoiceId]" +
                                " ,[txnAddrTypeId]" +
                                " ,[billingOrgId]" +
                                " ,[talukaId]" +
                                " ,[districtId]" +
                                " ,[stateId]" +
                                " ,[countryId]" +
                                " ,[billingName]" +
                                " ,[gstinNo]" +
                                " ,[panNo]" +
                                " ,[aadharNo]" +
                                " ,[contactNo]" +
                                " ,[address]" +
                                " ,[taluka]" +
                                " ,[district]" +
                                " ,[state]" +
                                " ,[pinCode]" +
                                " ,[addrSourceTypeId]" +
                                " ,[villageName]" +
                                " )" +
                    " VALUES (" +
                                "  @InvoiceId " +
                                " ,@TxnAddrTypeId " +
                                " ,@BillingOrgId " +
                                " ,@TalukaId " +
                                " ,@DistrictId " +
                                " ,@StateId " +
                                " ,@CountryId " +
                                " ,@BillingName " +
                                " ,@GstinNo " +
                                " ,@PanNo " +
                                " ,@AadharNo " +
                                " ,@ContactNo " +
                                " ,@Address " +
                                " ,@Taluka " +
                                " ,@District " +
                                " ,@State " +
                                " ,@PinCode " +
                                " ,@addrSourceTypeId " +
                                " ,@villageName" +
                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdInvoiceAddr", System.Data.SqlDbType.Int).Value = tblInvoiceAddressTO.IdInvoiceAddr;
            cmdInsert.Parameters.Add("@InvoiceId", System.Data.SqlDbType.Int).Value = tblInvoiceAddressTO.InvoiceId;
            cmdInsert.Parameters.Add("@TxnAddrTypeId", System.Data.SqlDbType.Int).Value = tblInvoiceAddressTO.TxnAddrTypeId;
            cmdInsert.Parameters.Add("@BillingOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.BillingOrgId);
            cmdInsert.Parameters.Add("@TalukaId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.TalukaId);
            cmdInsert.Parameters.Add("@DistrictId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.DistrictId);
            cmdInsert.Parameters.Add("@StateId", System.Data.SqlDbType.Int).Value = tblInvoiceAddressTO.StateId;
            cmdInsert.Parameters.Add("@CountryId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.CountryId);
            cmdInsert.Parameters.Add("@BillingName", System.Data.SqlDbType.NVarChar).Value = tblInvoiceAddressTO.BillingName;
            cmdInsert.Parameters.Add("@GstinNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.GstinNo);
            cmdInsert.Parameters.Add("@PanNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.PanNo);
            cmdInsert.Parameters.Add("@AadharNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.AadharNo);
            cmdInsert.Parameters.Add("@ContactNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.ContactNo);
            cmdInsert.Parameters.Add("@Address", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.Address);
            cmdInsert.Parameters.Add("@Taluka", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.Taluka);
            cmdInsert.Parameters.Add("@District", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.District);
            cmdInsert.Parameters.Add("@State", System.Data.SqlDbType.NVarChar).Value = tblInvoiceAddressTO.State;
            cmdInsert.Parameters.Add("@PinCode", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.PinCode);
            cmdInsert.Parameters.Add("@addrSourceTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.AddrSourceTypeId);
            cmdInsert.Parameters.Add("@villageName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceAddressTO.VillageName);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblInvoiceAddressTO.IdInvoiceAddr = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }

            return 0;
        }


        private static int InsertFinalInvoiceItemDetails(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionFinalInvoiceItemDetailsCommand(tblInvoiceItemDetailsTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        private static int ExecuteInsertionFinalInvoiceItemDetailsCommand(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [finalInvoiceItemDetails]( " +
                                "  [invoiceId]" +
                                " ,[loadingSlipExtId]" +
                                " ,[prodGstCodeId]" +
                                " ,[bundles]" +
                                " ,[invoiceQty]" +
                                " ,[rate]" +
                                " ,[basicTotal]" +
                                " ,[taxableAmt]" +
                                " ,[grandTotal]" +
                                " ,[prodItemDesc]" +
                                " ,[cdStructure]" +
                                " ,[cdAmt]" +
                                " ,[gstinCodeNo]" +
                                " ,[otherTaxId]" +
                                " ,[taxPct]" +
                                " ,[taxAmt]" +
                                " ,[cdStructureId]" +
                                " )" +
                    " VALUES (" +
                                "  @InvoiceId " +
                                " ,@LoadingSlipExtId " +
                                " ,@ProdGstCodeId " +
                                " ,@Bundles " +
                                " ,@InvoiceQty " +
                                " ,@Rate " +
                                " ,@BasicTotal " +
                                " ,@TaxableAmt " +
                                " ,@GrandTotal " +
                                " ,@ProdItemDesc " +
                                " ,@cdStructure " +
                                " ,@cdAmt " +
                                " ,@gstinCodeNo " +
                                " ,@otherTaxId " +
                                " ,@taxPct " +
                                " ,@taxAmt " +
                                " ,@cdStructureId " +
                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdInvoiceItem", System.Data.SqlDbType.Int).Value = tblInvoiceItemDetailsTO.IdInvoiceItem;
            cmdInsert.Parameters.Add("@InvoiceId", System.Data.SqlDbType.Int).Value = tblInvoiceItemDetailsTO.InvoiceId;
            cmdInsert.Parameters.Add("@LoadingSlipExtId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.LoadingSlipExtId);
            cmdInsert.Parameters.Add("@ProdGstCodeId", System.Data.SqlDbType.Int).Value = tblInvoiceItemDetailsTO.ProdGstCodeId;
            cmdInsert.Parameters.Add("@Bundles", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.Bundles);
            cmdInsert.Parameters.Add("@InvoiceQty", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.InvoiceQty);
            cmdInsert.Parameters.Add("@Rate", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.Rate);
            cmdInsert.Parameters.Add("@BasicTotal", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.BasicTotal);
            cmdInsert.Parameters.Add("@TaxableAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceItemDetailsTO.TaxableAmt;
            cmdInsert.Parameters.Add("@GrandTotal", System.Data.SqlDbType.NVarChar).Value = tblInvoiceItemDetailsTO.GrandTotal;
            cmdInsert.Parameters.Add("@ProdItemDesc", System.Data.SqlDbType.NVarChar).Value = tblInvoiceItemDetailsTO.ProdItemDesc;
            cmdInsert.Parameters.Add("@cdStructure", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.CdStructure);
            cmdInsert.Parameters.Add("@cdAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.CdAmt);
            cmdInsert.Parameters.Add("@gstinCodeNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.GstinCodeNo);
            cmdInsert.Parameters.Add("@otherTaxId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.OtherTaxId);
            cmdInsert.Parameters.Add("@taxPct", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.TaxPct);
            cmdInsert.Parameters.Add("@taxAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.TaxAmt);
            cmdInsert.Parameters.Add("@cdStructureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.CdStructureId);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblInvoiceItemDetailsTO.IdInvoiceItem = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }

            return 0;
        }


        private static int InsertFinalInvoiceItemTaxDtls(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionFinalInvoiceItemTaxDtlsCommand(tblInvoiceItemTaxDtlsTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        private static int ExecuteInsertionFinalInvoiceItemTaxDtlsCommand(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [finalInvoiceItemTaxDtls]( " +
                                "  [invoiceItemId]" +
                                " ,[taxRateId]" +
                                " ,[taxPct]" +
                                " ,[taxRatePct]" +
                                " ,[taxableAmt]" +
                                " ,[taxAmt]" +
                                " )" +
                    " VALUES (" +
                                "  @InvoiceItemId " +
                                " ,@TaxRateId " +
                                " ,@TaxPct " +
                                " ,@TaxRatePct " +
                                " ,@TaxableAmt " +
                                " ,@TaxAmt " +
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdInvItemTaxDtl", System.Data.SqlDbType.Int).Value = tblInvoiceItemTaxDtlsTO.IdInvItemTaxDtl;
            cmdInsert.Parameters.Add("@InvoiceItemId", System.Data.SqlDbType.Int).Value = tblInvoiceItemTaxDtlsTO.InvoiceItemId;
            cmdInsert.Parameters.Add("@TaxRateId", System.Data.SqlDbType.Int).Value = tblInvoiceItemTaxDtlsTO.TaxRateId;
            cmdInsert.Parameters.Add("@TaxPct", System.Data.SqlDbType.NVarChar).Value = tblInvoiceItemTaxDtlsTO.TaxPct;
            cmdInsert.Parameters.Add("@TaxRatePct", System.Data.SqlDbType.NVarChar).Value = tblInvoiceItemTaxDtlsTO.TaxRatePct;
            cmdInsert.Parameters.Add("@TaxableAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceItemTaxDtlsTO.TaxableAmt;
            cmdInsert.Parameters.Add("@TaxAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceItemTaxDtlsTO.TaxAmt;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblInvoiceItemTaxDtlsTO.IdInvItemTaxDtl = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }

            return 0;
        }


        private static int InsertFinalInvoiceHistory(TblInvoiceHistoryTO tblInvoiceHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionFinalInvoiceHistoryCommand(tblInvoiceHistoryTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        private static int ExecuteInsertionFinalInvoiceHistoryCommand(TblInvoiceHistoryTO tblInvoiceHistoryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [finalInvoiceHistory]( " +
                                "  [invoiceId]" +
                                " ,[invoiceItemId]" +
                                " ,[oldCdStructureId]" +
                                " ,[newCdStructureId]" +
                                " ,[statusId]" +
                                " ,[createdBy]" +
                                " ,[statusDate]" +
                                " ,[createdOn]" +
                                " ,[oldUnitRate]" +
                                " ,[newUnitRate]" +
                                " ,[oldQty]" +
                                " ,[newQty]" +
                                " ,[oldBillingAddr]" +
                                " ,[newBillingAddr]" +
                                " ,[oldConsinAddr]" +
                                " ,[newConsinAddr]" +
                                " ,[oldEwayBillNo]" +
                                " ,[newEwayBillNo]" +
                                " ,[statusRemark]" +
                                " )" +
                    " VALUES (" +
                                "  @InvoiceId " +
                                " ,@InvoiceItemId " +
                                " ,@OldCdStructureId " +
                                " ,@NewCdStructureId " +
                                " ,@StatusId " +
                                " ,@CreatedBy " +
                                " ,@StatusDate " +
                                " ,@CreatedOn " +
                                " ,@OldUnitRate " +
                                " ,@NewUnitRate " +
                                " ,@OldQty " +
                                " ,@NewQty " +
                                " ,@OldBillingAddr " +
                                " ,@NewBillingAddr " +
                                " ,@OldConsinAddr " +
                                " ,@NewConsinAddr " +
                                " ,@OldEwayBillNo " +
                                " ,@NewEwayBillNo " +
                                " ,@StatusRemark " +
                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdInvHistory", System.Data.SqlDbType.Int).Value = tblInvoiceHistoryTO.IdInvHistory;
            cmdInsert.Parameters.Add("@InvoiceId", System.Data.SqlDbType.Int).Value = tblInvoiceHistoryTO.InvoiceId;
            cmdInsert.Parameters.Add("@InvoiceItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.InvoiceItemId);
            cmdInsert.Parameters.Add("@OldCdStructureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.OldCdStructureId);
            cmdInsert.Parameters.Add("@NewCdStructureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.NewCdStructureId);
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblInvoiceHistoryTO.StatusId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblInvoiceHistoryTO.CreatedBy;
            cmdInsert.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblInvoiceHistoryTO.StatusDate;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblInvoiceHistoryTO.CreatedOn;
            cmdInsert.Parameters.Add("@OldUnitRate", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.OldUnitRate);
            cmdInsert.Parameters.Add("@NewUnitRate", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.NewUnitRate);
            cmdInsert.Parameters.Add("@OldQty", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.OldQty);
            cmdInsert.Parameters.Add("@NewQty", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.NewQty);
            cmdInsert.Parameters.Add("@OldBillingAddr", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.OldBillingAddr);
            cmdInsert.Parameters.Add("@NewBillingAddr", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.NewBillingAddr);
            cmdInsert.Parameters.Add("@OldConsinAddr", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.OldConsinAddr);
            cmdInsert.Parameters.Add("@NewConsinAddr", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.NewConsinAddr);
            cmdInsert.Parameters.Add("@OldEwayBillNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.OldEwayBillNo);
            cmdInsert.Parameters.Add("@NewEwayBillNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.NewEwayBillNo);
            cmdInsert.Parameters.Add("@StatusRemark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.StatusRemark);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblInvoiceHistoryTO.IdInvHistory = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }

        /// <summary>
        /// [28-05-2018]:Vijaymala added to save data in enquiry invoice document details
        /// </summary>
        /// <param name="tempInvoiceDocumentDetailsTO"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static int InsertFinalInvoiceDocumentDetails(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionFinalInvoiceDocumentDetailsCommand(tempInvoiceDocumentDetailsTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionFinalInvoiceDocumentDetailsCommand(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [finalInvoiceDocumentDetails]( " +
            //"  [idInvoiceDocument]" +
            " [invoiceId]" +
            " ,[documentId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[isActive]" +
            " )" +
" VALUES (" +
            //"  @IdInvoiceDocument " +
            "  @InvoiceId " +
            " ,@DocumentId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            ", @UpdatedOn " +
            " ,@IsActive " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdInvoiceDocument", System.Data.SqlDbType.Int).Value = tempInvoiceDocumentDetailsTO.IdInvoiceDocument;
            cmdInsert.Parameters.Add("@InvoiceId", System.Data.SqlDbType.Int).Value = tempInvoiceDocumentDetailsTO.InvoiceId;
            cmdInsert.Parameters.Add("@DocumentId", System.Data.SqlDbType.Int).Value = tempInvoiceDocumentDetailsTO.DocumentId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tempInvoiceDocumentDetailsTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tempInvoiceDocumentDetailsTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tempInvoiceDocumentDetailsTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tempInvoiceDocumentDetailsTO.UpdatedOn);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tempInvoiceDocumentDetailsTO.IsActive;

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tempInvoiceDocumentDetailsTO.IdInvoiceDocument = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            return 0;
        }



        #endregion

        #region Selection Methods

        private static List<int> SelectYesterdayStockSummaryId(DateTime fromDate, DateTime toDate, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            List<int> result = new List<int>();
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            string sqlQuery = "";

            try
            {
                if (fromDate == DateTime.MinValue)
                {
                    sqlQuery = " SELECT idStockSummary FROM tblStockSummary WHERE stockDate <= @ToDate";
                }
                else
                {
                    sqlQuery = " SELECT idStockSummary FROM tblStockSummary WHERE stockDate BETWEEN @FromDate and @ToDate";
                }

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@FromDate", SqlDbType.Date).Value = fromDate;
                cmdSelect.Parameters.Add("@ToDate", SqlDbType.Date).Value = toDate;

                //result = Convert.ToInt32(cmdSelect.ExecuteScalar());
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                if (sqlReader != null)
                {
                    while (sqlReader.Read())
                    {
                        if (sqlReader["idStockSummary"] != DBNull.Value)
                            result.Add(Convert.ToInt16(sqlReader["idStockSummary"]));
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectYesterdayStockSummaryId");
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();

                cmdSelect.Dispose();
            }
        }

        public static List<TblInvoiceRptTO> SelectTempInvoiceData(SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;

            try
            {
                String sqlQuery = " SELECT  DISTINCT invoice.idInvoice,invoice.invoiceNo,invoice.vehicleNo,invoice.invoiceDate,invoice.createdOn,   " +
                                  " invoiceAddress.billingName as partyName, org.firmName cnfName, booking.bookingRate,itemDetails.idInvoiceItem as invoiceItemId,  " +
                                  " itemDetails.prodItemDesc, itemDetails.bundles,itemDetails.rate, itemDetails.cdStructure,itemDetails.cdAmt,itemDetails.otherTaxId," +
                                  " itemTaxDetails.taxRatePct ,taxRate.taxTypeId , invoice.freightAmt,itemDetails.invoiceQty,itemDetails.basicTotal as taxableAmt  , " +
                                  " itemTaxDetails.taxAmt,itemDetails.grandTotal,loadingSlip.loadingId " +
                                  " FROM tempInvoice invoice INNER JOIN tempInvoiceAddress invoiceAddress" +
                                  " ON invoiceAddress.invoiceId = invoice.idInvoice INNER JOIN tblOrganization org   ON org.idOrganization = invoice.distributorOrgId" +
                                  " INNER JOIN tempInvoiceItemDetails itemDetails  ON itemDetails.invoiceId = invoice.idInvoice LEFT JOIN tempLoadingSlipExt lExt" +
                                  " ON lExt.idLoadingSlipExt = itemDetails.loadingSlipExtId LEFT JOIN tblBookings booking  ON lExt.bookingId = booking.idBooking" +
                                  " LEFT JOIN tempInvoiceItemTaxDtls itemTaxDetails ON itemTaxDetails.invoiceItemId = itemDetails.idInvoiceItem" +
                                  " LEFT JOIN tblTaxRates taxRate  ON taxRate.idTaxRate = itemTaxDetails.taxRateId " +
                                  " INNER join tempLoadingSlip loadingSlip on loadingSlip.idLoadingSlip = invoice.loadingSlipId " +
                                  " AND loadingSlip.isConfirmed = 0 AND invoice.statusId=9 and invoiceAddress.txnAddrTypeId=1 " +
                                  " AND loadingSlip.statusId= " + (int)Constants.TranStatusE.LOADING_DELIVERED + " ORDER BY loadingId ASC ";
                // " AND loadingSlip.loadingId in (" + loadingIds + ") ORDER BY idinvoice ASC ";

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                return TblInvoiceDAO.ConvertDTToListForRPTInvoice(reader);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectTempInvoiceData");
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        #endregion


        #region Deletion Methods 

        // Delete transactional data.
        public static int DeleteTempLoadingData(int loadingId, SqlConnection conn, SqlTransaction tran)
        {

            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            List<TblLoadingSlipTO> loadingSlipTOList = null;

            try
            {
                #region Delete Loading Slip Data
                // Select temp loading slip details.
                loadingSlipTOList = TblLoadingSlipBL.SelectAllLoadingSlipListWithDetails(loadingId, conn, tran);

                foreach (var loadingSlipTO in loadingSlipTOList)
                {

                    result = DeleteTempData(DelTranTablesE.tempInvoiceHistory.ToString(), loadingSlipTO.IdLoadingSlip, conn, tran);
                    if (result < 0)
                    {
                        resultMessage.DefaultBehaviour("Error while Deleting TempInvoiceHistory");
                        return -1;
                    }


                    result = DeleteTempData(DelTranTablesE.tempInvoiceItemTaxDtls.ToString(), loadingSlipTO.IdLoadingSlip, conn, tran);
                    if (result < 0)
                    {
                        resultMessage.DefaultBehaviour("Error while Deleting TempInvoiceItemTaxDtls");
                        return -1;
                    }


                    result = DeleteTempData(DelTranTablesE.tempInvoiceItemDetails.ToString(), loadingSlipTO.IdLoadingSlip, conn, tran);
                    if (result < 0)
                    {
                        resultMessage.DefaultBehaviour("Error while Deleting TempInvoiceItemDetails");
                        return -1;
                    }

                    result = DeleteTempData(DelTranTablesE.tempInvoiceAddress.ToString(), loadingSlipTO.IdLoadingSlip, conn, tran);
                    if (result < 0)
                    {
                        resultMessage.DefaultBehaviour("Error while Deleting TempInvoiceAddress");
                        return -1;
                    }

                    //[23-12-2020]:Dhananjay added to delete tempEInvoiceApiResponse.
                    result = DeleteTempData(DelTranTablesE.tempEInvoiceApiResponse.ToString(), loadingSlipTO.IdLoadingSlip, conn, tran);
                    if (result < 0)
                    {
                        resultMessage.DefaultBehaviour("Error while Deleting TempEInvoiceApiResponse");
                        return -1;
                    }

                    //[28-05-2018]:Vijaymala added to delete tempinvoicedocumentdetails.
                    result = DeleteTempData(DelTranTablesE.tempInvoiceDocumentDetails.ToString(), loadingSlipTO.IdLoadingSlip, conn, tran);
                    if (result < 0)
                    {
                        resultMessage.DefaultBehaviour("Error while Deleting TempInvoiceDocumentDetails");
                        return -1;
                    }


                    result = DeleteTempData(DelTranTablesE.tempInvoice.ToString(), loadingSlipTO.IdLoadingSlip, conn, tran);
                    if (result < 0)
                    {
                        resultMessage.DefaultBehaviour("Error while Deleting TempInvoice");
                        return -1;
                    }
                    result = DeleteTempData(DelTranTablesE.tempLoadingSlipExtHistory.ToString(), loadingSlipTO.IdLoadingSlip, conn, tran);
                    if (result < 0)
                    {
                        resultMessage.DefaultBehaviour("Error while Deleting TempLoadingSlipExtHistory");
                        return -1;
                    }


                    result = DeleteTempData(DelTranTablesE.tempLoadingSlipExt.ToString(), loadingSlipTO.IdLoadingSlip, conn, tran);
                    if (result < 0)
                    {
                        resultMessage.DefaultBehaviour("Error while Deleting TempLoadingSlipExt");
                        return -1;
                    }

                    result = DeleteTempData(DelTranTablesE.tempLoadingSlipDtl.ToString(), loadingSlipTO.IdLoadingSlip, conn, tran);
                    if (result < 0)
                    {
                        resultMessage.DefaultBehaviour("Error while Deleting TempLoadingSlipDtl");
                        return -1;
                    }

                    result = DeleteTempData(DelTranTablesE.tempLoadingSlipAddress.ToString(), loadingSlipTO.IdLoadingSlip, conn, tran);
                    if (result < 0)
                    {
                        resultMessage.DefaultBehaviour("Error while Deleting TempLoadingSlipAddress");
                        return -1;
                    }

                    result = DeleteTempData(DelTranTablesE.tempLoadingSlip.ToString(), loadingSlipTO.IdLoadingSlip, conn, tran);
                    if (result < 0)
                    {
                        resultMessage.DefaultBehaviour("Error while Deleting TempLoadingSlip");
                        return -1;
                    }
                }

                #endregion

                result = DeleteTempData(DelTranTablesE.tempWeighingMeasures.ToString(), loadingId, conn, tran);
                if (result < 0)
                {
                    resultMessage.DefaultBehaviour("Error while Deleting TempWeighingMeasures");
                    return -1;
                }

                result = DeleteTempData(DelTranTablesE.tempLoadingStatusHistory.ToString(), loadingId, conn, tran);
                if (result < 0)
                {
                    resultMessage.DefaultBehaviour("Error while Deleting TempLoadingStatusHistory");
                    return -1;
                }


                result = DeleteTempData(DelTranTablesE.tempLoading.ToString(), loadingId, conn, tran);
                if (result < 0)
                {
                    resultMessage.DefaultBehaviour("Error while Deleting TempLoading");
                    return -1;
                }

                resultMessage.DefaultSuccessBehaviour();
                return 1;
            }

            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "DeleteTempLoadingData");
                return -1;
            }
            finally
            {
                loadingSlipTOList = null;
            }
        }


        // Delete yesterdays stock.
        public static int DeleteYesterdaysStock(SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            try
            {
                //Get Config days

                TblConfigParamsTO configParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsValByName(Constants.CP_DELETE_PREVIOUS_STOCK_AND_PREVIOUS_QUOTADECLARATION_DAYS);
                int configDays = 0;

                DateTime fromDate;
                DateTime toDate;

                if (configParamsTO != null)
                {
                    configDays = Convert.ToInt16(configParamsTO.ConfigParamVal);
                }

                if (configDays > 0)
                    fromDate = Constants.ServerDateTime.AddDays(-configDays);
                else
                    fromDate = DateTime.MinValue;


                toDate = Constants.ServerDateTime.AddDays(-1);
                List<int> stockSummaryId = SelectYesterdayStockSummaryId(fromDate, toDate, conn, tran);

                string stockSummaryIds = string.Join(",", stockSummaryId);

                if (string.IsNullOrEmpty(stockSummaryIds))
                {
                    stockSummaryIds = "0";
                }

                result = DeleteYesterdaysStockConsumption(stockSummaryIds, conn, tran);

                if (result < 0)
                {
                    resultMessage.DefaultBehaviour("Error while DeleteYesterdaysStockConsumption");
                    return -1;
                }



                result = DeleteYesterdaysStockDetails(stockSummaryIds, conn, tran);

                if (result < 0)
                {
                    resultMessage.DefaultBehaviour("Error while DeleteYesterdaysStockDetails");
                    return -1;
                }


                result = DeleteYesterdaysStockSummary(stockSummaryIds, conn, tran);

                if (result < 0)
                {
                    resultMessage.DefaultBehaviour("Error while DeleteYesterdaysStockSummary");
                    return -1;
                }

                resultMessage.DefaultSuccessBehaviour();
                return 1;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "DeleteYesterdaysStock");
                return -1;
            }
        }

        // Delete yesterdays loading quota declaration.
        public static int DeleteYesterdaysLoadingQuotaDeclaration(SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            try
            {
                // Set dates.

                TblConfigParamsTO configParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsValByName(Constants.CP_DELETE_PREVIOUS_STOCK_AND_PREVIOUS_QUOTADECLARATION_DAYS);
                int configDays = 0;

                DateTime fromDate;
                DateTime toDate;

                if (configParamsTO != null)
                {
                    configDays = Convert.ToInt16(configParamsTO.ConfigParamVal);
                }

                if (configDays > 0)
                    fromDate = Constants.ServerDateTime.AddDays(-configDays);
                else
                    fromDate = DateTime.MinValue;

                toDate = Constants.ServerDateTime.AddDays(-1);


                result = DeleteYesterdaysLoadingQuotaConsumption(fromDate, toDate, conn, tran);

                if (result < 0)
                {
                    resultMessage.DefaultBehaviour("Error while DeleteYesterdaysLoadingQuotaConsumption");
                    return -1;
                }


                result = DeleteYesterdaysLoadingQuotaDeclaration(fromDate, toDate, conn, tran);
                if (result < 0)
                {
                    resultMessage.DefaultBehaviour("Error while DeleteYesterdaysQuotaDeclaration");
                    return -1;
                }

                resultMessage.DefaultSuccessBehaviour();
                return 1;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "DeleteYesterdaysQuotaDeclaration");
                return -1;
            }
        }

        #endregion

        #region Deletion Commands

        private static int DeleteTempData(String delTableName, int delId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;

                String sqlQuery = null;

                switch ((DelTranTablesE)Enum.Parse(typeof(DelTranTablesE), delTableName))
                {
                    case DelTranTablesE.tempInvoice:
                        sqlQuery = " DELETE FROM tempInvoice WHERE loadingSlipId = " + delId;
                        break;

                    case DelTranTablesE.tempInvoiceAddress:
                        sqlQuery = " DELETE FROM tempInvoiceAddress WHERE invoiceId IN (SELECT idInvoice FROM tempInvoice WHERE loadingSlipId = " + delId + " ) ";
                        break;

                    //[28-05-2018]:Vijaymala added to delete tempinvoicedocumentdetails.
                    case DelTranTablesE.tempInvoiceDocumentDetails:
                        sqlQuery = " DELETE FROM tempInvoiceDocumentDetails WHERE invoiceId  IN (SELECT idInvoice FROM tempInvoice WHERE loadingSlipId = " + delId + " ) ";
                        break;

                    //[23-12-2020]:Dhananjay added to delete tempEInvoiceApiResponse.
                    case DelTranTablesE.tempEInvoiceApiResponse:
                        sqlQuery = " DELETE FROM tempEInvoiceApiResponse WHERE invoiceId IN (SELECT idInvoice FROM tempInvoice WHERE loadingSlipId = " + delId + " ) ";
                        break;

                    case DelTranTablesE.tempInvoiceHistory:
                        sqlQuery = " DELETE FROM tempInvoiceHistory WHERE invoiceId IN (SELECT idInvoice FROM tempInvoice WHERE loadingSlipId = " + delId + " ) ";
                        break;

                    case DelTranTablesE.tempInvoiceItemDetails:
                        sqlQuery = " DELETE FROM tempInvoiceItemDetails WHERE invoiceId IN (SELECT idInvoice FROM tempInvoice WHERE loadingSlipId = " + delId + " ) ";
                        break;

                    case DelTranTablesE.tempInvoiceItemTaxDtls:
                        sqlQuery = " DELETE FROM tempInvoiceItemTaxDtls WHERE invoiceItemId IN " +
                                  " (SELECT idInvoiceItem FROM tempInvoiceItemDetails invoiceItemDetails " +
                                  " INNER JOIN tempInvoice invoice ON invoiceItemDetails.invoiceId = invoice.idInvoice " +
                                  " WHERE invoice.loadingSlipId = " + delId + " )";
                        break;

                    case DelTranTablesE.tempLoading:
                        sqlQuery = " DELETE FROM tempLoading WHERE idLoading = " + delId;
                        break;

                    case DelTranTablesE.tempLoadingSlip:
                        sqlQuery = " DELETE FROM tempLoadingSlip WHERE idLoadingSlip = " + delId;
                        break;

                    case DelTranTablesE.tempLoadingSlipAddress:
                        sqlQuery = " DELETE FROM tempLoadingSlipAddress WHERE loadingSlipId = " + delId;
                        break;

                    case DelTranTablesE.tempLoadingSlipDtl:
                        sqlQuery = " DELETE FROM tempLoadingSlipDtl WHERE loadingSlipId = " + delId;
                        break;

                    case DelTranTablesE.tempLoadingSlipExt:
                        sqlQuery = " DELETE FROM tempLoadingSlipExt WHERE loadingSlipId = " + delId;
                        break;

                    case DelTranTablesE.tempLoadingSlipExtHistory:
                        sqlQuery = " DELETE FROM tempLoadingSlipExtHistory WHERE loadingSlipExtId IN " +
                                  " (SELECT idLoadingSlipExt FROM tempLoadingSlipExt loadingSlipExt " +
                                  " INNER JOIN tempLoadingSlipExtHistory loadingSlipExtHistory ON " +
                                  " loadingSlipExt.idLoadingSlipExt = loadingSlipExtHistory.loadingSlipExtId" +
                                  " WHERE loadingSlipExt.loadingSlipId = " + delId + ")";
                        break;

                    case DelTranTablesE.tempLoadingStatusHistory:
                        sqlQuery = " DELETE FROM tempLoadingStatusHistory WHERE loadingId = " + delId;
                        break;

                    case DelTranTablesE.tempWeighingMeasures:
                        sqlQuery = " DELETE FROM tempWeighingMeasures WHERE loadingId = " + delId;
                        break;
                }

                if (sqlQuery != null)
                {
                    cmdDelete.CommandText = sqlQuery;
                    return cmdDelete.ExecuteNonQuery();
                }
                else
                    return -1;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "DeleteTempData");
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        private static int DeleteYesterdaysStockDetails(string stockSummaryIds, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;

                String sqlQuery = " DELETE FROM tblStockDetails WHERE stockSummaryId IN( " + stockSummaryIds + " ) ";

                cmdDelete.CommandText = sqlQuery;
                cmdDelete.CommandType = CommandType.Text;
                cmdDelete.CommandTimeout = 1000;
                //cmdDelete.Parameters.Add("@StockSummaryIds", SqlDbType.NVarChar).Value = stockSummaryIds;
                return cmdDelete.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        private static int DeleteYesterdaysStockSummary(string stockSummaryIds, SqlConnection conn, SqlTransaction tran)
        {

            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;

                String sqlQuery = " DELETE FROM tblStockSummary WHERE idStockSummary IN( " + stockSummaryIds + " ) ";

                cmdDelete.CommandText = sqlQuery;
                cmdDelete.CommandType = CommandType.Text;

                //cmdDelete.Parameters.Add("@StockSummaryIds", SqlDbType.NVarChar).Value = stockSummaryIds;
                return cmdDelete.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        private static int DeleteYesterdaysLoadingQuotaDeclaration(DateTime fromDate, DateTime toDate, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;

                String sqlQuery = " DELETE FROM tblLoadingQuotaDeclaration WHERE convert(date,createdOn) BETWEEN @FromDate AND @ToDate ";

                cmdDelete.CommandText = sqlQuery;
                cmdDelete.CommandType = CommandType.Text;
                cmdDelete.CommandTimeout = 0;
                cmdDelete.Parameters.Add("@FromDate", SqlDbType.Date).Value = Constants.GetStartDateTime(fromDate);
                cmdDelete.Parameters.Add("@ToDate", SqlDbType.Date).Value = Constants.GetEndDateTime(toDate);
                return cmdDelete.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        private static int DeleteYesterdaysStockConsumption(string stockSummaryIds, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;

                String sqlQuery = " DELETE FROM tblStockConsumption WHERE stockDtlId IN ( " +
                                  " SELECT idStockDtl FROM tblStockDetails WHERE stockSummaryId IN ( " + stockSummaryIds + ")) ";

                cmdDelete.CommandText = sqlQuery;
                cmdDelete.CommandType = CommandType.Text;

                //cmdDelete.Parameters.Add("@StockSummaryIds", SqlDbType.NVarChar).Value = stockSummaryIds;
                return cmdDelete.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        private static int DeleteYesterdaysLoadingQuotaConsumption(DateTime fromDate, DateTime toDate, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;

                String sqlQuery = " DELETE FROM tblLoadingQuotaConsumption WHERE loadingQuotaId IN( " +
                                  " SELECT idLoadingQuota FROM tblLoadingQuotaDeclaration WHERE convert(date, createdOn) BETWEEN @FromDate AND @ToDate ) ";

                cmdDelete.CommandText = sqlQuery;
                cmdDelete.CommandType = CommandType.Text;
                cmdDelete.CommandTimeout = 0;
                cmdDelete.Parameters.Add("@FromDate", SqlDbType.Date).Value = Constants.GetStartDateTime(fromDate);
                cmdDelete.Parameters.Add("@ToDate", SqlDbType.Date).Value = Constants.GetEndDateTime(toDate);
                return cmdDelete.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        #endregion

        #region Updation Commands

        private static int UpdateFinalLoadingSlipExt(int oldWeightMeasureId, int newWeightMeasureId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                cmdUpdate.CommandTimeout = 0;

                return ExecuteUpdationFinalLoadingSlipExtCommand(oldWeightMeasureId, newWeightMeasureId, cmdUpdate);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateFinalLoadingSlipExt");
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        private static int ExecuteUpdationFinalLoadingSlipExtCommand(int oldWeightMeasureId, int newWeightMeasureId, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [finalLoadingSlipExt] SET " +
            "  [weightMeasureId] = @NewWeightMeasureId" +
            " WHERE [weightMeasureId]= @OldWeightMeasureId ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@OldWeightMeasureId", System.Data.SqlDbType.Int).Value = oldWeightMeasureId;
            cmdUpdate.Parameters.Add("@NewWeightMeasureId", System.Data.SqlDbType.Int).Value = newWeightMeasureId;

            return cmdUpdate.ExecuteNonQuery();
        }

        private static int UpdateTblLoadingQuotaConsumption(int oldloadingSlipExtId, int newloadingSlipExtId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationTblLoadingQuotaConsumptionCommand(oldloadingSlipExtId, newloadingSlipExtId, cmdUpdate);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateTblLoadingQuotaConsumption");
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        private static int ExecuteUpdationTblLoadingQuotaConsumptionCommand(int oldloadingSlipExtId, int newloadingSlipExtId, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblLoadingQuotaConsumption] SET " +
            "  [loadingSlipExtId] = @NewloadingSlipExtId" +
            " WHERE [loadingSlipExtId]= @OldloadingSlipExtId ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@OldloadingSlipExtId", System.Data.SqlDbType.Int).Value = oldloadingSlipExtId;
            cmdUpdate.Parameters.Add("@NewloadingSlipExtId", System.Data.SqlDbType.Int).Value = newloadingSlipExtId;

            return cmdUpdate.ExecuteNonQuery();
        }

        private static int UpdateTblStockConsumption(int oldloadingSlipExtId, int newloadingSlipExtId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationTblStockConsumptionCommand(oldloadingSlipExtId, newloadingSlipExtId, cmdUpdate);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateTblStockConsumption");
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        private static int ExecuteUpdationTblStockConsumptionCommand(int oldloadingSlipExtId, int newloadingSlipExtId, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblStockConsumption] SET " +
            "  [loadingSlipExtId] = @NewloadingSlipExtId" +
            " WHERE [loadingSlipExtId]= @OldloadingSlipExtId ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@OldloadingSlipExtId", System.Data.SqlDbType.Int).Value = oldloadingSlipExtId;
            cmdUpdate.Parameters.Add("@NewloadingSlipExtId", System.Data.SqlDbType.Int).Value = newloadingSlipExtId;

            return cmdUpdate.ExecuteNonQuery();
        }


        private static int UpdateTotalLoadingQty(double totalLoadingQty, int loadingId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                return ExecuteUpdationTotalLoadingQtyCommand(totalLoadingQty, loadingId, cmdUpdate);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "ExecuteUpdationTotalLoadingQtyCommand");
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        private static int ExecuteUpdationTotalLoadingQtyCommand(double totalLoadingQty, int loadingId, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [finalLoading] SET " +
            "  [totalLoadingQty] = @TotalLoadingQty " +
            " WHERE [idLoading] = @LoadingId";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@TotalLoadingQty", System.Data.SqlDbType.Decimal).Value = totalLoadingQty;
            cmdUpdate.Parameters.Add("@LoadingId", System.Data.SqlDbType.Int).Value = loadingId;

            return cmdUpdate.ExecuteNonQuery();
        }


        private static int UpdatePendingBookingQty(double newPendingBookingQty, int bookingId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                return ExecuteUpdationPendingBookingQtyCommand(newPendingBookingQty, bookingId, cmdUpdate);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "ExecuteUpdationPendingBookingQtyCommand");
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        private static int ExecuteUpdationPendingBookingQtyCommand(double newPendingBookingQty, int bookingId, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblBookings] SET " +
            "  [pendingQty] = @PendingQty " +
            " WHERE [idBooking] = @BookingId";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@PendingQty", System.Data.SqlDbType.Decimal).Value = newPendingBookingQty;
            cmdUpdate.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = bookingId;

            return cmdUpdate.ExecuteNonQuery();
        }


        private static int UpdateBookingQty(double newBookingQty, double newPendingBookingQty, int bookingId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                return ExecuteUpdationBookingQtyCommand(newBookingQty, newPendingBookingQty, bookingId, cmdUpdate);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "ExecuteUpdationPendingBookingQtyCommand");
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        private static int ExecuteUpdationBookingQtyCommand(double newBookingQty, double newPendingBookingQty, int bookingId, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblBookings] SET " +
            "  [bookingQty] = @BookingQty " +
            "  [pendingQty] = @PendingQty " +
            " WHERE [idBooking] = @BookingId";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@BookingQty", System.Data.SqlDbType.Decimal).Value = newBookingQty;
            cmdUpdate.Parameters.Add("@PendingQty", System.Data.SqlDbType.Decimal).Value = newPendingBookingQty;
            cmdUpdate.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = bookingId;

            return cmdUpdate.ExecuteNonQuery();
        }

        #endregion


        #region Create Temp Invoice Data Excel

        public static int CreateTempInvoiceExcel(List<TblInvoiceRptTO> tblInvoiceRptTOList, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            List<TblInvoiceRptTO> TblInvoiceRptTOList = new List<TblInvoiceRptTO>();
            List<TblInvoiceRptTO> TblInvoiceRptTOListByInvoiceItemId = new List<TblInvoiceRptTO>();
            ExcelPackage excelPackage = new ExcelPackage();
            int cellRow = 2;
            int invoiceId = 0;

            try
            {
                // Select temp invoice data.
                TblInvoiceRptTOList = tblInvoiceRptTOList;

                TblInvoiceRptTOListByInvoiceItemId = TblInvoiceRptTOList.GroupBy(ele => ele.InvoiceItemId).Select(ele => ele.FirstOrDefault()).ToList();

                foreach (var items in TblInvoiceRptTOListByInvoiceItemId)
                {
                    List<TblInvoiceRptTO> TblInvoiceRptTOListNew = TblInvoiceRptTOList.Where(ele => ele.InvoiceItemId == items.InvoiceItemId).Select(ele => ele).ToList();

                    foreach (var item in TblInvoiceRptTOListNew)
                    {
                        if (item.TaxTypeId == (int)Constants.TaxTypeE.IGST)
                        {
                            items.IgstTaxAmt = item.TaxAmt;
                            items.IgstPct = item.TaxRatePct;
                        }

                        if (item.TaxTypeId == (int)Constants.TaxTypeE.CGST)
                        {
                            items.CgstTaxAmt = item.TaxAmt;
                            items.CgstPct = item.TaxRatePct;
                        }
                        if (item.TaxTypeId == (int)Constants.TaxTypeE.SGST)
                        {
                            items.SgstTaxAmt = item.TaxAmt;
                            items.SgstPct = item.TaxRatePct;
                        }
                    }
                }


                string minDate = TblInvoiceRptTOListByInvoiceItemId.Min(ele => ele.InvoiceDate).ToString("ddMMyy");
                string maxDate = TblInvoiceRptTOListByInvoiceItemId.Max(ele => ele.InvoiceDate).ToString("ddMMyy");

                #region ReadOnly File With SubTotal 

                #region Create Excel File
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add(Constants.ExcelSheetName);

                excelWorksheet.Cells[1, 1].Value = "Vehicle No";
                excelWorksheet.Cells[1, 2].Value = "Invoice No";
                excelWorksheet.Cells[1, 3].Value = "Transaction Date";
                excelWorksheet.Cells[1, 4].Value = "Party Name";
                excelWorksheet.Cells[1, 5].Value = "C & F Name";
                excelWorksheet.Cells[1, 6].Value = "Booking Rate";
                excelWorksheet.Cells[1, 7].Value = "Size";
                excelWorksheet.Cells[1, 8].Value = "Size Bundle";
                excelWorksheet.Cells[1, 9].Value = "Size Rate";
                excelWorksheet.Cells[1, 10].Value = "CD (%)";
                excelWorksheet.Cells[1, 11].Value = "CGST (%)";
                excelWorksheet.Cells[1, 12].Value = "SGST(%)";
                excelWorksheet.Cells[1, 13].Value = "IGST(%)";
                excelWorksheet.Cells[1, 14].Value = "SIZE WEIGHT";
                excelWorksheet.Cells[1, 15].Value = "BASIC SALE VALUE";
                excelWorksheet.Cells[1, 16].Value = "CGST VALUE";
                excelWorksheet.Cells[1, 17].Value = "SGST VALUE";
                excelWorksheet.Cells[1, 18].Value = "IGST VALUE";
                excelWorksheet.Cells[1, 19].Value = "GROSS VALUE";
                excelWorksheet.Cells[1, 20].Value = "CD VALUE";
                excelWorksheet.Cells[1, 21].Value = "PARTY RECEIVABLE";

                excelWorksheet.Cells[1, 1, 1, 21].Style.Font.Bold = true;

                for (int i = 0; i < TblInvoiceRptTOListByInvoiceItemId.Count; i++)
                {
                    if (invoiceId != 0 && invoiceId != TblInvoiceRptTOListByInvoiceItemId[i].IdInvoice)
                    {
                        List<TblInvoiceRptTO> TblInvoiceRptTOListnew = TblInvoiceRptTOListByInvoiceItemId.Where(ele => ele.IdInvoice == invoiceId).Select(ele => ele).ToList();

                        excelWorksheet.Cells[cellRow, 2].Value = "Total";
                        excelWorksheet.Cells[cellRow, 10].Value = TblInvoiceRptTOListnew.Select(ele => ele.CdStructure);
                        excelWorksheet.Cells[cellRow, 11].Value = TblInvoiceRptTOListnew.Select(ele => ele.CgstPct);
                        excelWorksheet.Cells[cellRow, 12].Value = TblInvoiceRptTOListnew.Select(ele => ele.SgstPct);
                        excelWorksheet.Cells[cellRow, 13].Value = TblInvoiceRptTOListnew.Select(ele => ele.IgstPct);
                        excelWorksheet.Cells[cellRow, 14].Value = TblInvoiceRptTOListnew.Sum(ele => ele.InvoiceQty);
                        excelWorksheet.Cells[cellRow, 15].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.TaxableAmt / 1000), 2);

                        excelWorksheet.Cells[cellRow, 16].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.CgstTaxAmt / 1000), 2);
                        excelWorksheet.Cells[cellRow, 17].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.SgstTaxAmt / 1000), 2);
                        excelWorksheet.Cells[cellRow, 18].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.IgstTaxAmt / 1000), 2);
                        excelWorksheet.Cells[cellRow, 19].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.GrandTotal / 1000), 2);
                        excelWorksheet.Cells[cellRow, 20].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.CdAmt / 1000), 2);
                        excelWorksheet.Cells[cellRow, 21].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.GrandTotal / 1000), 2);

                        excelWorksheet.Cells[cellRow, 1, cellRow, 21].Style.Font.Bold = true;
                        cellRow++;

                    }

                    excelWorksheet.Cells[cellRow, 1].Value = TblInvoiceRptTOListByInvoiceItemId[i].VehicleNo;
                    excelWorksheet.Cells[cellRow, 2].Value = TblInvoiceRptTOListByInvoiceItemId[i].InvoiceNoWrtDate;
                    excelWorksheet.Cells[cellRow, 3].Value = TblInvoiceRptTOListByInvoiceItemId[i].InvoiceDateStr;
                    excelWorksheet.Cells[cellRow, 4].Value = TblInvoiceRptTOListByInvoiceItemId[i].PartyName;
                    excelWorksheet.Cells[cellRow, 5].Value = TblInvoiceRptTOListByInvoiceItemId[i].CnfName;

                    excelWorksheet.Cells[cellRow, 6].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].BookingRate / 1000, 2);
                    excelWorksheet.Cells[cellRow, 7].Value = TblInvoiceRptTOListByInvoiceItemId[i].ProdItemDesc;
                    excelWorksheet.Cells[cellRow, 8].Value = TblInvoiceRptTOListByInvoiceItemId[i].Bundles;
                    excelWorksheet.Cells[cellRow, 9].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].Rate / 1000, 2);
                    excelWorksheet.Cells[cellRow, 10].Value = TblInvoiceRptTOListByInvoiceItemId[i].CdStructure;

                    excelWorksheet.Cells[cellRow, 11].Value = TblInvoiceRptTOListByInvoiceItemId[i].CgstPct;
                    excelWorksheet.Cells[cellRow, 12].Value = TblInvoiceRptTOListByInvoiceItemId[i].SgstPct;
                    excelWorksheet.Cells[cellRow, 13].Value = TblInvoiceRptTOListByInvoiceItemId[i].IgstPct;
                    excelWorksheet.Cells[cellRow, 14].Value = TblInvoiceRptTOListByInvoiceItemId[i].InvoiceQty;
                    excelWorksheet.Cells[cellRow, 15].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].TaxableAmt / 1000, 2);

                    excelWorksheet.Cells[cellRow, 16].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].CgstTaxAmt / 1000, 2);
                    excelWorksheet.Cells[cellRow, 17].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].SgstTaxAmt / 1000, 2);
                    excelWorksheet.Cells[cellRow, 18].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].IgstTaxAmt / 1000, 2);
                    excelWorksheet.Cells[cellRow, 19].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].GrandTotal / 1000, 2);
                    excelWorksheet.Cells[cellRow, 20].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].CdAmt / 1000, 2);
                    excelWorksheet.Cells[cellRow, 21].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].GrandTotal / 1000, 2);

                    invoiceId = TblInvoiceRptTOListByInvoiceItemId[i].IdInvoice;
                    cellRow++;

                    // For last record.
                    if (i == (TblInvoiceRptTOListByInvoiceItemId.Count - 1))
                    {
                        List<TblInvoiceRptTO> TblInvoiceRptTOListnew = TblInvoiceRptTOListByInvoiceItemId.Where(ele => ele.IdInvoice == invoiceId).Select(ele => ele).ToList();

                        excelWorksheet.Cells[cellRow, 2].Value = "Total";
                        excelWorksheet.Cells[cellRow, 10].Value = TblInvoiceRptTOListnew.Select(ele => ele.CdStructure);
                        excelWorksheet.Cells[cellRow, 11].Value = TblInvoiceRptTOListnew.Select(ele => ele.CgstPct);
                        excelWorksheet.Cells[cellRow, 12].Value = TblInvoiceRptTOListnew.Select(ele => ele.SgstPct);
                        excelWorksheet.Cells[cellRow, 13].Value = TblInvoiceRptTOListnew.Select(ele => ele.IgstPct);
                        excelWorksheet.Cells[cellRow, 14].Value = TblInvoiceRptTOListnew.Sum(ele => ele.InvoiceQty);
                        excelWorksheet.Cells[cellRow, 15].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.TaxableAmt / 1000), 2);

                        excelWorksheet.Cells[cellRow, 16].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.CgstTaxAmt / 1000), 2);
                        excelWorksheet.Cells[cellRow, 17].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.SgstTaxAmt / 1000), 2);
                        excelWorksheet.Cells[cellRow, 18].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.IgstTaxAmt / 1000), 2);
                        excelWorksheet.Cells[cellRow, 19].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.GrandTotal / 1000), 2);
                        excelWorksheet.Cells[cellRow, 20].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.CdAmt / 1000), 2);
                        excelWorksheet.Cells[cellRow, 21].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.GrandTotal / 1000), 2);

                        excelWorksheet.Cells[cellRow, 1, cellRow, 21].Style.Font.Bold = true;
                        cellRow++;

                        // For final total.
                        excelWorksheet.Cells[cellRow, 1].Value = "Grand Total";
                        excelWorksheet.Cells[cellRow, 14].Value = TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.InvoiceQty);
                        excelWorksheet.Cells[cellRow, 15].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.TaxableAmt / 1000), 2);

                        excelWorksheet.Cells[cellRow, 16].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.CgstTaxAmt / 1000), 2);
                        excelWorksheet.Cells[cellRow, 17].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.SgstTaxAmt / 1000), 2);
                        excelWorksheet.Cells[cellRow, 18].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.IgstTaxAmt / 1000), 2);
                        excelWorksheet.Cells[cellRow, 19].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.GrandTotal / 1000), 2);
                        excelWorksheet.Cells[cellRow, 20].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.CdAmt / 1000), 2);
                        excelWorksheet.Cells[cellRow, 21].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.GrandTotal / 1000), 2);

                        excelWorksheet.Cells[cellRow, 1, cellRow, 21].Style.Font.Bold = true;

                        using (ExcelRange range = excelWorksheet.Cells[1, 1, cellRow, 21])
                        {
                            range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                            range.Style.Font.Name = "Times New Roman";
                            range.Style.Font.Size = 10;
                        }
                    }
                }

                excelWorksheet.Protection.IsProtected = true;
                excelPackage.Workbook.Protection.LockStructure = true;


                #endregion

                #region Upload File to Azure

                // Create azure storage  account connection.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StaticStuff.Constants.AzureConnectionStr);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a target container.
                CloudBlobContainer container = blobClient.GetContainerReference(Constants.AzureSourceContainerName);

                String fileName = Constants.ExcelFileName + Constants.ServerDateTime.ToString("ddMMyyyyHHmmss") + "-" + minDate + "-" + maxDate + "-R" + ".xlsx";
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

                var fileStream = excelPackage.GetAsByteArray();

                Task t1 = blockBlob.UploadFromByteArrayAsync(fileStream, 0, fileStream.Length);

                excelPackage.Dispose();
                #endregion

                #endregion

                #region Editable File Without SubTotal

                #region Create Excel File

                excelPackage = new ExcelPackage();
                cellRow = 2;
                invoiceId = 0;

                ExcelWorksheet excel_Worksheet = excelPackage.Workbook.Worksheets.Add(Constants.ExcelSheetName);

                excel_Worksheet.Cells[1, 1].Value = "Vehicle No";
                excel_Worksheet.Cells[1, 2].Value = "Invoice No";
                excel_Worksheet.Cells[1, 3].Value = "Transaction Date";
                excel_Worksheet.Cells[1, 4].Value = "Party Name";
                excel_Worksheet.Cells[1, 5].Value = "C & F Name";
                excel_Worksheet.Cells[1, 6].Value = "Booking Rate";
                excel_Worksheet.Cells[1, 7].Value = "Size";
                excel_Worksheet.Cells[1, 8].Value = "Size Bundle";
                excel_Worksheet.Cells[1, 9].Value = "Size Rate";
                excel_Worksheet.Cells[1, 10].Value = "CD (%)";
                excel_Worksheet.Cells[1, 11].Value = "CGST (%)";
                excel_Worksheet.Cells[1, 12].Value = "SGST(%)";
                excel_Worksheet.Cells[1, 13].Value = "IGST(%)";
                excel_Worksheet.Cells[1, 14].Value = "SIZE WEIGHT";
                excel_Worksheet.Cells[1, 15].Value = "BASIC SALE VALUE";
                excel_Worksheet.Cells[1, 16].Value = "CGST VALUE";
                excel_Worksheet.Cells[1, 17].Value = "SGST VALUE";
                excel_Worksheet.Cells[1, 18].Value = "IGST VALUE";
                excel_Worksheet.Cells[1, 19].Value = "GROSS VALUE";
                excel_Worksheet.Cells[1, 20].Value = "CD VALUE";
                excel_Worksheet.Cells[1, 21].Value = "PARTY RECEIVABLE";

                excel_Worksheet.Cells[1, 1, 1, 21].Style.Font.Bold = true;

                for (int i = 0; i < TblInvoiceRptTOListByInvoiceItemId.Count; i++)
                {                    

                    excel_Worksheet.Cells[cellRow, 1].Value = TblInvoiceRptTOListByInvoiceItemId[i].VehicleNo;
                    excel_Worksheet.Cells[cellRow, 2].Value = TblInvoiceRptTOListByInvoiceItemId[i].InvoiceNoWrtDate;
                    excel_Worksheet.Cells[cellRow, 3].Value = TblInvoiceRptTOListByInvoiceItemId[i].InvoiceDateStr;
                    excel_Worksheet.Cells[cellRow, 4].Value = TblInvoiceRptTOListByInvoiceItemId[i].PartyName;
                    excel_Worksheet.Cells[cellRow, 5].Value = TblInvoiceRptTOListByInvoiceItemId[i].CnfName;

                    excel_Worksheet.Cells[cellRow, 6].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].BookingRate / 1000, 2);
                    excel_Worksheet.Cells[cellRow, 7].Value = TblInvoiceRptTOListByInvoiceItemId[i].ProdItemDesc;
                    excel_Worksheet.Cells[cellRow, 8].Value = TblInvoiceRptTOListByInvoiceItemId[i].Bundles;
                    excel_Worksheet.Cells[cellRow, 9].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].Rate / 1000, 2);
                    excel_Worksheet.Cells[cellRow, 10].Value = TblInvoiceRptTOListByInvoiceItemId[i].CdStructure;

                    excel_Worksheet.Cells[cellRow, 11].Value = TblInvoiceRptTOListByInvoiceItemId[i].CgstPct;
                    excel_Worksheet.Cells[cellRow, 12].Value = TblInvoiceRptTOListByInvoiceItemId[i].SgstPct;
                    excel_Worksheet.Cells[cellRow, 13].Value = TblInvoiceRptTOListByInvoiceItemId[i].IgstPct;
                    excel_Worksheet.Cells[cellRow, 14].Value = TblInvoiceRptTOListByInvoiceItemId[i].InvoiceQty;
                    excel_Worksheet.Cells[cellRow, 15].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].TaxableAmt / 1000, 2);

                    excel_Worksheet.Cells[cellRow, 16].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].CgstTaxAmt / 1000, 2);
                    excel_Worksheet.Cells[cellRow, 17].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].SgstTaxAmt / 1000, 2);
                    excel_Worksheet.Cells[cellRow, 18].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].IgstTaxAmt / 1000, 2);
                    excel_Worksheet.Cells[cellRow, 19].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].GrandTotal / 1000, 2);
                    excel_Worksheet.Cells[cellRow, 20].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].CdAmt / 1000, 2);
                    excel_Worksheet.Cells[cellRow, 21].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].GrandTotal / 1000, 2);

                    invoiceId = TblInvoiceRptTOListByInvoiceItemId[i].IdInvoice;
                    cellRow++;

                    // For last record.
                    if (i == (TblInvoiceRptTOListByInvoiceItemId.Count - 1))
                    {
                       cellRow++;

                        // For final total.
                        excel_Worksheet.Cells[cellRow, 1].Value = "Grand Total";
                        excel_Worksheet.Cells[cellRow, 14].Value = TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.InvoiceQty);
                        excel_Worksheet.Cells[cellRow, 15].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.TaxableAmt / 1000), 2);

                        excel_Worksheet.Cells[cellRow, 16].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.CgstTaxAmt / 1000), 2);
                        excel_Worksheet.Cells[cellRow, 17].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.SgstTaxAmt / 1000), 2);
                        excel_Worksheet.Cells[cellRow, 18].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.IgstTaxAmt / 1000), 2);
                        excel_Worksheet.Cells[cellRow, 19].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.GrandTotal / 1000), 2);
                        excel_Worksheet.Cells[cellRow, 20].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.CdAmt / 1000), 2);
                        excel_Worksheet.Cells[cellRow, 21].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.GrandTotal / 1000), 2);

                        excel_Worksheet.Cells[cellRow, 1, cellRow, 21].Style.Font.Bold = true;

                        using (ExcelRange range = excel_Worksheet.Cells[1, 1, cellRow, 21])
                        {
                            range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                            range.Style.Font.Name = "Times New Roman";
                            range.Style.Font.Size = 10;
                        }
                    }
                }
                
                #endregion

                #region Upload File to Azure

                // Create azure storage  account connection.
                CloudStorageAccount storage_Account = CloudStorageAccount.Parse(StaticStuff.Constants.AzureConnectionStr);

                // Create the blob client.
                CloudBlobClient blob_Client = storage_Account.CreateCloudBlobClient();

                // Retrieve reference to a target container.
                CloudBlobContainer container_ = blob_Client.GetContainerReference(Constants.AzureSourceContainerName);

                String file_Name = Constants.ExcelFileName + Constants.ServerDateTime.ToString("ddMMyyyyHHmmss") + "-" + minDate + "-" + maxDate + "-RW" + ".xlsx";
                CloudBlockBlob block_Blob = container_.GetBlockBlobReference(file_Name);

                var file_Stream = excelPackage.GetAsByteArray();

                Task t2 = block_Blob.UploadFromByteArrayAsync(file_Stream, 0, file_Stream.Length);

                #endregion

                #endregion

                resultMessage.DefaultSuccessBehaviour();
                return 1;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "CreateTempInvoiceExcel");
                return -1;
            }
        }

        #endregion


        #region Update Identity
        public static int UpdateIdentityFinalTables(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                cmdUpdate.CommandTimeout = 0;

                String sqlQuery = null;

                foreach (DelTranTablesE tableName in Enum.GetValues(typeof(DelTranTablesE)))
                {
                    switch (tableName)
                    {
                        case DelTranTablesE.tempInvoice:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idInvoice) FROM finalInvoice),0) " +
                                       " DBCC CHECKIDENT(finalInvoice, RESEED, @id) ";
                            break;

                        case DelTranTablesE.tempInvoiceAddress:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idInvoiceAddr) FROM finalInvoiceAddress),0) " +
                                       " DBCC CHECKIDENT(finalInvoiceAddress, RESEED, @id) ";
                            break;

                        //[28-05-2018]:Vijaymala added to update identity of finalInvoiceDocumentDetails
                        case DelTranTablesE.tempInvoiceDocumentDetails:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idInvoiceDocument) FROM finalInvoiceDocumentDetails),0) " +
                                       " DBCC CHECKIDENT(finalInvoiceDocumentDetails, RESEED, @id) ";
                            break;


                        case DelTranTablesE.tempInvoiceHistory:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idInvHistory) FROM finalInvoiceHistory),0) " +
                                       " DBCC CHECKIDENT(finalInvoiceHistory, RESEED, @id) ";
                            break;

                        case DelTranTablesE.tempInvoiceItemDetails:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idInvoiceItem) FROM finalInvoiceItemDetails),0) " +
                                       " DBCC CHECKIDENT(finalInvoiceItemDetails, RESEED, @id) ";
                            break;

                        case DelTranTablesE.tempInvoiceItemTaxDtls:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idInvItemTaxDtl) FROM finalInvoiceItemTaxDtls),0) " +
                                       " DBCC CHECKIDENT(finalInvoiceItemTaxDtls, RESEED, @id) ";
                            break;

                        case DelTranTablesE.tempLoading:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idLoading) FROM finalLoading),0) " +
                                       " DBCC CHECKIDENT(finalLoading, RESEED, @id) ";
                            break;

                        case DelTranTablesE.tempLoadingSlip:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idLoadingSlip) FROM finalLoadingSlip),0) " +
                                       " DBCC CHECKIDENT(finalLoadingSlip, RESEED, @id) ";
                            break;

                        case DelTranTablesE.tempLoadingSlipAddress:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idLoadSlipAddr) FROM finalLoadingSlipAddress),0) " +
                                       " DBCC CHECKIDENT(finalLoadingSlipAddress, RESEED, @id) ";
                            break;

                        case DelTranTablesE.tempLoadingSlipDtl:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idLoadSlipDtl) FROM finalLoadingSlipDtl),0) " +
                                       " DBCC CHECKIDENT(finalLoadingSlipDtl, RESEED, @id) ";
                            break;

                        case DelTranTablesE.tempLoadingSlipExt:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idLoadingSlipExt) FROM finalLoadingSlipExt),0) " +
                                       " DBCC CHECKIDENT(finalLoadingSlipExt, RESEED, @id) ";
                            break;

                        case DelTranTablesE.tempLoadingSlipExtHistory:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idConfirmHistory) FROM finalLoadingSlipExtHistory),0) " +
                                       " DBCC CHECKIDENT(finalLoadingSlipExtHistory, RESEED, @id) ";
                            break;

                        case DelTranTablesE.tempLoadingStatusHistory:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idLoadingHistory) FROM finalLoadingStatusHistory),0) " +
                                       " DBCC CHECKIDENT(finalLoadingStatusHistory, RESEED, @id) ";
                            break;

                        case DelTranTablesE.tempWeighingMeasures:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idWeightMeasure) FROM finalWeighingMeasures),0) " +
                                       " DBCC CHECKIDENT(finalWeighingMeasures, RESEED, @id)";
                            break;
                    }


                    if (sqlQuery != null)
                    {
                        cmdUpdate.CommandText = sqlQuery;
                        result = cmdUpdate.ExecuteNonQuery();
                    }
                    else
                        result = -1;
                }
                return result;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateIdentityFinalTables");
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }
        #endregion
    }
}
