using System;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;
using System.Collections.Generic;

namespace PurchaseTrackerAPI.DAL
{
    public class TblScheduleDensityDAO : ITblScheduleDensityDAO
    {
        #region Methods

        private readonly IConnectionString _iConnectionString;

        public TblScheduleDensityDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblScheduleDensity]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblScheduleDensityTO> SelectAllTblScheduleDensity()
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

                //cmdSelect.Parameters.Add("@idScheduleDensity", System.Data.SqlDbType.Int).Value = tblScheduleDensityTO.IdScheduleDensity;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblScheduleDensityTO> list = ConvertDTToList(rdr);
                rdr.Dispose();
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

        public TblScheduleDensityTO SelectTblScheduleDensity(Int32 idScheduleDensity)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idScheduleDensity = " + idScheduleDensity +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idScheduleDensity", System.Data.SqlDbType.Int).Value = tblScheduleDensityTO.IdScheduleDensity;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblScheduleDensityTO> list = ConvertDTToList(rdr);
                rdr.Dispose();
                return list[0];
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

        public List<TblScheduleDensityTO> SelectAllTblScheduleDensity(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idScheduleDensity", System.Data.SqlDbType.Int).Value = tblScheduleDensityTO.IdScheduleDensity;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblScheduleDensityTO> list = ConvertDTToList(rdr);
                rdr.Dispose();
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


        public static List<TblScheduleDensityTO> ConvertDTToList(SqlDataReader tblScheduleDensityTODT)
        {
            List<TblScheduleDensityTO> tblScheduleDensityTOList = new List<TblScheduleDensityTO>();
            if (tblScheduleDensityTODT != null)
            {
                 while (tblScheduleDensityTODT.Read())
                 {
                    TblScheduleDensityTO tblScheduleDensityTONew = new TblScheduleDensityTO();
                    if (tblScheduleDensityTODT["idScheduleDensity"] != DBNull.Value)
                        tblScheduleDensityTONew.IdScheduleDensity = Convert.ToInt32(tblScheduleDensityTODT["idScheduleDensity"].ToString());
                    if (tblScheduleDensityTODT["purchaseScheduleSummaryId"] != DBNull.Value)
                        tblScheduleDensityTONew.PurchaseScheduleSummaryId = Convert.ToInt32(tblScheduleDensityTODT["purchaseScheduleSummaryId"].ToString());
                    if (tblScheduleDensityTODT["phaseId"] != DBNull.Value)
                        tblScheduleDensityTONew.PhaseId = Convert.ToInt32(tblScheduleDensityTODT["phaseId"].ToString());
                    if (tblScheduleDensityTODT["vehicleTypeId"] != DBNull.Value)
                        tblScheduleDensityTONew.VehicleTypeId = Convert.ToInt32(tblScheduleDensityTODT["vehicleTypeId"].ToString());
                    if (tblScheduleDensityTODT["createdBy"] != DBNull.Value)
                        tblScheduleDensityTONew.CreatedBy = Convert.ToInt32(tblScheduleDensityTODT["createdBy"].ToString());
                    if (tblScheduleDensityTODT["updatedBy"] != DBNull.Value)
                        tblScheduleDensityTONew.UpdatedBy = Convert.ToInt32(tblScheduleDensityTODT["updatedBy"].ToString());
                    if (tblScheduleDensityTODT["createdOn"] != DBNull.Value)
                        tblScheduleDensityTONew.CreatedOn = Convert.ToDateTime(tblScheduleDensityTODT["createdOn"].ToString());
                    if (tblScheduleDensityTODT["updatedOn"] != DBNull.Value)
                        tblScheduleDensityTONew.UpdatedOn = Convert.ToDateTime(tblScheduleDensityTODT["updatedOn"].ToString());
                    if (tblScheduleDensityTODT["height"] != DBNull.Value)
                        tblScheduleDensityTONew.Height = Convert.ToDouble(tblScheduleDensityTODT["height"].ToString());
                    if (tblScheduleDensityTODT["width"] != DBNull.Value)
                        tblScheduleDensityTONew.Width = Convert.ToDouble(tblScheduleDensityTODT["width"].ToString());
                    if (tblScheduleDensityTODT["length"] != DBNull.Value)
                        tblScheduleDensityTONew.Length = Convert.ToDouble(tblScheduleDensityTODT["length"].ToString());
                    tblScheduleDensityTOList.Add(tblScheduleDensityTONew);
                }
            }
            return tblScheduleDensityTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblScheduleDensity(TblScheduleDensityTO tblScheduleDensityTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblScheduleDensityTO, cmdInsert);
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

        public  int InsertTblScheduleDensity(TblScheduleDensityTO tblScheduleDensityTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblScheduleDensityTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblScheduleDensityTO tblScheduleDensityTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblScheduleDensity]( " + 
            "  [purchaseScheduleSummaryId]" +
            " ,[phaseId]" +
            " ,[vehicleTypeId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[height]" +
            " ,[width]" +
            " ,[length]" +
            " )" +
" VALUES (" +
            "  @PurchaseScheduleSummaryId " +
            " ,@PhaseId " +
            " ,@VehicleTypeId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@Height " +
            " ,@Width " +
            " ,@Length " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdScheduleDensity", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblScheduleDensityTO.IdScheduleDensity);
            cmdInsert.Parameters.Add("@PurchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblScheduleDensityTO.PurchaseScheduleSummaryId);
            cmdInsert.Parameters.Add("@PhaseId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblScheduleDensityTO.PhaseId);
            cmdInsert.Parameters.Add("@VehicleTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblScheduleDensityTO.VehicleTypeId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblScheduleDensityTO.CreatedBy);
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblScheduleDensityTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblScheduleDensityTO.CreatedOn);
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblScheduleDensityTO.UpdatedOn);
            cmdInsert.Parameters.Add("@Height", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblScheduleDensityTO.Height);
            cmdInsert.Parameters.Add("@Width", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblScheduleDensityTO.Width);
            cmdInsert.Parameters.Add("@Length", System.Data.SqlDbType.Decimal).Value =Constants.GetSqlDataValueNullForBaseValue(tblScheduleDensityTO.Length);
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblScheduleDensity(TblScheduleDensityTO tblScheduleDensityTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblScheduleDensityTO, cmdUpdate);
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

        public  int UpdateTblScheduleDensity(TblScheduleDensityTO tblScheduleDensityTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblScheduleDensityTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblScheduleDensityTO tblScheduleDensityTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblScheduleDensity] SET " + 
            "  [purchaseScheduleSummaryId]= @PurchaseScheduleSummaryId" +
            " ,[phaseId]= @PhaseId" +
            " ,[vehicleTypeId]= @VehicleTypeId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[height]= @Height" +
            " ,[width]= @Width" +
            " ,[length] = @Length" +
            " WHERE 1 = 1 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdScheduleDensity", System.Data.SqlDbType.Int).Value = tblScheduleDensityTO.IdScheduleDensity;
            cmdUpdate.Parameters.Add("@PurchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = tblScheduleDensityTO.PurchaseScheduleSummaryId;
            cmdUpdate.Parameters.Add("@PhaseId", System.Data.SqlDbType.Int).Value = tblScheduleDensityTO.PhaseId;
            cmdUpdate.Parameters.Add("@VehicleTypeId", System.Data.SqlDbType.Int).Value = tblScheduleDensityTO.VehicleTypeId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblScheduleDensityTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblScheduleDensityTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblScheduleDensityTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblScheduleDensityTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@Height", System.Data.SqlDbType.Decimal).Value = tblScheduleDensityTO.Height;
            cmdUpdate.Parameters.Add("@Width", System.Data.SqlDbType.Decimal).Value = tblScheduleDensityTO.Width;
            cmdUpdate.Parameters.Add("@Length", System.Data.SqlDbType.Decimal).Value = tblScheduleDensityTO.Length;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion 
        
        #region Deletion
        public  int DeleteTblScheduleDensity(Int32 idScheduleDensity)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idScheduleDensity, cmdDelete);
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

        public  int DeleteTblScheduleDensity(Int32 idScheduleDensity, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idScheduleDensity, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idScheduleDensity, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblScheduleDensity] " +
            " WHERE idScheduleDensity = " + idScheduleDensity +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idScheduleDensity", System.Data.SqlDbType.Int).Value = tblScheduleDensityTO.IdScheduleDensity;
            return cmdDelete.ExecuteNonQuery();
        }

        public int DeletePurchaseVehDensityDtls(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.CommandText = "DELETE FROM tblScheduleDensity WHERE purchaseScheduleSummaryId=" + purchaseScheduleId + "";
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
