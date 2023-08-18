using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblTRSLADAO : ITblTRSLADAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblTRSLADAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods
        String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblTRSLA]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public DataTable SelectAllTblTRSLA()
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

                //cmdSelect.Parameters.Add("@idSLA", System.Data.SqlDbType.Int).Value = tblTRSLATO.IdSLA;
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public DataTable SelectTblTRSLA(Int32 idSLA)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idSLA = " + idSLA + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idSLA", System.Data.SqlDbType.Int).Value = tblTRSLATO.IdSLA;
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public DataTable SelectAllTblTRSLA(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idSLA", System.Data.SqlDbType.Int).Value = tblTRSLATO.IdSLA;
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
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

        #endregion

        #region Insertion
        public int InsertTblTRSLA(TblTRSLATO tblTRSLATO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblTRSLATO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public int InsertTblTRSLA(TblTRSLATO tblTRSLATO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblTRSLATO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        int ExecuteInsertionCommand(TblTRSLATO tblTRSLATO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblTRSLA]( " +
            "  [idSLA]" +
            " ,[transferRequestId]" +
            " ,[unloadingId]" +
            " ,[mixMaterialId]" +
            " ,[waste]" +
            " ,[offChemistryId]" +
            " ,[descity]" +
            " ,[statusId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[isActive]" +
            " ,[overSizePer]" +
            " ,[displayNo]" +
            " )" +
" VALUES (" +
            "  @IdSLA " +
            " ,@TransferRequestId " +
            " ,@UnloadingId " +
            " ,@MixMaterialId " +
            " ,@Waste " +
            " ,@OffChemistryId " +
            " ,@Descity " +
            " ,@StatusId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@IsActive " +
            " ,@OverSizePer " +
            " ,@DisplayNo " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdSLA", System.Data.SqlDbType.Int).Value = tblTRSLATO.IdSLA;
            cmdInsert.Parameters.Add("@TransferRequestId", System.Data.SqlDbType.Int).Value = tblTRSLATO.TransferRequestId;
            cmdInsert.Parameters.Add("@UnloadingId", System.Data.SqlDbType.Int).Value = tblTRSLATO.UnloadingId;
            cmdInsert.Parameters.Add("@MixMaterialId", System.Data.SqlDbType.Int).Value = tblTRSLATO.MixMaterialId;
            cmdInsert.Parameters.Add("@Waste", System.Data.SqlDbType.Int).Value = tblTRSLATO.Waste;
            cmdInsert.Parameters.Add("@OffChemistryId", System.Data.SqlDbType.Int).Value = tblTRSLATO.OffChemistryId;
            cmdInsert.Parameters.Add("@Descity", System.Data.SqlDbType.Int).Value = tblTRSLATO.Descity;
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblTRSLATO.StatusId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblTRSLATO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblTRSLATO.UpdatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblTRSLATO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblTRSLATO.UpdatedOn;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = tblTRSLATO.IsActive;
            cmdInsert.Parameters.Add("@OverSizePer", System.Data.SqlDbType.Decimal).Value = tblTRSLATO.OverSizePer;
            cmdInsert.Parameters.Add("@DisplayNo", System.Data.SqlDbType.NVarChar).Value = tblTRSLATO.DisplayNo;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public int UpdateTblTRSLA(TblTRSLATO tblTRSLATO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblTRSLATO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public int UpdateTblTRSLA(TblTRSLATO tblTRSLATO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblTRSLATO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        int ExecuteUpdationCommand(TblTRSLATO tblTRSLATO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblTRSLA] SET " +
            "  [idSLA] = @IdSLA" +
            " ,[transferRequestId]= @TransferRequestId" +
            " ,[unloadingId]= @UnloadingId" +
            " ,[mixMaterialId]= @MixMaterialId" +
            " ,[waste]= @Waste" +
            " ,[offChemistryId]= @OffChemistryId" +
            " ,[descity]= @Descity" +
            " ,[statusId]= @StatusId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[isActive]= @IsActive" +
            " ,[overSizePer]= @OverSizePer" +
            " ,[displayNo] = @DisplayNo" +
            " WHERE 1 = 2 ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdSLA", System.Data.SqlDbType.Int).Value = tblTRSLATO.IdSLA;
            cmdUpdate.Parameters.Add("@TransferRequestId", System.Data.SqlDbType.Int).Value = tblTRSLATO.TransferRequestId;
            cmdUpdate.Parameters.Add("@UnloadingId", System.Data.SqlDbType.Int).Value = tblTRSLATO.UnloadingId;
            cmdUpdate.Parameters.Add("@MixMaterialId", System.Data.SqlDbType.Int).Value = tblTRSLATO.MixMaterialId;
            cmdUpdate.Parameters.Add("@Waste", System.Data.SqlDbType.Int).Value = tblTRSLATO.Waste;
            cmdUpdate.Parameters.Add("@OffChemistryId", System.Data.SqlDbType.Int).Value = tblTRSLATO.OffChemistryId;
            cmdUpdate.Parameters.Add("@Descity", System.Data.SqlDbType.Int).Value = tblTRSLATO.Descity;
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblTRSLATO.StatusId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblTRSLATO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblTRSLATO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblTRSLATO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblTRSLATO.UpdatedOn;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = tblTRSLATO.IsActive;
            cmdUpdate.Parameters.Add("@OverSizePer", System.Data.SqlDbType.Decimal).Value = tblTRSLATO.OverSizePer;
            cmdUpdate.Parameters.Add("@DisplayNo", System.Data.SqlDbType.NVarChar).Value = tblTRSLATO.DisplayNo;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion


    }
}
