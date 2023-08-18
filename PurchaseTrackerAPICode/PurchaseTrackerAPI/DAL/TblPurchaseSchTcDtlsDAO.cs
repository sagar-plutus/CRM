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
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.DAL
{
   

    public class TblPurchaseSchTcDtlsDAO : ITblPurchaseSchTcDtlsDAO
    {

        private readonly IConnectionString _iConnectionString;

        public TblPurchaseSchTcDtlsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT dimPurchaseTcType.TcTypeName,dimPurchaseTcElement.TcElementName,tblPurchaseSchTcDtls.* FROM [tblPurchaseSchTcDtls] tblPurchaseSchTcDtls " +
                                  " LEFT JOIN dimPurchaseTcElement ON tblPurchaseSchTcDtls.tcElementId = dimPurchaseTcElement.idTcElement " +
                                  " LEFT JOIN dimPurchaseTcType ON tblPurchaseSchTcDtls.tcTypeId = dimPurchaseTcType.idTcType"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblPurchaseSchTcDtlsTO> SelectAllTblPurchaseSchTcDtls()
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
                List<TblPurchaseSchTcDtlsTO> list = ConvertDTToList(sqlReader);
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

        public List<TblPurchaseSchTcDtlsTO> SelectTblPurchaseSchTcDtls(Int32 idPurchasseSchTcDtls)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idPurchasseSchTcDtls = " + idPurchasseSchTcDtls +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseSchTcDtlsTO> list = ConvertDTToList(sqlReader);
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

        public List<TblPurchaseSchTcDtlsTO> SelectScheTcDtlsByRootScheduleId(String rootScheduleIds)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblPurchaseSchTcDtls.isActive = 1  AND tblPurchaseSchTcDtls.purchaseScheduleSummaryId IN ( " + rootScheduleIds + " )";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseSchTcDtlsTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
                else return null;
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

        public List<TblPurchaseSchTcDtlsTO> SelectAllTblPurchaseSchTcDtls(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseSchTcDtlsTO> list = ConvertDTToList(sqlReader);
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

        public List<TblPurchaseSchTcDtlsTO> ConvertDTToList(SqlDataReader tblPurchaseSchTcDtlsTODT)
        {
            List<TblPurchaseSchTcDtlsTO> tblPurchaseSchTcDtlsTOList = new List<TblPurchaseSchTcDtlsTO>();
            if (tblPurchaseSchTcDtlsTODT != null)
            {
                while(tblPurchaseSchTcDtlsTODT.Read())
                {
                    TblPurchaseSchTcDtlsTO tblPurchaseSchTcDtlsTONew = new TblPurchaseSchTcDtlsTO();
                    if (tblPurchaseSchTcDtlsTODT ["idPurchasseSchTcDtls"] != DBNull.Value)
                        tblPurchaseSchTcDtlsTONew.IdPurchasseSchTcDtls = Convert.ToInt32(tblPurchaseSchTcDtlsTODT ["idPurchasseSchTcDtls"].ToString());
                    if (tblPurchaseSchTcDtlsTODT ["tcTypeId"] != DBNull.Value)
                        tblPurchaseSchTcDtlsTONew.TcTypeId = Convert.ToInt32(tblPurchaseSchTcDtlsTODT ["tcTypeId"].ToString());
                    if (tblPurchaseSchTcDtlsTODT ["tcElementId"] != DBNull.Value)
                        tblPurchaseSchTcDtlsTONew.TcElementId = Convert.ToInt32(tblPurchaseSchTcDtlsTODT ["tcElementId"].ToString());
                    if (tblPurchaseSchTcDtlsTODT ["isActive"] != DBNull.Value)
                        tblPurchaseSchTcDtlsTONew.IsActive = Convert.ToInt32(tblPurchaseSchTcDtlsTODT ["isActive"].ToString());
                    if (tblPurchaseSchTcDtlsTODT ["purchaseScheduleSummaryId"] != DBNull.Value)
                        tblPurchaseSchTcDtlsTONew.PurchaseScheduleSummaryId = Convert.ToInt32(tblPurchaseSchTcDtlsTODT ["purchaseScheduleSummaryId"].ToString());
                    if (tblPurchaseSchTcDtlsTODT ["createdBy"] != DBNull.Value)
                        tblPurchaseSchTcDtlsTONew.CreatedBy = Convert.ToInt32(tblPurchaseSchTcDtlsTODT ["createdBy"].ToString());
                    if (tblPurchaseSchTcDtlsTODT ["updatedBy"] != DBNull.Value)
                        tblPurchaseSchTcDtlsTONew.UpdatedBy = Convert.ToInt32(tblPurchaseSchTcDtlsTODT ["updatedBy"].ToString());
                    if (tblPurchaseSchTcDtlsTODT ["createdOn"] != DBNull.Value)
                        tblPurchaseSchTcDtlsTONew.CreatedOn = Convert.ToDateTime(tblPurchaseSchTcDtlsTODT ["createdOn"].ToString());
                    if (tblPurchaseSchTcDtlsTODT ["updatedOn"] != DBNull.Value)
                        tblPurchaseSchTcDtlsTONew.UpdatedOn = Convert.ToDateTime(tblPurchaseSchTcDtlsTODT ["updatedOn"].ToString());
                    if (tblPurchaseSchTcDtlsTODT ["tcEleValue"] != DBNull.Value)
                        tblPurchaseSchTcDtlsTONew.TcEleValue = Convert.ToString(tblPurchaseSchTcDtlsTODT ["tcEleValue"].ToString());
                    if (tblPurchaseSchTcDtlsTODT["tcTypeName"] != DBNull.Value)
                        tblPurchaseSchTcDtlsTONew.TcTypeName = Convert.ToString(tblPurchaseSchTcDtlsTODT["tcTypeName"].ToString());
                    if (tblPurchaseSchTcDtlsTODT["tcElementName"] != DBNull.Value)
                        tblPurchaseSchTcDtlsTONew.TcElementName = Convert.ToString(tblPurchaseSchTcDtlsTODT["tcElementName"].ToString());
                    tblPurchaseSchTcDtlsTOList.Add(tblPurchaseSchTcDtlsTONew);
                }
            }
            return tblPurchaseSchTcDtlsTOList;
        }
        #endregion

        #region Insertion
        public int InsertTblPurchaseSchTcDtls(TblPurchaseSchTcDtlsTO tblPurchaseSchTcDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPurchaseSchTcDtlsTO, cmdInsert);
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

        public int InsertTblPurchaseSchTcDtls(TblPurchaseSchTcDtlsTO tblPurchaseSchTcDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPurchaseSchTcDtlsTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblPurchaseSchTcDtlsTO tblPurchaseSchTcDtlsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPurchaseSchTcDtls]( " + 
            //"  [idPurchasseSchTcDtls]" +
            "  [tcTypeId]" +
            " ,[tcElementId]" +
            " ,[isActive]" +
            " ,[purchaseScheduleSummaryId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[tcEleValue]" +
            " )" +
" VALUES (" +
            //"  @IdPurchasseSchTcDtls " +
            "  @TcTypeId " +
            " ,@TcElementId " +
            " ,@IsActive " +
            " ,@PurchaseScheduleSummaryId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@TcEleValue " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdPurchasseSchTcDtls", System.Data.SqlDbType.Int).Value = tblPurchaseSchTcDtlsTO.IdPurchasseSchTcDtls;
            cmdInsert.Parameters.Add("@TcTypeId", System.Data.SqlDbType.Int).Value = tblPurchaseSchTcDtlsTO.TcTypeId;
            cmdInsert.Parameters.Add("@TcElementId", System.Data.SqlDbType.Int).Value = tblPurchaseSchTcDtlsTO.TcElementId;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblPurchaseSchTcDtlsTO.IsActive;
            cmdInsert.Parameters.Add("@PurchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = tblPurchaseSchTcDtlsTO.PurchaseScheduleSummaryId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseSchTcDtlsTO.CreatedBy);
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseSchTcDtlsTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseSchTcDtlsTO.CreatedOn);
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseSchTcDtlsTO.UpdatedOn);
            cmdInsert.Parameters.Add("@TcEleValue", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseSchTcDtlsTO.TcEleValue);
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public int UpdateTblPurchaseSchTcDtls(TblPurchaseSchTcDtlsTO tblPurchaseSchTcDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPurchaseSchTcDtlsTO, cmdUpdate);
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

        public int UpdateTblPurchaseSchTcDtls(TblPurchaseSchTcDtlsTO tblPurchaseSchTcDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPurchaseSchTcDtlsTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblPurchaseSchTcDtlsTO tblPurchaseSchTcDtlsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseSchTcDtls] SET " + 
            //"  [idPurchasseSchTcDtls] = @IdPurchasseSchTcDtls" +
            "  [tcTypeId]= @TcTypeId" +
            " ,[tcElementId]= @TcElementId" +
            " ,[isActive]= @IsActive" +
            " ,[purchaseScheduleSummaryId]= @PurchaseScheduleSummaryId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[tcEleValue] = @TcEleValue" +
            " WHERE idPurchasseSchTcDtls = @IdPurchasseSchTcDtls "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPurchasseSchTcDtls", System.Data.SqlDbType.Int).Value = tblPurchaseSchTcDtlsTO.IdPurchasseSchTcDtls;
            cmdUpdate.Parameters.Add("@TcTypeId", System.Data.SqlDbType.Int).Value = tblPurchaseSchTcDtlsTO.TcTypeId;
            cmdUpdate.Parameters.Add("@TcElementId", System.Data.SqlDbType.Int).Value = tblPurchaseSchTcDtlsTO.TcElementId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblPurchaseSchTcDtlsTO.IsActive;
            cmdUpdate.Parameters.Add("@PurchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = tblPurchaseSchTcDtlsTO.PurchaseScheduleSummaryId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseSchTcDtlsTO.CreatedBy);
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseSchTcDtlsTO.UpdatedBy);
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseSchTcDtlsTO.CreatedOn);
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseSchTcDtlsTO.UpdatedOn);
            cmdUpdate.Parameters.Add("@TcEleValue", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseSchTcDtlsTO.TcEleValue);
            return cmdUpdate.ExecuteNonQuery();
        }

        public int UpdateIsActiveAgainstSch(Int32 rootScheduleId,Int32 isActive, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();

            String sqlQuery = @" UPDATE [tblPurchaseSchTcDtls] SET " +
             "[isActive]= @IsActive" +
            " WHERE purchaseScheduleSummaryId = @PurchaseScheduleSummaryId ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.Connection = conn;
            cmdUpdate.Transaction = tran;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@PurchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = rootScheduleId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(isActive);

            return cmdUpdate.ExecuteNonQuery();

        }


        #endregion

        #region Deletion
        public int DeleteTblPurchaseSchTcDtls(Int32 idPurchasseSchTcDtls)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPurchasseSchTcDtls, cmdDelete);
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

        public int DeleteTblPurchaseSchTcDtls(Int32 idPurchasseSchTcDtls, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPurchasseSchTcDtls, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idPurchasseSchTcDtls, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPurchaseSchTcDtls] " +
            " WHERE idPurchasseSchTcDtls = " + idPurchasseSchTcDtls +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPurchasseSchTcDtls", System.Data.SqlDbType.Int).Value = tblPurchaseSchTcDtlsTO.IdPurchasseSchTcDtls;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
