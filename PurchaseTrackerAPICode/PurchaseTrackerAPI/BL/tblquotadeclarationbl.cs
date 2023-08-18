using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblQuotaDeclarationBL : Itblquotadeclarationbl
    {
        #region Selection
        private readonly IConnectionString _iConnectionString;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly ITblGlobalRateBL _iTblGlobalRateBL;
        private readonly ITblQuotaDeclarationDAO _iTblQuotaDeclarationDAO;
        private readonly Itblalertinstancebl _iTblAlertInstanceBL;
        private readonly Icommondao _iCommonDAO;

        public TblQuotaDeclarationBL(Icommondao icommondao, IConnectionString iConnectionString, ITblQuotaDeclarationDAO iTblQuotaDeclarationDAO, ITblGlobalRateBL iTblGlobalRateBL, ITblConfigParamsBL iTblConfigParamsBL, Itblalertinstancebl iTblAlertInstanceBL)
        {
            _iCommonDAO = icommondao;
            _iConnectionString = iConnectionString;
            _iTblAlertInstanceBL = iTblAlertInstanceBL;
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iTblGlobalRateBL = iTblGlobalRateBL;
            _iTblQuotaDeclarationDAO = iTblQuotaDeclarationDAO;
        }

        public  List<TblQuotaDeclarationTO> SelectAllTblQuotaDeclarationList()
        {
            return _iTblQuotaDeclarationDAO.SelectAllTblQuotaDeclaration();

        }

        public  List<TblQuotaDeclarationTO> SelectAllTblQuotaDeclarationList(Int32 globalRateId)
        {
            return _iTblQuotaDeclarationDAO.SelectAllTblQuotaDeclaration(globalRateId);

        }

        public  TblQuotaDeclarationTO SelectTblQuotaDeclarationTO(Int32 idQuotaDeclaration)
        {
            return _iTblQuotaDeclarationDAO.SelectTblQuotaDeclaration(idQuotaDeclaration);

        }

        public  TblQuotaDeclarationTO SelectPreviousTblQuotaDeclarationTO(Int32 idQuotaDeclaration, Int32 cnfOrgId)
        {
            return _iTblQuotaDeclarationDAO.SelectPreviousTblQuotaDeclarationTO(idQuotaDeclaration, cnfOrgId);

        }

        public  TblQuotaDeclarationTO SelectTblQuotaDeclarationTO(Int32 idQuotaDeclaration, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblQuotaDeclarationDAO.SelectTblQuotaDeclaration(idQuotaDeclaration, conn, tran);

        }

        public  TblQuotaDeclarationTO SelectLatestQuotaDeclarationTO(SqlConnection conn, SqlTransaction tran)
        {
            return _iTblQuotaDeclarationDAO.SelectLatestQuotaDeclarationTO(conn, tran);
        }

        public  List<TblQuotaDeclarationTO> SelectLatestQuotaDeclarationTOList(Int32 cnfId, DateTime date)
        {
            return _iTblQuotaDeclarationDAO.SelectLatestQuotaDeclaration(cnfId, date);

        }

        public  PurchaseTrackerAPI.DashboardModels.QuotaAndRateInfo SelectQuotaAndRateDashboardInfo(Int32 roleId, Int32 orgId, DateTime sysDate)
        {
            try
            {
                return _iTblQuotaDeclarationDAO.SelectDashboardQuotaAndRateInfo(roleId, orgId, sysDate);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public  Boolean CheckForValidityAndReset(TblQuotaDeclarationTO tblQuotaDeclarationTO)
        {
            TblQuotaDeclarationTO prevQuotaDeclaTO = SelectPreviousTblQuotaDeclarationTO(tblQuotaDeclarationTO.IdQuotaDeclaration, tblQuotaDeclarationTO.OrgId);
            if (prevQuotaDeclaTO == null)
                return true;
            else
            {
                DateTime timeToCheck = prevQuotaDeclaTO.QuotaAllocDate.AddMinutes(tblQuotaDeclarationTO.ValidUpto);
                DateTime serverDateTime =  _iCommonDAO.ServerDateTime;
                if (timeToCheck < serverDateTime)
                {
                    tblQuotaDeclarationTO.IsActive = 0;
                    TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_SYTEM_ADMIN_USER_ID);

                    tblQuotaDeclarationTO.UpdatedBy = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
                    tblQuotaDeclarationTO.UpdatedOn = serverDateTime;
                    int result = UpdateTblQuotaDeclaration(tblQuotaDeclarationTO);
                    return false;
                }
                else return true;
            }
        }
        #endregion

        #region Insertion
        public  int InsertTblQuotaDeclaration(TblQuotaDeclarationTO tblQuotaDeclarationTO)
        {
            return _iTblQuotaDeclarationDAO.InsertTblQuotaDeclaration(tblQuotaDeclarationTO);
        }

        public  int InsertTblQuotaDeclaration(TblQuotaDeclarationTO tblQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblQuotaDeclarationDAO.InsertTblQuotaDeclaration(tblQuotaDeclarationTO, conn, tran);
        }


        /// <summary>
        /// Sanjay [2017-02-10] To Save the Declared Rate and Allocated Quota of C&F Agents
        /// </summary>
        /// <param name="quotaList"></param>
        /// <param name="tblGlobalRateTO"></param>
        /// <returns></returns>
        public  int SaveDeclaredRateAndAllocatedQuota(List<TblQuotaDeclarationTO> quotaExtList, List<TblQuotaDeclarationTO> quotaList, TblGlobalRateTO tblGlobalRateTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                #region 1. Save the Declared Rate

                Boolean isRateAlreadyDeclare = _iTblGlobalRateBL.IsRateAlreadyDeclaredForTheDate(tblGlobalRateTO.CreatedOn, conn, tran);

                //This condition means if new declared quota is not found then new rate can not be declared
                if (quotaList != null && quotaList.Count > 0)
                {
                    if (tblGlobalRateTO.RateReasonDesc != "Other")
                        tblGlobalRateTO.Comments = tblGlobalRateTO.RateReasonDesc;

                    result = _iTblGlobalRateBL.InsertTblGlobalRate(tblGlobalRateTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        return 0;
                    }
                }

                #endregion

                #region 2. Deactivate All Previous Declared Quota

                result = _iTblQuotaDeclarationDAO.DeactivateAllDeclaredQuota(tblGlobalRateTO.CreatedBy, conn, tran);
                if (result == -1)
                {
                    tran.Rollback();
                    return 0;
                }

                #endregion

                #region 3. Update Existing Quota for Validity

                for (int i = 0; i < quotaExtList.Count; i++)
                {
                    TblQuotaDeclarationTO tblQuotaDeclarationTO = quotaExtList[i];

                    result = _iTblQuotaDeclarationDAO.UpdateQuotaDeclarationValidity(tblQuotaDeclarationTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        return 0;
                    }
                }

                #endregion

                #region 4. Save C&F Allocated Quota

                List<TblSmsTO> smsTOList = new List<TblSmsTO>();
                for (int i = 0; i < quotaList.Count; i++)
                {
                    TblQuotaDeclarationTO tblQuotaDeclarationTO = quotaList[i];
                    tblQuotaDeclarationTO.GlobalRateId = tblGlobalRateTO.IdGlobalRate;
                    tblQuotaDeclarationTO.BalanceQty = tblQuotaDeclarationTO.AllocQty;
                    tblQuotaDeclarationTO.CalculatedRate = tblGlobalRateTO.Rate - tblQuotaDeclarationTO.RateBand;
                    result = InsertTblQuotaDeclaration(tblQuotaDeclarationTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        return 0;
                    }

                    if (tblQuotaDeclarationTO.Tag != null && tblQuotaDeclarationTO.Tag.GetType() == typeof(TblOrganizationTO))
                    {
                        if (!string.IsNullOrEmpty(((TblOrganizationTO)tblQuotaDeclarationTO.Tag).RegisteredMobileNos))
                        {
                            TblSmsTO smsTO = new TblSmsTO();
                            smsTO.MobileNo = ((TblOrganizationTO)tblQuotaDeclarationTO.Tag).RegisteredMobileNos;


                            smsTO.SourceTxnDesc = "Quota & Rate Declaration";
                            String reasonDesc = tblGlobalRateTO.RateReasonDesc;
                            if (tblGlobalRateTO.RateReasonDesc == "Other")
                                reasonDesc = tblGlobalRateTO.Comments;

                            if (isRateAlreadyDeclare)
                                smsTO.SmsTxt = "New Rate and Quota is declared. Rate = " + tblGlobalRateTO.Rate + " Rs/MT , Reason : " + reasonDesc;
                            else
                                smsTO.SmsTxt = "Today's Rate and Quota is declared. Rate = " + tblGlobalRateTO.Rate + " Rs/MT , Reason : " + reasonDesc;

                            smsTOList.Add(smsTO);
                        }
                    }
                }

                #endregion

                #region 5. Send Notifications Via SMS Or Email To All C&F

                TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.NEW_RATE_AND_QUOTA_DECLARED;
                tblAlertInstanceTO.AlertAction = "NEW_RATE_AND_QUOTA_DECLARED";
                if (!isRateAlreadyDeclare)
                    tblAlertInstanceTO.AlertComment = "Today's Rate is Declared. Rate = " + tblGlobalRateTO.Rate + " (Rs/MT)";
                else
                    tblAlertInstanceTO.AlertComment = "New Rate is Declared. Rate = " + tblGlobalRateTO.Rate + " (Rs/MT)";

                tblAlertInstanceTO.EffectiveFromDate = tblGlobalRateTO.CreatedOn;
                tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                tblAlertInstanceTO.IsActive = 1;
                tblAlertInstanceTO.SourceDisplayId = "NEW_RATE_AND_QUOTA_DECLARED";
                tblAlertInstanceTO.SourceEntityId = tblGlobalRateTO.IdGlobalRate;
                tblAlertInstanceTO.RaisedBy = tblGlobalRateTO.CreatedBy;
                tblAlertInstanceTO.RaisedOn = tblGlobalRateTO.CreatedOn;
                tblAlertInstanceTO.IsAutoReset = 1;
                if (smsTOList != null)
                {
                    tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();
                    tblAlertInstanceTO.SmsTOList = smsTOList;
                }

                String alertDefIds = (int)NotificationConstants.NotificationsE.NEW_RATE_AND_QUOTA_DECLARED + "," + (int)NotificationConstants.NotificationsE.BOOKINGS_CLOSED;
                result = _iTblAlertInstanceBL.ResetAlertInstanceByDef(alertDefIds, conn, tran);
                if (result < 0)
                {
                    tran.Rollback();
                    return 0;
                }

                ResultMessage rMessage = _iTblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                if (rMessage.MessageType != ResultMessageE.Information)
                {
                    tran.Rollback();
                    return 0;
                }
                #endregion

                // #region 6. Update booking Status As OPEN

                // TblBookingActionsTO existinBookingActionsTO = BL.TblBookingActionsBL.SelectLatestBookingActionTO(conn, tran);
                // if (existinBookingActionsTO == null || existinBookingActionsTO.BookingStatus == "CLOSE")
                // {
                //     TblBookingActionsTO bookingActionTO = new TblBookingActionsTO();
                //     bookingActionTO.BookingStatus = "OPEN";
                //     bookingActionTO.IsAuto = 1;
                //     bookingActionTO.StatusBy = tblGlobalRateTO.CreatedBy;
                //     bookingActionTO.StatusDate = tblGlobalRateTO.CreatedOn;

                //     result = BL.TblBookingActionsBL.InsertTblBookingActions(bookingActionTO, conn, tran);
                //     if (result != 1)
                //     {
                //         tran.Rollback();
                //         return 0;
                //     }
                // }
                // #endregion

                tran.Commit();
                return 1;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public  ResultMessage SaveDeclaredRateAndAllocatedBand(List<TblGlobalRateTO> tblGlobalRateTOList)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                #region 1. Save the Declared Rate

                Boolean isRateAlreadyDeclare = _iTblGlobalRateBL.IsRateAlreadyDeclaredForTheDate(tblGlobalRateTOList[0].CreatedOn, conn, tran);
                String rateString = string.Empty;
                //This condition means if new declared quota is not found then new rate can not be declared
                if (tblGlobalRateTOList != null && tblGlobalRateTOList.Count > 0)
                {
                    #region 1.1 Deactivate All Previous Declared Quota

                    result = _iTblQuotaDeclarationDAO.DeactivateAllDeclaredQuota(tblGlobalRateTOList[0].CreatedBy, conn, tran);
                    if (result == -1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("DeactivateAllDeclaredQuota");
                        return resultMessage;
                    }

                    #endregion

                    for (int i = 0; i < tblGlobalRateTOList.Count; i++)
                    {
                        TblGlobalRateTO tblGlobalRateTO = tblGlobalRateTOList[i];
                        //commented by swati
                        //   rateString += tblGlobalRateTO.BrandName + " " + tblGlobalRateTO.Rate + ", ";
                        if (tblGlobalRateTO.RateReasonDesc != "Other")
                            tblGlobalRateTO.Comments = tblGlobalRateTO.RateReasonDesc;

                        result = _iTblGlobalRateBL.InsertTblGlobalRate(tblGlobalRateTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While InsertTblGlobalRate");
                            return resultMessage;
                        }

                        #region 1.2. Save C&F Allocated Rate Band
                        if (tblGlobalRateTO.QuotaDeclarationTOList != null && tblGlobalRateTO.QuotaDeclarationTOList.Count > 0)
                        {
                            for (int qd = 0; qd < tblGlobalRateTO.QuotaDeclarationTOList.Count; qd++)
                            {
                                TblQuotaDeclarationTO tblQuotaDeclarationTO = tblGlobalRateTO.QuotaDeclarationTOList[qd];
                                tblQuotaDeclarationTO.GlobalRateId = tblGlobalRateTO.IdGlobalRate;
                                tblQuotaDeclarationTO.BalanceQty = tblQuotaDeclarationTO.AllocQty;
                                tblQuotaDeclarationTO.CalculatedRate = tblGlobalRateTO.Rate - tblQuotaDeclarationTO.RateBand;
                                result = InsertTblQuotaDeclaration(tblQuotaDeclarationTO, conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour("Error While InsertTblQuotaDeclaration");
                                    return resultMessage;
                                }
                            }
                        }
                        #endregion
                    }
                }

                #endregion
                 //added condition by swati if list will be null
                if (tblGlobalRateTOList[0].QuotaDeclarationTOList != null)
                {
                    #region 2. Prepare SMS List

                    List<TblSmsTO> smsTOList = new List<TblSmsTO>();
                    rateString = rateString.TrimEnd(',');
                    for (int i = 0; i < tblGlobalRateTOList[0].QuotaDeclarationTOList.Count; i++)
                    {
                        TblQuotaDeclarationTO tblQuotaDeclarationTO = tblGlobalRateTOList[0].QuotaDeclarationTOList[i];
                        if (tblQuotaDeclarationTO.Tag != null && tblQuotaDeclarationTO.Tag.GetType() == typeof(TblOrganizationTO))
                        {
                            if (!string.IsNullOrEmpty(((TblOrganizationTO)tblQuotaDeclarationTO.Tag).RegisteredMobileNos))
                            {
                                TblSmsTO smsTO = new TblSmsTO();
                                smsTO.MobileNo = ((TblOrganizationTO)tblQuotaDeclarationTO.Tag).RegisteredMobileNos;


                                smsTO.SourceTxnDesc = "Quota & Rate Declaration";
                                String reasonDesc = tblGlobalRateTOList[0].RateReasonDesc;
                                if (tblGlobalRateTOList[0].RateReasonDesc == "Other")
                                    reasonDesc = tblGlobalRateTOList[0].Comments;
                                if (isRateAlreadyDeclare)
                                    //[12/12/2017] Vijaymala :Commented the code because rate reason is not mandatory
                                    smsTO.SmsTxt = "New Rate is declared. Rate = " + rateString + " Rs/MT ";//, Reason : " + reasonDesc;
                                else
                                    smsTO.SmsTxt = "Today's Rate is declared. Rate = " + rateString + " Rs/MT ";//, Reason : " + reasonDesc;

                                smsTOList.Add(smsTO);
                            }
                        }
                    }
                    #endregion

                    #region 3. Send Notifications Via SMS Or Email To All C&F

                    TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                    tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.NEW_RATE_AND_QUOTA_DECLARED;
                    tblAlertInstanceTO.AlertAction = "NEW_RATE_AND_QUOTA_DECLARED";
                    if (!isRateAlreadyDeclare)
                        tblAlertInstanceTO.AlertComment = "Today's Rate is Declared. Rate = " + rateString + " (Rs/MT)";
                    else
                        tblAlertInstanceTO.AlertComment = "New Rate is Declared. Rate = " + rateString + " (Rs/MT)";

                    tblAlertInstanceTO.EffectiveFromDate = tblGlobalRateTOList[0].CreatedOn;
                    tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                    tblAlertInstanceTO.IsActive = 1;
                    tblAlertInstanceTO.SourceDisplayId = "NEW_RATE_AND_QUOTA_DECLARED";
                    tblAlertInstanceTO.SourceEntityId = tblGlobalRateTOList[0].IdGlobalRate;
                    tblAlertInstanceTO.RaisedBy = tblGlobalRateTOList[0].CreatedBy;
                    tblAlertInstanceTO.RaisedOn = tblGlobalRateTOList[0].CreatedOn;
                    tblAlertInstanceTO.IsAutoReset = 1;
                    if (smsTOList != null)
                    {
                        tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();
                        tblAlertInstanceTO.SmsTOList = smsTOList;
                    }

                    String alertDefIds = (int)NotificationConstants.NotificationsE.NEW_RATE_AND_QUOTA_DECLARED + "," + (int)NotificationConstants.NotificationsE.BOOKINGS_CLOSED;
                    result = _iTblAlertInstanceBL.ResetAlertInstanceByDef(alertDefIds, conn, tran);
                    if (result < 0)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Error While ResetAlertInstanceByDef");
                        return resultMessage;
                    }

                    ResultMessage rMessage = _iTblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                    if (rMessage.MessageType != ResultMessageE.Information)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Error While SaveNewAlertInstance");
                        return resultMessage;
                    }

                    #endregion
                }
                #region 4. Update booking Status As OPEN

                // TblBookingActionsTO existinBookingActionsTO = BL.TblBookingActionsBL.SelectLatestBookingActionTO(conn, tran);
                // if (existinBookingActionsTO == null || existinBookingActionsTO.BookingStatus == "CLOSE")
                // {
                //     TblBookingActionsTO bookingActionTO = new TblBookingActionsTO();
                //     bookingActionTO.BookingStatus = "OPEN";
                //     bookingActionTO.IsAuto = 1;
                //     bookingActionTO.StatusBy = tblGlobalRateTOList[0].CreatedBy;
                //     bookingActionTO.StatusDate = tblGlobalRateTOList[0].CreatedOn;

                //     result = BL.TblBookingActionsBL.InsertTblBookingActions(bookingActionTO, conn, tran);
                //     if (result != 1)
                //     {
                //         tran.Rollback();
                //         resultMessage.DefaultBehaviour("InsertTblBookingActions");
                //         return resultMessage;
                //     }
                // }
                #endregion

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SaveDeclaredRateAndAllocatedBand");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region Updation
        public  int UpdateTblQuotaDeclaration(TblQuotaDeclarationTO tblQuotaDeclarationTO)
        {
            return _iTblQuotaDeclarationDAO.UpdateTblQuotaDeclaration(tblQuotaDeclarationTO);
        }

        public  int UpdateTblQuotaDeclaration(TblQuotaDeclarationTO tblQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblQuotaDeclarationDAO.UpdateTblQuotaDeclaration(tblQuotaDeclarationTO, conn, tran);
        }

        #endregion

        #region Deletion
        public  int DeleteTblQuotaDeclaration(Int32 idQuotaDeclaration)
        {
            return _iTblQuotaDeclarationDAO.DeleteTblQuotaDeclaration(idQuotaDeclaration);
        }

        public  int DeleteTblQuotaDeclaration(Int32 idQuotaDeclaration, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblQuotaDeclarationDAO.DeleteTblQuotaDeclaration(idQuotaDeclaration, conn, tran);
        }

        #endregion

    }
}
