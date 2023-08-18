using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.DAL; 
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.BL
{
    public class TblTRLoadingBL : ITblTRLoadingBL
    {
        private readonly ITblTRLoadingHistoryDAO _iTblTRLoadingHistoryDAO;
        private readonly ITblTRLoadingDAO _iTblTRLoadingDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly Icommondao _iCommonDAO;
        private readonly ITblTransferRequestBL _iTblTransferRequestBL;
        private readonly ITblEntityRangeBL _iTblEntityRangeBL;
        private readonly Idimensionbl _idimensionbl;
        private readonly ITblVehicleBL _iTblVehicleBL;


        public TblTRLoadingBL(IConnectionString iConnectionString, Icommondao iCommonDAO, ITblTRLoadingDAO iTblTRLoadingDAO
        , ITblTRLoadingHistoryDAO iTblTRLoadingHistoryDAO, ITblTransferRequestBL iTblTransferRequestBL, ITblEntityRangeBL iTblEntityRangeBL
            , Idimensionbl idimensionbl, ITblVehicleBL iTblVehicleBL)
        {
            _iCommonDAO = iCommonDAO;
            _iConnectionString = iConnectionString;
            _iTblTRLoadingDAO = iTblTRLoadingDAO;
            _iTblTRLoadingHistoryDAO = iTblTRLoadingHistoryDAO;
            _iTblTransferRequestBL = iTblTransferRequestBL;
            _iTblEntityRangeBL = iTblEntityRangeBL;
            _idimensionbl = idimensionbl;
            _iTblVehicleBL = iTblVehicleBL;
        }

        #region Selection
        public DataTable SelectAllTblTRLoading()
        {
            return _iTblTRLoadingDAO.SelectAllTblTRLoading();
        }

        public int SelectNextLoadingId()
        {
            DataTable dt = _iTblTRLoadingDAO.SelectNextLoadingId();
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0][0] != null)
                {
                    return Convert.ToInt32(dt.Rows[0][0]) + 1;
                }
                else
                    return -1;
            }
            else
            {
                return 1;
            }
        }


        /// <summary>
        /// AmolG[2022-Jan-20] Get All Active TR Loading
        /// </summary>
        /// <returns></returns>
        public List<TblTRLoadingTO> SelectAllTblTRLoadingList()
        {
            DataTable tblTRLoadingTODT = _iTblTRLoadingDAO.SelectAllTblTRLoading();
            return ConvertDTToList(tblTRLoadingTODT);
        }

        public TblTRLoadingTO SelectTblTRLoadingTO(Int32 idLoading)
        {
            DataTable tblTRLoadingTODT = _iTblTRLoadingDAO.SelectTblTRLoading(idLoading);
            List<TblTRLoadingTO> tblTRLoadingTOList = ConvertDTToList(tblTRLoadingTODT);
            if (tblTRLoadingTOList != null && tblTRLoadingTOList.Count == 1)
                return tblTRLoadingTOList[0];
            else
                return null;
        }

        public TblTRLoadingTO SelectTblTRLoadingTO(Int32 idLoading, SqlConnection conn, SqlTransaction tran)
        {
            DataTable tblTRLoadingTODT = _iTblTRLoadingDAO.SelectTblTRLoading(idLoading, conn, tran);
            List<TblTRLoadingTO> tblTRLoadingTOList = ConvertDTToList(tblTRLoadingTODT);
            if (tblTRLoadingTOList != null && tblTRLoadingTOList.Count == 1)
                return tblTRLoadingTOList[0];
            else
                return null;
        }

        public List<TblTRLoadingTO> ConvertDTToList(DataTable tblTRLoadingTODT)
        {
            List<TblTRLoadingTO> tblTRLoadingTOList = new List<TblTRLoadingTO>();
            if (tblTRLoadingTODT != null)
            {
                for (int rowCount = 0; rowCount < tblTRLoadingTODT.Rows.Count; rowCount++)
                {
                    TblTRLoadingTO tblTRLoadingTONew = new TblTRLoadingTO();
                    if (tblTRLoadingTODT.Rows[rowCount]["idLoading"] != DBNull.Value)
                        tblTRLoadingTONew.IdLoading = Convert.ToInt32(tblTRLoadingTODT.Rows[rowCount]["idLoading"].ToString());
                    if (tblTRLoadingTODT.Rows[rowCount]["loadingSlipNo"] != DBNull.Value)
                        tblTRLoadingTONew.LoadingSlipNo = Convert.ToString(tblTRLoadingTODT.Rows[rowCount]["loadingSlipNo"].ToString());
                    if (tblTRLoadingTODT.Rows[rowCount]["loadingTypeId"] != DBNull.Value)
                        tblTRLoadingTONew.LoadingTypeId = Convert.ToInt32(tblTRLoadingTODT.Rows[rowCount]["loadingTypeId"].ToString());
                    if (tblTRLoadingTODT.Rows[rowCount]["transferRequestId"] != DBNull.Value)
                        tblTRLoadingTONew.TransferRequestId = Convert.ToInt32(tblTRLoadingTODT.Rows[rowCount]["transferRequestId"].ToString());
                    if (tblTRLoadingTODT.Rows[rowCount]["fromLocationId"] != DBNull.Value)
                        tblTRLoadingTONew.FromLocationId = Convert.ToInt32(tblTRLoadingTODT.Rows[rowCount]["fromLocationId"].ToString());
                    if (tblTRLoadingTODT.Rows[rowCount]["toLocationId"] != DBNull.Value)
                        tblTRLoadingTONew.ToLocationId = Convert.ToInt32(tblTRLoadingTODT.Rows[rowCount]["toLocationId"].ToString());
                    if (tblTRLoadingTODT.Rows[rowCount]["materialTypeId"] != DBNull.Value)
                        tblTRLoadingTONew.MaterialTypeId = Convert.ToInt32(tblTRLoadingTODT.Rows[rowCount]["materialTypeId"].ToString());
                    if (tblTRLoadingTODT.Rows[rowCount]["materialSubTypeId"] != DBNull.Value)
                        tblTRLoadingTONew.MaterialSubTypeId = Convert.ToInt32(tblTRLoadingTODT.Rows[rowCount]["materialSubTypeId"].ToString());
                    if (tblTRLoadingTODT.Rows[rowCount]["vehicleId"] != DBNull.Value)
                        tblTRLoadingTONew.VehicleId = Convert.ToInt32(tblTRLoadingTODT.Rows[rowCount]["vehicleId"].ToString());
                    if (tblTRLoadingTODT.Rows[rowCount]["statusId"] != DBNull.Value)
                        tblTRLoadingTONew.StatusId = Convert.ToInt32(tblTRLoadingTODT.Rows[rowCount]["statusId"].ToString());
                    if (tblTRLoadingTODT.Rows[rowCount]["createdBy"] != DBNull.Value)
                        tblTRLoadingTONew.CreatedBy = Convert.ToInt32(tblTRLoadingTODT.Rows[rowCount]["createdBy"].ToString());
                    if (tblTRLoadingTODT.Rows[rowCount]["updatedBy"] != DBNull.Value)
                        tblTRLoadingTONew.UpdatedBy = Convert.ToInt32(tblTRLoadingTODT.Rows[rowCount]["updatedBy"].ToString());
                    if (tblTRLoadingTODT.Rows[rowCount]["statusBy"] != DBNull.Value)
                        tblTRLoadingTONew.StatusBy = Convert.ToInt32(tblTRLoadingTODT.Rows[rowCount]["statusBy"].ToString());
                    if (tblTRLoadingTODT.Rows[rowCount]["createdOn"] != DBNull.Value)
                        tblTRLoadingTONew.CreatedOn = Convert.ToDateTime(tblTRLoadingTODT.Rows[rowCount]["createdOn"].ToString());
                    if (tblTRLoadingTODT.Rows[rowCount]["updatedOn"] != DBNull.Value)
                        tblTRLoadingTONew.UpdatedOn = Convert.ToDateTime(tblTRLoadingTODT.Rows[rowCount]["updatedOn"].ToString());
                    if (tblTRLoadingTODT.Rows[rowCount]["statusOn"] != DBNull.Value)
                        tblTRLoadingTONew.StatusOn = Convert.ToDateTime(tblTRLoadingTODT.Rows[rowCount]["statusOn"].ToString());
                    if (tblTRLoadingTODT.Rows[rowCount]["isActive"] != DBNull.Value)
                        tblTRLoadingTONew.IsActive = Convert.ToInt32(tblTRLoadingTODT.Rows[rowCount]["isActive"].ToString());
                    if (tblTRLoadingTODT.Rows[rowCount]["scheduleQty"] != DBNull.Value)
                        tblTRLoadingTONew.ScheduleQty = Convert.ToDouble(tblTRLoadingTODT.Rows[rowCount]["scheduleQty"].ToString());
                    if (tblTRLoadingTODT.Rows[rowCount]["narration"] != DBNull.Value)
                        tblTRLoadingTONew.Narration = Convert.ToString(tblTRLoadingTODT.Rows[rowCount]["narration"].ToString());
                    if (tblTRLoadingTODT.Rows[rowCount]["driverName"] != DBNull.Value)
                        tblTRLoadingTONew.DriverName = Convert.ToString(tblTRLoadingTODT.Rows[rowCount]["driverName"].ToString());

                    if (tblTRLoadingTODT.Rows[rowCount]["MaterialType"] != DBNull.Value)
                        tblTRLoadingTONew.MaterialType = Convert.ToString(tblTRLoadingTODT.Rows[rowCount]["MaterialType"].ToString());
                    if (tblTRLoadingTODT.Rows[rowCount]["MaterialSubType"] != DBNull.Value)
                        tblTRLoadingTONew.MaterialSubType = Convert.ToString(tblTRLoadingTODT.Rows[rowCount]["MaterialSubType"].ToString());
                    if (tblTRLoadingTODT.Rows[rowCount]["fromLocation"] != DBNull.Value)
                        tblTRLoadingTONew.FromLocation = Convert.ToString(tblTRLoadingTODT.Rows[rowCount]["fromLocation"].ToString());
                    if (tblTRLoadingTODT.Rows[rowCount]["toLocation"] != DBNull.Value)
                        tblTRLoadingTONew.ToLocation= Convert.ToString(tblTRLoadingTODT.Rows[rowCount]["toLocation"].ToString());
                    if (tblTRLoadingTODT.Rows[rowCount]["unloadingPoint"] != DBNull.Value)
                        tblTRLoadingTONew.UnloadingPoint= Convert.ToString(tblTRLoadingTODT.Rows[rowCount]["unloadingPoint"].ToString());
                    if (tblTRLoadingTODT.Rows[rowCount]["Status"] != DBNull.Value)
                        tblTRLoadingTONew.Status = Convert.ToString(tblTRLoadingTODT.Rows[rowCount]["Status"].ToString());
                    if (tblTRLoadingTODT.Rows[rowCount]["CreatedByName"] != DBNull.Value)
                        tblTRLoadingTONew.CreatedByName = Convert.ToString(tblTRLoadingTODT.Rows[rowCount]["CreatedByName"].ToString());
                    if (tblTRLoadingTODT.Rows[rowCount]["UpdatedByName"] != DBNull.Value)
                        tblTRLoadingTONew.UpdatedByName = Convert.ToString(tblTRLoadingTODT.Rows[rowCount]["UpdatedByName"].ToString());

                    tblTRLoadingTOList.Add(tblTRLoadingTONew);
                }
            }
            return tblTRLoadingTOList;
        }

        /// <summary>
        /// /AmolG[2022-Jan-20] Get All TR Loading by status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public List<TblTRLoadingTO> SelectAllTblTRLoadingList(Int32 statusId)
        {
            DataTable tblTRLoadingTODT = _iTblTRLoadingDAO.SelectAllTblTRLoading(statusId);
            return ConvertDTToList(tblTRLoadingTODT);
        }

        public List<TblTRLoadingTO> SelectAllTblTRLoadingList(String statusIds)
        {
            DataTable tblTRLoadingTODT = _iTblTRLoadingDAO.SelectAllTblTRLoading(statusIds);
            return ConvertDTToList(tblTRLoadingTODT);
        }

        public List<TblTRLoadingTO> SelectAllTblTRLoadingList(String statusIds, DateTime fromDate, DateTime toDate)
        {
            DataTable tblTRLoadingTODT = _iTblTRLoadingDAO.SelectAllTblTRLoading(statusIds, fromDate, toDate);
            return ConvertDTToList(tblTRLoadingTODT);
        }

        public List<TblTRLoadingTO> SelectAllTblTRLoadingList(DateTime fromDate, DateTime toDate)
        {
            DataTable tblTRLoadingTODT = _iTblTRLoadingDAO.SelectAllTblTRLoading(fromDate, toDate);
            return ConvertDTToList(tblTRLoadingTODT);
        }

        public List<TblTRLoadingTO> SelectAllTblTRLoadingList(String statusIds, DateTime fromDate, DateTime toDate, TRLoadingFilterE tRLoadingFilterE)
        {
            
            DataTable tblTRLoadingTODT = _iTblTRLoadingDAO.SelectAllTblTRLoading(statusIds, fromDate, toDate, tRLoadingFilterE);
            return ConvertDTToList(tblTRLoadingTODT);
        }

        #endregion

        #region Insertion
        public int InsertTblTRLoading(TblTRLoadingTO tblTRLoadingTO)
        {
            return _iTblTRLoadingDAO.InsertTblTRLoading(tblTRLoadingTO);
        }

        public int InsertTblTRLoading(TblTRLoadingTO tblTRLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblTRLoadingDAO.InsertTblTRLoading(tblTRLoadingTO, conn, tran);
        }


        /// <summary>
        /// AmolG[2022-Jan-20] Insert new TR Loading and Auto Internal Transfer request
        /// </summary>
        /// <param name="tblTRLoadingTO"></param>
        /// <param name="userId"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns> 
        public int InsertTblTRLoading(TblTRLoadingTO tblTRLoadingTO, int userId, ref string errorMsg)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);

            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                //int nextLoadingDisplayNo = SelectNextLoadingId();
                //if (nextLoadingDisplayNo <= 0)
                //{
                //    return 0;
                //}

                DateTime serverDateTime = _iCommonDAO.ServerDateTime;


                //Check the vehical 
                TblVehicleTO tblVehicleTO = _iTblVehicleBL.SelectTblVehicleTO(tblTRLoadingTO.VehicleId, conn, tran);
                if (tblVehicleTO == null || tblVehicleTO.IdVehicle <= 0)
                {
                    errorMsg = "Vehical is not Found";
                    return 0;
                }

                if (tblVehicleTO.VehicleStatusId != (int)Constants.InternalTransferRequestVehicalStatusE.NEW)
                {
                    errorMsg = "Vehical Status is " + (Constants.InternalTransferRequestVehicalStatusE)tblVehicleTO.VehicleStatusId;
                    return 0;
                }

                tblVehicleTO.VehicleStatusId = (int)Constants.InternalTransferRequestVehicalStatusE.IN_PROCESS;
                tblVehicleTO.UpdatedBy = userId;
                tblVehicleTO.UpdatedOn = serverDateTime;
                ResultMessage resultMessageL = _iTblVehicleBL.UpdateVehicalStatus(tblVehicleTO, conn, tran);
                if (resultMessageL.MessageType == ResultMessageE.Error)
                {
                    errorMsg = resultMessageL.Text;
                    return 0;
                }

                //Create Display No
                DimFinYearTO dimFinYearTO = _idimensionbl.GetCurrentFinancialYear(_iCommonDAO.ServerDateTime, conn, tran);
                TblEntityRangeTO tblEntityRangeTO = _iTblEntityRangeBL.SelectEntityRangeTOFromInvoiceType(Constants.LOADING_DISPLAY_NO, dimFinYearTO.IdFinYear, conn, tran);
                string txnRefNo = string.Empty;
                Int32 entityPrevVal = tblEntityRangeTO.EntityPrevValue;
                entityPrevVal = entityPrevVal + tblEntityRangeTO.IncrementBy;
                txnRefNo = tblEntityRangeTO.Prefix + entityPrevVal;

                tblEntityRangeTO.EntityPrevValue = entityPrevVal;
                int updateResult = _iTblEntityRangeBL.UpdateTblEntityRange(tblEntityRangeTO, conn, tran);
                if (updateResult < 0)
                {
                    tran.Rollback();
                    errorMsg = "Error in UpdateTblEntityRange()";
                    return 0;
                }


                //Check is it empty loading slip, then save the one record in internal transfer table.
                if (tblTRLoadingTO.LoadingTypeId == (Int32)TRLoadingTypeE.Manual)// Use Enum for Manual or From Transfer Request.
                {
                    //Use Internal Transfer Request Insert Call
                    TblTransferRequestTO tblTransferRequestTO = new TblTransferRequestTO();
                    tblTransferRequestTO.CreatedBy = userId;
                    tblTransferRequestTO.CreatedOn = serverDateTime;
                    tblTransferRequestTO.FromLocationId = tblTRLoadingTO.FromLocationId;
                    tblTransferRequestTO.ToLocationId = tblTRLoadingTO.ToLocationId;
                    tblTransferRequestTO.Scheduleqty = tblTRLoadingTO.ScheduleQty;
                    tblTransferRequestTO.StatusChangedBy = userId;
                    tblTransferRequestTO.StatusChangedOn = serverDateTime;
                    tblTransferRequestTO.Qty = tblTRLoadingTO.ScheduleQty;
                    tblTransferRequestTO.Narration = tblTRLoadingTO.Narration;
                    tblTransferRequestTO.MaterialTypeId = tblTRLoadingTO.MaterialTypeId;
                    tblTransferRequestTO.MaterialSubTypeId = tblTRLoadingTO.MaterialSubTypeId;
                    tblTransferRequestTO.IsAutoCreated = 1;
                    tblTransferRequestTO.IsActive = 1;
                    tblTransferRequestTO.StatusId = (Int32)InternalTransferRequestStatusE.CLOSE;

                    tblTransferRequestTO.UnloadingPointId = tblTRLoadingTO.UnloadingPointId;

                    ResultMessage resultMessage = _iTblTransferRequestBL.InsertTblTransferRequestData(tblTransferRequestTO, conn, tran);
                    if (resultMessage.MessageType == ResultMessageE.Error)
                    {
                        errorMsg = resultMessage.Text;
                        return 0;
                    }
                    tblTRLoadingTO.TransferRequestId = tblTransferRequestTO.IdTransferRequest;

                }
                else
                {
                    TblTransferRequestTO tblTransferRequestTO = _iTblTransferRequestBL.SelectTblTransferRequestTO(tblTRLoadingTO.TransferRequestId, conn, tran);

                    if (tblTransferRequestTO == null || tblTransferRequestTO.IdTransferRequest <= 0)
                    {
                        errorMsg = "Transfer Request Not Found.";
                        return 0;
                    }

                    if (tblTransferRequestTO.Scheduleqty + tblTRLoadingTO.ScheduleQty <= tblTransferRequestTO.Qty)
                    {
                        if (tblTransferRequestTO.Scheduleqty + tblTRLoadingTO.ScheduleQty == tblTransferRequestTO.Qty)
                        {
                            tblTransferRequestTO.StatusId = (Int32)InternalTransferRequestStatusE.CLOSE;
                        }
                        else
                            tblTransferRequestTO.StatusId = (Int32)InternalTransferRequestStatusE.IN_PROCESS;

                        tblTransferRequestTO.Scheduleqty += tblTRLoadingTO.ScheduleQty;
                        tblTransferRequestTO.StatusChangedBy = userId;
                        tblTransferRequestTO.StatusChangedOn = serverDateTime;

                        result = _iTblTransferRequestBL.UpdateTblTransferRequestScheduleQtyNStatus(tblTransferRequestTO, conn, tran);
                        if (result <= 0)
                        {
                            errorMsg = "Error When Update Transfer Request Schedule Qty and it's Status";
                            return 0;
                        }
                    }
                    else
                    {
                        errorMsg = "Schedule Qty is more than Transfer request qty.";
                        return 0;
                    }
                }

                tblTRLoadingTO.CreatedBy = userId;
                tblTRLoadingTO.CreatedOn = serverDateTime;
                tblTRLoadingTO.StatusBy = userId;
                tblTRLoadingTO.StatusOn = serverDateTime;
                tblTRLoadingTO.LoadingSlipNo = txnRefNo;
                tblTRLoadingTO.IsActive = 1;
                result = _iTblTRLoadingDAO.InsertTblTRLoading(tblTRLoadingTO, conn, tran);

                if (result <= 0)
                {
                    errorMsg = "Error When Insert Loading";
                    return 0;
                }

                TblTRLoadingHistoryTO tblTRLoadingHistoryTO = new TblTRLoadingHistoryTO();
                tblTRLoadingHistoryTO.StatusId = tblTRLoadingTO.StatusId;
                tblTRLoadingHistoryTO.StatusOn = tblTRLoadingTO.StatusOn;
                tblTRLoadingHistoryTO.StatusBy = tblTRLoadingTO.StatusBy;
                tblTRLoadingHistoryTO.LoadingId = tblTRLoadingTO.IdLoading;

                result = _iTblTRLoadingHistoryDAO.InsertTblTRLoadingHistory(tblTRLoadingHistoryTO, conn, tran);

                if (result <= 0)
                {
                    errorMsg = "Error When Insert Loading History";
                    return 0;
                }

                tran.Commit();
                return 1;
            }
            catch (Exception ex)
            {
                errorMsg = "Exception. InsertTblTRLoading() Ex : " + ex.GetBaseException().ToString();
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }


        #endregion

        #region Updation
        public int UpdateTblTRLoading(TblTRLoadingTO tblTRLoadingTO)
        {
            return _iTblTRLoadingDAO.UpdateTblTRLoading(tblTRLoadingTO);
        }

        /// <summary>
        /// AmolG[2022-Jan-20] Update TR Loading
        /// </summary>
        /// <param name="tblTRLoadingTO"></param>
        /// <param name="userId"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public int UpdateTblTRLoading(TblTRLoadingTO tblTRLoadingTO, int userId, ref string errorMsg)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);

            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                DateTime serverDateTime = _iCommonDAO.ServerDateTime;

                TblTRLoadingTO tblTRLoadingTOPrev = SelectTblTRLoadingTO(tblTRLoadingTO.IdLoading, conn, tran);

                if (tblTRLoadingTOPrev == null)
                {
                    errorMsg = "Erro When get Loading";
                    return 0;
                }

                if (tblTRLoadingTOPrev.VehicleId != tblTRLoadingTO.VehicleId)
                {

                    TblVehicleTO tblVehicleTO = _iTblVehicleBL.SelectTblVehicleTO(tblTRLoadingTO.VehicleId, conn, tran);
                    if (tblVehicleTO == null || tblVehicleTO.IdVehicle <= 0)
                    {
                        errorMsg = "Vehical is not Found";
                        return 0;
                    }

                    if (tblVehicleTO.VehicleStatusId != (int)Constants.InternalTransferRequestVehicalStatusE.NEW)
                    {
                        errorMsg = "Vehical Status is " + (Constants.InternalTransferRequestVehicalStatusE)tblVehicleTO.VehicleStatusId;
                        return 0;
                    }

                    tblVehicleTO.VehicleStatusId = (int)Constants.InternalTransferRequestVehicalStatusE.IN_PROCESS;
                    tblVehicleTO.UpdatedBy = userId;
                    tblVehicleTO.UpdatedOn = serverDateTime;
                    ResultMessage resultMessageL = _iTblVehicleBL.UpdateVehicalStatus(tblVehicleTO, conn, tran);
                    if (resultMessageL.MessageType == ResultMessageE.Error)
                    {
                        errorMsg = resultMessageL.Text;
                        return 0;
                    }

                    TblVehicleTO tblVehicleTOOld = new TblVehicleTO();
                    tblVehicleTOOld.VehicleStatusId = (int)Constants.InternalTransferRequestVehicalStatusE.NEW;
                    tblVehicleTOOld.UpdatedBy = userId;
                    tblVehicleTOOld.UpdatedOn = serverDateTime;
                    resultMessageL = _iTblVehicleBL.UpdateVehicalStatus(tblVehicleTOOld, conn, tran);
                    if (resultMessageL.MessageType == ResultMessageE.Error)
                    {
                        errorMsg = resultMessageL.Text;
                        return 0;
                    }
                }

                TblTransferRequestTO tblTransferRequestTO = _iTblTransferRequestBL.SelectTblTransferRequestTO(tblTRLoadingTO.TransferRequestId, conn, tran);

                if (tblTransferRequestTO == null || tblTransferRequestTO.IdTransferRequest <= 0)
                {
                    errorMsg = "Transfer Request Not Found.";
                    return 0;
                }

                if (tblTransferRequestTO.IsAutoCreated == 0)
                {
                    Double oldSchQty = tblTRLoadingTOPrev.ScheduleQty;
                    Double transferQty = tblTransferRequestTO.Scheduleqty - tblTRLoadingTOPrev.ScheduleQty;

                    if (transferQty + tblTRLoadingTO.ScheduleQty <= tblTransferRequestTO.Qty)
                    {
                        if (transferQty + tblTRLoadingTO.ScheduleQty == tblTransferRequestTO.Qty)
                        {
                            tblTransferRequestTO.StatusId = (Int32)InternalTransferRequestStatusE.CLOSE;
                        }
                        else
                            tblTransferRequestTO.StatusId = (Int32)InternalTransferRequestStatusE.IN_PROCESS;

                        tblTransferRequestTO.Scheduleqty = transferQty + tblTRLoadingTO.ScheduleQty;
                        tblTransferRequestTO.StatusChangedBy = userId;
                        tblTransferRequestTO.StatusChangedOn = serverDateTime;

                        result = _iTblTransferRequestBL.UpdateTblTransferRequestScheduleQtyNStatus(tblTransferRequestTO, conn, tran);
                        if (result <= 0)
                        {
                            errorMsg = "Error When Update Transfer Request Schedule Qty and it's Status";
                            return 0;
                        }
                    }
                    else
                    {
                        errorMsg = "Schedule Qty is more than Transfer request qty.";
                        return 0;
                    }
                }
                else
                {
                    tblTransferRequestTO.Scheduleqty =  tblTRLoadingTO.ScheduleQty;
                    tblTransferRequestTO.StatusChangedBy = userId;
                    tblTransferRequestTO.StatusChangedOn = serverDateTime;
                    tblTransferRequestTO.StatusId = (Int32)InternalTransferRequestStatusE.CLOSE;

                    result = _iTblTransferRequestBL.UpdateTblTransferRequestScheduleQtyNStatus(tblTransferRequestTO, conn, tran);
                    if (result <= 0)
                    {
                        errorMsg = "Error When Update Transfer Request Schedule Qty and it's Status";
                        return 0;
                    }
                }

                tblTRLoadingTO.UpdatedBy = userId;
                tblTRLoadingTO.UpdatedOn = serverDateTime;
                tblTRLoadingTO.StatusBy = userId;
                tblTRLoadingTO.StatusOn = serverDateTime;
                tblTRLoadingTO.IsActive = 1;

                result = _iTblTRLoadingDAO.UpdateTblTRLoading(tblTRLoadingTO, conn, tran);

                if (result <= 0)
                {
                    errorMsg = "Error When Save Loading";
                    return 0;
                }

                TblTRLoadingHistoryTO tblTRLoadingHistoryTO = new TblTRLoadingHistoryTO();
                tblTRLoadingHistoryTO.StatusId = tblTRLoadingTO.StatusId;
                tblTRLoadingHistoryTO.StatusOn = tblTRLoadingTO.StatusOn;
                tblTRLoadingHistoryTO.StatusBy = tblTRLoadingTO.StatusBy;
                tblTRLoadingHistoryTO.LoadingId = tblTRLoadingTO.IdLoading;

                result = _iTblTRLoadingHistoryDAO.InsertTblTRLoadingHistory(tblTRLoadingHistoryTO, conn, tran);

                if (result <= 0)
                {
                    errorMsg = "ErrorWhen Save Loading History";
                    return 0;
                }

                tran.Commit();
                return 1;
            }
            catch (Exception ex)
            {
                errorMsg = "Exception. UpdateTblTRLoading() Ex : " + ex.GetBaseException().ToString();
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// AmolG[2022-Jan-20] Update the Loading Status
        /// </summary>
        /// <param name="tblTRLoadingTO"></param>
        /// <param name="userId"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public int UpdateTblTRLoadingStatus(TblTRLoadingTO tblTRLoadingTO, int userId, ref string errorMsg)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);

            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                DateTime serverDateTime = _iCommonDAO.ServerDateTime;

                tblTRLoadingTO.UpdatedBy = userId;
                tblTRLoadingTO.UpdatedOn = serverDateTime;
                tblTRLoadingTO.StatusBy = userId;
                tblTRLoadingTO.StatusOn = serverDateTime;
                tblTRLoadingTO.IsActive = 1;

                result = _iTblTRLoadingDAO.UpdateTblTRLoadingStatus(tblTRLoadingTO, conn, tran);

                if (result <= 0)
                {
                    return 0;
                }

                TblTRLoadingHistoryTO tblTRLoadingHistoryTO = new TblTRLoadingHistoryTO();
                tblTRLoadingHistoryTO.StatusId = tblTRLoadingTO.StatusId;
                tblTRLoadingHistoryTO.StatusOn = tblTRLoadingTO.StatusOn;
                tblTRLoadingHistoryTO.StatusBy = tblTRLoadingTO.StatusBy;
                tblTRLoadingHistoryTO.LoadingId = tblTRLoadingTO.IdLoading;

                result = _iTblTRLoadingHistoryDAO.InsertTblTRLoadingHistory(tblTRLoadingHistoryTO, conn, tran);

                if (result <= 0)
                {
                    return 0;
                }


                tran.Commit();
                return 1;
            }
            catch (Exception ex)
            {
                errorMsg = "Exception. UpdateTblTRLoadingStatus() Ex : " + ex.GetBaseException().ToString();
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }

        public int DeleteTblTRLoading(TblTRLoadingTO tblTRLoadingTO)
        {
            return _iTblTRLoadingDAO.UpdateTblTRLoading(tblTRLoadingTO);
        }


        public int UpdateTblTRLoading(TblTRLoadingTO tblTRLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblTRLoadingDAO.UpdateTblTRLoading(tblTRLoadingTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblTRLoading(TblTRLoadingTO tblTRLoadingTO, int userId)
        {
            DateTime serverDateTime = _iCommonDAO.ServerDateTime;

            tblTRLoadingTO.UpdatedBy = userId;
            tblTRLoadingTO.UpdatedOn = serverDateTime;
            tblTRLoadingTO.IsActive = 0;
            return _iTblTRLoadingDAO.DeleteTblTRLoading(tblTRLoadingTO);
        }

        public int DeleteTblTRLoading(TblTRLoadingTO tblTRLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblTRLoadingDAO.DeleteTblTRLoading(tblTRLoadingTO, conn, tran);
        }

        #endregion

    }
}
