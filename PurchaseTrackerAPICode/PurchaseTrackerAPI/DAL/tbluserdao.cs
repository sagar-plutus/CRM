using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblUserDAO : ITblUserDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblUserDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT userDtl.* ,userExt.personId,userExt.addressId,userExt.organizationId,org.firmName ,org.isSpecialCnf " +
                                  " FROM tblUser userDtl " +
                                  " LEFT JOIN tblUserExt userExt " +
                                  " ON userDtl.idUser = userExt.userId " +
                                  " LEFT JOIN tblOrganization org ON org.idOrganization=userExt.organizationId";

            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblUserTO> SelectAllTblUser(Boolean onlyActiveYn)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
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

        public  TblUserTO SelectTblUser(Int32 idUser, SqlConnection conn, SqlTransaction tran)
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

        public  TblUserTO SelectTblUser(String userID, String password)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
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

        public  Boolean IsThisUserExists(String userID, SqlConnection conn, SqlTransaction tran)
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

        public  Dictionary<int, string> SelectUserMobileNoDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            Dictionary<int, string> DCT = null;
            String whereCond = string.Empty;
            try
            {
                if (isUser)
                {
                    whereCond = " WHERE idUser IN(" + userOrRoleIds + ") AND mobileNo IS NOT NULL";
                }
                else
                    whereCond = " WHERE roleId IN(" + userOrRoleIds + ") AND mobileNo IS NOT NULL";

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

        public  Dictionary<int, string> SelectUserDeviceRegNoDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran)
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

        public  List<TblUserTO> SelectAllTblUser(Int32 orgId, SqlConnection conn, SqlTransaction tran)
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

        public  List<DropDownTO> SelectAllActiveUsersForDropDown()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = " SELECT idUser,userDisplayName FROM tblUser " +
                                  " INNER JOIN tblUserRole ON userId = idUser " +
                                  " WHERE tblUser.isActive=1 ";

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

        public  List<TblUserTO> ConvertDTToList(SqlDataReader tblUserTODT)
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
                    tblUserTOList.Add(tblUserTONew);
                }
            }
            return tblUserTOList;
        }

        public  TblUserTO SelectUserByImeiNumber(string imeiNumber, SqlConnection conn, SqlTransaction tran)
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

        public  List<DropDownTO> GetUnloadingPersonListForDropDown(string roleId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = " select * from tblUser tblUser " +
                                  " INNER JOIN tblUserRole tblUserRole on tblUserRole.userId=tblUser.idUser " +
                                  " INNER JOIN tblRole tblRole on tblRole.idRole= tblUserRole.roleId" +
                                  " INNER JOIN tblUserExt UserExt ON UserExt.userId=tblUser.idUser " + 
                                  " where tblUser.isActive=1 and tblUserRole.isActive=1 and tblRole.idRole in (" + roleId + ")";

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

           public  List<DropDownTO> GetUnloadingPersonListForDropDown(Int32 roleId,SqlConnection conn,SqlTransaction tran)
        {

            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                String aqlQuery = " select * from tblUser tblUser " +
                                  " INNER JOIN tblUserRole tblUserRole on tblUserRole.userId=tblUser.idUser " +
                                  " INNER JOIN tblRole tblRole on tblRole.idRole= tblUserRole.roleId" +
                                  " where tblRole.idRole=" + roleId;

                cmdSelect = new SqlCommand(aqlQuery, conn,tran);
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
                cmdSelect.Dispose();
            }

        }
        #endregion

        #region Insertion
        public  int InsertTblUser(TblUserTO tblUserTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
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

        public  int InsertTblUser(TblUserTO tblUserTO, SqlConnection conn, SqlTransaction tran)
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

        public  int ExecuteInsertionCommand(TblUserTO tblUserTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblUser]( " +
                            "  [isActive]" +
                            " ,[userLogin]" +
                            " ,[userPasswd]" +
                            " ,[userDisplayName]" +
                            " ,[registeredDeviceId]" +
                            " ,[imeiNumber]" +
                            " )" +
                " VALUES (" +
                            "  @IsActive " +
                            " ,@UserLogin " +
                            " ,@UserPasswd " +
                            " ,@userDisplayName " +
                            " ,@registeredDeviceId " +
                            " ,@imeiNumber " +
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
        public  int UpdateTblUser(TblUserTO tblUserTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
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

        public  int UpdateTblUser(TblUserTO tblUserTO, SqlConnection conn, SqlTransaction tran)
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

        public  int ExecuteUpdationCommand(TblUserTO tblUserTO, SqlCommand cmdUpdate)
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
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public  int DeleteTblUser(Int32 idUser)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
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

        public  int DeleteTblUser(Int32 idUser, SqlConnection conn, SqlTransaction tran)
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

        public  int ExecuteDeletionCommand(Int32 idUser, SqlCommand cmdDelete)
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
