using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;

using System.Text;
using PurchaseTrackerAPI.DAL.Interfaces;
using System.Data;

namespace PurchaseTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class ReportController : Controller
    {

        private readonly ITblPurchaseScheduleSummaryBL _iTblPurchaseScheduleSummaryBL;
        private readonly IReportBL _ireportBL;
        public ReportController(IReportBL ireportBL, ITblPurchaseScheduleSummaryBL iTblPurchaseScheduleSummaryBL)
        {
            _iTblPurchaseScheduleSummaryBL = iTblPurchaseScheduleSummaryBL;
            _ireportBL = ireportBL;
        }

        [Route("GetOldMasterReport")]
        [HttpGet]
        public List<dynamic> GetOldMasterReport(string fromDate, string toDate, Int32 masterReportTypeE,String purchaseManagerIds)
        {
            return _iTblPurchaseScheduleSummaryBL.GetOldMasterReport(fromDate, toDate, masterReportTypeE, purchaseManagerIds);
        }

        //Added by minal for BRM Report TaskId = 1020 Grade Note Enquiry
        [Route("GetListOfMasterReport")]
        [HttpGet]
        public List<JObject> GetListOfMasterReport(string fromDate, string toDate, Int32 masterReportTypeE,String purchaseManagerIds)
        {
            String flagDropbox = String.Empty;
            return _iTblPurchaseScheduleSummaryBL.GetListOfMasterReport(fromDate, toDate, masterReportTypeE, flagDropbox, purchaseManagerIds);
        }


        [Route("PrintListOfMasterReport")]
        [HttpGet]
        public ResultMessage PrintListOfMasterReport(string fromDate, string toDate,String purchaseManagerIds)
        {
            return _iTblPurchaseScheduleSummaryBL.PrintListOfMasterReport(fromDate, toDate, purchaseManagerIds);
            
        }

        //Task Id 1089
        //Added by minal For BRM Report [26 March 2021]
        [Route("GetListOfTallyReport")]
        [HttpGet]
        public List<TallyReportTO> GetListOfTallyReport(string fromDate, string toDate, int ConfirmTypeId, int supplierId, String purchaseManagerIds, int materialTypeId,Boolean isForNewTallyReport)
        {
            string vehicleIds = string.Empty;
            return _iTblPurchaseScheduleSummaryBL.GetListOfTallyReport(fromDate, toDate, ConfirmTypeId, supplierId, purchaseManagerIds, materialTypeId,isForNewTallyReport, vehicleIds);
        }

        [Route("PrintTallyReport")]
        [HttpGet]
        public ResultMessage PrintTallyReport(string fromDate, string toDate, int ConfirmTypeId, int supplierId, String purchaseManagerIds, int materialTypeId, Boolean isForNewTallyReport)
        {
            string vehicleIds = string.Empty;
            return _iTblPurchaseScheduleSummaryBL.PrintTallyReport(fromDate, toDate, ConfirmTypeId, supplierId, purchaseManagerIds, materialTypeId, isForNewTallyReport, vehicleIds);
        }

        //Task Id 1089
        //Added by minal [03 May 2021] For Vehicle Wise
        [Route("GetVehicleWiseReport")]
        [HttpPost]
        public List<dynamic> GetVehicleWiseReport([FromBody] JObject data)
        {
            TblReportsTO tblReportsTO = JsonConvert.DeserializeObject<TblReportsTO>(data["tblReportsTO"].ToString());

            if (tblReportsTO == null)
            {
                return null;
            }

            return _ireportBL.GetVehicleWiseReportForGeneric(tblReportsTO);
        }

        //Task Id 1089
        //Added by minal [03 May 2021] For Party Wise
        [Route("GetPartyWiseReport")]
        [HttpPost]
        public List<dynamic> GetPartyWiseReport([FromBody] JObject data)
        {
            TblReportsTO tblReportsTO = JsonConvert.DeserializeObject<TblReportsTO>(data["tblReportsTO"].ToString());

            if (tblReportsTO == null)
            {
                return null;
            }

            return _ireportBL.GetPartyWiseReportForGeneric(tblReportsTO);
        }

        //Bug Id 9687
        //Added by minal [03 May 2021] For Grade Wise
        [Route("GetGradeWiseReport")]
        [HttpPost]
        public List<dynamic> GetGradeWiseReport([FromBody] JObject data)
        {
            TblReportsTO tblReportsTO = JsonConvert.DeserializeObject<TblReportsTO>(data["tblReportsTO"].ToString());

            if (tblReportsTO == null)
            {
                return null;
            }

            return _ireportBL.GetGradeWiseReportForGeneric(tblReportsTO);
        }

        [Route("GetListOfPadtaReport")]
        [HttpGet]
        public List<PadtaReportTO> GetListOfPadtaReport(string fromDate, string toDate,String purchaseManagerIds)
        {
            return _ireportBL.GetListOfPadtaReport(fromDate, toDate, purchaseManagerIds);
        }

        [Route("GetPartyWeightComparisonReport")]
        [HttpGet]
        public List<PartyWiseWeighingCompaReportTO> GetPartyWeightComparisonReport(string fromDateStr, string toDateStr,String purchaseManagerIds)
        {
            DateTime fromDate = Convert.ToDateTime(fromDateStr);
            DateTime toDate = Convert.ToDateTime(toDateStr);
            return _ireportBL.GetPartyWeightComparisonReport(fromDate, toDate, purchaseManagerIds);
        }


        //Added by minal For BRM Report Task Id 1015 and 1016 [26 March 2021]
        [Route("GetListOfGradeNoteSummaryReport")]
        [HttpGet]
        public List<dynamic> GetListOfGradeNoteSummaryReport(string fromDate, string toDate,int cOrNCId,String purchaseManagerIds)
        {
            return _ireportBL.GetListOfGradeNoteSummaryReport(fromDate, toDate, cOrNCId, purchaseManagerIds);
        }

        //Added by minal For BRM Report Task Id = 1017
        //[Route("GetListOfTallyDrNoteOrder")]
        //[HttpPost]
        //public List<dynamic> GetListOfTallyDrNoteOrder([FromBody] JObject data)
        //{
        //    TblReportsTO tblReportsTO1 = JsonConvert.DeserializeObject<TblReportsTO>(data["tblReportsTO"].ToString());

        //    if (tblReportsTO1 == null)
        //    {
        //        return null;
        //    }

        //    return _ireportBL.GetListOfTallyDrNoteOrder(tblReportsTO1); 
        //}

        [Route("GetListOfTallyDrNoteOrderReport")]
        [HttpGet]
        public List<TallyDrNoteReportTO> GetListOfTallyDrNoteOrderReport(string fromDate, string toDate,String purchaseManagerIds)
        {
            return _ireportBL.GetListOfTallyDrNoteOrder(fromDate, toDate, purchaseManagerIds);
        }

        ////Added by minal For BRM Report
        //[Route("GetListOfTallyCrNoteOrderCS")]
        //[HttpPost]
        //public List<dynamic> GetListOfTallyCrNoteOrderCS([FromBody] JObject data)
        //{
        //    TblReportsTO tblReportsTO1 = JsonConvert.DeserializeObject<TblReportsTO>(data["tblReportsTO"].ToString());

        //    if (tblReportsTO1 == null)
        //    {
        //        return null;
        //    }

        //    return _ireportBL.GetListOfTallyCrNoteOrderCS(tblReportsTO1);
        //}

        //Added by minal For BRM Report  [26 March 2021]
        [Route("GetPurchaseSummaryReport")]
        [HttpGet]
        public List<purchaseSummuryReportTo> GetPurchaseSummaryReport(string fromDate, string toDate,int isOldOrNewFlag,String purchaseManagerIds)
        {
            return _ireportBL.GetPurchaseSummaryReport(fromDate, toDate, isOldOrNewFlag, purchaseManagerIds);
        }

        [Route("PurchaseSummaryReportH")]
        [HttpGet]

        public List<purchaseSummuryReportTo> PurchaseSummaryReportH(string fromDate, string toDate,String purchaseMangerIds)
        {
            return _ireportBL.PurchaseSummaryReportH(fromDate, toDate, purchaseMangerIds);
        }
        //chetan[25-feb-2020] added for print report on template
        [Route("PrintPurchaseSummaryReport")]
        [HttpGet]
        public ResultMessage PrintPurchaseSummaryReport(string fromDate, string toDate,String purchaseMangerIds)
        {
            return _ireportBL.PrintPurchaseSummaryReport(fromDate, toDate, purchaseMangerIds);
        }

        [Route("PrintSaudaReport")]
        [HttpGet]
        public ResultMessage PrintSaudaReport(string fromDate, string toDate,String purchaseManagerIds)
        {
            return _ireportBL.PrintSaudaReport(fromDate, toDate, purchaseManagerIds);
        }

        [Route("GetSaudaReportList")]
        [HttpGet]
        public List<TblPurchaseEnquiryTO> GetSaudaReportList(string fromDate, string toDate,String purchaseMangerIds)
        {
            return _ireportBL.GetSaudaReportList(fromDate, toDate, purchaseMangerIds);
        }


        //Harshala[28-08-2020] added to print weighment slip report on template
        [Route("PrintWeighmentSlipTemplateReport")]
        [HttpGet]
        public ResultMessage PrintWeighmentSlipTemplateReport(Int32 purchaseScheduleId, Int32 idSchedulePurchaseSummary)
        {
            return _ireportBL.PrintWeighmentSlipTemplateReport(purchaseScheduleId);
        }

        [Route("PrintScheduleTcDetailsReport")]
        [HttpGet]
        public ResultMessage PrintScheduleTcDetailsReport(Int32 rootScheduleId)
        {
            return _ireportBL.PrintScheduleTcDetailsReport(rootScheduleId);
        }

        [Route("PrintUnlodingTimeRport")]
        [HttpGet]
        public ResultMessage PrintUnlodingTimeRport(string fromDate, string toDate, Int32 isForWeighingPointWise,String purchaseManagerIds)
        {
            return _ireportBL.PrintUnlodingTimeRport(fromDate, toDate, isForWeighingPointWise, purchaseManagerIds);
        }
        [Route("GetCorrectionUnloadingReports")]
        [HttpGet]
        public List<CorrectionUnloadingReportTO> GetCorrectionUnloadingReports(string fromDate, string toDate,String purchaseManagerIds)
        {
            return _ireportBL.GetCorrectionUnloadingReports(fromDate, toDate, false, purchaseManagerIds);
        }
        [Route("PrintCorrectionUnloadingReports")]
        [HttpGet]
        public ResultMessage PrintCorrectionUnloadingReports(string fromDate, string toDate,String purchaseManagerIds)
        {
            return _ireportBL.PrintCorrectionUnloadingReports(fromDate, toDate, purchaseManagerIds);
        }
        [Route("PrintBirimMakinaReports")]
        [HttpGet]
        public ResultMessage PrintBirimMakinaReports(string fromDate, string toDate)
        {
            return _ireportBL.PrintBirimMakinaReports(fromDate, toDate);
        }
        
        [Route("GetPurchaseSummaryWithInvoiceForPrint")]
        [HttpGet]
        public TblPurchaseScheduleSummaryTO GetPurchaseSummaryWithInvoiceForPrint(int rootScheduleId)
        {
            return _ireportBL.GetPurchaseSummaryWithInvoiceForPrint(rootScheduleId);
        }

        [Route("GetSaudaChartReport")]
        [HttpGet]
        public List<SaudaReportTo> GetSaudaChartReport(string fromDate, string toDate, Int32 PmId, Int32 SupplierId)
        {
            return _ireportBL.GetSaudaChartReport(fromDate, toDate, PmId, SupplierId);
        }

       

        //[Route("GetPartyWiseUnldReport")]
        //[HttpGet]
        //public List<PartyWiseUnldReportTO> GetPartyWiseUnldReport(TblPurSchSummaryFilterTO tblPurSchSummaryFilterTO)
        //{
        //    return _ireportBL.GetPartyWiseUnldReport(tblPurSchSummaryFilterTO);
        //}

        [Route("GradeWiseWnloadingReport")]
        [HttpGet]
        public List<GradeWiseWnloadingReportTO> GradeWiseWnloadingReport(string fromDateStr, string toDateStr, int confirmTypeId,string purchaseManagerIds)
        {
            DateTime fromDate = Convert.ToDateTime(fromDateStr);
            DateTime toDate = Convert.ToDateTime(toDateStr);
            return _ireportBL.SelectGradeWiseWnloadingReport(fromDate, toDate, confirmTypeId, purchaseManagerIds);
        }
        [Route("GetDayWiseRateChartReport")]
        [HttpGet]
        public ResultMessage GetDayWiseRateChartReport(string fromDate, string toDate)
        {
            return _ireportBL.GetDayWiseRateChartReport(fromDate, toDate);
        }

        [Route("GetDayWiseRateChartDtls")]
        [HttpGet]
        public DataTable GetDayWiseRateChartDtls(string fromDate, string toDate)
        {
            return _ireportBL.GetDayWiseRateChartDtls(fromDate, toDate);
        }
    }
}