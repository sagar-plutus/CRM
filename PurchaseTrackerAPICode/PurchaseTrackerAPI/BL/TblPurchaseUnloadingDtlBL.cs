using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL;
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
    public class TblPurchaseUnloadingDtlBL : ITblPurchaseUnloadingDtlBL
    {
        private readonly IConnectionString _iConnectionString;
        private readonly ITblPurchaseWeighingStageSummaryBL _iTblPurchaseWeighingStageSummaryBL;
        private readonly ITblQualityPhaseBL _iTblQualityPhaseBL;
        private readonly ICircularDependancyBL _iTblPurchaseScheduleSummaryBL;
        private readonly Idimensionbl _idimensionBL;
        private readonly ITblPurchaseUnloadingDtlDAO _iTblPurchaseUnloadingDtlDAO;
        private readonly ITblPurchaseVehicleStageCntBL _iTblPurchaseVehicleStageCntBL;
        private readonly INotification notify;
        private readonly Icommondao _iCommonDAO;
        public TblPurchaseUnloadingDtlBL(Idimensionbl idimensionBL,
            INotification inotify,
            Icommondao icommondao,
           ICircularDependancyBL iTblPurchaseScheduleSummaryBL
           , ITblQualityPhaseBL iTblQualityPhaseBL
            , IConnectionString iConnectionString
           , ITblPurchaseUnloadingDtlDAO iTblPurchaseUnloadingDtlDAO
           , ITblPurchaseWeighingStageSummaryBL iTblPurchaseWeighingStageSummaryBL
           , ITblPurchaseVehicleStageCntBL iTblPurchaseVehicleStageCntBL)
        {
            _iTblQualityPhaseBL = iTblQualityPhaseBL;
            _iTblPurchaseVehicleStageCntBL = iTblPurchaseVehicleStageCntBL;
            _idimensionBL = idimensionBL;
            _iTblPurchaseWeighingStageSummaryBL = iTblPurchaseWeighingStageSummaryBL;
            _iTblPurchaseScheduleSummaryBL = iTblPurchaseScheduleSummaryBL;
            _iTblPurchaseUnloadingDtlDAO = iTblPurchaseUnloadingDtlDAO;
            notify = inotify;
            _iCommonDAO = icommondao;
            _iConnectionString = iConnectionString;

        }
        #region Selection
        public List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtl()
        {
            return _iTblPurchaseUnloadingDtlDAO.SelectAllTblPurchaseUnloadingDtl();
        }

        public List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtlList()
        {
            return _iTblPurchaseUnloadingDtlDAO.SelectAllTblPurchaseUnloadingDtl();

        }
        public List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtl(string purchaseWeighingStageIdStr)
        {
            return _iTblPurchaseUnloadingDtlDAO.SelectAllTblPurchaseUnloadingDtl(purchaseWeighingStageIdStr);
        }
        public List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtlList(Int32 purchaseWeighingStageId)
        {
            return _iTblPurchaseUnloadingDtlDAO.SelectAllTblPurchaseUnloadingDtl(purchaseWeighingStageId);

        }
        public List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtlList(Int32 purchaseWeighingStageId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseUnloadingDtlDAO.SelectAllTblPurchaseUnloadingDtl(purchaseWeighingStageId, conn, tran);

        }
        public List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtlList(Int32 purchaseWeighingStageId, Int32 isGradingBeforeUnld = 0)
        {
            return _iTblPurchaseUnloadingDtlDAO.SelectAllTblPurchaseUnloadingDtl(purchaseWeighingStageId, isGradingBeforeUnld);

        }

        public List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtlListByScheduleId(Int32 purchaseScheduleId,Int32 isGradingBeforeUnloading, SqlConnection conn = null, SqlTransaction tran = null)
        {
            if (conn != null && tran != null)
            {
                return _iTblPurchaseUnloadingDtlDAO.SelectAllTblPurchaseUnloadingDtlListByScheduleId(purchaseScheduleId,isGradingBeforeUnloading, conn, tran);
            }
            else
            {
                return _iTblPurchaseUnloadingDtlDAO.SelectAllTblPurchaseUnloadingDtlListByScheduleId(purchaseScheduleId,isGradingBeforeUnloading);
            }


        }

        // public  TblPurchaseUnloadingDtlTO SelectTblPurchaseUnloadingDtlTO(Int32 idPurchaseUnloadingDtl)
        // {
        //     DataTable tblPurchaseUnloadingDtlTODT = TblPurchaseUnloadingDtlDAO.SelectTblPurchaseUnloadingDtl(idPurchaseUnloadingDtl);
        //     List<TblPurchaseUnloadingDtlTO> tblPurchaseUnloadingDtlTOList = ConvertDTToList(tblPurchaseUnloadingDtlTODT);
        //     if(tblPurchaseUnloadingDtlTOList != null && tblPurchaseUnloadingDtlTOList.Count == 1)
        //         return tblPurchaseUnloadingDtlTOList[0];
        //     else
        //         return null;
        // }



        #endregion

        #region Insertion
        public int InsertTblPurchaseUnloadingDtl(TblPurchaseUnloadingDtlTO tblPurchaseUnloadingDtlTO)
        {
            return _iTblPurchaseUnloadingDtlDAO.InsertTblPurchaseUnloadingDtl(tblPurchaseUnloadingDtlTO);
        }

        public int InsertTblPurchaseUnloadingDtl(TblPurchaseUnloadingDtlTO tblPurchaseUnloadingDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseUnloadingDtlDAO.InsertTblPurchaseUnloadingDtl(tblPurchaseUnloadingDtlTO, conn, tran);
        }

        public ResultMessage SaveUnloadingMaterialDetails(List<TblPurchaseUnloadingDtlTO> tblPurchaseUnloadingDtlTOList, Boolean isSendNotification, Boolean isDeletePrevious)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            Int32 result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            Int32 weighingId = 0;
            DateTime currentDate = _iCommonDAO.ServerDateTime;
            Int32 isGradingUnldCompleted = 0;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                if (tblPurchaseUnloadingDtlTOList == null || tblPurchaseUnloadingDtlTOList.Count == 0)
                {
                    throw new Exception("Grade Details not found");
                }

                isGradingUnldCompleted = tblPurchaseUnloadingDtlTOList[0].IsGradingUnldCompleted;


                //Get First List deleted
                if (tblPurchaseUnloadingDtlTOList != null && tblPurchaseUnloadingDtlTOList.Count > 0)
                {
                    weighingId = tblPurchaseUnloadingDtlTOList[0].PurchaseWeighingStageId;

                    if (isDeletePrevious)
                    {
                        List<TblPurchaseUnloadingDtlTO> tblPurchaseUnloadingDtlTOListTemp = SelectAllTblPurchaseUnloadingDtlList(weighingId, conn, tran);
                        if (tblPurchaseUnloadingDtlTOListTemp != null && tblPurchaseUnloadingDtlTOListTemp.Count > 0)
                        {
                            for (int k = 0; k < tblPurchaseUnloadingDtlTOListTemp.Count; k++)
                            {
                                result = DeleteTblPurchaseUnloadingDtl(tblPurchaseUnloadingDtlTOListTemp[k].IdPurchaseUnloadingDtl, conn, tran);
                                if (result <= 0)
                                {
                                    tran.Rollback();
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Result = 0;
                                    resultMessage.Text = "Error While Updating Material Details";
                                    return resultMessage;
                                }

                            }
                        }

                    }

                    for (int i = 0; i < tblPurchaseUnloadingDtlTOList.Count; i++)
                    {
                        tblPurchaseUnloadingDtlTOList[i].CreatedOn = currentDate;
                        result = InsertTblPurchaseUnloadingDtl(tblPurchaseUnloadingDtlTOList[i], conn, tran);
                        if (result <= 0)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Result = 0;
                            resultMessage.Text = "Error While Updating Material Details";
                            return resultMessage;
                        }

                    }
                    //if record is confirmed then update count --Deepali [20-03-2019]
                    if (tblPurchaseUnloadingDtlTOList[0].IsConfirmUnloading > 0 && isDeletePrevious)
                    {
                        TblPurchaseScheduleSummaryTO ScheduleSummaryTO = new TblPurchaseScheduleSummaryTO();
                        ScheduleSummaryTO.RootScheduleId = tblPurchaseUnloadingDtlTOList[0].PurchaseScheduleSummaryId;
                        resultMessage = _iTblPurchaseVehicleStageCntBL.InsertOrUpdateVehicleWtStageCount(ScheduleSummaryTO, null, null, tblPurchaseUnloadingDtlTOList[0], null, conn, tran);
                        result = resultMessage.Result;
                    }


                    //Update isGradingUnldCompleted flag
                    TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = new TblPurchaseScheduleSummaryTO();
                    List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = _iTblPurchaseScheduleSummaryBL.SelectAllEnquiryScheduleSummaryTOByRootScheduleID(tblPurchaseUnloadingDtlTOList[0].PurchaseScheduleSummaryId, true);

                    if (tblPurchaseScheduleSummaryTOList != null && tblPurchaseScheduleSummaryTOList.Count > 0)
                    {
                        tblPurchaseScheduleSummaryTO = tblPurchaseScheduleSummaryTOList[0];

                        tblPurchaseScheduleSummaryTO.IsGradingUnldCompleted = isGradingUnldCompleted;

                        result = _iTblPurchaseScheduleSummaryBL.UpdateIsGradingWhileUnloadingFlag(tblPurchaseScheduleSummaryTO, conn, tran);
                        if(result == -1)
                        {
                            throw new Exception("Error in UpdateIsGradingWhileUnloadingFlag(tblPurchaseScheduleSummaryTO, conn, tran);");
                        }

                    }
                    //chetan[09-April-2020] added for update interval time
                    TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTOTemp = new TblPurchaseWeighingStageSummaryTO();
                    tblPurchaseWeighingStageSummaryTOTemp.IdPurchaseWeighingStage = tblPurchaseUnloadingDtlTOList[0].PurchaseWeighingStageId;

                    tblPurchaseWeighingStageSummaryTOTemp.IntervalTime = Convert.ToInt64(tblPurchaseUnloadingDtlTOList[0].IntervalTime);
                    tblPurchaseWeighingStageSummaryTOTemp.GradingEndTime= _iCommonDAO.ServerDateTime;
                    tblPurchaseWeighingStageSummaryTOTemp.UnloadingConfirmedBy = tblPurchaseUnloadingDtlTOList[0].CreatedBy;
                    result = _iTblPurchaseWeighingStageSummaryBL.UpdateUnlodingEndTime(tblPurchaseWeighingStageSummaryTOTemp, conn, tran);
                    if (result >= 1)
                    {
                        tran.Commit();

                    }
                    else
                    {
                        tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error While Updating Material Details";
                        resultMessage.Result = 0;
                        return resultMessage;
                    }

                    if (result >= 1 && isSendNotification)
                    {

                        List<TblPurchaseScheduleSummaryTO> TOList = new List<TblPurchaseScheduleSummaryTO>();
                        //TOList = BL.TblPurchaseScheduleSummaryBL.SelectAllEnquiryScheduleSummaryTOByRootId(tblPurchaseUnloadingDtlTOList[0].PurchaseScheduleSummaryId, conn, tran);

                        //TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = BL.TblPurchaseScheduleSummaryBL.SelectAllEnquiryScheduleSummaryTO(tblPurchaseUnloadingDtlTOList[0].PurchaseScheduleSummaryId, false, conn, tran);

                     

                        List<TblPurchaseWeighingStageSummaryTO> ListWeighing = _iTblPurchaseWeighingStageSummaryBL.GetVehicleWeighingDetailsBySchduleId(tblPurchaseUnloadingDtlTOList[0].PurchaseScheduleSummaryId, false);
                        string sourceEntityId = null;
                        foreach (var WeighingTo in ListWeighing)
                        {

                            if (sourceEntityId == null)
                            {
                                sourceEntityId = WeighingTo.IdPurchaseWeighingStage.ToString();
                            }
                            else
                            {
                                sourceEntityId = sourceEntityId + "," + WeighingTo.IdPurchaseWeighingStage.ToString();
                            }
                        }

                        List<TblAlertUsersTO> AlertUsersTOList = new List<TblAlertUsersTO>();

                        _iTblQualityPhaseBL.ResetAllPreviousNotification((int)NotificationConstants.NotificationsE.UNLOADING_STAGE_COMPLETED, sourceEntityId);

                        TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                        List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();

                        //TblPurchaseScheduleSummaryTO TO = TOList.Where(w => w.GraderId > 0).FirstOrDefault();
                        if (tblPurchaseScheduleSummaryTO != null && tblPurchaseScheduleSummaryTO.GraderId > 0)
                        {
                            TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                            tblAlertUsersTO.UserId = tblPurchaseScheduleSummaryTO.GraderId;
                            tblAlertUsersTO.RaisedOn = tblPurchaseScheduleSummaryTO.CreatedOn;
                            tblAlertUsersTOList.Add(tblAlertUsersTO);
                        }
                        else
                        {
                            List<DropDownTO> ListRoles = _idimensionBL.SelectAllSystemUsersListFromRoleType(Convert.ToInt32(Constants.SystemRoleTypeE.GRADER));
                            if (ListRoles != null && ListRoles.Count > 0)
                            {
                                foreach (var roleTo in ListRoles)
                                {
                                    TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                                    tblAlertUsersTO.RoleId = roleTo.Value;
                                    tblAlertUsersTO.RaisedOn = tblPurchaseScheduleSummaryTO.CreatedOn;
                                    tblAlertUsersTOList.Add(tblAlertUsersTO);
                                }
                            }
                        }

                        Int32 conversionFact = 1000;

                        //get purchase manager of supplier
                        // tblAlertUsersTOList = new List<TblAlertUsersTO>();
                        int RootScheduleId = tblPurchaseScheduleSummaryTO.ActualRootScheduleId;

                        _iTblQualityPhaseBL.ResetAllPreviousNotification((int)NotificationConstants.NotificationsE.WEIGHING_STAGE_COMPLETED, RootScheduleId.ToString());

                        TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO = _iTblPurchaseWeighingStageSummaryBL.SelectTblPurchaseWeighingStageSummaryTO(weighingId);
                        if (tblPurchaseWeighingStageSummaryTO != null)
                        {
                            tblAlertInstanceTO.AlertComment = "Unloading Stage " + tblPurchaseWeighingStageSummaryTO.WeightStageId + " completed for Vehicle No:" + tblPurchaseWeighingStageSummaryTO.VehicleNo + " with nt. wt-" + tblPurchaseWeighingStageSummaryTO.NetWeightMT / conversionFact + "(MT)";
                        }
                        else
                        {
                            tblAlertInstanceTO.AlertComment = "Unloading Stage completed";
                        }

                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.UNLOADING_STAGE_COMPLETED;
                        tblAlertInstanceTO.AlertAction = "UNLOADING_STAGE_COMPLETED";
                        // tblAlertInstanceTO.AlertComment = "Vehicle Is Reported";

                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                        tblAlertInstanceTO.EffectiveFromDate = currentDate;
                        tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                        tblAlertInstanceTO.IsActive = 1;
                        tblAlertInstanceTO.SourceDisplayId = "UNLOADING_STAGE_COMPLETED";
                        tblAlertInstanceTO.SourceEntityId = tblPurchaseUnloadingDtlTOList[0].PurchaseWeighingStageId;
                        tblAlertInstanceTO.RaisedBy = tblPurchaseUnloadingDtlTOList[0].CreatedBy;
                        tblAlertInstanceTO.RaisedOn = currentDate;
                        tblAlertInstanceTO.IsAutoReset = 1;

                        //Notification notify = new Notification();
                        notify.SendNotificationToUsers(tblAlertInstanceTO);

                        // ResultMessage rMessage = BL.TblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                        // if (rMessage.MessageType != ResultMessageE.Information)
                        // {
                        //     tran.Rollback();
                        //     resultMessage.DefaultBehaviour();
                        //     resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                        //     resultMessage.Text = "Error While Generating Notification";

                        //     return resultMessage;
                        // }
                    }


                }

                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Updated Successfully.";
                resultMessage.Result = 1;
                return resultMessage;

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
        public ResultMessage DeleteUnloadingDetails(List<TblPurchaseUnloadingDtlTO> tblPurchaseUnloadingDtlTOList)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            Int32 result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                //Get First List deleted
                if (tblPurchaseUnloadingDtlTOList != null && tblPurchaseUnloadingDtlTOList.Count > 0)
                {

                    for (int k = 0; k < tblPurchaseUnloadingDtlTOList.Count; k++)
                    {
                        result = DeleteTblPurchaseUnloadingDtl(tblPurchaseUnloadingDtlTOList[k].IdPurchaseUnloadingDtl, conn, tran);
                        if (result <= 0)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Result = 0;
                            resultMessage.Text = "Error While Updating Material Details";
                            return resultMessage;
                        }

                    }
                }

                if (result >= 1)
                {
                    tran.Commit();
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Text = "Record Updated Successfully.";
                    resultMessage.Result = 1;
                    return resultMessage;
                }
                else
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While Updating Material Details";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

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

        #region Updation
        public int UpdateTblPurchaseUnloadingDtl(TblPurchaseUnloadingDtlTO tblPurchaseUnloadingDtlTO)
        {
            return _iTblPurchaseUnloadingDtlDAO.UpdateTblPurchaseUnloadingDtl(tblPurchaseUnloadingDtlTO);
        }

        public int UpdateTblPurchaseUnloadingDtl(TblPurchaseUnloadingDtlTO tblPurchaseUnloadingDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseUnloadingDtlDAO.UpdateTblPurchaseUnloadingDtl(tblPurchaseUnloadingDtlTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblPurchaseUnloadingDtl(Int32 idPurchaseUnloadingDtl)
        {
            return _iTblPurchaseUnloadingDtlDAO.DeleteTblPurchaseUnloadingDtl(idPurchaseUnloadingDtl);
        }

        public int DeleteTblPurchaseUnloadingDtl(Int32 idPurchaseUnloadingDtl, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseUnloadingDtlDAO.DeleteTblPurchaseUnloadingDtl(idPurchaseUnloadingDtl, conn, tran);
        }

        public int DeleteAllUnloadingDtlsAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseUnloadingDtlDAO.DeleteAllUnloadingDtlsAgainstVehSchedule(purchaseScheduleId, conn, tran);
        }




        #endregion

    }
}
