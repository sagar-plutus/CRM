using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.DAL
{
    public class TblPurchaseSchStatusHistoryDAO : ITblPurchaseSchStatusHistoryDAO
    {

        private readonly IConnectionString _iConnectionString;

        public TblPurchaseSchStatusHistoryDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblPurchaseSchStatusHistory]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblPurchaseSchStatusHistoryTO> SelectAllTblPurchaseSchStatusHistory()
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

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseSchStatusHistoryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
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

        public List<TblPurchaseSchStatusHistoryTO> SelectTblPurchaseSchStatusHistory(Int32 idPurchaseSchStatusHistory)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idPurchaseSchStatusHistory = " + idPurchaseSchStatusHistory +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseSchStatusHistoryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
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

        public List<TblPurchaseSchStatusHistoryTO> SelectAllTblPurchaseSchStatusHistory(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseSchStatusHistoryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
                else return null;
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

        public List<TblPurchaseSchStatusHistoryTO> ConvertDTToList(SqlDataReader tblPurchaseSchStatusHistoryTODT)
        {
            List<TblPurchaseSchStatusHistoryTO> tblPurchaseSchStatusHistoryTOList = new List<TblPurchaseSchStatusHistoryTO>();
            if (tblPurchaseSchStatusHistoryTODT != null)
            {
                while(tblPurchaseSchStatusHistoryTODT.Read())
                {
                    TblPurchaseSchStatusHistoryTO tblPurchaseSchStatusHistoryTONew = new TblPurchaseSchStatusHistoryTO();
                    if (tblPurchaseSchStatusHistoryTODT ["idPurchaseSchStatusHistory"] != DBNull.Value)
                        tblPurchaseSchStatusHistoryTONew.IdPurchaseSchStatusHistory = Convert.ToInt32(tblPurchaseSchStatusHistoryTODT ["idPurchaseSchStatusHistory"].ToString());
                    if (tblPurchaseSchStatusHistoryTODT ["statusId"] != DBNull.Value)
                        tblPurchaseSchStatusHistoryTONew.StatusId = Convert.ToInt32(tblPurchaseSchStatusHistoryTODT ["statusId"].ToString());
                    if (tblPurchaseSchStatusHistoryTODT ["vehiclePhaseId"] != DBNull.Value)
                        tblPurchaseSchStatusHistoryTONew.VehiclePhaseId = Convert.ToInt32(tblPurchaseSchStatusHistoryTODT ["vehiclePhaseId"].ToString());
                    if (tblPurchaseSchStatusHistoryTODT ["createdBy"] != DBNull.Value)
                        tblPurchaseSchStatusHistoryTONew.CreatedBy = Convert.ToInt32(tblPurchaseSchStatusHistoryTODT ["createdBy"].ToString());
                    if (tblPurchaseSchStatusHistoryTODT ["purchaseScheduleSummaryId"] != DBNull.Value)
                        tblPurchaseSchStatusHistoryTONew.PurchaseScheduleSummaryId = Convert.ToInt32(tblPurchaseSchStatusHistoryTODT ["purchaseScheduleSummaryId"].ToString());
                    if (tblPurchaseSchStatusHistoryTODT ["createdOn"] != DBNull.Value)
                        tblPurchaseSchStatusHistoryTONew.CreatedOn = Convert.ToDateTime(tblPurchaseSchStatusHistoryTODT ["createdOn"].ToString());
                    if (tblPurchaseSchStatusHistoryTODT ["comment"] != DBNull.Value)
                        tblPurchaseSchStatusHistoryTONew.Comment = Convert.ToString(tblPurchaseSchStatusHistoryTODT ["comment"].ToString());
                    tblPurchaseSchStatusHistoryTOList.Add(tblPurchaseSchStatusHistoryTONew);
                }
            }
            return tblPurchaseSchStatusHistoryTOList;
        }


        #endregion

        #region Insertion
        public int InsertTblPurchaseSchStatusHistory(TblPurchaseSchStatusHistoryTO tblPurchaseSchStatusHistoryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPurchaseSchStatusHistoryTO, cmdInsert);
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

        public int InsertTblPurchaseSchStatusHistory(TblPurchaseSchStatusHistoryTO tblPurchaseSchStatusHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPurchaseSchStatusHistoryTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblPurchaseSchStatusHistoryTO tblPurchaseSchStatusHistoryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPurchaseSchStatusHistory]( " + 
            //"  [idPurchaseSchStatusHistory]" +
            "  [statusId]" +
            " ,[vehiclePhaseId]" +
            " ,[createdBy]" +
            " ,[purchaseScheduleSummaryId]" +
            " ,[createdOn]" +
            " ,[comment]" +
            " )" +
" VALUES (" +
            //"  @IdPurchaseSchStatusHistory " +
            "  @StatusId " +
            " ,@VehiclePhaseId " +
            " ,@CreatedBy " +
            " ,@PurchaseScheduleSummaryId " +
            " ,@CreatedOn " +
            " ,@Comment " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdPurchaseSchStatusHistory", System.Data.SqlDbType.Int).Value = tblPurchaseSchStatusHistoryTO.IdPurchaseSchStatusHistory;
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseSchStatusHistoryTO.StatusId);
            cmdInsert.Parameters.Add("@VehiclePhaseId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseSchStatusHistoryTO.VehiclePhaseId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseSchStatusHistoryTO.CreatedBy;
            cmdInsert.Parameters.Add("@PurchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseSchStatusHistoryTO.PurchaseScheduleSummaryId);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseSchStatusHistoryTO.CreatedOn);
            cmdInsert.Parameters.Add("@Comment", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseSchStatusHistoryTO.Comment);
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public int UpdateTblPurchaseSchStatusHistory(TblPurchaseSchStatusHistoryTO tblPurchaseSchStatusHistoryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPurchaseSchStatusHistoryTO, cmdUpdate);
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

        public int UpdateTblPurchaseSchStatusHistory(TblPurchaseSchStatusHistoryTO tblPurchaseSchStatusHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPurchaseSchStatusHistoryTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblPurchaseSchStatusHistoryTO tblPurchaseSchStatusHistoryTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseSchStatusHistory] SET " + 
            //"  [idPurchaseSchStatusHistory] = @IdPurchaseSchStatusHistory" +
            "  [statusId]= @StatusId" +
            " ,[vehiclePhaseId]= @VehiclePhaseId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[purchaseScheduleSummaryId]= @PurchaseScheduleSummaryId" +
            " ,[createdOn]= @CreatedOn" +
            " ,[comment] = @Comment" +
            " WHERE idPurchaseSchStatusHistory = @IdPurchaseSchStatusHistory "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPurchaseSchStatusHistory", System.Data.SqlDbType.Int).Value = tblPurchaseSchStatusHistoryTO.IdPurchaseSchStatusHistory;
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseSchStatusHistoryTO.StatusId);
            cmdUpdate.Parameters.Add("@VehiclePhaseId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseSchStatusHistoryTO.VehiclePhaseId);
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseSchStatusHistoryTO.CreatedBy;
            cmdUpdate.Parameters.Add("@PurchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseSchStatusHistoryTO.PurchaseScheduleSummaryId);
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseSchStatusHistoryTO.CreatedOn;
            cmdUpdate.Parameters.Add("@Comment", System.Data.SqlDbType.NVarChar).Value = tblPurchaseSchStatusHistoryTO.Comment;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblPurchaseSchStatusHistory(Int32 idPurchaseSchStatusHistory)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPurchaseSchStatusHistory, cmdDelete);
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

        public int DeleteTblPurchaseSchStatusHistory(Int32 idPurchaseSchStatusHistory, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPurchaseSchStatusHistory, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idPurchaseSchStatusHistory, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPurchaseSchStatusHistory] " +
            " WHERE idPurchaseSchStatusHistory = " + idPurchaseSchStatusHistory +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPurchaseSchStatusHistory", System.Data.SqlDbType.Int).Value = tblPurchaseSchStatusHistoryTO.IdPurchaseSchStatusHistory;
            return cmdDelete.ExecuteNonQuery();
        }

        public int DeletePurchaseVehHistoryDtls(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.CommandText = "DELETE FROM tblPurchaseSchStatusHistory WHERE purchaseScheduleSummaryId=" + purchaseScheduleId + "";
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                cmdDelete.CommandType = System.Data.CommandType.Text;

                return cmdDelete.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }
        #endregion

    }
}
