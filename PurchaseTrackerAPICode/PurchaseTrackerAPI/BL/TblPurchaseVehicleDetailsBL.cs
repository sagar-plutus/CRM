using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.IoT.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL
{
    public class TblPurchaseVehicleDetailsBL : ITblPurchaseVehicleDetailsBL
    {
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly ITblOrganizationBL _iTblOrganizationBL;
        readonly private Itblalertinstancebl _itblalertinstancebl;
        readonly private ITblGradeExpressionDtlsBL _iTblGradeExpressionDtlsBL;
        readonly private ITblPurchaseVehicleDetailsDAO _iTblPurchaseVehicleDetailsDAO;
        readonly private ITblUserBL _iTblUserBL;
        private readonly IConnectionString _iConnectionString;
        private readonly IIotCommunication _iIotCommunication;
        private readonly IGateCommunication _iGateCommunication;
        private readonly ITblWeighingMachineDAO _iTblWeighingMachineDAO;
        private readonly IWeighingCommunication _iWeighingCommunication;
        private readonly IDimStatusDAO _iDimStatusDAO;
        private readonly ITblGateBL _iTblGateBL;
        private readonly ITblGlobalRatePurchaseDAO _iTblGlobalRatePurchaseDAO;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        public TblPurchaseVehicleDetailsBL(ITblPurchaseVehicleDetailsDAO iTblPurchaseVehicleDetailsDAO, IConnectionString iConnectionString, ITblGradeExpressionDtlsBL iTblGradeExpressionDtlsBL, Itblalertinstancebl itblalertinstancebl
                                           , ITblOrganizationBL iTblOrganizationBL
                                           , ITblUserBL iTblUserBL
                                           , ITblConfigParamsBL iTblConfigParamsBL
             , ITblGateBL iTblGateBL,
             ITblWeighingMachineDAO iTblWeighingMachineDAO, IGateCommunication iGateCommunication, IIotCommunication iIotCommunication
            , IWeighingCommunication iWeighingCommunication, IDimStatusDAO iDimStatusDAO, ITblConfigParamsDAO iTblConfigParamsDAO)
        {
            _iConnectionString = iConnectionString;
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iTblUserBL = iTblUserBL;
            _itblalertinstancebl = itblalertinstancebl;
            _iTblGradeExpressionDtlsBL = iTblGradeExpressionDtlsBL;
            _iTblPurchaseVehicleDetailsDAO = iTblPurchaseVehicleDetailsDAO;
            _iTblOrganizationBL = iTblOrganizationBL;
            _iIotCommunication = iIotCommunication;
            _iGateCommunication = iGateCommunication;
            _iTblWeighingMachineDAO = iTblWeighingMachineDAO;
            _iWeighingCommunication = iWeighingCommunication;
            _iDimStatusDAO = iDimStatusDAO;
            _iTblGateBL = iTblGateBL;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
        }
        #region Selection

        public  List<TblPurchaseVehicleDetailsTO> GetVehicleDetailsByEnquiryId(Int32 purchaseEnquiryId)
        {
            return _iTblPurchaseVehicleDetailsDAO.GetVehicleDetailsByEnquiryId(purchaseEnquiryId);
        }

        public  List<TblPurchaseVehicleDetailsTO> GetGradeExpressionDetails(List<TblPurchaseVehicleDetailsTO> tblPurchaseVehicleDetailsTOList)
        {
            if (tblPurchaseVehicleDetailsTOList != null && tblPurchaseVehicleDetailsTOList.Count > 0)
            {
                for (int i = 0; i < tblPurchaseVehicleDetailsTOList.Count; i++)
                {
                    List<TblGradeExpressionDtlsTO> tblGradeExpressionDtlsTOList = new List<TblGradeExpressionDtlsTO>();

                    tblGradeExpressionDtlsTOList = _iTblGradeExpressionDtlsBL.SelectGradeExpressionDtlsByScheduleId(tblPurchaseVehicleDetailsTOList[i].IdVehiclePurchase.ToString());
                    if (tblGradeExpressionDtlsTOList == null)
                    {
                        tblGradeExpressionDtlsTOList = new List<TblGradeExpressionDtlsTO>();
                    }

                    tblPurchaseVehicleDetailsTOList[i].GradeExpressionDtlsTOList = tblGradeExpressionDtlsTOList;
                }
            }
            return tblPurchaseVehicleDetailsTOList;
        }

        // public  List<TblPurchaseVehicleDetailsTO> SelectAllTblPurchaseVehicleDetailsList(Int32 schedulePurchaseId, SqlConnection conn, SqlTransaction tran)
        // {
        //     List<TblPurchaseVehicleDetailsTO> tblPurchaseVehicleDetailsTOList = _iTblPurchaseVehicleDetailsDAO.SelectAllTblPurchaseVehicleDetailsList(schedulePurchaseId, conn, tran);
        //     if (tblPurchaseVehicleDetailsTOList != null && tblPurchaseVehicleDetailsTOList.Count > 0)
        //     {
        //         // GetGradeExpressionDetails(tblPurchaseVehicleDetailsTOList);
        //         // for (int i = 0; i < tblPurchaseVehicleDetailsTOList.Count; i++)
        //         // {
        //         //     tblPurchaseVehicleDetailsTOList[i].GradeExpressionDtlsTOList=BL.TblGradeExpressionDtlsBL.SelectGradeExpressionDtlsByScheduleId(tblPurchaseVehicleDetailsTOList[i].IdVehiclePurchase.ToString(),conn,tran);
        //         // }
        //         BL.TblGradeExpressionDtlsBL.SelectGradeExpDtlsList(tblPurchaseVehicleDetailsTOList, conn, tran);
        //     }
        //     return tblPurchaseVehicleDetailsTOList;
        // }

        public  List<TblPurchaseVehicleDetailsTO> SelectAllTblPurchaseVehicleDetailsList(Int32 schedulePurchaseId, Boolean isGetGradeExpDtls, SqlConnection conn = null, SqlTransaction tran = null)
        {
            List<TblPurchaseVehicleDetailsTO> tblPurchaseVehicleDetailsTOList = null;
            if (conn != null && tran != null)
            {
                tblPurchaseVehicleDetailsTOList = new List<TblPurchaseVehicleDetailsTO>();
                tblPurchaseVehicleDetailsTOList = _iTblPurchaseVehicleDetailsDAO.SelectAllTblPurchaseVehicleDetailsList(schedulePurchaseId, conn, tran);
                if (tblPurchaseVehicleDetailsTOList != null && tblPurchaseVehicleDetailsTOList.Count > 0)
                {
                    if (isGetGradeExpDtls)
                        _iTblGradeExpressionDtlsBL.SelectGradeExpDtlsList(tblPurchaseVehicleDetailsTOList, conn, tran);
                }
            }
            else
            {
                tblPurchaseVehicleDetailsTOList = new List<TblPurchaseVehicleDetailsTO>();
                tblPurchaseVehicleDetailsTOList = _iTblPurchaseVehicleDetailsDAO.SelectAllTblPurchaseVehicleDetailsList(schedulePurchaseId);
                if (tblPurchaseVehicleDetailsTOList != null && tblPurchaseVehicleDetailsTOList.Count > 0)
                {
                    if (isGetGradeExpDtls)
                        _iTblGradeExpressionDtlsBL.SelectGradeExpDtlsList(tblPurchaseVehicleDetailsTOList);
                }
            }

            return tblPurchaseVehicleDetailsTOList;
        }



        public  List<TblPurchaseVehicleDetailsTO> SelectAllTblPurchaseVehicleDtlsList(Int32 schedulePurchaseId)
        {
            List<TblPurchaseVehicleDetailsTO> tblPurchaseVehicleDetailsTOList = _iTblPurchaseVehicleDetailsDAO.SelectAllTblPurchaseVehicleDetailsList(schedulePurchaseId);
            return tblPurchaseVehicleDetailsTOList;
        }


        public  List<TblPurchaseVehicleDetailsTO> GetPurchaseScheduleDetailsBySpotEntryVehicleId(Int32 spotEntryVehicleId)
        {
            return _iTblPurchaseVehicleDetailsDAO.GetPurchaseScheduleDetailsBySpotEntryVehicleId(spotEntryVehicleId);
        }
        public  List<TblPurchaseVehicleDetailsTO> SelectAllVehicleDetailsList(DateTime fromDate, String Userid) //, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehicleDetailsDAO.SelectAllVehicleDetailsList(fromDate, Userid); //, conn, tran);
        }


        #region Optimized code to get vehicle item details with or without grade exp details

        public void SelectVehItemDtlsWithOrWithoutGradeExpDtls(List<TblPurchaseScheduleSummaryTO> scheduleTOList, Boolean isGetGradeExpDtls, SqlConnection conn = null, SqlTransaction tran = null)
        {
            if (scheduleTOList != null && scheduleTOList.Count > 0)
            {
                string scheduleDtlsIds = string.Join(", ", scheduleTOList.Select(i => i.IdPurchaseScheduleSummary));
                List<TblPurchaseVehicleDetailsTO> tblPurchaseVehicleDetailsTOList = null;

                if (conn != null && tran != null)
                {
                    tblPurchaseVehicleDetailsTOList = new List<TblPurchaseVehicleDetailsTO>();
                    tblPurchaseVehicleDetailsTOList = _iTblPurchaseVehicleDetailsDAO.SelectVehicleItemDetailsByScheduleSummaryIds(scheduleDtlsIds, conn, tran);
                }
                else
                {
                    tblPurchaseVehicleDetailsTOList = new List<TblPurchaseVehicleDetailsTO>();
                    tblPurchaseVehicleDetailsTOList = _iTblPurchaseVehicleDetailsDAO.SelectVehicleItemDetailsByScheduleSummaryIds(scheduleDtlsIds);
                }


                if (tblPurchaseVehicleDetailsTOList != null && tblPurchaseVehicleDetailsTOList.Count > 0)
                {
                    for (int i = 0; i < scheduleTOList.Count; i++)
                    {
                        TblPurchaseScheduleSummaryTO scheduleTO = scheduleTOList[i];

                        List<TblPurchaseVehicleDetailsTO> tempVehItemDtlsList = tblPurchaseVehicleDetailsTOList.Where(a => a.SchedulePurchaseId == scheduleTO.IdPurchaseScheduleSummary).ToList();
                        if (tempVehItemDtlsList != null && tempVehItemDtlsList.Count > 0)
                        {

                            scheduleTO.PurchaseScheduleSummaryDetailsTOList = tempVehItemDtlsList;
                            Boolean isForBRM = false;
                            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(StaticStuff.Constants.CP_SCRAP_IS_FOR_BHAGYALAXMI);
                            if (tblConfigParamsTO != null)
                            {

                                if (tblConfigParamsTO.ConfigParamVal == "1")
                                {
                                    isForBRM = true;
                                }

                            }
                            if (isGetGradeExpDtls && isForBRM)
                            {
                                _iTblGradeExpressionDtlsBL.SelectGradeExpDtlsList(scheduleTO.PurchaseScheduleSummaryDetailsTOList, conn, tran);
                            }
                        }
                       
                    }
                }
                //Added  by @KKM For fetch Gate Data From IoT
                //int confiqId = _iTblConfigParamsDAO.IoTSetting();
                //if (confiqId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
                //{
                //    for (int i = 0; i < scheduleTOList.Count; i++)
                //    {
                //        if(string.IsNullOrEmpty(scheduleTOList[i].VehicleNo))
                //            _iIotCommunication.GetItemDataFromIotAndMerge(scheduleTOList[i]);
                //    }
                //}
            }

        }
        // public  void SelectVehItemDtlsByScheduleSummaryIdsWithGradeExpDtls(List<TblPurchaseScheduleSummaryTO> scheduleTOList, SqlConnection conn, SqlTransaction tran)
        // {
        //     if (scheduleTOList != null && scheduleTOList.Count > 0)
        //     {
        //         string scheduleDtlsIds = string.Join(", ", scheduleTOList.Select(i => i.IdPurchaseScheduleSummary));
        //         List<TblPurchaseVehicleDetailsTO> tblPurchaseVehicleDetailsTOList = _iTblPurchaseVehicleDetailsDAO.SelectVehicleItemDetailsByScheduleSummaryIds(schedulePurchaseIds);

        //         if (tblPurchaseVehicleDetailsTOList != null && tblPurchaseVehicleDetailsTOList.Count > 0)
        //         {
        //             for (int i = 0; i < scheduleTOList.Count; i++)
        //             {
        //                 TblPurchaseScheduleSummaryTO scheduleTO = scheduleTOList[i];

        //                 List<TblPurchaseVehicleDetailsTO> tempVehItemDtlsList = tblPurchaseVehicleDetailsTOList.Where(a => a.SchedulePurchaseId == scheduleTO.IdPurchaseScheduleSummary).ToList();
        //                 if (tempVehItemDtlsList != null && tempVehItemDtlsList.Count > 0)
        //                 {
        //                     scheduleTO.PurchaseScheduleSummaryDetailsTOList = tempVehItemDtlsList;
        //                     BL.TblGradeExpressionDtlsBL.SelectGradeExpDtlsList(scheduleTO.PurchaseScheduleSummaryDetailsTOList);

        //                 }
        //             }
        //         }
        //     }
        // }

        #endregion

        public  List<TblPurchaseVehicleDetailsTO> SelectVehicleSpotEntryTO(String Userid) //, SqlConnection conn, SqlTransaction tran)
        {
            //return _iTblPurchaseVehicleDetailsDAO.SelectAllVehicleDetailsList(Userid); //, conn, tran);
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                List<TblPurchaseVehicleDetailsTO> tblVehicleDetailsTO = _iTblPurchaseVehicleDetailsDAO.SelectAllVehicleDetailsList(Userid, conn, tran);
                tran.Commit();
                return tblVehicleDetailsTO;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public  List<TblPurchaseVehicleDetailsTO> SelectAllVehicleDetailsByStatus(String statusId, DateTime fromDate, String vehicleNo)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblPurchaseVehicleDetailsDAO.SelectAllVehicleDetailsByStatus(statusId, fromDate, vehicleNo, conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region Insertion

        public  int InsertTblPurchaseVehicleDetails(TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehicleDetailsDAO.InsertTblPurchaseVehicleDetails(tblPurchaseVehicleDetailsTO, conn, tran);
        }

        public  ResultMessage SaveVehicleSpotEntry(TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;



            //return _iTblPurchaseVehicleDetailsDAO.SaveVehicleSpotEntry(tblPurchaseVehicleDetailsTO, conn, tran);
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                result = InsertVehicleSpotEntry(tblPurchaseVehicleDetailsTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.Text = "Error While InsertSpotEntryVehicleDetails : SaveVehicleSpotEntry";
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                else
                {
                    //tran.Commit();

                    #region Notifications & SMSs

                    if (tblPurchaseVehicleDetailsTO.SystemRolesE == Constants.SystemRolesE.PURCHASE_MANAGER)
                    {

                        TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                        List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();
                        List<TblUserTO> cnfUserList = _iTblUserBL.SelectAllTblUserList(tblPurchaseVehicleDetailsTO.RoleId, conn, tran);
                        TblUserTO userTO = _iTblUserBL.SelectTblUserTO(tblPurchaseVehicleDetailsTO.CreatedBy, conn, tran);
                        if (userTO != null)
                        {
                            TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                            tblAlertUsersTO.UserId = userTO.IdUser;
                            tblAlertUsersTO.DeviceId = userTO.RegisteredDeviceId;
                            tblAlertUsersTOList.Add(tblAlertUsersTO);
                        }

                        if (cnfUserList != null && cnfUserList.Count > 0)
                        {
                            for (int a = 0; a < cnfUserList.Count; a++)
                            {
                                TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                                tblAlertUsersTO.UserId = cnfUserList[a].IdUser;
                                tblAlertUsersTO.DeviceId = cnfUserList[a].RegisteredDeviceId;
                                if (tblAlertUsersTOList != null && tblAlertUsersTOList.Count > 0)
                                {
                                    var isExistTO = tblAlertUsersTOList.Where(x => x.UserId == tblAlertUsersTO.UserId).FirstOrDefault();
                                    if (isExistTO == null)
                                        tblAlertUsersTOList.Add(tblAlertUsersTO);
                                }
                                else
                                    tblAlertUsersTOList.Add(tblAlertUsersTO);
                            }
                        }

                        if (tblPurchaseVehicleDetailsTO.SystemRolesE == Constants.SystemRolesE.PURCHASE_MANAGER)
                        {
                            tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.PURCHASE_VEHICLESPOT_ENTRY;
                            tblAlertInstanceTO.AlertAction = "NEW_VEHICLE_ARRIVAL_ENTRY";
                            tblAlertInstanceTO.AlertComment = "New Vehicle Number #" + tblPurchaseVehicleDetailsTO.VehicleNo + " Arrival.";
                            tblAlertInstanceTO.SourceDisplayId = "NEW_VEHICLE_ARRIVAL_ENTRY";
                            // SMS to Dealer Need to check...
                            Dictionary<Int32, String> orgMobileNoDCT = _iTblOrganizationBL.SelectRegisteredMobileNoDCT(tblPurchaseVehicleDetailsTO.RoleId.ToString(), conn, tran);
                            if (orgMobileNoDCT != null && orgMobileNoDCT.Count == 1)
                            {
                                tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();
                                TblSmsTO smsTO = new TblSmsTO();
                                smsTO.MobileNo = orgMobileNoDCT[Convert.ToInt32(tblPurchaseVehicleDetailsTO.RoleId)];
                                smsTO.SourceTxnDesc = "NEW_VEHICLE_ARRIVAL_ENTRY";
                                string confirmMsg = string.Empty;
                                smsTO.SmsTxt = "New Vehicle arrival " + tblPurchaseVehicleDetailsTO.VehicleNo + " Arrival.";
                                tblAlertInstanceTO.SmsTOList.Add(smsTO);
                            }
                            //Need To conform
                            result = _itblalertinstancebl.ResetAlertInstance((int)NotificationConstants.NotificationsE.PURCHASE_VEHICLESPOT_ENTRY, Convert.ToInt32(tblPurchaseVehicleDetailsTO.RoleId), conn, tran);

                            if (result < 0)
                            {
                                tran.Rollback();
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Text = "Error While Reseting Prev Alert";
                                return resultMessage;
                            }
                        }

                        tblAlertInstanceTO.EffectiveFromDate = tblPurchaseVehicleDetailsTO.CreatedOn;//tblPurchaseVehicleDetailsTO.UpdatedOn;
                        tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                        tblAlertInstanceTO.IsActive = 1;
                        tblAlertInstanceTO.SourceEntityId = Convert.ToInt32(tblPurchaseVehicleDetailsTO.RoleId);//Need to check....
                        tblAlertInstanceTO.RaisedOn = tblPurchaseVehicleDetailsTO.CreatedOn;// tblPurchaseVehicleDetailsTO.UpdatedOn;
                        tblAlertInstanceTO.RaisedBy = tblPurchaseVehicleDetailsTO.CreatedBy;// tblPurchaseVehicleDetailsTO.UpdatedBy;
                        tblAlertInstanceTO.IsAutoReset = 1;

                        ResultMessage rMessage = _itblalertinstancebl.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                        if (rMessage.MessageType != ResultMessageE.Information)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour();
                            resultMessage.Text = "Error While Generating Notification";
                            //return resultMessage;
                        }
                    }

                    #endregion

                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Text = "Record Saved Sucessfully";
                    resultMessage.Result = 1;
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.Text = "Exception Error While Record Save : SaveVehicleSpotEntry";
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }

        }

        public  int InsertVehicleSpotEntry(TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehicleDetailsDAO.SaveVehicleSpotEntry(tblPurchaseVehicleDetailsTO, conn, tran);
        }

        #endregion

        #region Updation
        public  int UpdateTblPurchaseScheduleDetails(TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsTO)
        {
            return _iTblPurchaseVehicleDetailsDAO.UpdateTblPurchaseScheduleDetails(tblPurchaseVehicleDetailsTO);
        }

        public  int UpdateTblPurchaseScheduleDetails(TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehicleDetailsDAO.UpdateTblPurchaseScheduleDetails(tblPurchaseVehicleDetailsTO, conn, tran);
        }
        #endregion

        #region Deletion
        public  int DeleteTblPurchaseVehicleDetails(Int32 idSchedulePurchase, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehicleDetailsDAO.DeleteTblPurchaseVehicleDetails(idSchedulePurchase, conn, tran);
        }
        public  int DeleteTblPurchaseVehicleDetails(Int32 idSchedulePurchase)
        {
            return _iTblPurchaseVehicleDetailsDAO.DeleteTblPurchaseVehicleDetails(idSchedulePurchase);
        }

        public  int DeleteTblPurchaseVehicleDetailsByScheduleId(Int32 scheduleId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehicleDetailsDAO.DeleteTblPurchaseVehicleDetails2(scheduleId, conn, tran);
        }

        public  int DeleteAllVehicleItemDtlsAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehicleDetailsDAO.DeleteAllVehicleItemDtlsAgainstVehSchedule(purchaseScheduleId, conn, tran);
        }

        #endregion

    }
}
