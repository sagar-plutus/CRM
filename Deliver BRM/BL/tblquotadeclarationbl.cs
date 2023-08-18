using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using System.Linq;

namespace SalesTrackerAPI.BL
{
    public class TblQuotaDeclarationBL
    {
        #region Selection
       

        public static List<TblQuotaDeclarationTO> SelectAllTblQuotaDeclarationList()
        {
            return TblQuotaDeclarationDAO.SelectAllTblQuotaDeclaration();           


        }

        public static TblQuotaDeclarationTO SelectTblQuotaDeclarationTO(Int32 idQuotaDeclaration)
        {
            return  TblQuotaDeclarationDAO.SelectTblQuotaDeclaration(idQuotaDeclaration);
          
        }

        public static TblQuotaDeclarationTO SelectPreviousTblQuotaDeclarationTO(Int32 idQuotaDeclaration , Int32 cnfOrgId)
        {
            return TblQuotaDeclarationDAO.SelectPreviousTblQuotaDeclarationTO(idQuotaDeclaration,cnfOrgId);

        }

        public static TblQuotaDeclarationTO SelectTblQuotaDeclarationTO(Int32 idQuotaDeclaration,SqlConnection conn,SqlTransaction tran)
        {
            return TblQuotaDeclarationDAO.SelectTblQuotaDeclaration(idQuotaDeclaration,conn,tran);

        }

        public static TblQuotaDeclarationTO SelectLatestQuotaDeclarationTO(SqlConnection conn, SqlTransaction tran)
        {
            return TblQuotaDeclarationDAO.SelectLatestQuotaDeclarationTO(conn, tran);
        }

        public static List<TblQuotaDeclarationTO> SelectLatestQuotaDeclarationTOList(Int32 cnfId,DateTime date)
        {
            return TblQuotaDeclarationDAO.SelectLatestQuotaDeclaration(cnfId,date);

        }

