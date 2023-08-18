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
    public class TblPurchaseVehicleStatusHistoryDAO : ITblPurchaseVehicleStatusHistoryDAO
    {
        
        private readonly IConnectionString _iConnectionString;

        public TblPurchaseVehicleStatusHistoryDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblPurchaseVehicleStatusHistory]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblPurchaseVehicleStatusHistoryTO> SelectAllTblPurchaseVehicleStatusHistory()
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

                //cmdSelect.Parameters.Add("@idPurVehStatusHistory", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleStatusHistoryTO.IdPurVehStatusHistory;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehicleStatusHistoryTO> list = ConvertDTToList(reader);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public  List<TblPurchaseVehicleStatusHistoryTO> SelectTblPurchaseVehicleStatusHistory(Int32 idPurVehStatusHistory)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idPurVehStatusHistory = " + idPurVehStatusHistory +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurVehStatusHistory", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleStatusHistoryTO.IdPurVehStatusHistory;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehicleStatusHistoryTO> list = ConvertDTToList(reader);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public  List<TblPurchaseVehicleStatusHistoryTO>  SelectAllTblPurchaseVehicleStatusHistory(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurVehStatusHistory", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleStatusHistoryTO.IdPurVehStatusHistory;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehicleStatusHistoryTO> list = ConvertDTToList(reader);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static List<TblPurchaseVehicleStatusHistoryTO> ConvertDTToList(SqlDataReader tblPurchaseVehicleStatusHistoryTODT )
        {
            List<TblPurchaseVehicleStatusHistoryTO> tblPurchaseVehicleStatusHistoryTOList = new List<TblPurchaseVehicleStatusHistoryTO>();
            if (tblPurchaseVehicleStatusHistoryTODT != null)
            {
                while(tblPurchaseVehicleStatusHistoryTODT.Read())
                {
                    TblPurchaseVehicleStatusHistoryTO tblPurchaseVehicleStatusHistoryTONew = new TblPurchaseVehicleStatusHistoryTO();
                    if(tblPurchaseVehicleStatusHistoryTODT["idPurVehStatusHistory"] != DBNull.Value)
                        tblPurchaseVehicleStatusHistoryTONew.IdPurVehStatusHistory = Convert.ToInt32( tblPurchaseVehicleStatusHistoryTODT["idPurVehStatusHistory"].ToString());
                    if(tblPurchaseVehicleStatusHistoryTODT["purchaseScheduleSummaryId"] != DBNull.Value)
                        tblPurchaseVehicleStatusHistoryTONew.PurchaseScheduleSummaryId = Convert.ToInt32( tblPurchaseVehicleStatusHistoryTODT["purchaseScheduleSummaryId"].ToString());
                    if(tblPurchaseVehicleStatusHistoryTODT["statusId"] != DBNull.Value)
                        tblPurchaseVehicleStatusHistoryTONew.StatusId = Convert.ToInt32( tblPurchaseVehicleStatusHistoryTODT["statusId"].ToString());
                    if(tblPurchaseVehicleStatusHistoryTODT["vehiclePhaseId"] != DBNull.Value)
                        tblPurchaseVehicleStatusHistoryTONew.VehiclePhaseId = Convert.ToInt32( tblPurchaseVehicleStatusHistoryTODT["vehiclePhaseId"].ToString());
                    if(tblPurchaseVehicleStatusHistoryTODT["createdBy"] != DBNull.Value)
                        tblPurchaseVehicleStatusHistoryTONew.CreatedBy = Convert.ToInt32( tblPurchaseVehicleStatusHistoryTODT["createdBy"].ToString());
                    if(tblPurchaseVehicleStatusHistoryTODT["updatedBy"] != DBNull.Value)
                        tblPurchaseVehicleStatusHistoryTONew.UpdatedBy = Convert.ToInt32( tblPurchaseVehicleStatusHistoryTODT["updatedBy"].ToString());
                    if(tblPurchaseVehicleStatusHistoryTODT["createdOn"] != DBNull.Value)
                        tblPurchaseVehicleStatusHistoryTONew.CreatedOn = Convert.ToDateTime( tblPurchaseVehicleStatusHistoryTODT["createdOn"].ToString());
                    if(tblPurchaseVehicleStatusHistoryTODT["updatedOn"] != DBNull.Value)
                        tblPurchaseVehicleStatusHistoryTONew.UpdatedOn = Convert.ToDateTime( tblPurchaseVehicleStatusHistoryTODT["updatedOn"].ToString());
                    if(tblPurchaseVehicleStatusHistoryTODT["comment"] != DBNull.Value)
                        tblPurchaseVehicleStatusHistoryTONew.Comment = Convert.ToString( tblPurchaseVehicleStatusHistoryTODT["comment"].ToString());
                    tblPurchaseVehicleStatusHistoryTOList.Add(tblPurchaseVehicleStatusHistoryTONew);
                }
            }
            return tblPurchaseVehicleStatusHistoryTOList;
        }


        #endregion
        
        #region Insertion
        public  int InsertTblPurchaseVehicleStatusHistory(TblPurchaseVehicleStatusHistoryTO tblPurchaseVehicleStatusHistoryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPurchaseVehicleStatusHistoryTO, cmdInsert);
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

        public  int InsertTblPurchaseVehicleStatusHistory(TblPurchaseVehicleStatusHistoryTO tblPurchaseVehicleStatusHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPurchaseVehicleStatusHistoryTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblPurchaseVehicleStatusHistoryTO tblPurchaseVehicleStatusHistoryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPurchaseVehicleStatusHistory]( " + 
            //"  [idPurVehStatusHistory]" +
            "  [purchaseScheduleSummaryId]" +
            " ,[statusId]" +
            " ,[vehiclePhaseId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[comment]" +
            " )" +
" VALUES (" +
            //"  @IdPurVehStatusHistory " +
            "  @PurchaseScheduleSummaryId " +
            " ,@StatusId " +
            " ,@VehiclePhaseId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@Comment " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdPurVehStatusHistory", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleStatusHistoryTO.IdPurVehStatusHistory;
            cmdInsert.Parameters.Add("@PurchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleStatusHistoryTO.PurchaseScheduleSummaryId;
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleStatusHistoryTO.StatusId;
            cmdInsert.Parameters.Add("@VehiclePhaseId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleStatusHistoryTO.VehiclePhaseId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleStatusHistoryTO.CreatedBy);
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleStatusHistoryTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleStatusHistoryTO.CreatedOn);
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleStatusHistoryTO.UpdatedOn);
            cmdInsert.Parameters.Add("@Comment", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleStatusHistoryTO.Comment);
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblPurchaseVehicleStatusHistory(TblPurchaseVehicleStatusHistoryTO tblPurchaseVehicleStatusHistoryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPurchaseVehicleStatusHistoryTO, cmdUpdate);
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

        public  int UpdateTblPurchaseVehicleStatusHistory(TblPurchaseVehicleStatusHistoryTO tblPurchaseVehicleStatusHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPurchaseVehicleStatusHistoryTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblPurchaseVehicleStatusHistoryTO tblPurchaseVehicleStatusHistoryTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseVehicleStatusHistory] SET " + 
            //"  [idPurVehStatusHistory] = @IdPurVehStatusHistory" +
            "  [purchaseScheduleSummaryId]= @PurchaseScheduleSummaryId" +
            " ,[statusId]= @StatusId" +
            " ,[vehiclePhaseId]= @VehiclePhaseId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[comment] = @Comment" +
            " WHERE idPurVehStatusHistory =  @IdPurVehStatusHistory"; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPurVehStatusHistory", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleStatusHistoryTO.IdPurVehStatusHistory;
            cmdUpdate.Parameters.Add("@PurchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleStatusHistoryTO.PurchaseScheduleSummaryId;
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleStatusHistoryTO.StatusId;
            cmdUpdate.Parameters.Add("@VehiclePhaseId", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleStatusHistoryTO.VehiclePhaseId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleStatusHistoryTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleStatusHistoryTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseVehicleStatusHistoryTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseVehicleStatusHistoryTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@Comment", System.Data.SqlDbType.VarChar).Value = tblPurchaseVehicleStatusHistoryTO.Comment;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblPurchaseVehicleStatusHistory(Int32 idPurVehStatusHistory)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPurVehStatusHistory, cmdDelete);
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

        public  int DeleteTblPurchaseVehicleStatusHistory(Int32 idPurVehStatusHistory, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPurVehStatusHistory, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idPurVehStatusHistory, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPurchaseVehicleStatusHistory] " +
            " WHERE idPurVehStatusHistory = " + idPurVehStatusHistory +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPurVehStatusHistory", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleStatusHistoryTO.IdPurVehStatusHistory;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
