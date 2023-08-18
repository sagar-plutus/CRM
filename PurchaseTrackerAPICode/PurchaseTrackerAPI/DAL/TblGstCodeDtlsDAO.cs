using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblGstCodeDtlsDAO : ITblGstCodeDtlsDAO
    {


        private readonly IConnectionString _iConnectionString;
        public TblGstCodeDtlsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblGstCodeDtls]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblGstCodeDtlsTO> SelectAllTblGstCodeDtls()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE isActive=1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGstCodeDtlsTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }       

        public TblGstCodeDtlsTO SelectTblGstCodeDtls(Int32 idGstCode,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idGstCode = " + idGstCode +" ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGstCodeDtlsTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];

                return null;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public  TblGstCodeDtlsTO SelectGstCodeDtlsTO(Int32 prodCatId, Int32 prodSpecId, Int32 materialId, Int32 prodItemId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idGstCode In(" + " SELECT gstCodeId FROM tblProdGstCodeDtls WHERE isActive = 1 " +
                                       "AND ISNULL(materialId,0) = " + materialId + " " +
                                       "AND ISNULL(prodCatId,0) = " + prodCatId + " " +
                                       "AND ISNULL(prodSpecId,0) = " + prodSpecId + " " +
                                       "AND ISNULL(prodItemId,0)=" + prodItemId + ")";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGstCodeDtlsTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public  List<TblGstCodeDtlsTO> ConvertDTToList(SqlDataReader tblGstCodeDtlsTODT)
        {
            List<TblGstCodeDtlsTO> tblGstCodeDtlsTOList = new List<TblGstCodeDtlsTO>();
            if (tblGstCodeDtlsTODT != null)
            {
                while (tblGstCodeDtlsTODT.Read())
                {
                    TblGstCodeDtlsTO tblGstCodeDtlsTONew = new TblGstCodeDtlsTO();
                    if (tblGstCodeDtlsTODT["idGstCode"] != DBNull.Value)
                        tblGstCodeDtlsTONew.IdGstCode = Convert.ToInt32(tblGstCodeDtlsTODT["idGstCode"].ToString());
                    if (tblGstCodeDtlsTODT["codeTypeId"] != DBNull.Value)
                        tblGstCodeDtlsTONew.CodeTypeId = Convert.ToInt32(tblGstCodeDtlsTODT["codeTypeId"].ToString());
                    if (tblGstCodeDtlsTODT["createdBy"] != DBNull.Value)
                        tblGstCodeDtlsTONew.CreatedBy = Convert.ToInt32(tblGstCodeDtlsTODT["createdBy"].ToString());
                    if (tblGstCodeDtlsTODT["updatedBy"] != DBNull.Value)
                        tblGstCodeDtlsTONew.UpdatedBy = Convert.ToInt32(tblGstCodeDtlsTODT["updatedBy"].ToString());
                    if (tblGstCodeDtlsTODT["effectiveFromDt"] != DBNull.Value)
                        tblGstCodeDtlsTONew.EffectiveFromDt = Convert.ToDateTime(tblGstCodeDtlsTODT["effectiveFromDt"].ToString());
                    if (tblGstCodeDtlsTODT["effectiveToDt"] != DBNull.Value)
                        tblGstCodeDtlsTONew.EffectiveToDt = Convert.ToDateTime(tblGstCodeDtlsTODT["effectiveToDt"].ToString());
                    if (tblGstCodeDtlsTODT["createdOn"] != DBNull.Value)
                        tblGstCodeDtlsTONew.CreatedOn = Convert.ToDateTime(tblGstCodeDtlsTODT["createdOn"].ToString());
                    if (tblGstCodeDtlsTODT["updatedOn"] != DBNull.Value)
                        tblGstCodeDtlsTONew.UpdatedOn = Convert.ToDateTime(tblGstCodeDtlsTODT["updatedOn"].ToString());
                    if (tblGstCodeDtlsTODT["taxPct"] != DBNull.Value)
                        tblGstCodeDtlsTONew.TaxPct = Convert.ToDouble(tblGstCodeDtlsTODT["taxPct"].ToString());
                    if (tblGstCodeDtlsTODT["codeDesc"] != DBNull.Value)
                        tblGstCodeDtlsTONew.CodeDesc = Convert.ToString(tblGstCodeDtlsTODT["codeDesc"].ToString());
                    if (tblGstCodeDtlsTODT["codeNumber"] != DBNull.Value)
                        tblGstCodeDtlsTONew.CodeNumber = Convert.ToString(tblGstCodeDtlsTODT["codeNumber"].ToString());
                    if (tblGstCodeDtlsTODT["isActive"] != DBNull.Value)
                        tblGstCodeDtlsTONew.IsActive = Convert.ToInt32(tblGstCodeDtlsTODT["isActive"].ToString());
                    tblGstCodeDtlsTOList.Add(tblGstCodeDtlsTONew);
                }
            }
            return tblGstCodeDtlsTOList;
        }

        #endregion

        #region Insertion
        public  int InsertTblGstCodeDtls(TblGstCodeDtlsTO tblGstCodeDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblGstCodeDtlsTO, cmdInsert);
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

        public  int InsertTblGstCodeDtls(TblGstCodeDtlsTO tblGstCodeDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblGstCodeDtlsTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblGstCodeDtlsTO tblGstCodeDtlsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblGstCodeDtls]( " + 
                                "  [codeTypeId]" +
                                " ,[createdBy]" +
                                " ,[updatedBy]" +
                                " ,[effectiveFromDt]" +
                                " ,[effectiveToDt]" +
                                " ,[createdOn]" +
                                " ,[updatedOn]" +
                                " ,[taxPct]" +
                                " ,[codeDesc]" +
                                " ,[codeNumber]" +
                                " ,[isActive]" +
                                " )" +
                    " VALUES (" +
                                "  @CodeTypeId " +
                                " ,@CreatedBy " +
                                " ,@UpdatedBy " +
                                " ,@EffectiveFromDt " +
                                " ,@EffectiveToDt " +
                                " ,@CreatedOn " +
                                " ,@UpdatedOn " +
                                " ,@TaxPct " +
                                " ,@CodeDesc " +
                                " ,@CodeNumber " +
                                " ,@isActive " +
                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdGstCode", System.Data.SqlDbType.Int).Value = tblGstCodeDtlsTO.IdGstCode;
            cmdInsert.Parameters.Add("@CodeTypeId", System.Data.SqlDbType.Int).Value = tblGstCodeDtlsTO.CodeTypeId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblGstCodeDtlsTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblGstCodeDtlsTO.UpdatedBy);
            cmdInsert.Parameters.Add("@EffectiveFromDt", System.Data.SqlDbType.DateTime).Value = tblGstCodeDtlsTO.EffectiveFromDt;
            cmdInsert.Parameters.Add("@EffectiveToDt", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblGstCodeDtlsTO.EffectiveToDt);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblGstCodeDtlsTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue( tblGstCodeDtlsTO.UpdatedOn);
            cmdInsert.Parameters.Add("@TaxPct", System.Data.SqlDbType.NVarChar).Value = tblGstCodeDtlsTO.TaxPct;
            cmdInsert.Parameters.Add("@CodeDesc", System.Data.SqlDbType.NVarChar).Value = tblGstCodeDtlsTO.CodeDesc;
            cmdInsert.Parameters.Add("@CodeNumber", System.Data.SqlDbType.NVarChar).Value = tblGstCodeDtlsTO.CodeNumber;
            cmdInsert.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = tblGstCodeDtlsTO.IsActive;
            if(cmdInsert.ExecuteNonQuery()==1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblGstCodeDtlsTO.IdGstCode = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }

            return 0;
        }
        #endregion
        
        #region Updation
        public  int UpdateTblGstCodeDtls(TblGstCodeDtlsTO tblGstCodeDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblGstCodeDtlsTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public  int UpdateTblGstCodeDtls(TblGstCodeDtlsTO tblGstCodeDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblGstCodeDtlsTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public  int ExecuteUpdationCommand(TblGstCodeDtlsTO tblGstCodeDtlsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblGstCodeDtls] SET " + 
                            "  [codeTypeId]= @CodeTypeId" +
                            " ,[updatedBy]= @UpdatedBy" +
                            " ,[effectiveFromDt]= @EffectiveFromDt" +
                            " ,[effectiveToDt]= @EffectiveToDt" +
                            " ,[updatedOn]= @UpdatedOn" +
                            " ,[taxPct]= @TaxPct" +
                            " ,[codeDesc]= @CodeDesc" +
                            " ,[codeNumber] = @CodeNumber" +
                            " ,[isActive] = @isActive" +
                            " WHERE [idGstCode] = @IdGstCode"; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdGstCode", System.Data.SqlDbType.Int).Value = tblGstCodeDtlsTO.IdGstCode;
            cmdUpdate.Parameters.Add("@CodeTypeId", System.Data.SqlDbType.Int).Value = tblGstCodeDtlsTO.CodeTypeId;
            //cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblGstCodeDtlsTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblGstCodeDtlsTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@EffectiveFromDt", System.Data.SqlDbType.DateTime).Value = tblGstCodeDtlsTO.EffectiveFromDt;
            cmdUpdate.Parameters.Add("@EffectiveToDt", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblGstCodeDtlsTO.EffectiveToDt);
            //cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblGstCodeDtlsTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblGstCodeDtlsTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@TaxPct", System.Data.SqlDbType.NVarChar).Value = tblGstCodeDtlsTO.TaxPct;
            cmdUpdate.Parameters.Add("@CodeDesc", System.Data.SqlDbType.NVarChar).Value = tblGstCodeDtlsTO.CodeDesc;
            cmdUpdate.Parameters.Add("@CodeNumber", System.Data.SqlDbType.NVarChar).Value = tblGstCodeDtlsTO.CodeNumber;
            cmdUpdate.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = tblGstCodeDtlsTO.IsActive;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblGstCodeDtls(Int32 idGstCode)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idGstCode, cmdDelete);
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

        public  int DeleteTblGstCodeDtls(Int32 idGstCode, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idGstCode, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idGstCode, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblGstCodeDtls] " +
            " WHERE idGstCode = " + idGstCode +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idGstCode", System.Data.SqlDbType.Int).Value = tblGstCodeDtlsTO.IdGstCode;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
