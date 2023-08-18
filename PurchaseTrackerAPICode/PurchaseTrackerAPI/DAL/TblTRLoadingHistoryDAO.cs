using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces; 

namespace PurchaseTrackerAPI.DAL
{
    public class TblTRLoadingHistoryDAO : ITblTRLoadingHistoryDAO
    {
        private readonly IConnectionString _iConnectionString;

        public TblTRLoadingHistoryDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblTRLoadingHistory]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  DataTable SelectAllTblTRLoadingHistory()
        {
              String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idLoadingHistory", System.Data.SqlDbType.Int).Value = tblTRLoadingHistoryTO.IdLoadingHistory;
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
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

        public  DataTable SelectTblTRLoadingHistory(Int32 idLoadingHistory)
        {
              String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idLoadingHistory = " + idLoadingHistory +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idLoadingHistory", System.Data.SqlDbType.Int).Value = tblTRLoadingHistoryTO.IdLoadingHistory;
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
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

        public  DataTable SelectAllTblTRLoadingHistory(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idLoadingHistory", System.Data.SqlDbType.Int).Value = tblTRLoadingHistoryTO.IdLoadingHistory;
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

        #endregion
        
        #region Insertion
        public  int InsertTblTRLoadingHistory(TblTRLoadingHistoryTO tblTRLoadingHistoryTO)
        {
              String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblTRLoadingHistoryTO, cmdInsert);
            }
            catch(Exception ex)
            {
                
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public  int InsertTblTRLoadingHistory(TblTRLoadingHistoryTO tblTRLoadingHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblTRLoadingHistoryTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public  int ExecuteInsertionCommand(TblTRLoadingHistoryTO tblTRLoadingHistoryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblTRLoadingHistory]( " + 
            //"  [idLoadingHistory]" +
            " [loadingId]" +
            " ,[statusId]" +
            " ,[statusBy]" +
            " ,[statusOn]" +
            " )" +
" VALUES (" +
            //"  @IdLoadingHistory " +
            " @LoadingId " +
            " ,@StatusId " +
            " ,@StatusBy " +
            " ,@StatusOn " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdLoadingHistory", System.Data.SqlDbType.Int).Value = tblTRLoadingHistoryTO.IdLoadingHistory;
            cmdInsert.Parameters.Add("@LoadingId", System.Data.SqlDbType.Int).Value = tblTRLoadingHistoryTO.LoadingId;
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblTRLoadingHistoryTO.StatusId;
            cmdInsert.Parameters.Add("@StatusBy", System.Data.SqlDbType.Int).Value = tblTRLoadingHistoryTO.StatusBy;
            cmdInsert.Parameters.Add("@StatusOn", System.Data.SqlDbType.DateTime).Value = tblTRLoadingHistoryTO.StatusOn;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblTRLoadingHistory(TblTRLoadingHistoryTO tblTRLoadingHistoryTO)
        {
              String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblTRLoadingHistoryTO, cmdUpdate);
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

        public  int UpdateTblTRLoadingHistory(TblTRLoadingHistoryTO tblTRLoadingHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblTRLoadingHistoryTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblTRLoadingHistoryTO tblTRLoadingHistoryTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblTRLoadingHistory] SET " + 
            "  [idLoadingHistory] = @IdLoadingHistory" +
            " ,[loadingId]= @LoadingId" +
            " ,[statusId]= @StatusId" +
            " ,[statusBy]= @StatusBy" +
            " ,[statusOn] = @StatusOn" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdLoadingHistory", System.Data.SqlDbType.Int).Value = tblTRLoadingHistoryTO.IdLoadingHistory;
            cmdUpdate.Parameters.Add("@LoadingId", System.Data.SqlDbType.Int).Value = tblTRLoadingHistoryTO.LoadingId;
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblTRLoadingHistoryTO.StatusId;
            cmdUpdate.Parameters.Add("@StatusBy", System.Data.SqlDbType.Int).Value = tblTRLoadingHistoryTO.StatusBy;
            cmdUpdate.Parameters.Add("@StatusOn", System.Data.SqlDbType.DateTime).Value = tblTRLoadingHistoryTO.StatusOn;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblTRLoadingHistory(Int32 idLoadingHistory)
        {
              String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
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

        public  int DeleteTblTRLoadingHistory(Int32 idLoadingHistory, SqlConnection conn, SqlTransaction tran)
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

        public  int ExecuteDeletionCommand(Int32 idLoadingHistory, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblTRLoadingHistory] " +
            " WHERE idLoadingHistory = " + idLoadingHistory +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idLoadingHistory", System.Data.SqlDbType.Int).Value = tblTRLoadingHistoryTO.IdLoadingHistory;
            return cmdDelete.ExecuteNonQuery();
        }

       
        #endregion

    }
}
