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
    public class TblAlertActionDtlDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblAlertActionDtl]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblAlertActionDtlTO> SelectAllTblAlertActionDtl()
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
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }


 public static List<TblAlertActionDtlTO> SelectAllTblAlertActionDtlOnTime()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                String sqlQuery = "select act.*,u.registeredDeviceId,tblAlert.alertComment,tblDef.alertDefDesc from  [tblAlertActionDtl] act left join tblUser u on u.idUser = act.userId " +
                    " inner join tblAlertInstance tblAlert on tblAlert.idAlertInstance = act.alertInstanceId" +
                    " inner join tblAlertDefinition tblDef on tblDef.idAlertDef = tblAlert.alertDefinitionId " +
                    " where idAlertActionDtl IN"
                    + " ( select max(idAlertActionDtl) as latestidAlertActionDtl from[tblAlertActionDtl] " +
                    "group by alertInstanceId,userID) ";
             
                DateTime currDate = Constants.ServerDateTime;
                var strtDate = new DateTime(currDate.Year, currDate.Month, currDate.Day, currDate.Hour, currDate.Minute, 0).ToString("yyyy-MM-dd HH':'mm':'ss");
                var endDate = new DateTime(currDate.Year, currDate.Month, currDate.Day, currDate.Hour, currDate.Minute, 59).ToString("yyyy-MM-dd HH':'mm':'ss");
                //String snoozeWhere = "";
                String snoozeWhere = " AND ( snoozeOn > '" + strtDate + "' And  snoozeOn < '" + endDate + "' )";
                cmdSelect.CommandText = sqlQuery + snoozeWhere + " And u.registeredDeviceId is not null ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToListSnooze(sqlReader);
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

        public static List<TblAlertActionDtlTO> ConvertDTToListSnooze(SqlDataReader tblAlertActionDtlTODT)
        {
            List<TblAlertActionDtlTO> tblAlertActionDtlTOList = new List<TblAlertActionDtlTO>();
            if (tblAlertActionDtlTODT != null)
            {
                while (tblAlertActionDtlTODT.Read())
                {
                    TblAlertActionDtlTO tblAlertActionDtlTONew = new TblAlertActionDtlTO();
                    if (tblAlertActionDtlTODT["idAlertActionDtl"] != DBNull.Value)
                        tblAlertActionDtlTONew.IdAlertActionDtl = Convert.ToInt32(tblAlertActionDtlTODT["idAlertActionDtl"].ToString());
                    if (tblAlertActionDtlTODT["alertInstanceId"] != DBNull.Value)
                        tblAlertActionDtlTONew.AlertInstanceId = Convert.ToInt32(tblAlertActionDtlTODT["alertInstanceId"].ToString());
                    if (tblAlertActionDtlTODT["userId"] != DBNull.Value)
                        tblAlertActionDtlTONew.UserId = Convert.ToInt32(tblAlertActionDtlTODT["userId"].ToString());
                    if (tblAlertActionDtlTODT["snoozeTime"] != DBNull.Value)
                        tblAlertActionDtlTONew.SnoozeTime = Convert.ToInt32(tblAlertActionDtlTODT["snoozeTime"].ToString());
                    if (tblAlertActionDtlTODT["snoozeCount"] != DBNull.Value)
                        tblAlertActionDtlTONew.SnoozeCount = Convert.ToInt32(tblAlertActionDtlTODT["snoozeCount"].ToString());
                    if (tblAlertActionDtlTODT["acknowledgedOn"] != DBNull.Value)
                        tblAlertActionDtlTONew.AcknowledgedOn = Convert.ToDateTime(tblAlertActionDtlTODT["acknowledgedOn"].ToString());
                    if (tblAlertActionDtlTODT["snoozeOn"] != DBNull.Value)
                        tblAlertActionDtlTONew.SnoozeOn = Convert.ToDateTime(tblAlertActionDtlTODT["snoozeOn"].ToString());
                    if (tblAlertActionDtlTODT["resetDate"] != DBNull.Value)
                        tblAlertActionDtlTONew.ResetDate = Convert.ToDateTime(tblAlertActionDtlTODT["resetDate"].ToString());
                    if (tblAlertActionDtlTODT["registeredDeviceId"] != DBNull.Value)
                        tblAlertActionDtlTONew.DeviceId = Convert.ToString(tblAlertActionDtlTODT["registeredDeviceId"].ToString());
                    if (tblAlertActionDtlTODT["alertComment"] != DBNull.Value)
                        tblAlertActionDtlTONew.AlertComment = Convert.ToString(tblAlertActionDtlTODT["alertComment"].ToString());
                    if (tblAlertActionDtlTODT["alertDefDesc"] != DBNull.Value)
                        tblAlertActionDtlTONew.DefDesc = Convert.ToString(tblAlertActionDtlTODT["alertDefDesc"].ToString());
                    tblAlertActionDtlTOList.Add(tblAlertActionDtlTONew);

                }
            }
            return tblAlertActionDtlTOList;
        }
        public static TblAlertActionDtlTO SelectTblAlertActionDtl(Int32 idAlertActionDtl)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idAlertActionDtl = " + idAlertActionDtl +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblAlertActionDtlTO> list = ConvertDTToList(sqlReader);
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
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblAlertActionDtlTO SelectTblAlertActionDtl(Int32 alertInstanceId, Int32 userId,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE alertInstanceId = " + alertInstanceId + "  AND userId=" + userId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblAlertActionDtlTO> list = ConvertDTToList(sqlReader);
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

        public static List<TblAlertActionDtlTO> SelectAllTblAlertActionDtl(Int32 userId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE userId=" + userId + " AND alertInstanceId IN(SELECT idAlertInstance FROM tblAlertInstance WHERE isActive=1)";
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
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblAlertActionDtlTO> ConvertDTToList(SqlDataReader tblAlertActionDtlTODT)
        {
            List<TblAlertActionDtlTO> tblAlertActionDtlTOList = new List<TblAlertActionDtlTO>();
            if (tblAlertActionDtlTODT != null)
            {
                while (tblAlertActionDtlTODT.Read())
                {
                    TblAlertActionDtlTO tblAlertActionDtlTONew = new TblAlertActionDtlTO();
                    if (tblAlertActionDtlTODT["idAlertActionDtl"] != DBNull.Value)
                        tblAlertActionDtlTONew.IdAlertActionDtl = Convert.ToInt32(tblAlertActionDtlTODT["idAlertActionDtl"].ToString());
                    if (tblAlertActionDtlTODT["alertInstanceId"] != DBNull.Value)
                        tblAlertActionDtlTONew.AlertInstanceId = Convert.ToInt32(tblAlertActionDtlTODT["alertInstanceId"].ToString());
                    if (tblAlertActionDtlTODT["userId"] != DBNull.Value)
                        tblAlertActionDtlTONew.UserId = Convert.ToInt32(tblAlertActionDtlTODT["userId"].ToString());
                    if (tblAlertActionDtlTODT["snoozeTime"] != DBNull.Value)
                        tblAlertActionDtlTONew.SnoozeTime = Convert.ToInt32(tblAlertActionDtlTODT["snoozeTime"].ToString());
                    if (tblAlertActionDtlTODT["snoozeCount"] != DBNull.Value)
                        tblAlertActionDtlTONew.SnoozeCount = Convert.ToInt32(tblAlertActionDtlTODT["snoozeCount"].ToString());
                    if (tblAlertActionDtlTODT["acknowledgedOn"] != DBNull.Value)
                        tblAlertActionDtlTONew.AcknowledgedOn = Convert.ToDateTime(tblAlertActionDtlTODT["acknowledgedOn"].ToString());
                    if (tblAlertActionDtlTODT["snoozeOn"] != DBNull.Value)
                        tblAlertActionDtlTONew.SnoozeOn = Convert.ToDateTime(tblAlertActionDtlTODT["snoozeOn"].ToString());
                    if (tblAlertActionDtlTODT["resetDate"] != DBNull.Value)
                        tblAlertActionDtlTONew.ResetDate = Convert.ToDateTime(tblAlertActionDtlTODT["resetDate"].ToString());
                    tblAlertActionDtlTOList.Add(tblAlertActionDtlTONew);
                }
            }
            return tblAlertActionDtlTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblAlertActionDtl(TblAlertActionDtlTO tblAlertActionDtlTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblAlertActionDtlTO, cmdInsert);
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

        public static int InsertTblAlertActionDtl(TblAlertActionDtlTO tblAlertActionDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblAlertActionDtlTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblAlertActionDtlTO tblAlertActionDtlTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblAlertActionDtl]( " + 
                            "  [alertInstanceId]" +
                            " ,[userId]" +
                            " ,[snoozeTime]" +
                            " ,[snoozeCount]" +
                            " ,[acknowledgedOn]" +
                            " ,[snoozeOn]" +
                            " ,[resetDate]" +
                            " )" +
                " VALUES (" +
                            "  @AlertInstanceId " +
                            " ,@UserId " +
                            " ,@SnoozeTime " +
                            " ,@SnoozeCount " +
                            " ,@AcknowledgedOn " +
                            " ,@SnoozeOn " +
                            " ,@ResetDate " + 
                            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdAlertActionDtl", System.Data.SqlDbType.Int).Value = tblAlertActionDtlTO.IdAlertActionDtl;
            cmdInsert.Parameters.Add("@AlertInstanceId", System.Data.SqlDbType.Int).Value = tblAlertActionDtlTO.AlertInstanceId;
            cmdInsert.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblAlertActionDtlTO.UserId;
            cmdInsert.Parameters.Add("@SnoozeTime", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertActionDtlTO.SnoozeTime);
            cmdInsert.Parameters.Add("@SnoozeCount", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertActionDtlTO.SnoozeCount);
            cmdInsert.Parameters.Add("@AcknowledgedOn", System.Data.SqlDbType.DateTime).Value = tblAlertActionDtlTO.AcknowledgedOn;
            cmdInsert.Parameters.Add("@SnoozeOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertActionDtlTO.SnoozeOn);
            cmdInsert.Parameters.Add("@ResetDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue( tblAlertActionDtlTO.ResetDate);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblAlertActionDtlTO.IdAlertActionDtl = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblAlertActionDtl(TblAlertActionDtlTO tblAlertActionDtlTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblAlertActionDtlTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public static int UpdateTblAlertActionDtl(TblAlertActionDtlTO tblAlertActionDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblAlertActionDtlTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteUpdationCommand(TblAlertActionDtlTO tblAlertActionDtlTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblAlertActionDtl] SET " + 
                            "  [alertInstanceId]= @AlertInstanceId" +
                            " ,[userId]= @UserId" +
                            " ,[snoozeTime]= @SnoozeTime" +
                            " ,[snoozeCount]= @SnoozeCount" +
                            " ,[acknowledgedOn]= @AcknowledgedOn" +
                            " ,[snoozeOn]= @SnoozeOn" +
                            " ,[resetDate] = @ResetDate" +
                            " WHERE [idAlertActionDtl] = @IdAlertActionDtl "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdAlertActionDtl", System.Data.SqlDbType.Int).Value = tblAlertActionDtlTO.IdAlertActionDtl;
            cmdUpdate.Parameters.Add("@AlertInstanceId", System.Data.SqlDbType.Int).Value = tblAlertActionDtlTO.AlertInstanceId;
            cmdUpdate.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblAlertActionDtlTO.UserId;
            cmdUpdate.Parameters.Add("@SnoozeTime", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertActionDtlTO.SnoozeTime);
            cmdUpdate.Parameters.Add("@SnoozeCount", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertActionDtlTO.SnoozeCount);
            cmdUpdate.Parameters.Add("@AcknowledgedOn", System.Data.SqlDbType.DateTime).Value = tblAlertActionDtlTO.AcknowledgedOn;
            cmdUpdate.Parameters.Add("@SnoozeOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertActionDtlTO.SnoozeOn);
            cmdUpdate.Parameters.Add("@ResetDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertActionDtlTO.ResetDate);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblAlertActionDtl(Int32 idAlertActionDtl)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idAlertActionDtl, cmdDelete);
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

        public static int DeleteTblAlertActionDtl(Int32 idAlertActionDtl, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idAlertActionDtl, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idAlertActionDtl, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblAlertActionDtl] " +
            " WHERE idAlertActionDtl = " + idAlertActionDtl +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idAlertActionDtl", System.Data.SqlDbType.Int).Value = tblAlertActionDtlTO.IdAlertActionDtl;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
