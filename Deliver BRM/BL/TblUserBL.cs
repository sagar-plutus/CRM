using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.BL
{
    public class TblUserBL
    {
        #region Selection

        public static List<TblUserTO> SelectAllTblUserList(Boolean onlyActiveYn)
        {
            return TblUserDAO.SelectAllTblUser(onlyActiveYn);
        }

        public static TblUserTO SelectTblUserTO(Int32 idUser)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblUserDAO.SelectTblUser(idUser, conn, tran);
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

        public static int SelectUserByImeiNumber(string idDevice)
        {
            TblUserTO tblUserTo = new TblUserTO();
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                tblUserTo = TblUserDAO.SelectUserByImeiNumber(idDevice, conn, tran);
                if (tblUserTo != null)
                    return tblUserTo.IdUser;
                else return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
            }

        }

        public static TblUserTO SelectTblUserTO(Int32 idUser, SqlConnection conn, SqlTransaction tran)
        {
            return TblUserDAO.SelectTblUser(idUser, conn, tran);

        }

        public static TblUserTO SelectTblUserTO(String userID, String password)
        {
            return TblUserDAO.SelectTblUser(userID, password);
        }

        public static Boolean IsThisUserExists(String userId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                Boolean result = IsThisUserExists(userId, conn, tran);
                tran.Commit();
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static Boolean IsThisUserExists(String userId, SqlConnection conn, SqlTransaction tran)
        {
            return TblUserDAO.IsThisUserExists(userId, conn, tran);
        }

        public static Dictionary<int, string> SelectUserMobileNoDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran)
        {
            return TblUserDAO.SelectUserMobileNoDCTByUserIdOrRole(userOrRoleIds, isUser, conn, tran);

        }

        public static Dictionary<int, string> SelectUserMobileNoDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser)
        {
            return TblUserDAO.SelectUserMobileNoDCTByUserIdOrRole(userOrRoleIds, isUser);

        }

        public static Dictionary<int, List<string>> SelectUserMobileNoAndAlterMobileDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran)
        {
            return TblUserDAO.SelectUserMobileNoAndAlterMobileDCTByUserIdOrRole(userOrRoleIds, isUser, conn, tran);

        }

        public static Dictionary<int, string> SelectUserDeviceRegNoDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran)
        {
            return TblUserDAO.SelectUserDeviceRegNoDCTByUserIdOrRole(userOrRoleIds, isUser, conn, tran);

        }

        public static Dictionary<int, string> SelectUserDeviceRegNoDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser)
        {
            return TblUserDAO.SelectUserDeviceRegNoDCTByUserIdOrRole(userOrRoleIds, isUser);

        }

        public static Dictionary<int, string> SelectUserUsingRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran)
        {
            return TblUserDAO.SelectUserUsingRole(userOrRoleIds, isUser, conn, tran);

        }
        public static Dictionary<int, string> SelectUserUsingRole(String userOrRoleIds, Boolean isUser)
        {
            return TblUserDAO.SelectUserUsingRole(userOrRoleIds, isUser);

        }
        public static List<TblUserTO> SelectAllTblUserList(Int32 orgId, SqlConnection conn, SqlTransaction tran)
        {
            return TblUserDAO.SelectAllTblUser(orgId, conn, tran);
        }

        public static List<DropDownTO> SelectAllActiveUsersForDropDown()
        {
            return TblUserDAO.SelectAllActiveUsersForDropDown();
        }

        //Sudhir[08-MAR-2018] Added for Get Users for Organizations Structre Based on User ID's
        public static List<DropDownTO> SelectUsersOnUserIds(string userIds)
        {
            return TblUserDAO.SelectUsersOnUserIds(userIds);
        }

        internal static TblUserTO SelectTblUserTOIfExists(string mobileNo, string primaryEmail)
        {
           return TblUserDAO.SelectTblUserTOIfExists(mobileNo,primaryEmail);
        }

        #endregion

        #region Insertion
        public static int InsertTblUser(TblUserTO tblUserTO)
        {
            return TblUserDAO.InsertTblUser(tblUserTO);
        }

        public static int InsertTblUser(TblUserTO tblUserTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblUserDAO.InsertTblUser(tblUserTO, conn, tran);
        }

        public static String CreateUserName(string firstName, string lastName, SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                String userName = string.Empty;
                userName = firstName.TrimEnd(' ') + "." + lastName.TrimEnd(' ');
                Boolean isUserExist = true;
                for (int i = 0; i < 5; i++) //Max 5 Is Considered
                {
                    if (i == 0)
                    {
                        isUserExist = IsThisUserExists(userName, conn, tran);
                        if (!isUserExist)
                            return userName;
                        else continue;
                    }
                    else
                    {
                        string newUser = userName + i;
                        isUserExist = IsThisUserExists(newUser, conn, tran);
                        if (!isUserExist)
                            return newUser;
                        else continue;
                    }
                }

                userName = userName + Constants.ServerDateTime.ToString("ddMMyyyy");
                return userName;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static ResultMessage SaveNewUser(TblUserTO tblUserTO, Int32 loginUserId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage rMessage = new ResultMessage();
            DateTime serverDateTime = Constants.ServerDateTime;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                #region Check User Creation Limit Added By @KM 13/02/2019
                Boolean isProceedToCreate = BL.TblLoginBL.GetUsersQuata(conn, tran);
                if (!isProceedToCreate)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.DisplayMessage = "User Quota Exceeded, Please contact your administrative";
                    rMessage.Text = "User Quota Exceeded, Please contact your administrative";
                    return rMessage;
                }
                #endregion

                String userId = TblUserBL.CreateUserName(tblUserTO.UserPersonTO.FirstName, tblUserTO.UserPersonTO.LastName, conn, tran);
                userId = userId.ToLower();
                String pwd = Constants.DefaultPassword;

                if (tblUserTO.UserPersonTO.DobDay > 0 && tblUserTO.UserPersonTO.DobMonth > 0 && tblUserTO.UserPersonTO.DobYear > 0)
                {
                    tblUserTO.UserPersonTO.DateOfBirth = new DateTime(tblUserTO.UserPersonTO.DobYear, tblUserTO.UserPersonTO.DobMonth, tblUserTO.UserPersonTO.DobDay);
                }
                else
                {
                    tblUserTO.UserPersonTO.DateOfBirth = DateTime.MinValue;
                }

                tblUserTO.UserPersonTO.CreatedBy = loginUserId;
                tblUserTO.UserPersonTO.CreatedOn = serverDateTime;
                int result = BL.TblPersonBL.InsertTblPerson(tblUserTO.UserPersonTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    rMessage.Text = "Error While InsertTblPerson for Users in Method SaveNewUser ";
                    return rMessage;
                }

                tblUserTO.UserDisplayName = tblUserTO.UserPersonTO.FirstName + " " + tblUserTO.UserPersonTO.LastName;
                tblUserTO.IsActive = 1;
                tblUserTO.UserLogin = userId;
                tblUserTO.UserPasswd = pwd;
                result = BL.TblUserBL.InsertTblUser(tblUserTO, conn, tran);

                if (result != 1)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    rMessage.Text = "Error While InsertTblUser for Users in Method SaveNewUser ";
                    return rMessage;
                }

                tblUserTO.UserExtTO = new TblUserExtTO();
                tblUserTO.UserExtTO.CreatedBy = loginUserId;
                tblUserTO.UserExtTO.CreatedOn = serverDateTime;
                tblUserTO.UserExtTO.PersonId = tblUserTO.UserPersonTO.IdPerson;
                tblUserTO.UserExtTO.UserId = tblUserTO.IdUser;
                tblUserTO.UserExtTO.OrganizationId = tblUserTO.OrganizationId;

                TblAddressTO addressTO = BL.TblAddressBL.SelectOrgAddressWrtAddrType(tblUserTO.OrganizationId, Constants.AddressTypeE.OFFICE_ADDRESS, conn, tran);
                if (addressTO == null)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "Error..Record could not be saved. Address Details for the organization + " + tblUserTO.OrganizationName + " is not set";
                    rMessage.DisplayMessage = "Error..Record could not be saved. Address Details for the organization + " + tblUserTO.OrganizationName + " is not set";
                    return rMessage;
                }
                tblUserTO.UserExtTO.AddressId = addressTO.IdAddr;

                result = BL.TblUserExtBL.InsertTblUserExt(tblUserTO.UserExtTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "Error While InsertTblUserExt for Users in Method SaveNewUser ";
                    rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return rMessage;
                }

                if (tblUserTO.OrgStructId != 0 && tblUserTO.OrgStructId > 0)
                {
                    TblUserReportingDetailsTO tblUserReportingDetailsTO = new TblUserReportingDetailsTO();
                    tblUserReportingDetailsTO.CreatedBy = loginUserId;
                    tblUserReportingDetailsTO.OrgStructureId = tblUserTO.OrgStructId;
                    tblUserReportingDetailsTO.ReportingTo = tblUserTO.ReportingTo;
                    tblUserReportingDetailsTO.UserId = tblUserTO.IdUser;
                    tblUserReportingDetailsTO.CreatedOn = serverDateTime;
                    tblUserReportingDetailsTO.LevelId = tblUserTO.LevelId;
                    tblUserReportingDetailsTO.IsActive = 1;
                    List<TblUserReportingDetailsTO> emptyList = new List<TblUserReportingDetailsTO>();
                    rMessage = TblOrgStructureBL.AttachNewUserToOrgStructure(tblUserReportingDetailsTO, emptyList, conn,tran);
                    if (rMessage.MessageType != ResultMessageE.Information)
                    {
                        tran.Rollback();
                        rMessage.MessageType = ResultMessageE.Error;
                        rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        rMessage.Text = "Error While Attch New User to Organization Structure in AttachNewUserToOrgStructure Method";
                        return rMessage;
                    }
                }
                else
                {
                    tblUserTO.UserRoleList[0].UserId = tblUserTO.IdUser;
                    tblUserTO.UserRoleList[0].IsActive = 1;
                    tblUserTO.UserRoleList[0].CreatedBy = loginUserId;
                    tblUserTO.UserRoleList[0].CreatedOn = serverDateTime;

                    result = BL.TblUserRoleBL.InsertTblUserRole(tblUserTO.UserRoleList[0], conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        rMessage.MessageType = ResultMessageE.Error;
                        rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        rMessage.Text = "Error While InsertTblUserRole for C&F Users in Method SaveNewUser ";
                        return rMessage;
                    }
                }

                tran.Commit();
                rMessage.MessageType = ResultMessageE.Information;
                rMessage.Text = "Record Saved Successfully";
                rMessage.DisplayMessage = "Record Saved Successfully";
                rMessage.Result = 1;
                return rMessage;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Exception = ex;
                rMessage.Result = -1;
                rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                rMessage.Text = "Error While InsertTblUserRole for C&F Users in Method SaveNewUser ";
                return rMessage;
            }
            finally
            {
                conn.Close();
            }
        }


        #endregion

        #region Updation
        public static int UpdateTblUser(TblUserTO tblUserTO)
        {
            return TblUserDAO.UpdateTblUser(tblUserTO);
        }

        /// <summary>
        /// Saket [2018-03-06] Added to save the history for PWd
        /// </summary>
        /// <param name="tblUserTO"></param>
        /// <param name="tblUserPwdHistoryTO"></param>
        /// <returns></returns>
        public static ResultMessage UpdateTblUser(TblUserTO tblUserTO, TblUserPwdHistoryTO tblUserPwdHistoryTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage rMessage = new ResultMessage();
            DateTime serverDateTime = Constants.ServerDateTime;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                int result = BL.TblUserBL.UpdateTblUser(tblUserTO, conn, tran);
                if (result != 1)
                {
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Result = 0;
                    rMessage.Text = "Error In Updating Password";
                    return rMessage;
                }

                if (tblUserPwdHistoryTO != null)
                {
                    result = TblUserPwdHistoryBL.InsertTblUserPwdHistory(tblUserPwdHistoryTO, conn, tran);
                    if (result != 1)
                    {
                        rMessage.MessageType = ResultMessageE.Error;
                        rMessage.Result = 0;
                        rMessage.Text = "Error In Inserting Password history";
                        return rMessage;
                    }
                }


                tran.Commit();
                rMessage.MessageType = ResultMessageE.Information;
                rMessage.Text = "Record Saved Successfully";
                rMessage.DisplayMessage = "Record Saved Successfully";
                rMessage.Result = 1;
                return rMessage;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Exception = ex;
                rMessage.Result = -1;
                rMessage.Text = "Exception Error While UpdateUser Method UpdateUser";
                rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                return rMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public static int UpdateTblUser(TblUserTO tblUserTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblUserDAO.UpdateTblUser(tblUserTO, conn, tran);
        }

        public static ResultMessage UpdateUser(TblUserTO tblUserTO, Int32 loginUserId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage rMessage = new ResultMessage();
            DateTime serverDateTime = Constants.ServerDateTime;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                int result = 0;
                if (tblUserTO.UserPersonTO != null)
                {
                    if (tblUserTO.UserPersonTO.DobDay > 0 && tblUserTO.UserPersonTO.DobMonth > 0 && tblUserTO.UserPersonTO.DobYear > 0)
                    {
                        tblUserTO.UserPersonTO.DateOfBirth = new DateTime(tblUserTO.UserPersonTO.DobYear, tblUserTO.UserPersonTO.DobMonth, tblUserTO.UserPersonTO.DobDay);
                    }
                    else
                    {
                        tblUserTO.UserPersonTO.DateOfBirth = DateTime.MinValue;
                    }

                    result = BL.TblPersonBL.UpdateTblPerson(tblUserTO.UserPersonTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        rMessage.MessageType = ResultMessageE.Error;
                        rMessage.Text = "Error While UpdateTblPerson for Users in Method UpdateUser ";
                        rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        return rMessage;
                    }

                    tblUserTO.UserDisplayName = tblUserTO.UserPersonTO.FirstName + " " + tblUserTO.UserPersonTO.LastName;

                }

                result = BL.TblUserBL.UpdateTblUser(tblUserTO, conn, tran);

                if (result != 1)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "Error While InsertTblUser for Users in Method UpdateUser ";
                    rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return rMessage;
                }

                tblUserTO.UserExtTO.PersonId = tblUserTO.UserPersonTO.IdPerson;
                tblUserTO.UserExtTO.UserId = tblUserTO.IdUser;
                tblUserTO.UserExtTO.OrganizationId = tblUserTO.OrganizationId;

                result = BL.TblUserExtBL.UpdateTblUserExt(tblUserTO.UserExtTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "Error While InsertTblUserExt for Users in Method UpdateUser ";
                    rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return rMessage;
                }

                result = BL.TblUserRoleBL.UpdateTblUserRole(tblUserTO.UserRoleList[0], conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "Error While UpdateTblUserRole for C&F Users in Method UpdateUser ";
                    rMessage.DisplayMessage = Constants.DefaultErrorMsg;

                    return rMessage;
                }

                //Update User Reporting Details
                if(tblUserTO.UserReportingId > 0)
                {
                    TblUserReportingDetailsTO tblUserReportingDetailsTO = BL.TblOrgStructureBL.SelectUserReportingDetailsTO(tblUserTO.UserReportingId, conn, tran);
                    if(tblUserReportingDetailsTO != null)
                    {
                        tblUserReportingDetailsTO.UpdatedBy = loginUserId;
                        tblUserReportingDetailsTO.UpdatedOn = serverDateTime;
                        tblUserReportingDetailsTO.UserId = tblUserTO.IdUser;
                        tblUserReportingDetailsTO.ReportingTo = tblUserTO.ReportingTo;
                        tblUserReportingDetailsTO.OrgStructureId = tblUserTO.OrgStructId;
                        tblUserReportingDetailsTO.LevelId = tblUserTO.LevelId;
                        tblUserReportingDetailsTO.IsActive = 1;
                        result = TblOrgStructureBL.UpdateUserReportingDetail(tblUserReportingDetailsTO, conn, tran);
                        if(result !=1 )
                        {
                            tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While UpdateTblUserRole for C&F Users in Method UpdateUser ";
                            rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            return rMessage;
                        }
                    }
                }

                tran.Commit();
                rMessage.MessageType = ResultMessageE.Information;
                rMessage.Text = "Record Saved Successfully";
                rMessage.DisplayMessage = "Record Saved Successfully";
                rMessage.Result = 1;
                return rMessage;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Exception = ex;
                rMessage.Result = -1;
                rMessage.Text = "Exception Error While UpdateUser Method UpdateUser ";
                rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                return rMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region Deletion
        public static int DeleteTblUser(Int32 idUser)
        {
            return TblUserDAO.DeleteTblUser(idUser);
        }

        public static int DeleteTblUser(Int32 idUser, SqlConnection conn, SqlTransaction tran)
        {
            return TblUserDAO.DeleteTblUser(idUser, conn, tran);
        }

        #endregion

    }
}
