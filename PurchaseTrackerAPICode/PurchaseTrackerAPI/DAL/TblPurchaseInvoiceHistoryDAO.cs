using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblPurchaseInvoiceHistoryDAO : ITblPurchaseInvoiceHistoryDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPurchaseInvoiceHistoryDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblPurchaseInvoiceHistory]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblPurchaseInvoiceHistoryTO> SelectAllTblPurchaseInvoiceHistory()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseInvHistory", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceHistoryTO.IdPurchaseInvHistory;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceHistoryTO> list = ConvertDTToList(reader);
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

        public  TblPurchaseInvoiceHistoryTO SelectTblPurchaseInvoiceHistory(Int64 idPurchaseInvHistory)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idPurchaseInvHistory = " + idPurchaseInvHistory +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseInvHistory", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceHistoryTO.IdPurchaseInvHistory;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceHistoryTO> list = ConvertDTToList(reader);
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

        public  List<TblPurchaseInvoiceHistoryTO> SelectAllTblPurchaseInvoiceHistory(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseInvHistory", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceHistoryTO.IdPurchaseInvHistory;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceHistoryTO> list = ConvertDTToList(reader);
                return list;
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
        public  List<TblPurchaseInvoiceHistoryTO> ConvertDTToList(SqlDataReader tblPurchaseInvoiceHistoryTODT)
        {
            List<TblPurchaseInvoiceHistoryTO> tblPurchaseInvoiceHistoryTOList = new List<TblPurchaseInvoiceHistoryTO>();
            if (tblPurchaseInvoiceHistoryTODT != null)
            {
                while (tblPurchaseInvoiceHistoryTODT.Read())
                {
                    TblPurchaseInvoiceHistoryTO tblPurchaseInvoiceHistoryTONew = new TblPurchaseInvoiceHistoryTO();
                    if (tblPurchaseInvoiceHistoryTODT["statusId"] != DBNull.Value)
                        tblPurchaseInvoiceHistoryTONew.StatusId = Convert.ToInt32(tblPurchaseInvoiceHistoryTODT["statusId"].ToString());
                    if (tblPurchaseInvoiceHistoryTODT["createdBy"] != DBNull.Value)
                        tblPurchaseInvoiceHistoryTONew.CreatedBy = Convert.ToInt32(tblPurchaseInvoiceHistoryTODT["createdBy"].ToString());
                    if (tblPurchaseInvoiceHistoryTODT["statusDate"] != DBNull.Value)
                        tblPurchaseInvoiceHistoryTONew.StatusDate = Convert.ToDateTime(tblPurchaseInvoiceHistoryTODT["statusDate"].ToString());
                    if (tblPurchaseInvoiceHistoryTODT["createdOn"] != DBNull.Value)
                        tblPurchaseInvoiceHistoryTONew.CreatedOn = Convert.ToDateTime(tblPurchaseInvoiceHistoryTODT["createdOn"].ToString());
                    if (tblPurchaseInvoiceHistoryTODT["idPurchaseInvHistory"] != DBNull.Value)
                        tblPurchaseInvoiceHistoryTONew.IdPurchaseInvHistory = Convert.ToInt64(tblPurchaseInvoiceHistoryTODT["idPurchaseInvHistory"].ToString());
                    if (tblPurchaseInvoiceHistoryTODT["purchaseInvoiceId"] != DBNull.Value)
                        tblPurchaseInvoiceHistoryTONew.PurchaseInvoiceId = Convert.ToInt64(tblPurchaseInvoiceHistoryTODT["purchaseInvoiceId"].ToString());
                    if (tblPurchaseInvoiceHistoryTODT["statusRemark"] != DBNull.Value)
                        tblPurchaseInvoiceHistoryTONew.StatusRemark = Convert.ToString(tblPurchaseInvoiceHistoryTODT["statusRemark"].ToString());
                    tblPurchaseInvoiceHistoryTOList.Add(tblPurchaseInvoiceHistoryTONew);
                }
            }
            return tblPurchaseInvoiceHistoryTOList;
        }

        #endregion

        #region Insertion
        public  int InsertTblPurchaseInvoiceHistory(TblPurchaseInvoiceHistoryTO tblPurchaseInvoiceHistoryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPurchaseInvoiceHistoryTO, cmdInsert);
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

        public  int InsertTblPurchaseInvoiceHistory(TblPurchaseInvoiceHistoryTO tblPurchaseInvoiceHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPurchaseInvoiceHistoryTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblPurchaseInvoiceHistoryTO tblPurchaseInvoiceHistoryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPurchaseInvoiceHistory]( " + 
            "  [statusId]" +
            " ,[createdBy]" +
            " ,[statusDate]" +
            " ,[createdOn]" +
            //" ,[idPurchaseInvHistory]" +
            " ,[purchaseInvoiceId]" +
            " ,[statusRemark]" +
            " )" +
" VALUES (" +
            "  @StatusId " +
            " ,@CreatedBy " +
            " ,@StatusDate " +
            " ,@CreatedOn " +
            //" ,@IdPurchaseInvHistory " +
            " ,@PurchaseInvoiceId " +
            " ,@StatusRemark " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceHistoryTO.StatusId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceHistoryTO.CreatedBy;
            cmdInsert.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblPurchaseInvoiceHistoryTO.StatusDate;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseInvoiceHistoryTO.CreatedOn;
            //cmdInsert.Parameters.Add("@IdPurchaseInvHistory", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceHistoryTO.IdPurchaseInvHistory;
            cmdInsert.Parameters.Add("@PurchaseInvoiceId", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceHistoryTO.PurchaseInvoiceId;
            cmdInsert.Parameters.Add("@StatusRemark", System.Data.SqlDbType.NVarChar).Value = PurchaseTrackerAPI.StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceHistoryTO.StatusRemark);
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblPurchaseInvoiceHistory(TblPurchaseInvoiceHistoryTO tblPurchaseInvoiceHistoryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPurchaseInvoiceHistoryTO, cmdUpdate);
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

        public  int UpdateTblPurchaseInvoiceHistory(TblPurchaseInvoiceHistoryTO tblPurchaseInvoiceHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPurchaseInvoiceHistoryTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblPurchaseInvoiceHistoryTO tblPurchaseInvoiceHistoryTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseInvoiceHistory] SET " + 
            "  [statusId] = @StatusId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[statusDate]= @StatusDate" +
            " ,[createdOn]= @CreatedOn" +
           // " ,[idPurchaseInvHistory]= @IdPurchaseInvHistory" +
            " ,[purchaseInvoiceId]= @PurchaseInvoiceId" +
            " ,[statusRemark] = @StatusRemark" +
            " WHERE [idPurchaseInvHistory]= @IdPurchaseInvHistory "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceHistoryTO.StatusId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceHistoryTO.CreatedBy;
            cmdUpdate.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblPurchaseInvoiceHistoryTO.StatusDate;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseInvoiceHistoryTO.CreatedOn;
            cmdUpdate.Parameters.Add("@IdPurchaseInvHistory", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceHistoryTO.IdPurchaseInvHistory;
            cmdUpdate.Parameters.Add("@PurchaseInvoiceId", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceHistoryTO.PurchaseInvoiceId;
            cmdUpdate.Parameters.Add("@StatusRemark", System.Data.SqlDbType.NVarChar).Value = tblPurchaseInvoiceHistoryTO.StatusRemark;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblPurchaseInvoiceHistory(Int64 idPurchaseInvHistory)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPurchaseInvHistory, cmdDelete);
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

        public  int DeleteTblPurchaseInvoiceHistory(Int64 idPurchaseInvHistory, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPurchaseInvHistory, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int64 idPurchaseInvHistory, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPurchaseInvoiceHistory] " +
            " WHERE idPurchaseInvHistory = " + idPurchaseInvHistory +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPurchaseInvHistory", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceHistoryTO.IdPurchaseInvHistory;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
