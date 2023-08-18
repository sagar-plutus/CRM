using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Z.Expressions;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.IoT.Interfaces;
using PurchaseTrackerAPI.IoT;
using PurchaseTrackerAPI.StaticStuff;
using System.Data;
using System.IO;

namespace PurchaseTrackerAPI.BL
{
    public class ReportBL : IReportBL
    {
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        private readonly ITblPurchaseInvoiceAddrDAO _iTblPurchaseInvoiceAddrDAO;
        private readonly ITblPurchaseInvoiceBL _iPurchaseInvoiceBL;
        private readonly IReportDAO _ireportDAO;
        private readonly ITblPurchaseScheduleSummaryBL _iTblPurchaseScheduleSummaryBL;
        private readonly ITblPurchaseInvoiceItemDetailsDAO _iTblPurchaseInvoiceItemDetailsDAO;
        private readonly ITblPurchaseInvoiceItemTaxDetailsDAO _iTblPurchaseInvoiceItemTaxDetailsDAO;
        private readonly ITblPurchaseVehicleDetailsBL _iTblPurchaseVehicleDetailsBL;
        private readonly ITblPurchaseInvoiceDocumentsBL _iTblPurchaseInvoiceDocumentsBL;
        private readonly ITblPurchaseDocToVerifyBL _iTblPurchaseDocToVerifyBL;
        private readonly IIotCommunication _iIotCommunication;
        private readonly IRunReport _iRunReport;
        private readonly ITblPurchaseUnloadingDtlBL _iTblPurchaseUnloadingDtlBL;
        private readonly IDimReportTemplateBL _iDimReportTemplateBL;
        private readonly ITblPurchaseWeighingStageSummaryBL _ITblPurchaseWeighingStageSummaryBL;
        private readonly IReportDAO _iReportDAO;
        private readonly ITblDashboardEntityHistoryBL _iTblDashboardEntityHistoryBL;
        private readonly ITblPurchaseEnquiryDetailsDAO _iTblPurchaseEnquiryDetailsDAO;
        private readonly ITblPurchaseWeighingStageSummaryBL _iTblPurchaseWeighingStageSummaryBL;
        private readonly Icommondao _iCommonDao;
        ITblPurchaseScheduleSummaryDAO _iTblPurchaseScheduleSummaryDAO;
        ITblGlobalRatePurchaseDAO _iTblGlobalRatePurchaseDAO;
        ITblVariablesBL _iTblVariablesBL;
        ITblProdClassificationBL _iTblProdClassificationBL;
        ITblPurchaseSchTcDtlsBL _iTblPurchaseSchTcDtlsBL;
        private readonly ITblPurchaseInvoiceInterfacingDtlDAO _iTblPurchaseInvoiceInterfacingDtlDAO;
        private readonly ITblAddressBL _iTblAddressBL;
        private readonly Idimensiondao _iDimensiondao;
        private readonly ITblGstCodeDtlsDAO _iTblGstCodeDtlsDAO;
        private readonly ITblGstCodeDtlsBL _iTblGstCodeDtlsBL;
        private readonly ITblTaxRatesBL _iTblTaxRatesBL;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;

        public ReportBL(ITblPurchaseInvoiceBL iPurchaseInvoiceBL, IReportDAO ireportDAO, ITblPurchaseScheduleSummaryBL iTblPurchaseScheduleSummaryBL
                        , ITblPurchaseInvoiceAddrDAO iTblPurchaseInvoiceAddrDAO
                        , ITblConfigParamsDAO iTblConfigParamsDAO, ITblPurchaseEnquiryDetailsDAO iTblPurchaseEnquiryDetailsDAO
                        , ITblPurchaseInvoiceItemDetailsDAO iTblPurchaseInvoiceItemDetailsDAO
                        , ITblPurchaseVehicleDetailsBL iTblPurchaseVehicleDetailsBL
                        , ITblPurchaseInvoiceDocumentsBL iTblPurchaseInvoiceDocumentsBL
                        , ITblPurchaseDocToVerifyBL iTblPurchaseDocToVerifyBL
                        , ITblPurchaseUnloadingDtlBL iTblPurchaseUnloadingDtlBL
                        , IIotCommunication iIotCommunication, IRunReport iRunReport,
                          IDimReportTemplateBL iDimReportTemplateBL
                        , ITblPurchaseInvoiceItemTaxDetailsDAO iTblPurchaseInvoiceItemTaxDetailsDAO
                        , ITblPurchaseWeighingStageSummaryBL ITblPurchaseWeighingStageSummaryBL
                        , IReportDAO iReportDAO,
                           ITblPurchaseWeighingStageSummaryBL iTblPurchaseWeighingStageSummaryBL,
                            Icommondao iCommonDao,
                            ITblPurchaseScheduleSummaryDAO iTblPurchaseScheduleSummaryDAO,
                          ITblDashboardEntityHistoryBL iTblDashboardEntityHistoryBL,
                          ITblGlobalRatePurchaseDAO iTblGlobalRatePurchaseDAO,
                          ITblVariablesBL iTblVariablesBL,
                          ITblProdClassificationBL iTblProdClassificationBL,
                          ITblPurchaseSchTcDtlsBL iTblPurchaseSchTcDtlsBL
                         , ITblPurchaseInvoiceInterfacingDtlDAO iTblPurchaseInvoiceInterfacingDtlDAO
                         , ITblAddressBL iTblAddressBL
                         , Idimensiondao idimensiondao
                         , ITblGstCodeDtlsDAO iTblGstCodeDtlsDAO
                         , ITblGstCodeDtlsBL itblGstCodeDtlsBL
                         , ITblTaxRatesBL iTblTaxRatesBL
                         , ITblConfigParamsBL iTblConfigParamsBL)
        {
            _iTblPurchaseDocToVerifyBL = iTblPurchaseDocToVerifyBL;
            _iTblPurchaseEnquiryDetailsDAO = iTblPurchaseEnquiryDetailsDAO;
            _iTblPurchaseInvoiceDocumentsBL = iTblPurchaseInvoiceDocumentsBL;
            _iTblPurchaseVehicleDetailsBL = iTblPurchaseVehicleDetailsBL;
            _iTblPurchaseInvoiceItemDetailsDAO = iTblPurchaseInvoiceItemDetailsDAO;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
            _iTblPurchaseInvoiceAddrDAO = iTblPurchaseInvoiceAddrDAO;
            _iTblPurchaseScheduleSummaryBL = iTblPurchaseScheduleSummaryBL;
            _iPurchaseInvoiceBL = iPurchaseInvoiceBL;
            _ireportDAO = ireportDAO;
            _iIotCommunication = iIotCommunication;
            _iTblPurchaseUnloadingDtlBL = iTblPurchaseUnloadingDtlBL;
            _iRunReport = iRunReport;
            _iDimReportTemplateBL = iDimReportTemplateBL;
            _iTblPurchaseInvoiceItemTaxDetailsDAO = iTblPurchaseInvoiceItemTaxDetailsDAO;
            _ITblPurchaseWeighingStageSummaryBL = ITblPurchaseWeighingStageSummaryBL;
            _iReportDAO = iReportDAO;
            _iTblDashboardEntityHistoryBL = iTblDashboardEntityHistoryBL;
            _iTblPurchaseWeighingStageSummaryBL = iTblPurchaseWeighingStageSummaryBL;
            _iCommonDao = iCommonDao;
            _iTblPurchaseScheduleSummaryDAO = iTblPurchaseScheduleSummaryDAO;
            _iTblGlobalRatePurchaseDAO = iTblGlobalRatePurchaseDAO;
            _iTblVariablesBL = iTblVariablesBL;
            _iTblProdClassificationBL = iTblProdClassificationBL;
            _iTblPurchaseSchTcDtlsBL = iTblPurchaseSchTcDtlsBL;
            _iTblPurchaseInvoiceInterfacingDtlDAO = iTblPurchaseInvoiceInterfacingDtlDAO;
            _iTblAddressBL = iTblAddressBL;
            _iDimensiondao = idimensiondao;
            _iTblGstCodeDtlsDAO = iTblGstCodeDtlsDAO;
            _iTblGstCodeDtlsBL = itblGstCodeDtlsBL;
            _iTblTaxRatesBL = iTblTaxRatesBL;
            _iTblConfigParamsBL = iTblConfigParamsBL;
        }

        #region Tally Report @KKM 2019-02-05
        //public List<TallyReportTO> GetListOfTallyReport(string fromDate, string toDate, int ConfirmTypeId, int supplierId, int purchaseManagerId, int materialTypeId,Boolean isForNewTallyReport,string vehicleIds)
        //{
        //    Int16 mathroundFact = 3;
        //    DateTime from_Date = DateTime.MinValue;
        //    DateTime to_Date = DateTime.MinValue;
        //    if (Constants.IsDateTime(fromDate))
        //        from_Date = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
        //    if (Constants.IsDateTime(toDate))
        //        to_Date = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));
        //    List<TallyReportTO> list = new List<TallyReportTO>();
        //    List<TallyReportTO> finalList = new List<TallyReportTO>();
        //    //Boolean isForBRM = false;

        //    //TblConfigParamsTO isBRMConfigParamTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(StaticStuff.Constants.CP_SCRAP_IS_FOR_BHAGYALAXMI);
        //    //if (isBRMConfigParamTO != null)
        //    //{

        //    //    if (isBRMConfigParamTO.ConfigParamVal == "1")
        //    //    {
        //    //        isForBRM = true;
        //    //    }

        //    //}

        //    Boolean isTakeNonCommercialQty = false;
        //    TblConfigParamsTO isTakeNonCommerQtyConfigTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_SCRAP_IS_TAKE_NONCOMMERCIAL_QTY_FOR_WEIGHTED_RATE);
        //    if (isTakeNonCommerQtyConfigTO != null)
        //    {
        //        if (isTakeNonCommerQtyConfigTO.ConfigParamVal.ToString() == "1")
        //        {
        //            isTakeNonCommercialQty = true;
        //        }
        //        else
        //        {
        //            isTakeNonCommercialQty = false;
        //        }
        //    }

        //    list = _ireportDAO.SelectTallyReportDetails(from_Date, to_Date, ConfirmTypeId, supplierId, purchaseManagerId, materialTypeId,vehicleIds);

        //    if (list != null && list.Count > 0)
        //    {
        //        if (isForNewTallyReport)
        //        {
        //            finalList = list;
        //        }
        //        else if (!isForNewTallyReport)
        //        {
        //            var summuryGroupList = list.ToLookup(p => p.IdPurchaseScheduleSummary).ToList();

        //            if (summuryGroupList != null)
        //            {
        //                for (int i = 0; i < summuryGroupList.Count; i++)
        //                {

        //                    TallyReportTO tallyReportTO = new TallyReportTO();
        //                    TallyReportTO tallyReportTOForProcessCharge = new TallyReportTO();
        //                    TallyReportTO tallyReportTOForGrandTotal = new TallyReportTO();

        //                    tallyReportTO.SupplierName = summuryGroupList[i].FirstOrDefault().SupplierName + " Total";
        //                    tallyReportTO.GradeQty = Math.Round(summuryGroupList[i].Sum(w => w.GradeQty), mathroundFact);
        //                    tallyReportTO.DustQty = Math.Round(summuryGroupList[i].Sum(w => w.DustQty), mathroundFact);

        //                    //tallyReportTO.GradeRate = Math.Round((summuryGroupList[i].Average(w => w.GradeRate)), mathroundFact);

        //                    tallyReportTO.TotalGradeQty = Math.Round((summuryGroupList[i].Sum(w => w.GradeQty)), mathroundFact);
        //                    tallyReportTO.DisplayTotalGradeQty = String.Format("{0:0.000}", tallyReportTO.TotalGradeQty);

        //                    double totalRate = Math.Round(summuryGroupList[i].Sum(w => w.GradeRate), mathroundFact);
        //                    //tallyReportTO.GradeRate = Math.Round(totalRate / tallyReportTO.GradeQty, mathroundFact);
        //                    tallyReportTO.Total = Math.Round((summuryGroupList[i].Sum(w => w.Total)), mathroundFact);
        //                    //Added by minal for display Total after two decimal point for report 

        //                    tallyReportTO.DisplayTotal = String.Format("{0:0.00}", tallyReportTO.Total);

        //                    if(Startup.IsForBRM)
        //                    {
        //                        tallyReportTO.DisplayTotal = String.Format("{0:0.000}", tallyReportTO.Total);
        //                    }

        //                    if (!Startup.IsForBRM)
        //                    {
        //                        double total = 0, processCharge = 0, grandTotal = 0;
        //                        //Added by minal 03 May 2021 For Add Process charge
        //                        tallyReportTOForProcessCharge.SupplierName = "Process Charge";
        //                        //if ((summuryGroupList[i].FirstOrDefault().IsBoth == 1) && (summuryGroupList[i].FirstOrDefault().COrNCId == (Int32)Constants.ConfirmTypeE.CONFIRM))
        //                        //{
        //                        //    tallyReportTOForProcessCharge.Total = 0.00;
        //                        //}
        //                        //else
        //                        //{
        //                        //    tallyReportTOForProcessCharge.Total = summuryGroupList[i].FirstOrDefault().ProcessChargePerVeh;
        //                        //}


        //                        if ((summuryGroupList[i].FirstOrDefault().IsBoth == 1))
        //                        {
        //                            Boolean isBothRecPresent = false;
        //                            for (int k = 0; k < summuryGroupList.Count; k++)
        //                            {
        //                                if(k != i)
        //                                {
        //                                    var isBothRecPresentList = summuryGroupList[k].Where(a => a.RootScheduleId == summuryGroupList[i].FirstOrDefault().RootScheduleId
        //                            ).ToList();

        //                                    if (isBothRecPresentList != null && isBothRecPresentList.Count > 0)
        //                                    {
        //                                        isBothRecPresent = true;
        //                                    }
        //                                    else
        //                                    {
        //                                        isBothRecPresent = false;
        //                                    }
        //                                }

        //                            }

        //                            if (isBothRecPresent)
        //                            {
        //                                tallyReportTOForProcessCharge.Total = 0.00;

        //                                var enquiryList = summuryGroupList[i].Where(a => a.COrNCId == (Int32)Constants.ConfirmTypeE.NONCONFIRM).ToList();
        //                                if (enquiryList != null && enquiryList.Count > 0)
        //                                {
        //                                    tallyReportTOForProcessCharge.Total = enquiryList.FirstOrDefault().ProcessChargePerVeh;
        //                                }

        //                            }
        //                            else
        //                            {
        //                                var res = summuryGroupList[i].Where(a => a.IsBoth == 1).ToList();
        //                                if (res != null && res.Count > 0)
        //                                {
        //                                    var enquiryList = res.Where(a => a.COrNCId == (Int32)Constants.ConfirmTypeE.NONCONFIRM).ToList();
        //                                    if (enquiryList != null && enquiryList.Count > 0)
        //                                    {
        //                                        tallyReportTOForProcessCharge.Total = enquiryList.FirstOrDefault().ProcessChargePerVeh;
        //                                    }
        //                                    else
        //                                    {
        //                                        var orderList = res.Where(a => a.COrNCId == (Int32)Constants.ConfirmTypeE.CONFIRM).ToList();
        //                                        if (orderList != null && orderList.Count > 0)
        //                                        {
        //                                            tallyReportTOForProcessCharge.Total = orderList.FirstOrDefault().ProcessChargePerVeh;
        //                                        }
        //                                    }
        //                                }

        //                            }

        //                        }
        //                        else
        //                        {
        //                            tallyReportTOForProcessCharge.Total = summuryGroupList[i].FirstOrDefault().ProcessChargePerVeh;
        //                        }



        //                        tallyReportTOForProcessCharge.DisplayTotal = String.Format("{0:0.000}", tallyReportTOForProcessCharge.Total);
                              

        //                        total = tallyReportTO.Total;
        //                        processCharge = tallyReportTOForProcessCharge.Total;
        //                        grandTotal = ((total) - (processCharge));

        //                        tallyReportTOForGrandTotal.SupplierName = "Grand Total";
        //                        tallyReportTOForGrandTotal.Total = (grandTotal);
        //                        tallyReportTOForGrandTotal.DisplayTotal = String.Format("{0:0.00}", tallyReportTOForGrandTotal.Total);
        //                        if (Startup.IsForBRM)
        //                        {
        //                            tallyReportTOForGrandTotal.DisplayTotal = String.Format("{0:0.000}", tallyReportTOForGrandTotal.Total);
        //                        }
        //                        //Added by minal 
        //                    }

        //                    double qty = tallyReportTO.GradeQty;

        //                    if (!isTakeNonCommercialQty)
        //                    {
        //                        qty = tallyReportTO.GradeQty - tallyReportTO.DustQty;
        //                    }

        //                    tallyReportTO.GradeRate = Math.Round(tallyReportTO.Total / qty, mathroundFact);
        //                    //Added by minal for display gradeRate after two decimal point for report 
        //                    tallyReportTO.DisplayGradeRate = String.Format("{0:0.00}", tallyReportTO.GradeRate);
        //                    tallyReportTO.DisplayGradeQty = String.Format("{0:0.000}", tallyReportTO.GradeQty);

        //                    var gruopList = summuryGroupList[i].ToList();
        //                    gruopList.Add(tallyReportTO);
        //                    if (!Startup.IsForBRM)
        //                    {
        //                        gruopList.Add(tallyReportTOForProcessCharge);
        //                        gruopList.Add(tallyReportTOForGrandTotal);
        //                    }
        //                    finalList.AddRange(gruopList);
        //                }
        //            }
        //        }
        //    }
            
        //    return finalList;
        //}


        #endregion
        public List<dynamic> GetVehicleWiseReportForGeneric(TblReportsTO tblReportsTO)
        {
            List<dynamic> vehicleWiseReportTODynamicList = new List<dynamic>();

            Int16 mathroundFact = 3;
            DateTime fromDate = DateTime.MinValue;
            DateTime toDate = DateTime.MinValue;

            List<VehicleWiseReportTO> vehicleWiseReportTOList = new List<VehicleWiseReportTO>();
            List<VehicleWiseReportTO> vehicleWiseReportTOFinalList = new List<VehicleWiseReportTO>();

            
            vehicleWiseReportTOList = _ireportDAO.SelectVehicleWiseReportDetails(tblReportsTO);
            if (vehicleWiseReportTOList != null && vehicleWiseReportTOList.Count > 0)
            {
                var summuryGroupList = vehicleWiseReportTOList.ToLookup(p => p.IdPurchaseScheduleSummary).ToList();
                if (summuryGroupList != null)
                {
                    for (int i = 0; i < summuryGroupList.Count; i++)
                    {
                        double totalAmount = 0, processCharge = 0, grandTotalAmount = 0;

                        VehicleWiseReportTO vehicleWiseReportTOTO = new VehicleWiseReportTO();
                        VehicleWiseReportTO vehicleWiseReportTOForProcessCharge = new VehicleWiseReportTO();
                        VehicleWiseReportTO vehicleWiseReportTOForGrandTotal = new VehicleWiseReportTO();

                        vehicleWiseReportTOTO.Grade = "Total";
                        vehicleWiseReportTOTO.TotalQty = Math.Round((summuryGroupList[i].Sum(w => w.TotalQty)), mathroundFact);
                        vehicleWiseReportTOTO.TotalAmount = Math.Round((summuryGroupList[i].Sum(w => w.TotalAmount)), mathroundFact);
                        vehicleWiseReportTOTO.IsTotalRow = 1;
                        vehicleWiseReportTOTO.DisplayVehicleNo = summuryGroupList[i].FirstOrDefault().DisplayVehicleNo;
                        vehicleWiseReportTOTO.DisplayDate = summuryGroupList[i].FirstOrDefault().DisplayDate;
                        vehicleWiseReportTOTO.DisplaySupplier = summuryGroupList[i].FirstOrDefault().DisplaySupplier;
                        vehicleWiseReportTOTO.DisplayRemark = summuryGroupList[i].FirstOrDefault().DisplayRemark;

                        vehicleWiseReportTOForProcessCharge.Grade = "Process Charge";
                        if ((summuryGroupList[i].FirstOrDefault().IsBoth == 1) && (summuryGroupList[i].FirstOrDefault().COrNCId == (Int32)Constants.ConfirmTypeE.CONFIRM))
                        {
                            vehicleWiseReportTOForProcessCharge.TotalAmount = 0.00;
                        }
                        else
                        {
                            vehicleWiseReportTOForProcessCharge.TotalAmount = summuryGroupList[i].FirstOrDefault().ProcessChargePerVeh;
                        }

                        vehicleWiseReportTOForProcessCharge.IsTotalRow = 1;
                        vehicleWiseReportTOForProcessCharge.DisplayVehicleNo = summuryGroupList[i].FirstOrDefault().DisplayVehicleNo;
                        vehicleWiseReportTOForProcessCharge.DisplayDate = summuryGroupList[i].FirstOrDefault().DisplayDate;
                        vehicleWiseReportTOForProcessCharge.DisplaySupplier = summuryGroupList[i].FirstOrDefault().DisplaySupplier;
                        vehicleWiseReportTOForProcessCharge.DisplayRemark = summuryGroupList[i].FirstOrDefault().DisplayRemark;
                        
                        totalAmount = vehicleWiseReportTOTO.TotalAmount;
                        processCharge = vehicleWiseReportTOForProcessCharge.TotalAmount;
                        grandTotalAmount = ((totalAmount) - (processCharge));

                        vehicleWiseReportTOForGrandTotal.Grade = "Grand Total";
                        vehicleWiseReportTOForGrandTotal.TotalAmount = grandTotalAmount;
                        vehicleWiseReportTOForGrandTotal.IsTotalRow = 1;
                        vehicleWiseReportTOForGrandTotal.DisplayVehicleNo = summuryGroupList[i].FirstOrDefault().DisplayVehicleNo;
                        vehicleWiseReportTOForGrandTotal.DisplayDate = summuryGroupList[i].FirstOrDefault().DisplayDate;
                        vehicleWiseReportTOForGrandTotal.DisplaySupplier = summuryGroupList[i].FirstOrDefault().DisplaySupplier;
                        vehicleWiseReportTOForGrandTotal.DisplayRemark = summuryGroupList[i].FirstOrDefault().DisplayRemark;
                        
                        var gruopList = summuryGroupList[i].ToList();

                        gruopList.Add(vehicleWiseReportTOTO);
                        gruopList.Add(vehicleWiseReportTOForProcessCharge);
                        gruopList.Add(vehicleWiseReportTOForGrandTotal);
                        vehicleWiseReportTOFinalList.AddRange(gruopList);

                    }
                }
            }
            if (vehicleWiseReportTOFinalList != null && vehicleWiseReportTOFinalList.Count > 0)
            {
                foreach (var item in vehicleWiseReportTOFinalList)
                {
                    dynamic vehicleWiseReportDynamicTO = new JObject();

                    vehicleWiseReportDynamicTO["Vehicle Number"] = item.VehicleNumber;
                    vehicleWiseReportDynamicTO["Date"] = item.Date;
                    vehicleWiseReportDynamicTO["Supplier"] = item.Supplier;
                    vehicleWiseReportDynamicTO["Remark"] = item.Remark;
                    vehicleWiseReportDynamicTO["Grade"] = item.Grade;
                    vehicleWiseReportDynamicTO["Total Qty"] = String.Format("{0:0.000}", item.TotalQty);
                    vehicleWiseReportDynamicTO["Rate"] = String.Format("{0:0.00}", item.Rate);
                    vehicleWiseReportDynamicTO["Total Amount"] = String.Format("{0:0.00}", item.TotalAmount);
                    vehicleWiseReportDynamicTO["isTotalRow"] = item.IsTotalRow;
                    vehicleWiseReportDynamicTO["displayVehicleNo"] = item.DisplayVehicleNo;
                    vehicleWiseReportDynamicTO["displayDate"] = item.DisplayDate;
                    vehicleWiseReportDynamicTO["displaySupplier"] = item.DisplaySupplier;
                    vehicleWiseReportDynamicTO["displayRemark"] = item.DisplayRemark;
                    vehicleWiseReportTODynamicList.Add(vehicleWiseReportDynamicTO);
                }
            }

            return vehicleWiseReportTODynamicList;
        }

        public List<dynamic> GetPartyWiseReportForGeneric(TblReportsTO tblReportsTO)
        {
            List<dynamic> partyWiseReportTODynamicList = new List<dynamic>();

            Int16 mathroundFact = 3;
            DateTime fromDate = DateTime.MinValue;
            DateTime toDate = DateTime.MinValue;

            List<PartWiseReportTO> partyWiseReportTOList = new List<PartWiseReportTO>();
            List<PartWiseReportTO> partyWiseReportTOFinalList = new List<PartWiseReportTO>();

            if (tblReportsTO.TblFilterReportTOList1 != null && tblReportsTO.TblFilterReportTOList1.Count > 0)
            {
                TblFilterReportTO fromDateTO = tblReportsTO.TblFilterReportTOList1.Where(a => a.SqlDbTypeValue == 33 && a.SqlParameterName == "FromDate").FirstOrDefault();
                if (fromDateTO != null)
                {
                    fromDate = Convert.ToDateTime(fromDateTO.OutputValue);
                }

                TblFilterReportTO toDateTO = tblReportsTO.TblFilterReportTOList1.Where(a => a.SqlDbTypeValue == 33 && a.SqlParameterName == "ToDate").FirstOrDefault();
                if (toDateTO != null)
                {
                    toDate = Convert.ToDateTime(toDateTO.OutputValue);
                }
            }

            partyWiseReportTOList = _ireportDAO.SelectPartyWiseReportDetails(tblReportsTO);

            if (partyWiseReportTOList != null && partyWiseReportTOList.Count > 0)
            {
                var summuryGroupList = partyWiseReportTOList.ToLookup(p => p.SupplierId).ToList();
                if (summuryGroupList != null)
                {
                    for (int i = 0; i < summuryGroupList.Count; i++)
                    {
                        double processChargePerVeh = 0;

                        PartWiseReportTO partyWiseReportTOTO = new PartWiseReportTO();
                        PartWiseReportTO partyWiseReportTOForProcessCharge = new PartWiseReportTO();
                        PartWiseReportTO partyWiseReportTOForGrandTotal = new PartWiseReportTO();

                        partyWiseReportTOTO.Grade = "Total";
                        partyWiseReportTOTO.TotalQty = Math.Round((summuryGroupList[i].Sum(w => w.TotalQty)), mathroundFact);
                        partyWiseReportTOTO.TotalAmount = Math.Round((summuryGroupList[i].Sum(w => w.TotalAmount)), mathroundFact);
                        partyWiseReportTOTO.IsTotalRow = 1;
                        partyWiseReportTOTO.DisplaySupplier = summuryGroupList[i].FirstOrDefault().DisplaySupplier;

                        Int64 supplierId = summuryGroupList[i].FirstOrDefault().SupplierId;
                        processChargePerVeh = _ireportDAO.GetPartyWiseProcessChargeForReportDetails(fromDate, toDate,supplierId);

                        partyWiseReportTOForProcessCharge.Grade = "Process Charge";
                        partyWiseReportTOForProcessCharge.TotalAmount = processChargePerVeh;
                        partyWiseReportTOForProcessCharge.IsTotalRow = 1;
                        partyWiseReportTOForProcessCharge.DisplaySupplier = summuryGroupList[i].FirstOrDefault().DisplaySupplier;

                        double totalAmount = 0, grandTotalAmount = 0;
                        totalAmount = partyWiseReportTOTO.TotalAmount;                        
                        grandTotalAmount = ((totalAmount) - (processChargePerVeh));

                        partyWiseReportTOForGrandTotal.Grade = "Grand Total";
                        partyWiseReportTOForGrandTotal.TotalAmount = grandTotalAmount;
                        partyWiseReportTOForGrandTotal.IsTotalRow = 1;
                        partyWiseReportTOForGrandTotal.DisplaySupplier = summuryGroupList[i].FirstOrDefault().DisplaySupplier;

                        var gruopList = summuryGroupList[i].ToList();

                        gruopList.Add(partyWiseReportTOTO);
                        gruopList.Add(partyWiseReportTOForProcessCharge);
                        gruopList.Add(partyWiseReportTOForGrandTotal);
                        partyWiseReportTOFinalList.AddRange(gruopList);

                    }
                }
            }
            if (partyWiseReportTOFinalList != null && partyWiseReportTOFinalList.Count > 0)
            {
                foreach (var item in partyWiseReportTOFinalList)
                {
                    dynamic partyWiseReportDynamicTO = new JObject();

                    partyWiseReportDynamicTO["Supplier"] = item.Supplier;
                    partyWiseReportDynamicTO["Grade"] = item.Grade;
                    partyWiseReportDynamicTO["Total Qty"] = String.Format("{0:0.000}", item.TotalQty);
                    partyWiseReportDynamicTO["Total Amount"] = String.Format("{0:0.00}", item.TotalAmount);
                    partyWiseReportDynamicTO["Average Rate"] = String.Format("{0:0.00}", item.AverageRate);
                    partyWiseReportDynamicTO["isTotalRow"] = item.IsTotalRow;
                    partyWiseReportDynamicTO["displaySupplier"] = item.DisplaySupplier;
                    partyWiseReportTODynamicList.Add(partyWiseReportDynamicTO);
                }
            }

            return partyWiseReportTODynamicList;
        }

