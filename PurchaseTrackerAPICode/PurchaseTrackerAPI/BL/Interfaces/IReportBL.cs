using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface IReportBL
    {
        List<dynamic> GetGradeWiseReportForGeneric(TblReportsTO tblReportsTO);
        List<dynamic> GetPartyWiseReportForGeneric(TblReportsTO tblReportsTO);
        List<dynamic> GetVehicleWiseReportForGeneric(TblReportsTO tblReportsTO);
       // List<dynamic> GetVehicleWiseReport(TblReportsTO tblReportsTO);
        List<dynamic> GetListOfGradeNoteSummaryReport(string fromDate, string toDate, int cOrNCId,String purchaseManagerIds);
        //List<TallyReportTO> GetListOfTallyReport(string fromDate, string toDate, int ConfirmTypeId, int supplierId, int purchaseManagerId, int materialTypeId,Boolean isForNewTallyReport,string vehicleIds);
        List<PadtaReportTO> GetListOfPadtaReport(string fromDate, string toDate,String purchaseManagerIds);
        //List<dynamic> GetListOfTallyDrNoteOrder(TblReportsTO tblReportsTO);
        List<TallyDrNoteReportTO> GetListOfTallyDrNoteOrder(string fromDate, string toDate,String purchaseManagerIds);
        //List<dynamic> GetListOfTallyCrNoteOrderCS(TblReportsTO tblReportsTO);
        TblPurchaseScheduleSummaryTO GetPurchaseSummaryWithInvoiceForPrint(Int32 rootScheduleId);
        List<purchaseSummuryReportTo> GetPurchaseSummaryReport(string fromDate, string toDate,int isOldOrNewFlag,String purchaseMangerIds);
        List<purchaseSummuryReportTo> PurchaseSummaryReportH(string fromDate, string toDate, String purchaseMangerIds);
        ResultMessage PrintPurchaseSummaryReport(string fromDate, string toDate,String purchaseMangerIds);
        List<SaudaReportTo> GetSaudaChartReport(string fromDate, string toDate,Int32 PmId,Int32 SupplierId);
        List<GradeWiseWnloadingReportTO> SelectGradeWiseWnloadingReport(DateTime fromDate, DateTime toDate, int confirmTypeId,String purchaseManagerIds);
        //List<PartyWiseUnldReportTO> GetPartyWiseUnldReport(TblPurSchSummaryFilterTO tblPurSchSummaryFilterTO);
        List<PartyWiseWeighingCompaReportTO> GetPartyWeightComparisonReport(DateTime fromDate, DateTime toDate,String purchaseManagerIds);

        ResultMessage PrintUnlodingTimeRport(string fromDate, string toDate, Int32 isForWeighingPointWise,String purchaseManagerIds);

       List<CorrectionUnloadingReportTO> GetCorrectionUnloadingReports(string fromDate, string toDate, Boolean isForReport,String purchaseManagerIds);

        ResultMessage PrintCorrectionUnloadingReports(string fromDate, string toDate,String purchaseManagerIds);

        ResultMessage PrintBirimMakinaReports(string fromDate, string toDate);
        ResultMessage PrintSaudaReport(string fromDate, string toDate,String purchaseMangerIds);
        List<TblPurchaseEnquiryTO> GetSaudaReportList(string fromDate, string toDate,String purchaseMangerIds);

        ResultMessage PrintWeighmentSlipTemplateReport(Int32 purchaseScheduleId);

        ResultMessage GetDayWiseRateChartReport(string fromDate, string toDate);
        DataTable GetDayWiseRateChartDtls(String fromDate, String toDate);
        ResultMessage PrintScheduleTcDetailsReport(Int32 rootScheduleId);
    }
}