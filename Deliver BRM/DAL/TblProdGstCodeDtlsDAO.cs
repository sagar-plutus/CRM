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
    public class TblProdGstCodeDtlsDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT tblProdGstCodeDtls.* , " +
                                  " CASE WHEN prodItemId IS NULL AND tblProdGstCodeDtls.prodClassId IS NULL THEN prodCateDesc + ' ' + prodSpecDesc + '-' + materialSubType " +
                                   "  WHEN tblProdGstCodeDtls.prodClassId IS NOT NULL" +
                                  "  THEN(select tblProdClassification.displayName from tblProdClassification " +
                                  "  where idProdClass = tblProdGstCodeDtls.prodClassId)" +
                                  " ELSE cat.prodClassDesc + '/' + subCat.prodClassDesc + '/' + spec.prodClassDesc + '/' + itemName END AS prodItemDesc " +
                                  " FROM tblProdGstCodeDtls " +
                                  " LEFT JOIN dimProdCat ON idProdCat = prodCatId LEFT JOIN dimProdSpec ON idProdSpec = prodSpecId " +
                                  " LEFT JOIN tblMaterial ON idMaterial = materialId LEFT JOIN tblProductItem ON idProdItem = prodItemId " +
                                  " LEFT JOIN tblProdClassification spec ON idProdClass = tblProductItem.prodClassId " +
                                  " LEFT JOIN tblProdClassification subCat ON subCat.idProdClass = spec.parentProdClassId " +
                                  " LEFT JOIN tblProdClassification cat ON cat.idProdClass = subCat.parentProdClassId " +
                                 "  LEFT JOIN tblProdClassification  SS oN SS.idProdClass = tblProdGstCodeDtls.prodClassId";


            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblProdGstCodeDtlsTO> SelectAllTblProdGstCodeDtls(Int32 gstCodeId ,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                if (gstCodeId == 0)
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblProdGstCodeDtls.isActive=1";
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblProdGstCodeDtls.isActive=1 AND gstCodeId=" + gstCodeId;

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProdGstCodeDtlsTO> list = ConvertDTToList(reader);
                return list;
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

        public static TblProdGstCodeDtlsTO SelectTblProdGstCodeDtls(Int32 idProdGstCode,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idProdGstCode = " + idProdGstCode + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction= tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProdGstCodeDtlsTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1) return list[0];
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


        public static List<TblProdGstCodeDtlsTO> SelectTblProdGstCodeDtls(String idProdGstCodes, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idProdGstCode IN ( " + idProdGstCodes + " )";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProdGstCodeDtlsTO> list = ConvertDTToList(reader);
                return list;
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

        public static TblProdGstCodeDtlsTO SelectTblProdGstCodeDtls(Int32 prodCatId, Int32 prodSpecId, Int32 materialId, Int32 prodItemId, Int32 prodClassId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblProdGstCodeDtls.isActive=1 AND ISNULL(materialId,0) = " + materialId + " AND ISNULL(prodCatId,0) = " + prodCatId + " AND ISNULL(prodSpecId,0) = " + prodSpecId + " AND ISNULL(prodItemId,0)=" + prodItemId + " AND ISNULL(tblProdGstCodeDtls.prodClassId,0)=" + prodClassId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProdGstCodeDtlsTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1) return list[0];
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

        public static List<TblProdGstCodeDtlsTO> ConvertDTToList(SqlDataReader tblProdGstCodeDtlsTODT)
        {
            List<TblProdGstCodeDtlsTO> tblProdGstCodeDtlsTOList = new List<TblProdGstCodeDtlsTO>();
            if (tblProdGstCodeDtlsTODT != null)
            {
                while (tblProdGstCodeDtlsTODT.Read())
                {
                    TblProdGstCodeDtlsTO tblProdGstCodeDtlsTONew = new TblProdGstCodeDtlsTO();
                    if (tblProdGstCodeDtlsTODT["idProdGstCode"] != DBNull.Value)
                        tblProdGstCodeDtlsTONew.IdProdGstCode = Convert.ToInt32(tblProdGstCodeDtlsTODT["idProdGstCode"].ToString());
                    if (tblProdGstCodeDtlsTODT["prodCatId"] != DBNull.Value)
                        tblProdGstCodeDtlsTONew.ProdCatId = Convert.ToInt32(tblProdGstCodeDtlsTODT["prodCatId"].ToString());
                    if (tblProdGstCodeDtlsTODT["prodSpecId"] != DBNull.Value)
                        tblProdGstCodeDtlsTONew.ProdSpecId = Convert.ToInt32(tblProdGstCodeDtlsTODT["prodSpecId"].ToString());
                    if (tblProdGstCodeDtlsTODT["gstCodeId"] != DBNull.Value)
                        tblProdGstCodeDtlsTONew.GstCodeId = Convert.ToInt32(tblProdGstCodeDtlsTODT["gstCodeId"].ToString());
                    if (tblProdGstCodeDtlsTODT["createdBy"] != DBNull.Value)
                        tblProdGstCodeDtlsTONew.CreatedBy = Convert.ToInt32(tblProdGstCodeDtlsTODT["createdBy"].ToString());
                    if (tblProdGstCodeDtlsTODT["updatedBy"] != DBNull.Value)
                        tblProdGstCodeDtlsTONew.UpdatedBy = Convert.ToInt32(tblProdGstCodeDtlsTODT["updatedBy"].ToString());
                    if (tblProdGstCodeDtlsTODT["effectiveFromDt"] != DBNull.Value)
                        tblProdGstCodeDtlsTONew.EffectiveFromDt = Convert.ToDateTime(tblProdGstCodeDtlsTODT["effectiveFromDt"].ToString());
                    if (tblProdGstCodeDtlsTODT["effectiveTodt"] != DBNull.Value)
                        tblProdGstCodeDtlsTONew.EffectiveTodt = Convert.ToDateTime(tblProdGstCodeDtlsTODT["effectiveTodt"].ToString());
                    if (tblProdGstCodeDtlsTODT["createdOn"] != DBNull.Value)
                        tblProdGstCodeDtlsTONew.CreatedOn = Convert.ToDateTime(tblProdGstCodeDtlsTODT["createdOn"].ToString());
                    if (tblProdGstCodeDtlsTODT["updatedOn"] != DBNull.Value)
                        tblProdGstCodeDtlsTONew.UpdatedOn = Convert.ToDateTime(tblProdGstCodeDtlsTODT["updatedOn"].ToString());
                    if (tblProdGstCodeDtlsTODT["remark"] != DBNull.Value)
                        tblProdGstCodeDtlsTONew.Remark = Convert.ToString(tblProdGstCodeDtlsTODT["remark"].ToString());

                    if (tblProdGstCodeDtlsTODT["prodItemId"] != DBNull.Value)
                        tblProdGstCodeDtlsTONew.ProdItemId = Convert.ToInt32(tblProdGstCodeDtlsTODT["prodItemId"].ToString());

                    if (tblProdGstCodeDtlsTODT["isActive"] != DBNull.Value)
                        tblProdGstCodeDtlsTONew.IsActive = Convert.ToInt32(tblProdGstCodeDtlsTODT["isActive"].ToString());
                    if (tblProdGstCodeDtlsTODT["materialId"] != DBNull.Value)
                        tblProdGstCodeDtlsTONew.MaterialId = Convert.ToInt32(tblProdGstCodeDtlsTODT["materialId"].ToString());
                    if (tblProdGstCodeDtlsTODT["prodItemDesc"] != DBNull.Value)
                        tblProdGstCodeDtlsTONew.ProdItemDesc = Convert.ToString(tblProdGstCodeDtlsTODT["prodItemDesc"].ToString());
                    if (tblProdGstCodeDtlsTODT["ProdClassId"] != DBNull.Value)
                        tblProdGstCodeDtlsTONew.ProdClassId = Convert.ToInt32(tblProdGstCodeDtlsTODT["prodClassId"].ToString());

                    tblProdGstCodeDtlsTOList.Add(tblProdGstCodeDtlsTONew);
                }
            }
            return tblProdGstCodeDtlsTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblProdGstCodeDtls(TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblProdGstCodeDtlsTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public static int InsertTblProdGstCodeDtls(TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblProdGstCodeDtlsTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblProdGstCodeDtls]( " +
                                "  [prodCatId]" +
                                " ,[prodSpecId]" +
                                " ,[gstCodeId]" +
                                " ,[createdBy]" +
                                " ,[updatedBy]" +
                                " ,[effectiveFromDt]" +
                                " ,[effectiveTodt]" +
                                " ,[createdOn]" +
                                " ,[updatedOn]" +
                                " ,[remark]" +
                                " ,[prodItemId]" +
                                " ,[isActive]" +
                                " ,[materialId]" +
                                " ,[prodClassId]" +
                                " )" +
                    " VALUES (" +
                                "  @ProdCatId " +
                                " ,@ProdSpecId " +
                                " ,@GstCodeId " +
                                " ,@CreatedBy " +
                                " ,@UpdatedBy " +
                                " ,@EffectiveFromDt " +
                                " ,@EffectiveTodt " +
                                " ,@CreatedOn " +
                                " ,@UpdatedOn " +
                                " ,@Remark " +
                                " ,@prodItemId " +
                                " ,@isActive " +
                                " ,@materialId " +
                                  "  ,@ProdClassId " +
                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdProdGstCode", System.Data.SqlDbType.Int).Value = tblProdGstCodeDtlsTO.IdProdGstCode;
            cmdInsert.Parameters.Add("@ProdCatId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdGstCodeDtlsTO.ProdCatId);
            cmdInsert.Parameters.Add("@ProdSpecId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdGstCodeDtlsTO.ProdSpecId);
            cmdInsert.Parameters.Add("@GstCodeId", System.Data.SqlDbType.Int).Value = tblProdGstCodeDtlsTO.GstCodeId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblProdGstCodeDtlsTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdGstCodeDtlsTO.UpdatedBy);
            cmdInsert.Parameters.Add("@EffectiveFromDt", System.Data.SqlDbType.DateTime).Value = tblProdGstCodeDtlsTO.EffectiveFromDt;
            cmdInsert.Parameters.Add("@EffectiveTodt", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdGstCodeDtlsTO.EffectiveTodt);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblProdGstCodeDtlsTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdGstCodeDtlsTO.UpdatedOn);
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdGstCodeDtlsTO.Remark);
            cmdInsert.Parameters.Add("@prodItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdGstCodeDtlsTO.ProdItemId);
            cmdInsert.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = tblProdGstCodeDtlsTO.IsActive;
            cmdInsert.Parameters.Add("@materialId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdGstCodeDtlsTO.MaterialId);
            cmdInsert.Parameters.Add("@ProdClassId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdGstCodeDtlsTO.ProdClassId);


            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblProdGstCodeDtlsTO.IdProdGstCode = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            return 0;
        }
        #endregion

        #region Updation
        public static int UpdateTblProdGstCodeDtls(TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblProdGstCodeDtlsTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public static int UpdateTblProdGstCodeDtls(TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblProdGstCodeDtlsTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteUpdationCommand(TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblProdGstCodeDtls] SET " +
                        "  [prodCatId]= @ProdCatId" +
                        " ,[prodSpecId]= @ProdSpecId" +
                        " ,[gstCodeId]= @GstCodeId" +
                        " ,[updatedBy]= @UpdatedBy" +
                        " ,[effectiveFromDt]= @EffectiveFromDt" +
                        " ,[effectiveTodt]= @EffectiveTodt" +
                        " ,[updatedOn]= @UpdatedOn" +
                        " ,[remark] = @Remark" +
                        " ,[prodItemId] = @prodItemId" +
                        " ,[isActive] = @isActive" +
                        " ,[materialId] = @materialId" +
                        " ,[prodClassId] = @ProdClassId" +

                        " WHERE [idProdGstCode] = @IdProdGstCode";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdProdGstCode", System.Data.SqlDbType.Int).Value = tblProdGstCodeDtlsTO.IdProdGstCode;
            cmdUpdate.Parameters.Add("@ProdCatId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdGstCodeDtlsTO.ProdCatId);
            cmdUpdate.Parameters.Add("@ProdSpecId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdGstCodeDtlsTO.ProdSpecId);
            cmdUpdate.Parameters.Add("@GstCodeId", System.Data.SqlDbType.Int).Value = tblProdGstCodeDtlsTO.GstCodeId;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblProdGstCodeDtlsTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@EffectiveFromDt", System.Data.SqlDbType.DateTime).Value = tblProdGstCodeDtlsTO.EffectiveFromDt;
            cmdUpdate.Parameters.Add("@EffectiveTodt", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdGstCodeDtlsTO.EffectiveTodt);
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblProdGstCodeDtlsTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdGstCodeDtlsTO.Remark);
            cmdUpdate.Parameters.Add("@prodItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdGstCodeDtlsTO.ProdItemId);
            cmdUpdate.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = tblProdGstCodeDtlsTO.IsActive;
            cmdUpdate.Parameters.Add("@materialId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdGstCodeDtlsTO.MaterialId);
            cmdUpdate.Parameters.Add("@ProdClassId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdGstCodeDtlsTO.ProdClassId);

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblProdGstCodeDtls(Int32 idProdGstCode)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idProdGstCode, cmdDelete);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public static int DeleteTblProdGstCodeDtls(Int32 idProdGstCode, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idProdGstCode, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idProdGstCode, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblProdGstCodeDtls] " +
            " WHERE idProdGstCode = " + idProdGstCode + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idProdGstCode", System.Data.SqlDbType.Int).Value = tblProdGstCodeDtlsTO.IdProdGstCode;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
