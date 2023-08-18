using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.DAL
{
    public class TblUnLoadingItemDetDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblUnLoadingItemDet]";
            return sqlSelectQry;
        }

        #endregion

        #region Selection
        public static List<TblUnLoadingItemDetTO> SelectAllTblUnLoadingItemDetails(int unLoadingId = 0)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                conn.Open();

                if (unLoadingId == 0)
                    cmdSelect.CommandText = SqlSelectQuery();
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE unLoadingId = " + unLoadingId;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUnLoadingItemDetTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllTblUnLoadingItemDetails");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblUnLoadingItemDetTO SelectTblUnLoadingItemDet(Int32 idUnloadingItemDet)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idUnloadingItemDet = " + idUnloadingItemDet + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUnLoadingItemDetTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();

                if (list != null && list.Count == 1)
                    return list[0];
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
        /// <summary>
        /// Vaibhav [13-Sep-2017] Added to select unloadingitem details of specific unloading slip
        /// </summary>
        /// <param name="unLoadingId"></param>
        /// <returns></returns>
        public static List<TblUnLoadingItemDetTO> SelectAllUnLoadingItemDetails(Int32 unLoadingId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE unLoadingId = " + unLoadingId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(sqlReader);

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllUnLoadingItemDetails");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblUnLoadingItemDetTO> ConvertDTToList(SqlDataReader tblUnLoadingItemDetTODT)
        {
            List<TblUnLoadingItemDetTO> tblUnLoadingItemDetTOList = new List<TblUnLoadingItemDetTO>();
            if (tblUnLoadingItemDetTODT != null)
            {
                while (tblUnLoadingItemDetTODT.Read())
                {
                    TblUnLoadingItemDetTO tblUnLoadingItemDetTONew = new TblUnLoadingItemDetTO();
                    if (tblUnLoadingItemDetTODT["idUnloadingItemDet"] != DBNull.Value)
                        tblUnLoadingItemDetTONew.IdUnloadingItemDet = Convert.ToInt32(tblUnLoadingItemDetTODT["idUnloadingItemDet"]);

                    if (tblUnLoadingItemDetTODT["unLoadingId"] != DBNull.Value)
                        tblUnLoadingItemDetTONew.UnLoadingId = Convert.ToInt32(tblUnLoadingItemDetTODT["unLoadingId"].ToString());
                    if (tblUnLoadingItemDetTODT["productCatId"] != DBNull.Value)
                        tblUnLoadingItemDetTONew.ProductCatId = Convert.ToInt32(tblUnLoadingItemDetTODT["productCatId"].ToString());
                    if (tblUnLoadingItemDetTODT["unLoadingQty"] != DBNull.Value)
                        tblUnLoadingItemDetTONew.UnLoadingQty = Convert.ToInt32(tblUnLoadingItemDetTODT["unLoadingQty"].ToString());
                    if (tblUnLoadingItemDetTODT["weightMeasurUnitId"] != DBNull.Value)
                        tblUnLoadingItemDetTONew.WeightMeasurUnitId = Convert.ToInt32(tblUnLoadingItemDetTODT["weightMeasurUnitId"].ToString());
                    if (tblUnLoadingItemDetTODT["weightedQty"] != DBNull.Value)
                        tblUnLoadingItemDetTONew.WeightedQty = Convert.ToDouble(tblUnLoadingItemDetTODT["weightedQty"]);
                    if (tblUnLoadingItemDetTODT["createdOn"] != DBNull.Value)
                        tblUnLoadingItemDetTONew.CreatedOn = Convert.ToDateTime(tblUnLoadingItemDetTODT["createdOn"]);
                    if (tblUnLoadingItemDetTODT["createdBy"] != DBNull.Value)
                        tblUnLoadingItemDetTONew.CreatedBy = Convert.ToInt32(tblUnLoadingItemDetTODT["createdBy"]);
                    if (tblUnLoadingItemDetTODT["updatedOn"] != DBNull.Value)
                        tblUnLoadingItemDetTONew.CreatedOn = Convert.ToDateTime(tblUnLoadingItemDetTODT["updatedOn"]);
                    if (tblUnLoadingItemDetTODT["updatedBy"] != DBNull.Value)
                        tblUnLoadingItemDetTONew.CreatedBy = Convert.ToInt32(tblUnLoadingItemDetTODT["updatedBy"]);
                    if (tblUnLoadingItemDetTODT["productCatName"] != DBNull.Value)
                        tblUnLoadingItemDetTONew.ProductCatName = tblUnLoadingItemDetTODT["productCatName"].ToString();

                    tblUnLoadingItemDetTOList.Add(tblUnLoadingItemDetTONew);
                }
            }
            return tblUnLoadingItemDetTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblUnLoadingItemDet(TblUnLoadingItemDetTO tblUnLoadingItemDetTO)
        {

            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblUnLoadingItemDetTO, cmdInsert);
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

        public static int InsertTblUnLoadingItemDet(TblUnLoadingItemDetTO tblUnLoadingItemDetTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblUnLoadingItemDetTO, cmdInsert);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblUnLoadingItemDet");
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(TblUnLoadingItemDetTO tblUnLoadingItemDetTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblUnLoadingItemDet]( " +
            " [unLoadingId]" +
            " ,[productCatId]" +
            " ,[weightMeasurUnitId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[unLoadingQty]" +
            " ,[weightedQty]" +
            " ,[productCatName]" +
            " )" +
" VALUES (" +
            " @UnLoadingId " +
            " ,@ProductCatId " +
            " ,@weightMeasurUnitId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@UnLoadingQty " +
            " ,@weightedQty " +
            " ,@productCatName " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@UnLoadingId", System.Data.SqlDbType.Int).Value = tblUnLoadingItemDetTO.UnLoadingId;
            cmdInsert.Parameters.Add("@ProductCatId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingItemDetTO.ProductCatId);
            cmdInsert.Parameters.Add("@weightMeasurUnitId", System.Data.SqlDbType.Int).Value = tblUnLoadingItemDetTO.WeightMeasurUnitId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingItemDetTO.CreatedBy);
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingItemDetTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingItemDetTO.CreatedOn);
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingItemDetTO.UpdatedOn);
            cmdInsert.Parameters.Add("@UnLoadingQty", System.Data.SqlDbType.Decimal).Value = Convert.ToDecimal(tblUnLoadingItemDetTO.UnLoadingQty);
            cmdInsert.Parameters.Add("@WeightedQty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingItemDetTO.WeightedQty);
            cmdInsert.Parameters.Add("@productCatName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingItemDetTO.ProductCatName);
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public static int UpdateTblUnLoadingItemDet(TblUnLoadingItemDetTO tblUnLoadingItemDetTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblUnLoadingItemDetTO, cmdUpdate);
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

        public static int UpdateTblUnLoadingItemDet(TblUnLoadingItemDetTO tblUnLoadingItemDetTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblUnLoadingItemDetTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateTblUnLoadingItemDet");
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteUpdationCommand(TblUnLoadingItemDetTO tblUnLoadingItemDetTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblUnLoadingItemDet] SET " +           
            " ,[unLoadingId]= @UnLoadingId" +
            " ,[productCatId]= @ProductCatId" +
            " ,[weightMeasurUnitId]= @weightMeasurUnitId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[unLoadingQty]= @UnLoadingQty" +
            " ,[weightedQty] = @WeightedQty" +
             " ,[productCatName] =@productCatName" +
            " WHERE [IdUnloadingItemDet] = @IdUnloadingItemDet ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdUnloadingItemDet", System.Data.SqlDbType.Int).Value = tblUnLoadingItemDetTO.IdUnloadingItemDet;
            cmdUpdate.Parameters.Add("@UnLoadingId", System.Data.SqlDbType.Int).Value = tblUnLoadingItemDetTO.UnLoadingId;
            cmdUpdate.Parameters.Add("@ProductCatId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingItemDetTO.ProductCatId);
            cmdUpdate.Parameters.Add("@weightMeasurUnitId", System.Data.SqlDbType.Int).Value = tblUnLoadingItemDetTO.WeightMeasurUnitId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingItemDetTO.CreatedBy);
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingItemDetTO.UpdatedBy);
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingItemDetTO.CreatedOn);
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingItemDetTO.UpdatedOn);
            cmdUpdate.Parameters.Add("@UnLoadingQty", System.Data.SqlDbType.NVarChar).Value = tblUnLoadingItemDetTO.UnLoadingQty;
            cmdUpdate.Parameters.Add("@WeightedQty", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingItemDetTO.WeightedQty);
            cmdUpdate.Parameters.Add("@productCatName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblUnLoadingItemDetTO.ProductCatName);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblUnLoadingItemDet(Int32 idUnloadingItemDet)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idUnloadingItemDet, cmdDelete);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public static int DeleteTblUnLoadingItemDet(Int32 idUnloadingItemDet, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idUnloadingItemDet, cmdDelete);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public static int ExecuteDeletionCommand(Int32 idUnloadingItemDet, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblUnLoadingItemDet] " +
            " WHERE idUnloadingItemDet = " + idUnloadingItemDet + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idUnloadingItemDet", System.Data.SqlDbType.Int).Value = tblUnLoadingItemDetTO.IdUnloadingItemDet;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
