using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL
{
    public class PurchaseScheduleSummerycircularBL : IPurchaseScheduleSummerycircularBL
    {
        private readonly Icommondao _iCommonDAO;
        private readonly ITblPurchaseWeighingStageSummaryDAO _iTblPurchaseWeighingStageSummaryDAO;
        private readonly ITblPurchaseScheduleSummaryDAO _iTblPurchaseScheduleSummaryDAO;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly ITblPurchaseScheduleStatusHistoryDAO _iTblPurchaseScheduleStatusHistoryDAO;
        public PurchaseScheduleSummerycircularBL(ITblPurchaseScheduleSummaryDAO iTblPurchaseScheduleSummaryDAO, ITblConfigParamsBL iTblConfigParamsBL
                  , Icommondao icommondao , ITblPurchaseWeighingStageSummaryDAO iTblPurchaseWeighingStageSummaryDAO, ITblPurchaseScheduleStatusHistoryDAO iTblPurchaseScheduleStatusHistoryDAO)
        {
            _iCommonDAO = icommondao;
            _iTblPurchaseScheduleStatusHistoryDAO = iTblPurchaseScheduleStatusHistoryDAO;
            _iTblPurchaseWeighingStageSummaryDAO = iTblPurchaseWeighingStageSummaryDAO;
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iTblPurchaseScheduleSummaryDAO = iTblPurchaseScheduleSummaryDAO;

        }
        public List<TblPurchaseScheduleSummaryTO> SelectAllEnquiryScheduleSummary(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseScheduleSummaryDAO.SelectAllEnquiryScheduleSummary(purchaseEnquiryId, conn, tran);
        }

        public List<TblPurchaseWeighingStageSummaryTO> GetVehicleWeightDetails(Int32 purchaseScheduleId, string weightTypeId, SqlConnection conn, SqlTransaction tran, Boolean isGetAllWeighingDtls = false)
        {
            return _iTblPurchaseWeighingStageSummaryDAO.GetVehicleWeightDetails(purchaseScheduleId, weightTypeId, conn, tran, isGetAllWeighingDtls);
        }
        public ResultMessage checkIfQtyGoesOutofBand(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTONew, TblPurchaseEnquiryTO enquiryTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage msg = new ResultMessage();
            msg.Result = 1;
            Boolean isSendForApproval = false;
            List<TblPurchaseScheduleSummaryTO> TblPurchaseScheduleSummaryListForId = SelectAllEnquiryScheduleSummary(tblPurchaseScheduleSummaryTONew.PurchaseEnquiryId, conn, tran);
            TblConfigParamsTO TblConfigParamsTORec = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.CP_SCRAP_PADTA_CONFIGURATION_LIMIT_FOR_UNLOADING_QTY_BOOKING_QTY, conn, tran);
            double totalSaudaQty = enquiryTO.BookingQty;
            if (TblConfigParamsTORec.ConfigParamVal.ToString() == "0")
            {
                    isSendForApproval = false;
            }
            else
            {
                isSendForApproval = true;
                string[] arr = TblConfigParamsTORec.ConfigParamVal.Split(',');
                if (arr.Length == 1)
                {
                    Double limit = Convert.ToDouble(TblConfigParamsTORec.ConfigParamVal);
                    totalSaudaQty = totalSaudaQty + limit;
                }
            }
            if (TblPurchaseScheduleSummaryListForId != null && isSendForApproval)
            {
                double totalScheduledQty = 0;

                foreach (var TO in TblPurchaseScheduleSummaryListForId)
                {
                    List<TblPurchaseWeighingStageSummaryTO> WeighingDtlList = GetVehicleWeightDetails(TO.RootScheduleId > 0 ? TO.RootScheduleId : TO.IdPurchaseScheduleSummary, ((Int32)Constants.TransMeasureTypeE.INTERMEDIATE_WEIGHT).ToString(), conn, tran);
                    foreach (var item in WeighingDtlList)
                    {
                        if (item.IsValid != 1)
                            totalScheduledQty += (item.NetWeightMT / 1000);
                    }
                }
                if (totalScheduledQty > totalSaudaQty)
                {

                    int result = 0;
                    TblPurchaseScheduleStatusHistoryTO HistoryTO = new TblPurchaseScheduleStatusHistoryTO();
                    HistoryTO.PurchaseScheduleSummaryId = tblPurchaseScheduleSummaryTONew.ActualRootScheduleId;
                    HistoryTO.StatusId = tblPurchaseScheduleSummaryTONew.StatusId;
                    HistoryTO.AcceptStatusId = Convert.ToInt32(Constants.TranStatusE.UNLOADING_IS_IN_PROCESS);
                    HistoryTO.RejectStatusId = Convert.ToInt32(Constants.TranStatusE.UNLOADING_IS_IN_PROCESS);
                    HistoryTO.PhaseId = tblPurchaseScheduleSummaryTONew.VehiclePhaseId;
                    HistoryTO.AcceptPhaseId = (int)Constants.PurchaseVehiclePhasesE.OUTSIDE_INSPECTION;
                    HistoryTO.RejectPhaseId = (int)Constants.PurchaseVehiclePhasesE.OUTSIDE_INSPECTION;
                    HistoryTO.CreatedBy = tblPurchaseScheduleSummaryTONew.UpdatedBy;
                    HistoryTO.CreatedOn =  _iCommonDAO.ServerDateTime;
                    HistoryTO.NavigationUrl = "Unloading/OutsideInspectionForVehicleInspectn";
                    double diffVal = totalScheduledQty - totalSaudaQty;
                    diffVal = Math.Round(diffVal, 3);

                    HistoryTO.StatusRemark = "Pending for approval ,Allowed unloading Qty : " + totalSaudaQty + ", Scheduled Qty : " + totalScheduledQty + ", Diff : " + diffVal;
                    HistoryTO.IsActive = 1;
                    HistoryTO.IsLatest = 1;

                    result = _iTblPurchaseScheduleStatusHistoryDAO.InsertTblPurchaseScheduleStatusHistory(HistoryTO, conn, tran);
                    if (result <= 0)
                    {
                        throw new Exception("Error In InsertTblPurchaseScheduleStatusHistory(HistoryTO, conn, tran)");
                    }
                    msg.Result = 0;
                    msg.MessageType = ResultMessageE.Information;
                    msg.DisplayMessage = HistoryTO.StatusRemark;
                    msg.Text = HistoryTO.StatusRemark;
                }
            }

            return msg;
        }

    }

}
