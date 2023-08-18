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
    public class TblAlertEscalationSettingsDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblAlertEscalationSettings]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblAlertEscalationSettingsTO> SelectAllTblAlertEscalationSettings()
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

        public static TblAlertEscalationSettingsTO SelectTblAlertEscalationSettings(Int32 idEscalationSetting)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idEscalationSetting = " + idEscalationSetting + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblAlertEscalationSettingsTO> list = ConvertDTToList(sqlReader);
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
                    sqlReader.Dispose(); conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblAlertEscalationSettingsTO> ConvertDTToList(SqlDataReader tblAlertEscalationSettingsTODT)
        {
            List<TblAlertEscalationSettingsTO> tblAlertEscalationSettingsTOList = new List<TblAlertEscalationSettingsTO>();
            if (tblAlertEscalationSettingsTODT != null)
            {
                while (tblAlertEscalationSettingsTODT.Read())
                {
                    TblAlertEscalationSettingsTO tblAlertEscalationSettingsTONew = new TblAlertEscalationSettingsTO();
                    if (tblAlertEscalationSettingsTODT["idEscalationSetting"] != DBNull.Value)
                        tblAlertEscalationSettingsTONew.IdEscalationSetting = Convert.ToInt32(tblAlertEscalationSettingsTODT["idEscalationSetting"].ToString());
                    if (tblAlertEscalationSettingsTODT["alertDefId"] != DBNull.Value)
                        tblAlertEscalationSettingsTONew.AlertDefId = Convert.ToInt32(tblAlertEscalationSettingsTODT["alertDefId"].ToString());
                    if (tblAlertEscalationSettingsTODT["userId"] != DBNull.Value)
                        tblAlertEscalationSettingsTONew.UserId = Convert.ToInt32(tblAlertEscalationSettingsTODT["userId"].ToString());
                    if (tblAlertEscalationSettingsTODT["roleId"] != DBNull.Value)
                        tblAlertEscalationSettingsTONew.RoleId = Convert.ToInt32(tblAlertEscalationSettingsTODT["roleId"].ToString());
                    if (tblAlertEscalationSettingsTODT["escalationPeriodMin"] != DBNull.Value)
                        tblAlertEscalationSettingsTONew.EscalationPeriodMin = Convert.ToInt32(tblAlertEscalationSettingsTODT["escalationPeriodMin"].ToString());
                    if (tblAlertEscalationSettingsTODT["createdBy"] != DBNull.Value)
                        tblAlertEscalationSettingsTONew.CreatedBy = Convert.ToInt32(tblAlertEscalationSettingsTODT["createdBy"].ToString());
                    if (tblAlertEscalationSettingsTODT["createdOn"] != DBNull.Value)
                        tblAlertEscalationSettingsTONew.CreatedOn = Convert.ToDateTime(tblAlertEscalationSettingsTODT["createdOn"].ToString());
                    tblAlertEscalationSettingsTOList.Add(tblAlertEscalationSettingsTONew);
                }
            }
            return tblAlertEscalationSettingsTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblAlertEscalationSettings(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblAlertEscalationSettingsTO, cmdInsert);
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

        public static int InsertTblAlertEscalationSettings(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblAlertEscalationSettingsTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblAlertEscalationSettings]( " + 
                                "  [alertDefId]" +
                                " ,[userId]" +
                                " ,[roleId]" +
                                " ,[escalationPeriodMin]" +
                                " ,[createdBy]" +
                                " ,[createdOn]" +
                                " )" +
                    " VALUES (" +
                                "  @AlertDefId " +
                                " ,@UserId " +
                                " ,@RoleId " +
                                " ,@EscalationPeriodMin " +
                                " ,@CreatedBy " +
                                " ,@CreatedOn " + 
                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdEscalationSetting", System.Data.SqlDbType.Int).Value = tblAlertEscalationSettingsTO.IdEscalationSetting;
            cmdInsert.Parameters.Add("@AlertDefId", System.Data.SqlDbType.Int).Value = tblAlertEscalationSettingsTO.AlertDefId;
            cmdInsert.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblAlertEscalationSettingsTO.UserId;
            cmdInsert.Parameters.Add("@RoleId", System.Data.SqlDbType.Int).Value = tblAlertEscalationSettingsTO.RoleId;
            cmdInsert.Parameters.Add("@EscalationPeriodMin", System.Data.SqlDbType.Int).Value = tblAlertEscalationSettingsTO.EscalationPeriodMin;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblAlertEscalationSettingsTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblAlertEscalationSettingsTO.CreatedOn;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblAlertEscalationSettingsTO.IdEscalationSetting = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblAlertEscalationSettings(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblAlertEscalationSettingsTO, cmdUpdate);
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

        public static int UpdateTblAlertEscalationSettings(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblAlertEscalationSettingsTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblAlertEscalationSettings] SET " + 
                            "  [alertDefId]= @AlertDefId" +
                            " ,[userId]= @UserId" +
                            " ,[roleId]= @RoleId" +
                            " ,[escalationPeriodMin]= @EscalationPeriodMin" +
                            " ,[createdBy]= @CreatedBy" +
                            " ,[createdOn] = @CreatedOn" +
                            " WHERE  [idEscalationSetting] = @IdEscalationSetting";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdEscalationSetting", System.Data.SqlDbType.Int).Value = tblAlertEscalationSettingsTO.IdEscalationSetting;
            cmdUpdate.Parameters.Add("@AlertDefId", System.Data.SqlDbType.Int).Value = tblAlertEscalationSettingsTO.AlertDefId;
            cmdUpdate.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblAlertEscalationSettingsTO.UserId;
            cmdUpdate.Parameters.Add("@RoleId", System.Data.SqlDbType.Int).Value = tblAlertEscalationSettingsTO.RoleId;
            cmdUpdate.Parameters.Add("@EscalationPeriodMin", System.Data.SqlDbType.Int).Value = tblAlertEscalationSettingsTO.EscalationPeriodMin;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblAlertEscalationSettingsTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblAlertEscalationSettingsTO.CreatedOn;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblAlertEscalationSettings(Int32 idEscalationSetting)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idEscalationSetting, cmdDelete);
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

        public static int DeleteTblAlertEscalationSettings(Int32 idEscalationSetting, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idEscalationSetting, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idEscalationSetting, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblAlertEscalationSettings] " +
            " WHERE idEscalationSetting = " + idEscalationSetting +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idEscalationSetting", System.Data.SqlDbType.Int).Value = tblAlertEscalationSettingsTO.IdEscalationSetting;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
