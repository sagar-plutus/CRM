using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.BL
{
    public class TblVisitFollowupInfoBL
    {
        #region Selection
        public static DataTable SelectAllTblVisitFollowupInfo()
        {
            return TblVisitFollowupInfoDAO.SelectAllTblVisitFollowupInfo();
        }

        public static List<TblVisitFollowupInfoTO> SelectAllTblVisitFollowupInfoList()
        {
            DataTable tblVisitFollowupInfoTODT = TblVisitFollowupInfoDAO.SelectAllTblVisitFollowupInfo();
            return ConvertDTToList(tblVisitFollowupInfoTODT);
        }

        public static TblVisitFollowupInfoTO SelectTblVisitFollowupInfoTO(Int32 idProjectFollowUpInfo)
        {
            DataTable tblVisitFollowupInfoTODT = TblVisitFollowupInfoDAO.SelectTblVisitFollowupInfo(idProjectFollowUpInfo);
            List<TblVisitFollowupInfoTO> tblVisitFollowupInfoTOList = ConvertDTToList(tblVisitFollowupInfoTODT);
            if (tblVisitFollowupInfoTOList != null && tblVisitFollowupInfoTOList.Count == 1)
                return tblVisitFollowupInfoTOList[0];
            else
                return null;
        }

        public static List<TblVisitFollowupInfoTO> ConvertDTToList(DataTable tblVisitFollowupInfoTODT)
        {
            List<TblVisitFollowupInfoTO> tblVisitFollowupInfoTOList = new List<TblVisitFollowupInfoTO>();
            if (tblVisitFollowupInfoTODT != null)
            {

            }
            return tblVisitFollowupInfoTOList;
        }

        public static TblVisitFollowupInfoTO SelectVisitFollowupInfoTO(Int32 visitid)
        {
            TblVisitFollowupInfoTO visitFollowupInfoTO = TblVisitFollowupInfoDAO.SelectVisitFollowupInfo(visitid);
            if (visitFollowupInfoTO != null )
                return visitFollowupInfoTO;
            else
                return null;
        }

        #endregion

        #region Insertion
        public static int InsertTblVisitFollowupInfo(TblVisitFollowupInfoTO tblVisitFollowupInfoTO)
        {
            return TblVisitFollowupInfoDAO.InsertTblVisitFollowupInfo(tblVisitFollowupInfoTO);
        }

        public static int InsertTblVisitFollowupInfo(TblVisitFollowupInfoTO tblVisitFollowupInfoTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitFollowupInfoDAO.InsertTblVisitFollowupInfo(tblVisitFollowupInfoTO, conn, tran);
        }

        // Vaibhav [9-Oct-2017] save visit follow up information
        public static ResultMessage SaveVisitFollowUpInfo(TblVisitFollowupInfoTO tblVisitFollowupInfoTO,SqlConnection conn,SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            try
            {
                //Sudhir[20-NOV-2017]Insert New Entrys for FollowUpRoles.
                if (tblVisitFollowupInfoTO.VisitFollowUpInfoNewList != null)
                {
                    foreach (var followUpRole in tblVisitFollowupInfoTO.VisitFollowUpInfoNewList)
                    {
                        if (Convert.ToInt16(followUpRole.Tag) == (int)Constants.VisitFollowUpActionE.ARRANGE_FOR ||
                            Convert.ToInt16(followUpRole.Tag) == (int)Constants.VisitFollowUpActionE.ARRANGE_VISIT_TO)
                        {
                            TblRoleTO tblRoleTO = new TblRoleTO();
                            tblRoleTO.RoleDesc = followUpRole.Text;
                            tblRoleTO.IsActive = 1;
                            tblRoleTO.CreatedOn = tblVisitFollowupInfoTO.CreatedOn;
                            tblRoleTO.CreatedBy = tblVisitFollowupInfoTO.CreatedBy;
                            result = TblVisitFollowUpRolesBL.InsertTblFollowUpRole(tblRoleTO, conn, tran);
                            if (result > 0)
                            {
                                TblVisitFollowUpRolesTO tblVisitFollowUpRolesTO = new TblVisitFollowUpRolesTO();
                                tblVisitFollowUpRolesTO.FollowUpActionId = Convert.ToInt16(followUpRole.Tag);
                                tblVisitFollowUpRolesTO.FollowUpRoleId = tblRoleTO.IdRole;
                                tblVisitFollowUpRolesTO.CreatedBy = tblVisitFollowupInfoTO.CreatedBy;
                                tblVisitFollowUpRolesTO.CreatedOn = tblVisitFollowupInfoTO.CreatedOn;
                                result = TblVisitFollowUpRolesBL.InsertTblVisitFollowUpRoles(tblVisitFollowUpRolesTO, conn, tran);
                                if (result > 0 && Convert.ToInt16(followUpRole.Tag) == (int)Constants.VisitFollowUpActionE.ARRANGE_FOR)
                                {
                                    tblVisitFollowupInfoTO.ArrangeVisitFor = tblRoleTO.IdRole;
                                }
                                else if(result > 0 && Convert.ToInt16(followUpRole.Tag) == (int)Constants.VisitFollowUpActionE.ARRANGE_VISIT_TO)
                                {
                                    tblVisitFollowupInfoTO.ArrangeVisitTo= tblRoleTO.IdRole; 
                                }
                            }
                        }
                    }
                    if(tblVisitFollowupInfoTO.ArrangeVisitFor>0 && tblVisitFollowupInfoTO.ArrangeVisitTo>0)
                    {
                        tblVisitFollowupInfoTO.VisitFollowUpInfoNewList = null;
                    }
                }
                //Sudhir[23-Nov-2017] Added for Inserting New CallBySelfToWhom User
                if (tblVisitFollowupInfoTO.VisitCallBySelfToIsNew == 0 && tblVisitFollowupInfoTO.CallBySelfToWhomId == 0 && tblVisitFollowupInfoTO.VisitCallBySelfToName != null)
                {
                    TblPersonTO personTO = new TblPersonTO();
                    personTO.SalutationId = Constants.DefaultSalutationId; //set default 1
                    String contactPersonName = tblVisitFollowupInfoTO.VisitCallBySelfToName;
                    if (contactPersonName.Contains(' '))
                    {
                        String[] contactPersonNames = contactPersonName.Split(' ');

                        if (contactPersonNames.Length >= 0)
                            personTO.FirstName = contactPersonNames[0] != null ? contactPersonNames[0].ToString() : null;
                        if (contactPersonNames.Length > 1)
                            personTO.LastName = contactPersonNames[1] != null ? contactPersonNames[1].ToString() : null;
                        if (contactPersonNames.Length > 2)
                            personTO.LastName = contactPersonNames[2] != null ? contactPersonNames[2].ToString() : null;
                    }
                    else
                    {
                        personTO.FirstName = contactPersonName;
                        personTO.LastName = "-";
                    }
                    personTO.MobileNo = tblVisitFollowupInfoTO.VisitCallBySelfToMobNo;
                    personTO.CreatedBy = tblVisitFollowupInfoTO.CreatedBy;
                    personTO.CreatedOn = tblVisitFollowupInfoTO.CreatedOn;

                    result = BL.TblPersonBL.InsertTblPerson(personTO, conn, tran);

                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error While InsertTblSiteStatus");
                        tran.Rollback();
                        return resultMessage;
                    }
                    else
                        tblVisitFollowupInfoTO.CallBySelfToWhomId = personTO.IdPerson;

                    // Visit person insertion
                    TblVisitPersonDetailsTO tblVisitPersonDetailsTO = new TblVisitPersonDetailsTO();

                    //tblVisitPersonDetailsTO.VisitId = TblVisitDetailsBL.SelectLastVisitId(conn, tran);

                    tblVisitPersonDetailsTO.VisitId = tblVisitFollowupInfoTO.VisitId;
                    tblVisitPersonDetailsTO.PersonId = personTO.IdPerson;
                    tblVisitPersonDetailsTO.VisitRoleId = tblVisitFollowupInfoTO.CallBySelfToWhom;

                    result = TblVisitPersonDetailsBL.InsertTblVisitPersonDetails(tblVisitPersonDetailsTO, conn, tran);
                }


                result = InsertTblVisitFollowupInfo(tblVisitFollowupInfoTO, conn, tran);

                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While InsertTblVisitFollowupInfo");
                    tran.Rollback();
                    return resultMessage;
                }
                else
                {
                    //Kiran[27 - NOV - 2017] Added for Alert Notifications.--ShareInformation to Whom.
                    List<KeyValuePair<int,int>> dctIds = new List<KeyValuePair<int, int>>();

                    if (tblVisitFollowupInfoTO.ShareInfoToWhomId>0)
                    {
                        dctIds.Add(new KeyValuePair<int, int>(1, tblVisitFollowupInfoTO.ShareInfoToWhomId));
                    }
                    if (tblVisitFollowupInfoTO.ArrangeVisitFor>0)
                    {
                        dctIds.Add(new KeyValuePair<int, int>(2, tblVisitFollowupInfoTO.CreatedBy));
                    }
                    for (int i = 0; i < dctIds.Count; i++)
                    {
                        TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                        List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();
                        TblUserTO userTo = new TblUserTO();
                        if (dctIds[i].Key == 1)
                        {
                            userTo = BL.TblUserBL.SelectTblUserTO(tblVisitFollowupInfoTO.ShareInfoToWhomId, conn, tran);
                            tblAlertInstanceTO.EffectiveFromDate = tblVisitFollowupInfoTO.CreatedOn;
                            tblAlertInstanceTO.EffectiveToDate = tblVisitFollowupInfoTO.ShareInfoOn.AddHours(10);
                            tblAlertInstanceTO.AlertComment = "Visit Inserted";
                        }
                        else if (dctIds[i].Key == 2)
                        {
                            userTo = BL.TblUserBL.SelectTblUserTO(tblVisitFollowupInfoTO.CreatedBy, conn, tran);
                            tblAlertInstanceTO.EffectiveFromDate = tblVisitFollowupInfoTO.CreatedOn;
                            tblAlertInstanceTO.EffectiveToDate = tblVisitFollowupInfoTO.CallBySelfOn.AddHours(10);
                            tblAlertInstanceTO.AlertComment = "Reminder is SET For Visit #"+tblVisitFollowupInfoTO.VisitId;
                        }
                        if (userTo != null)
                        {
                            TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                            tblAlertUsersTO.UserId = userTo.IdUser;
                            tblAlertUsersTO.DeviceId = userTo.RegisteredDeviceId;
                            tblAlertUsersTOList.Add(tblAlertUsersTO);
                        }
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.SITE_VISTED;
                        tblAlertInstanceTO.AlertAction = "New Visit Inserted";
                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                        tblAlertInstanceTO.IsActive = 1;
                        //tblAlertInstanceTO.SourceDisplayId = "STRAIGHT_TO_BEND_TRANSFER_REQUEST";
                        tblAlertInstanceTO.SourceEntityId = tblVisitFollowupInfoTO.VisitId;
                        tblAlertInstanceTO.RaisedBy = tblVisitFollowupInfoTO.CreatedBy;
                        tblAlertInstanceTO.RaisedOn = tblVisitFollowupInfoTO.CreatedOn;
                        tblAlertInstanceTO.IsAutoReset = 1;

                        resultMessage = BL.TblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While SaveNewAlertInstance";
                            resultMessage.Tag = tblAlertInstanceTO;
                            return resultMessage;
                        }
                    }
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SaveVisistFollowUpInfo");
                tran.Rollback();
                return resultMessage;
            }
        }

        #endregion

        #region Updation
        public static int UpdateTblVisitFollowupInfo(TblVisitFollowupInfoTO tblVisitFollowupInfoTO)
        {
            return TblVisitFollowupInfoDAO.UpdateTblVisitFollowupInfo(tblVisitFollowupInfoTO);
        }

        public static int UpdateTblVisitFollowupInfo(TblVisitFollowupInfoTO tblVisitFollowupInfoTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitFollowupInfoDAO.UpdateTblVisitFollowupInfo(tblVisitFollowupInfoTO, conn, tran);
        }

        // Vaibhav [1-Nov-2017] save visit follow up information
        public static ResultMessage UpdateVisitFollowUpInfo(TblVisitFollowupInfoTO tblVisitFollowupInfoTO,SqlConnection conn,SqlTransaction tran)
        {

            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            try
            {
                //Sudhir[20-NOV-2017]Insert New Entrys for FollowUpRoles.
                if (tblVisitFollowupInfoTO.VisitFollowUpInfoNewList != null)
                {
                    foreach (var followUpRole in tblVisitFollowupInfoTO.VisitFollowUpInfoNewList)
                    {
                        if (Convert.ToInt16(followUpRole.Tag) == (int)Constants.VisitFollowUpActionE.ARRANGE_FOR ||
                            Convert.ToInt16(followUpRole.Tag) == (int)Constants.VisitFollowUpActionE.ARRANGE_VISIT_TO)
                        {
                            TblRoleTO tblRoleTO = new TblRoleTO();
                            tblRoleTO.RoleDesc = followUpRole.Text;
                            tblRoleTO.IsActive = 1;
                            tblRoleTO.CreatedOn = tblVisitFollowupInfoTO.CreatedOn;
                            tblRoleTO.CreatedBy = tblVisitFollowupInfoTO.CreatedBy;
                            result = TblVisitFollowUpRolesBL.InsertTblFollowUpRole(tblRoleTO, conn, tran);
                            if (result > 0)
                            {
                                TblVisitFollowUpRolesTO tblVisitFollowUpRolesTO = new TblVisitFollowUpRolesTO();
                                tblVisitFollowUpRolesTO.FollowUpActionId = Convert.ToInt16(followUpRole.Tag);
                                tblVisitFollowUpRolesTO.FollowUpRoleId = tblRoleTO.IdRole;
                                tblVisitFollowUpRolesTO.CreatedBy = tblVisitFollowupInfoTO.CreatedBy;
                                tblVisitFollowUpRolesTO.CreatedOn = tblVisitFollowupInfoTO.CreatedOn;
                                result = TblVisitFollowUpRolesBL.InsertTblVisitFollowUpRoles(tblVisitFollowUpRolesTO, conn, tran);
                                if (result > 0 && Convert.ToInt16(followUpRole.Tag) == (int)Constants.VisitFollowUpActionE.ARRANGE_FOR)
                                {
                                    tblVisitFollowupInfoTO.ArrangeVisitFor = tblRoleTO.IdRole;
                                }
                                else if (result > 0 && Convert.ToInt16(followUpRole.Tag) == (int)Constants.VisitFollowUpActionE.ARRANGE_VISIT_TO)
                                {
                                    tblVisitFollowupInfoTO.ArrangeVisitTo = tblRoleTO.IdRole;
                                }
                            }
                        }
                    }
                }

                if (tblVisitFollowupInfoTO.VisitCallBySelfToIsNew == 0 && tblVisitFollowupInfoTO.CallBySelfToWhomId == 0 && tblVisitFollowupInfoTO.VisitCallBySelfToName != null)
                {
                    TblPersonTO personTO = new TblPersonTO();
                    personTO.SalutationId = Constants.DefaultSalutationId; //set default 1
                    String contactPersonName = tblVisitFollowupInfoTO.VisitCallBySelfToName;
                    if (contactPersonName.Contains(' '))
                    {
                        String[] contactPersonNames = contactPersonName.Split(' ');

                        if (contactPersonNames.Length >= 0)
                            personTO.FirstName = contactPersonNames[0] != null ? contactPersonNames[0].ToString() : null;
                        if (contactPersonNames.Length > 1)
                            personTO.LastName = contactPersonNames[1] != null ? contactPersonNames[1].ToString() : null;
                        if (contactPersonNames.Length > 2)
                            personTO.LastName = contactPersonNames[2] != null ? contactPersonNames[2].ToString() : null;
                    }
                    else
                    {
                        personTO.FirstName = contactPersonName;
                        personTO.LastName = "-";
                    }
                    personTO.MobileNo = tblVisitFollowupInfoTO.VisitCallBySelfToMobNo;
                    personTO.CreatedBy = tblVisitFollowupInfoTO.CreatedBy;
                    personTO.CreatedOn = tblVisitFollowupInfoTO.CreatedOn;

                    result = BL.TblPersonBL.InsertTblPerson(personTO, conn, tran);

                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error While InsertTblSiteStatus");
                        tran.Rollback();
                        return resultMessage;
                    }
                    else
                        tblVisitFollowupInfoTO.CallBySelfToWhomId = personTO.IdPerson;

                    // Visit person insertion
                    TblVisitPersonDetailsTO tblVisitPersonDetailsTO = new TblVisitPersonDetailsTO();

                    //tblVisitPersonDetailsTO.VisitId = TblVisitDetailsBL.SelectLastVisitId(conn, tran);

                    tblVisitPersonDetailsTO.VisitId = tblVisitFollowupInfoTO.VisitId;
                    tblVisitPersonDetailsTO.PersonId = personTO.IdPerson;
                    tblVisitPersonDetailsTO.VisitRoleId = tblVisitFollowupInfoTO.CallBySelfToWhom;

                    result = TblVisitPersonDetailsBL.InsertTblVisitPersonDetails(tblVisitPersonDetailsTO, conn, tran);
                }
                else
                {
                    if (tblVisitFollowupInfoTO.VisitCallBySelfToName != null && tblVisitFollowupInfoTO.VisitCallBySelfToMobNo != null)
                    {
                        TblPersonTO person = new TblPersonTO();
                        person.IdPerson = tblVisitFollowupInfoTO.CallBySelfToWhomId;
                        person.SalutationId = Constants.DefaultSalutationId;
                        String contactPersonName = tblVisitFollowupInfoTO.VisitCallBySelfToName;
                        if (contactPersonName != null)
                        {
                            if (contactPersonName.Contains(' '))
                            {
                                String[] contactPersonNames = contactPersonName.Split(' ');

                                if (contactPersonNames.Length >= 0)
                                    person.FirstName = contactPersonNames[0] != null ? contactPersonNames[0].ToString() : null;
                                if (contactPersonNames.Length > 1)
                                    person.LastName = contactPersonNames[1] != null ? contactPersonNames[1].ToString() : null;
                                if (contactPersonNames.Length > 2)
                                    person.LastName = contactPersonNames[2] != null ? contactPersonNames[2].ToString() : null;
                            }
                            else
                            {
                                person.FirstName = contactPersonName;
                                person.LastName = "-";
                            }
                        }

                        person.MobileNo = tblVisitFollowupInfoTO.VisitCallBySelfToMobNo;
                        person.CreatedBy = tblVisitFollowupInfoTO.UpdatedBy;
                        person.CreatedOn = tblVisitFollowupInfoTO.UpdatedOn;

                        result = BL.TblPersonBL.UpdateTblPerson(person, conn, tran);
                        //if(result!=1)
                        //{
                        //    resultMessage.DefaultBehaviour("Error While UpdateTblVisitFollowupInfo");
                        //    tran.Rollback();
                        //    return resultMessage;
                        //}

                    }
                }

                result = UpdateTblVisitFollowupInfo(tblVisitFollowupInfoTO, conn, tran);

                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While UpdateTblVisitFollowupInfo");
                    tran.Rollback();
                    return resultMessage;
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateVisitFollowUpInfo");
                tran.Rollback();
                return resultMessage;
            }
        }


        #endregion

        #region Deletion
        public static int DeleteTblVisitFollowupInfo(Int32 idProjectFollowUpInfo)
        {
            return TblVisitFollowupInfoDAO.DeleteTblVisitFollowupInfo(idProjectFollowUpInfo);
        }

        public static int DeleteTblVisitFollowupInfo(Int32 idProjectFollowUpInfo, SqlConnection conn, SqlTransaction tran)
        {
            return TblVisitFollowupInfoDAO.DeleteTblVisitFollowupInfo(idProjectFollowUpInfo, conn, tran);
        }

        #endregion

    }
}
