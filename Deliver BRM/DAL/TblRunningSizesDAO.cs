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
    public class TblRunningSizesDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT sizes.*,location.locationDesc,materialSubType " +
                                  " FROM tblRunningSizes sizes " +
                                  " LEFT JOIN tblLocation location ON sizes.locationId = location.idLocation " +
                                  " LEFT JOIN tblMaterial ON idMaterial = sizes.materialId ";

            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblRunningSizesTO> SelectAllTblRunningSizes()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblRunningSizesTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();
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

        public static List<TblRunningSizesTO> SelectAllTblRunningSizes(DateTime stockDate, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE stockDate=@stockDate";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@stockDate", System.Data.SqlDbType.DateTime).Value = stockDate.Date;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblRunningSizesTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();
                return list;
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

        public static TblRunningSizesTO SelectTblRunningSizes(Int32 idRunningSize)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idRunningSize = " + idRunningSize + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblRunningSizesTO> list = ConvertDTToList(reader);
                if (reader != null)
                    reader.Dispose();

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


        public static List<TblRunningSizesTO> ConvertDTToList(SqlDataReader tblRunningSizesTODT)
        {
            List<TblRunningSizesTO> tblRunningSizesTOList = new List<TblRunningSizesTO>();
            if (tblRunningSizesTODT != null)
            {
                while(tblRunningSizesTODT.Read())
                {
                    TblRunningSizesTO tblRunningSizesTONew = new TblRunningSizesTO();
                    if (tblRunningSizesTODT["idRunningSize"] != DBNull.Value)
                        tblRunningSizesTONew.IdRunningSize = Convert.ToInt32(tblRunningSizesTODT["idRunningSize"].ToString());
                    if (tblRunningSizesTODT["locationId"] != DBNull.Value)
                        tblRunningSizesTONew.LocationId = Convert.ToInt32(tblRunningSizesTODT["locationId"].ToString());
                    if (tblRunningSizesTODT["prodCatId"] != DBNull.Value)
                        tblRunningSizesTONew.ProdCatId = Convert.ToInt32(tblRunningSizesTODT["prodCatId"].ToString());
                    if (tblRunningSizesTODT["materialId"] != DBNull.Value)
                        tblRunningSizesTONew.MaterialId = Convert.ToInt32(tblRunningSizesTODT["materialId"].ToString());
                    if (tblRunningSizesTODT["prodSpecId"] != DBNull.Value)
                        tblRunningSizesTONew.ProdSpecId = Convert.ToInt32(tblRunningSizesTODT["prodSpecId"].ToString());
                    if (tblRunningSizesTODT["createdBy"] != DBNull.Value)
                        tblRunningSizesTONew.CreatedBy = Convert.ToInt32(tblRunningSizesTODT["createdBy"].ToString());
                    if (tblRunningSizesTODT["updatedBy"] != DBNull.Value)
                        tblRunningSizesTONew.UpdatedBy = Convert.ToInt32(tblRunningSizesTODT["updatedBy"].ToString());
                    if (tblRunningSizesTODT["stockDate"] != DBNull.Value)
                        tblRunningSizesTONew.StockDate = Convert.ToDateTime(tblRunningSizesTODT["stockDate"].ToString());
                    if (tblRunningSizesTODT["createdOn"] != DBNull.Value)
                        tblRunningSizesTONew.CreatedOn = Convert.ToDateTime(tblRunningSizesTODT["createdOn"].ToString());
                    if (tblRunningSizesTODT["updatedOn"] != DBNull.Value)
                        tblRunningSizesTONew.UpdatedOn = Convert.ToDateTime(tblRunningSizesTODT["updatedOn"].ToString());
                    if (tblRunningSizesTODT["noOfBundles"] != DBNull.Value)
                        tblRunningSizesTONew.NoOfBundles = Convert.ToDouble(tblRunningSizesTODT["noOfBundles"].ToString());
                    if (tblRunningSizesTODT["totalStock"] != DBNull.Value)
                        tblRunningSizesTONew.TotalStock = Convert.ToDouble(tblRunningSizesTODT["totalStock"].ToString());
                    if (tblRunningSizesTODT["locationDesc"] != DBNull.Value)
                        tblRunningSizesTONew.LocationName = Convert.ToString(tblRunningSizesTODT["locationDesc"].ToString());
                    if (tblRunningSizesTODT["materialSubType"] != DBNull.Value)
                        tblRunningSizesTONew.MaterialDesc = Convert.ToString(tblRunningSizesTODT["materialSubType"].ToString());
                    tblRunningSizesTOList.Add(tblRunningSizesTONew);
                }
            }
            return tblRunningSizesTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblRunningSizes(TblRunningSizesTO tblRunningSizesTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblRunningSizesTO, cmdInsert);
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

        public static int InsertTblRunningSizes(TblRunningSizesTO tblRunningSizesTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblRunningSizesTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblRunningSizesTO tblRunningSizesTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblRunningSizes]( " +
                            "  [locationId]" +
                            " ,[prodCatId]" +
                            " ,[materialId]" +
                            " ,[prodSpecId]" +
                            " ,[createdBy]" +
                            " ,[updatedBy]" +
                            " ,[stockDate]" +
                            " ,[createdOn]" +
                            " ,[updatedOn]" +
                            " ,[noOfBundles]" +
                            " ,[totalStock]" +
                            " )" +
                " VALUES (" +
                            "  @LocationId " +
                            " ,@ProdCatId " +
                            " ,@MaterialId " +
                            " ,@ProdSpecId " +
                            " ,@CreatedBy " +
                            " ,@UpdatedBy " +
                            " ,@StockDate " +
                            " ,@CreatedOn " +
                            " ,@UpdatedOn " +
                            " ,@NoOfBundles " +
                            " ,@TotalStock " +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdRunningSize", System.Data.SqlDbType.Int).Value = tblRunningSizesTO.IdRunningSize;
            cmdInsert.Parameters.Add("@LocationId", System.Data.SqlDbType.Int).Value = tblRunningSizesTO.LocationId;
            cmdInsert.Parameters.Add("@ProdCatId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblRunningSizesTO.ProdCatId);
            cmdInsert.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = tblRunningSizesTO.MaterialId;
            cmdInsert.Parameters.Add("@ProdSpecId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblRunningSizesTO.ProdSpecId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblRunningSizesTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblRunningSizesTO.UpdatedBy);
            cmdInsert.Parameters.Add("@StockDate", System.Data.SqlDbType.DateTime).Value = tblRunningSizesTO.StockDate;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblRunningSizesTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblRunningSizesTO.UpdatedOn);
            cmdInsert.Parameters.Add("@NoOfBundles", System.Data.SqlDbType.NVarChar).Value = tblRunningSizesTO.NoOfBundles;
            cmdInsert.Parameters.Add("@TotalStock", System.Data.SqlDbType.NVarChar).Value = tblRunningSizesTO.TotalStock;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblRunningSizesTO.IdRunningSize = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblRunningSizes(TblRunningSizesTO tblRunningSizesTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblRunningSizesTO, cmdUpdate);
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

        public static int UpdateTblRunningSizes(TblRunningSizesTO tblRunningSizesTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblRunningSizesTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblRunningSizesTO tblRunningSizesTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblRunningSizes] SET " +
                            "  [locationId]= @LocationId" +
                            " ,[prodCatId]= @ProdCatId" +
                            " ,[materialId]= @MaterialId" +
                            " ,[prodSpecId]= @ProdSpecId" +
                            " ,[updatedBy]= @UpdatedBy" +
                            " ,[stockDate]= @StockDate" +
                            " ,[updatedOn]= @UpdatedOn" +
                            " ,[noOfBundles]= @NoOfBundles" +
                            " ,[totalStock] = @TotalStock" +
                            " WHERE  [idRunningSize] = @IdRunningSize ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdRunningSize", System.Data.SqlDbType.Int).Value = tblRunningSizesTO.IdRunningSize;
            cmdUpdate.Parameters.Add("@LocationId", System.Data.SqlDbType.Int).Value = tblRunningSizesTO.LocationId;
            cmdUpdate.Parameters.Add("@ProdCatId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblRunningSizesTO.ProdCatId);
            cmdUpdate.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = tblRunningSizesTO.MaterialId;
            cmdUpdate.Parameters.Add("@ProdSpecId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblRunningSizesTO.ProdSpecId);
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblRunningSizesTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@StockDate", System.Data.SqlDbType.DateTime).Value = tblRunningSizesTO.StockDate;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblRunningSizesTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@NoOfBundles", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblRunningSizesTO.NoOfBundles);
            cmdUpdate.Parameters.Add("@TotalStock", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblRunningSizesTO.TotalStock);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblRunningSizes(Int32 idRunningSize)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idRunningSize, cmdDelete);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public static int DeleteTblRunningSizes(Int32 idRunningSize, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idRunningSize, cmdDelete);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public static int ExecuteDeletionCommand(Int32 idRunningSize, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblRunningSizes] " +
            " WHERE idRunningSize = " + idRunningSize +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idRunningSize", System.Data.SqlDbType.Int).Value = tblRunningSizesTO.IdRunningSize;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
