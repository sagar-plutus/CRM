using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using SalesTrackerAPI;

namespace SalesTrackerAPI.DAL
{
    public class TblUserDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT userDtl.* ,userExt.personId,userExt.addressId,userExt.organizationId,org.firmName ,org.isSpecialCnf, org.orgTypeId " +
                                  " ,DocumentDetails.path FROM tblUser userDtl " +
                                  " LEFT JOIN tblUserExt userExt " +
                                  " ON userDtl.idUser = userExt.userId " +
                                  " LEFT JOIN tblOrganization org ON org.idOrganization=userExt.organizationId"+
                                  " LEFT JOIN tblDocumentDetails DocumentDetails ON DocumentDetails.idDocument=userDtl.doucmentId "; 

            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblUserTO> SelectAllTblUser(Boolean onlyActiveYn)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                if (onlyActiveYn)
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE userDtl.isActive=1";
                else
                    cmdSelect.CommandText = SqlSelectQuery();

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserTO> list = ConvertDTToList(sqlReader);
                return list;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

                public static DropDownTO SelectTblUserOnDeviceId(String deviceId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE imeiNumber = '" + deviceId + "' AND userDtl.isActive=1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (sqlReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (sqlReader["idUser"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(sqlReader["idUser"].ToString());
                    if (sqlReader["userDisplayName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(sqlReader["userDisplayName"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }
                if (dropDownTOList != null && dropDownTOList.Count > 0)
                {
                    return dropDownTOList[0];
                }
                else
                {
                    return new DropDownTO();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        public static TblUserTO SelectTblUser(Int32 idUser, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idUser = " + idUser + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static DropDownTO SelectTblUser(Int32 idUser)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idUser = " + idUser + " AND userDtl.isActive=1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (sqlReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (sqlReader["idUser"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(sqlReader["idUser"].ToString());
                    if (sqlReader["userDisplayName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(sqlReader["userDisplayName"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }
                if (dropDownTOList != null && dropDownTOList.Count > 0)
                {
                    return dropDownTOList[0];
                }
                else
                {
                    return new DropDownTO();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblUserTO SelectTblUser(String userID, String password)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE userLogin ='" + userID + "'" + " AND userPasswd='" + password + "'" + " AND userDtl.isActive=1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static Boolean IsThisUserExists(String userID, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE userLogin ='" + userID + "'";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count > 0)
                    return true;
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        internal static TblUserTO SelectTblUserTOIfExists(string mobileNo, string primaryEmail)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select us.idUser,us.isActive,p.mobileNo,p.primaryEmail,us.userDisplayName,us.userPasswd,us.userLogin from tblUser us left join tblUserExt e on e.userId = us.idUser left join tblPerson  p on p.idPerson = e.personId WHERE (primaryEmail ='" + primaryEmail + "'"+ "OR us.userLogin ='" + primaryEmail + "'" + " OR mobileNo='" + mobileNo + "')" + " AND us.isActive=1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                TblUserTO tblUserTONew = new TblUserTO();
                while (sqlReader.Read())
                {

                    if (sqlReader["idUser"] != DBNull.Value)
                        tblUserTONew.IdUser = Convert.ToInt32(sqlReader["idUser"].ToString());

                    if (sqlReader["isActive"] != DBNull.Value)
                        tblUserTONew.IsActive = Convert.ToInt32(sqlReader["isActive"].ToString());

                    if (sqlReader["mobileNo"] != DBNull.Value)
                        tblUserTONew.UserPersonTO.MobileNo = Convert.ToString(sqlReader["mobileNo"].ToString());

                    if (sqlReader["primaryEmail"] != DBNull.Value)
                        tblUserTONew.UserPersonTO.PrimaryEmail = Convert.ToString(sqlReader["primaryEmail"].ToString());

                    if (sqlReader["userDisplayName"] != DBNull.Value)
                        tblUserTONew.UserDisplayName = Convert.ToString(sqlReader["userDisplayName"].ToString());

                    if (sqlReader["userPasswd"] != DBNull.Value)
                        tblUserTONew.UserPasswd = Convert.ToString(sqlReader["userPasswd"].ToString());

                        
                    if (sqlReader["userLogin"] != DBNull.Value)
                        tblUserTONew.UserLogin = Convert.ToString(sqlReader["userLogin"].ToString());
                }

                if (tblUserTONew != null)
                    return tblUserTONew;
                else
                    return null;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public static Dictionary<int, string> SelectUserMobileNoDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            Dictionary<int, string> DCT = null;
            String whereCond = string.Empty;
            try
            {
                if (isUser)
                {
                    whereCond = " WHERE idUser IN(" + userOrRoleIds + ") AND mobileNo IS NOT NULL AND ISNULL(userDtl.isActive,0) = 1 ";
                }
                else
                    whereCond = " WHERE roleId IN(" + userOrRoleIds + ") AND mobileNo IS NOT NULL AND ISNULL(userDtl.isActive,0) = 1 ";

                cmdSelect.CommandText = " SELECT idPerson,mobileNo FROM tblPerson person " +
                                        " INNER JOIN tblUserExt ext ON person.idPerson = ext.personId " +
                                        " INNER JOIN tblUser userDtl ON userDtl.idUser = ext.userId " +
                                        " INNER JOIN tblUserRole userRole ON userRole.userId = userDtl.idUser " +
                                        whereCond;

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (rdr != null)
                {
                    DCT = new Dictionary<int, string>();
                    while (rdr.Read())
                    {
                        Int32 orgId = 0;
                        string regMobNos = string.Empty;
                        if (rdr["idPerson"] != DBNull.Value)
                            orgId = Convert.ToInt32(rdr["idPerson"].ToString());
                        if (rdr["mobileNo"] != DBNull.Value)
                            regMobNos = Convert.ToString(rdr["mobileNo"].ToString());

                        if (orgId > 0 && !string.IsNullOrEmpty(regMobNos))
                        {
                            DCT.Add(orgId, regMobNos);
                        }
                    }

                    return DCT;
                }
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (rdr != null)
                    rdr.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static Dictionary<int, string> SelectUserMobileNoDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            Dictionary<int, string> DCT = null;
            String whereCond = string.Empty;
            try
            {
                conn.Open();
                if (isUser)
                {
                    whereCond = " WHERE idUser IN(" + userOrRoleIds + ") AND mobileNo IS NOT NULL AND ISNULL(userDtl.isActive,0) = 1 ";
                }
                else
                    whereCond = " WHERE roleId IN(" + userOrRoleIds + ") AND mobileNo IS NOT NULL AND ISNULL(userDtl.isActive,0) = 1 ";

                cmdSelect.CommandText = " SELECT idPerson,mobileNo FROM tblPerson person " +
                                        " INNER JOIN tblUserExt ext ON person.idPerson = ext.personId " +
                                        " INNER JOIN tblUser userDtl ON userDtl.idUser = ext.userId " +
                                        " INNER JOIN tblUserRole userRole ON userRole.userId = userDtl.idUser " +
                                        whereCond;

                cmdSelect.Connection = conn;
              //  cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (rdr != null)
                {
                    DCT = new Dictionary<int, string>();
                    while (rdr.Read())
                    {
                        Int32 orgId = 0;
                        string regMobNos = string.Empty;
                        if (rdr["idPerson"] != DBNull.Value)
                            orgId = Convert.ToInt32(rdr["idPerson"].ToString());
                        if (rdr["mobileNo"] != DBNull.Value)
                            regMobNos = Convert.ToString(rdr["mobileNo"].ToString());

                        if (orgId > 0 && !string.IsNullOrEmpty(regMobNos))
                        {
                            DCT.Add(orgId, regMobNos);
                        }
                    }

                    return DCT;
                }
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (rdr != null)
                    rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static Dictionary<int, List<string>> SelectUserMobileNoAndAlterMobileDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            Dictionary<int, List<string>> DCT = null;
            String whereCond = string.Empty;
            try
            {
                if (isUser)
                {
                    whereCond = " WHERE idUser IN(" + userOrRoleIds + ") AND mobileNo IS NOT NULL AND ISNULL(userDtl.isActive,0) = 1  AND  userRole.isActive = 1 ";
                }
                else
                    whereCond = " WHERE roleId IN(" + userOrRoleIds + ") AND mobileNo IS NOT NULL AND ISNULL(userDtl.isActive,0) = 1  AND  userRole.isActive = 1";

                cmdSelect.CommandText = " SELECT idPerson,mobileNo ,alternateMobNo FROM tblPerson person " +
                                        " INNER JOIN tblUserExt ext ON person.idPerson = ext.personId " +
                                        " INNER JOIN tblUser userDtl ON userDtl.idUser = ext.userId " +
                                        " INNER JOIN tblUserRole userRole ON userRole.userId = userDtl.idUser " +
                                        whereCond;

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (rdr != null)
                {
                    DCT = new Dictionary<int, List<string>>();
                    while (rdr.Read())
                    {
                        Int32 orgId = 0;
                        string regMobNos = string.Empty;
                        string regAltMobNos = string.Empty;

                        if (rdr["idPerson"] != DBNull.Value)
                            orgId = Convert.ToInt32(rdr["idPerson"].ToString());
                        if (rdr["mobileNo"] != DBNull.Value)
                            regMobNos = Convert.ToString(rdr["mobileNo"].ToString());
                        if (rdr["alternateMobNo"] != DBNull.Value)
                            regAltMobNos = Convert.ToString(rdr["alternateMobNo"].ToString());

                        if (orgId > 0)
                        {
                            if(!DCT.ContainsKey(orgId))
                            {
                                List<string> mobList = new List<string>();
                                if (!string.IsNullOrEmpty(regMobNos))
                                    mobList.Add(regMobNos);
                                if (!string.IsNullOrEmpty(regAltMobNos))
                                    mobList.Add(regAltMobNos);
                                if (mobList.Count > 0)
                                    DCT.Add(orgId, mobList);
                            }
                            
                        }
                    }

                    return DCT;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (rdr != null)
                    rdr.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static Dictionary<int, string> SelectUserDeviceRegNoDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            Dictionary<int, string> DCT = null;
            String whereCond = string.Empty;
            try
            {
                if (isUser)
                {
                    whereCond = " WHERE idUser IN(" + userOrRoleIds + ") AND registeredDeviceId IS NOT NULL AND userDtl.isActive=1";
                }
                else
                    whereCond = " WHERE roleId IN(" + userOrRoleIds + ") AND registeredDeviceId IS NOT NULL AND userDtl.isActive=1";

                cmdSelect.CommandText = " SELECT idUser,registeredDeviceId FROM tblUser userDtl " +
                                        " INNER JOIN tblUserExt ext ON userDtl.idUser = ext.userId " +
                                        " INNER JOIN tblUserRole userRole ON userRole.userId = userDtl.idUser " +
                                        whereCond;

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (rdr != null)
                {
                    DCT = new Dictionary<int, string>();
                    while (rdr.Read())
                    {
                        Int32 userId = 0;
                        string regDeviceNos = string.Empty;
                        if (rdr["idUser"] != DBNull.Value)
                            userId = Convert.ToInt32(rdr["idUser"].ToString());
                        if (rdr["registeredDeviceId"] != DBNull.Value)
                            regDeviceNos = Convert.ToString(rdr["registeredDeviceId"].ToString());

                        if (userId > 0 && !string.IsNullOrEmpty(regDeviceNos))
                        {
                            DCT.Add(userId, regDeviceNos);
                        }
                    }

                    return DCT;
                }
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (rdr != null)
                    rdr.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static Dictionary<int, string> SelectUserDeviceRegNoDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            Dictionary<int, string> DCT = null;
            String whereCond = string.Empty;
            try
            {
                conn.Open();
                if (isUser)
                {
                    whereCond = " WHERE idUser IN(" + userOrRoleIds + ") AND registeredDeviceId IS NOT NULL AND userDtl.isActive=1";
                }
                else
                    whereCond = " WHERE roleId IN(" + userOrRoleIds + ") AND registeredDeviceId IS NOT NULL AND userDtl.isActive=1";

                cmdSelect.CommandText = " SELECT idUser,registeredDeviceId FROM tblUser userDtl " +
                                        " INNER JOIN tblUserExt ext ON userDtl.idUser = ext.userId " +
                                        " INNER JOIN tblUserRole userRole ON userRole.userId = userDtl.idUser " +
                                        whereCond;

                cmdSelect.Connection = conn;
                // cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (rdr != null)
                {
                    DCT = new Dictionary<int, string>();
                    while (rdr.Read())
                    {
                        Int32 userId = 0;
                        string regDeviceNos = string.Empty;
                        if (rdr["idUser"] != DBNull.Value)
                            userId = Convert.ToInt32(rdr["idUser"].ToString());
                        if (rdr["registeredDeviceId"] != DBNull.Value)
                            regDeviceNos = Convert.ToString(rdr["registeredDeviceId"].ToString());

                        if (userId > 0 && !string.IsNullOrEmpty(regDeviceNos))
                        {
                            DCT.Add(userId, regDeviceNos);
                        }
                    }

                    return DCT;
                }
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (rdr != null)
                    rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static Dictionary<int, string> SelectUserUsingRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            Dictionary<int, string> DCT = null;
            String whereCond = string.Empty;
            try
            {
                if (isUser)
                {
                    whereCond = " WHERE idUser IN(" + userOrRoleIds + ") AND userDtl.isActive=1";
                }
                else
                    whereCond = " WHERE roleId IN(" + userOrRoleIds + ") AND userDtl.isActive=1";

                cmdSelect.CommandText = " SELECT idUser,registeredDeviceId FROM tblUser userDtl " +
                                        " INNER JOIN tblUserExt ext ON userDtl.idUser = ext.userId " +
                                        " INNER JOIN tblUserRole userRole ON userRole.userId = userDtl.idUser " +
                                        whereCond;

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (rdr != null)
                {
                    DCT = new Dictionary<int, string>();
                    while (rdr.Read())
                    {
                        Int32 userId = 0;
                        string regDeviceNos = string.Empty;
                        if (rdr["idUser"] != DBNull.Value)
                            userId = Convert.ToInt32(rdr["idUser"].ToString());
                        if (rdr["registeredDeviceId"] != DBNull.Value)
                            regDeviceNos = Convert.ToString(rdr["registeredDeviceId"].ToString());

                        if (userId > 0)
                        {
                            DCT.Add(userId, regDeviceNos);
                        }
                    }

                    return DCT;
                }
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (rdr != null)
                    rdr.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static Dictionary<int, string> SelectUserUsingRole(String userOrRoleIds, Boolean isUser)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            Dictionary<int, string> DCT = null;
            String whereCond = string.Empty;
            try
            {
                conn.Open();
                if (isUser)
                {
                    whereCond = " WHERE idUser IN(" + userOrRoleIds + ") AND userDtl.isActive=1";
                }
                else
                    whereCond = " WHERE roleId IN(" + userOrRoleIds + ") AND userDtl.isActive=1";

                cmdSelect.CommandText = " SELECT idUser,registeredDeviceId FROM tblUser userDtl " +
                                        " INNER JOIN tblUserExt ext ON userDtl.idUser = ext.userId " +
                                        " INNER JOIN tblUserRole userRole ON userRole.userId = userDtl.idUser " +
                                        whereCond;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (rdr != null)
                {
                    DCT = new Dictionary<int, string>();
                    while (rdr.Read())
                    {
                        Int32 userId = 0;
                        string regDeviceNos = string.Empty;
                        if (rdr["idUser"] != DBNull.Value)
                            userId = Convert.ToInt32(rdr["idUser"].ToString());
                        if (rdr["registeredDeviceId"] != DBNull.Value)
                            regDeviceNos = Convert.ToString(rdr["registeredDeviceId"].ToString());

                        if (userId > 0)
                        {
                            DCT.Add(userId, regDeviceNos);
                        }
                    }

                    return DCT;
                }
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (rdr != null)
                    rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblUserTO> SelectAllTblUser(Int32 orgId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE organizationId=" + orgId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserTO> list = ConvertDTToList(sqlReader);
                return list;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static List<DropDownTO> SelectAllActiveUsersForDropDown()
        {

            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = " SELECT DISTINCT  idUser,userDisplayName FROM tblUser " +
                                  " INNER JOIN tblUserRole ON userId = idUser AND tblUserRole.isActive = 1 " +
                                  " WHERE tblUser.isActive=1 AND tblUserRole.isActive=1  ORDER by userDisplayName ";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idUser"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idUser"].ToString());
                    if (dateReader["userDisplayName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["userDisplayName"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public static List<TblUserTO> ConvertDTToList(SqlDataReader tblUserTODT)
        {
            List<TblUserTO> tblUserTOList = new List<TblUserTO>();
            if (tblUserTODT != null)
            {
                while (tblUserTODT.Read())
                {
                    TblUserTO tblUserTONew = new TblUserTO();
                    if (tblUserTODT["idUser"] != DBNull.Value)
                        tblUserTONew.IdUser = Convert.ToInt32(tblUserTODT["idUser"].ToString());
                    if (tblUserTODT["isActive"] != DBNull.Value)
                        tblUserTONew.IsActive = Convert.ToInt32(tblUserTODT["isActive"].ToString());
                    if (tblUserTODT["userLogin"] != DBNull.Value)
                        tblUserTONew.UserLogin = Convert.ToString(tblUserTODT["userLogin"].ToString());
                    if (tblUserTODT["userPasswd"] != DBNull.Value)
                        tblUserTONew.UserPasswd = Convert.ToString(tblUserTODT["userPasswd"].ToString());
                    if (tblUserTODT["personId"] != DBNull.Value)
                        tblUserTONew.PersonId = Convert.ToInt32(tblUserTODT["personId"].ToString());
                    if (tblUserTODT["addressId"] != DBNull.Value)
                        tblUserTONew.AddressId = Convert.ToInt32(tblUserTODT["addressId"].ToString());
                    if (tblUserTODT["organizationId"] != DBNull.Value)
                        tblUserTONew.OrganizationId = Convert.ToInt32(tblUserTODT["organizationId"].ToString());
                    if (tblUserTODT["firmName"] != DBNull.Value)
                        tblUserTONew.OrganizationName = Convert.ToString(tblUserTODT["firmName"].ToString());
                    if (tblUserTODT["userDisplayName"] != DBNull.Value)
                        tblUserTONew.UserDisplayName = Convert.ToString(tblUserTODT["userDisplayName"].ToString());
                    if (tblUserTODT["registeredDeviceId"] != DBNull.Value)
                        tblUserTONew.RegisteredDeviceId = Convert.ToString(tblUserTODT["registeredDeviceId"].ToString());
                    if (tblUserTODT["isSpecialCnf"] != DBNull.Value)
                        tblUserTONew.IsSpecialCnf = Convert.ToInt32(tblUserTODT["isSpecialCnf"].ToString());
                    if (tblUserTODT["imeiNumber"] != DBNull.Value)
                        tblUserTONew.ImeiNumber = Convert.ToString(tblUserTODT["imeiNumber"].ToString());
                    if (tblUserTODT["doucmentId"] != DBNull.Value)
                        tblUserTONew.DoucmentId = Convert.ToInt32(tblUserTODT["doucmentId"].ToString());
                    if (tblUserTODT["path"] != DBNull.Value)
                        tblUserTONew.Path = Convert.ToString(tblUserTODT["path"].ToString());

                    //Priyanka [09-04-2019]
                    if (tblUserTODT["orgTypeId"] != DBNull.Value)
                        tblUserTONew.OrgTypeId = Convert.ToInt32(tblUserTODT["orgTypeId"].ToString());

                    tblUserTOList.Add(tblUserTONew);
                }
            }
            return tblUserTOList;
        }

        public static TblUserTO SelectUserByImeiNumber(string imeiNumber, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE imeiNumber = '" + imeiNumber + "' ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }


        //Sudhir[08-MAR-2018] Added for Get Users for Organizations Structre Based on User ID's
        public static List<DropDownTO> SelectUsersOnUserIds(string userIds)
        {

            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = SqlSelectQuery() + " WHERE userDtl.idUser IN (" + userIds + ") AND userDtl.isActive=1";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idUser"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idUser"].ToString());
                    if (dateReader["userDisplayName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["userDisplayName"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        #endregion

        #region Insertion
        public static int InsertTblUser(TblUserTO tblUserTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblUserTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public static int InsertTblUser(TblUserTO tblUserTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblUserTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(TblUserTO tblUserTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblUser]( " +
                            "  [isActive]" +
                            " ,[userLogin]" +
                            " ,[userPasswd]" +
                            " ,[userDisplayName]" +
                            " ,[registeredDeviceId]" +
                            " ,[imeiNumber]" +
                            " ,[doucmentId]" +
                            " )" +
                " VALUES (" +
                            "  @IsActive " +
                            " ,@UserLogin " +
                            " ,@UserPasswd " +
                            " ,@userDisplayName " +
                            " ,@registeredDeviceId " +
                            " ,@imeiNumber " +
                            " ,@doucmentId " +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdUser", System.Data.SqlDbType.Int).Value = tblUserTO.IdUser;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblUserTO.IsActive;
            cmdInsert.Parameters.Add("@UserLogin", System.Data.SqlDbType.VarChar).Value = tblUserTO.UserLogin;
            cmdInsert.Parameters.Add("@UserPasswd", System.Data.SqlDbType.VarChar).Value = tblUserTO.UserPasswd;
            cmdInsert.Parameters.Add("@userDisplayName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserTO.UserDisplayName);
            cmdInsert.Parameters.Add("@registeredDeviceId", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserTO.RegisteredDeviceId);
            cmdInsert.Parameters.Add("@imeiNumber", System.Data.SqlDbType.NVarChar, 50).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserTO.ImeiNumber);
            cmdInsert.Parameters.Add("@doucmentId ", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserTO.DoucmentId);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblUserTO.IdUser = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation
        public static int UpdateTblUser(TblUserTO tblUserTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblUserTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public static int UpdateTblUser(TblUserTO tblUserTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblUserTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteUpdationCommand(TblUserTO tblUserTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblUser] SET " +
                            "  [isActive]= @IsActive" +
                            " ,[userLogin]= @UserLogin" +
                            " ,[userPasswd] = @UserPasswd" +
                            " ,[userDisplayName] = @userDisplayName" +
                            " ,[registeredDeviceId] = @registeredDeviceId" +
                            " ,[deactivatedOn] = @deactivatedOn" +
                            " ,[deactivatedBy] = @deactivatedBy" +
                            " ,[imeiNumber] = @imeiNumber" +
                            " ,[doucmentId] = @doucmentId" +
                            " WHERE [idUser] = @IdUser ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdUser", System.Data.SqlDbType.Int).Value = tblUserTO.IdUser;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblUserTO.IsActive;
            cmdUpdate.Parameters.Add("@UserLogin", System.Data.SqlDbType.VarChar).Value = tblUserTO.UserLogin;
            cmdUpdate.Parameters.Add("@UserPasswd", System.Data.SqlDbType.VarChar).Value = tblUserTO.UserPasswd;
            cmdUpdate.Parameters.Add("@userDisplayName", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserTO.UserDisplayName);
            cmdUpdate.Parameters.Add("@registeredDeviceId", System.Data.SqlDbType.NVarChar, 500).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserTO.RegisteredDeviceId);
            cmdUpdate.Parameters.Add("@deactivatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserTO.DeactivatedOn);
            cmdUpdate.Parameters.Add("@deactivatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserTO.DeactivatedBy);
            cmdUpdate.Parameters.Add("@imeiNumber", System.Data.SqlDbType.NVarChar, 50).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserTO.ImeiNumber);
            cmdUpdate.Parameters.Add("@doucmentId ", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserTO.DoucmentId);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblUser(Int32 idUser)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idUser, cmdDelete);
            }
            catch (Exception ex)
            {


                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public static int DeleteTblUser(Int32 idUser, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idUser, cmdDelete);
            }
            catch (Exception ex)
            {


                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public static int ExecuteDeletionCommand(Int32 idUser, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblUser] " +
            " WHERE idUser = " + idUser + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idUser", System.Data.SqlDbType.Int).Value = tblUserTO.IdUser;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
