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
    public class TblLoadingQuotaConsumptionDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblLoadingQuotaConsumption]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblLoadingQuotaConsumptionTO> SelectAllTblLoadingQuotaConsumption()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingQuotaConsumptionTO> list = ConvertDTToList(reader);
                 return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblLoadingQuotaConsumptionTO SelectTblLoadingQuotaConsumption()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ "  ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingQuotaConsumptionTO> list = ConvertDTToList(reader);
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
                reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblLoadingQuotaConsumptionTO> ConvertDTToList(SqlDataReader tblLoadingQuotaConsumptionTODT)
        {
            List<TblLoadingQuotaConsumptionTO> tblLoadingQuotaConsumptionTOList = new List<TblLoadingQuotaConsumptionTO>();
            if (tblLoadingQuotaConsumptionTODT != null)
            {
                while (tblLoadingQuotaConsumptionTODT.Read())
                {
                    TblLoadingQuotaConsumptionTO tblLoadingQuotaConsumptionTONew = new TblLoadingQuotaConsumptionTO();
                    if (tblLoadingQuotaConsumptionTODT["idLoadQuotaConsum"] != DBNull.Value)
                        tblLoadingQuotaConsumptionTONew.IdLoadQuotaConsum = Convert.ToInt32(tblLoadingQuotaConsumptionTODT["idLoadQuotaConsum"].ToString());
                    if (tblLoadingQuotaConsumptionTODT["loadingQuotaId"] != DBNull.Value)
                        tblLoadingQuotaConsumptionTONew.LoadingQuotaId = Convert.ToInt32(tblLoadingQuotaConsumptionTODT["loadingQuotaId"].ToString());
                    if (tblLoadingQuotaConsumptionTODT["loadingSlipExtId"] != DBNull.Value)
                        tblLoadingQuotaConsumptionTONew.LoadingSlipExtId = Convert.ToInt32(tblLoadingQuotaConsumptionTODT["loadingSlipExtId"].ToString());
                    if (tblLoadingQuotaConsumptionTODT["transferNoteId"] != DBNull.Value)
                        tblLoadingQuotaConsumptionTONew.TransferNoteId = Convert.ToInt32(tblLoadingQuotaConsumptionTODT["transferNoteId"].ToString());
                    if (tblLoadingQuotaConsumptionTODT["txnOpTypeId"] != DBNull.Value)
                        tblLoadingQuotaConsumptionTONew.TxnOpTypeId = Convert.ToInt32(tblLoadingQuotaConsumptionTODT["txnOpTypeId"].ToString());
                    if (tblLoadingQuotaConsumptionTODT["createdBy"] != DBNull.Value)
                        tblLoadingQuotaConsumptionTONew.CreatedBy = Convert.ToInt32(tblLoadingQuotaConsumptionTODT["createdBy"].ToString());
                    if (tblLoadingQuotaConsumptionTODT["createdOn"] != DBNull.Value)
                        tblLoadingQuotaConsumptionTONew.CreatedOn = Convert.ToDateTime(tblLoadingQuotaConsumptionTODT["createdOn"].ToString());
                    if (tblLoadingQuotaConsumptionTODT["availableQuota"] != DBNull.Value)
                        tblLoadingQuotaConsumptionTONew.AvailableQuota = Convert.ToDouble(tblLoadingQuotaConsumptionTODT["availableQuota"].ToString());
                    if (tblLoadingQuotaConsumptionTODT["balanceQuota"] != DBNull.Value)
                        tblLoadingQuotaConsumptionTONew.BalanceQuota = Convert.ToDouble(tblLoadingQuotaConsumptionTODT["balanceQuota"].ToString());
                    if (tblLoadingQuotaConsumptionTODT["quotaQty"] != DBNull.Value)
                        tblLoadingQuotaConsumptionTONew.QuotaQty = Convert.ToDouble(tblLoadingQuotaConsumptionTODT["quotaQty"].ToString());
                    if (tblLoadingQuotaConsumptionTODT["remark"] != DBNull.Value)
                        tblLoadingQuotaConsumptionTONew.Remark = Convert.ToString(tblLoadingQuotaConsumptionTODT["remark"].ToString());
                    tblLoadingQuotaConsumptionTOList.Add(tblLoadingQuotaConsumptionTONew);
                }
            }
            return tblLoadingQuotaConsumptionTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblLoadingQuotaConsumption(TblLoadingQuotaConsumptionTO tblLoadingQuotaConsumptionTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblLoadingQuotaConsumptionTO, cmdInsert);
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

        public static int InsertTblLoadingQuotaConsumption(TblLoadingQuotaConsumptionTO tblLoadingQuotaConsumptionTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblLoadingQuotaConsumptionTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblLoadingQuotaConsumptionTO tblLoadingQuotaConsumptionTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblLoadingQuotaConsumption]( " +
                                "  [loadingQuotaId]" +
                                " ,[loadingSlipExtId]" +
                                " ,[transferNoteId]" +
                                " ,[txnOpTypeId]" +
                                " ,[createdBy]" +
                                " ,[createdOn]" +
                                " ,[availableQuota]" +
                                " ,[balanceQuota]" +
                                " ,[quotaQty]" +
                                " ,[remark]" +
                                " )" +
                    " VALUES (" +
                                "  @LoadingQuotaId " +
                                " ,@LoadingSlipExtId " +
                                " ,@TransferNoteId " +
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

            //cmdInsert.Parameters.Add("@IdLoadQuotaConsum", System.Data.SqlDbType.Int).Value = tblLoadingQuotaConsumptionTO.IdLoadQuotaConsum;
            cmdInsert.Parameters.Add("@LoadingQuotaId", System.Data.SqlDbType.Int).Value = tblLoadingQuotaConsumptionTO.LoadingQuotaId;
            cmdInsert.Parameters.Add("@LoadingSlipExtId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingQuotaConsumptionTO.LoadingSlipExtId);
            cmdInsert.Parameters.Add("@TransferNoteId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingQuotaConsumptionTO.TransferNoteId);
            cmdInsert.Parameters.Add("@TxnOpTypeId", System.Data.SqlDbType.Int).Value = tblLoadingQuotaConsumptionTO.TxnOpTypeId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLoadingQuotaConsumptionTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingQuotaConsumptionTO.CreatedOn;
            cmdInsert.Parameters.Add("@AvailableQuota", System.Data.SqlDbType.NVarChar).Value = tblLoadingQuotaConsumptionTO.AvailableQuota;
            cmdInsert.Parameters.Add("@BalanceQuota", System.Data.SqlDbType.NVarChar).Value = tblLoadingQuotaConsumptionTO.BalanceQuota;
            cmdInsert.Parameters.Add("@QuotaQty", System.Data.SqlDbType.NVarChar).Value = tblLoadingQuotaConsumptionTO.QuotaQty;
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = tblLoadingQuotaConsumptionTO.Remark;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblLoadingQuotaConsumptionTO.IdLoadQuotaConsum = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblLoadingQuotaConsumption(TblLoadingQuotaConsumptionTO tblLoadingQuotaConsumptionTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblLoadingQuotaConsumptionTO, cmdUpdate);
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

        public static int UpdateTblLoadingQuotaConsumption(TblLoadingQuotaConsumptionTO tblLoadingQuotaConsumptionTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblLoadingQuotaConsumptionTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblLoadingQuotaConsumptionTO tblLoadingQuotaConsumptionTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblLoadingQuotaConsumption] SET " + 
            "  [idLoadQuotaConsum] = @IdLoadQuotaConsum" +
            " ,[loadingQuotaId]= @LoadingQuotaId" +
            " ,[loadingSlipExtId]= @LoadingSlipExtId" +
            " ,[transferNoteId]= @TransferNoteId" +
            " ,[txnOpTypeId]= @TxnOpTypeId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[availableQuota]= @AvailableQuota" +
            " ,[balanceQuota]= @BalanceQuota" +
            " ,[quotaQty]= @QuotaQty" +
            " ,[remark] = @Remark" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdLoadQuotaConsum", System.Data.SqlDbType.Int).Value = tblLoadingQuotaConsumptionTO.IdLoadQuotaConsum;
            cmdUpdate.Parameters.Add("@LoadingQuotaId", System.Data.SqlDbType.Int).Value = tblLoadingQuotaConsumptionTO.LoadingQuotaId;
            cmdUpdate.Parameters.Add("@LoadingSlipExtId", System.Data.SqlDbType.Int).Value = tblLoadingQuotaConsumptionTO.LoadingSlipExtId;
            cmdUpdate.Parameters.Add("@TransferNoteId", System.Data.SqlDbType.Int).Value = tblLoadingQuotaConsumptionTO.TransferNoteId;
            cmdUpdate.Parameters.Add("@TxnOpTypeId", System.Data.SqlDbType.Int).Value = tblLoadingQuotaConsumptionTO.TxnOpTypeId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLoadingQuotaConsumptionTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingQuotaConsumptionTO.CreatedOn;
            cmdUpdate.Parameters.Add("@AvailableQuota", System.Data.SqlDbType.NVarChar).Value = tblLoadingQuotaConsumptionTO.AvailableQuota;
            cmdUpdate.Parameters.Add("@BalanceQuota", System.Data.SqlDbType.NVarChar).Value = tblLoadingQuotaConsumptionTO.BalanceQuota;
            cmdUpdate.Parameters.Add("@QuotaQty", System.Data.SqlDbType.NVarChar).Value = tblLoadingQuotaConsumptionTO.QuotaQty;
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = tblLoadingQuotaConsumptionTO.Remark;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblLoadingQuotaConsumption()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(cmdDelete);
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

        public static int DeleteTblLoadingQuotaConsumption(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(cmdDelete);
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

        public static int ExecuteDeletionCommand(SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblLoadingQuotaConsumption] " +
            " ";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