        public static SalesTrackerAPI.DashboardModels.QuotaAndRateInfo SelectQuotaAndRateDashboardInfo(Int32 roleTypeId, Int32 orgId, DateTime sysDate)
        {
            try
            {
                return TblQuotaDeclarationDAO.SelectDashboardQuotaAndRateInfo(roleTypeId, orgId, sysDate);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static Boolean CheckForValidityAndReset(TblQuotaDeclarationTO tblQuotaDeclarationTO)
        {
            TblQuotaDeclarationTO prevQuotaDeclaTO = BL.TblQuotaDeclarationBL.SelectPreviousTblQuotaDeclarationTO(tblQuotaDeclarationTO.IdQuotaDeclaration, tblQuotaDeclarationTO.OrgId);
            if (prevQuotaDeclaTO == null)
                return true;
            else
            {
                DateTime timeToCheck = prevQuotaDeclaTO.QuotaAllocDate.AddMinutes(tblQuotaDeclarationTO.ValidUpto);
                DateTime serverDateTime = Constants.ServerDateTime;
                if (timeToCheck < serverDateTime)
                {
                    tblQuotaDeclarationTO.IsActive = 0;
                    TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_SYTEM_ADMIN_USER_ID);

                    tblQuotaDeclarationTO.UpdatedBy = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
                    tblQuotaDeclarationTO.UpdatedOn = serverDateTime;
                    int result = BL.TblQuotaDeclarationBL.UpdateTblQuotaDeclaration(tblQuotaDeclarationTO);
                    return false;
                }
                else return true;
            }
        }
        #endregion

        #region Insertion

        public static int InsertTblQuotaDeclaration(TblQuotaDeclarationTO tblQuotaDeclarationTO)
        {
            return TblQuotaDeclarationDAO.InsertTblQuotaDeclaration(tblQuotaDeclarationTO);
        }

        public static int InsertTblQuotaDeclaration(TblQuotaDeclarationTO tblQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblQuotaDeclarationDAO.InsertTblQuotaDeclaration(tblQuotaDeclarationTO, conn, tran);
        }


        /// <summary>
        /// Sanjay [2017-02-10] To Save the Declared Rate and Allocated Quota of C&F Agents
        /// </summary>
        /// <param name="quotaList"></param>
        /// <param name="tblGlobalRateTO"></param>
        /// <returns></returns>
        public static int SaveDeclaredRateAndAllocatedQuota(List<TblQuotaDeclarationTO> quotaExtList, List<TblQuotaDeclarationTO> quotaList, TblGlobalRateTO tblGlobalRateTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                #region 1. Save the Declared Rate

                Boolean isRateAlreadyDeclare = BL.TblGlobalRateBL.IsRateAlreadyDeclaredForTheDate(tblGlobalRateTO.CreatedOn, conn, tran);

                //This condition means if new declared quota is not found then new rate can not be declared
                if (quotaList != null && quotaList.Count > 0)
                {
                    if (tblGlobalRateTO.RateReasonDesc != "Other")
                        tblGlobalRateTO.Comments = tblGlobalRateTO.RateReasonDesc;

                    result = BL.TblGlobalRateBL.InsertTblGlobalRate(tblGlobalRateTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        return 0;
                    }
                }

                #endregion

                #region 2. Deactivate All Previous Declared Quota

                result = TblQuotaDeclarationDAO.DeactivateAllDeclaredQuota(tblGlobalRateTO.CreatedBy, conn, tran);
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

                    result = TblQuotaDeclarationDAO.UpdateQuotaDeclarationValidity(tblQuotaDeclarationTO, conn, tran);
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

                    //[17/01/2018]Added to send sms for cnf subsidary number
                    List<String> mobileNoList = new List<String>();

                    if (tblQuotaDeclarationTO.Tag != null && tblQuotaDeclarationTO.Tag.GetType() == typeof(TblOrganizationTO))
                    {
                        //if (!string.IsNullOrEmpty(((TblOrganizationTO)tblQuotaDeclarationTO.Tag).RegisteredMobileNos))
                        if(true)
                        {
                            List<TblPersonTO> tblPersonTOList = BL.TblPersonBL.SelectAllPersonListByOrganization(tblQuotaDeclarationTO.OrgId);

                            if (tblPersonTOList == null || tblPersonTOList.Count == 0)
                            {
                                tblPersonTOList = new List<TblPersonTO>();
                            }
                            TblPersonTO tblPersonTORegMobNo = new TblPersonTO();
                            tblPersonTORegMobNo.MobileNo = ((TblOrganizationTO)tblQuotaDeclarationTO.Tag).RegisteredMobileNos;

                            if (!String.IsNullOrEmpty(tblPersonTORegMobNo.MobileNo))
                            {
                                tblPersonTOList.Add(tblPersonTORegMobNo);
                            }

                            if (tblPersonTOList != null && tblPersonTOList.Count > 0)
                            {
                                for (int k = 0; k < tblPersonTOList.Count; k++)
                                {
                                   
                                    if (!mobileNoList.Contains(tblPersonTOList[k].MobileNo))
                                    {
                                        TblSmsTO smsTO = new TblSmsTO();
                                        smsTO.MobileNo = tblPersonTOList[k].MobileNo;
                                        PrepareSmsObject(tblGlobalRateTO, isRateAlreadyDeclare, tblQuotaDeclarationTO, mobileNoList, smsTO);
                                        smsTOList.Add(smsTO);
                                    }
                                    if (!mobileNoList.Contains(tblPersonTOList[k].AlternateMobNo))
                                    {
                                        TblSmsTO smsTO = new TblSmsTO();
                                        smsTO.MobileNo = tblPersonTOList[k].AlternateMobNo;
                                        PrepareSmsObject(tblGlobalRateTO, isRateAlreadyDeclare, tblQuotaDeclarationTO, mobileNoList, smsTO);
                                        smsTOList.Add(smsTO);
                                    }
                                }
                            }
                        }
                    }
                }
                //[17/01/2018]Added to send sms for role manager,director,loading person mobile number


                //These Role are use to send reason other wise use alert configuration to send sms
                Dictionary<Int32, List<string>> roleDCT = new Dictionary<int, List<string>>();
                
                //Commented by PRIYANKA [22-08-2018] and added the rolesIds on the setting basis.
                //String roleIds = ((int) Constants.SystemRolesE.DIRECTOR + "," + (int)Constants.SystemRolesE.LOADING_PERSON + "," + (int)Constants.SystemRolesE.REGIONAL_MANAGER);

                string roleIds = string.Empty;
                TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_ROLES_TO_SEND_SMS_ABOUT_RATE_AND_QUOTA);
                if (tblConfigParamsTO != null)
                {
                    roleIds = tblConfigParamsTO.ConfigParamVal;
                }

