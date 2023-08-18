using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblAlertSubscribersDAO : ITblAlertSubscribersDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblAlertSubscribersDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblAlertSubscribers]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblAlertSubscribersTO> SelectAllTblAlertSubscribers()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
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

        public  List<TblAlertSubscribersTO> SelectAllTblAlertSubscribers(Int32 alertDefId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE alertDefId=" + alertDefId;
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

        public  TblAlertSubscribersTO SelectTblAlertSubscribers(Int32 idSubscription)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idSubscription = " + idSubscription +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblAlertSubscribersTO> list = ConvertDTToList(sqlReader);
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

        public  List<TblAlertSubscribersTO> ConvertDTToList(SqlDataReader tblAlertSubscribersTODT)
        {
            List<TblAlertSubscribersTO> tblAlertSubscribersTOList = new List<TblAlertSubscribersTO>();
            if (tblAlertSubscribersTODT != null)
            {
                while (tblAlertSubscribersTODT.Read())
                {
                    TblAlertSubscribersTO tblAlertSubscribersTONew = new TblAlertSubscribersTO();
                    if (tblAlertSubscribersTODT["idSubscription"] != DBNull.Value)
                        tblAlertSubscribersTONew.IdSubscription = Convert.ToInt32(tblAlertSubscribersTODT["idSubscription"].ToString());
                    if (tblAlertSubscribersTODT["alertDefId"] != DBNull.Value)
                        tblAlertSubscribersTONew.AlertDefId = Convert.ToInt32(tblAlertSubscribersTODT["alertDefId"].ToString());
                    if (tblAlertSubscribersTODT["userId"] != DBNull.Value)
                        tblAlertSubscribersTONew.UserId = Convert.ToInt32(tblAlertSubscribersTODT["userId"].ToString());
                    if (tblAlertSubscribersTODT["roleId"] != DBNull.Value)
                        tblAlertSubscribersTONew.RoleId = Convert.ToInt32(tblAlertSubscribersTODT["roleId"].ToString());
                    if (tblAlertSubscribersTODT["subscribedBy"] != DBNull.Value)
                        tblAlertSubscribersTONew.SubscribedBy = Convert.ToInt32(tblAlertSubscribersTODT["subscribedBy"].ToString());
                    if (tblAlertSubscribersTODT["subscribedOn"] != DBNull.Value)
                        tblAlertSubscribersTONew.SubscribedOn = Convert.ToDateTime(tblAlertSubscribersTODT["subscribedOn"].ToString());

                    tblAlertSubscribersTOList.Add(tblAlertSubscribersTONew);
                }
            }
            return tblAlertSubscribersTOList;
        }

        #endregion

        #region Insertion
        public  int InsertTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblAlertSubscribersTO, cmdInsert);
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

        public  int InsertTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblAlertSubscribersTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblAlertSubscribersTO tblAlertSubscribersTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblAlertSubscribers]( " +
                            "  [alertDefId]" +
                            " ,[userId]" +
                            " ,[roleId]" +
                            " ,[subscribedBy]" +
                            " ,[subscribedOn]" +
                            " )" +
                " VALUES (" +
                            "  @AlertDefId " +
                            " ,@UserId " +
                            " ,@RoleId " +
                            " ,@SubscribedBy " +
                            " ,@SubscribedOn " +
                            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdSubscription", System.Data.SqlDbType.Int).Value = tblAlertSubscribersTO.IdSubscription;
            cmdInsert.Parameters.Add("@AlertDefId", System.Data.SqlDbType.Int).Value = tblAlertSubscribersTO.AlertDefId;
            cmdInsert.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblAlertSubscribersTO.UserId;
            cmdInsert.Parameters.Add("@RoleId", System.Data.SqlDbType.Int).Value = tblAlertSubscribersTO.RoleId;
            cmdInsert.Parameters.Add("@SubscribedBy", System.Data.SqlDbType.Int).Value = tblAlertSubscribersTO.SubscribedBy;
            cmdInsert.Parameters.Add("@SubscribedOn", System.Data.SqlDbType.DateTime).Value = tblAlertSubscribersTO.SubscribedOn;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblAlertSubscribersTO.IdSubscription = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public  int UpdateTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblAlertSubscribersTO, cmdUpdate);
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

        public  int UpdateTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblAlertSubscribersTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblAlertSubscribersTO tblAlertSubscribersTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblAlertSubscribers] SET " + 
                            "  [alertDefId]= @AlertDefId" +
                            " ,[userId]= @UserId" +
                            " ,[roleId]= @RoleId" +
                            " ,[subscribedBy]= @SubscribedBy" +
                            " ,[subscribedOn] = @SubscribedOn" +
                            " WHERE [idSubscription] = @IdSubscription "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdSubscription", System.Data.SqlDbType.Int).Value = tblAlertSubscribersTO.IdSubscription;
            cmdUpdate.Parameters.Add("@AlertDefId", System.Data.SqlDbType.Int).Value = tblAlertSubscribersTO.AlertDefId;
            cmdUpdate.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblAlertSubscribersTO.UserId;
            cmdUpdate.Parameters.Add("@RoleId", System.Data.SqlDbType.Int).Value = tblAlertSubscribersTO.RoleId;
            cmdUpdate.Parameters.Add("@SubscribedBy", System.Data.SqlDbType.Int).Value = tblAlertSubscribersTO.SubscribedBy;
            cmdUpdate.Parameters.Add("@SubscribedOn", System.Data.SqlDbType.DateTime).Value = tblAlertSubscribersTO.SubscribedOn;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblAlertSubscribers(Int32 idSubscription)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idSubscription, cmdDelete);
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

        public  int DeleteTblAlertSubscribers(Int32 idSubscription, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idSubscription, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idSubscription, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblAlertSubscribers] " +
            " WHERE idSubscription = " + idSubscription +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idSubscription", System.Data.SqlDbType.Int).Value = tblAlertSubscribersTO.IdSubscription;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
