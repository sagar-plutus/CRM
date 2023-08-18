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
    public class TblQuotaConsumHistoryDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblQuotaConsumHistory]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblQuotaConsumHistoryTO> SelectAllTblQuotaConsumHistory()
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

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblQuotaConsumHistoryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                return list;
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

        public static TblQuotaConsumHistoryTO SelectTblQuotaConsumHistory(Int32 idQuotaConsmHIstory)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idQuotaConsmHIstory = " + idQuotaConsmHIstory +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblQuotaConsumHistoryTO> list = ConvertDTToList(reader);
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

        public static List<TblQuotaConsumHistoryTO> ConvertDTToList(SqlDataReader tblQuotaConsumHistoryTODT)
        {
            List<TblQuotaConsumHistoryTO> tblQuotaConsumHistoryTOList = new List<TblQuotaConsumHistoryTO>();
            if (tblQuotaConsumHistoryTODT != null)
            {
               while(tblQuotaConsumHistoryTODT.Read())
                {
                    TblQuotaConsumHistoryTO tblQuotaConsumHistoryTONew = new TblQuotaConsumHistoryTO();
                    if (tblQuotaConsumHistoryTODT["idQuotaConsmHIstory"] != DBNull.Value)
                        tblQuotaConsumHistoryTONew.IdQuotaConsmHIstory = Convert.ToInt32(tblQuotaConsumHistoryTODT["idQuotaConsmHIstory"].ToString());
                    if (tblQuotaConsumHistoryTODT["quotaDeclarationId"] != DBNull.Value)
                        tblQuotaConsumHistoryTONew.QuotaDeclarationId = Convert.ToInt32(tblQuotaConsumHistoryTODT["quotaDeclarationId"].ToString());
                    if (tblQuotaConsumHistoryTODT["bookingId"] != DBNull.Value)
                        tblQuotaConsumHistoryTONew.BookingId = Convert.ToInt32(tblQuotaConsumHistoryTODT["bookingId"].ToString());
                    if (tblQuotaConsumHistoryTODT["txnOpTypeId"] != DBNull.Value)
                        tblQuotaConsumHistoryTONew.TxnOpTypeId = Convert.ToInt32(tblQuotaConsumHistoryTODT["txnOpTypeId"].ToString());
                    if (tblQuotaConsumHistoryTODT["createdBy"] != DBNull.Value)
                        tblQuotaConsumHistoryTONew.CreatedBy = Convert.ToInt32(tblQuotaConsumHistoryTODT["createdBy"].ToString());
                    if (tblQuotaConsumHistoryTODT["createdOn"] != DBNull.Value)
                        tblQuotaConsumHistoryTONew.CreatedOn = Convert.ToDateTime(tblQuotaConsumHistoryTODT["createdOn"].ToString());
                    if (tblQuotaConsumHistoryTODT["availableQuota"] != DBNull.Value)
                        tblQuotaConsumHistoryTONew.AvailableQuota = Convert.ToDouble(tblQuotaConsumHistoryTODT["availableQuota"].ToString());
                    if (tblQuotaConsumHistoryTODT["balanceQuota"] != DBNull.Value)
                        tblQuotaConsumHistoryTONew.BalanceQuota = Convert.ToDouble(tblQuotaConsumHistoryTODT["balanceQuota"].ToString());
                    if (tblQuotaConsumHistoryTODT["quotaQty"] != DBNull.Value)
                        tblQuotaConsumHistoryTONew.QuotaQty = Convert.ToDouble(tblQuotaConsumHistoryTODT["quotaQty"].ToString());
                    tblQuotaConsumHistoryTOList.Add(tblQuotaConsumHistoryTONew);
                }
            }
            return tblQuotaConsumHistoryTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblQuotaConsumHistory(TblQuotaConsumHistoryTO tblQuotaConsumHistoryTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblQuotaConsumHistoryTO, cmdInsert);
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

        public static int InsertTblQuotaConsumHistory(TblQuotaConsumHistoryTO tblQuotaConsumHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblQuotaConsumHistoryTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblQuotaConsumHistoryTO tblQuotaConsumHistoryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblQuotaConsumHistory]( " +
                                "  [quotaDeclarationId]" +
                                " ,[bookingId]" +
                                " ,[txnOpTypeId]" +
                                " ,[createdBy]" +
                                " ,[createdOn]" +
                                " ,[availableQuota]" +
                                " ,[balanceQuota]" +
                                " ,[quotaQty]" +
                                " ,[remark]" +
                                " )" +
                    " VALUES (" +
                                "  @QuotaDeclarationId " +
                                " ,@BookingId " +
                                " ,@TxnOpTypeId " +
                                " ,@CreatedBy " +
                                " ,@CreatedOn " +
                                " ,@AvailableQuota " +
                                " ,@BalanceQuota " +
                                " ,@QuotaQty " +
                                " ,@Remark " +
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdQuotaConsmHIstory", System.Data.SqlDbType.Int).Value = tblQuotaConsumHistoryTO.IdQuotaConsmHIstory;
            cmdInsert.Parameters.Add("@QuotaDeclarationId", System.Data.SqlDbType.Int).Value = tblQuotaConsumHistoryTO.QuotaDeclarationId;
            cmdInsert.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = tblQuotaConsumHistoryTO.BookingId;
            cmdInsert.Parameters.Add("@TxnOpTypeId", System.Data.SqlDbType.Int).Value = tblQuotaConsumHistoryTO.TxnOpTypeId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblQuotaConsumHistoryTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblQuotaConsumHistoryTO.CreatedOn;
            cmdInsert.Parameters.Add("@AvailableQuota", System.Data.SqlDbType.NVarChar).Value = tblQuotaConsumHistoryTO.AvailableQuota;
            cmdInsert.Parameters.Add("@BalanceQuota", System.Data.SqlDbType.NVarChar).Value = tblQuotaConsumHistoryTO.BalanceQuota;
            cmdInsert.Parameters.Add("@QuotaQty", System.Data.SqlDbType.NVarChar).Value = tblQuotaConsumHistoryTO.QuotaQty;
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar,100).Value = Constants.GetSqlDataValueNullForBaseValue(tblQuotaConsumHistoryTO.Remark);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblQuotaConsumHistoryTO.IdQuotaConsmHIstory = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblQuotaConsumHistory(TblQuotaConsumHistoryTO tblQuotaConsumHistoryTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblQuotaConsumHistoryTO, cmdUpdate);
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

        public static int UpdateTblQuotaConsumHistory(TblQuotaConsumHistoryTO tblQuotaConsumHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblQuotaConsumHistoryTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblQuotaConsumHistoryTO tblQuotaConsumHistoryTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblQuotaConsumHistory] SET " + 
            "  [idQuotaConsmHIstory] = @IdQuotaConsmHIstory" +
            " ,[quotaDeclarationId]= @QuotaDeclarationId" +
            " ,[bookingId]= @BookingId" +
            " ,[txnOpTypeId]= @TxnOpTypeId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[availableQuota]= @AvailableQuota" +
            " ,[balanceQuota]= @BalanceQuota" +
            " ,[quotaQty] = @QuotaQty" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdQuotaConsmHIstory", System.Data.SqlDbType.Int).Value = tblQuotaConsumHistoryTO.IdQuotaConsmHIstory;
            cmdUpdate.Parameters.Add("@QuotaDeclarationId", System.Data.SqlDbType.Int).Value = tblQuotaConsumHistoryTO.QuotaDeclarationId;
            cmdUpdate.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = tblQuotaConsumHistoryTO.BookingId;
            cmdUpdate.Parameters.Add("@TxnOpTypeId", System.Data.SqlDbType.Int).Value = tblQuotaConsumHistoryTO.TxnOpTypeId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblQuotaConsumHistoryTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblQuotaConsumHistoryTO.CreatedOn;
            cmdUpdate.Parameters.Add("@AvailableQuota", System.Data.SqlDbType.NVarChar).Value = tblQuotaConsumHistoryTO.AvailableQuota;
            cmdUpdate.Parameters.Add("@BalanceQuota", System.Data.SqlDbType.NVarChar).Value = tblQuotaConsumHistoryTO.BalanceQuota;
            cmdUpdate.Parameters.Add("@QuotaQty", System.Data.SqlDbType.NVarChar).Value = tblQuotaConsumHistoryTO.QuotaQty;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblQuotaConsumHistory(Int32 idQuotaConsmHIstory)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idQuotaConsmHIstory, cmdDelete);
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

        public static int DeleteTblQuotaConsumHistory(Int32 idQuotaConsmHIstory, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idQuotaConsmHIstory, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idQuotaConsmHIstory, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblQuotaConsumHistory] " +
            " WHERE idQuotaConsmHIstory = " + idQuotaConsmHIstory +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idQuotaConsmHIstory", System.Data.SqlDbType.Int).Value = tblQuotaConsumHistoryTO.IdQuotaConsmHIstory;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
