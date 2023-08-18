using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.BL
{
    public class FinalEnquiryData
    {
        #region Insertion Method


        public static ResultMessage InsertFinalEnquiryData(int loadingId, SqlConnection bookingConn, SqlTransaction bookingTran, SqlConnection enquiryConn, SqlTransaction enquiryTran)
        {
            ResultMessage resultMessage = new ResultMessage();
            TblLoadingTO loadingTO = new TblLoadingTO();
            List<TblLoadingSlipTO> loadingSlipTOList = new List<TblLoadingSlipTO>();
            List<TblLoadingSlipTO> enquiryTOList = new List<TblLoadingSlipTO>();
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
                loadingTO = TblLoadingBL.SelectTblLoadingTO(loadingId, bookingConn, bookingTran);

                if (loadingTO == null)
                {
                    resultMessage.DefaultBehaviour("Error Enquiry loadingTO is null");
                    return resultMessage;
                }

                // Select temp loading slip details.
                loadingSlipTOList = TblLoadingSlipBL.SelectAllLoadingSlipListWithDetails(loadingId, bookingConn, bookingTran);
                // Select loading slip status history data.
                loadingStatusHistoryTOList = TblLoadingStatusHistoryBL.SelectAllTblLoadingStatusHistoryList(loadingId, bookingConn, bookingTran);
                // Select weighingMeasures details.
                weighingMeasuresTOList = TblWeighingMeasuresBL.SelectAllTblWeighingMeasuresListByLoadingId(loadingId, bookingConn, bookingTran);

                #endregion

                // To check not confirm loading slips are present or not.
                if (loadingTO != null && loadingSlipTOList != null && loadingSlipTOList.FindAll(ele => ele.IsConfirmed == 0).Count > 0)
                {

                    // Filter loading slip list with confirm flag.
                    if (loadingSlipTOList != null && loadingSlipTOList.Count > 0)
                        enquiryTOList = loadingSlipTOList.FindAll(ele => ele.IsConfirmed == 0).ToList();


                    // Insert final loading slip details.    
                    if (enquiryTOList != null && enquiryTOList.Count > 0)
                    {

                        // Insert final loading details.
                        // Set total loading qty to 0. 
                        loadingTO.TotalLoadingQty = 0;
                        loadingTO.NoOfDeliveries = enquiryTOList.Count;

                        result = InsertFinalLoading(loadingTO, enquiryConn, enquiryTran);

                        if (result <= 0)
                        {
                            resultMessage.DefaultBehaviour("Error while Enquiry InsertFinalLoading");
                            return resultMessage;
                        }

                        // Maintain history loadinSlipExtTO.
                        TblLoadingSlipExtTO oldLoadingSlipExtTO = null;                       
                        int loadingSlipExtTOCnt = 1;

                        foreach (var loadingSlipTO in enquiryTOList)
                        {
                            List<TblLoadingSlipExtTO> newLoadingSlipExtTOList = new List<TblLoadingSlipExtTO>();
                            // Set old loading slip id.
                            oldLoadingSlipId = loadingSlipTO.IdLoadingSlip;

                            // Select invoice details.
                            List<TblInvoiceTO> invoiceTOList = TblInvoiceBL.SelectTempInvoiceTOList(loadingSlipTO.IdLoadingSlip, bookingConn, bookingTran);

                            if (invoiceTOList == null && invoiceTOList.Count <= 0)
                            {
                                resultMessage.DefaultBehaviour("Error invoiceTOList is null");
                                return resultMessage;
                            }

                            // Insert loading slip. 
                            loadingSlipTO.LoadingId = loadingTO.IdLoading;
                            loadingSlipTO.NoOfDeliveries = enquiryTOList.Count;
                            result = InsertFinalLoadingSlip(loadingSlipTO, enquiryConn, enquiryTran);
                            if (result <= 0)
                            {
                                resultMessage.DefaultBehaviour("Error while Enquiry InsertFinalLoadingSlip");
                                return resultMessage;
                            }

                            // Insert loading slip address details.
                            if (loadingSlipTO.DeliveryAddressTOList != null && loadingSlipTO.DeliveryAddressTOList.Count > 0)
                            {
                                foreach (var loadingSlipAddressTO in loadingSlipTO.DeliveryAddressTOList)
                                {
                                    loadingSlipAddressTO.LoadingSlipId = loadingSlipTO.IdLoadingSlip;
                                    result = InsertFinalLoadingSlipAddress(loadingSlipAddressTO, enquiryConn, enquiryTran);
                                    if (result <= 0)
                                    {
                                        resultMessage.DefaultBehaviour("Error while Enquiry InsertFinalLoadingSlipAddress");
                                        return resultMessage;
                                    }
                                }
                            }

                            // Insert loading slip details data.
                            if (loadingSlipTO.TblLoadingSlipDtlTO != null)
                            {
                                loadingSlipTO.TblLoadingSlipDtlTO.LoadingSlipId = loadingSlipTO.IdLoadingSlip;
                                totalLoadingQty = totalLoadingQty + loadingSlipTO.TblLoadingSlipDtlTO.LoadingQty;

                                result = InsertFinalLoadingSlipDtl(loadingSlipTO.TblLoadingSlipDtlTO, enquiryConn, enquiryTran);
                                if (result <= 0)
                                {
                                    resultMessage.DefaultBehaviour("Error while Enquiry InsertFinalLoadingSlipDtl");
                                    return resultMessage;
                                }

                            }

                            // Insert loading slip ext details.
                            if (loadingSlipTO.LoadingSlipExtTOList != null && loadingSlipTO.LoadingSlipExtTOList.Count > 0)
                            {
                                //Sanjay Gunjal [04-July-2021] Added ordering of list by weighing sequence number as it was doing wrong tare weight calculation.
                                var extList = loadingSlipTO.LoadingSlipExtTOList.OrderBy(ws => ws.WeighingSequenceNumber).ToList();

                                foreach (var loadinSlipExtTO in extList)
                                {
                                    // Insert to old loadingSlipExtTOList.
                                    oldLoadingSlipExtTOList.Add(loadinSlipExtTO);

                                    // Select loading slip ext history data.
                                    List<TblLoadingSlipExtHistoryTO> loadingSlipExtHistoryTOList = TblLoadingSlipExtHistoryBL.SelectTempLoadingSlipExtHistoryTOList(loadinSlipExtTO.IdLoadingSlipExt, bookingConn, bookingTran);

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
                                        List<TblLoadingSlipExtTO> oldAllLoadingSlipExtTOList = TblLoadingSlipExtBL.SelectAllLoadingSlipExtListFromLoadingId(loadingId, bookingConn, bookingTran);

                                        //Sanjay Gunjal [04-July-2021] Added ordering of list by CalcTareWeight as it was doing wrong tare weight calculation.
                                        //loadinSlipExtTO.CalcTareWeight = oldAllLoadingSlipExtTOList[0].CalcTareWeight;
                                        if (oldAllLoadingSlipExtTOList != null && oldAllLoadingSlipExtTOList.Count > 0)
                                        {
                                            loadinSlipExtTO.CalcTareWeight = oldAllLoadingSlipExtTOList.OrderBy(tw => tw.CalcTareWeight).FirstOrDefault().CalcTareWeight;
                                        }
                                    }

                                    result = InsertFinalLoadingSlipExt(loadinSlipExtTO, enquiryConn, enquiryTran);
                                    if (result <= 0)
                                    {
                                        resultMessage.DefaultBehaviour("Error while Enquiry InsertFinalLoadingSlipExt");
                                        return resultMessage;
                                    }
                                    else
                                    {
                                        loadinSlipExthistoryList.Add(new KeyValuePair<int, int>(oldloadinSlipExtId, loadinSlipExtTO.IdLoadingSlipExt));

                                        // Assign old loadinSlipExtTO.
                                        oldLoadingSlipExtTO = loadinSlipExtTO;
                                        newLoadingSlipExtTOList.Add(loadinSlipExtTO);
                                    }


                                    // Insert loading slip ext history details.
                                    if (loadingSlipExtHistoryTOList != null && loadingSlipExtHistoryTOList.Count > 0)
                                    {
                                        foreach (var loadinSlipExtHistoryTO in loadingSlipExtHistoryTOList)
                                        {
                                            loadinSlipExtHistoryTO.LoadingSlipExtId = loadinSlipExtTO.IdLoadingSlipExt;
                                            result = InsertFinalLoadingSlipExtHistory(loadinSlipExtHistoryTO, enquiryConn, enquiryTran);
                                            if (result <= 0)
                                            {
                                                resultMessage.DefaultBehaviour("Error while Enquiry InsertFinalLoadingSlipExtHistory");
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
                                    // Select invoice address details.
                                    List<TblInvoiceAddressTO> invoiceAddressTOList = TblInvoiceAddressBL.SelectAllTblInvoiceAddressList(invoiceTO.IdInvoice, bookingConn, bookingTran);
                                    // Select invoice item details.
                                    List<TblInvoiceItemDetailsTO> invoiceItemDetailsTOList = TblInvoiceItemDetailsBL.SelectAllTblInvoiceItemDetailsList(invoiceTO.IdInvoice, bookingConn, bookingTran);
                                    // Select invoice history details.
                                    List<TblInvoiceHistoryTO> invoiceHistoryTOList = TblInvoiceHistoryBL.SelectTempInvoiceHistory(invoiceTO.IdInvoice, bookingConn, bookingTran);
                                    invoiceTO.LoadingSlipId = loadingSlipTO.IdLoadingSlip;

                                    //[28-05-2018]:Vijaymla added to get invoice document details
                                    List<TempInvoiceDocumentDetailsTO> tempInvoiceDocumentDetailsTOList = TempInvoiceDocumentDetailsBL.SelectTempInvoiceDocumentDetailsByInvoiceId(invoiceTO.IdInvoice, bookingConn, bookingTran);

                                    // Weighing calculation.
                                    if (newLoadingSlipExtTOList != null)
                                    {
                                        invoiceTO.TareWeight = newLoadingSlipExtTOList.Select(ele => ele.CalcTareWeight).First();
                                        invoiceTO.NetWeight = newLoadingSlipExtTOList.Sum(ele => ele.LoadedWeight);
                                        invoiceTO.GrossWeight = newLoadingSlipExtTOList.Select(ele => ele.LoadedWeight).Last() + newLoadingSlipExtTOList.Select(ele => ele.CalcTareWeight).Last();
                                    }

                                    result = InsertFinalInvoice(invoiceTO, enquiryConn, enquiryTran);
                                    if (result <= 0)
                                    {
                                        resultMessage.DefaultBehaviour("Error while Enquiry InsertFinalInvoice");
                                        return resultMessage;
                                    }

                                    // Insert invoice address details.
                                    if (invoiceAddressTOList != null && invoiceAddressTOList.Count > 0)
                                    {
                                        foreach (var invoiceAddressTO in invoiceAddressTOList)
                                        {
                                            invoiceAddressTO.InvoiceId = invoiceTO.IdInvoice;
                                            result = InsertFinalInvoiceAddress(invoiceAddressTO, enquiryConn, enquiryTran);
                                            if (result <= 0)
                                            {
                                                resultMessage.DefaultBehaviour("Error while Enquiry InsertFinalInvoiceAddress");
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
                                            result = InsertEnquiryInvoiceDocumentDetails(tempInvoiceDocumentDetailsTO, enquiryConn, enquiryTran);
                                            if (result <= 0)
                                            {
                                                resultMessage.DefaultBehaviour("Error while Enquiry InsertEnquiryInvoiceDocumentDetails");
                                                return resultMessage;
                                            }
                                        }
                                    }



                                    if (invoiceItemDetailsTOList != null && invoiceItemDetailsTOList.Count > 0)
                                    {
                                        foreach (var invoiceItemDetailsTO in invoiceItemDetailsTOList)
                                        {
                                            // Select invoice item tax details.
                                            List<TblInvoiceItemTaxDtlsTO> invoiceItemTaxDtlsTOList = TblInvoiceItemTaxDtlsBL.SelectAllTblInvoiceItemTaxDtlsList(invoiceItemDetailsTO.IdInvoiceItem, bookingConn, bookingTran);

                                            invoiceItemDetailsTO.InvoiceId = invoiceTO.IdInvoice;
                                            int oldInvoiceItemId = invoiceItemDetailsTO.IdInvoiceItem;

                                            if (loadinSlipExthistoryList != null && loadinSlipExthistoryList.Count > 0)
                                                invoiceItemDetailsTO.LoadingSlipExtId = loadinSlipExthistoryList.Find(ele => ele.Key == invoiceItemDetailsTO.LoadingSlipExtId).Value;

                                            result = InsertFinalInvoiceItemDetails(invoiceItemDetailsTO, enquiryConn, enquiryTran);
                                            if (result <= 0)
                                            {
                                                resultMessage.DefaultBehaviour("Error while Enquiry InsertFinalInvoiceItemDetails");
                                                return resultMessage;
                                            }
                                            else
                                            {
                                                invoiceItemDetailshistoryList.Add(new KeyValuePair<int, int>(oldInvoiceItemId, invoiceItemDetailsTO.IdInvoiceItem));
                                            }

                                            // Vaibhav [01-Dec-2017] No need to insert tax data [Ref By - Sanjay Sir]

                                            //if (invoiceItemTaxDtlsTOList != null && invoiceItemTaxDtlsTOList.Count > 0)
                                            //{
                                            //    foreach (var invoiceItemTaxDtlsTO in invoiceItemTaxDtlsTOList)
                                            //    {
                                            //        invoiceItemTaxDtlsTO.InvoiceItemId = invoiceItemDetailsTO.IdInvoiceItem;
                                            //        result = InsertFinalInvoiceItemTaxDtls(invoiceItemTaxDtlsTO, enquiryConn, enquiryTran);
                                            //        if (result <= 0)
                                            //        {
                                            //            resultMessage.DefaultBehaviour("Error while Enquiry InsertFinalInvoiceItemTaxDtls");
                                            //            enquiryTran.Rollback();
                                            //            return resultMessage;
                                            //        }
                                            //    }
                                            //}
                                        }
                                    }

                                    if (invoiceHistoryTOList != null && invoiceHistoryTOList.Count > 0)
                                    {
                                        foreach (var invoiceHistoryTO in invoiceHistoryTOList)
                                        {
                                            invoiceHistoryTO.InvoiceId = invoiceTO.IdInvoice;

                                            if (invoiceItemDetailshistoryList != null && invoiceItemDetailshistoryList.Count > 0)
                                                invoiceHistoryTO.InvoiceItemId = invoiceItemDetailshistoryList.Find(ele => ele.Key == invoiceHistoryTO.InvoiceItemId).Value;

                                            result = InsertFinalInvoiceHistory(invoiceHistoryTO, enquiryConn, enquiryTran);
                                            if (result <= 0)
                                            {
                                                resultMessage.DefaultBehaviour("Error while Enquiry InsertFinalInvoiceHistory");
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
                            // Filter weighingMeasures list by weightMeasureId from old loadingSlipExtTOList.
                            List<TblWeighingMeasuresTO> newWeighingMeasuresTOList = weighingMeasuresTOList.Where(item =>
                              oldLoadingSlipExtTOList.Any(ele => ele.WeightMeasureId.Equals(item.IdWeightMeasure))).ToList();

                            Double totalWeightMT = 0;


                            // First insert tare weight.
                            List<TblWeighingMeasuresTO> tareWeighingMeasuresTOList = weighingMeasuresTOList.FindAll(ele => ele.WeightMeasurTypeId == (int)Constants.TransMeasureTypeE.TARE_WEIGHT);

                            if (tareWeighingMeasuresTOList != null && tareWeighingMeasuresTOList.Count > 0)
                            {
                                foreach (var tareWeighingMeasuresTO in tareWeighingMeasuresTOList)
                                {
                                    totalWeightMT = tareWeighingMeasuresTO.WeightMT;
                                    tareWeighingMeasuresTO.LoadingId = loadingTO.IdLoading;

                                    result = InsertFinalWeighingMeasures(tareWeighingMeasuresTO, enquiryConn, enquiryTran);

                                    if (result <= 0)
                                    {
                                        resultMessage.DefaultBehaviour("Error while Enquiry InsertFinalWeighingMeasures");
                                        return resultMessage;
                                    }
                                }
                            }

                            foreach (var weighingMeasuresTO in newWeighingMeasuresTOList)
                            {
                                // Calculate only enquiry weighing.
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

                                result = InsertFinalWeighingMeasures(weighingMeasuresTO, enquiryConn, enquiryTran);
                                if (result <= 0)
                                {
                                    resultMessage.DefaultBehaviour("Error while Enquiry InsertFinalWeighingMeasures");
                                    return resultMessage;
                                }

                                // Update weighingMeasureid in finalLoadingSlipExt table.
                                result = UpdateFinalLoadingSlipExt(oldWeighingMeasuresId, weighingMeasuresTO.IdWeightMeasure, enquiryConn, enquiryTran);
                                if (result < 0)
                                {
                                    resultMessage.DefaultBehaviour("Error while Enquiry UpdateFinalLoadingSlipExt");
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
                                    result = InsertFinalWeighingMeasures(grossWeighingMeasuresTO, enquiryConn, enquiryTran);

                                    if (result <= 0)
                                    {
                                        resultMessage.DefaultBehaviour("Error while Enquiry InsertFinalWeighingMeasures");
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
                                result = InsertFinalLoadingStatusHistory(loadingStatusHistoryTO, enquiryConn, enquiryTran);
                                if (result <= 0)
                                {
                                    resultMessage.DefaultBehaviour("Error while Enquiry InsertFinalLoadingStatusHistory");
                                    return resultMessage;
                                }
                            }
                        }

                        // Update total loading qty.
                        result = UpdateTotalLoadingQty(totalLoadingQty, loadingTO.IdLoading, enquiryConn, enquiryTran);
                        if (result < 0)
                        {
                            resultMessage.DefaultBehaviour("Error while Enquiry UpdateTotalLoadingQty");
                            return resultMessage;
                        }

                    }
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertFinalEnquiryData");
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
            String sqlQuery = @" INSERT INTO [enquiryLoading]( " +
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
            cmdInsert.Parameters.Add("@VehicleNo", System.Data.SqlDbType.VarChar).Value = DBNull.Value;//tblLoadingTO.VehicleNo;
            cmdInsert.Parameters.Add("@StatusReason", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.StatusReason);
            cmdInsert.Parameters.Add("@cnfOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.CnfOrgId);
            cmdInsert.Parameters.Add("@totalLoadingQty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.TotalLoadingQty);
            cmdInsert.Parameters.Add("@statusReasonId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.StatusReasonId);
            cmdInsert.Parameters.Add("@transporterOrgId", System.Data.SqlDbType.Int).Value = DBNull.Value;//Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.TransporterOrgId);
            cmdInsert.Parameters.Add("@freightAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.FreightAmt);
            cmdInsert.Parameters.Add("@superwisorId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.SuperwisorId);
            cmdInsert.Parameters.Add("@isFreightIncluded", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.IsFreightIncluded);
            cmdInsert.Parameters.Add("@contactNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.ContactNo);
            cmdInsert.Parameters.Add("@driverName", System.Data.SqlDbType.NVarChar).Value = DBNull.Value;//Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.DriverName);
            cmdInsert.Parameters.Add("@parentLoadingId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.ParentLoadingId);
            cmdInsert.Parameters.Add("@callFlag", System.Data.SqlDbType.Int).Value = tblLoadingTO.CallFlag;
            cmdInsert.Parameters.Add("@flagUpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.FlagUpdatedOn);
            cmdInsert.Parameters.Add("@isAllowNxtLoading", System.Data.SqlDbType.Int).Value = tblLoadingTO.IsAllowNxtLoading;
            cmdInsert.Parameters.Add("@loadingType", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.LoadingType);
            cmdInsert.Parameters.Add("@currencyId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.CurrencyId);
            cmdInsert.Parameters.Add("@currencyRate", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.CurrencyRate);
            cmdInsert.Parameters.Add("@callFlagBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.CallFlagBy);

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
            String sqlQuery = @" INSERT INTO [enquiryLoadingSlip]( " +
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
                             " ,[orcAmt]" +
                             " ,[orcMeasure]" +                 //Priyanka [08-05-2018]
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
                              " ,@orcAmt" +                 //Priyanka [08-05-2018]
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
            cmdInsert.Parameters.Add("@VehicleNo", System.Data.SqlDbType.VarChar).Value = DBNull.Value;//tblLoadingSlipTO.VehicleNo;
            cmdInsert.Parameters.Add("@LoadingId", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.LoadingId;
            cmdInsert.Parameters.Add("@statusReasonId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.StatusReasonId);
            cmdInsert.Parameters.Add("@loadingSlipNo", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.LoadingSlipNo);
            cmdInsert.Parameters.Add("@isConfirmed", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.IsConfirmed;
            cmdInsert.Parameters.Add("@comment", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.Comment);
            cmdInsert.Parameters.Add("@contactNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.ContactNo);
            cmdInsert.Parameters.Add("@driverName", System.Data.SqlDbType.NVarChar).Value = DBNull.Value;//Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.DriverName);
            cmdInsert.Parameters.Add("@cdStructureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.CdStructureId);
            cmdInsert.Parameters.Add("@orcAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.OrcAmt);                          //Priyanka [08-05-20018]
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
            String sqlQuery = @" INSERT INTO [enquiryLoadingSlipAddress]( " +
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
            String sqlQuery = @" INSERT INTO [enquiryLoadingSlipDtl]( " +
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
            String sqlQuery = @" INSERT INTO [enquiryLoadingSlipExt]( " +
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
            cmdInsert.Parameters.Add("@ratePerMT", System.Data.SqlDbType.Decimal).Value = DBNull.Value;//tblLoadingSlipExtTO.RatePerMT;
            cmdInsert.Parameters.Add("@rateCalcDesc", System.Data.SqlDbType.NVarChar, 256).Value = DBNull.Value;//Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.RateCalcDesc);
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
            cmdInsert.Parameters.Add("@taxableRateMT", System.Data.SqlDbType.Decimal).Value = DBNull.Value;//Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.TaxableRateMT);
            cmdInsert.Parameters.Add("@freExpOtherAmt", System.Data.SqlDbType.Decimal).Value = DBNull.Value; //Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.FreExpOtherAmt);
            cmdInsert.Parameters.Add("@cdApplicableAmt", System.Data.SqlDbType.Decimal).Value = DBNull.Value; //Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.CdApplicableAmt);
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
            String sqlQuery = @" INSERT INTO [enquiryLoadingSlipExtHistory]( " +
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
            cmdInsert.Parameters.Add("@LastRatePerMT", System.Data.SqlDbType.NVarChar).Value = DBNull.Value;//tblLoadingSlipExtHistoryTO.LastRatePerMT;
            cmdInsert.Parameters.Add("@CurrentRatePerMT", System.Data.SqlDbType.NVarChar).Value = DBNull.Value;//tblLoadingSlipExtHistoryTO.CurrentRatePerMT;
            cmdInsert.Parameters.Add("@LastRateCalcDesc", System.Data.SqlDbType.NVarChar).Value = DBNull.Value;//Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtHistoryTO.LastRateCalcDesc);
            cmdInsert.Parameters.Add("@CurrentRateCalcDesc", System.Data.SqlDbType.NVarChar).Value = DBNull.Value;//Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtHistoryTO.CurrentRateCalcDesc);
            cmdInsert.Parameters.Add("@lastCdAplAmt", System.Data.SqlDbType.Decimal).Value = DBNull.Value;//tblLoadingSlipExtHistoryTO.LastCdAplAmt;
            cmdInsert.Parameters.Add("@currentCdAplAmt", System.Data.SqlDbType.Decimal).Value = DBNull.Value;//tblLoadingSlipExtHistoryTO.CurrentCdAplAmt;

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
            String sqlQuery = @" INSERT INTO [enquiryWeighingMeasures]( " +
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
            cmdInsert.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = DBNull.Value;//tblWeighingMeasuresTO.VehicleNo;
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
            String sqlQuery = @" INSERT INTO [enquiryLoadingStatusHistory]( " +
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
            String sqlQuery = @" INSERT INTO [enquiryInvoice]( " +
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
                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdInvoice", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IdInvoice;
            cmdInsert.Parameters.Add("@InvoiceTypeId", System.Data.SqlDbType.Int).Value = tblInvoiceTO.InvoiceTypeId;
            cmdInsert.Parameters.Add("@TransportOrgId", System.Data.SqlDbType.Int).Value = DBNull.Value;//Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TransportOrgId);
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
            cmdInsert.Parameters.Add("@BasicAmt", System.Data.SqlDbType.NVarChar).Value = DBNull.Value;//tblInvoiceTO.BasicAmt;
            cmdInsert.Parameters.Add("@DiscountAmt", System.Data.SqlDbType.NVarChar).Value = DBNull.Value;//tblInvoiceTO.DiscountAmt;
            cmdInsert.Parameters.Add("@TaxableAmt", System.Data.SqlDbType.NVarChar).Value = DBNull.Value;//tblInvoiceTO.TaxableAmt;
            cmdInsert.Parameters.Add("@CgstAmt", System.Data.SqlDbType.NVarChar).Value = DBNull.Value;//tblInvoiceTO.CgstAmt;
            cmdInsert.Parameters.Add("@SgstAmt", System.Data.SqlDbType.NVarChar).Value = DBNull.Value;//tblInvoiceTO.SgstAmt;
            cmdInsert.Parameters.Add("@IgstAmt", System.Data.SqlDbType.NVarChar).Value = DBNull.Value;//tblInvoiceTO.IgstAmt;
            cmdInsert.Parameters.Add("@FreightPct", System.Data.SqlDbType.NVarChar).Value = DBNull.Value;//tblInvoiceTO.FreightPct;
            cmdInsert.Parameters.Add("@FreightAmt", System.Data.SqlDbType.NVarChar).Value = DBNull.Value;//tblInvoiceTO.FreightAmt;
            cmdInsert.Parameters.Add("@RoundOffAmt", System.Data.SqlDbType.NVarChar).Value = DBNull.Value;//tblInvoiceTO.RoundOffAmt;
            cmdInsert.Parameters.Add("@GrandTotal", System.Data.SqlDbType.NVarChar).Value = DBNull.Value;//tblInvoiceTO.GrandTotal;
            cmdInsert.Parameters.Add("@InvoiceNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.InvoiceNo);
            cmdInsert.Parameters.Add("@ElectronicRefNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.ElectronicRefNo);
            cmdInsert.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = DBNull.Value;//Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.VehicleNo);
            cmdInsert.Parameters.Add("@LrNumber", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.LrNumber);
            cmdInsert.Parameters.Add("@RoadPermitNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.RoadPermitNo);
            cmdInsert.Parameters.Add("@TransportorForm", System.Data.SqlDbType.NVarChar).Value = DBNull.Value;//Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TransportorForm);
            cmdInsert.Parameters.Add("@AirwayBillNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.AirwayBillNo);
            cmdInsert.Parameters.Add("@Narration", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.Narration);
            cmdInsert.Parameters.Add("@BankDetails", System.Data.SqlDbType.NChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.BankDetails);
            cmdInsert.Parameters.Add("@invoiceModeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.InvoiceModeId);
            cmdInsert.Parameters.Add("@deliveryLocation", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DeliveryLocation);
            cmdInsert.Parameters.Add("@tareWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TareWeight);
            cmdInsert.Parameters.Add("@netWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.NetWeight);
            cmdInsert.Parameters.Add("@grossWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.GrossWeight);
            cmdInsert.Parameters.Add("@changeIn", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.ChangeIn);
            cmdInsert.Parameters.Add("@expenseAmt", System.Data.SqlDbType.NVarChar).Value = DBNull.Value;//tblInvoiceTO.ExpenseAmt;
            cmdInsert.Parameters.Add("@otherAmt", System.Data.SqlDbType.NVarChar).Value = DBNull.Value;//tblInvoiceTO.OtherAmt;
            cmdInsert.Parameters.Add("@isConfirmed", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IsConfirmed;
            cmdInsert.Parameters.Add("@rcmFlag", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.RcmFlag);
            cmdInsert.Parameters.Add("@remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.Remark);
            cmdInsert.Parameters.Add("@invFromOrgId", System.Data.SqlDbType.Int).Value =tblInvoiceTO.InvFromOrgId;
            cmdInsert.Parameters.Add("@deliveredOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DeliveredOn);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblInvoiceTO.IdInvoice = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }

            return 0;
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

        private static int ExecuteInsertionFinalInvoiceAddressCommand(TblInvoiceAddressTO tblInvoiceAddressTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [enquiryInvoiceAddress]( " +
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

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblInvoiceAddressTO.IdInvoiceAddr = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }

            return 0;
        }


        public static int InsertFinalInvoiceItemDetails(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO, SqlConnection conn, SqlTransaction tran)
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

        public static int ExecuteInsertionFinalInvoiceItemDetailsCommand(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [enquiryInvoiceItemDetails]( " +
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
            cmdInsert.Parameters.Add("@Rate", System.Data.SqlDbType.NVarChar).Value = DBNull.Value;//Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.Rate);
            cmdInsert.Parameters.Add("@BasicTotal", System.Data.SqlDbType.NVarChar).Value = DBNull.Value;//Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.BasicTotal);
            cmdInsert.Parameters.Add("@TaxableAmt", System.Data.SqlDbType.NVarChar).Value = DBNull.Value;//tblInvoiceItemDetailsTO.TaxableAmt;
            cmdInsert.Parameters.Add("@GrandTotal", System.Data.SqlDbType.NVarChar).Value = DBNull.Value;//tblInvoiceItemDetailsTO.GrandTotal;
            cmdInsert.Parameters.Add("@ProdItemDesc", System.Data.SqlDbType.NVarChar).Value = tblInvoiceItemDetailsTO.ProdItemDesc;
            cmdInsert.Parameters.Add("@cdStructure", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.CdStructure);
            cmdInsert.Parameters.Add("@cdAmt", System.Data.SqlDbType.Decimal).Value = DBNull.Value;//Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.CdAmt);
            cmdInsert.Parameters.Add("@gstinCodeNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.GstinCodeNo);
            cmdInsert.Parameters.Add("@otherTaxId", System.Data.SqlDbType.Int).Value = DBNull.Value;//Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.OtherTaxId);
            cmdInsert.Parameters.Add("@taxPct", System.Data.SqlDbType.Decimal).Value = DBNull.Value;//Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.TaxPct);
            cmdInsert.Parameters.Add("@taxAmt", System.Data.SqlDbType.Decimal).Value = DBNull.Value;//Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.TaxAmt);
            cmdInsert.Parameters.Add("@cdStructureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceItemDetailsTO.CdStructureId);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblInvoiceItemDetailsTO.IdInvoiceItem = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }

            return 0;
        }


        public static int InsertFinalInvoiceItemTaxDtls(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO, SqlConnection conn, SqlTransaction tran)
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

        public static int ExecuteInsertionFinalInvoiceItemTaxDtlsCommand(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [enquiryInvoiceItemTaxDtls]( " +
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


        public static int InsertFinalInvoiceHistory(TblInvoiceHistoryTO tblInvoiceHistoryTO, SqlConnection conn, SqlTransaction tran)
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

        public static int ExecuteInsertionFinalInvoiceHistoryCommand(TblInvoiceHistoryTO tblInvoiceHistoryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [enquiryInvoiceHistory]( " +
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
            cmdInsert.Parameters.Add("@OldCdStructureId", System.Data.SqlDbType.Int).Value = DBNull.Value;//Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.OldCdStructureId);
            cmdInsert.Parameters.Add("@NewCdStructureId", System.Data.SqlDbType.Int).Value = DBNull.Value;//Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.NewCdStructureId);
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblInvoiceHistoryTO.StatusId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblInvoiceHistoryTO.CreatedBy;
            cmdInsert.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblInvoiceHistoryTO.StatusDate;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblInvoiceHistoryTO.CreatedOn;
            cmdInsert.Parameters.Add("@OldUnitRate", System.Data.SqlDbType.NVarChar).Value = DBNull.Value;//Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.OldUnitRate);
            cmdInsert.Parameters.Add("@NewUnitRate", System.Data.SqlDbType.NVarChar).Value = DBNull.Value;//Constants.GetSqlDataValueNullForBaseValue(tblInvoiceHistoryTO.NewUnitRate);
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
        public static int InsertEnquiryInvoiceDocumentDetails(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionEnquiryInvoiceDocumentDetailsCommand(tempInvoiceDocumentDetailsTO, cmdInsert);
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

        public static int ExecuteInsertionEnquiryInvoiceDocumentDetailsCommand(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [enquiryInvoiceDocumentDetails]( " +
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
            String sqlQuery = @" UPDATE [enquiryLoadingSlipExt] SET " +
            "  [weightMeasureId] = @NewWeightMeasureId" +
            " WHERE [weightMeasureId]= @OldWeightMeasureId ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@OldWeightMeasureId", System.Data.SqlDbType.Int).Value = oldWeightMeasureId;
            cmdUpdate.Parameters.Add("@NewWeightMeasureId", System.Data.SqlDbType.Int).Value = newWeightMeasureId;

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
            String sqlQuery = @" UPDATE [enquiryLoading] SET " +
            "  [totalLoadingQty] = @TotalLoadingQty " +
            " WHERE [idLoading] = @LoadingId";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@TotalLoadingQty", System.Data.SqlDbType.Decimal).Value = totalLoadingQty;
            cmdUpdate.Parameters.Add("@LoadingId", System.Data.SqlDbType.Int).Value = loadingId;

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

                foreach (FinalBookingData.DelTranTablesE tableName in Enum.GetValues(typeof(FinalBookingData.DelTranTablesE)))
                {
                    switch (tableName)
                    {
                        case FinalBookingData.DelTranTablesE.tempInvoice:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idInvoice) FROM enquiryInvoice),0) " +
                                       " DBCC CHECKIDENT(enquiryInvoice, RESEED, @id) ";
                            break;

                        case FinalBookingData.DelTranTablesE.tempInvoiceAddress:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idInvoiceAddr) FROM enquiryInvoiceAddress),0) " +
                                       " DBCC CHECKIDENT(enquiryInvoiceAddress, RESEED, @id) ";
                            break;

                        //[28-05-2018]:Vijaymala added to update identity of enquiryInvoicedocumentdetails
                        case FinalBookingData.DelTranTablesE.tempInvoiceDocumentDetails:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idInvoiceDocument) FROM enquiryInvoicedocumentdetails),0) " +
                                       " DBCC CHECKIDENT(enquiryInvoicedocumentdetails, RESEED, @id) ";
                            break;

                        case FinalBookingData.DelTranTablesE.tempInvoiceHistory:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idInvHistory) FROM enquiryInvoiceHistory),0) " +
                                       " DBCC CHECKIDENT(enquiryInvoiceHistory, RESEED, @id) ";
                            break;

                        case FinalBookingData.DelTranTablesE.tempInvoiceItemDetails:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idInvoiceItem) FROM enquiryInvoiceItemDetails),0) " +
                                       " DBCC CHECKIDENT(enquiryInvoiceItemDetails, RESEED, @id) ";
                            break;

                        case FinalBookingData.DelTranTablesE.tempInvoiceItemTaxDtls:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idInvItemTaxDtl) FROM enquiryInvoiceItemTaxDtls),0) " +
                                       " DBCC CHECKIDENT(enquiryInvoiceItemTaxDtls, RESEED, @id) ";
                            break;

                        case FinalBookingData.DelTranTablesE.tempLoading:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idLoading) FROM enquiryLoading),0) " +
                                       " DBCC CHECKIDENT(enquiryLoading, RESEED, @id) ";
                            break;

                        case FinalBookingData.DelTranTablesE.tempLoadingSlip:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idLoadingSlip) FROM enquiryLoadingSlip),0) " +
                                       " DBCC CHECKIDENT(enquiryLoadingSlip, RESEED, @id) ";
                            break;

                        case FinalBookingData.DelTranTablesE.tempLoadingSlipAddress:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idLoadSlipAddr) FROM enquiryLoadingSlipAddress),0) " +
                                       " DBCC CHECKIDENT(enquiryLoadingSlipAddress, RESEED, @id) ";
                            break;

                        case FinalBookingData.DelTranTablesE.tempLoadingSlipDtl:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idLoadSlipDtl) FROM enquiryLoadingSlipDtl),0) " +
                                       " DBCC CHECKIDENT(enquiryLoadingSlipDtl, RESEED, @id) ";
                            break;

                        case FinalBookingData.DelTranTablesE.tempLoadingSlipExt:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idLoadingSlipExt) FROM enquiryLoadingSlipExt),0) " +
                                       " DBCC CHECKIDENT(enquiryLoadingSlipExt, RESEED, @id) ";
                            break;

                        case FinalBookingData.DelTranTablesE.tempLoadingSlipExtHistory:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idConfirmHistory) FROM enquiryLoadingSlipExtHistory),0) " +
                                       " DBCC CHECKIDENT(enquiryLoadingSlipExtHistory, RESEED, @id) ";
                            break;

                        case FinalBookingData.DelTranTablesE.tempLoadingStatusHistory:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idLoadingHistory) FROM enquiryLoadingStatusHistory),0) " +
                                       " DBCC CHECKIDENT(enquiryLoadingStatusHistory, RESEED, @id) ";
                            break;

                        case FinalBookingData.DelTranTablesE.tempWeighingMeasures:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idWeightMeasure) FROM enquiryWeighingMeasures),0) " +
                                       " DBCC CHECKIDENT(enquiryWeighingMeasures, RESEED, @id)";
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
