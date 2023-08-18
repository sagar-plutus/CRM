using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblAlertSubscriptSettingsDAO : ITblAlertSubscriptSettingsDAO
    {

       
    private readonly IConnectionString _iConnectionString;
        public TblAlertSubscriptSettingsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblAlertSubscriptSettings]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettings()
        {
            String sqlConnStr =  _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
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

        public  List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettings(int subscriptionId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE subscriptionId=" + subscriptionId + " AND isActive=1";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
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
                cmdSelect.Dispose();
            }
        }

        public  TblAlertSubscriptSettingsTO SelectTblAlertSubscriptSettings(Int32 idSubscriSettings)
        {
            String sqlConnStr =  _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idSubscriSettings = " + idSubscriSettings +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblAlertSubscriptSettingsTO> list = ConvertDTToList(sqlReader);
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

        public  List<TblAlertSubscriptSettingsTO> ConvertDTToList(SqlDataReader tblAlertSubscriptSettingsTODT)
        {
            List<TblAlertSubscriptSettingsTO> tblAlertSubscriptSettingsTOList = new List<TblAlertSubscriptSettingsTO>();
            if (tblAlertSubscriptSettingsTODT != null)
            {
                while (tblAlertSubscriptSettingsTODT.Read())
                {
                    TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTONew = new TblAlertSubscriptSettingsTO();
                    if (tblAlertSubscriptSettingsTODT["idSubscriSettings"] != DBNull.Value)
                        tblAlertSubscriptSettingsTONew.IdSubscriSettings = Convert.ToInt32(tblAlertSubscriptSettingsTODT["idSubscriSettings"].ToString());
                    if (tblAlertSubscriptSettingsTODT["subscriptionId"] != DBNull.Value)
                        tblAlertSubscriptSettingsTONew.SubscriptionId = Convert.ToInt32(tblAlertSubscriptSettingsTODT["subscriptionId"].ToString());
                    if (tblAlertSubscriptSettingsTODT["escalationSettingId"] != DBNull.Value)
                        tblAlertSubscriptSettingsTONew.EscalationSettingId = Convert.ToInt32(tblAlertSubscriptSettingsTODT["escalationSettingId"].ToString());
                    if (tblAlertSubscriptSettingsTODT["notificationTypeId"] != DBNull.Value)
                        tblAlertSubscriptSettingsTONew.NotificationTypeId = Convert.ToInt32(tblAlertSubscriptSettingsTODT["notificationTypeId"].ToString());
                    if (tblAlertSubscriptSettingsTODT["isActive"] != DBNull.Value)
                        tblAlertSubscriptSettingsTONew.IsActive = Convert.ToInt32(tblAlertSubscriptSettingsTODT["isActive"].ToString());
                    if (tblAlertSubscriptSettingsTODT["createdOn"] != DBNull.Value)
                        tblAlertSubscriptSettingsTONew.CreatedOn = Convert.ToDateTime(tblAlertSubscriptSettingsTODT["createdOn"].ToString());
                    tblAlertSubscriptSettingsTOList.Add(tblAlertSubscriptSettingsTONew);
                }
            }
            return tblAlertSubscriptSettingsTOList;
        }
        #endregion

        #region Insertion
        public  int InsertTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO)
        {
            String sqlConnStr =  _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblAlertSubscriptSettingsTO, cmdInsert);
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

        public  int InsertTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblAlertSubscriptSettingsTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblAlertSubscriptSettings]( " + 
                                "  [subscriptionId]" +
                                " ,[escalationSettingId]" +
                                " ,[notificationTypeId]" +
                                " ,[isActive]" +
                                " ,[createdOn]" +
                                " )" +
                    " VALUES (" +
                                "  @SubscriptionId " +
                                " ,@EscalationSettingId " +
                                " ,@NotificationTypeId " +
                                " ,@IsActive " +
                                " ,@CreatedOn " + 
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdSubscriSettings", System.Data.SqlDbType.Int).Value = tblAlertSubscriptSettingsTO.IdSubscriSettings;
            cmdInsert.Parameters.Add("@SubscriptionId", System.Data.SqlDbType.Int).Value = tblAlertSubscriptSettingsTO.SubscriptionId;
            cmdInsert.Parameters.Add("@EscalationSettingId", System.Data.SqlDbType.Int).Value = tblAlertSubscriptSettingsTO.EscalationSettingId;
            cmdInsert.Parameters.Add("@NotificationTypeId", System.Data.SqlDbType.Int).Value = tblAlertSubscriptSettingsTO.NotificationTypeId;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblAlertSubscriptSettingsTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblAlertSubscriptSettingsTO.CreatedOn;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblAlertSubscriptSettingsTO.IdSubscriSettings = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public  int UpdateTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO)
        {
            String sqlConnStr =  _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblAlertSubscriptSettingsTO, cmdUpdate);
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

        public  int UpdateTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblAlertSubscriptSettingsTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblAlertSubscriptSettings] SET " + 
            "  [idSubscriSettings] = @IdSubscriSettings" +
            " ,[subscriptionId]= @SubscriptionId" +
            " ,[escalationSettingId]= @EscalationSettingId" +
            " ,[notificationTypeId]= @NotificationTypeId" +
            " ,[isActive]= @IsActive" +
            " ,[createdOn] = @CreatedOn" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdSubscriSettings", System.Data.SqlDbType.Int).Value = tblAlertSubscriptSettingsTO.IdSubscriSettings;
            cmdUpdate.Parameters.Add("@SubscriptionId", System.Data.SqlDbType.Int).Value = tblAlertSubscriptSettingsTO.SubscriptionId;
            cmdUpdate.Parameters.Add("@EscalationSettingId", System.Data.SqlDbType.Int).Value = tblAlertSubscriptSettingsTO.EscalationSettingId;
            cmdUpdate.Parameters.Add("@NotificationTypeId", System.Data.SqlDbType.Int).Value = tblAlertSubscriptSettingsTO.NotificationTypeId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblAlertSubscriptSettingsTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblAlertSubscriptSettingsTO.CreatedOn;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblAlertSubscriptSettings(Int32 idSubscriSettings)
        {
            String sqlConnStr =  _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idSubscriSettings, cmdDelete);
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

        public  int DeleteTblAlertSubscriptSettings(Int32 idSubscriSettings, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idSubscriSettings, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idSubscriSettings, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblAlertSubscriptSettings] " +
            " WHERE idSubscriSettings = " + idSubscriSettings +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idSubscriSettings", System.Data.SqlDbType.Int).Value = tblAlertSubscriptSettingsTO.IdSubscriSettings;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
