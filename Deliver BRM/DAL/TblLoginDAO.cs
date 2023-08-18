using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.DAL
{
    public class TblLoginDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = "SELECT login.*,userdisplayname AS userName FROM [tblLogin] AS login LEFT JOIN tbluser userdls ON login.userid = userdls.iduser";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblLoginTO> SelectAllTblLogin()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoginTO> list = ConvertDTToList(rdr);
                rdr.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public static List<TblLoginTO> GetCurrentActiveUsers()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " where logoutDate IS NULL ORDER BY logindate DESC";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoginTO> list = ConvertDTToList(rdr);
                rdr.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static dimUserConfigrationTO GetUsersConfigration(int configdesc)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT * FROM dimUserConfigration WHERE idUserConfigration =" + configdesc;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                dimUserConfigrationTO UserConfigrationTO = new dimUserConfigrationTO();
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        if (reader["idUserConfigration"] != DBNull.Value)
                            UserConfigrationTO.IdUserConfigration = Convert.ToInt32(reader["idUserConfigration"].ToString());
                        if (reader["configDesc"] != DBNull.Value)
                            UserConfigrationTO.ConfigDesc = Convert.ToString(reader["configDesc"].ToString());
                        if (reader["configValue"] != DBNull.Value)
                            UserConfigrationTO.ConfigValue = Convert.ToString(reader["configValue"].ToString());
                    }
                }
                reader.Dispose();
                return UserConfigrationTO;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static dimUserConfigrationTO GetUsersConfigration(int configdesc, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = "SELECT * FROM dimUserConfigration WHERE idUserConfigration =" + configdesc;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                dimUserConfigrationTO UserConfigrationTO = new dimUserConfigrationTO();
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        if (reader["idUserConfigration"] != DBNull.Value)
                            UserConfigrationTO.IdUserConfigration = Convert.ToInt32(reader["idUserConfigration"].ToString());
                        if (reader["configDesc"] != DBNull.Value)
                            UserConfigrationTO.ConfigDesc = Convert.ToString(reader["configDesc"].ToString());
                        if (reader["configValue"] != DBNull.Value)
                            UserConfigrationTO.ConfigValue = Convert.ToString(reader["configValue"].ToString());
                    }
                }
                reader.Dispose();
                return UserConfigrationTO;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }
        public static TblLoginTO SelectTblLogin(Int32 idLogin)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idLogin = " + idLogin + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoginTO> list = ConvertDTToList(rdr);
                rdr.Dispose();
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblLoginTO> ConvertDTToList(SqlDataReader tblLoginTODT)
        {
            List<TblLoginTO> tblLoginTOList = new List<TblLoginTO>();
            if (tblLoginTODT != null)
            {
                while (tblLoginTODT.Read())
                {
                    TblLoginTO tblLoginTONew = new TblLoginTO();
                    if (tblLoginTODT["idLogin"] != DBNull.Value)
                        tblLoginTONew.IdLogin = Convert.ToInt32(tblLoginTODT["idLogin"].ToString());
                    if (tblLoginTODT["userId"] != DBNull.Value)
                        tblLoginTONew.UserId = Convert.ToInt32(tblLoginTODT["userId"].ToString());
                    if (tblLoginTODT["loginDate"] != DBNull.Value)
                        tblLoginTONew.LoginDate = Convert.ToDateTime(tblLoginTODT["loginDate"].ToString());
                    if (tblLoginTODT["logoutDate"] != DBNull.Value)
                        tblLoginTONew.LogoutDate = Convert.ToDateTime(tblLoginTODT["logoutDate"].ToString());
                    if (tblLoginTODT["loginIP"] != DBNull.Value)
                        tblLoginTONew.LoginIP = Convert.ToString(tblLoginTODT["loginIP"].ToString());
                    if (tblLoginTODT["deviceId"] != DBNull.Value)
                        tblLoginTONew.DeviceId = Convert.ToString(tblLoginTODT["deviceId"].ToString());
                    if (tblLoginTODT["latitude"] != DBNull.Value)
                        tblLoginTONew.Latitude = Convert.ToString(tblLoginTODT["latitude"].ToString());
                    if (tblLoginTODT["longitude"] != DBNull.Value)
                        tblLoginTONew.Longitude = Convert.ToString(tblLoginTODT["longitude"].ToString());
                    if (tblLoginTODT["platformDetails"] != DBNull.Value)
                        tblLoginTONew.PlatformDetails = Convert.ToString(tblLoginTODT["platformDetails"].ToString());
                    if (tblLoginTODT["apkVersion"] != DBNull.Value)
                        tblLoginTONew.ApkVersion = Convert.ToString(tblLoginTODT["apkVersion"].ToString());
                    if (tblLoginTODT["userName"] != DBNull.Value)
                        tblLoginTONew.UserName = Convert.ToString(tblLoginTODT["userName"].ToString());
                    tblLoginTOList.Add(tblLoginTONew);
                }
            }
            return tblLoginTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblLogin(TblLoginTO tblLoginTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblLoginTO, cmdInsert);
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

        public static int InsertTblLogin(TblLoginTO tblLoginTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblLoginTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblLoginTO tblLoginTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblLogin]( " +
                                "  [userId]" +
                                " ,[loginDate]" +
                                " ,[logoutDate]" +
                                " ,[loginIP]" +
                                " ,[deviceId]" +
                                " ,[latitude]" +
                                " ,[longitude]" +
                                " ,[browserName]" +
                                " ,[platformDetails]" +
                                " ,[apkVersion]" +
                                " )" +
                    " VALUES (" +
                                "  @UserId " +
                                " ,@LoginDate " +
                                " ,@LogoutDate " +
                                " ,@LoginIP " +
                                " ,@deviceId " +
                                " ,@latitude " +
                                " ,@longitude " +
                                " ,@BrowserName " +
                                " ,@PlatformDetails " +
                                " ,@ApkVersion " +
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdLogin", System.Data.SqlDbType.Int).Value = tblLoginTO.IdLogin;
            cmdInsert.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblLoginTO.UserId;
            cmdInsert.Parameters.Add("@LoginDate", System.Data.SqlDbType.DateTime).Value = tblLoginTO.LoginDate;
            cmdInsert.Parameters.Add("@LogoutDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoginTO.LogoutDate);
            cmdInsert.Parameters.Add("@LoginIP", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoginTO.LoginIP);
            cmdInsert.Parameters.Add("@deviceId", System.Data.SqlDbType.NVarChar, 128).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoginTO.DeviceId);
            cmdInsert.Parameters.Add("@latitude", System.Data.SqlDbType.NVarChar, 128).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoginTO.Latitude);
            cmdInsert.Parameters.Add("@longitude", System.Data.SqlDbType.NVarChar, 128).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoginTO.Longitude);
            cmdInsert.Parameters.Add("@browserName", System.Data.SqlDbType.NVarChar, 128).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoginTO.BrowserName);
            cmdInsert.Parameters.Add("@platformDetails", System.Data.SqlDbType.NVarChar, 128).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoginTO.PlatformDetails);
            cmdInsert.Parameters.Add("@apkVersion", System.Data.SqlDbType.NVarChar, 128).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoginTO.ApkVersion);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblLoginTO.IdLogin = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation
        public static int UpdateTblLogin(TblLoginTO tblLoginTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblLoginTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public static int UpdateTblLogin(TblLoginTO tblLoginTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblLoginTO, cmdUpdate);
            }
            catch (Exception ex)
            {


                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteUpdationCommand(TblLoginTO tblLoginTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblLogin] SET " +
                            "  [userId]= @UserId" +
                            " ,[loginDate]= @LoginDate" +
                            " ,[logoutDate]= @LogoutDate" +
                            " ,[loginIP] = @LoginIP" +
                            " WHERE[idLogin] = @IdLogin";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdLogin", System.Data.SqlDbType.Int).Value = tblLoginTO.IdLogin;
            cmdUpdate.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblLoginTO.UserId;
            cmdUpdate.Parameters.Add("@LoginDate", System.Data.SqlDbType.DateTime).Value = tblLoginTO.LoginDate;
            cmdUpdate.Parameters.Add("@LogoutDate", System.Data.SqlDbType.DateTime).Value = tblLoginTO.LogoutDate;
            cmdUpdate.Parameters.Add("@LoginIP", System.Data.SqlDbType.VarChar).Value = tblLoginTO.LoginIP;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblLogin(Int32 idLogin)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idLogin, cmdDelete);
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

        public static int DeleteTblLogin(Int32 idLogin, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idLogin, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idLogin, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblLogin] " +
            " WHERE idLogin = " + idLogin + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idLogin", System.Data.SqlDbType.Int).Value = tblLoginTO.IdLogin;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