        public List<dynamic> GetGradeWiseReportForGeneric(TblReportsTO tblReportsTO)
        {
            List<dynamic> gradeWiseReportTODynamicList = new List<dynamic>();

            DataTable resultDT = new DataTable();

            resultDT = _ireportDAO.SelectGradeWiseReportDetails(tblReportsTO);

            if (resultDT != null && resultDT.Rows.Count > 0)
            {
                for (int i = 0; i < resultDT.Rows.Count; i++)
                {
                    dynamic gradeWiseReportDynamicTO = new JObject();

                    DataRow reportDtlsRow = resultDT.Rows[i];

                    gradeWiseReportDynamicTO["Grade"] = reportDtlsRow["Grade"].ToString();
                    gradeWiseReportDynamicTO["Quantity"] = String.Format("{0:0.000}", reportDtlsRow["Quantity"].ToString());
                    gradeWiseReportDynamicTO["AverageRate"] = String.Format("{0:0.00}", reportDtlsRow["AverageRate"].ToString());
                    gradeWiseReportDynamicTO["Amount"] = String.Format("{0:0.00}", reportDtlsRow["Amount"].ToString());                    
                    gradeWiseReportDynamicTO["isTotalRow"] = reportDtlsRow["isTotalRow"].ToString();
                    gradeWiseReportTODynamicList.Add(gradeWiseReportDynamicTO);
                }
            }

            return gradeWiseReportTODynamicList;

        }

        //Added by minal

        public List<PadtaReportTO> GetListOfPadtaReport(string fromDate, string toDate,String purchaseManagerIds)
        {
            Int16 mathroundFact = 3;
            DateTime from_Date = DateTime.MinValue;
            DateTime to_Date = DateTime.MinValue;
            if (Constants.IsDateTime(fromDate))
                from_Date = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
            if (Constants.IsDateTime(toDate))
                to_Date = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));

            List<PadtaReportTO> list = new List<PadtaReportTO>();
            List<PadtaReportTO> finalList = new List<PadtaReportTO>();

