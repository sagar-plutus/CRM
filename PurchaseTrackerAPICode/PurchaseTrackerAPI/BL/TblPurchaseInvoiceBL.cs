using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System.Linq;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;
using System.Runtime.Serialization.Json;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL

{
    public class TblPurchaseInvoiceBL : ITblPurchaseInvoiceBL
    {
        private readonly ITblPurchaseInvoiceAddrBL _iTblPurchaseInvoiceAddrBL;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly ITblGstCodeDtlsBL _iTblGstCodeDtlsBL;
        private readonly ITblProdGstCodeDtlsBL _iTblProdGstCodeDtlsBL;
        private readonly Idimensionbl _idimensionbl;
        private readonly ITblPurchaseInvoiceDAO _iTblPurchaseInvoiceDAO;
        private readonly ITblPurchaseScheduleSummaryBL _iTblPurchaseScheduleSummaryBL;
        private readonly ITblPurchaseInvoiceInterfacingDtlBL _iTblPurchaseInvoiceInterfacingDtlBL;
        private readonly ITblTaxRatesBL _iTblTaxRatesBL;
        private readonly ITblPurchaseInvoiceItemDetailsBL _iTblPurchaseInvoiceItemDetailsBL;
        private readonly ITblPurchaseInvoiceItemTaxDetailsBL _iTblPurchaseInvoiceItemTaxDetailsBL;
        private readonly ITblPurchaseInvoiceDocumentsBL _iTblPurchaseInvoiceDocumentsBL;
        private readonly ITblPurchaseInvoiceHistoryBL _iTblPurchaseInvoiceHistoryBL;
        private readonly Icommondao _iCommonDAO;
        private readonly IConnectionString _iConnectionString;
        public TblPurchaseInvoiceBL(Idimensionbl idimensionBL,
            ITblPurchaseInvoiceDAO iTblPurchaseInvoiceDAO,
            ITblProdGstCodeDtlsBL iTblProdGstCodeDtlsBL,
            ITblGstCodeDtlsBL iTblGstCodeDtlsBL
            , Icommondao icommondao
           , IConnectionString iConnectionString


                                    , ITblPurchaseInvoiceAddrBL iTblPurchaseInvoiceAddrBL,
                                     ITblPurchaseScheduleSummaryBL iTblPurchaseScheduleSummaryBL,
                                     ITblConfigParamsBL iTblConfigParamsBL
                                    , ITblPurchaseInvoiceInterfacingDtlBL iTblPurchaseInvoiceInterfacingDtlBL
                                    , ITblTaxRatesBL iTblTaxRatesBL
                                    , ITblPurchaseInvoiceItemDetailsBL iTblPurchaseInvoiceItemDetailsBL
                                    , ITblPurchaseInvoiceItemTaxDetailsBL iTblPurchaseInvoiceItemTaxDetailsBL
                                    , ITblPurchaseInvoiceDocumentsBL iTblPurchaseInvoiceDocumentsBL
                                    , ITblPurchaseInvoiceHistoryBL iTblPurchaseInvoiceHistoryBL
                                   )
        {
            _iConnectionString = iConnectionString;
            _iCommonDAO = icommondao;
            _iTblPurchaseInvoiceHistoryBL = iTblPurchaseInvoiceHistoryBL;
            _iTblPurchaseInvoiceDocumentsBL = iTblPurchaseInvoiceDocumentsBL;
            _iTblPurchaseInvoiceItemTaxDetailsBL = iTblPurchaseInvoiceItemTaxDetailsBL;
            _iTblPurchaseInvoiceItemDetailsBL = iTblPurchaseInvoiceItemDetailsBL;
            _iTblTaxRatesBL = iTblTaxRatesBL;
            _iTblPurchaseInvoiceInterfacingDtlBL = iTblPurchaseInvoiceInterfacingDtlBL;
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iTblPurchaseScheduleSummaryBL = iTblPurchaseScheduleSummaryBL;
            _iTblPurchaseInvoiceAddrBL = iTblPurchaseInvoiceAddrBL;
            _iTblGstCodeDtlsBL = iTblGstCodeDtlsBL;
            _iTblProdGstCodeDtlsBL = iTblProdGstCodeDtlsBL;
            _idimensionbl = idimensionBL;
            _iTblPurchaseInvoiceDAO = iTblPurchaseInvoiceDAO;
        }

        #region Selection
        public List<TblPurchaseInvoiceTO> SelectAllTblPurchaseInvoice()
        {
            return _iTblPurchaseInvoiceDAO.SelectAllTblPurchaseInvoice();
        }

        public List<TblPurchaseInvoiceTO> SelectAllTblPurchaseInvoiceList()
        {
            return _iTblPurchaseInvoiceDAO.SelectAllTblPurchaseInvoice();
        }

        public List<TblPurchaseInvoiceTO> SelectAllTblPurchaseInvoiceListAgainstSchedule(Int32 rootPurchaseSchId)
        {
            return _iTblPurchaseInvoiceDAO.SelectAllTblPurchaseInvoiceListAgainstSchedule(rootPurchaseSchId);
        }
        public List<TblPurchaseInvoiceTO> SelectAllTblPurchaseInvoiceListAgainstSchedule(Int32 rootPurchaseSchId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceDAO.SelectAllTblPurchaseInvoiceListAgainstSchedule(rootPurchaseSchId, conn, tran);
        }

        public TblPurchaseInvoiceTO GetPurchaseInvoiceAgainstScheduleWithDtls(Int32 rootPurchaseSchId)
        {
            List<TblPurchaseInvoiceTO> tblPurchaseInvoiceTOList = _iTblPurchaseInvoiceDAO.SelectAllTblPurchaseInvoiceListAgainstSchedule(rootPurchaseSchId);
            if (tblPurchaseInvoiceTOList != null && tblPurchaseInvoiceTOList.Count > 0)
            {
                TblPurchaseInvoiceTO tblPurchaseInvoiceTO = SelectTblPurchaseInvoiceTOWithDetails(tblPurchaseInvoiceTOList[0].IdInvoicePurchase);

                //Saket [2017-11-21]
                String strProdGstCode = String.Join(",", tblPurchaseInvoiceTO.TblPurchaseInvoiceItemDetailsTOList.Select(s => s.ProdGstCodeId.ToString()).ToArray());

                List<TblProdGstCodeDtlsTO> tblProdGstCodeDtlsTOList = _iTblProdGstCodeDtlsBL.SelectTblProdGstCodeDtlsTOList(strProdGstCode);

                for (int p = 0; p < tblPurchaseInvoiceTO.TblPurchaseInvoiceItemDetailsTOList.Count; p++)
                {
                    TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO = tblProdGstCodeDtlsTOList.Where(w => w.IdProdGstCode == tblPurchaseInvoiceTO.TblPurchaseInvoiceItemDetailsTOList[p].ProdGstCodeId).FirstOrDefault();
                    if (tblProdGstCodeDtlsTO != null)
                    {
                        if (tblProdGstCodeDtlsTO.GstCodeId > 0)
                        {
                            tblPurchaseInvoiceTO.TblPurchaseInvoiceItemDetailsTOList[p].GstCodeDtlsTO = _iTblGstCodeDtlsBL.SelectTblGstCodeDtlsTO(tblProdGstCodeDtlsTO.GstCodeId);
                            tblPurchaseInvoiceTO.TblPurchaseInvoiceItemDetailsTOList[p].GstCodeDtlsTO.TaxRatesTOList = _iTblTaxRatesBL.SelectAllTblTaxRatesList(tblProdGstCodeDtlsTO.GstCodeId);
                        }
                    }
                }

                return tblPurchaseInvoiceTO;
            }

            return null;
        }

        public TblPurchaseInvoiceTO SelectTblPurchaseInvoiceTO(Int64 idInvoicePurchase)
        {
            return _iTblPurchaseInvoiceDAO.SelectTblPurchaseInvoice(idInvoicePurchase);
        }

        public TblPurchaseInvoiceTO SelectTblPurchaseInvoiceTO(Int64 idInvoicePurchase, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceDAO.SelectTblPurchaseInvoice(idInvoicePurchase, conn, tran);
        }

        public TblPurchaseInvoiceTO SelectTblPurchaseInvoiceTOWithDetails(Int64 purchaseInvoiceId)
        {
            try
            {
                TblPurchaseInvoiceTO tblPurchaseInvoiceTO = SelectTblPurchaseInvoiceTO(purchaseInvoiceId);
                if (tblPurchaseInvoiceTO != null)
                {
                    tblPurchaseInvoiceTO.TblPurchaseInvoiceAddrTOList = _iTblPurchaseInvoiceAddrBL.SelectAllTblPurchaseInvoiceAddrList(purchaseInvoiceId);
                    List<TblPurchaseInvoiceItemDetailsTO> itemList = _iTblPurchaseInvoiceItemDetailsBL.SelectAllTblPurchaseInvoiceItemDetailsList(purchaseInvoiceId);
                    if (itemList != null)
                    {
                        for (int i = 0; i < itemList.Count; i++)
                        {
                            itemList[i].TblPurchaseInvoiceItemTaxDetailsTOList = _iTblPurchaseInvoiceItemTaxDetailsBL.SelectAllTblPurchaseInvoiceItemTaxDetailsList(itemList[i].IdPurchaseInvoiceItem);

                        }
                        tblPurchaseInvoiceTO.TblPurchaseInvoiceItemDetailsTOList = itemList;
                    }


                    tblPurchaseInvoiceTO.TblPurchaseInvoiceIntefacingDtls = _iTblPurchaseInvoiceInterfacingDtlBL.SelectTblPurchaseInvoiceInterfacingDtlTOAgainstPurInvId(purchaseInvoiceId);
                    tblPurchaseInvoiceTO.TblPurchaseInvoiceDocumentsTOList = _iTblPurchaseInvoiceDocumentsBL.SelecTblPurDocToVerifyWithDocDtlsAgainstPurInvId(purchaseInvoiceId);
                }

                return tblPurchaseInvoiceTO;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }
        }


        public TblPurchaseInvoiceTO SelectTblPurchaseInvoiceTOWithDetails(Int64 purchaseInvoiceId, SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                TblPurchaseInvoiceTO tblPurchaseInvoiceTO = SelectTblPurchaseInvoiceTO(purchaseInvoiceId, conn, tran);
                if (tblPurchaseInvoiceTO != null)
                {
                    tblPurchaseInvoiceTO.TblPurchaseInvoiceAddrTOList = _iTblPurchaseInvoiceAddrBL.SelectAllTblPurchaseInvoiceAddrList(purchaseInvoiceId, conn, tran);
                    List<TblPurchaseInvoiceItemDetailsTO> itemList = _iTblPurchaseInvoiceItemDetailsBL.SelectAllTblPurchaseInvoiceItemDetailsList(purchaseInvoiceId, conn, tran);
                    if (itemList != null && itemList.Count > 0)
                    {
                        for (int i = 0; i < itemList.Count; i++)
                        {
                            itemList[i].TblPurchaseInvoiceItemTaxDetailsTOList = _iTblPurchaseInvoiceItemTaxDetailsBL.SelectAllTblPurchaseInvoiceItemTaxDetailsList(itemList[i].IdPurchaseInvoiceItem, conn, tran);

                        }
                        tblPurchaseInvoiceTO.TblPurchaseInvoiceItemDetailsTOList = itemList;
                    }

                    tblPurchaseInvoiceTO.TblPurchaseInvoiceIntefacingDtls = _iTblPurchaseInvoiceInterfacingDtlBL.SelectTblPurchaseInvoiceInterfacingDtlTOAgainstPurInvId(purchaseInvoiceId, conn, tran);

                }

                return tblPurchaseInvoiceTO;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }
        }


        #endregion

        #region Insertion
        public int InsertTblPurchaseInvoice(TblPurchaseInvoiceTO tblPurchaseInvoiceTO)
        {
            return _iTblPurchaseInvoiceDAO.InsertTblPurchaseInvoice(tblPurchaseInvoiceTO);
        }

        public int InsertTblPurchaseInvoice(TblPurchaseInvoiceTO tblPurchaseInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceDAO.InsertTblPurchaseInvoice(tblPurchaseInvoiceTO, conn, tran);
        }

        public int SelectPurchaseInvoiceByInvoiceIdandFinYear(TblPurchaseInvoiceTO tblPurchaseInvoiceTO,Boolean isFromEdit, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceDAO.SelectPurchaseInvoiceByInvoiceIdandFinYear(tblPurchaseInvoiceTO,isFromEdit,conn, tran);
        }

        public ResultMessage InsertPurchaseInvoice(TblPurchaseInvoiceTO tblPurchaseInvoiceTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            resultMessage.Text = "Not Entered In The Loop";
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                resultMessage = SaveNewPurchaseInvoice(tblPurchaseInvoiceTO, conn, tran);
                if (resultMessage.MessageType == ResultMessageE.Information && resultMessage.Result == 1)
                {
                    tran.Commit();
                    resultMessage.Tag = tblPurchaseInvoiceTO.IdInvoicePurchase;
                    resultMessage.DefaultSuccessBehaviour();
                }
                else
                {
                    tran.Rollback();

                }
                return resultMessage;
            }
            catch (Exception ex)
            {
                if (tran.Connection.State == ConnectionState.Open)
                    tran.Rollback();

                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblInvoice");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }


        }

        public ResultMessage SubmitInvoiceAgainstVehicle(TblPurchaseInvoiceTO tblPurchaseInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {

            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            resultMessage.Text = "Not Entered In The Loop";

            if (tblPurchaseInvoiceTO.PurchaseInvoiceStatusE == Constants.InvoiceStatusE.AUTHORIZED)
            {
                if (tblPurchaseInvoiceTO.PurSchSummaryId > 0)
                {
                    Int32 result = _iTblPurchaseScheduleSummaryBL.UpdateTblPurchaseScheduleSummaryCommercialApproval(tblPurchaseInvoiceTO.PurSchSummaryId, 1, 1, conn, tran);
                    if (result == -1)
                    {
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.DisplayMessage = "Error while updating UpdateTblPurchaseScheduleSummaryCommercialApproval";
                        return resultMessage;
                    }
                }
            }

            if (tblPurchaseInvoiceTO.PurchaseInvoiceStatusE == Constants.InvoiceStatusE.COMMERCIAL_VERIFIED)
            {
                if (tblPurchaseInvoiceTO.PurSchSummaryId > 0)
                {
                    Int32 result = _iTblPurchaseScheduleSummaryBL.UpdateTblPurchaseScheduleSummaryCommercialApproval(tblPurchaseInvoiceTO.PurSchSummaryId, 0, 1, conn, tran);
                    if (result == -1)
                    {
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.DisplayMessage = "Error while updating UpdateTblPurchaseScheduleSummaryCommercialApproval";
                        return resultMessage;
                    }
                }
            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }

        public ResultMessage SaveNewPurchaseInvoice(TblPurchaseInvoiceTO tblPurchaseInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            Int32 checkOnCommerApproval = 1;
            int result = 0;
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            resultMessage.Text = "Not Entered In The Loop";
            //int isCommerciallyVerfied = 0;
            int dbCheckFlag1 = 0;
            Boolean isForBRM = false;
            try
            {


                TblConfigParamsTO isBRMConfigParamTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(StaticStuff.Constants.CP_SCRAP_IS_FOR_BHAGYALAXMI);
                if (isBRMConfigParamTO != null)
                {

                    if (isBRMConfigParamTO.ConfigParamVal == "1")
                    {
                        isForBRM = true;
                    }

                }

                TblPurchaseScheduleSummaryTO existPurchaseScheduleSummaryTO = _iTblPurchaseScheduleSummaryBL.SelectAllEnquiryScheduleSummaryTOByRootScheduleID(tblPurchaseInvoiceTO.PurSchSummaryId, true, conn, tran);
                if (existPurchaseScheduleSummaryTO == null)
                {
                    resultMessage.DefaultBehaviour("Root schedule summary TO not found against id-" + tblPurchaseInvoiceTO.PurSchSummaryId);
                    return resultMessage;
                }
                //isCommerciallyVerfied = existPurchaseScheduleSummaryTO.CommercialVerified;

                dbCheckFlag1 = existPurchaseScheduleSummaryTO.CommercialVerified;

                if (checkOnCommerApproval == 1)
                {
                    dbCheckFlag1 = existPurchaseScheduleSummaryTO.CommercialApproval;
                }
                #region 1. Save the New Invoice

                if (tblPurchaseInvoiceTO.InvoiceToOrgId == 0)
                {
                    TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID, conn, tran);
                    if (tblConfigParamsTO == null)
                    {
                        resultMessage.DefaultBehaviour("Internal Self Organization Not Found in Configuration.");
                        return resultMessage;
                    }
                    Int32 internalOrgId = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);

                    tblPurchaseInvoiceTO.InvoiceToOrgId = internalOrgId;
                }

                DimFinYearTO curFinYearTO = _idimensionbl.GetCurrentFinancialYear(tblPurchaseInvoiceTO.InvoiceDate, conn, tran);
                if (curFinYearTO == null)
                {
                    resultMessage.DefaultBehaviour("Current Fin Year Object Not Found");
                    return resultMessage;
                }
                tblPurchaseInvoiceTO.FinYearId = curFinYearTO.IdFinYear;

                //Added By Gokul [] To check the current Duplicate Invoice number into the SAme financial Year
                tblPurchaseInvoiceTO.InvoiceNo = Constants.removeUnwantedSpaces(tblPurchaseInvoiceTO.InvoiceNo);
                Boolean isFromEdit = false;
                Int32 isDuplicateInvoiceNo = SelectPurchaseInvoiceByInvoiceIdandFinYear(tblPurchaseInvoiceTO, isFromEdit, conn,tran);
                if (isDuplicateInvoiceNo == 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "Duplicate Invoice no not Allowed";
                    return resultMessage;
                }
                //Priyanka [27-03-2019] : Added to verify that invoice is generated against the vehicle or not. 
                List<TblPurchaseInvoiceTO> tempPurchaseInvoiceTOList = SelectAllTblPurchaseInvoiceListAgainstSchedule(tblPurchaseInvoiceTO.PurSchSummaryId);
                if (tempPurchaseInvoiceTOList != null && tempPurchaseInvoiceTOList.Count > 0)
                {
                    resultMessage.DefaultBehaviour("Invoice already generated against this vehicle -" + tblPurchaseInvoiceTO.VehicleNo);
                    return resultMessage;
                }

                result = InsertTblPurchaseInvoice(tblPurchaseInvoiceTO, conn, tran);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error in Insert InvoiceTbl");
                    return resultMessage;
                }
                #endregion

                #region 2. Save the Address Details 
                //if (tblPurchaseInvoiceTO.TblPurchaseInvoiceAddrTOList == null || tblPurchaseInvoiceTO.TblPurchaseInvoiceAddrTOList.Count == 0)
                // {
                //resultMessage.DefaultBehaviour("Error : Invoce Item Address Det List Found Empty Or Null");
                //return resultMessage;
                //}
                for (int i = 0; i < tblPurchaseInvoiceTO.TblPurchaseInvoiceAddrTOList.Count; i++)
                {
                    tblPurchaseInvoiceTO.TblPurchaseInvoiceAddrTOList[i].PurchaseInvoiceId = tblPurchaseInvoiceTO.IdInvoicePurchase;
                    result = _iTblPurchaseInvoiceAddrBL.InsertTblPurchaseInvoiceAddr(tblPurchaseInvoiceTO.TblPurchaseInvoiceAddrTOList[i], conn, tran);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error in Insert InvoiceAddressDetailTbl");
                        return resultMessage;
                    }
                }
                //}
                #endregion

                #region 3. Save the Invoice Item Details
                // if (tblPurchaseInvoiceTO.TblPurchaseInvoiceItemDetailsTOList == null || tblPurchaseInvoiceTO.TblPurchaseInvoiceItemDetailsTOList.Count == 0)
                // {
                //resultMessage.DefaultBehaviour("Error : Invoce Item Det List Found Empty Or Null");
                //return resultMessage;
                //}
                for (int i = 0; i < tblPurchaseInvoiceTO.TblPurchaseInvoiceItemDetailsTOList.Count; i++)
                {
                    TblPurchaseInvoiceItemDetailsTO tblPurchaseInvoiceItemDetailsTO = new TblPurchaseInvoiceItemDetailsTO();
                    tblPurchaseInvoiceItemDetailsTO = tblPurchaseInvoiceTO.TblPurchaseInvoiceItemDetailsTOList[i];
                    tblPurchaseInvoiceItemDetailsTO.PurchaseInvoiceId = tblPurchaseInvoiceTO.IdInvoicePurchase;

                    result = _iTblPurchaseInvoiceItemDetailsBL.InsertTblPurchaseInvoiceItemDetails(tblPurchaseInvoiceItemDetailsTO, conn, tran);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error in Insert InvoiceItemDetailTbl");
                        return resultMessage;
                    }
                    #region 1. Save the Invoice Tax Item Details
                    if (tblPurchaseInvoiceTO.TblPurchaseInvoiceItemDetailsTOList[i].TblPurchaseInvoiceItemTaxDetailsTOList == null
                        || tblPurchaseInvoiceTO.TblPurchaseInvoiceItemDetailsTOList[i].TblPurchaseInvoiceItemTaxDetailsTOList.Count == 0)
                    {
                        //if (tblPurchaseInvoiceTO.PurhcaseInvoiceTypeE == Constants.InvoiceTypeE.REGULAR_TAX_INVOICE
                        //    || tblPurchaseInvoiceTO.PurhcaseInvoiceTypeE == Constants.InvoiceTypeE.SEZ_WITH_DUTY)
                        //{
                        //    resultMessage.DefaultBehaviour("Error : Invoice Item Det Tax List Found Empty Or Null");
                        //    return resultMessage;
                        //}
                    }
                    if (tblPurchaseInvoiceItemDetailsTO.TblPurchaseInvoiceItemTaxDetailsTOList != null || tblPurchaseInvoiceItemDetailsTO.TblPurchaseInvoiceItemTaxDetailsTOList.Count > 0)
                    {

                        for (int j = 0; j < tblPurchaseInvoiceItemDetailsTO.TblPurchaseInvoiceItemTaxDetailsTOList.Count; j++)
                        {
                            tblPurchaseInvoiceItemDetailsTO.TblPurchaseInvoiceItemTaxDetailsTOList[j].PurchaseInvoiceItemId = tblPurchaseInvoiceTO.TblPurchaseInvoiceItemDetailsTOList[i].IdPurchaseInvoiceItem;
                            result = _iTblPurchaseInvoiceItemTaxDetailsBL.InsertTblPurchaseInvoiceItemTaxDetails(tblPurchaseInvoiceItemDetailsTO.TblPurchaseInvoiceItemTaxDetailsTOList[j], conn, tran);
                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error in Insert InvoiceItemTaxDetailTbl");
                                return resultMessage;
                            }
                        }
                    }
                    #endregion
                }
                //}
                #endregion

                #region 4. Save the Documents Details 
                //if (tblPurchaseInvoiceTO.TblPurchaseInvoiceDocumentsTOList == null || tblPurchaseInvoiceTO.TblPurchaseInvoiceDocumentsTOList.Count == 0)
                // {
                //resultMessage.DefaultBehaviour("Error : Invoice Document Details List Found Empty Or Null");
                //return resultMessage;
                //}
                for (int i = 0; i < tblPurchaseInvoiceTO.TblPurchaseInvoiceDocumentsTOList.Count; i++)
                {

                    tblPurchaseInvoiceTO.TblPurchaseInvoiceDocumentsTOList[i].PurchaseInvoiceId = tblPurchaseInvoiceTO.IdInvoicePurchase;
                    tblPurchaseInvoiceTO.TblPurchaseInvoiceDocumentsTOList[i].CreatedBy = tblPurchaseInvoiceTO.CreatedBy;
                    tblPurchaseInvoiceTO.TblPurchaseInvoiceDocumentsTOList[i].CreatedOn = tblPurchaseInvoiceTO.CreatedOn;
                    tblPurchaseInvoiceTO.TblPurchaseInvoiceDocumentsTOList[i].IsActive = 1;
                    result = _iTblPurchaseInvoiceDocumentsBL.InsertTblPurchaseInvoiceDocuments(tblPurchaseInvoiceTO.TblPurchaseInvoiceDocumentsTOList[i], conn, tran);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error in Insert InvoiceDocumentDtlTbl");
                        return resultMessage;
                    }

                }
                //}
                #endregion

                #region 5. Save the Intefacing Details 
                //if (tblPurchaseInvoiceTO.TblPurchaseInvoiceIntefacingDtls == null)
                // {
                //resultMessage.DefaultBehaviour("Error : Invoice ITblPurchaseInvoiceIntefacingDtls Found Empty Or Null");
                //return resultMessage;
                //}
                tblPurchaseInvoiceTO.TblPurchaseInvoiceIntefacingDtls.PurchaseInvoiceId = tblPurchaseInvoiceTO.IdInvoicePurchase;
                result = _iTblPurchaseInvoiceInterfacingDtlBL.InsertTblPurchaseInvoiceInterfacingDtl(tblPurchaseInvoiceTO.TblPurchaseInvoiceIntefacingDtls, conn, tran);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error in Insert PurchaseInvoiceIntefacingDtlsTbl");
                    return resultMessage;
                }
                //}
                #endregion

                #region 6.Update Vehicle 

                resultMessage = SubmitInvoiceAgainstVehicle(tblPurchaseInvoiceTO, conn, tran);
                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                {
                    return resultMessage;
                }

                #endregion

                //Priyanka [20-02-2019] : Update the vehicle status details.
                #region 7.Update the vehicle status details. 
                TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = new TblPurchaseScheduleSummaryTO();
                if (tblPurchaseInvoiceTO.PurSchSummaryId > 0)
                {
                    if (dbCheckFlag1 == 0)
                    {
                        tblPurchaseScheduleSummaryTO = _iTblPurchaseScheduleSummaryBL.SelectAllEnquiryScheduleSummaryTOByRootScheduleID(tblPurchaseInvoiceTO.PurSchSummaryId, true, conn, tran);

                        int dbCheckFlag = 0;

                        dbCheckFlag = tblPurchaseScheduleSummaryTO.CommercialVerified;

                        if (checkOnCommerApproval == 1)
                        {
                            dbCheckFlag = tblPurchaseScheduleSummaryTO.CommercialApproval;
                        }

                        //if (tblPurchaseScheduleSummaryTO.CommercialVerified == 1)
                        if (dbCheckFlag == 1)
                        {

                            if (isForBRM)
                            {
                                //tblPurchaseScheduleSummaryTO.StatusId = Convert.ToInt32(Constants.TranStatusE.SEND_FOR_INSPECTION);
                                //resultMessage = BL.TblPurchaseScheduleSummaryBL.saveData(tblPurchaseScheduleSummaryTO, false, true,  _iCommonDAO.ServerDateTime, conn, tran);

                                tblPurchaseScheduleSummaryTO.StatusId = Convert.ToInt32(Constants.TranStatusE.SEND_FOR_INSPECTION);
                                //Saket [2019-02-25] Saket Added Need to checck
                                //tblPurchaseScheduleSummaryTO.StatusId = tblPurchaseScheduleSummaryTO.PreviousStatusId;

                                resultMessage = _iTblPurchaseScheduleSummaryBL.CheckIfVehicleScheduleAlreadyExits(tblPurchaseScheduleSummaryTO, conn, tran, 0);
                                if (resultMessage != null && resultMessage.MessageType == ResultMessageE.Information)
                                {
                                    tblPurchaseScheduleSummaryTO.CreatedBy = tblPurchaseInvoiceTO.CreatedBy;
                                    tblPurchaseScheduleSummaryTO.CreatedOn = tblPurchaseInvoiceTO.CreatedOn;
                                    tblPurchaseScheduleSummaryTO.UpdatedBy = tblPurchaseInvoiceTO.CreatedBy;
                                    tblPurchaseScheduleSummaryTO.UpdatedOn = tblPurchaseInvoiceTO.CreatedOn;

                                    DateTime serverDate = _iCommonDAO.ServerDateTime;
                                    string padtaApprovalMsg = "";
                                    resultMessage = _iTblPurchaseScheduleSummaryBL.saveData(tblPurchaseScheduleSummaryTO, false, true, serverDate, ref padtaApprovalMsg, conn, tran);

                                    if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                                    {
                                        return resultMessage;
                                    }

                                    ResultMessage temp = _iTblPurchaseScheduleSummaryBL.NotifyStatusChangeAgainstVehicle(tblPurchaseScheduleSummaryTO, true, serverDate);

                                }
                                else //Already outside inspection is done
                                {
                                    resultMessage.DefaultSuccessBehaviour();
                                }
                            }
                            else
                            {
                                //Update commercial approval related flags for generic cycle
                                result = _iTblPurchaseScheduleSummaryBL.UpdateTblPurchaseScheduleSummaryCommercialApproval(tblPurchaseScheduleSummaryTO.ActualRootScheduleId, tblPurchaseScheduleSummaryTO.CommercialApproval, tblPurchaseScheduleSummaryTO.CommercialVerified, conn, tran);
                                if (result == -1)
                                {
                                    resultMessage.DefaultBehaviour("Error in UpdateTblPurchaseScheduleSummaryCommercialApproval()");
                                    return resultMessage;
                                }

                            }

                        }
                    }
                }
                #endregion
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;


            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblInvoice");
                return resultMessage;
            }
            finally
            {

            }

        }


        #endregion

        #region Updation
        public int UpdateTblPurchaseInvoice(TblPurchaseInvoiceTO tblPurchaseInvoiceTO)
        {
            return _iTblPurchaseInvoiceDAO.UpdateTblPurchaseInvoice(tblPurchaseInvoiceTO);
        }

        public int UpdateTblPurchaseInvoice(TblPurchaseInvoiceTO tblPurchaseInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceDAO.UpdateTblPurchaseInvoice(tblPurchaseInvoiceTO, conn, tran);
        }
        public int UpdatePOAndGrrNoForInvoice(TblPurchaseInvoiceTO tblPurchaseInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceDAO.UpdatePOAndGrrNoForInvoice(tblPurchaseInvoiceTO, conn, tran);
        }


        public ResultMessage SaveUpdatedPurchaseInvoice(TblPurchaseInvoiceTO tblPurchaseInvoiceTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMSg = new ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                //Check Duplicate Invoice Number
                
                //Added By Gokul [] To check the current Duplicate Invoice number into the SAme financial Year
                Boolean isFromEdit = true;
                tblPurchaseInvoiceTO.InvoiceNo = Constants.removeUnwantedSpaces(tblPurchaseInvoiceTO.InvoiceNo);
                Int32 isDuplicateInvoiceNo = SelectPurchaseInvoiceByInvoiceIdandFinYear(tblPurchaseInvoiceTO, isFromEdit, conn, tran);
                if (isDuplicateInvoiceNo == 0)
                {
                    resultMSg.DefaultBehaviour();
                    resultMSg.Text = "Duplicate Invoice no not Allowed";
                    return resultMSg;
                }
                resultMSg = UpdatePurchaseInvoice(tblPurchaseInvoiceTO, conn, tran);
                if (resultMSg.MessageType == ResultMessageE.Information)
                {
                    tran.Commit();
                    resultMSg.DefaultSuccessBehaviour();
                }
                else
                {
                    tran.Rollback();
                }
                return resultMSg;
            }
            catch (Exception ex)
            {
                resultMSg.DefaultExceptionBehaviour(ex, "");
                return resultMSg;
            }
            finally
            {
                conn.Close();
            }
        }

        public ResultMessage CreateHistoryTO(TblPurchaseInvoiceTO existingInvoiceTO, TblPurchaseInvoiceTO currentInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            if (existingInvoiceTO == null || existingInvoiceTO.StatusId != currentInvoiceTO.StatusId)
            {
                TblPurchaseInvoiceHistoryTO tblPurchaseInvoiceHistoryTO = new TblPurchaseInvoiceHistoryTO(currentInvoiceTO, _iCommonDAO);

                Int32 result = _iTblPurchaseInvoiceHistoryBL.InsertTblPurchaseInvoiceHistory(tblPurchaseInvoiceHistoryTO, conn, tran);
                if (result != 1)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.DisplayMessage = "Error while inserting status history record";
                    return resultMessage;
                }
            }

            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }


        public ResultMessage UpdatePurchaseInvoice(TblPurchaseInvoiceTO tblPurchaseInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            Int32 checkOnCommerApproval = 1;
            int result = 0;
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            resultMessage.Text = "Not Entered In The Loop";
            String changeIn = string.Empty;
            int dbCheckFlag1 = 0;
            Boolean isForBRM = false;

            try
            {

                TblConfigParamsTO isBRMConfigParamTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(StaticStuff.Constants.CP_SCRAP_IS_FOR_BHAGYALAXMI);
                if (isBRMConfigParamTO != null)
                {

                    if (isBRMConfigParamTO.ConfigParamVal == "1")
                    {
                        isForBRM = true;
                    }

                }

                TblPurchaseScheduleSummaryTO existPurchaseScheduleSummaryTO = _iTblPurchaseScheduleSummaryBL.SelectAllEnquiryScheduleSummaryTOByRootScheduleID(tblPurchaseInvoiceTO.PurSchSummaryId, true, conn, tran);
                if (existPurchaseScheduleSummaryTO == null)
                {
                    resultMessage.DefaultBehaviour("Root schedule summary TO not found against id-" + tblPurchaseInvoiceTO.PurSchSummaryId);
                    return resultMessage;
                }
                dbCheckFlag1 = existPurchaseScheduleSummaryTO.CommercialVerified;

                if (checkOnCommerApproval == 1)
                {
                    dbCheckFlag1 = existPurchaseScheduleSummaryTO.CommercialApproval;
                }

                #region 1. Update the Invoice

                TblPurchaseInvoiceTO existingInvoiceTO = SelectTblPurchaseInvoiceTOWithDetails(tblPurchaseInvoiceTO.IdInvoicePurchase, conn, tran);
                if (existingInvoiceTO == null)
                {
                    resultMessage.DefaultBehaviour("existingInvoiceTO Object Not Found");
                    return resultMessage;
                }


                resultMessage = CreateHistoryTO(existingInvoiceTO, tblPurchaseInvoiceTO, conn, tran);
                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                {
                    return resultMessage;
                }


                result = UpdateTblPurchaseInvoice(tblPurchaseInvoiceTO, conn, tran);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error in UpdateTblPurchaseInvoice");
                    return resultMessage;
                }
                #endregion

                #region 2. Save the Address Details

                for (int i = 0; i < tblPurchaseInvoiceTO.TblPurchaseInvoiceAddrTOList.Count; i++)
                {
                    TblPurchaseInvoiceAddrTO newAddrTO = tblPurchaseInvoiceTO.TblPurchaseInvoiceAddrTOList[i];
                    newAddrTO.PurchaseInvoiceId = tblPurchaseInvoiceTO.IdInvoicePurchase;
                    result = _iTblPurchaseInvoiceAddrBL.UpdateTblPurchaseInvoiceAddr(newAddrTO, conn, tran);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error in Insert UpdateTblPurchaseInvoiceAddr");
                        return resultMessage;
                    }
                }


                #endregion

                #region 3. Save the Invoice Item Details

                #region Delete Previous Tax Details

                // if (tblPurchaseInvoiceTO.TblPurchaseInvoiceItemDetailsTOList.Count > 0)
                //  {
                result = _iTblPurchaseInvoiceItemTaxDetailsBL.DeleteTblPurchaseInvoiceItemTaxDetailsByPurchaseInvoiceId(tblPurchaseInvoiceTO.IdInvoicePurchase, conn, tran);
                if (result == -1)
                {
                    resultMessage.DefaultBehaviour("Error in DeleteTblInvoiceItemTaxDtls");
                    return resultMessage;
                }



                #endregion

                if (tblPurchaseInvoiceTO.TblPurchaseInvoiceItemDetailsTOList == null || tblPurchaseInvoiceTO.TblPurchaseInvoiceItemDetailsTOList.Count == 0)
                {
                    tblPurchaseInvoiceTO.TblPurchaseInvoiceItemDetailsTOList = new List<TblPurchaseInvoiceItemDetailsTO>();
                    //resultMessage.DefaultBehaviour("Error : Invoice Item Det List Found Empty Or Null");
                    //return resultMessage;
                }

                for (int i = 0; i < tblPurchaseInvoiceTO.TblPurchaseInvoiceItemDetailsTOList.Count; i++)
                {
                    TblPurchaseInvoiceItemDetailsTO tblPurchaseInvoiceItemDetailsTO = tblPurchaseInvoiceTO.TblPurchaseInvoiceItemDetailsTOList[i];
                    tblPurchaseInvoiceItemDetailsTO.PurchaseInvoiceId = tblPurchaseInvoiceTO.IdInvoicePurchase;

                    var exInvItemTO = existingInvoiceTO.TblPurchaseInvoiceItemDetailsTOList.Where(ei => ei.IdPurchaseInvoiceItem == tblPurchaseInvoiceTO.TblPurchaseInvoiceItemDetailsTOList[i].IdPurchaseInvoiceItem).FirstOrDefault();
                    if (exInvItemTO == null)
                    {
                        result = _iTblPurchaseInvoiceItemDetailsBL.InsertTblPurchaseInvoiceItemDetails(tblPurchaseInvoiceItemDetailsTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error in Insert InvoiceItemDetailTbl");
                            return resultMessage;
                        }
                    }
                    else
                    {
                        result = _iTblPurchaseInvoiceItemDetailsBL.UpdateTblPurchaseInvoiceItemDetails(tblPurchaseInvoiceItemDetailsTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error in update UpdateTblInvoiceItemDetails");
                            return resultMessage;
                        }
                    }

                    #region 1. Save the Invoice Tax Item Details
                    if (tblPurchaseInvoiceItemDetailsTO.TblPurchaseInvoiceItemTaxDetailsTOList == null
                        || tblPurchaseInvoiceItemDetailsTO.TblPurchaseInvoiceItemTaxDetailsTOList.Count == 0)
                    {
                        if (tblPurchaseInvoiceTO.PurhcaseInvoiceTypeE == Constants.InvoiceTypeE.REGULAR_TAX_INVOICE
                            || tblPurchaseInvoiceTO.PurhcaseInvoiceTypeE == Constants.InvoiceTypeE.SEZ_WITH_DUTY)
                        {
                            resultMessage.DefaultBehaviour("Error : Invoice Item Det Tax List Found Empty Or Null");
                            return resultMessage;
                        }
                    }

                    for (int j = 0; j < tblPurchaseInvoiceItemDetailsTO.TblPurchaseInvoiceItemTaxDetailsTOList.Count; j++)
                    {

                        TblPurchaseInvoiceItemTaxDetailsTO tblPurchaseInvoiceItemTaxDetailsTO = tblPurchaseInvoiceItemDetailsTO.TblPurchaseInvoiceItemTaxDetailsTOList[j];
                        tblPurchaseInvoiceItemTaxDetailsTO.PurchaseInvoiceItemId = tblPurchaseInvoiceItemDetailsTO.IdPurchaseInvoiceItem;
                        result = _iTblPurchaseInvoiceItemTaxDetailsBL.InsertTblPurchaseInvoiceItemTaxDetails(tblPurchaseInvoiceItemTaxDetailsTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error in Insert Tax Details");
                            return resultMessage;
                        }
                    }
                    #endregion

                }

                for (int i = 0; i < existingInvoiceTO.TblPurchaseInvoiceItemDetailsTOList.Count; i++)
                {
                    var exInvItemTO = tblPurchaseInvoiceTO.TblPurchaseInvoiceItemDetailsTOList.Where(ei => ei.IdPurchaseInvoiceItem == existingInvoiceTO.TblPurchaseInvoiceItemDetailsTOList[i].IdPurchaseInvoiceItem).FirstOrDefault();
                    if (exInvItemTO == null)
                    {
                        result = _iTblPurchaseInvoiceItemDetailsBL.DeleteTblPurchaseInvoiceItemDetails(existingInvoiceTO.TblPurchaseInvoiceItemDetailsTOList[i].IdPurchaseInvoiceItem, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error in Delete DeleteTblInvoiceItemDetails");
                            return resultMessage;
                        }
                    }

                }
                // }
                #endregion

                #region 4. Update the Document Details

                for (int i = 0; i < tblPurchaseInvoiceTO.TblPurchaseInvoiceDocumentsTOList.Count; i++)
                {
                    TblPurchaseInvoiceDocumentsTO tblPurchaseInvoiceDocumentsTO = tblPurchaseInvoiceTO.TblPurchaseInvoiceDocumentsTOList[i];
                    result = _iTblPurchaseInvoiceDocumentsBL.UpdateTblPurchaseInvoiceDocuments(tblPurchaseInvoiceDocumentsTO, conn, tran);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error in Insert UpdateTblPurchaseInvoiceDocuments");
                        return resultMessage;
                    }
                }


                #endregion

                #region 5. Update the Intefacing Details 
                if (tblPurchaseInvoiceTO.TblPurchaseInvoiceIntefacingDtls == null || tblPurchaseInvoiceTO.TblPurchaseInvoiceIntefacingDtls.IdPurInvInterfacingDtl == 0)
                {
                    resultMessage.DefaultBehaviour("Error : Invoice ITblPurchaseInvoiceIntefacingDtls Found Empty Or Null");
                    return resultMessage;
                }
                // tblPurchaseInvoiceTO.TblPurchaseInvoiceIntefacingDtls.PurchaseInvoiceId = tblPurchaseInvoiceTO.IdInvoicePurchase;
                result = _iTblPurchaseInvoiceInterfacingDtlBL.UpdateTblPurchaseInvoiceInterfacingDtl(tblPurchaseInvoiceTO.TblPurchaseInvoiceIntefacingDtls, conn, tran);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error in Update PurchaseInvoiceIntefacingDtlsTbl");
                    return resultMessage;
                }

                #endregion

                #region 5.Update Vehicle 

                resultMessage = SubmitInvoiceAgainstVehicle(tblPurchaseInvoiceTO, conn, tran);
                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                {
                    return resultMessage;
                }

                #endregion
                //Priyanka [20-02-2019] : Update the vehicle status details.
                #region 7.Update the vehicle status details. 
                TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = new TblPurchaseScheduleSummaryTO();
                if (tblPurchaseInvoiceTO.PurSchSummaryId > 0)
                {
                    if (dbCheckFlag1 == 0)
                    {
                        tblPurchaseScheduleSummaryTO = _iTblPurchaseScheduleSummaryBL.SelectAllEnquiryScheduleSummaryTOByRootScheduleID(tblPurchaseInvoiceTO.PurSchSummaryId, true, conn, tran);

                        int dbCheckFlag = 0;

                        dbCheckFlag = tblPurchaseScheduleSummaryTO.CommercialVerified;

                        if (checkOnCommerApproval == 1)
                        {
                            dbCheckFlag = tblPurchaseScheduleSummaryTO.CommercialApproval;
                        }
                        // tblPurchaseScheduleSummaryTO.CommercialVerified == 1)
                        if (dbCheckFlag == 1)
                        {

                            if (isForBRM)
                            {
                                // tblPurchaseScheduleSummaryTO.StatusId = Convert.ToInt32(Constants.TranStatusE.SEND_FOR_INSPECTION);
                                //resultMessage = BL.TblPurchaseScheduleSummaryBL.saveData(tblPurchaseScheduleSummaryTO, false, true,  _iCommonDAO.ServerDateTime, conn, tran);

                                tblPurchaseScheduleSummaryTO.StatusId = Convert.ToInt32(Constants.TranStatusE.SEND_FOR_INSPECTION);
                                //Saket [2019-02-25] Saket Added Need to checck
                                //tblPurchaseScheduleSummaryTO.StatusId = tblPurchaseScheduleSummaryTO.PreviousStatusId;

                                resultMessage = _iTblPurchaseScheduleSummaryBL.CheckIfVehicleScheduleAlreadyExits(tblPurchaseScheduleSummaryTO, conn, tran, 0);
                                if (resultMessage != null && resultMessage.MessageType == ResultMessageE.Information)
                                {

                                    tblPurchaseScheduleSummaryTO.CreatedBy = tblPurchaseInvoiceTO.UpdatedBy;
                                    tblPurchaseScheduleSummaryTO.CreatedOn = tblPurchaseInvoiceTO.UpdatedOn;
                                    tblPurchaseScheduleSummaryTO.UpdatedBy = tblPurchaseInvoiceTO.UpdatedBy;
                                    tblPurchaseScheduleSummaryTO.UpdatedOn = tblPurchaseInvoiceTO.UpdatedOn;

                                    DateTime serverDate = _iCommonDAO.ServerDateTime;
                                    string padtaApprovalMsg = "";
                                    resultMessage = _iTblPurchaseScheduleSummaryBL.saveData(tblPurchaseScheduleSummaryTO, false, true, serverDate, ref padtaApprovalMsg, conn, tran);
                                    if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                                    {
                                        return resultMessage;
                                    }

                                    ResultMessage temp = _iTblPurchaseScheduleSummaryBL.NotifyStatusChangeAgainstVehicle(tblPurchaseScheduleSummaryTO, true, serverDate);


                                }
                                else //Already outside inspection is done
                                {
                                    resultMessage.DefaultSuccessBehaviour();
                                }
                            }
                            else
                            {
                                //Update commercial approval related flags for generic cycle
                                result = _iTblPurchaseScheduleSummaryBL.UpdateTblPurchaseScheduleSummaryCommercialApproval(tblPurchaseScheduleSummaryTO.ActualRootScheduleId, tblPurchaseScheduleSummaryTO.CommercialApproval, tblPurchaseScheduleSummaryTO.CommercialVerified, conn, tran);
                                if (result == -1)
                                {
                                    resultMessage.DefaultBehaviour("Error in UpdateTblPurchaseScheduleSummaryCommercialApproval()");
                                    return resultMessage;
                                }
                            }


                        }
                    }
                }
                #endregion
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateInvoice");
                return resultMessage;
            }
            finally
            {

            }
        }

        #endregion

        #region Deletion
        public int DeleteTblPurchaseInvoice(Int64 idInvoicePurchase)
        {
            return _iTblPurchaseInvoiceDAO.DeleteTblPurchaseInvoice(idInvoicePurchase);
        }

        public int DeleteTblPurchaseInvoice(Int64 idInvoicePurchase, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceDAO.DeleteTblPurchaseInvoice(idInvoicePurchase, conn, tran);
        }

        #endregion

        //public ResultMessage CreatePurchaseInvoicePOWithGRR(Int32 rootScheduleId)
        //{
        //    ResultMessage resultMessage = new ResultMessage();
        //    SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
        //    SqlTransaction tran = null;

        //    try
        //    {
        //        conn.Open();
        //        tran = conn.BeginTransaction();

        //        resultMessage = CreatePurchaseInvoicePOWithGRR(rootScheduleId, conn, tran);
        //        if (resultMessage.MessageType != ResultMessageE.Information)
        //        {
        //            return resultMessage;
        //        }

        //        tran.Commit();
        //        resultMessage.DefaultSuccessBehaviour();
        //        return resultMessage;
        //    }
        //    catch (Exception ex)
        //    {
        //        resultMessage.DefaultExceptionBehaviour(ex, "Error in CreatePurchaseInvoicePOWithGRR(Int32 rootScheduleId)");
        //        return resultMessage;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //}
     
    }
}
