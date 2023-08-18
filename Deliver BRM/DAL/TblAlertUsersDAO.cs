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
    public class TblAlertUsersDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT alertUser.*  , alert.raisedOn ,alert.alertComment, alert.sourceEntityId ,userDtl.userDisplayName ,alertDef.moduleId,alert.alertDefinitionId,alertDef.navigationUrl " +
                                  " FROM tblAlertUsers alertUser " +
                                  " INNER JOIN tblAlertInstance alert ON alertUser.alertInstanceId = alert.idAlertInstance " +
                                  " LEFT JOIN tblUser userDtl ON alert.raisedBy = userDtl.idUser" +
                                  " LEFT JOIN tblAlertDefinition alertDef ON alertDef.idAlertDef=alert.alertDefinitionId ";

            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblAlertUsersTO> SelectAllTblAlertUsers()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(sqlReader);
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose(); conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblAlertUsersTO> SelectAllActiveNotAKAlertList(Int32 userId, Int32 roleId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE (alertUser.userId=" + userId + " OR alertUser.roleId=" + roleId + ")" +
                                        " AND alertUser.alertInstanceId NOT IN(SELECT alertInstanceId FROM tblAlertActionDtl WHERE userId=" + userId + ")" +
                                        " AND alert.isActive=1";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(sqlReader);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose(); conn.Close();
                cmdSelect.Dispose();
            }
        }


        public static List<TblAlertUsersTO> SelectAllActiveAlertList(Int32 userId, String roleIds)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE (alertUser.userId=" + userId + " OR alertUser.roleId  In (" + roleIds + "))" +
                                        " AND alert.isActive=1";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(sqlReader);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose(); conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblAlertUsersTO SelectTblAlertUsers(Int32 idAlertUser)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idAlertUser = " + idAlertUser +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblAlertUsersTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose(); conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblAlertUsersTO> ConvertDTToList(SqlDataReader tblAlertUsersTODT)
        {
            List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();
            if (tblAlertUsersTODT != null)
            {
                while (tblAlertUsersTODT.Read())
                {
                    TblAlertUsersTO tblAlertUsersTONew = new TblAlertUsersTO();
                    if (tblAlertUsersTODT["idAlertUser"] != DBNull.Value)
                        tblAlertUsersTONew.IdAlertUser = Convert.ToInt32(tblAlertUsersTODT["idAlertUser"].ToString());
                    if (tblAlertUsersTODT["alertInstanceId"] != DBNull.Value)
                        tblAlertUsersTONew.AlertInstanceId = Convert.ToInt32(tblAlertUsersTODT["alertInstanceId"].ToString());
                    if (tblAlertUsersTODT["userId"] != DBNull.Value)
                        tblAlertUsersTONew.UserId = Convert.ToInt32(tblAlertUsersTODT["userId"].ToString());
                    if (tblAlertUsersTODT["roleId"] != DBNull.Value)
                        tblAlertUsersTONew.RoleId = Convert.ToInt32(tblAlertUsersTODT["roleId"].ToString());
                    if (tblAlertUsersTODT["alertComment"] != DBNull.Value)
                        tblAlertUsersTONew.AlertComment = Convert.ToString(tblAlertUsersTODT["alertComment"].ToString());
                    if (tblAlertUsersTODT["userDisplayName"] != DBNull.Value)
                        tblAlertUsersTONew.RaisedByUserName = Convert.ToString(tblAlertUsersTODT["userDisplayName"].ToString());
                    if (tblAlertUsersTODT["raisedOn"] != DBNull.Value)
                        tblAlertUsersTONew.RaisedOn = Convert.ToDateTime(tblAlertUsersTODT["raisedOn"].ToString());
                    if (tblAlertUsersTODT["alertDefinitionId"] != DBNull.Value)
                        tblAlertUsersTONew.AlertDefinitionId = Convert.ToInt32(tblAlertUsersTODT["alertDefinitionId"].ToString());
                    if (tblAlertUsersTODT["navigationUrl"] != DBNull.Value)
                        tblAlertUsersTONew.NavigationUrl = Convert.ToString(tblAlertUsersTODT["navigationUrl"].ToString());
                    if (tblAlertUsersTODT["moduleId"] != DBNull.Value)
                        tblAlertUsersTONew.ModuleId = Convert.ToInt32(tblAlertUsersTODT["moduleId"].ToString());
                    if (tblAlertUsersTODT["sourceEntityId"] != DBNull.Value)
                        tblAlertUsersTONew.SourceEntityId = Convert.ToInt32(tblAlertUsersTODT["sourceEntityId"].ToString());

                    tblAlertUsersTOList.Add(tblAlertUsersTONew);
                }
            }
            return tblAlertUsersTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblAlertUsers(TblAlertUsersTO tblAlertUsersTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblAlertUsersTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public static int InsertTblAlertUsers(TblAlertUsersTO tblAlertUsersTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblAlertUsersTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(TblAlertUsersTO tblAlertUsersTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblAlertUsers]( " +
                            "  [alertInstanceId]" +
                            " ,[userId]" +
                            " ,[roleId]" +
                            " )" +
                " VALUES (" +
                            "  @AlertInstanceId " +
                            " ,@UserId " +
                            " ,@RoleId " +
                            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdAlertUser", System.Data.SqlDbType.Int).Value = tblAlertUsersTO.IdAlertUser;
            cmdInsert.Parameters.Add("@AlertInstanceId", System.Data.SqlDbType.Int).Value = tblAlertUsersTO.AlertInstanceId;
            cmdInsert.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertUsersTO.UserId);
            cmdInsert.Parameters.Add("@RoleId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue( tblAlertUsersTO.RoleId);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblAlertUsersTO.IdAlertUser = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblAlertUsers(TblAlertUsersTO tblAlertUsersTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblAlertUsersTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                
               
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public static int UpdateTblAlertUsers(TblAlertUsersTO tblAlertUsersTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblAlertUsersTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                
               
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteUpdationCommand(TblAlertUsersTO tblAlertUsersTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblAlertUsers] SET " + 
            "  [idAlertUser] = @IdAlertUser" +
            " ,[alertInstanceId]= @AlertInstanceId" +
            " ,[userId]= @UserId" +
            " ,[roleId] = @RoleId" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdAlertUser", System.Data.SqlDbType.Int).Value = tblAlertUsersTO.IdAlertUser;
            cmdUpdate.Parameters.Add("@AlertInstanceId", System.Data.SqlDbType.Int).Value = tblAlertUsersTO.AlertInstanceId;
            cmdUpdate.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblAlertUsersTO.UserId;
            cmdUpdate.Parameters.Add("@RoleId", System.Data.SqlDbType.Int).Value = tblAlertUsersTO.RoleId;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblAlertUsers(Int32 idAlertUser)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idAlertUser, cmdDelete);
            }
            catch(Exception ex)
            {
                
               
                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public static int DeleteTblAlertUsers(Int32 idAlertUser, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idAlertUser, cmdDelete);
            }
            catch(Exception ex)
            {
                
               
                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public static int ExecuteDeletionCommand(Int32 idAlertUser, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblAlertUsers] " +
            " WHERE idAlertUser = " + idAlertUser +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idAlertUser", System.Data.SqlDbType.Int).Value = tblAlertUsersTO.IdAlertUser;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
