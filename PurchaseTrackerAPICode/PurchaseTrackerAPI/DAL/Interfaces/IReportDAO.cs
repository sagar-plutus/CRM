using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface IReportDAO
    {
        DataTable SelectGradeWiseReportDetails(TblReportsTO tblReportsTO);
        Double GetPartyWiseProcessChargeForReportDetails(DateTime fromDate, DateTime toDate,Int64 supplierId);
        List<PartWiseReportTO> SelectPartyWiseReportDetails(TblReportsTO tblReportsTO);
        List<VehicleWiseReportTO> SelectVehicleWiseReportDetails(TblReportsTO tblReportsTO);
        List<TallyReportTO> SelectTallyReportDetails(DateTime fromDate, DateTime toDate, int ConfirmTypeId, int supplierId, String purchaseManagerIds, int materialTypeId,string vehicleIds,String dateOfBackYears, Int32 isConsiderTm = 0);
        List<TallyReportTO> SelectTallyReportForExcel(String vehicleIds, int ConfirmTypeId);
        List<PadtaReportTO> SelectPadtaReportDetails(DateTime fromDate, DateTime toDate,String purchaseManagerIds);
        List<purchaseSummuryReportTo> SelectPurchaseSummuryReportForOld(DateTime fromDate, DateTime toDate, string otherTaxIds, string otherTaxIdsTransporter,String purchaseMangerIds);
        List<purchaseSummuryReportTo> SelectPurchaseSummuryReportForNew(DateTime fromDate, DateTime toDate, string otherTaxIds, string otherTaxIdsTransporter, string otherExpensesInsuranceId,String purchaseMangerIds);
        List<purchaseSummuryReportTo> SelectPurchaseSummuryReport(DateTime fromDate, DateTime toDate, string otherTaxIds, string otherTaxIdsTransporter);
        List<purchaseSummuryReportTo> PurchaseSummaryReportH(DateTime fromDate, DateTime toDate,String purchaseMangerIds);
        List<TallyReportTO> ConvertDTToList(SqlDataReader tallyReportTODT);
        List<PadtaReportTO> ConvertDTToListForPadta(SqlDataReader padtaReportTODT);
        List<purchaseSummuryReportTo> ConvertDTToListForPurschase(SqlDataReader purchaseSummuryReportTODT);
        List<SaudaReportTo> GetSaudaChartReport(DateTime fromDate, DateTime toDate, Int32 PmId, Int32 SupplierId);
        List<SaudaReportTo> GetSaudaChartReportComplete(DateTime fromDate, DateTime toDate, Int32 PmId, Int32 SupplierId);
        List<GradeWiseWnloadingReportTO> SelectGradeWiseWnloadingReport(DateTime fromDate, DateTime toDate, int confirmTypeId,String purchaseManagerIds);

            double GetTotalUnloadedQty(int enquiryId);

        double GetTotalScheduledQty(int enquiryId);
        double GetOpeningQty(int enquiryId);
        double GetOpeningScheduledQty(int enquiryId);
        double GetClosingQty(int enquiryId);

        //List<PartyWiseUnldReportTO> GetPartyWiseUnldReport(TblPurSchSummaryFilterTO tblPurSchSummaryFilterTO);
        List<TblPurchaseWeighingStageSummaryTO> GetPartyWeightComparisonReport(DateTime fromDate, DateTime toDate,Int32 statusId,String purchaseManagerIds);

        List<CorrectionUnloadingReportTO> GetCorrectionUnloadingReports(DateTime fromDate, DateTime toDate, int isCorrectionCompleted, int vehiclePhaseId, int statusId,String purchaseManagerIds);

        //Added by minal 26 May 2021
        List<TallyTransportEnquiryTO> SelectTallyTransportEnquiryDetails(string vehicleIds, int cOrNcId);
        List<TallyTransportEnquiryTO> SelectTallyTransportEnquiryDetailsForCopy(DateTime fromDate, DateTime toDate,Int32 cOrNcId);

        List<CCTransportEnquiryTO> SelectCCTransportEnquiryDetails(string vehicleIds, int cOrNcId);
        List<CCTransportEnquiryTO> SelectCCTransportEnquiryDetailsForCopy(DateTime fromDate, DateTime toDate,Int32 cOrNcId);

        List<TallyReportTO> SelectTallyPREnquiryDetails(string vehicleIds, int cOrNcId);
        List<TallyReportTO> SelectTallyPREnquiryDetailsForCopy(DateTime fromDate, DateTime toDate,String materialIds,Int32 cOrNcId);

        List<TblWBRptTO> SelectWBForPurchaseReportList(string vehicleIds, int cOrNcId);
        List<TblWBRptTO> SelectWBForPurchaseReportListForCopy(DateTime fromDate, DateTime toDate);
        List<TblWBRptTO> SelectWBForLoadReportListForCopy(DateTime fromDate, DateTime toDate);
        List<TblWBRptTO> SelectWBForUnloadReportListForCopy(DateTime fromDate, DateTime toDate);
    }
}