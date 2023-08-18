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
    public class TblLoadingStatusHistoryDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tempLoadingStatusHistory]" +

                                  // Vaibhav [20-Nov-2017] Added to select  from finalLoadingStatusHistory
                                  " UNION ALL " +
                                  " SELECT * FROM [finalLoadingStatusHistory]";

            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblLoadingStatusHistoryTO> SelectAllTblLoadingStatusHistory(int loadingId,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE loadingId=" + loadingId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingStatusHistoryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                return list;
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

        public static TblLoadingStatusHistoryTO SelectTblLoadingStatusHistory(Int32 idLoadingHistory)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM ("+  SqlSelectQuery()+ ")sq1 WHERE idLoadingHistory = " + idLoadingHistory +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingStatusHistoryTO> list = ConvertDTToList(reader);
                reader.Dispose();
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblLoadingStatusHistoryTO> ConvertDTToList(SqlDataReader tblLoadingStatusHistoryTODT)
        {
            List<TblLoadingStatusHistoryTO> tblLoadingStatusHistoryTOList = new List<TblLoadingStatusHistoryTO>();
            if (tblLoadingStatusHistoryTODT != null)
            {
                while (tblLoadingStatusHistoryTODT.Read())
                {
                    TblLoadingStatusHistoryTO tblLoadingStatusHistoryTONew = new TblLoadingStatusHistoryTO();
                    if (tblLoadingStatusHistoryTODT["idLoadingHistory"] != DBNull.Value)
                        tblLoadingStatusHistoryTONew.IdLoadingHistory = Convert.ToInt32(tblLoadingStatusHistoryTODT["idLoadingHistory"].ToString());
                    if (tblLoadingStatusHistoryTODT["loadingId"] != DBNull.Value)
                        tblLoadingStatusHistoryTONew.LoadingId = Convert.ToInt32(tblLoadingStatusHistoryTODT["loadingId"].ToString());
                    if (tblLoadingStatusHistoryTODT["statusId"] != DBNull.Value)
                        tblLoadingStatusHistoryTONew.StatusId = Convert.ToInt32(tblLoadingStatusHistoryTODT["statusId"].ToString());
                    if (tblLoadingStatusHistoryTODT["createdBy"] != DBNull.Value)
                        tblLoadingStatusHistoryTONew.CreatedBy = Convert.ToInt32(tblLoadingStatusHistoryTODT["createdBy"].ToString());
                    if (tblLoadingStatusHistoryTODT["statusDate"] != DBNull.Value)
                        tblLoadingStatusHistoryTONew.StatusDate = Convert.ToDateTime(tblLoadingStatusHistoryTODT["statusDate"].ToString());
                    if (tblLoadingStatusHistoryTODT["createdOn"] != DBNull.Value)
                        tblLoadingStatusHistoryTONew.CreatedOn = Convert.ToDateTime(tblLoadingStatusHistoryTODT["createdOn"].ToString());
                    if (tblLoadingStatusHistoryTODT["statusRemark"] != DBNull.Value)
                        tblLoadingStatusHistoryTONew.StatusRemark = Convert.ToString(tblLoadingStatusHistoryTODT["statusRemark"].ToString());
                    if (tblLoadingStatusHistoryTODT["statusReasonId"] != DBNull.Value)
                        tblLoadingStatusHistoryTONew.StatusReasonId = Convert.ToInt32(tblLoadingStatusHistoryTODT["statusReasonId"].ToString());
                    tblLoadingStatusHistoryTOList.Add(tblLoadingStatusHistoryTONew);
                }
            }
            return tblLoadingStatusHistoryTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblLoadingStatusHistory(TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblLoadingStatusHistoryTO, cmdInsert);
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

        public static int InsertTblLoadingStatusHistory(TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblLoadingStatusHistoryTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tempLoadingStatusHistory]( " +
                                "  [loadingId]" +
                                " ,[statusId]" +
                                " ,[createdBy]" +
                                " ,[statusDate]" +
                                " ,[createdOn]" +
                                " ,[statusRemark]" +
                                " ,[statusReasonId]" +
                                " )" +
                    " VALUES (" +
                                "  @LoadingId " +
                                " ,@StatusId " +
                                " ,@CreatedBy " +
                                " ,@StatusDate " +
                                " ,@CreatedOn " +
                                " ,@StatusRemark " +
                                " ,@statusReasonId " +
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdLoadingHistory", System.Data.SqlDbType.Int).Value = tblLoadingStatusHistoryTO.IdLoadingHistory;
            cmdInsert.Parameters.Add("@LoadingId", System.Data.SqlDbType.Int).Value = tblLoadingStatusHistoryTO.LoadingId;
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblLoadingStatusHistoryTO.StatusId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLoadingStatusHistoryTO.CreatedBy;
            cmdInsert.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblLoadingStatusHistoryTO.StatusDate;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingStatusHistoryTO.CreatedOn;
            cmdInsert.Parameters.Add("@StatusRemark", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingStatusHistoryTO.StatusRemark);
            cmdInsert.Parameters.Add("@statusReasonId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingStatusHistoryTO.StatusReasonId);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblLoadingStatusHistoryTO.IdLoadingHistory = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblLoadingStatusHistory(TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblLoadingStatusHistoryTO, cmdUpdate);
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

        public static int UpdateTblLoadingStatusHistory(TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblLoadingStatusHistoryTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tempLoadingStatusHistory] SET " + 
            "  [idLoadingHistory] = @IdLoadingHistory" +
            " ,[loadingId]= @LoadingId" +
            " ,[statusId]= @StatusId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[statusDate]= @StatusDate" +
            " ,[createdOn]= @CreatedOn" +
            " ,[statusRemark] = @StatusRemark" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdLoadingHistory", System.Data.SqlDbType.Int).Value = tblLoadingStatusHistoryTO.IdLoadingHistory;
            cmdUpdate.Parameters.Add("@LoadingId", System.Data.SqlDbType.Int).Value = tblLoadingStatusHistoryTO.LoadingId;
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblLoadingStatusHistoryTO.StatusId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLoadingStatusHistoryTO.CreatedBy;
            cmdUpdate.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblLoadingStatusHistoryTO.StatusDate;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingStatusHistoryTO.CreatedOn;
            cmdUpdate.Parameters.Add("@StatusRemark", System.Data.SqlDbType.VarChar).Value = tblLoadingStatusHistoryTO.StatusRemark;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblLoadingStatusHistory(Int32 idLoadingHistory)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idLoadingHistory, cmdDelete);
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

        public static int DeleteTblLoadingStatusHistory(Int32 idLoadingHistory, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idLoadingHistory, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idLoadingHistory, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tempLoadingStatusHistory] " +
            " WHERE idLoadingHistory = " + idLoadingHistory +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idLoadingHistory", System.Data.SqlDbType.Int).Value = tblLoadingStatusHistoryTO.IdLoadingHistory;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