                roleDCT = BL.TblUserBL.SelectUserMobileNoAndAlterMobileDCTByUserIdOrRole(roleIds, false, conn, tran);

                if (roleDCT != null)
                {
                    foreach (var item in roleDCT.Keys)
                    {
                        List<string> list = roleDCT[item];
                        if (list != null && list.Count > 0)
                        {
                            for (int mn = 0; mn < list.Count; mn++)
                            {
                                TblSmsTO smsTOExist = smsTOList.Where(w => w.MobileNo == list[mn]).FirstOrDefault();
                                if (smsTOExist == null)
                                {
                                    TblSmsTO smsTO = new TblSmsTO();
                                    smsTO.MobileNo = list[mn];
                                    smsTO.SourceTxnDesc = "Quota & Rate Declaration";
                                    String reasonDesc = tblGlobalRateTO.RateReasonDesc;
                                    if (tblGlobalRateTO.RateReasonDesc == "Other")
                                        reasonDesc = tblGlobalRateTO.Comments;

                                    if (isRateAlreadyDeclare)
                                        smsTO.SmsTxt = "New Rate and Quota is declared. Rate = " + tblGlobalRateTO.Rate + " Rs/MT , Reason : " + reasonDesc + " , Your Quota : " + 0 + " MT";
                                    else
                                        smsTO.SmsTxt = "Today's Rate and Quota is declared. Rate = " + tblGlobalRateTO.Rate + " Rs/MT , Reason : " + reasonDesc + " , Your Quota : " + 0 + " MT";

                                    smsTOList.Add(smsTO);
                                }
                            }
                        }
                    }
                }

                #endregion

                #region 5. Send Notifications Via SMS Or Email To All C&F

                TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.NEW_RATE_AND_QUOTA_DECLARED;
                tblAlertInstanceTO.AlertAction = "NEW_RATE_AND_QUOTA_DECLARED";
                if (!isRateAlreadyDeclare)
                    tblAlertInstanceTO.AlertComment = "Today's Rate and Quota is Declared. Rate = " + tblGlobalRateTO.Rate + " (Rs/MT)";
                else
                    tblAlertInstanceTO.AlertComment = "New Rate and Quota is Declared. Rate = " + tblGlobalRateTO.Rate + " (Rs/MT)";

                tblAlertInstanceTO.EffectiveFromDate = tblGlobalRateTO.CreatedOn;
                tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                tblAlertInstanceTO.IsActive = 1;
                tblAlertInstanceTO.SourceDisplayId= "NEW_RATE_AND_QUOTA_DECLARED";
                tblAlertInstanceTO.SourceEntityId = tblGlobalRateTO.IdGlobalRate;
                tblAlertInstanceTO.RaisedBy = tblGlobalRateTO.CreatedBy;
                tblAlertInstanceTO.RaisedOn = tblGlobalRateTO.CreatedOn;
                tblAlertInstanceTO.IsAutoReset = 1;
                if(smsTOList!=null)
                {
                    tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();
                    tblAlertInstanceTO.SmsTOList = smsTOList;
                }