            Boolean isTakeNonCommercialQty = false;
            TblConfigParamsTO isTakeNonCommerQtyConfigTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_SCRAP_IS_TAKE_NONCOMMERCIAL_QTY_FOR_WEIGHTED_RATE);
            if (isTakeNonCommerQtyConfigTO != null)
            {
                if (isTakeNonCommerQtyConfigTO.ConfigParamVal.ToString() == "1")
                {
                    isTakeNonCommercialQty = true;
                }
                else
                {
                    isTakeNonCommercialQty = false;
                }
            }

            list = _ireportDAO.SelectPadtaReportDetails(from_Date, to_Date, purchaseManagerIds);
            if (list != null && list.Count > 0)
            {
                var summuryGroupList = list.ToLookup(p => p.IdPurchaseScheduleSummary).ToList();
                if (summuryGroupList != null)
                {
                    for (int i = 0; i < summuryGroupList.Count; i++)
                    {
                        List<string> gradeList = new List<string>();
                        var defualtItem = summuryGroupList[i].FirstOrDefault();
                        defualtItem.TodaysRate = defualtItem.GradeRate;//Math.Round((summuryGroupList[i].Average(w => w.GradeRate)) / 1000, 2);
                        defualtItem.TodaysRate = Math.Round(defualtItem.TodaysRate, mathroundFact) / 1000;
                        foreach (var item in summuryGroupList[i])
                        {
                            var str = "";
                            if (item.Grade != null)
                            {
                                var characters = item.Grade.Split(' ');
                                foreach (var singleWords in characters)
                                {
                                    var initialChar = singleWords.Substring(0, 1);
                                    if (initialChar != "(")
                                        str += singleWords.Substring(0, 1);
                                }
                            }
                            gradeList.Add(str);
                        }

                        double qtyForWeightedRate = 0;

                        var sumforWRT = summuryGroupList[i].Sum(w => w.ProductAomunt);
                        var sumforWY = summuryGroupList[i].Sum(w => (Math.Round(w.ProductRecovery * w.Qty, mathroundFact)));

                        var dustQty = summuryGroupList[i].Sum(w => w.DustQty);
                        defualtItem.DustQty = Math.Round(dustQty, mathroundFact);

                        var toatQuntity = summuryGroupList[i].Sum(w => w.Qty);
                        defualtItem.Qty = Math.Round(toatQuntity, mathroundFact);

                        qtyForWeightedRate = defualtItem.Qty;

                        if (!isTakeNonCommercialQty)
                        {
                            qtyForWeightedRate = defualtItem.Qty - dustQty;
                            qtyForWeightedRate = Math.Round(qtyForWeightedRate, mathroundFact);
                        }

                        defualtItem.Wrt = Math.Round((sumforWRT / qtyForWeightedRate) / 1000, mathroundFact);
                        defualtItem.Wy = Math.Round((sumforWY / toatQuntity), mathroundFact);
                        defualtItem.Type = string.Join(",", gradeList.ToArray());
                        defualtItem.Padta_MT = Math.Round((defualtItem.Padta_MT / defualtItem.Qty) / 1000, mathroundFact);
                        defualtItem.SRate = Math.Round(((defualtItem.Wrt / defualtItem.Wy) * defualtItem.BaseItemRecovery), mathroundFact);
                        finalList.Add(defualtItem);
                    }
                    //For add last row to all row sum
                    PadtaReportTO padtaReportTO = new PadtaReportTO();
                    padtaReportTO.Pm = "TTL & W VALUES";
                    padtaReportTO.Qty = Math.Round(finalList.Sum(w => w.Qty), mathroundFact);

                    Double totalWrt = Math.Round(finalList.Sum(w => (w.Wrt * w.Qty)), mathroundFact);
                    padtaReportTO.Wrt = Math.Round((totalWrt / padtaReportTO.Qty), mathroundFact);
                    //padtaReportTO.Wrt = Math.Round(finalList.Sum(w => w.Wrt), 2);

                    Double totalWy = Math.Round(finalList.Sum(w => (w.Wy * w.Qty)), mathroundFact);
                    padtaReportTO.Wy = Math.Round((totalWy / padtaReportTO.Qty), mathroundFact);

                    Double totalSRate = Math.Round(finalList.Sum(w => (w.SRate * w.Qty)), mathroundFact);
                    padtaReportTO.SRate = Math.Round((totalSRate / padtaReportTO.Qty), mathroundFact);

                    padtaReportTO.Padta_MT = Math.Round(finalList.Sum(w => w.Padta_MT), mathroundFact);
                    padtaReportTO.SRate = Math.Round((finalList.Sum(w => w.SRate)), mathroundFact);
                    finalList.Add(padtaReportTO);
                }
            }
            return finalList;
        }

        //Added by minal For BRM Report
        public List<dynamic> GetListOfGradeNoteSummaryReport(string fromDate, string toDate, int cOrNCId,String purchaseManagerIds)
        {
            if (cOrNCId == (Int32)Constants.ConfirmTypeE.CONFIRM)
            {
                return GetListOfGradeNoteSummaryReportForOrder(fromDate, toDate, cOrNCId, purchaseManagerIds);
            }
            else if (cOrNCId == (Int32)Constants.ConfirmTypeE.NONCONFIRM)
            {
                return GetListOfGradeNoteSummaryReportForEnquiry(fromDate, toDate, cOrNCId, purchaseManagerIds);
            }
            return null;
        }

        public List<TallyDrNoteReportTO> GetListOfTallyDrNoteOrder(string fromDate, string toDate,String purchaseManagerIds)
        {
            Int16 mathroundFact = 0;
            double grandBasicGradeAmount = 0;
            double grandTotal = 0;
            double grandRate = 0;
            Boolean isWithinState = false;

            DateTime from_Date = DateTime.MinValue;
            DateTime to_Date = DateTime.MinValue;

            if (Constants.IsDateTime(fromDate))
                from_Date = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
            if (Constants.IsDateTime(toDate))
                to_Date = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));


            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryBL.getListofShcheduleSummaryForReport(from_Date, to_Date, purchaseManagerIds);

            //dynamic ScheduleTOList = new List<dynamic>();
            List<TallyDrNoteReportTO> tallyDrNoteReportTOList = new List<TallyDrNoteReportTO>();
            if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count > 0)
            {
                //Prajakta[2019-03-21] Added to get only order vehicles report
                tblPurchaseScheduleSummaryTOList = tblPurchaseScheduleSummaryTOList.Where(a => a.COrNcId == (Int32)Constants.ConfirmTypeE.CONFIRM).ToList();
                if (tblPurchaseScheduleSummaryTOList == null || tblPurchaseScheduleSummaryTOList.Count == 0)
                    return null;

                foreach (var ScheduleSummaryTO in tblPurchaseScheduleSummaryTOList)
                {
                    //Prajakta[2019-09-27] Commented to optimize code
                    //if (ScheduleSummaryTO.VehiclePhaseId == (Int32)Constants.PurchaseVehiclePhasesE.CORRECTIONS && ScheduleSummaryTO.StatusId == (Int32)Constants.TranStatusE.UNLOADING_COMPLETED)
                    {
                        double totalAmount = 0;
                        double totalQty = 0;
                        double totalBasicGradeAmount = 0;
                        List<TblPurchaseInvoiceTO> InvoiceTOList = _iPurchaseInvoiceBL.SelectAllTblPurchaseInvoiceListAgainstSchedule(ScheduleSummaryTO.RootScheduleId);

                        if (InvoiceTOList != null && InvoiceTOList.Count > 0)
                        {
                            TblPurchaseInvoiceTO InvoiceTO = InvoiceTOList[0];
                            InvoiceTO.TblPurchaseInvoiceAddrTOList = _iTblPurchaseInvoiceAddrDAO.SelectAllTblPurchaseInvoiceAddr(InvoiceTO.IdInvoicePurchase);
                            InvoiceTO.TblPurchaseInvoiceIntefacingDtls = _iTblPurchaseInvoiceInterfacingDtlDAO.SelectTblPurchaseInvoiceInterfacingDtlTOForReport(InvoiceTO.IdInvoicePurchase);
                            List<TblPurchaseInvoiceItemDetailsTO> purchaseInvoiceItemDetailsTOList = _iTblPurchaseInvoiceItemDetailsDAO.SelectAllTblPurchaseInvoiceItemDetails(InvoiceTO.IdInvoicePurchase);
                            string configParamName = Constants.CP_SCRAP_OTHER_TAXES_FOR_TCS_IN_GRADE_NOTE;
                            TblConfigParamsTO configParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(configParamName);
                            if (configParamsTO != null)
                            {
                                List<TblPurchaseInvoiceItemDetailsTO> purchaseInvoiceItemDetailsTOListTemp = purchaseInvoiceItemDetailsTOList.Where(w => configParamsTO.ConfigParamVal.Contains(w.OtherTaxId.ToString())).ToList();
                                if (purchaseInvoiceItemDetailsTOListTemp != null && purchaseInvoiceItemDetailsTOListTemp.Count > 0)
                                {
                                    foreach (var arr in purchaseInvoiceItemDetailsTOListTemp)
                                    {
                                        if (arr.OtherTaxId == (Int32)Constants.OthrTaxTypeE.TCS)
                                        {
                                            InvoiceTO.TcsAmt = arr.TaxableAmt;
                                        }
                                        if (arr.OtherTaxId == (Int32)Constants.OthrTaxTypeE.OTHER_EXPENCES)
                                        {
                                            InvoiceTO.OtherExpAmt = arr.TaxableAmt;
                                        }
                                        if (arr.OtherTaxId == (Int32)Constants.OthrTaxTypeE.TRANSPORTER_ADVANCE)
                                        {
                                            InvoiceTO.TransportorAdvAmt = arr.TaxableAmt;
                                        }
                                    }
                                }
                            }
                            TblPurchaseInvoiceItemDetailsTO tblPurchaseInvoiceItemDetailsTO = purchaseInvoiceItemDetailsTOList.Where(a => a.OtherTaxId == 0).FirstOrDefault();
                            TblGstCodeDtlsTO tblGstCodeDtlsTO = _iTblGstCodeDtlsBL.SelectTblGstCodeDtlsTO(tblPurchaseInvoiceItemDetailsTO.ProdGstCodeId);
                            tblGstCodeDtlsTO.TaxRatesTOList = _iTblTaxRatesBL.SelectAllTblTaxRatesList(tblGstCodeDtlsTO.IdGstCode);

                            double igstPerc = 0, cgstPerc = 0, sgstPerc = 0;
                            TblTaxRatesTO igstTaxRateTO = tblGstCodeDtlsTO.TaxRatesTOList.Where(a => a.TaxTypeId == Convert.ToInt32(Constants.TaxTypeE.IGST) && a.IsActive == 1).FirstOrDefault();
                            if (igstTaxRateTO != null)
                            {
                                igstPerc = igstTaxRateTO.TaxPct;
                            }
                            TblTaxRatesTO cgstTaxRateTO = tblGstCodeDtlsTO.TaxRatesTOList.Where(a => a.TaxTypeId == Convert.ToInt32(Constants.TaxTypeE.CGST) && a.IsActive == 1).FirstOrDefault();
                            if (cgstTaxRateTO != null)
                            {
                                cgstPerc = cgstTaxRateTO.TaxPct;
                            }
                            TblTaxRatesTO sgstTaxRateTO = tblGstCodeDtlsTO.TaxRatesTOList.Where(a => a.TaxTypeId == Convert.ToInt32(Constants.TaxTypeE.SGST) && a.IsActive == 1).FirstOrDefault();
                            if (sgstTaxRateTO != null)
                            {
                                sgstPerc = sgstTaxRateTO.TaxPct;
                            }

                            //string configParamName = Constants.CP_SCRAP_OTHER_TAXES_FOR_TCS_IN_GRADE_NOTE;
                            //TblConfigParamsTO configParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(configParamName);
                            string configParamName1 = Constants.CP_DEFAULT_MATE_COMP_ORGID;
                            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(configParamName1);
                            List<TblAddressTO> tblAddressTO = _iTblAddressBL.SelectOrgAddressList(Convert.ToInt32(tblConfigParamsTO.ConfigParamVal));

                            if (ScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList != null && ScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList.Count > 0)
                            {
                                foreach (var DetailsTO in ScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList)
                                {
                                    totalQty = totalQty + DetailsTO.Qty;
                                    totalAmount = totalAmount + DetailsTO.Rate;
                                    totalBasicGradeAmount = Math.Round(totalBasicGradeAmount + (DetailsTO.Qty * DetailsTO.Rate), mathroundFact);

                                }
                            }
                            //dynamic ScheduleTO1 = new JObject();
                            TallyDrNoteReportTO tallyDrNoteReportTO = new TallyDrNoteReportTO();
                            tallyDrNoteReportTO.Date = ScheduleSummaryTO.CorretionCompletedOn.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                            tallyDrNoteReportTO.VoucherType = "Debit Note";
                            tallyDrNoteReportTO.OriginalInvoiceNo = InvoiceTO.InvoiceNo;
                            tallyDrNoteReportTO.InvoiceDate = InvoiceTO.InvoiceDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                            tallyDrNoteReportTO.SupplierName = InvoiceTO.TblPurchaseInvoiceAddrTOList[0].BillingPartyName;

                            double totaltx = InvoiceTO.CgstAmt + InvoiceTO.SgstAmt + InvoiceTO.IgstAmt;
                            totaltx = Math.Round(totaltx, mathroundFact);
                            double TotalAmountToBePaidToParty = Math.Round(totalBasicGradeAmount + totaltx + InvoiceTO.TcsAmt, mathroundFact);
                            //Double BasicAmount = Math.Round((InvoiceTO.GrandTotal + ScheduleSummaryTO.FreightAmount) - TotalAmountToBePaidToParty, 2);
                            Double basicAmount = Math.Round((InvoiceTO.GrandTotal - TotalAmountToBePaidToParty) + ScheduleSummaryTO.FreightAmount, mathroundFact);

                            if (basicAmount < 0)
                            {
                                continue;
                            }


                            if (InvoiceTO.TblPurchaseInvoiceAddrTOList != null && InvoiceTO.TblPurchaseInvoiceAddrTOList.Count > 0)
                            {
                                TblPurchaseInvoiceAddrTO invoiceAddrTO = InvoiceTO.TblPurchaseInvoiceAddrTOList.Where(a => a.TxnAddrTypeId == Convert.ToInt32(Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS)).FirstOrDefault();
                                if (invoiceAddrTO != null)
                                {
                                    if (tblAddressTO[0].StateId == invoiceAddrTO.StateId)
                                    {
                                        isWithinState = true;
                                    }
                                    else
                                    {
                                        isWithinState = false;
                                    }
                                }
                            }
                            if (isWithinState)
                            {
                                Double perc = 18; // 18 percentage
                                Double cgstPercAmt = ((perc) * (cgstPerc / 100));
                                Double sgstPercAmt = ((perc) * (sgstPerc / 100));
                                Double cgstInputAmt = 0, sgstInputAmt = 0;

                                cgstInputAmt = Math.Round(((basicAmount * cgstPercAmt) / 100), mathroundFact);
                                sgstInputAmt = Math.Round(((basicAmount * sgstPercAmt) / 100), mathroundFact);

                                Double debitAmountRscsgstamt = basicAmount + cgstInputAmt + sgstInputAmt;
                                tallyDrNoteReportTO.DebitAmountRs = String.Format("{0:0.00}", debitAmountRscsgstamt);
                                tallyDrNoteReportTO.AgainstRef = InvoiceTO.InvoiceNo;
                                tallyDrNoteReportTO.PurchaseLedger = InvoiceTO.TblPurchaseInvoiceIntefacingDtls.PurchaseAcc;
                                tallyDrNoteReportTO.PurchaseLedgerAmountRs = String.Format("{0:0.00}", basicAmount);
                                tallyDrNoteReportTO.CGSTINPUT = InvoiceTO.TblPurchaseInvoiceIntefacingDtls.CGSTINPUT;
                                tallyDrNoteReportTO.CGSTINPUTAmountRs = String.Format("{0:0.00}", cgstInputAmt);
                                tallyDrNoteReportTO.SGSTINPUT = InvoiceTO.TblPurchaseInvoiceIntefacingDtls.SGSTINPUT;
                                tallyDrNoteReportTO.SGSTINPUTAmountRs = String.Format("{0:0.00}", sgstInputAmt);
                                tallyDrNoteReportTO.IGSTINPUT = "";
                                tallyDrNoteReportTO.IGSTINPUTAmountRs = "";
                            }
                            else
                            {

                                Double perc = 18; // 18 percentage
                                Double igstPercAmt = ((perc) * (igstPerc / 100));
                                Double igstInputAmt = 0;

                                igstInputAmt = Math.Round(((basicAmount * igstPercAmt) / 100), mathroundFact);
                                Double debitAmountRsigstamt = basicAmount + igstInputAmt;
                                tallyDrNoteReportTO.DebitAmountRs = String.Format("{0:0.00}", debitAmountRsigstamt);
                                tallyDrNoteReportTO.AgainstRef = InvoiceTO.InvoiceNo;
                                tallyDrNoteReportTO.PurchaseLedger = InvoiceTO.TblPurchaseInvoiceIntefacingDtls.PurchaseAcc;
                                tallyDrNoteReportTO.PurchaseLedgerAmountRs = String.Format("{0:0.00}", basicAmount);
                                tallyDrNoteReportTO.CGSTINPUT = "";
                                tallyDrNoteReportTO.CGSTINPUTAmountRs = "";
                                tallyDrNoteReportTO.SGSTINPUT = "";
                                tallyDrNoteReportTO.SGSTINPUTAmountRs = "";
                                tallyDrNoteReportTO.IGSTINPUT = InvoiceTO.TblPurchaseInvoiceIntefacingDtls.IGSTINPUT;
                                tallyDrNoteReportTO.IGSTINPUTAmountRs = String.Format("{0:0.00}", igstInputAmt);

                            }

                            tallyDrNoteReportTO.Narration = "BEING DEBIT NOTE RAISED FOR MATERIAL GRADE VARIATIONS : " + InvoiceTO.InvoiceNo + " + " + ScheduleSummaryTO.CorretionCompletedOn.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) + " + " + ScheduleSummaryTO.VehicleNo;

                            tallyDrNoteReportTOList.Add(tallyDrNoteReportTO);
                            //ScheduleTOList.Add(ScheduleTO1);

                        }

                    }
                }

                return tallyDrNoteReportTOList;
            }
            return null;
        }

        

        //public List<dynamic> GetListOfTallyCrNoteOrderCS(TblReportsTO tblReportsTO)
        //{
        //    Double conversionFact = 1000;
        //    Int16 mathroundFact = 3;
        //    double grandBasicGradeAmount = 0;
        //    double grandTotal = 0;
        //    double grandRate = 0;
        //    /*DateTime from_Date = DateTime.MinValue;
        //    DateTime to_Date = DateTime.MinValue;
        //    if (Constants.IsDateTime(fromDate))
        //        from_Date = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
        //    if (Constants.IsDateTime(toDate))
        //        to_Date = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));*/

        //    DateTime fromDate = DateTime.MinValue;
        //    DateTime toDate = DateTime.MinValue;

        //    if (tblReportsTO.TblFilterReportTOList1 != null && tblReportsTO.TblFilterReportTOList1.Count > 0)
        //    {
        //        TblFilterReportTO fromDateTO = tblReportsTO.TblFilterReportTOList1.Where(a => a.SqlDbTypeValue == 33 && a.SqlParameterName == "FromDate").FirstOrDefault();
        //        if (fromDateTO != null)
        //        {
        //            fromDate = Convert.ToDateTime(fromDateTO.OutputValue);
        //        }

        //        TblFilterReportTO toDateTO = tblReportsTO.TblFilterReportTOList1.Where(a => a.SqlDbTypeValue == 33 && a.SqlParameterName == "ToDate").FirstOrDefault();
        //        if (fromDateTO != null)
        //        {
        //            toDate = Convert.ToDateTime(toDateTO.OutputValue);
        //        }
        //    }

        //    List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryBL.getListofShcheduleSummaryForReport(fromDate, toDate);

        //    dynamic ScheduleTOList = new List<dynamic>();
        //    if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count > 0)
        //    {
        //        //Prajakta[2019-03-21] Added to get only order vehicles report
        //        tblPurchaseScheduleSummaryTOList = tblPurchaseScheduleSummaryTOList.Where(a => a.COrNcId == (Int32)Constants.ConfirmTypeE.CONFIRM).ToList();
        //        if (tblPurchaseScheduleSummaryTOList == null || tblPurchaseScheduleSummaryTOList.Count == 0)
        //            return null;


        //       // _iTblPurchaseScheduleSummaryBL.GetSameProdItemsCombinedListForReport(tblPurchaseScheduleSummaryTOList);

        //        foreach (var ScheduleSummaryTO in tblPurchaseScheduleSummaryTOList)
        //        {
        //            //Prajakta[2019-09-27] Commented to optimize code
        //            //if (ScheduleSummaryTO.VehiclePhaseId == (Int32)Constants.PurchaseVehiclePhasesE.CORRECTIONS && ScheduleSummaryTO.StatusId == (Int32)Constants.TranStatusE.UNLOADING_COMPLETED)
        //            {
        //                double totalAmount = 0;
        //                double totalQty = 0;
        //                double totalBasicGradeAmount = 0;
        //                List<TblPurchaseInvoiceTO> InvoiceTOList = _iPurchaseInvoiceBL.SelectAllTblPurchaseInvoiceListAgainstSchedule(ScheduleSummaryTO.RootScheduleId);
        //                //List<TblPurchaseInvoiceTO> InvoiceTOList = new List<TblPurchaseInvoiceTO>();
        //                //TblPurchaseInvoiceTO InvoiceTO1 = new TblPurchaseInvoiceTO();
        //                //InvoiceTOList.Add(InvoiceTO1);

        //                if (InvoiceTOList != null && InvoiceTOList.Count > 0)
        //                {
        //                    TblPurchaseInvoiceTO InvoiceTO = InvoiceTOList[0];
        //                    //InvoiceTO.TblPurchaseInvoiceAddrTOList = _iTblPurchaseInvoiceAddrDAO.SelectAllTblPurchaseInvoiceAddr(InvoiceTO.IdInvoicePurchase);
        //                    //InvoiceTO.TblPurchaseInvoiceIntefacingDtls = _iTblPurchaseInvoiceInterfacingDtlDAO.SelectTblPurchaseInvoiceInterfacingDtlTOForReport(InvoiceTO.IdInvoicePurchase);

        //                    string configParamName = Constants.CP_SCRAP_OTHER_TAXES_FOR_TCS_IN_GRADE_NOTE;
        //                    TblConfigParamsTO configParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(configParamName);
        //                    if (configParamsTO != null)
        //                    {
        //                        List<TblPurchaseInvoiceItemDetailsTO> purchaseInvoiceItemDetailsTOList = _iTblPurchaseInvoiceItemDetailsDAO.SelectAllTblPurchaseInvoiceItemDetails(InvoiceTO.IdInvoicePurchase);
        //                        if (purchaseInvoiceItemDetailsTOList != null && purchaseInvoiceItemDetailsTOList.Count > 0)
        //                        {
        //                            List<TblPurchaseInvoiceItemDetailsTO> purchaseInvoiceItemDetailsTOListTemp = purchaseInvoiceItemDetailsTOList.Where(w => configParamsTO.ConfigParamVal.Contains(w.OtherTaxId.ToString())).ToList();
        //                            if (purchaseInvoiceItemDetailsTOListTemp != null && purchaseInvoiceItemDetailsTOListTemp.Count > 0)
        //                            {
        //                                foreach (var arr in purchaseInvoiceItemDetailsTOListTemp)
        //                                {
        //                                    if (arr.OtherTaxId == (Int32)Constants.OthrTaxTypeE.TCS)
        //                                    {
        //                                        InvoiceTO.TcsAmt = arr.TaxableAmt;
        //                                    }
        //                                    if (arr.OtherTaxId == (Int32)Constants.OthrTaxTypeE.OTHER_EXPENCES)
        //                                    {
        //                                        InvoiceTO.OtherExpAmt = arr.TaxableAmt;
        //                                    }
        //                                    if (arr.OtherTaxId == (Int32)Constants.OthrTaxTypeE.TRANSPORTER_ADVANCE)
        //                                    {
        //                                        InvoiceTO.TransportorAdvAmt = arr.TaxableAmt;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }

        //                    if (ScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList != null && ScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList.Count > 0)
        //                    {
        //                        foreach (var DetailsTO in ScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList)
        //                        {
        //                            totalQty = totalQty + DetailsTO.Qty;
        //                            totalAmount = totalAmount + DetailsTO.Rate;
        //                            totalBasicGradeAmount = Math.Round(totalBasicGradeAmount + (DetailsTO.Qty * DetailsTO.Rate), mathroundFact);

        //                        }
        //                    }
        //                    dynamic ScheduleTO1 = new JObject();
        //                    ScheduleTO1.Date = ScheduleSummaryTO.CorretionCompletedOn.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
        //                    ScheduleTO1["Voucher Type"] = "Journal";
        //                    ScheduleTO1["Dr Ledger Name"] = ScheduleSummaryTO.ProdClassDesc;
        //                    if (ScheduleSummaryTO.ProdClassDesc == "Local Scrap")
        //                    {
        //                        ScheduleTO1["Dr Ledger Name"] = "SRP";
        //                    }                            
                            
        //                    double totaltx = InvoiceTO.CgstAmt + InvoiceTO.SgstAmt + InvoiceTO.IgstAmt;
        //                    totaltx = Math.Round(totaltx, mathroundFact);
        //                    double TotalAmountToBePaidToParty = Math.Round(totalBasicGradeAmount + totaltx + InvoiceTO.TcsAmt, mathroundFact);
        //                    //Double BasicAmount = Math.Round((InvoiceTO.GrandTotal + ScheduleSummaryTO.FreightAmount) - TotalAmountToBePaidToParty, 2);
        //                    Double BasicAmount = Math.Round((InvoiceTO.GrandTotal - TotalAmountToBePaidToParty) + ScheduleSummaryTO.FreightAmount, 3);

        //                    if (BasicAmount > 0)
        //                    {
        //                        continue;
        //                    }
        //                    Double absAmount = Math.Abs(BasicAmount/ conversionFact);
        //                    ScheduleTO1["Dr Ledger Amount "] = String.Format("{0:0.000}", absAmount);
        //                    ScheduleTO1["Cr Ledger Name"] = ScheduleSummaryTO.SupplierName;
        //                    ScheduleTO1["Cr Ledger Amount "] = String.Format("{0:0.000}", absAmount);
        //                    ScheduleTO1["Narration : [Supplier Name] + [Sauda Name] + [Vehicle Number]"] = "BEING CREDIT NOTE RAISED FOR :" + ScheduleSummaryTO.SupplierName + " + " + ScheduleSummaryTO.EnqDisplayNo + " + " + ScheduleSummaryTO.VehicleNo;
        //                    //ScheduleTO1["Narration : [Supplier Name] + [Sauda Name] + [Vehicle Number]"] = "CREDIT NOTE :" + "Minal Chaudhari" + " + " + "12345678910" + " + " + "MH 10 SI 1010";

        //                    ScheduleTOList.Add(ScheduleTO1);

        //                }

        //            }
        //        }

        //        return ScheduleTOList;
        //    }
        //    return null;
        //}

        public List<dynamic> GetListOfTallyCrNoteOrderCSForExcel(string vehicleIds, int cOrNcId)
        {
            Int16 mathroundFact = 2;
            double grandBasicGradeAmount = 0;
            double grandTotal = 0;
            double grandRate = 0;
           
            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryBL.getListofShcheduleSummaryForDropbox(vehicleIds, cOrNcId);

            dynamic ScheduleTOList = new List<dynamic>();
            if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count > 0)
            {
                //Prajakta[2019-03-21] Added to get only order vehicles report
                tblPurchaseScheduleSummaryTOList = tblPurchaseScheduleSummaryTOList.Where(a => a.COrNcId == (Int32)Constants.ConfirmTypeE.NONCONFIRM).ToList();
                if (tblPurchaseScheduleSummaryTOList == null || tblPurchaseScheduleSummaryTOList.Count == 0)
                    return null;


                // _iTblPurchaseScheduleSummaryBL.GetSameProdItemsCombinedListForReport(tblPurchaseScheduleSummaryTOList);

                foreach (var ScheduleSummaryTO in tblPurchaseScheduleSummaryTOList)
                {
                    //Prajakta[2019-09-27] Commented to optimize code
                    //if (ScheduleSummaryTO.VehiclePhaseId == (Int32)Constants.PurchaseVehiclePhasesE.CORRECTIONS && ScheduleSummaryTO.StatusId == (Int32)Constants.TranStatusE.UNLOADING_COMPLETED)
                    {
                        double totalAmount = 0;
                        double totalQty = 0;
                        double totalBasicGradeAmount = 0;
                        //List<TblPurchaseInvoiceTO> InvoiceTOList = _iPurchaseInvoiceBL.SelectAllTblPurchaseInvoiceListAgainstSchedule(ScheduleSummaryTO.RootScheduleId);
                        List<TblPurchaseInvoiceTO> InvoiceTOList = new List<TblPurchaseInvoiceTO>();
                        TblPurchaseInvoiceTO InvoiceTO1 = new TblPurchaseInvoiceTO();
                        InvoiceTOList.Add(InvoiceTO1);

                        if (InvoiceTOList != null && InvoiceTOList.Count > 0)
                        {
                            TblPurchaseInvoiceTO InvoiceTO = InvoiceTOList[0];
                            //InvoiceTO.TblPurchaseInvoiceAddrTOList = _iTblPurchaseInvoiceAddrDAO.SelectAllTblPurchaseInvoiceAddr(InvoiceTO.IdInvoicePurchase);
                            //InvoiceTO.TblPurchaseInvoiceIntefacingDtls = _iTblPurchaseInvoiceInterfacingDtlDAO.SelectTblPurchaseInvoiceInterfacingDtlTOForReport(InvoiceTO.IdInvoicePurchase);

                            string configParamName = Constants.CP_SCRAP_OTHER_TAXES_FOR_TCS_IN_GRADE_NOTE;
                            TblConfigParamsTO configParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(configParamName);
                            if (configParamsTO != null)
                            {
                                List<TblPurchaseInvoiceItemDetailsTO> purchaseInvoiceItemDetailsTOList = _iTblPurchaseInvoiceItemDetailsDAO.SelectAllTblPurchaseInvoiceItemDetails(InvoiceTO.IdInvoicePurchase);
                                if (purchaseInvoiceItemDetailsTOList != null && purchaseInvoiceItemDetailsTOList.Count > 0)
                                {
                                    List<TblPurchaseInvoiceItemDetailsTO> purchaseInvoiceItemDetailsTOListTemp = purchaseInvoiceItemDetailsTOList.Where(w => configParamsTO.ConfigParamVal.Contains(w.OtherTaxId.ToString())).ToList();
                                    if (purchaseInvoiceItemDetailsTOListTemp != null && purchaseInvoiceItemDetailsTOListTemp.Count > 0)
                                    {
                                        foreach (var arr in purchaseInvoiceItemDetailsTOListTemp)
                                        {
                                            if (arr.OtherTaxId == (Int32)Constants.OthrTaxTypeE.TCS)
                                            {
                                                InvoiceTO.TcsAmt = arr.TaxableAmt;
                                            }
                                            if (arr.OtherTaxId == (Int32)Constants.OthrTaxTypeE.OTHER_EXPENCES)
                                            {
                                                InvoiceTO.OtherExpAmt = arr.TaxableAmt;
                                            }
                                            if (arr.OtherTaxId == (Int32)Constants.OthrTaxTypeE.TRANSPORTER_ADVANCE)
                                            {
                                                InvoiceTO.TransportorAdvAmt = arr.TaxableAmt;
                                            }
                                        }
                                    }
                                }
                            }

                            if (ScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList != null && ScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList.Count > 0)
                            {
                                foreach (var DetailsTO in ScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList)
                                {
                                    totalQty = totalQty + DetailsTO.Qty;
                                    totalAmount = totalAmount + DetailsTO.Rate;
                                    totalBasicGradeAmount = Math.Round(totalBasicGradeAmount + (DetailsTO.Qty * DetailsTO.Rate), mathroundFact);

                                }
                            }
                            dynamic ScheduleTO1 = new JObject();
                            ScheduleTO1.Date = ScheduleSummaryTO.CorretionCompletedOn.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                            ScheduleTO1["Voucher Type"] = "Journal";
                            ScheduleTO1["Dr Ledger Name"] = ScheduleSummaryTO.ProdClassDesc;

                            double totaltx = InvoiceTO.CgstAmt + InvoiceTO.SgstAmt + InvoiceTO.IgstAmt;
                            totaltx = Math.Round(totaltx, mathroundFact);
                            double TotalAmountToBePaidToParty = Math.Round(totalBasicGradeAmount + totaltx + InvoiceTO.TcsAmt, mathroundFact);
                            //Double BasicAmount = Math.Round((InvoiceTO.GrandTotal + ScheduleSummaryTO.FreightAmount) - TotalAmountToBePaidToParty, 2);
                            Double BasicAmount = Math.Round((InvoiceTO.GrandTotal - TotalAmountToBePaidToParty) + ScheduleSummaryTO.FreightAmount, 3);

                            if (BasicAmount > 0)
                            {
                                continue;
                            }
                            ScheduleTO1["Dr Ledger Amount "] = String.Format("{0:0.000}", BasicAmount);
                            ScheduleTO1["Cr Ledger Name"] = ScheduleSummaryTO.SupplierName;
                            ScheduleTO1["Cr Ledger Amount "] = String.Format("{0:0.000}", BasicAmount);
                            ScheduleTO1["Narration : [Supplier Name] + [Sauda Name] + [Vehicle Number]"] = "BEING CREDIT NOTE RAISED FOR :" + ScheduleSummaryTO.SupplierName + " + " + ScheduleSummaryTO.EnqDisplayNo + " + " + ScheduleSummaryTO.VehicleNo;
                            //ScheduleTO1["Narration : [Supplier Name] + [Sauda Name] + [Vehicle Number]"] = "CREDIT NOTE :" + "Minal Chaudhari" + " + " + "12345678910" + " + " + "MH 10 SI 1010";

                            ScheduleTOList.Add(ScheduleTO1);

                        }

                    }
                }

                return ScheduleTOList;
            }
            return null;
        }


        public List<dynamic> GetListOfGradeNoteSummaryReportForOrder(string fromDate, string toDate, int cOrNCId,String purchaseManagerIds)
        {
            string configParamName = Constants.CP_SCRAP_OTHER_TAXES_FOR_TCS_IN_GRADE_NOTE;
            TblConfigParamsTO configParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(configParamName);

            Int16 mathroundFact = 2;
            double grandBasicGradeAmount = 0;
            double grandTotal = 0;
            double grandRate = 0;
            DateTime from_Date = DateTime.MinValue;
            DateTime to_Date = DateTime.MinValue;
            TblConfigParamsTO Material_Type_Narration_Remark_configParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName("CP_GRADE_NOTES_ORDER_P_MATERIAL_TYPE_NARRATION_REMARK");
            if (Constants.IsDateTime(fromDate))
                from_Date = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
            if (Constants.IsDateTime(toDate))
                to_Date = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));
           
            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryBL.getListofShcheduleSummary(fromDate, toDate, purchaseManagerIds);

            dynamic ScheduleTOList = new List<dynamic>();
            if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count > 0)
            {
                //Prajakta[2019-03-21] Added to get only order vehicles report
                tblPurchaseScheduleSummaryTOList = tblPurchaseScheduleSummaryTOList.Where(a => a.COrNcId == (Int32)Constants.ConfirmTypeE.CONFIRM).ToList();
                if (tblPurchaseScheduleSummaryTOList == null || tblPurchaseScheduleSummaryTOList.Count == 0)
                    return null;


                _iTblPurchaseScheduleSummaryBL.GetSameProdItemsCombinedListForReport(tblPurchaseScheduleSummaryTOList);

                foreach (var ScheduleSummaryTO in tblPurchaseScheduleSummaryTOList)
                {
                    //Prajakta[2019-09-27] Commented to optimize code
                    //if (ScheduleSummaryTO.VehiclePhaseId == (Int32)Constants.PurchaseVehiclePhasesE.CORRECTIONS && ScheduleSummaryTO.StatusId == (Int32)Constants.TranStatusE.UNLOADING_COMPLETED)
                    {
                        double totalAmount = 0;
                        double totalQty = 0;
                        double totalBasicGradeAmount = 0;
                        List<TblPurchaseInvoiceTO> InvoiceTOList = _iPurchaseInvoiceBL.SelectAllTblPurchaseInvoiceListAgainstSchedule(ScheduleSummaryTO.RootScheduleId);

                        if (InvoiceTOList != null && InvoiceTOList.Count > 0)
                        {
                            TblPurchaseInvoiceTO InvoiceTO = InvoiceTOList[0];
                            InvoiceTO.TblPurchaseInvoiceAddrTOList = _iTblPurchaseInvoiceAddrDAO.SelectAllTblPurchaseInvoiceAddr(InvoiceTO.IdInvoicePurchase);

                             if (configParamsTO != null)
                            {
                                List<TblPurchaseInvoiceItemDetailsTO> purchaseInvoiceItemDetailsTOList = _iTblPurchaseInvoiceItemDetailsDAO.SelectAllTblPurchaseInvoiceItemDetails(InvoiceTO.IdInvoicePurchase);
                                if (purchaseInvoiceItemDetailsTOList != null && purchaseInvoiceItemDetailsTOList.Count > 0)
                                {
                                    List<TblPurchaseInvoiceItemDetailsTO> purchaseInvoiceItemDetailsTOListTemp = purchaseInvoiceItemDetailsTOList.Where(w => configParamsTO.ConfigParamVal.Contains(w.OtherTaxId.ToString())).ToList();
                                    if (purchaseInvoiceItemDetailsTOListTemp != null && purchaseInvoiceItemDetailsTOListTemp.Count > 0)
                                    {
                                        foreach (var arr in purchaseInvoiceItemDetailsTOListTemp)
                                        {
                                            if (arr.OtherTaxId == (Int32)Constants.OthrTaxTypeE.TCS)
                                            {
                                                InvoiceTO.TcsAmt = arr.TaxableAmt;
                                            }
                                            if (arr.OtherTaxId == (Int32)Constants.OthrTaxTypeE.OTHER_EXPENCES)
                                            {
                                                InvoiceTO.OtherExpAmt = arr.TaxableAmt;
                                            }
                                            if (arr.OtherTaxId == (Int32)Constants.OthrTaxTypeE.TRANSPORTER_ADVANCE)
                                            {
                                                InvoiceTO.TransportorAdvAmt = arr.TaxableAmt;
                                            }
                                        }
                                    }
                                }
                            }

                            if (ScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList != null && ScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList.Count > 0)
                            {
                                foreach (var DetailsTO in ScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList)
                                {
                                    dynamic ScheduleTO = new JObject();
                                    //Prajakta[2019-06-06] Commented and added
                                    //ScheduleTO.Date = ScheduleSummaryTO.CreatedOn.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                                    ScheduleTO.Date = ScheduleSummaryTO.CorretionCompletedOn.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                                    ScheduleTO.TruckNo = ScheduleSummaryTO.VehicleNo;
                                    if (InvoiceTO.TblPurchaseInvoiceAddrTOList != null && InvoiceTO.TblPurchaseInvoiceAddrTOList.Count > 0)
                                    {
                                        ScheduleTO["Supplier Name"] = InvoiceTO.TblPurchaseInvoiceAddrTOList[0].BillingPartyName;
                                    }
                                    else
                                    {
                                        ScheduleTO["Supplier Name"] = "";
                                    }
                                    ScheduleTO.PM = ScheduleSummaryTO.PurchaseManager;
                                    ScheduleTO.Location = ScheduleSummaryTO.Location;
                                    ScheduleTO.Grade = DetailsTO.ItemName;
                                    ScheduleTO.Qty = DetailsTO.Qty;
                                    totalQty = totalQty + DetailsTO.Qty;
                                    //ScheduleTO.Rate = String.Format("{0:n}", DetailsTO.Rate);
                                    ScheduleTO.Rate = Math.Round(DetailsTO.Rate, mathroundFact);
                                    totalAmount = totalAmount + DetailsTO.Rate;
                                    //ScheduleTO.BasicGradeAmount = String.Format("{0:n}", DetailsTO.Qty * DetailsTO.Rate);
                                    ScheduleTO["Basic Grade Amount"] = Math.Round((DetailsTO.Qty * DetailsTO.Rate), mathroundFact);
                                    totalBasicGradeAmount = Math.Round(totalBasicGradeAmount + (DetailsTO.Qty * DetailsTO.Rate), mathroundFact);
                                    ScheduleTO.CGST = "";
                                    ScheduleTO.SGST = "";
                                    ScheduleTO.IGST = "";
                                    ScheduleTO["Total Taxes"] = "";
                                    ScheduleTO.TCS = "";
                                    ScheduleTO["Total Amount To Be Paid To Party"] = "";
                                    ScheduleTO["Invoice Amount"] = "";
                                    ScheduleTO.Freight = "";
                                    ScheduleTO["Balance Amount Rs"] = "";
                                    ScheduleTO["BillNo And Date"] = "";
                                    ScheduleTO["E-way Bill No"] = "";
                                    ScheduleTO["E_Way Bill Date"] = "";

                                    ScheduleTO["Narration Remark"] = ""; //[2021-10-06] Dhananjay commented ScheduleSummaryTO.Narration;
                                    ScheduleTO["Party Name"] = ScheduleSummaryTO.SupplierName;
                                    //if (ScheduleSummaryTO.COrNcId == (Int32)Constants.ConfirmTypeE.NONCONFIRM)
                                    //{
                                    //    ScheduleTO["Bill Type"] = "Enquiry";
                                    //}
                                    //else if (ScheduleSummaryTO.COrNcId == (Int32)Constants.ConfirmTypeE.CONFIRM)
                                    //{
                                    //    ScheduleTO["Bill Type"] = "Order";
                                    //}

                                    ScheduleTOList.Add(ScheduleTO);
                                }
                            }
                            dynamic ScheduleTO1 = new JObject();
                            //ScheduleTO1.Date = ScheduleSummaryTO.CreatedOn.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                            ScheduleTO1.Date = ScheduleSummaryTO.CorretionCompletedOn.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                            //ScheduleTO1.TruckNo = ScheduleSummaryTO.VehicleNo;
                            ScheduleTO1.TruckNo = ScheduleSummaryTO.VehicleNo + " Total";
                            ScheduleTO1["Supplier Name"] = "";
                            ScheduleTO1.PM = "";
                            ScheduleTO1.Location = "";
                            ScheduleTO1.Grade = "";
                            ScheduleTO1.Qty = totalQty;
                            grandTotal += totalQty;
                            grandRate += Math.Round(totalAmount, mathroundFact);
                            grandBasicGradeAmount += Math.Round(totalBasicGradeAmount, mathroundFact);
                            //ScheduleTO1.Rate = String.Format("{0:n}", totalAmount);
                            ScheduleTO1.Rate = Math.Round(totalAmount, mathroundFact);
                            //ScheduleTO1.BasicGradeAmount = String.Format("{0:n}", totalBasicGradeAmount);
                            ScheduleTO1["Basic Grade Amount"] = Math.Round(totalBasicGradeAmount, mathroundFact);
                            //ScheduleTO1.CGST = String.Format("{0:n}", InvoiceTO.CgstAmt);
                            ScheduleTO1.CGST = Math.Round(InvoiceTO.CgstAmt, mathroundFact);
                            //ScheduleTO1.SGST = String.Format("{0:n}", InvoiceTO.SgstAmt);
                            ScheduleTO1.SGST = Math.Round(InvoiceTO.SgstAmt, mathroundFact);
                            //ScheduleTO1.IGST = String.Format("{0:n}", InvoiceTO.IgstAmt);
                            ScheduleTO1.IGST = Math.Round(InvoiceTO.IgstAmt, mathroundFact);

                            //ScheduleTO1.TotalTaxes = String.Format("{0:n}", InvoiceTO.CgstAmt + InvoiceTO.SgstAmt + InvoiceTO.IgstAmt);
                            double totaltx = InvoiceTO.CgstAmt + InvoiceTO.SgstAmt + InvoiceTO.IgstAmt;
                            totaltx = Math.Round(totaltx, mathroundFact);
                            ScheduleTO1["Total Taxes"] = totaltx;

                            //ScheduleTO1.TCS = String.Format("{0:n}", InvoiceTO.TcsAmt);
                            ScheduleTO1.TCS = Math.Round(InvoiceTO.TcsAmt, mathroundFact);

                            //ScheduleTO1.TotalAmountToBePaidToParty = String.Format("{0:n}", totalBasicGradeAmount + totaltx + InvoiceTO.TcsAmt);
                            ScheduleTO1["Total Amount To Be Paid To Party"] = Math.Round(totalBasicGradeAmount + totaltx + InvoiceTO.TcsAmt, mathroundFact);

                            double TotalAmountToBePaidToParty = Math.Round(totalBasicGradeAmount + totaltx + InvoiceTO.TcsAmt, mathroundFact);
                            //ScheduleTO1.InvoiceAmount = String.Format("{0:n}", InvoiceTO.GrandTotal);
                            ScheduleTO1["Invoice Amount"] = Math.Round(InvoiceTO.GrandTotal, mathroundFact);
                            ScheduleTO1.Freight = Math.Round(ScheduleSummaryTO.FreightAmount, mathroundFact);
                            //ScheduleTO1.BalanceAmountRs = String.Format("{0:n}", InvoiceTO.GrandTotal - TotalAmountToBePaidToParty);
                            Double BasicAmount = Math.Round((InvoiceTO.GrandTotal - TotalAmountToBePaidToParty) + ScheduleSummaryTO.FreightAmount, mathroundFact);

                            //ScheduleTO1["Balance Amount Rs"] = Math.Round((InvoiceTO.GrandTotal + ScheduleSummaryTO.FreightAmount) - TotalAmountToBePaidToParty, 2);
                            ScheduleTO1["Balance Amount Rs"] = BasicAmount;
                            ScheduleTO1["BillNo And Date"] = InvoiceTO.InvoiceNo + "/" + InvoiceTO.InvoiceDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                            ScheduleTO1["E-way Bill No"] = InvoiceTO.ElectronicRefNo;
                            ScheduleTO1["E_Way Bill Date"] = InvoiceTO.EwayBillDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                            //ScheduleTO1.Freight = String.Format("{0:n}", InvoiceTO.FreightAmt);
                            

                            ScheduleTO1["Narration Remark"] = ScheduleSummaryTO.Narration;

                            if (Material_Type_Narration_Remark_configParamsTO != null)
                            {
                                var MaterailType = Material_Type_Narration_Remark_configParamsTO.ConfigParamVal.Split(",");
                                if (MaterailType.Contains(ScheduleSummaryTO.ProdClassId.ToString()))
                                {
                                    if (BasicAmount < 0)
                                        ScheduleTO1["Narration Remark"] = "NC";
                                    else
                                        ScheduleTO1["Narration Remark"] = "Debit Note";
                                }
                            }

                            for (int i = 0; i < ScheduleTOList.Count; i++)
                            {
                                if(ScheduleTOList[i]["Narration Remark"] == "")
                                {
                                    ScheduleTOList[i]["Narration Remark"] = ScheduleTO1["Narration Remark"];
                                }
                            }


                            ScheduleTO1["Party Name"] = ScheduleSummaryTO.SupplierName;
                            /* if (ScheduleSummaryTO.COrNcId == (Int32)Constants.ConfirmTypeE.NONCONFIRM)
                             {
                                 ScheduleTO1["Bill Type"] = "Enquiry";
                             }
                             else if (ScheduleSummaryTO.COrNcId == (Int32)Constants.ConfirmTypeE.CONFIRM)
                             {
                                 ScheduleTO1["Bill Type"] = "Order";
                             }*/


                            ScheduleTOList.Add(ScheduleTO1);
                            // dynamic ScheduleTO3 = new JObject();
                            // ScheduleTOList.Add(ScheduleTO3);
                        }

                    }
                }

                dynamic ScheduleTO2 = new JObject();
                ScheduleTO2.Date = "Grand total";
                ScheduleTO2.TruckNo = "";
                //ScheduleTO2.TruckNo = "";
                ScheduleTO2["Supplier Name"] = "";
                ScheduleTO2.PM = "";
                ScheduleTO2.Location = "";
                ScheduleTO2.Grade = "";
                ScheduleTO2.Qty = Math.Round(grandTotal, mathroundFact);
                // grandTotal = +totalQty;
                // grandRate = +Math.Round(totalAmount, mathroundFact);
                // grandBasicGradeAmount = +Math.Round(totalBasicGradeAmount, mathroundFact);
                //ScheduleTO1.Rate = String.Format("{0:n}", totalAmount);
                ScheduleTO2.Rate = Math.Round(grandRate, mathroundFact);
                //ScheduleTO1.BasicGradeAmount = String.Format("{0:n}", grandBasicGradeAmount);
                ScheduleTO2["Basic Grade Amount"] = Math.Round(grandBasicGradeAmount, mathroundFact);
                //ScheduleTO1.CGST = String.Format("{0:n}", InvoiceTO.CgstAmt);
                ScheduleTO2.CGST = "";
                //ScheduleTO1.SGST = String.Format("{0:n}", InvoiceTO.SgstAmt);
                ScheduleTO2.SGST = "";
                //ScheduleTO1.IGST = String.Format("{0:n}", InvoiceTO.IgstAmt);
                ScheduleTO2.IGST = "";

                //ScheduleTO1.TotalTaxes = String.Format("{0:n}", InvoiceTO.CgstAmt + InvoiceTO.SgstAmt + InvoiceTO.IgstAmt);
                ScheduleTO2["Total Taxes"] = "";

                //ScheduleTO1.TCS = String.Format("{0:n}", InvoiceTO.TcsAmt);
                ScheduleTO2.TCS = "";

                //ScheduleTO1.TotalAmountToBePaidToParty = String.Format("{0:n}", totalBasicGradeAmount + totaltx + InvoiceTO.TcsAmt);
                ScheduleTO2["Total Amount To Be Paid To Party"] = "";

                // double TotalAmountToBePaidToParty = Math.Round(totalBasicGradeAmount + totaltx + InvoiceTO.TcsAmt, mathroundFact);
                //ScheduleTO1.InvoiceAmount = String.Format("{0:n}", InvoiceTO.GrandTotal);
                ScheduleTO2["Invoice Amount"] = "";
                ScheduleTO2.Freight = "";
                //ScheduleTO1.BalanceAmountRs = String.Format("{0:n}", InvoiceTO.GrandTotal - TotalAmountToBePaidToParty);
                ScheduleTO2["Balance Amount Rs"] = "";

                ScheduleTO2["BillNo And Date"] = "";
                ScheduleTO2["E-way Bill No"] = "";
                ScheduleTO2["E_Way Bill Date"] = "";
                //ScheduleTO1.Freight = String.Format("{0:n}", InvoiceTO.FreightAmt);
                

                ScheduleTO2["Naration Remark"] = "";

                ScheduleTO2["Party Name"] = "";              


                ScheduleTOList.Add(ScheduleTO2);


                return ScheduleTOList;
            }
            return null;

            return null;
        }


        public List<dynamic> GetListOfGradeNoteSummaryReportForEnquiry(string fromDate, string toDate,int cOrNCId,String purchaseManagerIds)
        {
            Int16 mathroundFact = 2;
            double grandBasicGradeAmount = 0;
            double grandTotal = 0;
            double grandRate = 0;
            double grandTotalFreight = 0;
            double grandTotalBalanceAmt = 0;
            DateTime from_Date = DateTime.MinValue;
            DateTime to_Date = DateTime.MinValue;
            if (Constants.IsDateTime(fromDate))
                from_Date = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
            if (Constants.IsDateTime(toDate))
                to_Date = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));
            
            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryBL.getListofShcheduleSummary(fromDate, toDate, purchaseManagerIds);

            dynamic ScheduleTOList = new List<dynamic>();
            if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count > 0)
            {
                //Minal 26 March 2021 Added to get only enquiry vehicles report
                tblPurchaseScheduleSummaryTOList = tblPurchaseScheduleSummaryTOList.Where(a => a.COrNcId == (Int32)Constants.ConfirmTypeE.NONCONFIRM).ToList();
                if (tblPurchaseScheduleSummaryTOList == null || tblPurchaseScheduleSummaryTOList.Count == 0)
                    return null;

                _iTblPurchaseScheduleSummaryBL.GetSameProdItemsCombinedListForReport(tblPurchaseScheduleSummaryTOList);

                foreach (var ScheduleSummaryTO in tblPurchaseScheduleSummaryTOList)
                {
                    //Prajakta[2019-09-27] Commented to optimize code
                    //if (ScheduleSummaryTO.VehiclePhaseId == (Int32)Constants.PurchaseVehiclePhasesE.CORRECTIONS && ScheduleSummaryTO.StatusId == (Int32)Constants.TranStatusE.UNLOADING_COMPLETED)
                    {
                        double totalAmount = 0;
                        double totalQty = 0;
                        double totalBasicGradeAmount = 0;
                        

                            if (ScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList != null && ScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList.Count > 0)
                            {
                                foreach (var DetailsTO in ScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList)
                                {
                                    dynamic ScheduleTO = new JObject();
                                    ScheduleTO.Date = ScheduleSummaryTO.CorretionCompletedOn.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                                    ScheduleTO.TruckNo = ScheduleSummaryTO.VehicleNo;
                                    ScheduleTO["Supplier Name"] = ScheduleSummaryTO.SupplierName;                                    
                                    ScheduleTO.PM = ScheduleSummaryTO.PurchaseManager;
                                    ScheduleTO.Location = ScheduleSummaryTO.Location;
                                    ScheduleTO.Grade = DetailsTO.ItemName; 
                                    ScheduleTO.Qty = String.Format("{0:0.000}", DetailsTO.Qty);
                                    totalQty = totalQty + DetailsTO.Qty;                                   
                                    ScheduleTO.Rate = Math.Round(DetailsTO.Rate, 3);
                                    totalAmount = totalAmount + DetailsTO.Rate;
                                    ScheduleTO["Basic Grade Amount"] = Math.Round((DetailsTO.Qty * DetailsTO.Rate), mathroundFact);
                                    totalBasicGradeAmount = Math.Round(totalBasicGradeAmount + (DetailsTO.Qty * DetailsTO.Rate), mathroundFact);
                                    ScheduleTO["Total Amount To Be Paid To Party"] = "";
                                    ScheduleTO.Freight = "";
                                    //double TotalAmountToBePaidToParty = Math.Round(totalBasicGradeAmount, mathroundFact);
                                    //Double BasicAmount = Math.Round((TotalAmountToBePaidToParty - ScheduleSummaryTO.FreightAmount), mathroundFact);
                                    ScheduleTO["Balance Amount Rs"] = "";
                                    ScheduleTO["Party Name"] = ScheduleSummaryTO.SupplierName;
                                    
                                    ScheduleTOList.Add(ScheduleTO);
                                }
                            }
                            dynamic ScheduleTO1 = new JObject();
                            ScheduleTO1.Date = ScheduleSummaryTO.CorretionCompletedOn.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                            ScheduleTO1.TruckNo = ScheduleSummaryTO.VehicleNo;
                            ScheduleTO1["Supplier Name"] = ScheduleSummaryTO.SupplierName;
                            ScheduleTO1.PM = ScheduleSummaryTO.PurchaseManager;
                            ScheduleTO1.Location = ScheduleSummaryTO.Location;
                            ScheduleTO1.Grade = " Total ";
                            ScheduleTO1.Qty = Math.Round(totalQty,3);
                            grandTotal += totalQty;
                            grandRate += Math.Round(totalAmount, mathroundFact);
                            grandBasicGradeAmount += Math.Round(totalBasicGradeAmount, mathroundFact);
                            ScheduleTO1.Rate = Math.Round(totalAmount, 3);
                            //ScheduleTO1.BasicGradeAmount = String.Format("{0:n}", totalBasicGradeAmount);
                            ScheduleTO1["Basic Grade Amount"] = Math.Round(totalBasicGradeAmount, mathroundFact);
                            ScheduleTO1["Total Amount To Be Paid To Party"] = Math.Round(totalBasicGradeAmount, mathroundFact);
                            ScheduleTO1.Freight = Math.Round(ScheduleSummaryTO.FreightAmount, mathroundFact);

                            grandTotalFreight += ScheduleSummaryTO.FreightAmount;
                            Double basicAmount1 = Math.Round((totalBasicGradeAmount - ScheduleSummaryTO.FreightAmount), mathroundFact);
                            grandTotalBalanceAmt += basicAmount1;
                            ScheduleTO1["Balance Amount Rs"] = basicAmount1;
                            ScheduleTO1["Party Name"] = ScheduleSummaryTO.SupplierName;
                            ScheduleTOList.Add(ScheduleTO1);                            
                        }

                    }
                }

                dynamic ScheduleTO2 = new JObject();
                ScheduleTO2.Date = "Grand total";
                ScheduleTO2.TruckNo = "";
                ScheduleTO2.TruckNo = "";
                ScheduleTO2["Supplier Name"] = "";
                ScheduleTO2.PM = "";
                ScheduleTO2.Location = "";
                ScheduleTO2.Grade = "";
                ScheduleTO2.Qty = Math.Round(grandTotal,3);                
                ScheduleTO2.Rate = Math.Round(grandRate, 3);                
                ScheduleTO2["Basic Grade Amount"] = Math.Round(grandBasicGradeAmount, mathroundFact);
                ScheduleTO2["Total Amount To Be Paid To Party"] = Math.Round(grandBasicGradeAmount, mathroundFact);
                ScheduleTO2.Freight = Math.Round(grandTotalFreight,2);
                ScheduleTO2["Balance Amount Rs"] = Math.Round(grandTotalBalanceAmt,2);
                ScheduleTO2["Party Name"] = "";

                ScheduleTOList.Add(ScheduleTO2);

           return ScheduleTOList;          


        }


        //public List<dynamic> GetListOfGradeNoteSummaryReport(string fromDate, string toDate,int cOrNCId)
        //{
        //    Int16 mathroundFact = 3;
        //    double grandBasicGradeAmount = 0;
        //    double grandTotal = 0;
        //    double grandRate = 0;
        //    DateTime from_Date = DateTime.MinValue;
        //    DateTime to_Date = DateTime.MinValue;
        //    if (Constants.IsDateTime(fromDate))
        //        from_Date = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
        //    if (Constants.IsDateTime(toDate))
        //        to_Date = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));
        //    List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryBL.getListofShcheduleSummary(fromDate, toDate);

        //    dynamic ScheduleTOList = new List<dynamic>();
        //    if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count > 0)
        //    {
        //        //Prajakta[2019-03-21] Added to get only order vehicles report
        //        tblPurchaseScheduleSummaryTOList = tblPurchaseScheduleSummaryTOList.Where(a => a.COrNcId == (Int32)Constants.ConfirmTypeE.CONFIRM).ToList();
        //        if (tblPurchaseScheduleSummaryTOList == null || tblPurchaseScheduleSummaryTOList.Count == 0)
        //            return null;


        //        _iTblPurchaseScheduleSummaryBL.GetSameProdItemsCombinedListForReport(tblPurchaseScheduleSummaryTOList);

        //        foreach (var ScheduleSummaryTO in tblPurchaseScheduleSummaryTOList)
        //        {
        //            //Prajakta[2019-09-27] Commented to optimize code
        //            //if (ScheduleSummaryTO.VehiclePhaseId == (Int32)Constants.PurchaseVehiclePhasesE.CORRECTIONS && ScheduleSummaryTO.StatusId == (Int32)Constants.TranStatusE.UNLOADING_COMPLETED)
        //            {
        //                double totalAmount = 0;
        //                double totalQty = 0;
        //                double totalBasicGradeAmount = 0;
        //                List<TblPurchaseInvoiceTO> InvoiceTOList = _iPurchaseInvoiceBL.SelectAllTblPurchaseInvoiceListAgainstSchedule(ScheduleSummaryTO.RootScheduleId);

        //                if (InvoiceTOList != null && InvoiceTOList.Count > 0)
        //                {
        //                    TblPurchaseInvoiceTO InvoiceTO = InvoiceTOList[0];
        //                    InvoiceTO.TblPurchaseInvoiceAddrTOList = _iTblPurchaseInvoiceAddrDAO.SelectAllTblPurchaseInvoiceAddr(InvoiceTO.IdInvoicePurchase);

        //                    string configParamName = Constants.CP_SCRAP_OTHER_TAXES_FOR_TCS_IN_GRADE_NOTE;
        //                    TblConfigParamsTO configParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(configParamName);
        //                    if (configParamsTO != null)
        //                    {
        //                        List<TblPurchaseInvoiceItemDetailsTO> purchaseInvoiceItemDetailsTOList = _iTblPurchaseInvoiceItemDetailsDAO.SelectAllTblPurchaseInvoiceItemDetails(InvoiceTO.IdInvoicePurchase);
        //                        if (purchaseInvoiceItemDetailsTOList != null && purchaseInvoiceItemDetailsTOList.Count > 0)
        //                        {
        //                            List<TblPurchaseInvoiceItemDetailsTO> purchaseInvoiceItemDetailsTOListTemp = purchaseInvoiceItemDetailsTOList.Where(w => configParamsTO.ConfigParamVal.Contains(w.OtherTaxId.ToString())).ToList();
        //                            if (purchaseInvoiceItemDetailsTOListTemp != null && purchaseInvoiceItemDetailsTOListTemp.Count > 0)
        //                            {
        //                                foreach (var arr in purchaseInvoiceItemDetailsTOListTemp)
        //                                {
        //                                    if (arr.OtherTaxId == (Int32)Constants.OthrTaxTypeE.TCS)
        //                                    {
        //                                        InvoiceTO.TcsAmt = arr.TaxableAmt;
        //                                    }
        //                                    if (arr.OtherTaxId == (Int32)Constants.OthrTaxTypeE.OTHER_EXPENCES)
        //                                    {
        //                                        InvoiceTO.OtherExpAmt = arr.TaxableAmt;
        //                                    }
        //                                    if (arr.OtherTaxId == (Int32)Constants.OthrTaxTypeE.TRANSPORTER_ADVANCE)
        //                                    {
        //                                        InvoiceTO.TransportorAdvAmt = arr.TaxableAmt;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }

        //                    if (ScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList != null && ScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList.Count > 0)
        //                    {
        //                        foreach (var DetailsTO in ScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList)
        //                        {
        //                            dynamic ScheduleTO = new JObject();
        //                            //Prajakta[2019-06-06] Commented and added
        //                            //ScheduleTO.Date = ScheduleSummaryTO.CreatedOn.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
        //                            ScheduleTO.Date = ScheduleSummaryTO.CorretionCompletedOn.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
        //                            ScheduleTO.TruckNo = ScheduleSummaryTO.VehicleNo;
        //                            if (InvoiceTO.TblPurchaseInvoiceAddrTOList != null && InvoiceTO.TblPurchaseInvoiceAddrTOList.Count > 0)
        //                            {
        //                                ScheduleTO["Supplier Name"] = InvoiceTO.TblPurchaseInvoiceAddrTOList[0].BillingPartyName;
        //                            }
        //                            else
        //                            {
        //                                ScheduleTO["Supplier Name"] = "";
        //                            }
        //                            ScheduleTO.PM = ScheduleSummaryTO.PurchaseManager;
        //                            ScheduleTO.Location = ScheduleSummaryTO.Location;
        //                            ScheduleTO.Grade = DetailsTO.ItemName;
        //                            ScheduleTO.Qty = DetailsTO.Qty;
        //                            totalQty = totalQty + DetailsTO.Qty;
        //                            //ScheduleTO.Rate = String.Format("{0:n}", DetailsTO.Rate);
        //                            ScheduleTO.Rate = Math.Round(DetailsTO.Rate, mathroundFact);
        //                            totalAmount = totalAmount + DetailsTO.Rate;
        //                            //ScheduleTO.BasicGradeAmount = String.Format("{0:n}", DetailsTO.Qty * DetailsTO.Rate);
        //                            ScheduleTO["Basic Grade Amount"] = Math.Round((DetailsTO.Qty * DetailsTO.Rate), mathroundFact);
        //                            totalBasicGradeAmount = Math.Round(totalBasicGradeAmount + (DetailsTO.Qty * DetailsTO.Rate), mathroundFact);
        //                            ScheduleTO.CGST = "";
        //                            ScheduleTO.SGST = "";
        //                            ScheduleTO.IGST = "";
        //                            ScheduleTO["Total Taxes"] = "";
        //                            ScheduleTO.TCS = "";
        //                            ScheduleTO["Total Amount To Be Paid To Party"] = "";
        //                            ScheduleTO["Invoice Amount"] = "";
        //                            ScheduleTO["Balance Amount Rs"] = "";
        //                            ScheduleTO["BillNo And Date"] = "";
        //                            ScheduleTO["E-way Bill No"] = "";
        //                            ScheduleTO["E_Way Bill Date"] = "";
        //                            ScheduleTO.Freight = "";
        //                            ScheduleTO["Narration Remark"] = ScheduleSummaryTO.Narration;
        //                            if (ScheduleSummaryTO.COrNcId == (Int32)Constants.ConfirmTypeE.NONCONFIRM)
        //                            {
        //                                ScheduleTO["Bill Type"] = "Enquiry";
        //                            }
        //                            else if (ScheduleSummaryTO.COrNcId == (Int32)Constants.ConfirmTypeE.CONFIRM)
        //                            {
        //                                ScheduleTO["Bill Type"] = "Order";
        //                            }

        //                            ScheduleTOList.Add(ScheduleTO);
        //                        }
        //                    }
        //                    dynamic ScheduleTO1 = new JObject();
        //                    //ScheduleTO1.Date = ScheduleSummaryTO.CreatedOn.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
        //                    ScheduleTO1.Date = ScheduleSummaryTO.CorretionCompletedOn.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
        //                    ScheduleTO1.TruckNo = ScheduleSummaryTO.VehicleNo;
        //                    ScheduleTO1.TruckNo = ScheduleSummaryTO.VehicleNo + " Total";
        //                    ScheduleTO1["Supplier Name"] = "";
        //                    ScheduleTO1.PM = "";
        //                    ScheduleTO1.Location = "";
        //                    ScheduleTO1.Grade = "";
        //                    ScheduleTO1.Qty = totalQty;
        //                    grandTotal += totalQty;
        //                    grandRate += Math.Round(totalAmount, mathroundFact);
        //                    grandBasicGradeAmount += Math.Round(totalBasicGradeAmount, mathroundFact);
        //                    //ScheduleTO1.Rate = String.Format("{0:n}", totalAmount);
        //                    ScheduleTO1.Rate = Math.Round(totalAmount, mathroundFact);
        //                    //ScheduleTO1.BasicGradeAmount = String.Format("{0:n}", totalBasicGradeAmount);
        //                    ScheduleTO1["Basic Grade Amount"] = Math.Round(totalBasicGradeAmount, mathroundFact);
        //                    //ScheduleTO1.CGST = String.Format("{0:n}", InvoiceTO.CgstAmt);
        //                    ScheduleTO1.CGST = Math.Round(InvoiceTO.CgstAmt, mathroundFact);
        //                    //ScheduleTO1.SGST = String.Format("{0:n}", InvoiceTO.SgstAmt);
        //                    ScheduleTO1.SGST = Math.Round(InvoiceTO.SgstAmt, mathroundFact);
        //                    //ScheduleTO1.IGST = String.Format("{0:n}", InvoiceTO.IgstAmt);
        //                    ScheduleTO1.IGST = Math.Round(InvoiceTO.IgstAmt, mathroundFact);

        //                    //ScheduleTO1.TotalTaxes = String.Format("{0:n}", InvoiceTO.CgstAmt + InvoiceTO.SgstAmt + InvoiceTO.IgstAmt);
        //                    double totaltx = InvoiceTO.CgstAmt + InvoiceTO.SgstAmt + InvoiceTO.IgstAmt;
        //                    totaltx = Math.Round(totaltx, mathroundFact);
        //                    ScheduleTO1["Total Taxes"] = totaltx;

        //                    //ScheduleTO1.TCS = String.Format("{0:n}", InvoiceTO.TcsAmt);
        //                    ScheduleTO1.TCS = Math.Round(InvoiceTO.TcsAmt, mathroundFact);

        //                    //ScheduleTO1.TotalAmountToBePaidToParty = String.Format("{0:n}", totalBasicGradeAmount + totaltx + InvoiceTO.TcsAmt);
        //                    ScheduleTO1["Total Amount To Be Paid To Party"] = Math.Round(totalBasicGradeAmount + totaltx + InvoiceTO.TcsAmt, mathroundFact);

        //                    double TotalAmountToBePaidToParty = Math.Round(totalBasicGradeAmount + totaltx + InvoiceTO.TcsAmt, mathroundFact);
        //                    //ScheduleTO1.InvoiceAmount = String.Format("{0:n}", InvoiceTO.GrandTotal);
        //                    ScheduleTO1["Invoice Amount"] = Math.Round(InvoiceTO.GrandTotal, 2);

        //                    //ScheduleTO1.BalanceAmountRs = String.Format("{0:n}", InvoiceTO.GrandTotal - TotalAmountToBePaidToParty);
        //                    ScheduleTO1["Balance Amount Rs"] = Math.Round(InvoiceTO.GrandTotal - TotalAmountToBePaidToParty, 2);

        //                    ScheduleTO1["BillNo And Date"] = InvoiceTO.InvoiceNo + "/" + InvoiceTO.InvoiceDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
        //                    ScheduleTO1["E-way Bill No"] = InvoiceTO.ElectronicRefNo;
        //                    ScheduleTO1["E_Way Bill Date"] = InvoiceTO.EwayBillDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
        //                    //ScheduleTO1.Freight = String.Format("{0:n}", InvoiceTO.FreightAmt);
        //                    ScheduleTO1.Freight = Math.Round(InvoiceTO.FreightAmt, 2);

        //                    ScheduleTO1["Naration Remark"] = ScheduleSummaryTO.Narration;
        //                    if (ScheduleSummaryTO.COrNcId == (Int32)Constants.ConfirmTypeE.NONCONFIRM)
        //                    {
        //                        ScheduleTO1["Bill Type"] = "Enquiry";
        //                    }
        //                    else if (ScheduleSummaryTO.COrNcId == (Int32)Constants.ConfirmTypeE.CONFIRM)
        //                    {
        //                        ScheduleTO1["Bill Type"] = "Order";
        //                    }


        //                    ScheduleTOList.Add(ScheduleTO1);
        //                    // dynamic ScheduleTO3 = new JObject();
        //                    // ScheduleTOList.Add(ScheduleTO3);
        //                }

        //            }
        //        }

        //        dynamic ScheduleTO2 = new JObject();
        //        ScheduleTO2.Date = "Grand total";
        //        ScheduleTO2.TruckNo = "";
        //        ScheduleTO2.TruckNo = "";
        //        ScheduleTO2["Supplier Name"] = "";
        //        ScheduleTO2.PM = "";
        //        ScheduleTO2.Location = "";
        //        ScheduleTO2.Grade = "";
        //        ScheduleTO2.Qty = Math.Round(grandTotal, mathroundFact);
        //        // grandTotal = +totalQty;
        //        // grandRate = +Math.Round(totalAmount, mathroundFact);
        //        // grandBasicGradeAmount = +Math.Round(totalBasicGradeAmount, mathroundFact);
        //        //ScheduleTO1.Rate = String.Format("{0:n}", totalAmount);
        //        ScheduleTO2.Rate = Math.Round(grandRate, mathroundFact);
        //        //ScheduleTO1.BasicGradeAmount = String.Format("{0:n}", grandBasicGradeAmount);
        //        ScheduleTO2["Basic Grade Amount"] = Math.Round(grandBasicGradeAmount, mathroundFact);
        //        //ScheduleTO1.CGST = String.Format("{0:n}", InvoiceTO.CgstAmt);
        //        ScheduleTO2.CGST = "";
        //        //ScheduleTO1.SGST = String.Format("{0:n}", InvoiceTO.SgstAmt);
        //        ScheduleTO2.SGST = "";
        //        //ScheduleTO1.IGST = String.Format("{0:n}", InvoiceTO.IgstAmt);
        //        ScheduleTO2.IGST = "";

        //        //ScheduleTO1.TotalTaxes = String.Format("{0:n}", InvoiceTO.CgstAmt + InvoiceTO.SgstAmt + InvoiceTO.IgstAmt);
        //        ScheduleTO2["Total Taxes"] = "";

        //        //ScheduleTO1.TCS = String.Format("{0:n}", InvoiceTO.TcsAmt);
        //        ScheduleTO2.TCS = "";

        //        //ScheduleTO1.TotalAmountToBePaidToParty = String.Format("{0:n}", totalBasicGradeAmount + totaltx + InvoiceTO.TcsAmt);
        //        ScheduleTO2["Total Amount To Be Paid To Party"] = "";

        //        // double TotalAmountToBePaidToParty = Math.Round(totalBasicGradeAmount + totaltx + InvoiceTO.TcsAmt, mathroundFact);
        //        //ScheduleTO1.InvoiceAmount = String.Format("{0:n}", InvoiceTO.GrandTotal);
        //        ScheduleTO2["Invoice Amount"] = "";

        //        //ScheduleTO1.BalanceAmountRs = String.Format("{0:n}", InvoiceTO.GrandTotal - TotalAmountToBePaidToParty);
        //        ScheduleTO2["Balance Amount Rs"] = "";

        //        ScheduleTO2["BillNo And Date"] = "";
        //        ScheduleTO2["E-way Bill No"] = "";
        //        ScheduleTO2["E_Way Bill Date"] = "";
        //        //ScheduleTO1.Freight = String.Format("{0:n}", InvoiceTO.FreightAmt);
        //        ScheduleTO2.Freight = "";

        //        ScheduleTO2["Naration Remark"] = "";

        //        ScheduleTO2["Bill Type"] = "";



        //        ScheduleTOList.Add(ScheduleTO2);


        //        return ScheduleTOList;
        //    }
        //    return null;


        //}

        //public List<dynamic> GetListOfGradeNoteSummaryReport(string fromDate, string toDate, int cOrNCId)
        //{
        //    Int16 mathroundFact = 3;
        //    double grandBasicGradeAmount = 0;
        //    double grandTotal = 0;
        //    double grandRate = 0;
        //    DateTime from_Date = DateTime.MinValue;
        //    DateTime to_Date = DateTime.MinValue;
        //    if (Constants.IsDateTime(fromDate))
        //        from_Date = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
        //    if (Constants.IsDateTime(toDate))
        //        to_Date = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));
        //    List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryBL.getListofShcheduleSummary(fromDate, toDate);

        //    dynamic ScheduleTOList = new List<dynamic>();
        //    if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count > 0)
        //    {
        //        //Prajakta[2019-03-21] Added to get only order vehicles report
        //        //tblPurchaseScheduleSummaryTOList = tblPurchaseScheduleSummaryTOList.Where(a => a.COrNcId == (Int32)Constants.ConfirmTypeE.CONFIRM).ToList();
        //        //if (tblPurchaseScheduleSummaryTOList == null || tblPurchaseScheduleSummaryTOList.Count == 0)
        //        //return null;
        //        //Added by minal for BRM Report
        //        if (cOrNCId == (Int32)Constants.ConfirmTypeE.CONFIRM) //For Order
        //        {
        //            tblPurchaseScheduleSummaryTOList = tblPurchaseScheduleSummaryTOList.Where(a => a.COrNcId == (Int32)Constants.ConfirmTypeE.CONFIRM).ToList();
        //            if (tblPurchaseScheduleSummaryTOList == null || tblPurchaseScheduleSummaryTOList.Count == 0)
        //                return null;
        //        }
        //        else if (cOrNCId == (Int32)Constants.ConfirmTypeE.NONCONFIRM) // For Enquiry
        //        {
        //            tblPurchaseScheduleSummaryTOList = tblPurchaseScheduleSummaryTOList.Where(a => a.COrNcId == (Int32)Constants.ConfirmTypeE.NONCONFIRM).ToList();
        //            if (tblPurchaseScheduleSummaryTOList == null || tblPurchaseScheduleSummaryTOList.Count == 0)
        //                return null;
        //        }

        //        _iTblPurchaseScheduleSummaryBL.GetSameProdItemsCombinedListForReport(tblPurchaseScheduleSummaryTOList);

        //        foreach (var ScheduleSummaryTO in tblPurchaseScheduleSummaryTOList)
        //        {
        //            //Prajakta[2019-09-27] Commented to optimize code
        //            //if (ScheduleSummaryTO.VehiclePhaseId == (Int32)Constants.PurchaseVehiclePhasesE.CORRECTIONS && ScheduleSummaryTO.StatusId == (Int32)Constants.TranStatusE.UNLOADING_COMPLETED)
        //            {
        //                double totalAmount = 0;
        //                double totalQty = 0;
        //                double totalBasicGradeAmount = 0;
        //                List<TblPurchaseInvoiceTO> InvoiceTOList = _iPurchaseInvoiceBL.SelectAllTblPurchaseInvoiceListAgainstSchedule(ScheduleSummaryTO.RootScheduleId);

        //                if (InvoiceTOList != null && InvoiceTOList.Count > 0)
        //                {
        //                    TblPurchaseInvoiceTO InvoiceTO = InvoiceTOList[0];
        //                    InvoiceTO.TblPurchaseInvoiceAddrTOList = _iTblPurchaseInvoiceAddrDAO.SelectAllTblPurchaseInvoiceAddr(InvoiceTO.IdInvoicePurchase);

        //                    string configParamName = Constants.CP_SCRAP_OTHER_TAXES_FOR_TCS_IN_GRADE_NOTE;
        //                    TblConfigParamsTO configParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(configParamName);
        //                    if (configParamsTO != null)
        //                    {
        //                        List<TblPurchaseInvoiceItemDetailsTO> purchaseInvoiceItemDetailsTOList = _iTblPurchaseInvoiceItemDetailsDAO.SelectAllTblPurchaseInvoiceItemDetails(InvoiceTO.IdInvoicePurchase);
        //                        if (purchaseInvoiceItemDetailsTOList != null && purchaseInvoiceItemDetailsTOList.Count > 0)
        //                        {
        //                            List<TblPurchaseInvoiceItemDetailsTO> purchaseInvoiceItemDetailsTOListTemp = purchaseInvoiceItemDetailsTOList.Where(w => configParamsTO.ConfigParamVal.Contains(w.OtherTaxId.ToString())).ToList();
        //                            if (purchaseInvoiceItemDetailsTOListTemp != null && purchaseInvoiceItemDetailsTOListTemp.Count > 0)
        //                            {
        //                                foreach (var arr in purchaseInvoiceItemDetailsTOListTemp)
        //                                {
        //                                    if (arr.OtherTaxId == (Int32)Constants.OthrTaxTypeE.TCS)
        //                                    {
        //                                        InvoiceTO.TcsAmt = arr.TaxableAmt;
        //                                    }
        //                                    if (arr.OtherTaxId == (Int32)Constants.OthrTaxTypeE.OTHER_EXPENCES)
        //                                    {
        //                                        InvoiceTO.OtherExpAmt = arr.TaxableAmt;
        //                                    }
        //                                    if (arr.OtherTaxId == (Int32)Constants.OthrTaxTypeE.TRANSPORTER_ADVANCE)
        //                                    {
        //                                        InvoiceTO.TransportorAdvAmt = arr.TaxableAmt;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }

        //                    if (ScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList != null && ScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList.Count > 0)
        //                    {
        //                        foreach (var DetailsTO in ScheduleSummaryTO.PurchaseScheduleSummaryDetailsTOList)
        //                        {
        //                            dynamic ScheduleTO = new JObject();
        //                            //Prajakta[2019-06-06] Commented and added
        //                            //ScheduleTO.Date = ScheduleSummaryTO.CreatedOn.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);

        //                            //Added by minal For BRM Report
        //                            if (cOrNCId == (Int32)Constants.ConfirmTypeE.CONFIRM) // For Order
        //                            {
        //                                //ScheduleTO.Date = ScheduleSummaryTO.CorretionCompletedOn.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
        //                                //ScheduleTO.IGST = "";
        //                                //ScheduleTO["Total Taxes"] = "";
        //                                //ScheduleTO.TCS = "";
        //                                //ScheduleTO["Total Amount To Be Paid To Party"] = "";
        //                                //ScheduleTO["Invoice Amount"] = "";
        //                                //ScheduleTO.Freight = "";
        //                                //ScheduleTO["Balance Amount Rs"] = "";
        //                                //ScheduleTO["BillNo And Date"] = "";
        //                                //ScheduleTO["E-way Bill No"] = "";
        //                                //ScheduleTO["E_Way Bill Date"] = "";
        //                                //totalQty = totalQty + DetailsTO.Qty;
        //                                //totalAmount = totalAmount + DetailsTO.Rate;
        //                                //totalBasicGradeAmount = Math.Round(totalBasicGradeAmount + (DetailsTO.Qty * DetailsTO.Rate), mathroundFact);

        //                                //ScheduleTO["Narration Remark"] = ScheduleSummaryTO.Narration;
        //                                //ScheduleTO["Party Name"] = ScheduleSummaryTO.SupplierName;
        //                            }
        //                            else if (cOrNCId == (Int32)Constants.ConfirmTypeE.NONCONFIRM) // For Enquiry
        //                            {
        //                                ScheduleTO.Date = ScheduleSummaryTO.CorretionCompletedOn.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
        //                                ScheduleTO.TruckNo = ScheduleSummaryTO.VehicleNo;
        //                                if (InvoiceTO.TblPurchaseInvoiceAddrTOList != null && InvoiceTO.TblPurchaseInvoiceAddrTOList.Count > 0)
        //                                {
        //                                    ScheduleTO["Supplier Name"] = InvoiceTO.TblPurchaseInvoiceAddrTOList[0].BillingPartyName;
        //                                }
        //                                else
        //                                {
        //                                    ScheduleTO["Supplier Name"] = "";
        //                                }
        //                                ScheduleTO.PM = ScheduleSummaryTO.PurchaseManager;
        //                                ScheduleTO.Location = ScheduleSummaryTO.Location;
        //                                ScheduleTO.Grade = DetailsTO.ItemName;
        //                                ScheduleTO.Qty = DetailsTO.Qty;
        //                                ScheduleTO.Rate = Math.Round(DetailsTO.Rate, mathroundFact);
        //                                totalQty = totalQty + DetailsTO.Qty;
        //                                totalAmount = totalAmount + DetailsTO.Rate;
        //                                totalBasicGradeAmount = Math.Round(totalBasicGradeAmount + (DetailsTO.Qty * DetailsTO.Rate), mathroundFact);
        //                                ScheduleTO["Basic Grade Amount"] = Math.Round((DetailsTO.Qty * DetailsTO.Rate), mathroundFact);
        //                                ScheduleTO["Party Name"] = ScheduleSummaryTO.SupplierName;
        //                            }
        //                            ScheduleTOList.Add(ScheduleTO);
        //                            //ScheduleTO.Date = ScheduleSummaryTO.CorretionCompletedOn.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
        //                            //ScheduleTO.TruckNo = ScheduleSummaryTO.VehicleNo;
        //                            //if (InvoiceTO.TblPurchaseInvoiceAddrTOList != null && InvoiceTO.TblPurchaseInvoiceAddrTOList.Count > 0)
        //                            //{
        //                            //    ScheduleTO["Supplier Name"] = InvoiceTO.TblPurchaseInvoiceAddrTOList[0].BillingPartyName;
        //                            //}
        //                            //else
        //                            //{
        //                            //    ScheduleTO["Supplier Name"] = "";
        //                            //}
        //                            //ScheduleTO.PM = ScheduleSummaryTO.PurchaseManager;
        //                            //ScheduleTO.Location = ScheduleSummaryTO.Location;
        //                            //ScheduleTO.Grade = DetailsTO.ItemName;
        //                            //ScheduleTO.Qty = DetailsTO.Qty;
        //                            //totalQty = totalQty + DetailsTO.Qty;
        //                            //ScheduleTO.Rate = String.Format("{0:n}", DetailsTO.Rate);
        //                            //ScheduleTO.Rate = Math.Round(DetailsTO.Rate, mathroundFact);
        //                            //totalAmount = totalAmount + DetailsTO.Rate;
        //                            //ScheduleTO.BasicGradeAmount = String.Format("{0:n}", DetailsTO.Qty * DetailsTO.Rate);
        //                            //ScheduleTO["Basic Grade Amount"] = Math.Round((DetailsTO.Qty * DetailsTO.Rate), mathroundFact);
        //                            //totalBasicGradeAmount = Math.Round(totalBasicGradeAmount + (DetailsTO.Qty * DetailsTO.Rate), mathroundFact);
        //                            //ScheduleTO.CGST = "";
        //                            //ScheduleTO.SGST = "";
        //                            //ScheduleTO.IGST = "";
        //                            //ScheduleTO["Total Taxes"] = "";
        //                            //ScheduleTO.TCS = "";
        //                            //ScheduleTO["Total Amount To Be Paid To Party"] = "";
        //                            //ScheduleTO["Invoice Amount"] = "";
        //                            //ScheduleTO["Balance Amount Rs"] = "";
        //                            //ScheduleTO["BillNo And Date"] = "";
        //                            //ScheduleTO["E-way Bill No"] = "";
        //                            //ScheduleTO["E_Way Bill Date"] = "";
        //                            //ScheduleTO.Freight = "";
        //                            //ScheduleTO["Narration Remark"] = ScheduleSummaryTO.Narration;
        //                            //if (ScheduleSummaryTO.COrNcId == (Int32)Constants.ConfirmTypeE.NONCONFIRM)
        //                            //{
        //                            //ScheduleTO["Bill Type"] = "Enquiry";
        //                            //}
        //                            //else if (ScheduleSummaryTO.COrNcId == (Int32)Constants.ConfirmTypeE.CONFIRM)
        //                            //{
        //                            // ScheduleTO["Bill Type"] = "Order";
        //                            //}
        //                            //ScheduleTO["Party Name"] = ScheduleSummaryTO.SupplierName;

        //                            //ScheduleTOList.Add(ScheduleTO);
        //                        }
        //                    }
        //                    dynamic ScheduleTO1 = new JObject();
        //                    double totaltx = 0, TotalAmountToBePaidToParty = 0;
        //                    //Added by minal For BRM Report
        //                    if (cOrNCId == (Int32)Constants.ConfirmTypeE.CONFIRM) // For Order
        //                    {
        //                        ScheduleTO1.Date = ScheduleSummaryTO.CorretionCompletedOn.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
        //                        ScheduleTO1.IGST = Math.Round(InvoiceTO.IgstAmt, mathroundFact);
        //                        totaltx = InvoiceTO.CgstAmt + InvoiceTO.SgstAmt + InvoiceTO.IgstAmt;
        //                        totaltx = Math.Round(totaltx, mathroundFact);
        //                        ScheduleTO1["Total Taxes"] = totaltx;
        //                        ScheduleTO1.TCS = Math.Round(InvoiceTO.TcsAmt, mathroundFact);
        //                        grandTotal += totalQty;
        //                        grandRate += Math.Round(totalAmount, mathroundFact);
        //                        grandBasicGradeAmount += Math.Round(totalBasicGradeAmount, mathroundFact);
        //                        ScheduleTO1["Total Amount To Be Paid To Party"] = Math.Round(totalBasicGradeAmount + totaltx + InvoiceTO.TcsAmt, mathroundFact);
        //                        ScheduleTO1["Invoice Amount"] = Math.Round(InvoiceTO.GrandTotal, 2);
        //                        TotalAmountToBePaidToParty = Math.Round(totalBasicGradeAmount + totaltx + InvoiceTO.TcsAmt, mathroundFact);
        //                        ScheduleTO1["Balance Amount Rs"] = Math.Round((InvoiceTO.GrandTotal + ScheduleSummaryTO.FreightAmount) - TotalAmountToBePaidToParty, 2);
        //                        ScheduleTO1["BillNo And Date"] = InvoiceTO.InvoiceNo + "/" + InvoiceTO.InvoiceDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
        //                        ScheduleTO1["E-way Bill No"] = InvoiceTO.ElectronicRefNo;
        //                        ScheduleTO1["E_Way Bill Date"] = InvoiceTO.EwayBillDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
        //                        ScheduleTO1.Freight = Math.Round(ScheduleSummaryTO.FreightAmount, 2);
        //                        ScheduleTO1["Naration Remark"] = ScheduleSummaryTO.Narration;
        //                        ScheduleTO1["Party Name"] = ScheduleSummaryTO.SupplierName;
        //                    }
        //                    else if (cOrNCId == (Int32)Constants.ConfirmTypeE.NONCONFIRM) // For Enquiry
        //                    {
        //                        ScheduleTO1.Date = ScheduleSummaryTO.CorretionCompletedOn.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
        //                        ScheduleTO1.TruckNo = ScheduleSummaryTO.VehicleNo;
        //                        ScheduleTO1.TruckNo = ScheduleSummaryTO.VehicleNo + " Total";
        //                        ScheduleTO1["Supplier Name"] = "";
        //                        ScheduleTO1.PM = "";
        //                        ScheduleTO1.Location = "";
        //                        ScheduleTO1.Grade = "";
        //                        ScheduleTO1.Qty = totalQty;
        //                        grandTotal += totalQty;
        //                        grandRate += Math.Round(totalAmount, mathroundFact);
        //                        grandBasicGradeAmount += Math.Round(totalBasicGradeAmount, mathroundFact);
        //                        //ScheduleTO1.Rate = String.Format("{0:n}", totalAmount);
        //                        ScheduleTO1.Rate = Math.Round(totalAmount, mathroundFact);
        //                        //ScheduleTO1.BasicGradeAmount = String.Format("{0:n}", totalBasicGradeAmount);
        //                        ScheduleTO1["Basic Grade Amount"] = Math.Round(totalBasicGradeAmount, mathroundFact);
        //                        totaltx = InvoiceTO.CgstAmt + InvoiceTO.SgstAmt + InvoiceTO.IgstAmt;
        //                        totaltx = Math.Round(totaltx, mathroundFact);
        //                        ScheduleTO1["Total Amount To Be Paid To Party"] = Math.Round(totalBasicGradeAmount + totaltx + InvoiceTO.TcsAmt, mathroundFact);
        //                        ScheduleTO1.Freight = Math.Round(ScheduleSummaryTO.FreightAmount, 2);
        //                        TotalAmountToBePaidToParty = Math.Round(totalBasicGradeAmount + totaltx + InvoiceTO.TcsAmt, mathroundFact);
        //                        ScheduleTO1["Balance Amount Rs"] = Math.Round((InvoiceTO.GrandTotal + ScheduleSummaryTO.FreightAmount) - TotalAmountToBePaidToParty, 2);
        //                        ScheduleTO1["Party Name"] = ScheduleSummaryTO.SupplierName;
        //                    }
        //                    ScheduleTOList.Add(ScheduleTO1);
        //                    //ScheduleTO1.Date = ScheduleSummaryTO.CreatedOn.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
        //                    //ScheduleTO1.Date = ScheduleSummaryTO.CorretionCompletedOn.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
        //                    //ScheduleTO1.TruckNo = ScheduleSummaryTO.VehicleNo;
        //                    //ScheduleTO1.TruckNo = ScheduleSummaryTO.VehicleNo + " Total";
        //                    //ScheduleTO1["Supplier Name"] = "";
        //                    //ScheduleTO1.PM = "";
        //                    //ScheduleTO1.Location = "";
        //                    //ScheduleTO1.Grade = "";
        //                    //ScheduleTO1.Qty = totalQty;
        //                    //grandTotal += totalQty;
        //                    //grandRate += Math.Round(totalAmount, mathroundFact);
        //                    //grandBasicGradeAmount += Math.Round(totalBasicGradeAmount, mathroundFact);
        //                    ////ScheduleTO1.Rate = String.Format("{0:n}", totalAmount);
        //                    //ScheduleTO1.Rate = Math.Round(totalAmount, mathroundFact);
        //                    ////ScheduleTO1.BasicGradeAmount = String.Format("{0:n}", totalBasicGradeAmount);
        //                    //ScheduleTO1["Basic Grade Amount"] = Math.Round(totalBasicGradeAmount, mathroundFact);
        //                    //ScheduleTO1.CGST = String.Format("{0:n}", InvoiceTO.CgstAmt);
        //                    //ScheduleTO1.CGST = Math.Round(InvoiceTO.CgstAmt, mathroundFact);
        //                    //ScheduleTO1.SGST = String.Format("{0:n}", InvoiceTO.SgstAmt);
        //                    //ScheduleTO1.SGST = Math.Round(InvoiceTO.SgstAmt, mathroundFact);
        //                    //ScheduleTO1.IGST = String.Format("{0:n}", InvoiceTO.IgstAmt);
        //                    //ScheduleTO1.IGST = Math.Round(InvoiceTO.IgstAmt, mathroundFact);

        //                    //ScheduleTO1.TotalTaxes = String.Format("{0:n}", InvoiceTO.CgstAmt + InvoiceTO.SgstAmt + InvoiceTO.IgstAmt);
        //                    //double totaltx = InvoiceTO.CgstAmt + InvoiceTO.SgstAmt + InvoiceTO.IgstAmt;
        //                    //totaltx = Math.Round(totaltx, mathroundFact);
        //                    //ScheduleTO1["Total Taxes"] = totaltx;

        //                    //ScheduleTO1.TCS = String.Format("{0:n}", InvoiceTO.TcsAmt);
        //                    //ScheduleTO1.TCS = Math.Round(InvoiceTO.TcsAmt, mathroundFact);

        //                    //ScheduleTO1.TotalAmountToBePaidToParty = String.Format("{0:n}", totalBasicGradeAmount + totaltx + InvoiceTO.TcsAmt);
        //                    //ScheduleTO1["Total Amount To Be Paid To Party"] = Math.Round(totalBasicGradeAmount + totaltx + InvoiceTO.TcsAmt, mathroundFact);

        //                    //TotalAmountToBePaidToParty = Math.Round(totalBasicGradeAmount + totaltx + InvoiceTO.TcsAmt, mathroundFact);
        //                    //ScheduleTO1.InvoiceAmount = String.Format("{0:n}", InvoiceTO.GrandTotal);
        //                    //ScheduleTO1["Invoice Amount"] = Math.Round(InvoiceTO.GrandTotal, 2);

        //                    //ScheduleTO1.BalanceAmountRs = String.Format("{0:n}", InvoiceTO.GrandTotal - TotalAmountToBePaidToParty);
        //                    //ScheduleTO1["Balance Amount Rs"] = Math.Round(InvoiceTO.GrandTotal - TotalAmountToBePaidToParty, 2);

        //                    //ScheduleTO1["BillNo And Date"] = InvoiceTO.InvoiceNo + "/" + InvoiceTO.InvoiceDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
        //                    //ScheduleTO1["E-way Bill No"] = InvoiceTO.ElectronicRefNo;
        //                    //ScheduleTO1["E_Way Bill Date"] = InvoiceTO.EwayBillDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
        //                    ////ScheduleTO1.Freight = String.Format("{0:n}", InvoiceTO.FreightAmt);
        //                    //ScheduleTO1.Freight = Math.Round(InvoiceTO.FreightAmt, 2);
        //                    //ScheduleTO1["Naration Remark"] = ScheduleSummaryTO.Narration;


        //                    //if (ScheduleSummaryTO.COrNcId == (Int32)Constants.ConfirmTypeE.NONCONFIRM)
        //                    //{
        //                    //ScheduleTO1["Bill Type"] = "Enquiry";
        //                    //}
        //                    //else if (ScheduleSummaryTO.COrNcId == (Int32)Constants.ConfirmTypeE.CONFIRM)
        //                    //{
        //                    //ScheduleTO1["Bill Type"] = "Order";
        //                    //}

        //                    //ScheduleTO1["Party Name"] = ScheduleSummaryTO.SupplierName;


        //                    //ScheduleTOList.Add(ScheduleTO1);
        //                    // dynamic ScheduleTO3 = new JObject();
        //                    // ScheduleTOList.Add(ScheduleTO3);
        //                }

        //            }
        //        }

        //        dynamic ScheduleTO2 = new JObject();

        //        //Added by minal For BRM Report
        //        if (cOrNCId == (Int32)Constants.ConfirmTypeE.CONFIRM) // For Order
        //        {
        //            ScheduleTO2.IGST = "";
        //            ScheduleTO2["Total Taxes"] = "";
        //            ScheduleTO2.TCS = "";
        //            ScheduleTO2["Total Amount To Be Paid To Party"] = "";
        //            ScheduleTO2["Invoice Amount"] = "";
        //            ScheduleTO2["Balance Amount Rs"] = "";
        //            ScheduleTO2.Freight = "";
        //            ScheduleTO2["Naration Remark"] = "";
        //        }
        //        else if (cOrNCId == (Int32)Constants.ConfirmTypeE.NONCONFIRM) // For Enquiry
        //        {
        //            ScheduleTO2.Date = "Grand total";
        //            ScheduleTO2.TruckNo = "";
        //            ScheduleTO2.TruckNo = "";
        //            ScheduleTO2["Supplier Name"] = "";
        //            ScheduleTO2.PM = "";
        //            ScheduleTO2.Location = "";
        //            ScheduleTO2.Grade = "";
        //            ScheduleTO2.Qty = Math.Round(grandTotal, mathroundFact);
        //            ScheduleTO2.Rate = Math.Round(grandRate, mathroundFact);
        //            ScheduleTO2["Basic Grade Amount"] = Math.Round(grandBasicGradeAmount, mathroundFact);
        //            ScheduleTO2["Total Amount To Be Paid To Party"] = "";
        //            ScheduleTO2["Balance Amount Rs"] = "";
        //            ScheduleTO2.Freight = "";
        //        }
        //        ScheduleTOList.Add(ScheduleTO2);
        //        /*ScheduleTO2.Date = "Grand total";
        //        ScheduleTO2.TruckNo = "";
        //        ScheduleTO2.TruckNo = "";
        //        ScheduleTO2["Supplier Name"] = "";
        //        ScheduleTO2.PM = "";
        //        ScheduleTO2.Location = "";
        //        ScheduleTO2.Grade = "";
        //        ScheduleTO2.Qty = Math.Round(grandTotal, mathroundFact);
        //        // grandTotal = +totalQty;
        //        // grandRate = +Math.Round(totalAmount, mathroundFact);
        //        // grandBasicGradeAmount = +Math.Round(totalBasicGradeAmount, mathroundFact);
        //        //ScheduleTO1.Rate = String.Format("{0:n}", totalAmount);
        //        ScheduleTO2.Rate = Math.Round(grandRate, mathroundFact);
        //        //ScheduleTO1.BasicGradeAmount = String.Format("{0:n}", grandBasicGradeAmount);
        //        ScheduleTO2["Basic Grade Amount"] = Math.Round(grandBasicGradeAmount, mathroundFact);
        //        //ScheduleTO1.CGST = String.Format("{0:n}", InvoiceTO.CgstAmt);
        //        ScheduleTO2.CGST = "";
        //        //ScheduleTO1.SGST = String.Format("{0:n}", InvoiceTO.SgstAmt);
        //        ScheduleTO2.SGST = "";
        //        //ScheduleTO1.IGST = String.Format("{0:n}", InvoiceTO.IgstAmt);
        //        ScheduleTO2.IGST = "";

        //        //ScheduleTO1.TotalTaxes = String.Format("{0:n}", InvoiceTO.CgstAmt + InvoiceTO.SgstAmt + InvoiceTO.IgstAmt);
        //        ScheduleTO2["Total Taxes"] = "";

        //        //ScheduleTO1.TCS = String.Format("{0:n}", InvoiceTO.TcsAmt);
        //        ScheduleTO2.TCS = "";

        //        //ScheduleTO1.TotalAmountToBePaidToParty = String.Format("{0:n}", totalBasicGradeAmount + totaltx + InvoiceTO.TcsAmt);
        //        ScheduleTO2["Total Amount To Be Paid To Party"] = "";

        //        // double TotalAmountToBePaidToParty = Math.Round(totalBasicGradeAmount + totaltx + InvoiceTO.TcsAmt, mathroundFact);
        //        //ScheduleTO1.InvoiceAmount = String.Format("{0:n}", InvoiceTO.GrandTotal);
        //        ScheduleTO2["Invoice Amount"] = "";

        //        //ScheduleTO1.BalanceAmountRs = String.Format("{0:n}", InvoiceTO.GrandTotal - TotalAmountToBePaidToParty);
        //        ScheduleTO2["Balance Amount Rs"] = "";

        //        ScheduleTO2["BillNo And Date"] = "";
        //        ScheduleTO2["E-way Bill No"] = "";
        //        ScheduleTO2["E_Way Bill Date"] = "";
        //        //ScheduleTO1.Freight = String.Format("{0:n}", InvoiceTO.FreightAmt);
        //        ScheduleTO2.Freight = "";

        //        ScheduleTO2["Naration Remark"] = "";

        //       // ScheduleTO2["Bill Type"] = "";

        //        ScheduleTOList.Add(ScheduleTO2);*/


        //        return ScheduleTOList;
        //    }
        //    return null;


        //}


        public TblPurchaseScheduleSummaryTO GetPurchaseSummaryWithInvoiceForPrint(Int32 rootScheduleId)
        {
            try
            {
                Int32 statusId = (Int32)Constants.TranStatusE.UNLOADING_COMPLETED;
                Int32 vehiclePhaseId = (Int32)Constants.PurchaseVehiclePhasesE.CORRECTIONS;
                Boolean isGetGradeExpDtls = true;

                List<TblPurchaseScheduleSummaryTO> scheduleList = _iTblPurchaseScheduleSummaryBL.SelectVehicleScheduleByRootAndStatusId(rootScheduleId, statusId, vehiclePhaseId);
                if (scheduleList != null && scheduleList.Count > 0)
                {
                    scheduleList = scheduleList.Where(a => a.COrNcId == (Int32)Constants.ConfirmTypeE.CONFIRM).ToList();
                    if (scheduleList != null && scheduleList.Count == 1)
                    {
                        scheduleList[0].PurchaseScheduleSummaryDetailsTOList = _iTblPurchaseVehicleDetailsBL.SelectAllTblPurchaseVehicleDetailsList(scheduleList[0].IdPurchaseScheduleSummary, isGetGradeExpDtls);

                        if (scheduleList[0].PurchaseScheduleSummaryDetailsTOList != null && scheduleList[0].PurchaseScheduleSummaryDetailsTOList.Count > 0)
                        {
                            _iTblPurchaseScheduleSummaryBL.GetSameProdItemsCombinedListForReport(scheduleList);

                            List<TblPurchaseInvoiceTO> InvoiceTOList = _iPurchaseInvoiceBL.SelectAllTblPurchaseInvoiceListAgainstSchedule(rootScheduleId);

                            if (InvoiceTOList != null && InvoiceTOList.Count > 0)
                            {
                                TblPurchaseInvoiceTO InvoiceTO = InvoiceTOList[0];
                                InvoiceTO.TblPurchaseInvoiceAddrTOList = _iTblPurchaseInvoiceAddrDAO.SelectAllTblPurchaseInvoiceAddr(InvoiceTO.IdInvoicePurchase);
                                if (InvoiceTO.TblPurchaseInvoiceAddrTOList != null && InvoiceTO.TblPurchaseInvoiceAddrTOList.Count > 0)
                                {
                                    InvoiceTO.SupplierName = InvoiceTO.TblPurchaseInvoiceAddrTOList[0].BillingPartyName;
                                }
                                string configParamName = Constants.CP_SCRAP_OTHER_TAXES_FOR_TCS_IN_GRADE_NOTE;
                                TblConfigParamsTO configParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(configParamName);
                                if (configParamsTO != null)
                                {
                                    List<TblPurchaseInvoiceItemDetailsTO> purchaseInvoiceItemDetailsTOList = _iTblPurchaseInvoiceItemDetailsDAO.SelectAllTblPurchaseInvoiceItemDetails(InvoiceTO.IdInvoicePurchase);
                                    if (purchaseInvoiceItemDetailsTOList != null && purchaseInvoiceItemDetailsTOList.Count > 0)
                                    {
                                        List<TblPurchaseInvoiceItemDetailsTO> purchaseInvoiceItemDetailsTOListTemp = purchaseInvoiceItemDetailsTOList.Where(w => configParamsTO.ConfigParamVal.Contains(w.OtherTaxId.ToString())).ToList();
                                        if (purchaseInvoiceItemDetailsTOListTemp != null && purchaseInvoiceItemDetailsTOListTemp.Count > 0)
                                        {
                                            foreach (var arr in purchaseInvoiceItemDetailsTOListTemp)
                                            {
                                                if (arr.OtherTaxId == (Int32)Constants.OthrTaxTypeE.TCS)
                                                {
                                                    InvoiceTO.TcsAmt = arr.TaxableAmt;
                                                }
                                                if (arr.OtherTaxId == (Int32)Constants.OthrTaxTypeE.OTHER_EXPENCES)
                                                {
                                                    InvoiceTO.OtherExpAmt = arr.TaxableAmt;
                                                }
                                                if (arr.OtherTaxId == (Int32)Constants.OthrTaxTypeE.TRANSPORTER_ADVANCE)
                                                {
                                                    InvoiceTO.TransportorAdvAmt = arr.TaxableAmt;
                                                }
                                            }
                                        }
                                    }
                                }


                                List<TblPurchaseInvoiceDocumentsTO> TblPurchaseInvoiceDocumentsTOList = _iTblPurchaseInvoiceDocumentsBL.SelectAllTblPurchaseInvoiceDocuments(InvoiceTO.IdInvoicePurchase);
                                List<TblPurchaseDocToVerifyTO> TblPurchaseDocToVerifyTOList = _iTblPurchaseDocToVerifyBL.SelectAllTblPurchaseDocToVerify();
                                InvoiceTO.TblPurchaseDocToVerifyTOList = TblPurchaseDocToVerifyTOList;
                                InvoiceTO.TblPurchaseInvoiceDocumentsTOList = TblPurchaseInvoiceDocumentsTOList;
                                scheduleList[0].TblPurchaseInvoiceTO = InvoiceTO;
                                return scheduleList[0];
                            }
                        }

                    }
                }
                return null;
            }
            catch (System.Exception ex)
            {
                return null;
            }
            finally
            {

            }

        }

        public List<purchaseSummuryReportTo> GetPurchaseSummaryReport(string fromDate, string toDate,int isOldOrNewFlag,String purchaseMangerIds)
        {
            if (isOldOrNewFlag == 1)
            {
                return GetPurchaseSummaryReportForOld(fromDate, toDate, purchaseMangerIds);
            }
            else if (isOldOrNewFlag == 0)
            {
                return GetPurchaseSummaryReportForNew(fromDate, toDate, purchaseMangerIds);
            }
            return null;
        }

        public List<purchaseSummuryReportTo> GetPurchaseSummaryReportForOld(string fromDate, string toDate,String purchaseMangerIds)
        {
            string otherTaxIdsTcs = "";
            string otherTaxIdsTransporter = "";
            DateTime from_Date = DateTime.MinValue;
            DateTime to_Date = DateTime.MinValue;
            if (Constants.IsDateTime(fromDate))
                from_Date = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
            if (Constants.IsDateTime(toDate))
                to_Date = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));

            TblConfigParamsTO configParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_SCRAP_OTHER_TAXES_FOR_OTHER_IN_PURCHASE_SUMMARY_REPORT);
            if (configParamsTO != null)
            {
                otherTaxIdsTcs = configParamsTO.ConfigParamVal;
            }
            configParamsTO = null;
            configParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_SCRAP_OTHER_TAXES_FOR_TRANSPORTER_ADVANCE_PURCHASE_SUMMARY_REPORT);
            if (configParamsTO != null)
            {
                otherTaxIdsTransporter = configParamsTO.ConfigParamVal;
            }
            List<purchaseSummuryReportTo> list = _ireportDAO.SelectPurchaseSummuryReportForOld(from_Date, to_Date, otherTaxIdsTcs, otherTaxIdsTransporter, purchaseMangerIds);
            return list;
        }

        public List<purchaseSummuryReportTo> GetPurchaseSummaryReportForNew(string fromDate, string toDate,String purchaseMangerIds)
        {
            string otherTaxIdsTcs = "";
            string otherTaxIdsTransporter = "";
            string otherExpensesInsuranceId = string.Empty;
            DateTime from_Date = DateTime.MinValue;
            DateTime to_Date = DateTime.MinValue;
            if (Constants.IsDateTime(fromDate))
                from_Date = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
            if (Constants.IsDateTime(toDate))
                to_Date = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));

            TblConfigParamsTO configParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_SCRAP_OTHER_TAXES_FOR_OTHER_IN_PURCHASE_SUMMARY_REPORT);
            if (configParamsTO != null)
            {
                otherTaxIdsTcs = configParamsTO.ConfigParamVal;
            }
            configParamsTO = null;
            configParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_SCRAP_OTHER_TAXES_FOR_TRANSPORTER_ADVANCE_PURCHASE_SUMMARY_REPORT);
            if (configParamsTO != null)
            {
                otherTaxIdsTransporter = configParamsTO.ConfigParamVal;
            }
            configParamsTO = null;
            configParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_SCRAP_OTHER_TAXES_FOR_OTHER_EXPENSES_INSURANCE_AMT_PURCHASE_SUMMARY_REPORT);
            if (configParamsTO != null)
            {
                otherExpensesInsuranceId = configParamsTO.ConfigParamVal;
            }
            List<purchaseSummuryReportTo> list = _ireportDAO.SelectPurchaseSummuryReportForNew(from_Date, to_Date, otherTaxIdsTcs, otherTaxIdsTransporter, otherExpensesInsuranceId, purchaseMangerIds);
            return list;
        }

        public List<purchaseSummuryReportTo> PurchaseSummaryReportH(string fromDate, string toDate,String purchaseMangerIds)
        {
            DateTime from_Date = DateTime.MinValue;
            DateTime to_Date = DateTime.MinValue;
            if (Constants.IsDateTime(fromDate))
                from_Date = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
            if (Constants.IsDateTime(toDate))
                to_Date = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));

            List<purchaseSummuryReportTo> list = _ireportDAO.PurchaseSummaryReportH(from_Date, to_Date, purchaseMangerIds);
            if (list != null && list.Count > 0)
            {
                int invoicePurchaseId = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    purchaseSummuryReportTo purchaseSummuryReportTO = list[i];
                    if (invoicePurchaseId != purchaseSummuryReportTO.IdInvoicePurchase)
                    {
                        invoicePurchaseId = purchaseSummuryReportTO.IdInvoicePurchase;

                    }
                    else
                    {
                        purchaseSummuryReportTO.Igst = null;
                        purchaseSummuryReportTO.IgstAmt = 0;
                        purchaseSummuryReportTO.Cgst = null;
                        purchaseSummuryReportTO.CgstAmt = 0;
                        purchaseSummuryReportTO.Sgst = null;
                        purchaseSummuryReportTO.SgstAmt = 0;
                        purchaseSummuryReportTO.FreightAmt = 0;
                        purchaseSummuryReportTO.GrandTotal = 0;
                        // purchaseSummuryReportTO.LrDate = null;
                        // purchaseSummuryReportTO.LrNo = null;
                        // purchaseSummuryReportTO.BrokerMobNo = null;
                        //  purchaseSummuryReportTO.SupplierAddress = null;
                        //  purchaseSummuryReportTO.SupplierMobNo = null;
                    }

                }
            }

            return list;
        }
        //chetan[25-feb-2020] added for print report on template
        public ResultMessage PrintPurchaseSummaryReport(string fromDate, string toDate,String purchaseMangerIds)
        {
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                DateTime from_Date = DateTime.MinValue;
                DateTime to_Date = DateTime.MinValue;
                DataSet printDataSet = new DataSet();
                DataTable GSTPurchaseReportDT = new DataTable();
                // ResultMessage resultMessage = new ResultMessage();
                if (Constants.IsDateTime(fromDate))
                    from_Date = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
                if (Constants.IsDateTime(toDate))
                    to_Date = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));
                List<purchaseSummuryReportTo> purchaseSummuryReportToList = PurchaseSummaryReportH(fromDate, toDate, purchaseMangerIds);
                if (purchaseSummuryReportToList != null && purchaseSummuryReportToList.Count > 0)
                {
                    GSTPurchaseReportDT.Columns.Add("InvoiceNo", typeof(string));
                    GSTPurchaseReportDT.Columns.Add("TransactionDate", typeof(string));
                    GSTPurchaseReportDT.Columns.Add("VehEntryDate", typeof(string));
                    GSTPurchaseReportDT.Columns.Add("PartyName", typeof(string));
                    GSTPurchaseReportDT.Columns.Add("BuyersState", typeof(string));
                    GSTPurchaseReportDT.Columns.Add("BuyersGSTINOrUIN", typeof(string));
                    GSTPurchaseReportDT.Columns.Add("MarketingOfficer", typeof(string));
                    GSTPurchaseReportDT.Columns.Add("ConsigneeName", typeof(string));
                    GSTPurchaseReportDT.Columns.Add("ConsigneeAddressLine2", typeof(string));
                    GSTPurchaseReportDT.Columns.Add("ConsigneeAddressLine1", typeof(string));
                    GSTPurchaseReportDT.Columns.Add("State", typeof(string));
                    GSTPurchaseReportDT.Columns.Add("GSTINOrUIN", typeof(string));
                    GSTPurchaseReportDT.Columns.Add("BookingRate", typeof(double));
                    GSTPurchaseReportDT.Columns.Add("ItemDescription", typeof(string));
                    GSTPurchaseReportDT.Columns.Add("Size", typeof(string));
                    GSTPurchaseReportDT.Columns.Add("SizeRate", typeof(double));
                    GSTPurchaseReportDT.Columns.Add("SIZEWEIGHT", typeof(double));
                    GSTPurchaseReportDT.Columns.Add("BASICVALUE", typeof(double));
                    GSTPurchaseReportDT.Columns.Add("PurchaseLedger", typeof(string));
                    GSTPurchaseReportDT.Columns.Add("CGSTOUTPUT", typeof(double));
                    GSTPurchaseReportDT.Columns.Add("SGSTOUTPUT", typeof(double));
                    GSTPurchaseReportDT.Columns.Add("IGSTOUTPUT", typeof(double));

                    GSTPurchaseReportDT.Columns.Add("billvalue", typeof(double));
                    GSTPurchaseReportDT.Columns.Add("DispatchedThroughOrTransporter", typeof(string));
                    GSTPurchaseReportDT.Columns.Add("BillofLadingOrLR-RRNo", typeof(string));
                    GSTPurchaseReportDT.Columns.Add("LRDate", typeof(string));
                    GSTPurchaseReportDT.Columns.Add("MotorVehicleNo", typeof(string));
                    GSTPurchaseReportDT.Columns.Add("CustomerPhoneNO", typeof(string));
                    GSTPurchaseReportDT.Columns.Add("BrokerPhoneNo", typeof(string));
                    GSTPurchaseReportDT.Columns.Add("DriverPhoneNo", typeof(string));
                    GSTPurchaseReportDT.Columns.Add("FreightValue", typeof(double));
                    GSTPurchaseReportDT.Columns.Add("TallyRefIdParty", typeof(string));
                    GSTPurchaseReportDT.Columns.Add("SupplierName", typeof(string));
                    GSTPurchaseReportDT.Columns.Add("TCSAmt", typeof(double));


                    for (int i = 0; i < purchaseSummuryReportToList.Count; i++)
                    {
                        GSTPurchaseReportDT.Rows.Add();
                        int rowNo = GSTPurchaseReportDT.Rows.Count - 1;

                        purchaseSummuryReportTo purchaseSummuryReportTO = purchaseSummuryReportToList[i];

                        GSTPurchaseReportDT.Rows[rowNo]["InvoiceNo"] = purchaseSummuryReportTO.InvoiceNo;
                        GSTPurchaseReportDT.Rows[rowNo]["TransactionDate"] = purchaseSummuryReportTO.InvoiceDate;
                        GSTPurchaseReportDT.Rows[rowNo]["VehEntryDate"] = purchaseSummuryReportTO.CreatedOn;
                        GSTPurchaseReportDT.Rows[rowNo]["PartyName"] = purchaseSummuryReportTO.SupplierName;
                        GSTPurchaseReportDT.Rows[rowNo]["BuyersState"] = purchaseSummuryReportTO.SalerState;
                        GSTPurchaseReportDT.Rows[rowNo]["BuyersGSTINOrUIN"] = purchaseSummuryReportTO.SalerGstin;
                        GSTPurchaseReportDT.Rows[rowNo]["MarketingOfficer"] = purchaseSummuryReportTO.PurchaseManager;
                        GSTPurchaseReportDT.Rows[rowNo]["ConsigneeName"] = purchaseSummuryReportTO.SupplierName;
                        GSTPurchaseReportDT.Rows[rowNo]["ConsigneeAddressLine2"] = purchaseSummuryReportTO.SupplierDist;
                        GSTPurchaseReportDT.Rows[rowNo]["ConsigneeAddressLine1"] = purchaseSummuryReportTO.SupplierAddress;

                        GSTPurchaseReportDT.Rows[rowNo]["GSTINOrUIN"] = purchaseSummuryReportTO.SalerGstin;
                        GSTPurchaseReportDT.Rows[rowNo]["BookingRate"] = purchaseSummuryReportTO.BookingRate;
                        GSTPurchaseReportDT.Rows[rowNo]["ItemDescription"] = purchaseSummuryReportTO.MaterialType;
                        GSTPurchaseReportDT.Rows[rowNo]["Size"] = purchaseSummuryReportTO.ProductItemDesc;
                        GSTPurchaseReportDT.Rows[rowNo]["SizeRate"] = purchaseSummuryReportTO.Rate;
                        GSTPurchaseReportDT.Rows[rowNo]["SIZEWEIGHT"] = purchaseSummuryReportTO.InvoiceQty;
                        GSTPurchaseReportDT.Rows[rowNo]["BASICVALUE"] = purchaseSummuryReportTO.BasicTotal;
                        GSTPurchaseReportDT.Rows[rowNo]["PurchaseLedger"] = "Purchase gst";
                        GSTPurchaseReportDT.Rows[rowNo]["CGSTOUTPUT"] = Convert.ToDouble(purchaseSummuryReportTO.Cgst);
                        GSTPurchaseReportDT.Rows[rowNo]["SGSTOUTPUT"] = Convert.ToDouble(purchaseSummuryReportTO.Sgst);
                        GSTPurchaseReportDT.Rows[rowNo]["IGSTOUTPUT"] = Convert.ToDouble(purchaseSummuryReportTO.Igst);
                        GSTPurchaseReportDT.Rows[rowNo]["billvalue"] = purchaseSummuryReportTO.GrandTotal;
                        GSTPurchaseReportDT.Rows[rowNo]["DispatchedThroughOrTransporter"] = purchaseSummuryReportTO.TransportorName;
                        GSTPurchaseReportDT.Rows[rowNo]["BillofLadingOrLR-RRNo"] = purchaseSummuryReportTO.LrNo;
                        GSTPurchaseReportDT.Rows[rowNo]["LRDate"] = purchaseSummuryReportTO.LrDate;
                        GSTPurchaseReportDT.Rows[rowNo]["MotorVehicleNo"] = purchaseSummuryReportTO.VehicleNo; //
                        GSTPurchaseReportDT.Rows[rowNo]["CustomerPhoneNO"] = purchaseSummuryReportTO.SupplierMobNo;
                        GSTPurchaseReportDT.Rows[rowNo]["BrokerPhoneNo"] = purchaseSummuryReportTO.BrokerMobNo;
                        //GSTPurchaseReportDT.Rows[rowNo]["DriverPhoneNo"] = purchaseSummuryReportTO.TransporterMobNo;
                        GSTPurchaseReportDT.Rows[rowNo]["FreightValue"] = Convert.ToDouble(purchaseSummuryReportTO.FreightAmt);
                        GSTPurchaseReportDT.Rows[rowNo]["TallyRefIdParty"] = purchaseSummuryReportTO.TallyRefId;
                        GSTPurchaseReportDT.Rows[rowNo]["SupplierName"] = purchaseSummuryReportTO.OrgSupplierName;
                        GSTPurchaseReportDT.Rows[rowNo]["TCSAmt"] = purchaseSummuryReportTO.TCSAmt;


                    }
                    GSTPurchaseReportDT.TableName = "GSTPurchaseReportDT";
                    printDataSet.Tables.Add(GSTPurchaseReportDT);
                    string templateName = "RecycleGSTPurchase";
                    String templateFilePath = _iDimReportTemplateBL.SelectReportFullName(templateName);
                    String fileName = "RecycleGSTPurchase-" + DateTime.Now.Ticks;
                    String saveLocation = AppDomain.CurrentDomain.BaseDirectory + fileName + ".xls";
                    Boolean IsProduction = true;

                    resultMessage = _iRunReport.GenrateMktgInvoiceReport(printDataSet, templateFilePath, saveLocation, Constants.ReportE.EXCEL_DONT_OPEN, IsProduction);

                    if (resultMessage.MessageType == ResultMessageE.Information)
                    {
                        String filePath = String.Empty;
                        if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(String))
                        {
                            filePath = resultMessage.Tag.ToString();
                        }
                        String fileName1 = Path.GetFileName(saveLocation);
                        Byte[] bytes = File.ReadAllBytes(filePath);
                        if (bytes != null && bytes.Length > 0)
                        {
                            resultMessage.Tag = bytes;
                            string resFname = Path.GetFileNameWithoutExtension(saveLocation);
                            string directoryName;
                            directoryName = Path.GetDirectoryName(saveLocation);
                            string[] fileEntries = Directory.GetFiles(directoryName, "*" + fileName + "*");
                            string[] filesList = Directory.GetFiles(directoryName, "*" + fileName + "*");


                            foreach (string file in filesList)
                            {
                                //if (file.ToUpper().Contains(resFname.ToUpper()))
                                {
                                    File.Delete(file);
                                }
                            }
                        }
                        if (resultMessage.MessageType == ResultMessageE.Information)
                        {
                            resultMessage.DefaultSuccessBehaviour();
                        }
                    }





                    return resultMessage;
                }
                else
                {
                    //errorMsg = "No data found between from" + fromDate + " to " + toDate;
                    resultMessage.MessageType = ResultMessageE.None;
                    resultMessage.DefaultSuccessBehaviour("No data found between from" + fromDate + " to " + toDate);
                }
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.DefaultExceptionBehaviour(ex, "PrintPurchaseSummaryReport");
                return resultMessage;
            }
            return resultMessage;
        }

        public ResultMessage PrintSaudaReport(string fromDate, string toDate,String purchaseMangerIds)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                DataSet printDataSet = new DataSet();
                DataTable headerDT = new DataTable();
                
                List<TblPurchaseEnquiryTO> saudaReportToList = GetSaudaReportList(fromDate, toDate, purchaseMangerIds);
                if (saudaReportToList != null && saudaReportToList.Count > 0)
                {
                    headerDT = _iCommonDao.ToDataTable(saudaReportToList);
                }

                headerDT.TableName = "headerDT";
                printDataSet.Tables.Add(headerDT);
                String ReportTemplateName = Constants.SAUDA_REPORT_TEMPLATE;

                String templateFilePath = _iDimReportTemplateBL.SelectReportFullName(ReportTemplateName);
                String fileName = "Doc-" + DateTime.Now.Ticks;

                //download location for rewrite  template file
                String saveLocation = AppDomain.CurrentDomain.BaseDirectory + fileName + ".xls";
                // RunReport runReport = new RunReport();
                Boolean IsProduction = true;

                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName("IS_PRODUCTION_ENVIRONMENT_ACTIVE");
                if (tblConfigParamsTO != null)
                {
                    if (Convert.ToInt32(tblConfigParamsTO.ConfigParamVal) == 0)
                    {
                        IsProduction = false;
                    }
                }
                resultMessage = _iRunReport.GenrateMktgInvoiceReport(printDataSet, templateFilePath, saveLocation, Constants.ReportE.EXCEL_DONT_OPEN, IsProduction);
                if (resultMessage.MessageType == ResultMessageE.Information)
                {
                    String filePath = String.Empty;
                    if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(String))
                    {

                        filePath = resultMessage.Tag.ToString();
                    }
                    //driveName + path;
                    int returnPath = 0;
                    if (returnPath != 1)
                    {
                        String fileName1 = Path.GetFileName(saveLocation);
                        Byte[] bytes = File.ReadAllBytes(filePath);
                        if (bytes != null && bytes.Length > 0)
                        {
                            resultMessage.Tag = bytes;

                            string resFname = Path.GetFileNameWithoutExtension(saveLocation);
                            string directoryName;
                            directoryName = Path.GetDirectoryName(saveLocation);
                            string[] fileEntries = Directory.GetFiles(directoryName, "*Doc*");
                            string[] filesList = Directory.GetFiles(directoryName, "*Doc*");

                            foreach (string file in filesList)
                            {
                                //if (file.ToUpper().Contains(resFname.ToUpper()))
                                {
                                    File.Delete(file);
                                }
                            }
                        }

                        if (resultMessage.MessageType == ResultMessageE.Information)
                        {
                            resultMessage.DefaultSuccessBehaviour();
                        }
                    }

                }
                else
                {
                    resultMessage.Text = "Something wents wrong please try again";
                    resultMessage.DisplayMessage = "Something wents wrong please try again";
                    resultMessage.Result = 0;
                }
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
            finally
            {

            }
        }


        public string GetGradesAgainstScheduleSummaryId(Int32 IdPurchaseEnquiry, Int32 StateId)
        {
            string grades = "";
            List<TblPurchaseEnquiryDetailsTO> tblEnquiryDetailsTOList = _iTblPurchaseEnquiryDetailsDAO.SelectAllTblEnquiryDetailsList(IdPurchaseEnquiry, StateId);
            if (tblEnquiryDetailsTOList != null && tblEnquiryDetailsTOList.Count > 0)
            {
                for (int i = 0; i < tblEnquiryDetailsTOList.Count; i++)
                {
                    if (i == 0)
                    {
                        grades = tblEnquiryDetailsTOList[i].ItemName;
                    }
                    else
                    {
                        grades = grades + "," + tblEnquiryDetailsTOList[i].ItemName;
                    }
                }
            }
            return grades;
        }
        public List<TblPurchaseEnquiryTO> GetSaudaReportList(string fromDate, string toDate,String purchaseMangerIds)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                DateTime from_Date = DateTime.MinValue;
                DateTime to_Date = DateTime.MinValue;
                if (Constants.IsDateTime(fromDate))
                    from_Date = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
                if (Constants.IsDateTime(toDate))
                    to_Date = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));

                List<TblPurchaseEnquiryTO> saudaReportToList = _iDimReportTemplateBL.GetAllEnquiryList(from_Date, to_Date, purchaseMangerIds);
                //if (saudaReportToList != null && saudaReportToList.Count > 0)
                //{
                //    for (int x = 0; x < saudaReportToList.Count; x++)
                //    {
                //        saudaReportToList[x].Grades = GetGradesAgainstScheduleSummaryId(saudaReportToList[x].IdPurchaseEnquiry, saudaReportToList[x].StateId);
                //    }
                //}               

                return saudaReportToList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return null;
            }
            finally
            {

            }
        }

        //Harshala [28-08-2020] added to print weighment slip report on template
        public ResultMessage PrintWeighmentSlipTemplateReport(Int32 purchaseScheduleId)
        {
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                DataSet printDataSet = new DataSet();
                DataTable PurchaseWeighingStageSummaryReportDT = new DataTable();
                DataTable PurchaseScheduleSummaryTODT = new DataTable();
                DataTable WeightsDT = new DataTable();
                DataTable PartyWeightDT = new DataTable();
                List<TblPurchaseWeighingStageSummaryTO> tblPurchaseWeighingStageSummaryTOList = _iTblPurchaseWeighingStageSummaryBL.GetVehicleWeighingDetailsBySchduleIdForWeighingReport(purchaseScheduleId, true);
                //List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryDAO.SelectTblPurchaseScheduleSummaryDetailsList(idSchedulePurchaseSummary, purchaseScheduleId);

                List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryDAO.SelectAllEnquiryScheduleSummaryTOByRootID(purchaseScheduleId,true);
                if (tblPurchaseWeighingStageSummaryTOList != null && tblPurchaseWeighingStageSummaryTOList.Count > 0)
                {
                    if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count > 0)
                    {
                        PurchaseScheduleSummaryTODT = _iCommonDao.ToDataTable(tblPurchaseScheduleSummaryTOList);
                    }

                    WeightsDT.Columns.Add("GrossWt", typeof(double));
                    WeightsDT.Columns.Add("GrossWtDate", typeof(string));

                    WeightsDT.Columns.Add("NetWt", typeof(double));
                    WeightsDT.Columns.Add("TareWt", typeof(double));
                    WeightsDT.Columns.Add("TareWtDate", typeof(string));
                    WeightsDT.Columns.Add("Difference", typeof(string));
                    WeightsDT.Columns.Add("TotalNetWt", typeof(double));

                    PartyWeightDT.Columns.Add("PartyGrossWt", typeof(double));
                    PartyWeightDT.Columns.Add("PartyNetWt", typeof(double));
                    PartyWeightDT.Columns.Add("PartyTareWt", typeof(double));

                    //add rows to Weight DT
                    WeightsDT.Rows.Add();
                    int rowNo = WeightsDT.Rows.Count - 1;
                    var grossWeight = tblPurchaseWeighingStageSummaryTOList.Where(p => p.WeightMeasurTypeId == (int)Constants.TransMeasureTypeE.GROSS_WEIGHT).ToList();
                    if (grossWeight != null && grossWeight.Count > 0)
                        WeightsDT.Rows[rowNo]["GrossWt"] = grossWeight[0].GrossWeightMT;

                    WeightsDT.Rows[rowNo]["GrossWtDate"] = grossWeight[0].CreatedOn.ToShortDateString();

                    var tareWeight = tblPurchaseWeighingStageSummaryTOList.Where(p => p.WeightMeasurTypeId == (int)Constants.TransMeasureTypeE.TARE_WEIGHT).ToList();
                    if (tareWeight != null && tareWeight.Count > 0)
                        WeightsDT.Rows[rowNo]["TareWt"] = tareWeight[0].ActualWeightMT;
                    WeightsDT.Rows[rowNo]["TareWtDate"] = tareWeight[0].CreatedOn.ToShortDateString();

                    Double NetWeight = 0;
                    Double Difference = 0;

                    if (grossWeight != null && tareWeight != null)
                    {
                        NetWeight = grossWeight[0].GrossWeightMT - tareWeight[0].ActualWeightMT;
                        if (tblPurchaseWeighingStageSummaryTOList[0].PartyNetWt != 0)
                            Difference = NetWeight - tblPurchaseWeighingStageSummaryTOList[0].PartyNetWt;
                        else
                            Difference = 0;

                    }

                    WeightsDT.Rows[rowNo]["NetWt"] = NetWeight;

                    if (Difference != 0)
                        WeightsDT.Rows[rowNo]["Difference"] = Difference;
                    else
                        WeightsDT.Rows[rowNo]["Difference"] = " ";


                    //Add rows to Party Weight DT

                    PartyWeightDT.Rows.Add();
                    int rowNoPartywt = PartyWeightDT.Rows.Count - 1;
                    PartyWeightDT.Rows[rowNoPartywt]["PartyGrossWt"] = tblPurchaseWeighingStageSummaryTOList[0].PartyGrossWt;
                    PartyWeightDT.Rows[rowNoPartywt]["PartyNetWt"] = tblPurchaseWeighingStageSummaryTOList[0].PartyNetWt;
                    PartyWeightDT.Rows[rowNoPartywt]["PartyTareWt"] = tblPurchaseWeighingStageSummaryTOList[0].PartyTareWt;


                    tblPurchaseWeighingStageSummaryTOList = tblPurchaseWeighingStageSummaryTOList.Where(e => e.WeightMeasurTypeId == (int)Constants.TransMeasureTypeE.INTERMEDIATE_WEIGHT).ToList();
                    PurchaseWeighingStageSummaryReportDT = _iCommonDao.ToDataTable(tblPurchaseWeighingStageSummaryTOList);
                    Double totalNetWt = 0;
                    tblPurchaseWeighingStageSummaryTOList.ForEach(e =>
                    {
                        totalNetWt += e.NetWeightMT;
                    });
                    WeightsDT.Rows[rowNo]["TotalNetWt"] = totalNetWt;

                    PurchaseWeighingStageSummaryReportDT.TableName = "PurchaseWeighingStageSummaryReportDT";
                    PurchaseScheduleSummaryTODT.TableName = "PurchaseScheduleSummaryTODT";
                    WeightsDT.TableName = "WeightsDT";
                    PartyWeightDT.TableName = "PartyWeightDT";

                    printDataSet.Tables.Add(PurchaseWeighingStageSummaryReportDT);
                    printDataSet.Tables.Add(PurchaseScheduleSummaryTODT);
                    printDataSet.Tables.Add(WeightsDT);
                    printDataSet.Tables.Add(PartyWeightDT);

                    string templateName = "WeightmentSlipReport";
                    String templateFilePath = _iDimReportTemplateBL.SelectReportFullName(templateName);
                    String fileName = "WeightmentSlipReport-" + DateTime.Now.Ticks;
                    String saveLocation = AppDomain.CurrentDomain.BaseDirectory + fileName + ".xls";
                    Boolean IsProduction = true;

                    resultMessage = _iRunReport.GenrateMktgInvoiceReport(printDataSet, templateFilePath, saveLocation, Constants.ReportE.PDF_DONT_OPEN, IsProduction);

                    if (resultMessage.MessageType == ResultMessageE.Information)
                    {
                        String filePath = String.Empty;
                        if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(String))
                        {
                            filePath = resultMessage.Tag.ToString();
                        }
                        String fileName1 = Path.GetFileName(saveLocation);
                        Byte[] bytes = File.ReadAllBytes(filePath);
                        if (bytes != null && bytes.Length > 0)
                        {
                            resultMessage.Tag = bytes;
                            string resFname = Path.GetFileNameWithoutExtension(saveLocation);
                            string directoryName;
                            directoryName = Path.GetDirectoryName(saveLocation);
                            string[] fileEntries = Directory.GetFiles(directoryName, "*WeightmentSlipReport*");
                            string[] filesList = Directory.GetFiles(directoryName, "*WeightmentSlipReport*");


                            foreach (string file in filesList)
                            {
                                //if (file.ToUpper().Contains(resFname.ToUpper()))
                                {
                                    File.Delete(file);
                                }
                            }
                        }
                        if (resultMessage.MessageType == ResultMessageE.Information)
                        {
                            resultMessage.DefaultSuccessBehaviour();
                        }
                    }

                    return resultMessage;
                }
                else
                {
                    //errorMsg = "No data found between from" + fromDate + " to " + toDate;
                    resultMessage.MessageType = ResultMessageE.None;
                    resultMessage.DefaultSuccessBehaviour("No data found for" + purchaseScheduleId);
                }
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.DefaultExceptionBehaviour(ex, "PrintWeighmentSlipTemplateReport");
                return resultMessage;
            }
            return resultMessage;
        }

        public List<SaudaReportTo> GetSaudaChartReport(string fromDate, string toDate, Int32 PmId, Int32 SupplierId)
        {
            DateTime from_Date = DateTime.MinValue;
            DateTime to_Date = DateTime.MinValue;
            if (Constants.IsDateTime(fromDate))
                from_Date = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
            if (Constants.IsDateTime(toDate))
                to_Date = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));
            List<SaudaReportTo> saudaReportToList = _ireportDAO.GetSaudaChartReport(from_Date, to_Date, PmId, SupplierId);
            List<SaudaReportTo> saudaReportToListComplete = _ireportDAO.GetSaudaChartReportComplete(from_Date, to_Date, PmId, SupplierId);
            if (saudaReportToListComplete != null && saudaReportToListComplete.Count > 0)
            {
                foreach (var item in saudaReportToListComplete)
                {
                    saudaReportToList.Add(item);
                }
            }

            saudaReportToList = saudaReportToList.OrderByDescending(x => x.EnquiryId).ToList();
            SaudaReportTo saudaReportTOTotal = new SaudaReportTo();
            if (saudaReportToList != null && saudaReportToList.Count > 0)
            {
                saudaReportTOTotal.TodaysUnloadingQty = 0;
                saudaReportTOTotal.OpeningSaudaQty = 0;
                saudaReportTOTotal.ClosingSaudaQty = 0;
                saudaReportTOTotal.OrgSaudaQty = 0;
                saudaReportTOTotal.PurchaseManager = "Total";
                foreach (var saudaReportTO in saudaReportToList)
                {

                    if (saudaReportTO.OrgSaudaQty > 0)
                    {
                        saudaReportTOTotal.OrgSaudaQty += saudaReportTO.OrgSaudaQty;
                    }
                    double totalQty = _ireportDAO.GetTotalUnloadedQty(saudaReportTO.EnquiryId);
                    if (totalQty != null)
                    {
                        saudaReportTO.TodayConfirmedUnloadQty = totalQty;
                    }
                    double totalScheduledQty = _ireportDAO.GetTotalScheduledQty(saudaReportTO.EnquiryId);
                    if (totalScheduledQty != null)
                    {
                        saudaReportTO.TodaysUnloadingQty = totalScheduledQty;
                        saudaReportTOTotal.TodaysUnloadingQty += totalScheduledQty;
                    }
                    // double closingQty = _ireportDAO.GetClosingQty(saudaReportTO.EnquiryId);
                    double closingQty = saudaReportTO.ClosingSaudaQty;
                    if (closingQty != null)
                    {
                        saudaReportTO.ClosingSaudaQty = closingQty;
                        saudaReportTOTotal.ClosingSaudaQty += closingQty;
                    }
                    // double openingQty = _ireportDAO.GetOpeningQty(saudaReportTO.EnquiryId);
                    double openingQty = _ireportDAO.GetOpeningScheduledQty(saudaReportTO.EnquiryId);

                    if (openingQty != null && openingQty > 0)
                    {
                        saudaReportTO.OpeningSaudaQty = openingQty;
                        saudaReportTOTotal.OpeningSaudaQty += openingQty;
                    }
                    else
                    {
                        saudaReportTO.OpeningSaudaQty = saudaReportTO.ClosingSaudaQty + saudaReportTO.TodaysUnloadingQty;
                    }
                }

                saudaReportToList.Add(saudaReportTOTotal);

            }

            return saudaReportToList;
        }

        //chetan[2019-11-01] added
        public List<GradeWiseWnloadingReportTO> SelectGradeWiseWnloadingReport(DateTime fromDate, DateTime toDate, int confirmTypeId,String purchaseManagerIds)
        {
            return _ireportDAO.SelectGradeWiseWnloadingReport(fromDate, toDate, confirmTypeId, purchaseManagerIds);
        }

        //List<PartyWiseUnldReportTO> GetPartyWiseUnldReport(TblPurSchSummaryFilterTO tblPurSchSummaryFilterTO)
        //{
        //    return _ireportDAO.GetPartyWiseUnldReport(tblPurSchSummaryFilterTO);
        //}

        public List<PartyWiseWeighingCompaReportTO> GetPartyWeightComparisonReport(DateTime fromDate, DateTime toDate,String purchaseManagerIds)
        {
            List<PartyWiseWeighingCompaReportTO> partyWeightList = new List<PartyWiseWeighingCompaReportTO>();
            Int32 isGradingBeforeUnloading = 0;
            double conversionFact = 1000;

            Int32 statusId = (Int32)Constants.TranStatusE.UNLOADING_COMPLETED;

            List<TblPurchaseWeighingStageSummaryTO> weighingList = _ireportDAO.GetPartyWeightComparisonReport(fromDate, toDate, statusId, purchaseManagerIds);
            if (weighingList == null)
                weighingList = new List<TblPurchaseWeighingStageSummaryTO>();
            int weightSourceConfigId = _iTblConfigParamsDAO.IoTSetting();
            if (weightSourceConfigId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
            {
                statusId = Convert.ToInt32((int)Constants.TranStatusE.New);

                List<TblPurchaseWeighingStageSummaryTO> tempList = _ireportDAO.GetPartyWeightComparisonReport(fromDate, toDate, statusId, purchaseManagerIds);
                if (tempList != null && tempList.Count > 0)
                {
                    List<int> purchaseScheduleSummaryIdList = tempList.Select(x => x.PurchaseScheduleSummaryId).Distinct().ToList();
                    for (int i = 0; i < purchaseScheduleSummaryIdList.Count; i++)
                    {
                        List<TblPurchaseWeighingStageSummaryTO> WeighingStageSummaryTOList = tempList.Where(w => w.PurchaseScheduleSummaryId == purchaseScheduleSummaryIdList[i]).ToList();
                        TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = _iTblPurchaseScheduleSummaryBL.SelectTblPurchaseScheduleSummaryDtlsTO(purchaseScheduleSummaryIdList[i], 0);
                        _iIotCommunication.GetItemDataFromIotAndMerge(tblPurchaseScheduleSummaryTO);

                        for (int j = 0; j < WeighingStageSummaryTOList.Count; j++)
                        {
                            WeighingStageSummaryTOList[j].VehicleNo = tblPurchaseScheduleSummaryTO.VehicleNo;
                        }
                        //  tempList[i].VehicleNo = tblPurchaseScheduleSummaryTO.VehicleNo;      
                        WeighingStageSummaryTOList = _iIotCommunication.GetWeightDataFromIotAndMerge(tblPurchaseScheduleSummaryTO, WeighingStageSummaryTOList);
                        weighingList.AddRange(WeighingStageSummaryTOList);
                    }
                    // weighingList.AddRange(tempList);
                }
            }

            if (weighingList != null && weighingList.Count > 0)
            {
                PartyWiseWeighingCompaReportTO totalTO = new PartyWiseWeighingCompaReportTO();

                List<Int32> purchaseScheList = new List<int>();
                purchaseScheList = weighingList.Select(x => x.PurchaseScheduleSummaryId).Distinct().ToList();

                if (purchaseScheList != null && purchaseScheList.Count > 0)
                {
                    for (int i = 0; i < purchaseScheList.Count; i++)
                    {
                        Int32 scheduleId = purchaseScheList[i];
                        double dustQty = 0;
                        PartyWiseWeighingCompaReportTO partyWiseWeighingCompaReportTO = new PartyWiseWeighingCompaReportTO();

                        List<TblPurchaseWeighingStageSummaryTO> tempWeighingList = weighingList.Where(a => a.PurchaseScheduleSummaryId == scheduleId).ToList();

                        if (tempWeighingList != null && tempWeighingList.Count > 0)
                        {
                            partyWiseWeighingCompaReportTO.TruckNo = tempWeighingList[0].VehicleNo;
                            partyWiseWeighingCompaReportTO.PartyName = tempWeighingList[0].SupplierName;
                            partyWiseWeighingCompaReportTO.Freight = tempWeighingList[0].Freight;
                            //partyWiseWeighingCompaReportTO.TotalFreight = 
                            partyWiseWeighingCompaReportTO.Advance = tempWeighingList[0].Advance;
                            partyWiseWeighingCompaReportTO.UnloadingQty = tempWeighingList[0].UnloadingQty;
                            partyWiseWeighingCompaReportTO.Shortage = tempWeighingList[0].Shortage;
                            partyWiseWeighingCompaReportTO.Amount = tempWeighingList[0].Amount;
                            partyWiseWeighingCompaReportTO.IsValidInfo = tempWeighingList[0].IsValidInfo;
                            partyWiseWeighingCompaReportTO.IsStorageExcess = tempWeighingList[0].IsStorageExcess;
                            partyWiseWeighingCompaReportTO.PartyTareWt = tempWeighingList[0].PartyTareWt;
                            partyWiseWeighingCompaReportTO.PartyTareWt = partyWiseWeighingCompaReportTO.PartyTareWt / conversionFact;

                            partyWiseWeighingCompaReportTO.PartyNetWt = tempWeighingList[0].PartyNetWt;
                            partyWiseWeighingCompaReportTO.PartyNetWt = partyWiseWeighingCompaReportTO.PartyNetWt / conversionFact;
                            partyWiseWeighingCompaReportTO.PartyGrossWt = tempWeighingList[0].PartyGrossWt;
                            partyWiseWeighingCompaReportTO.PartyGrossWt = partyWiseWeighingCompaReportTO.PartyGrossWt / conversionFact;

                            //Int32 tempStatusId = (Int32)Constants.TranStatusE.UNLOADING_COMPLETED;

                            //List<TblPurchaseScheduleSummaryTO> TblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryBL.SelectVehicleScheduleByRootAndStatusId(tempWeighingList[0].PurchaseScheduleSummaryId, tempStatusId, (int)StaticStuff.Constants.PurchaseVehiclePhasesE.CORRECTIONS);
                            List<TblPurchaseScheduleSummaryTO> TblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryBL.SelectTblPurchaseScheduleSummaryDetails(tempWeighingList[0].CorrectionRecId, false);
                            if (TblPurchaseScheduleSummaryTOList != null && TblPurchaseScheduleSummaryTOList.Count == 1)
                            {
                                TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = TblPurchaseScheduleSummaryTOList[0];
                                List<TblPurchaseVehicleDetailsTO> tblPurchaseVehicleDetailsTOList = _iTblPurchaseVehicleDetailsBL.SelectAllTblPurchaseVehicleDtlsList(tblPurchaseScheduleSummaryTO.IdPurchaseScheduleSummary);
                                if (tblPurchaseVehicleDetailsTOList != null && tblPurchaseVehicleDetailsTOList.Count > 0)
                                {
                                    tblPurchaseVehicleDetailsTOList = tblPurchaseVehicleDetailsTOList.Where(w => w.IsNonCommercialItem == 1).ToList();
                                    if (tblPurchaseVehicleDetailsTOList != null && tblPurchaseVehicleDetailsTOList.Count > 0)
                                    {
                                        dustQty = tblPurchaseVehicleDetailsTOList.Sum(a => a.Qty);
                                        dustQty = Math.Round(dustQty, 3);
                                    }
                                }
                            }
                            //List<TblPurchaseUnloadingDtlTO> unloadingDtlTOList = _iTblPurchaseUnloadingDtlBL.SelectAllTblPurchaseUnloadingDtlListByScheduleId(tempWeighingList[0].PurchaseScheduleSummaryId, isGradingBeforeUnloading);
                            //if (unloadingDtlTOList != null && unloadingDtlTOList.Count > 0)
                            //{
                            //    unloadingDtlTOList = unloadingDtlTOList.Where(a => a.IsNextUnldGrade == 0 && a.IsNonCommercialItem == 1).ToList();
                            //    if (unloadingDtlTOList != null && unloadingDtlTOList.Count > 0)
                            //    {
                            //        dustQty = unloadingDtlTOList.Sum(a => a.QtyMT);
                            //        dustQty = Math.Round(dustQty, 3);
                            //    }
                            //}
                            double impurities = _iTblPurchaseScheduleSummaryBL.GetWeighingStageWiseSumOfImpurities(tempWeighingList[0].PurchaseScheduleSummaryId, string.Empty);
                            partyWiseWeighingCompaReportTO.Dust = dustQty;
                            partyWiseWeighingCompaReportTO.Impurities = impurities;
                            List<TblPurchaseWeighingStageSummaryTO> grossWtList = tempWeighingList.Where(a => a.WeightMeasurTypeId == (Int32)Constants.TransMeasureTypeE.GROSS_WEIGHT).ToList();
                            if (grossWtList != null && grossWtList.Count > 0)
                            {
                                partyWiseWeighingCompaReportTO.SrjGrossWt = grossWtList[0].GrossWeightMT;
                                partyWiseWeighingCompaReportTO.SrjGrossWt = partyWiseWeighingCompaReportTO.SrjGrossWt / conversionFact;
                            }

                            List<TblPurchaseWeighingStageSummaryTO> netWtList = tempWeighingList.Where(a => a.WeightMeasurTypeId == (Int32)Constants.TransMeasureTypeE.INTERMEDIATE_WEIGHT).ToList();
                            if (netWtList != null && netWtList.Count > 0)
                            {
                                partyWiseWeighingCompaReportTO.SrjNetWt = netWtList.Sum(a => a.NetWeightMT);
                                partyWiseWeighingCompaReportTO.SrjNetWt = partyWiseWeighingCompaReportTO.SrjNetWt / conversionFact;
                            }

                            List<TblPurchaseWeighingStageSummaryTO> tareWtList = tempWeighingList.Where(a => a.WeightMeasurTypeId == (Int32)Constants.TransMeasureTypeE.TARE_WEIGHT).ToList();
                            if (tareWtList != null && tareWtList.Count > 0)
                            {
                                partyWiseWeighingCompaReportTO.SrjTareWt = tareWtList[0].ActualWeightMT;
                                partyWiseWeighingCompaReportTO.SrjTareWt = partyWiseWeighingCompaReportTO.SrjTareWt / conversionFact;
                            }

                            if (partyWiseWeighingCompaReportTO.IsValidInfo == "Yes")
                            {
                                partyWiseWeighingCompaReportTO.SrjActualWt = partyWiseWeighingCompaReportTO.PartyNetWt - dustQty;
                            }
                            else
                            {
                                partyWiseWeighingCompaReportTO.SrjActualWt = partyWiseWeighingCompaReportTO.SrjNetWt - dustQty;
                            }
                            //partyWiseWeighingCompaReportTO.SrjActualWt = partyWiseWeighingCompaReportTO.SrjActualWt / conversionFact;
                            partyWiseWeighingCompaReportTO.SrjActualWt = Math.Round(partyWiseWeighingCompaReportTO.SrjActualWt, 3);

                            double actualShort = 0;
                            if (partyWiseWeighingCompaReportTO.PartyNetWt > 0)
                            {
                                //actualShort = partyWiseWeighingCompaReportTO.PartyNetWt - partyWiseWeighingCompaReportTO.SrjActualWt;
                                actualShort = partyWiseWeighingCompaReportTO.PartyNetWt - partyWiseWeighingCompaReportTO.SrjNetWt;


                                //actualShort = actualShort / conversionFact;
                                actualShort = Math.Round(actualShort, 3);
                            }
                            if (actualShort > 0)
                            {
                                partyWiseWeighingCompaReportTO.IsStorageExcess = "Yes";
                                partyWiseWeighingCompaReportTO.PossitiveDiff = actualShort;
                            }
                            else
                            {
                                partyWiseWeighingCompaReportTO.IsStorageExcess = "No";
                                partyWiseWeighingCompaReportTO.NegativeDiff = Math.Abs(actualShort);
                            }
                        }
                        partyWiseWeighingCompaReportTO.IsTotal = 0;
                        partyWeightList.Add(partyWiseWeighingCompaReportTO);                        
                    }
                }
                double totalPartyTareWt = 0, totalPartyNetWt = 0, totalPartyGrossWt = 0, totalSrjNetWt = 0, totalSrjGrossWt = 0, totalSrjActualWt = 0, totalpossitiveDiff = 0;
                double totalSrjTareWt = 0, totalNegativeDiff = 0, totalDust = 0, totalImpurities = 0, totalFreight = 0, totalAdvance = 0, totalUnloadingQty = 0, totalShortage = 0, totalAmount = 0;
                if (partyWeightList != null && partyWeightList.Count > 0)
                {
                    totalTO.Total = "Grand Total";

                    totalPartyTareWt = partyWeightList.Sum(a => a.PartyTareWt);
                    totalTO.TotalPartyTareWt = Math.Round(totalPartyTareWt, 3);

                    totalPartyNetWt = partyWeightList.Sum(a => a.PartyNetWt);
                    totalTO.TotalPartyNetWt = Math.Round(totalPartyNetWt,3);

                    totalPartyGrossWt = partyWeightList.Sum(a => a.PartyGrossWt);
                    totalTO.TotalPartyGrossWt = Math.Round(totalPartyGrossWt,3);

                    totalSrjTareWt = partyWeightList.Sum(a => a.SrjTareWt);
                    totalTO.TotalSrjTareWt = Math.Round(totalSrjTareWt, 3);

                    totalSrjNetWt = partyWeightList.Sum(a => a.SrjNetWt);
                    totalTO.TotalSrjNetWt = Math.Round(totalSrjNetWt,3);

                    totalSrjGrossWt = partyWeightList.Sum(a => a.SrjGrossWt);
                    totalTO.TotalSrjGrossWt = Math.Round(totalSrjGrossWt,3);

                    totalSrjActualWt = partyWeightList.Sum(a => a.SrjActualWt);
                    totalTO.TotalSrjActualWt = Math.Round(totalSrjActualWt,3);

                    totalpossitiveDiff = partyWeightList.Sum(a => a.PossitiveDiff);
                    totalTO.TotalpossitiveDiff = Math.Round(totalpossitiveDiff,3);

                    totalNegativeDiff = partyWeightList.Sum(a => a.NegativeDiff);
                    totalTO.TotalNegativeDiff = Math.Round(totalNegativeDiff,3);

                    totalDust = partyWeightList.Sum(a => a.Dust);
                    totalTO.TotalDust = Math.Round(totalDust,3);

                    totalImpurities = partyWeightList.Sum(a => a.Impurities);
                    totalTO.TotalImpurities = Math.Round(totalImpurities,3);

                    totalFreight = partyWeightList.Sum(a => a.Freight);
                    totalTO.TotalFreight = Math.Round(totalFreight,3);

                    totalAdvance = partyWeightList.Sum(a => a.Advance);
                    totalTO.TotalAdvance = Math.Round(totalAdvance,3);

                    totalUnloadingQty = partyWeightList.Sum(a => a.UnloadingQty);
                    totalTO.TotalUnloadingQty = Math.Round(totalUnloadingQty, 3);

                    totalShortage = partyWeightList.Sum(a => a.Shortage);
                    totalTO.TotalShortage = Math.Round(totalShortage,3);

                    totalAmount = partyWeightList.Sum(a => a.Amount);
                    totalTO.TotalAmount = Math.Round(totalAmount,3);

                    totalTO.IsTotal = 1;
                    partyWeightList.Add(totalTO);
                }
            }

            return partyWeightList;
        }
        public ResultMessage PrintUnlodingTimeRport(string fromDate, string toDate, Int32 isForWeighingPointWise,String purchaseManagerIds)
        {
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                DataTable printUnlodingTimeReportDT = new DataTable();
                DataTable FooterReportDT = new DataTable();
                FooterReportDT.Columns.Add("avgTotalUnloadedTime", typeof(double));
                printUnlodingTimeReportDT.Columns.Add("SrNo", typeof(int));
                printUnlodingTimeReportDT.Columns.Add("vehicleNo", typeof(string));
                printUnlodingTimeReportDT.Columns.Add("unloadingPointName", typeof(string));
                printUnlodingTimeReportDT.Columns.Add("graderName", typeof(string));
                printUnlodingTimeReportDT.Columns.Add("avgUnloadingTime", typeof(double));
                printUnlodingTimeReportDT.Columns.Add("partyName", typeof(string));
                printUnlodingTimeReportDT.Columns.Add("gradeUnloaded", typeof(string));
                printUnlodingTimeReportDT.Columns.Add("quantityUnloaded", typeof(string));
                double totalUnloadedTime = 0;
               
                List<UnloadedTimeReportTO> unloadedTimeReportTOList = _ITblPurchaseWeighingStageSummaryBL.UnlodingTimeRport(fromDate, toDate, isForWeighingPointWise, true, purchaseManagerIds);
                if (unloadedTimeReportTOList != null && unloadedTimeReportTOList.Count > 0)
                {

                    totalUnloadedTime = unloadedTimeReportTOList.Sum(s => s.AvgUnloadingTime);
                    if (totalUnloadedTime > 0)
                    {
                        totalUnloadedTime = Math.Round(totalUnloadedTime / unloadedTimeReportTOList.Count, 2);
                        FooterReportDT.Rows.Add();
                        FooterReportDT.Rows[0][0] = totalUnloadedTime;
                    }
                    for (int i = 0; i < unloadedTimeReportTOList.Count; i++)
                    {
                        UnloadedTimeReportTO unloadedTimeReportTO = unloadedTimeReportTOList[i];
                        printUnlodingTimeReportDT.Rows.Add();
                        printUnlodingTimeReportDT.Rows[i]["SrNo"] = i + 1;

                        printUnlodingTimeReportDT.Rows[i]["vehicleNo"] = unloadedTimeReportTO.VehicleNo;
                        printUnlodingTimeReportDT.Rows[i]["unloadingPointName"] = unloadedTimeReportTO.UnloadingPointName;
                        printUnlodingTimeReportDT.Rows[i]["graderName"] = unloadedTimeReportTO.GraderName;
                        printUnlodingTimeReportDT.Rows[i]["avgUnloadingTime"] = unloadedTimeReportTO.AvgUnloadingTime;
                        printUnlodingTimeReportDT.Rows[i]["partyName"] = unloadedTimeReportTO.PartyName;
                        printUnlodingTimeReportDT.Rows[i]["gradeUnloaded"] = unloadedTimeReportTO.GradeUnloaded;
                        printUnlodingTimeReportDT.Rows[i]["quantityUnloaded"] = unloadedTimeReportTO.QuantityUnloaded;
                    }
                    DataSet printDataSet = new DataSet();
                    printUnlodingTimeReportDT.TableName = "printUnlodingTimeReportDT";
                    FooterReportDT.TableName = "FooterReportDT";
                    printDataSet.Tables.Add(printUnlodingTimeReportDT);
                    printDataSet.Tables.Add(FooterReportDT);
                    string templateName = string.Empty;
                    String templateFilePath = string.Empty;
                    String fileName = string.Empty;
                    if (isForWeighingPointWise == 1)
                    {
                        templateName = "UnloadingPointReport";
                        templateFilePath = _iDimReportTemplateBL.SelectReportFullName(templateName);
                        fileName = "UnloadingPointReport-" + DateTime.Now.Ticks;
                    }
                    else
                    {
                        templateName = "VehicleUnloadingPointReport";
                        templateFilePath = _iDimReportTemplateBL.SelectReportFullName(templateName);
                        fileName = "UnloadingPointReport-" + DateTime.Now.Ticks;
                    }

                    String saveLocation = AppDomain.CurrentDomain.BaseDirectory + fileName + ".xls";
                    Boolean IsProduction = true;

                    resultMessage = _iRunReport.GenrateMktgInvoiceReport(printDataSet, templateFilePath, saveLocation, Constants.ReportE.EXCEL_DONT_OPEN, IsProduction);

                    if (resultMessage.MessageType == ResultMessageE.Information)
                    {
                        String filePath = String.Empty;
                        if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(String))
                        {
                            filePath = resultMessage.Tag.ToString();
                        }
                        String fileName1 = Path.GetFileName(saveLocation);
                        Byte[] bytes = File.ReadAllBytes(filePath);
                        if (bytes != null && bytes.Length > 0)
                        {
                            resultMessage.Tag = bytes;
                            string resFname = Path.GetFileNameWithoutExtension(saveLocation);
                            string directoryName;
                            directoryName = Path.GetDirectoryName(saveLocation);
                            string[] fileEntries = Directory.GetFiles(directoryName, "*" + fileName + "*");
                            string[] filesList = Directory.GetFiles(directoryName, "*" + fileName + "*");


                            foreach (string file in filesList)
                            {
                                //if (file.ToUpper().Contains(resFname.ToUpper()))
                                {
                                    File.Delete(file);
                                }
                            }
                        }
                        if (resultMessage.MessageType == ResultMessageE.Information)
                        {
                            resultMessage.DefaultSuccessBehaviour();
                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }
            return resultMessage;
        }

        public ResultMessage PrintBirimMakinaReports(string fromDate, string toDate)
        {
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                List<TblDashboardEntityHistoryTO> tblDashboardEntityHistoryTOList = _iTblDashboardEntityHistoryBL.SelectAllDashboardEntityReportData(fromDate, toDate);
                if (tblDashboardEntityHistoryTOList != null && tblDashboardEntityHistoryTOList.Count > 0)
                {
                    DataTable PrintBirimMakinaDT = new DataTable();
                    PrintBirimMakinaDT.Columns.Add("Sr.No", typeof(int));
                    PrintBirimMakinaDT.Columns.Add("Date", typeof(DateTime));
                    PrintBirimMakinaDT.Columns.Add("Amount", typeof(double));
                    for (int i = 0; i < tblDashboardEntityHistoryTOList.Count; i++)
                    {
                        PrintBirimMakinaDT.Rows.Add();
                        TblDashboardEntityHistoryTO tblDashboardEntityHistoryTO = tblDashboardEntityHistoryTOList[i];
                        PrintBirimMakinaDT.Rows[i]["Sr.No"] = i + 1;
                        PrintBirimMakinaDT.Rows[i]["Date"] = tblDashboardEntityHistoryTO.CreatedOn;
                        PrintBirimMakinaDT.Rows[i]["Amount"] = Convert.ToDouble(tblDashboardEntityHistoryTO.EntityValue);
                    }
                    DataSet printDataSet = new DataSet();
                    PrintBirimMakinaDT.TableName = "PrintBirimMakinaDT";
                    //FooterReportDT.TableName = "FooterReportDT";
                    printDataSet.Tables.Add(PrintBirimMakinaDT);
                    //  printDataSet.Tables.Add(FooterReportDT);
                    string templateName = string.Empty;
                    String templateFilePath = string.Empty;
                    String fileName = string.Empty;
                    templateName = "BirimMakinaReports";
                    templateFilePath = _iDimReportTemplateBL.SelectReportFullName(templateName);
                    fileName = "BirimMakinaReports-" + DateTime.Now.Ticks;

                    String saveLocation = AppDomain.CurrentDomain.BaseDirectory + fileName + ".xls";
                    Boolean IsProduction = true;

                    resultMessage = _iRunReport.GenrateMktgInvoiceReport(printDataSet, templateFilePath, saveLocation, Constants.ReportE.EXCEL_DONT_OPEN, IsProduction);

                    if (resultMessage.MessageType == ResultMessageE.Information)
                    {
                        String filePath = String.Empty;
                        if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(String))
                        {
                            filePath = resultMessage.Tag.ToString();
                        }
                        String fileName1 = Path.GetFileName(saveLocation);
                        Byte[] bytes = File.ReadAllBytes(filePath);
                        if (bytes != null && bytes.Length > 0)
                        {
                            resultMessage.Tag = bytes;
                            string resFname = Path.GetFileNameWithoutExtension(saveLocation);
                            string directoryName;
                            directoryName = Path.GetDirectoryName(saveLocation);
                            string[] fileEntries = Directory.GetFiles(directoryName, "*" + fileName + "*");
                            string[] filesList = Directory.GetFiles(directoryName, "*" + fileName + "*");

                            foreach (string file in filesList)
                            {
                                //if (file.ToUpper().Contains(resFname.ToUpper()))
                                {
                                    File.Delete(file);
                                }
                            }
                        }
                        if (resultMessage.MessageType == ResultMessageE.Information)
                        {
                            resultMessage.DefaultSuccessBehaviour();
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return resultMessage;
        }

        public ResultMessage PrintCorrectionUnloadingReports(string fromDate, string toDate,String purchaseManagerIds)
        {
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                DataTable CorrectionUnloadingReportDT = new DataTable();
                CorrectionUnloadingReportDT.Columns.Add("Sr.No", typeof(int));
                CorrectionUnloadingReportDT.Columns.Add("Date", typeof(DateTime));
                CorrectionUnloadingReportDT.Columns.Add("UnloadingPoint", typeof(string));
                CorrectionUnloadingReportDT.Columns.Add("Quantity", typeof(double));
                CorrectionUnloadingReportDT.Columns.Add("Quality", typeof(string));
                CorrectionUnloadingReportDT.Columns.Add("AveragePrice", typeof(double));
                
                List<CorrectionUnloadingReportTO> correctionUnloadingReportTOList = GetCorrectionUnloadingReports(fromDate, toDate, true, purchaseManagerIds);
                if (correctionUnloadingReportTOList != null && correctionUnloadingReportTOList.Count > 0)
                {
                    for (int i = 0; i < correctionUnloadingReportTOList.Count; i++)
                    {
                        CorrectionUnloadingReportDT.Rows.Add();
                        CorrectionUnloadingReportTO correctionUnloadingReportTO = correctionUnloadingReportTOList[i];
                        CorrectionUnloadingReportDT.Rows[i]["Sr.No"] = i + 1;
                        CorrectionUnloadingReportDT.Rows[i]["Date"] = correctionUnloadingReportTO.CorretionCompletedOn;
                        CorrectionUnloadingReportDT.Rows[i]["UnloadingPoint"] = correctionUnloadingReportTO.SupervisorName;
                        //Added by minal 13-04-2021 for qty = 3 decimal and amt = 2decimal
                        //CorrectionUnloadingReportDT.Rows[i]["Quantity"] = correctionUnloadingReportTO.CorrectionQty;
                        CorrectionUnloadingReportDT.Rows[i]["Quantity"] = correctionUnloadingReportTO.CorrectionQtyForReport;
                        CorrectionUnloadingReportDT.Rows[i]["Quality"] = correctionUnloadingReportTO.ItemName;
                        //CorrectionUnloadingReportDT.Rows[i]["AveragePrice"] = correctionUnloadingReportTO.CorrectionAmt;
                        CorrectionUnloadingReportDT.Rows[i]["AveragePrice"] = correctionUnloadingReportTO.CorrectionAmtForReport;
                    }
                    DataSet printDataSet = new DataSet();
                    CorrectionUnloadingReportDT.TableName = "CorrectionUnloadingReportDT";
                    //FooterReportDT.TableName = "FooterReportDT";
                    printDataSet.Tables.Add(CorrectionUnloadingReportDT);
                    //  printDataSet.Tables.Add(FooterReportDT);
                    string templateName = string.Empty;
                    String templateFilePath = string.Empty;
                    String fileName = string.Empty;
                    templateName = "UnloadingPointWiseCorrectionReport";
                    templateFilePath = _iDimReportTemplateBL.SelectReportFullName(templateName);
                    fileName = "UnloadingPointWiseCorrectionReport-" + DateTime.Now.Ticks;

                    String saveLocation = AppDomain.CurrentDomain.BaseDirectory + fileName + ".xls";
                    Boolean IsProduction = true;

                    resultMessage = _iRunReport.GenrateMktgInvoiceReport(printDataSet, templateFilePath, saveLocation, Constants.ReportE.EXCEL_DONT_OPEN, IsProduction);

                    if (resultMessage.MessageType == ResultMessageE.Information)
                    {
                        String filePath = String.Empty;
                        if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(String))
                        {
                            filePath = resultMessage.Tag.ToString();
                        }
                        String fileName1 = Path.GetFileName(saveLocation);
                        Byte[] bytes = File.ReadAllBytes(filePath);
                        if (bytes != null && bytes.Length > 0)
                        {
                            resultMessage.Tag = bytes;
                            string resFname = Path.GetFileNameWithoutExtension(saveLocation);
                            string directoryName;
                            directoryName = Path.GetDirectoryName(saveLocation);
                            string[] fileEntries = Directory.GetFiles(directoryName, "*" + fileName + "*");
                            string[] filesList = Directory.GetFiles(directoryName, "*" + fileName + "*");

                            foreach (string file in filesList)
                            {
                                //if (file.ToUpper().Contains(resFname.ToUpper()))
                                {
                                    File.Delete(file);
                                }
                            }
                        }
                        if (resultMessage.MessageType == ResultMessageE.Information)
                        {
                            resultMessage.DefaultSuccessBehaviour();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return resultMessage;
        }

        public List<CorrectionUnloadingReportTO> GetCorrectionUnloadingReports(string fromDate, string toDate, Boolean isForReport,String purchaseManagerIds)
        {
            int isCorrectionCompleted = 1;
            int vehiclePhaseId = (int)StaticStuff.Constants.PurchaseVehiclePhasesE.CORRECTIONS;
            int statusId = (int)(Int32)Constants.TranStatusE.UNLOADING_COMPLETED;
            DateTime _fromdate = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
            DateTime _toDate = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));
            _fromdate = Constants.GetStartDateTime(_fromdate);
            _toDate = Constants.GetEndDateTime(_toDate);
            string strSeprator = ",";
            if (isForReport)
                strSeprator = "\n";
            List<CorrectionUnloadingReportTO> finalCorrectionUnloadingReportTOList = new List<CorrectionUnloadingReportTO>();
            try
            {
                List<CorrectionUnloadingReportTO> correctionUnloadingReportTOList = _ireportDAO.GetCorrectionUnloadingReports(_fromdate, _toDate, isCorrectionCompleted, vehiclePhaseId, statusId, purchaseManagerIds);
                if (correctionUnloadingReportTOList != null && correctionUnloadingReportTOList.Count > 0)
                {
                    List<CorrectionUnloadingReportTO> supervisorIdList = correctionUnloadingReportTOList.GroupBy(g => g.SupervisorId).Select(s => s.FirstOrDefault()).ToList();
                    if (supervisorIdList != null && supervisorIdList.Count > 0)
                    {
                        for (int i = 0; i < supervisorIdList.Count; i++)
                        {
                            List<CorrectionUnloadingReportTO> correctionUnloadingReportTOTempList = correctionUnloadingReportTOList.Where(w => w.SupervisorId == supervisorIdList[i].SupervisorId).ToList();
                            if (correctionUnloadingReportTOTempList != null && correctionUnloadingReportTOTempList.Count > 0)
                            {
                                List<CorrectionUnloadingReportTO> dateWiseList = correctionUnloadingReportTOList.GroupBy(g => g.CorretionCompletedDate).Select(s => s.FirstOrDefault()).ToList();
                                for (int j = 0; j < dateWiseList.Count; j++)
                                {
                                    CorrectionUnloadingReportTO correctionUnloadingReportTO = new CorrectionUnloadingReportTO();
                                    List<CorrectionUnloadingReportTO> correctionUnloadingReportTODateList = correctionUnloadingReportTOTempList.Where(w => w.CorretionCompletedDate == dateWiseList[j].CorretionCompletedDate).ToList();
                                    if (correctionUnloadingReportTODateList != null && correctionUnloadingReportTODateList.Count > 0)
                                    {
                                        correctionUnloadingReportTO.SupervisorName = correctionUnloadingReportTODateList[0].SupervisorName;
                                        correctionUnloadingReportTO.SupervisorId = correctionUnloadingReportTODateList[0].SupervisorId;
                                        correctionUnloadingReportTO.CorretionCompletedOn = correctionUnloadingReportTODateList[0].CorretionCompletedOn;
                                        // correctionUnloadingReportTO.CorretionCompletedDate = correctionUnloadingReportTODateList[0].CorretionCompletedDate;
                                        Dictionary<string, double> gradeUnloadedDCT = new Dictionary<string, double>();
                                        for (int c = 0; c < correctionUnloadingReportTODateList.Count; c++)
                                        {
                                            CorrectionUnloadingReportTO correctionUnloadingReportDateTO = correctionUnloadingReportTODateList[c];
                                            if (gradeUnloadedDCT.ContainsKey(correctionUnloadingReportDateTO.ItemName))
                                            {
                                                gradeUnloadedDCT[correctionUnloadingReportDateTO.ItemName] += correctionUnloadingReportDateTO.CorrectionQty;
                                            }
                                            else
                                            {
                                                gradeUnloadedDCT.Add(correctionUnloadingReportDateTO.ItemName, correctionUnloadingReportDateTO.CorrectionQty);
                                            }
                                            // correctionUnloadingReportTO.ItemName += correctionUnloadingReportDateTO.ItemName + "-" + correctionUnloadingReportDateTO.CorrectionQty+ strSeprator;
                                            correctionUnloadingReportTO.CorrectionQty += correctionUnloadingReportDateTO.CorrectionQty;
                                            correctionUnloadingReportTO.CorrectionAmt += correctionUnloadingReportDateTO.CorrectionAmt;

                                        }

                                        correctionUnloadingReportTO.CorrectionQty = Math.Round(correctionUnloadingReportTO.CorrectionQty, 3);
                                        correctionUnloadingReportTO.CorrectionQtyForReport = String.Format("{0:0.000}", correctionUnloadingReportTO.CorrectionQty);
                                        correctionUnloadingReportTO.CorrectionAmt = Math.Round((correctionUnloadingReportTO.CorrectionAmt / correctionUnloadingReportTODateList.Count), 3);
                                        correctionUnloadingReportTO.CorrectionAmtForReport = String.Format("{0:0.00}", correctionUnloadingReportTO.CorrectionAmt);
                                        string gradeUnloadeStr = string.Empty;
                                        foreach (var item in gradeUnloadedDCT)
                                        {
                                            gradeUnloadeStr += item.Key.ToString() + "-" + item.Value.ToString() + strSeprator;
                                        }
                                        //gradeUnloadeStr = gradeUnloadeStr.TrimEnd(',');
                                        // correctionUnloadingReportTO.ItemName.TrimEnd();
                                        correctionUnloadingReportTO.ItemName = gradeUnloadeStr;
                                        finalCorrectionUnloadingReportTOList.Add(correctionUnloadingReportTO);
                                    }
                                }

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return null;
            }

            return finalCorrectionUnloadingReportTOList;
        }

        public ResultMessage GetDayWiseRateChartReport(String fromDate, String toDate)
        {
            List<DayWiseRateChartTO> dayWiseRateChartTOList = new List<DayWiseRateChartTO>();
            DataSet reportDS = new DataSet();
            DataTable dayWiseRateChartDt = new DataTable();

            dayWiseRateChartDt = GetDayWiseRateChartDtls(fromDate, toDate);
            reportDS.Tables.Add(dayWiseRateChartDt);
            String ReportTemplateName = Constants.RATE_CHART_REPORT_TEMPLATE;

            String templateFilePath = _iDimReportTemplateBL.SelectReportFullName(ReportTemplateName);
            String fileName = "Doc-" + DateTime.Now.Ticks;

            //download location for rewrite  template file
            String saveLocation = AppDomain.CurrentDomain.BaseDirectory + fileName + ".xls";
            // RunReport runReport = new RunReport();
            Boolean IsProduction = true;

            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName("IS_PRODUCTION_ENVIRONMENT_ACTIVE");
            if (tblConfigParamsTO != null)
            {
                if (Convert.ToInt32(tblConfigParamsTO.ConfigParamVal) == 0)
                {
                    IsProduction = false;
                }
            }
            ResultMessage resultMessage = _iRunReport.GenrateMktgInvoiceReport(reportDS, templateFilePath, saveLocation, Constants.ReportE.EXCEL_DONT_OPEN, IsProduction);
            if (resultMessage.MessageType == ResultMessageE.Information)
            {
                String filePath = String.Empty;
                if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(String))
                {

                    filePath = resultMessage.Tag.ToString();
                }
                //driveName + path;
                int returnPath = 0;
                if (returnPath != 1)
                {
                    String fileName1 = Path.GetFileName(saveLocation);
                    Byte[] bytes = File.ReadAllBytes(filePath);
                    if (bytes != null && bytes.Length > 0)
                    {
                        resultMessage.Tag = bytes;

                        string resFname = Path.GetFileNameWithoutExtension(saveLocation);
                        string directoryName;
                        directoryName = Path.GetDirectoryName(saveLocation);
                        string[] fileEntries = Directory.GetFiles(directoryName, "*Doc*");
                        string[] filesList = Directory.GetFiles(directoryName, "*Doc*");

                        foreach (string file in filesList)
                        {
                            //if (file.ToUpper().Contains(resFname.ToUpper()))
                            {
                                File.Delete(file);
                            }
                        }
                    }
                }
            }

            return resultMessage;

        }

        public DataTable GetDayWiseRateChartDtls(String fromDate, String toDate)
        {
            DataTable dayWiseRateChartDt = new DataTable();

            DateTime from_Date = DateTime.MinValue;
            DateTime to_Date = DateTime.MinValue;
            if (Constants.IsDateTime(fromDate))
                from_Date = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
            if (Constants.IsDateTime(toDate))
                to_Date = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));

            DateTime tempFromDate = from_Date.AddDays(-30);


            Constants.ItemProdCategoryE itemProdCategoryE = Constants.ItemProdCategoryE.SCRAP_OR_WASTE;

            List<TblProdClassificationTO> TblProdClassificationTOCatlist = _iTblProdClassificationBL.SelectAllProdClassificationListyByItemProdCatgE(itemProdCategoryE);
            List<TblProdClassificationTO> TblProdClassificationTOSpecificationlist = new List<TblProdClassificationTO>();
            if (TblProdClassificationTOCatlist != null && TblProdClassificationTOCatlist.Count > 0)
            {
                string catStr = (string.Join(",", TblProdClassificationTOCatlist.Select(x => x.IdProdClass.ToString()).ToArray()));

                List<TblProdClassificationTO> TblProdClassificationTOSubCatlist = _iTblProdClassificationBL.SelectAllTblProdClassification(catStr, "SC");
                if (TblProdClassificationTOSubCatlist != null && TblProdClassificationTOSubCatlist.Count > 0)
                {
                    string subCatStr = (string.Join(",", TblProdClassificationTOSubCatlist.Select(x => x.IdProdClass.ToString()).ToArray()));

                    TblProdClassificationTOSpecificationlist = _iTblProdClassificationBL.SelectAllTblProdClassification(subCatStr, "S");
                }

            }


            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryBL.UnloadingRateScrapQuery(from_Date, to_Date);

            List<TblPurchaseScheduleSummaryTO> tblSaleSummaryTOList = _iTblGlobalRatePurchaseDAO.GetAvgSaleDateWise(from_Date, to_Date);

            dayWiseRateChartDt.Columns.Add("Date", typeof(DateTime));
            dayWiseRateChartDt.Columns.Add("DeclaredRate", typeof(double));
            dayWiseRateChartDt.Columns.Add("QuoatedRate", typeof(double));
            dayWiseRateChartDt.Columns.Add("TMTRate", typeof(double));
            dayWiseRateChartDt.Columns.Add("SaleRate", typeof(double));
            dayWiseRateChartDt.Columns.Add("BillType");

            List<String> materialTypeList = new List<string>();

            List<TblPurchaseScheduleSummaryTO> tempDistList = new List<TblPurchaseScheduleSummaryTO>();
            if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count > 0)
            {
                tempDistList = tblPurchaseScheduleSummaryTOList.GroupBy(g => g.ProdClassDesc).ToList().Select(s => s.FirstOrDefault()).ToList();
                if (tempDistList != null && tempDistList.Count > 0)
                {
                    for (int d = 0; d < tempDistList.Count; d++)
                    {
                        if (!dayWiseRateChartDt.Columns.Contains(tempDistList[d].ProdClassDesc))
                        {
                            dayWiseRateChartDt.Columns.Add(tempDistList[d].ProdClassDesc, typeof(double));
                            materialTypeList.Add(tempDistList[d].ProdClassDesc);
                        }
                    }
                }
            }

            if (TblProdClassificationTOSpecificationlist != null && TblProdClassificationTOSpecificationlist.Count > 0)
            {
                var tempDistList1 = TblProdClassificationTOSpecificationlist.GroupBy(g => g.ProdClassDesc).ToList().Select(s => s.FirstOrDefault()).ToList();
                if (tempDistList1 != null && tempDistList1.Count > 0)
                {
                    for (int d = 0; d < tempDistList1.Count; d++)
                    {
                        if (!dayWiseRateChartDt.Columns.Contains(tempDistList1[d].ProdClassDesc))
                        {
                            dayWiseRateChartDt.Columns.Add(tempDistList1[d].ProdClassDesc, typeof(double));
                            materialTypeList.Add(tempDistList1[d].ProdClassDesc);
                        }
                    }
                }
            }



            List<TblGlobalRatePurchaseTO> globalRatePurchaseList = _iTblGlobalRatePurchaseDAO.GetGlobalPurchaseRateList(tempFromDate, to_Date);

            List<TblGlobalRatePurchaseTO> globalRateList = _iTblGlobalRatePurchaseDAO.GetGlobalRateList(tempFromDate, to_Date);

            String variableCode = "V48";
            List<TblVariablesTO> tblVariablesTOList = _iTblVariablesBL.SelectVariableCodeDtls(variableCode, new DateTime(), to_Date);
            Int32 billType = 0;

            for (DateTime date1 = from_Date; date1.Date <= to_Date.Date; date1 = date1.AddDays(1))
            {
                DateTime date = date1;

                String billTypStr = String.Empty;
                if (billType == 0)
                {
                    billType = 1;
                    billTypStr = "Order";
                    date1 = date1.AddDays(-1);
                }
                else
                {
                    billType = 0;
                    billTypStr = "Enquiry";
                }

                Int32 rwCnt = dayWiseRateChartDt.Rows.Count;
                dayWiseRateChartDt.Rows.Add();
                dayWiseRateChartDt.Rows[rwCnt]["Date"] = date.Date;
                //dayWiseRateChartDt.Rows[rwCnt]["QuoatedRate"] = 0;
                dayWiseRateChartDt.Rows[rwCnt]["TMTRate"] = 0;
                dayWiseRateChartDt.Rows[rwCnt]["BillType"] = billTypStr;


                #region Declare Rate
                double avgDeclareRate = 0;
                if (globalRatePurchaseList != null && globalRatePurchaseList.Count > 0)
                {
                    List<TblGlobalRatePurchaseTO> curentglobalList = globalRatePurchaseList.Where(a => a.CreatedOn.Date == date.Date).ToList();
                    if (curentglobalList == null || curentglobalList.Count == 0)
                    {
                        var previousTO = globalRatePurchaseList.Where(a => a.CreatedOn.Date <= date.Date).OrderByDescending(o => o.CreatedOn).FirstOrDefault();
                        if (previousTO != null)
                        {
                            curentglobalList.Add(previousTO);
                        }
                    }
                    if (curentglobalList != null && curentglobalList.Count > 0)
                    {
                        avgDeclareRate = curentglobalList.Sum(s => s.Rate) / curentglobalList.Count;
                        dayWiseRateChartDt.Rows[rwCnt]["DeclaredRate"] = Math.Round(avgDeclareRate, 0);
                    }
                }
                #endregion

                #region Quoated Rate
                double avgQuoatedRate = 0;
                if (tblVariablesTOList != null && tblVariablesTOList.Count > 0)
                {
                    List<TblVariablesTO> curentVariableList = tblVariablesTOList.Where(a => a.CreatedOn.Date == date.Date).ToList();
                    if (curentVariableList == null || curentVariableList.Count == 0)
                    {
                        var previousTO = tblVariablesTOList.Where(a => a.CreatedOn.Date <= date.Date).OrderByDescending(o => o.CreatedOn).FirstOrDefault();
                        if (previousTO != null)
                        {
                            curentVariableList.Add(previousTO);
                        }
                    }
                    if (curentVariableList != null && curentVariableList.Count > 0)
                    {
                        avgQuoatedRate = curentVariableList.Sum(s => s.VariableValue) / curentVariableList.Count;
                        dayWiseRateChartDt.Rows[rwCnt]["QuoatedRate"] = Math.Round(avgQuoatedRate, 0);
                    }
                }
                #endregion

                #region Material type

                if (materialTypeList != null && materialTypeList.Count > 0)
                {
                    for (int p = 0; p < materialTypeList.Count; p++)
                    {
                        Double materialWiseAvg = 0;
                        String prodDesc = materialTypeList[p];
                        if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count > 0)
                        {
                            var tempList = tblPurchaseScheduleSummaryTOList.Where(w => w.UnloadingDate.Date == date.Date && w.COrNcId == billType && w.ProdClassDesc == prodDesc).ToList();
                            if (tempList != null && tempList.Count > 0)
                            {
                                materialWiseAvg = tempList.Sum(s => s.Total) / tempList.Count;
                                dayWiseRateChartDt.Rows[rwCnt][prodDesc] = Math.Round(materialWiseAvg, 0);
                            }
                        }

                    }
                }


                #endregion


                #region TMT decalrate Rate

                double avgDeclareRateForSale = 0;
                if (globalRateList != null && globalRateList.Count > 0)
                {
                    List<TblGlobalRatePurchaseTO> curentglobalListForSale = globalRateList.Where(a => a.CreatedOn.Date == date.Date).ToList();
                    if (curentglobalListForSale == null || curentglobalListForSale.Count == 0)
                    {
                        var previousTO = globalRateList.Where(a => a.CreatedOn.Date <= date.Date).OrderByDescending(o => o.CreatedOn).FirstOrDefault();
                        if (previousTO != null)
                        {
                            curentglobalListForSale.Add(previousTO);
                        }
                    }
                    if (curentglobalListForSale != null && curentglobalListForSale.Count > 0)
                    {
                        avgDeclareRateForSale = curentglobalListForSale.Sum(s => s.Rate) / curentglobalListForSale.Count;
                        dayWiseRateChartDt.Rows[rwCnt]["TMTRate"] = Math.Round(avgDeclareRateForSale, 0);
                    }

                }



                #endregion


                #region TMT sale Rate

                Double SaleWiseAvg = 0;
                if (tblSaleSummaryTOList != null && tblSaleSummaryTOList.Count > 0)
                {
                    var tempList1 = tblSaleSummaryTOList.Where(w => w.CreatedOn.Date == date.Date && w.COrNcId == billType).ToList();
                    if (tempList1 != null && tempList1.Count > 0)
                    {
                        SaleWiseAvg = tempList1.Sum(s => s.Rate) / tempList1.Count;
                        dayWiseRateChartDt.Rows[rwCnt]["SaleRate"] = Math.Round(SaleWiseAvg, 0);
                    }
                }

                #endregion

            }

            dayWiseRateChartDt.TableName = "dayWiseRateChartDt";
            return dayWiseRateChartDt;
        }

        public ResultMessage PrintScheduleTcDetailsReport(Int32 rootScheduleId)
        {
            List<TblPurchaseSchTcDtlsTO> tblPurchaseSchTcDtlsTOList = _iTblPurchaseSchTcDtlsBL.SelectScheTcDtlsByRootScheduleId(rootScheduleId.ToString());

            List<TblPurchaseScheduleSummaryTO> scheduleSummaryList = _iTblPurchaseScheduleSummaryBL.SelectTblPurchaseScheduleDtlsByRootScheduleId(rootScheduleId);

            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                DataSet printDataSet = new DataSet();
                DataTable headerDT = new DataTable();
                DataTable scheduleTcDtlsDT = new DataTable();

                if (scheduleSummaryList == null || scheduleSummaryList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("Purchase Sch TO not found against - Id " + rootScheduleId);
                    return resultMessage;
                }

                scheduleSummaryList = scheduleSummaryList.Where(a => a.IsActive == 1).ToList();
                if (scheduleSummaryList == null || scheduleSummaryList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("Purchase Sch TO not found against - Id " + rootScheduleId);
                    return resultMessage;
                }

                if (tblPurchaseSchTcDtlsTOList == null || tblPurchaseSchTcDtlsTOList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("TC details not found against - Id " + rootScheduleId);
                    return resultMessage;
                }

                headerDT = _iCommonDao.ToDataTable(scheduleSummaryList);

                headerDT.TableName = "headerDT";
                printDataSet.Tables.Add(headerDT);

                //if (tblPurchaseSchTcDtlsTOList != null && tblPurchaseSchTcDtlsTOList.Count > 0)
                //{
                //    scheduleTcDtlsDT = _iCommonDao.ToDataTable(tblPurchaseSchTcDtlsTOList);
                //}

                #region tc details

                List<TblPurchaseSchTcDtlsTO> distitnctTcTypeList = tblPurchaseSchTcDtlsTOList.GroupBy(g => g.TcTypeId).Select(s => s.FirstOrDefault()).ToList();

                scheduleTcDtlsDT.Columns.Add("Element");

                for (int i = 0; i < distitnctTcTypeList.Count; i++)
                {
                    scheduleTcDtlsDT.Columns.Add(distitnctTcTypeList[i].TcTypeName);
                }

                List<TblPurchaseSchTcDtlsTO> distitnctTcElementList = tblPurchaseSchTcDtlsTOList.GroupBy(g => g.TcElementId).Select(s => s.FirstOrDefault()).ToList();

                for (int i = 0; i < distitnctTcElementList.Count; i++)
                {
                    var distitnctTcElementTO = distitnctTcElementList[i];

                    Int32 rwCnt = scheduleTcDtlsDT.Rows.Count;
                    scheduleTcDtlsDT.Rows.Add();

                    scheduleTcDtlsDT.Rows[rwCnt]["Element"] = distitnctTcElementTO.TcElementName;


                    for (int j = 0; j < distitnctTcTypeList.Count; j++)
                    {
                        var distitnctTcTypeTO = distitnctTcTypeList[j];

                        var temp = tblPurchaseSchTcDtlsTOList.Where(w => w.TcTypeId == distitnctTcTypeTO.TcTypeId && w.TcElementId == distitnctTcElementTO.TcElementId).FirstOrDefault();
                        if (temp != null)
                            scheduleTcDtlsDT.Rows[rwCnt][distitnctTcTypeTO.TcTypeName] = temp.TcEleValue;

                    }

                }

                scheduleTcDtlsDT.TableName = "scheduleTcDtlsDT";
                printDataSet.Tables.Add(scheduleTcDtlsDT);

                #endregion


                string templateName = "PurchaseSchTcDtlsReport";
                String templateFilePath = _iDimReportTemplateBL.SelectReportFullName(templateName);
                String fileName = "PurchaseSchTcDtlsReport-" + DateTime.Now.Ticks;
                String saveLocation = AppDomain.CurrentDomain.BaseDirectory + fileName + ".xls";
                Boolean IsProduction = true;

                resultMessage = _iRunReport.GenrateMktgInvoiceReport(printDataSet, templateFilePath, saveLocation, Constants.ReportE.PDF_DONT_OPEN, IsProduction);

                if (resultMessage.MessageType == ResultMessageE.Information)
                {
                    String filePath = String.Empty;
                    if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(String))
                    {
                        filePath = resultMessage.Tag.ToString();
                    }
                    String fileName1 = Path.GetFileName(saveLocation);
                    Byte[] bytes = File.ReadAllBytes(filePath);
                    if (bytes != null && bytes.Length > 0)
                    {
                        resultMessage.Tag = bytes;
                        string resFname = Path.GetFileNameWithoutExtension(saveLocation);
                        string directoryName;
                        directoryName = Path.GetDirectoryName(saveLocation);
                        string[] fileEntries = Directory.GetFiles(directoryName, "*PurchaseSchTcDtlsReport*");
                        string[] filesList = Directory.GetFiles(directoryName, "*PurchaseSchTcDtlsReport*");

                        foreach (string file in filesList)
                        {
                            //if (file.ToUpper().Contains(resFname.ToUpper()))
                            {
                                File.Delete(file);
                            }
                        }
                    }
                    if (resultMessage.MessageType == ResultMessageE.Information)
                    {
                        resultMessage.DefaultSuccessBehaviour();
                    }
                }
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.DefaultExceptionBehaviour(ex, "PrintWeighmentSlipTemplateReport");
                return resultMessage;
            }
            return resultMessage;
        }


    }
}