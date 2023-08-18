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
    public class TblProductItemDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT tblProductItem.*, dimUnitMeasures.weightMeasurUnitDesc, dimUnitMeasures.mappedEInvoiceUOM FROM [tblProductItem] tblProductItem " +
                                  " LEFT JOIN dimUnitMeasures ON dimUnitMeasures.idWeightMeasurUnit = tblProductItem.weightMeasureUnitId"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblProductItemTO> SelectAllTblProductItem(Int32 specificationId = 0)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                if (specificationId == 0)
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblProductItem.isActive=1";
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblProductItem.isActive=1 AND prodClassId=" + specificationId;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductItemTO> list = ConvertDTToList(reader);
                return list;
            }
            catch(Exception ex)
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

        public static TblProductItemTO SelectTblProductItem(Int32 idProdItem,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idProdItem = " + idProdItem +" ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductItemTO> list = ConvertDTToList(reader);
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



        //sudhir[04-APR-2018] added for getlistof items whose stockupdate is require.
        public static List<TblProductItemTO> SelectProductItemListStockUpdateRequire(int isStockRequire)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                if (isStockRequire == 0)
                    cmdSelect.CommandText = " SELECT ProdClassification.displayName,* FROM [tblProductItem] ProductItem " +
                                            " INNER JOIN tblProdClassification ProdClassification ON" +
                                            " ProductItem.prodClassId = ProdClassification.idProdClass WHERE ProductItem.isActive=1 AND isStockRequire=0";
                else
                    cmdSelect.CommandText = " SELECT ProdClassification.displayName,* FROM [tblProductItem] ProductItem " +
                                            " INNER JOIN tblProdClassification ProdClassification ON" +
                                            " ProductItem.prodClassId = ProdClassification.idProdClass WHERE ProductItem.isActive=1 AND ProductItem.isStockRequire=1";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductItemTO> list = ConvertDTToListForUpdate(reader);
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

        public static List<TblProductItemTO> ConvertDTToListForUpdate(SqlDataReader tblProductItemTODT)
        {
            List<TblProductItemTO> tblProductItemTOList = new List<TblProductItemTO>();
            if (tblProductItemTODT != null)
            {
                while (tblProductItemTODT.Read())
                {
                    TblProductItemTO tblProductItemTONew = new TblProductItemTO();
                    if (tblProductItemTODT["idProdItem"] != DBNull.Value)
                        tblProductItemTONew.IdProdItem = Convert.ToInt32(tblProductItemTODT["idProdItem"].ToString());
                    if (tblProductItemTODT["prodClassId"] != DBNull.Value)
                        tblProductItemTONew.ProdClassId = Convert.ToInt32(tblProductItemTODT["prodClassId"].ToString());
                    if (tblProductItemTODT["createdBy"] != DBNull.Value)
                        tblProductItemTONew.CreatedBy = Convert.ToInt32(tblProductItemTODT["createdBy"].ToString());
                    if (tblProductItemTODT["updatedBy"] != DBNull.Value)
                        tblProductItemTONew.UpdatedBy = Convert.ToInt32(tblProductItemTODT["updatedBy"].ToString());
                    if (tblProductItemTODT["createdOn"] != DBNull.Value)
                        tblProductItemTONew.CreatedOn = Convert.ToDateTime(tblProductItemTODT["createdOn"].ToString());
                    if (tblProductItemTODT["updatedOn"] != DBNull.Value)
                        tblProductItemTONew.UpdatedOn = Convert.ToDateTime(tblProductItemTODT["updatedOn"].ToString());
                    if (tblProductItemTODT["itemName"] != DBNull.Value)
                        tblProductItemTONew.ItemName = Convert.ToString(tblProductItemTODT["itemName"].ToString());
                    if (tblProductItemTODT["itemDesc"] != DBNull.Value)
                        tblProductItemTONew.ItemDesc = Convert.ToString(tblProductItemTODT["itemDesc"].ToString());
                    if (tblProductItemTODT["remark"] != DBNull.Value)
                        tblProductItemTONew.Remark = Convert.ToString(tblProductItemTODT["remark"].ToString());
                    if (tblProductItemTODT["isActive"] != DBNull.Value)
                        tblProductItemTONew.IsActive = Convert.ToInt32(tblProductItemTODT["isActive"].ToString());
                    if (tblProductItemTODT["weightMeasureUnitId"] != DBNull.Value)
                        tblProductItemTONew.WeightMeasureUnitId = Convert.ToInt32(tblProductItemTODT["weightMeasureUnitId"]);
                    if (tblProductItemTODT["conversionUnitOfMeasure"] != DBNull.Value)
                        tblProductItemTONew.ConversionUnitOfMeasure = Convert.ToInt32(tblProductItemTODT["conversionUnitOfMeasure"]);
                    if (tblProductItemTODT["conversionFactor"] != DBNull.Value)
                        tblProductItemTONew.ConversionFactor = Convert.ToDouble(tblProductItemTODT["conversionFactor"]);
                    if (tblProductItemTODT["isStockRequire"] != DBNull.Value)
                        tblProductItemTONew.IsStockRequire = Convert.ToInt32(tblProductItemTODT["isStockRequire"].ToString());
                    if (tblProductItemTODT["displayName"] != DBNull.Value)
                        tblProductItemTONew.ProdClassDisplayName = tblProductItemTODT["displayName"].ToString();
                    if (tblProductItemTODT["isBaseItemForRate"] != DBNull.Value)
                        tblProductItemTONew.IsBaseItemForRate = Convert.ToInt32(tblProductItemTODT["isBaseItemForRate"].ToString());
                    if (tblProductItemTODT["isNonCommercialItem"] != DBNull.Value)
                        tblProductItemTONew.IsNonCommercialItem = Convert.ToInt32(tblProductItemTODT["isNonCommercialItem"].ToString());
                    //if (tblProductItemTODT["isParity"] != DBNull.Value)
                    //    tblProductItemTONew.IsParity = Convert.ToInt32(tblProductItemTODT["isParity"].ToString());

                    tblProductItemTOList.Add(tblProductItemTONew);
                }
            }
            return tblProductItemTOList;
        }


        //Sudhir[20-MAR-2018] Added for Get ProductItem List which has Parity Yes.
        public static List<DropDownTO> SelectProductItemListIsParity(Int32 isParity)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE isParity=" + isParity;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> list = new List<DropDownTO>();
                while (reader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (reader["idProdItem"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(reader["idProdItem"].ToString());
                    if (reader["itemName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(reader["itemName"].ToString());

                    list.Add(dropDownTONew);
                }
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
                conn.Close();
            }
        }



        //Sudhir[15-MAR-2018] Added for get List of ProductItem based on Category/SubCategory/Specification
        public static List<TblProductItemTO> SelectListOfProductItemTOOnprdClassId(string prodClassIds)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE prodClassId IN (" + prodClassIds + ")";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductItemTO> list = ConvertDTToList(reader);
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
                conn.Close();
            }
        }

        public static List<TblProductItemTO> ConvertDTToList(SqlDataReader tblProductItemTODT)
        {
            List<TblProductItemTO> tblProductItemTOList = new List<TblProductItemTO>();
            if (tblProductItemTODT != null)
            {
                while (tblProductItemTODT.Read())
                {
                    TblProductItemTO tblProductItemTONew = new TblProductItemTO();
                    if (tblProductItemTODT["idProdItem"] != DBNull.Value)
                        tblProductItemTONew.IdProdItem = Convert.ToInt32(tblProductItemTODT["idProdItem"].ToString());
                    if (tblProductItemTODT["prodClassId"] != DBNull.Value)
                        tblProductItemTONew.ProdClassId = Convert.ToInt32(tblProductItemTODT["prodClassId"].ToString());
                    if (tblProductItemTODT["createdBy"] != DBNull.Value)
                        tblProductItemTONew.CreatedBy = Convert.ToInt32(tblProductItemTODT["createdBy"].ToString());
                    if (tblProductItemTODT["updatedBy"] != DBNull.Value)
                        tblProductItemTONew.UpdatedBy = Convert.ToInt32(tblProductItemTODT["updatedBy"].ToString());
                    if (tblProductItemTODT["createdOn"] != DBNull.Value)
                        tblProductItemTONew.CreatedOn = Convert.ToDateTime(tblProductItemTODT["createdOn"].ToString());
                    if (tblProductItemTODT["updatedOn"] != DBNull.Value)
                        tblProductItemTONew.UpdatedOn = Convert.ToDateTime(tblProductItemTODT["updatedOn"].ToString());
                    if (tblProductItemTODT["itemName"] != DBNull.Value)
                        tblProductItemTONew.ItemName = Convert.ToString(tblProductItemTODT["itemName"].ToString());
                    if (tblProductItemTODT["itemDesc"] != DBNull.Value)
                        tblProductItemTONew.ItemDesc = Convert.ToString(tblProductItemTODT["itemDesc"].ToString());
                    if (tblProductItemTODT["remark"] != DBNull.Value)
                        tblProductItemTONew.Remark = Convert.ToString(tblProductItemTODT["remark"].ToString());
                    if (tblProductItemTODT["isActive"] != DBNull.Value)
                        tblProductItemTONew.IsActive = Convert.ToInt32(tblProductItemTODT["isActive"].ToString());
                    if (tblProductItemTODT["weightMeasureUnitId"] != DBNull.Value)
                        tblProductItemTONew.WeightMeasureUnitId = Convert.ToInt32(tblProductItemTODT["weightMeasureUnitId"]);
                    if (tblProductItemTODT["conversionUnitOfMeasure"] != DBNull.Value)
                        tblProductItemTONew.ConversionUnitOfMeasure = Convert.ToInt32(tblProductItemTODT["conversionUnitOfMeasure"]);
                    if (tblProductItemTODT["conversionFactor"] != DBNull.Value)
                        tblProductItemTONew.ConversionFactor = Convert.ToDouble(tblProductItemTODT["conversionFactor"]);
                    if (tblProductItemTODT["isStockRequire"] != DBNull.Value)
                        tblProductItemTONew.IsStockRequire = Convert.ToInt32(tblProductItemTODT["isStockRequire"].ToString());
                    if (tblProductItemTODT["isParity"] != DBNull.Value)
                        tblProductItemTONew.IsParity = Convert.ToInt32(tblProductItemTODT["isParity"].ToString());
                    if (tblProductItemTODT["basePrice"] != DBNull.Value)
                        tblProductItemTONew.BasePrice = Convert.ToDouble(tblProductItemTODT["basePrice"].ToString());
                    if (tblProductItemTODT["isBaseItemForRate"] != DBNull.Value)
                        tblProductItemTONew.IsBaseItemForRate = Convert.ToInt32(tblProductItemTODT["isBaseItemForRate"].ToString());
                    if (tblProductItemTODT["isNonCommercialItem"] != DBNull.Value)
                        tblProductItemTONew.IsNonCommercialItem = Convert.ToInt32(tblProductItemTODT["isNonCommercialItem"].ToString());
                    //Priyanka [22-05-2018] 
                    if (tblProductItemTODT["codeTypeId"] != DBNull.Value)
                        tblProductItemTONew.CodeTypeId = Convert.ToInt32(tblProductItemTODT["codeTypeId"].ToString());
                    //Dhananjay [11-05-2021] 
                    if (tblProductItemTODT["weightMeasurUnitDesc"] != DBNull.Value)
                        tblProductItemTONew.WeightMeasurUnitDesc = tblProductItemTODT["weightMeasurUnitDesc"].ToString();
                    if (tblProductItemTODT["mappedEInvoiceUOM"] != DBNull.Value)
                        tblProductItemTONew.MappedEInvoiceUOM = tblProductItemTODT["mappedEInvoiceUOM"].ToString();
                    tblProductItemTOList.Add(tblProductItemTONew);
                }
            }
            return tblProductItemTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblProductItem(TblProductItemTO tblProductItemTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblProductItemTO, cmdInsert);
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

        public static int InsertTblProductItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblProductItemTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblProductItemTO tblProductItemTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblProductItem]( " + 
                            "  [prodClassId]" +
                            " ,[createdBy]" +
                            " ,[updatedBy]" +
                            " ,[createdOn]" +
                            " ,[updatedOn]" +
                            " ,[itemName]" +
                            " ,[itemDesc]" +
                            " ,[remark]" +
                            " ,[isActive]" +
                              " ,[weightMeasureUnitId]" +
                            " ,[conversionUnitOfMeasure]" +
                            " ,[conversionFactor]" +
                            ", [isStockRequire]" +
                            ", [isParity]" +
                            ", [basePrice]" +
                            ", [codeTypeId]"+                       //Priyanka [22-05-2018]
                               ",[isBaseItemForRate]" +
                             ",[IsNonCommercialItem]" +

                            " )" +
                " VALUES (" +
                            "  @ProdClassId " +
                            " ,@CreatedBy " +
                            " ,@UpdatedBy " +
                            " ,@CreatedOn " +
                            " ,@UpdatedOn " +
                            " ,@ItemName " +
                            " ,@ItemDesc " +
                            " ,@Remark " +
                            " ,@isActive " +
                            " ,@weightMeasureUnitId " +
                            " ,@conversionUnitOfMeasure " +
                            " ,@conversionFactor " +
                            " ,@isStockRequire" +
                            " ,@IsParity" +
                            ", @BasePrice" +
                            ", @CodeTypeId"+                            //Priyanka [22-05-2018]
                              ", @isBase" +
                            ", @nonCommertial" +
                            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdProdItem", System.Data.SqlDbType.Int).Value = tblProductItemTO.IdProdItem;
            cmdInsert.Parameters.Add("@ProdClassId", System.Data.SqlDbType.Int).Value = tblProductItemTO.ProdClassId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblProductItemTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblProductItemTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.UpdatedOn);
            cmdInsert.Parameters.Add("@ItemName", System.Data.SqlDbType.NVarChar).Value = tblProductItemTO.ItemName;
            cmdInsert.Parameters.Add("@ItemDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ItemDesc);
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.Remark);
            cmdInsert.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = tblProductItemTO.IsActive;
            cmdInsert.Parameters.Add("@weightMeasureUnitId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.WeightMeasureUnitId);
            cmdInsert.Parameters.Add("@conversionUnitOfMeasure", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ConversionUnitOfMeasure);
            cmdInsert.Parameters.Add("@conversionFactor", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ConversionFactor);
            cmdInsert.Parameters.Add("@isStockRequire", System.Data.SqlDbType.Int).Value = tblProductItemTO.IsStockRequire;
            cmdInsert.Parameters.Add("@isParity", System.Data.SqlDbType.Int).Value = tblProductItemTO.IsParity;
            cmdInsert.Parameters.Add("@BasePrice", System.Data.SqlDbType.Decimal).Value = tblProductItemTO.BasePrice;
            cmdInsert.Parameters.Add("@CodeTypeId", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.CodeTypeId);          //Priyanka [22-05-2018]
            cmdInsert.Parameters.Add("@isBase", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsBaseItemForRate);
            cmdInsert.Parameters.Add("@nonCommertial", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsNonCommercialItem);
            if (cmdInsert.ExecuteNonQuery()==1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblProductItemTO.IdProdItem = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }

            return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblProductItem(TblProductItemTO tblProductItemTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblProductItemTO, cmdUpdate);
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

        //remove previous Base Items while adding or updating new
        public static int updatePreviousBase(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblProductItem] SET " +
                                " [isBaseItemForRate] = " + 0 +                //Priyanka [16-05-2018] 
                                " WHERE [isBaseItemForRate] = " + 1;

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                return cmdUpdate.ExecuteNonQuery();
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


        public static int UpdateTblProductItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblProductItemTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblProductItemTO tblProductItemTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblProductItem] SET " + 
                                "  [prodClassId]= @ProdClassId" +
                                " ,[updatedBy]= @UpdatedBy" +
                                " ,[updatedOn]= @UpdatedOn" +
                                " ,[itemName]= @ItemName" +
                                " ,[itemDesc]= @ItemDesc" +
                                " ,[remark] = @Remark" +
                                " ,[isActive] = @isActive" +
                                " ,[weightMeasureUnitId] = @weightMeasureUnitId " +
                                " ,[conversionUnitOfMeasure] = @conversionUnitOfMeasure " +
                                " ,[conversionFactor] = @conversionFactor " +
                                " ,[isStockRequire] = @isStockRequire " +
                                " ,[isParity] = @IsParity " +
                                " ,[basePrice] = @BasePrice " +
                                " ,[codeTypeId] = @CodeTypeId" +                //Priyanka [22-05-2018]
                                      " ,[isBaseItemForRate] = @isBase" +
                                " ,[isNonCommercialItem] = @nonCommertial" +
                                " WHERE [idProdItem] = @IdProdItem "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdProdItem", System.Data.SqlDbType.Int).Value = tblProductItemTO.IdProdItem;
            cmdUpdate.Parameters.Add("@ProdClassId", System.Data.SqlDbType.Int).Value = tblProductItemTO.ProdClassId;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblProductItemTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblProductItemTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@ItemName", System.Data.SqlDbType.NVarChar).Value = tblProductItemTO.ItemName;
            cmdUpdate.Parameters.Add("@ItemDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ItemDesc);
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.Remark);
            cmdUpdate.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = tblProductItemTO.IsActive;
            cmdUpdate.Parameters.Add("@weightMeasureUnitId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.WeightMeasureUnitId);
            cmdUpdate.Parameters.Add("@conversionUnitOfMeasure", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ConversionUnitOfMeasure);
            cmdUpdate.Parameters.Add("@conversionFactor", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ConversionFactor);
            cmdUpdate.Parameters.Add("@IsParity", System.Data.SqlDbType.Int).Value = tblProductItemTO.IsParity;
            cmdUpdate.Parameters.Add("@BasePrice", System.Data.SqlDbType.Decimal).Value = tblProductItemTO.BasePrice;
            cmdUpdate.Parameters.Add("@isStockRequire", System.Data.SqlDbType.Int).Value = tblProductItemTO.IsStockRequire;
            cmdUpdate.Parameters.Add("@CodeTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.CodeTypeId);
            cmdUpdate.Parameters.Add("@isBase", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsBaseItemForRate);
            cmdUpdate.Parameters.Add("@nonCommertial", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsNonCommercialItem);
            return cmdUpdate.ExecuteNonQuery();
        }
        //Priyanka [22-05-2018] : Added to update  product item tax type.
        public static int UpdateTblProductItemTaxType(String idClassStr, Int32 codeTypeId, Int32 isActive, SqlConnection conn, SqlTransaction tran)
        {

            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdateTblProductItemTaxType(idClassStr, codeTypeId, isActive,cmdUpdate);
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
        public static int ExecuteUpdateTblProductItemTaxType(String idClassStr, Int32 codeTypeId, Int32 isActive, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblProductItem] SET " +
                                " [codeTypeId] = " + codeTypeId + "," + "[isActive] =" + isActive +               //Priyanka [16-05-2018] 
                                " WHERE [prodClassId] IN (" + idClassStr + ")";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblProductItem(Int32 idProdItem)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idProdItem, cmdDelete);
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

        public static int DeleteTblProductItem(Int32 idProdItem, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idProdItem, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idProdItem, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblProductItem] " +
            " WHERE idProdItem = " + idProdItem +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idProdItem", System.Data.SqlDbType.Int).Value = tblProductItemTO.IdProdItem;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