                String alertDefIds = (int)NotificationConstants.NotificationsE.NEW_RATE_AND_QUOTA_DECLARED + "," + (int)NotificationConstants.NotificationsE.BOOKINGS_CLOSED;
                result = BL.TblAlertInstanceBL.ResetAlertInstanceByDef(alertDefIds, conn, tran);
                if (result < 0)
                {
                    tran.Rollback();
                    return 0;
                }

                ResultMessage rMessage = BL.TblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                if(rMessage.MessageType!= ResultMessageE.Information)
                {
                    tran.Rollback();
                    return 0;
                }
                #endregion

                #region 6. Update booking Status As OPEN

                TblBookingActionsTO existinBookingActionsTO = BL.TblBookingActionsBL.SelectLatestBookingActionTO(conn, tran);
                if (existinBookingActionsTO==null || existinBookingActionsTO.BookingStatus == "CLOSE")
                {
                    TblBookingActionsTO bookingActionTO = new TblBookingActionsTO();
                    bookingActionTO.BookingStatus = "OPEN";
                    bookingActionTO.IsAuto = 1;
                    bookingActionTO.StatusBy = tblGlobalRateTO.CreatedBy;
                    bookingActionTO.StatusDate = tblGlobalRateTO.CreatedOn;

                    result = BL.TblBookingActionsBL.InsertTblBookingActions(bookingActionTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        return 0;
                    }
                }
                #endregion

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

        private static void PrepareSmsObject(TblGlobalRateTO tblGlobalRateTO, bool isRateAlreadyDeclare, TblQuotaDeclarationTO tblQuotaDeclarationTO, List<string> mobileNoList, TblSmsTO smsTO)
        {
            smsTO.SourceTxnDesc = "Quota & Rate Declaration";
            String reasonDesc = tblGlobalRateTO.RateReasonDesc;
            if (tblGlobalRateTO.RateReasonDesc == "Other")
                reasonDesc = tblGlobalRateTO.Comments;

            if (isRateAlreadyDeclare)
                smsTO.SmsTxt = "New Rate and Quota is declared. Rate = " + tblGlobalRateTO.Rate + " Rs/MT , Reason : " + reasonDesc + " , Your Quota : " + tblQuotaDeclarationTO.AllocQty + " MT";
            else
                smsTO.SmsTxt = "Today's Rate and Quota is declared. Rate = " + tblGlobalRateTO.Rate + " Rs/MT , Reason : " + reasonDesc + " , Your Quota : " + tblQuotaDeclarationTO.AllocQty + " MT";

            mobileNoList.Add(smsTO.MobileNo);
        }

        #endregion

        #region Updation
        public static int UpdateTblQuotaDeclaration(TblQuotaDeclarationTO tblQuotaDeclarationTO)
        {
            return TblQuotaDeclarationDAO.UpdateTblQuotaDeclaration(tblQuotaDeclarationTO);
        }

        public static int UpdateTblQuotaDeclaration(TblQuotaDeclarationTO tblQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblQuotaDeclarationDAO.UpdateTblQuotaDeclaration(tblQuotaDeclarationTO, conn, tran);
        }


        /// <summary>
        /// Sanjay [03 aug 2018] While booking it is observed that elapsed quota is get activated if new rate is declared.
        /// hence update only pending qty
        /// </summary>
        /// <param name="tblQuotaDeclarationTO"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static int UpdateBalanceQuotaQty(TblQuotaDeclarationTO tblQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblQuotaDeclarationDAO.UpdateBalanceQuotaQty(tblQuotaDeclarationTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblQuotaDeclaration(Int32 idQuotaDeclaration)
        {
            return TblQuotaDeclarationDAO.DeleteTblQuotaDeclaration(idQuotaDeclaration);
        }

        public static int DeleteTblQuotaDeclaration(Int32 idQuotaDeclaration, SqlConnection conn, SqlTransaction tran)
        {
            return TblQuotaDeclarationDAO.DeleteTblQuotaDeclaration(idQuotaDeclaration, conn, tran);
        }

        #endregion
        
    }
}
