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
    public class TblPurchaseParityDetailsDAO : ITblPurchaseParityDetailsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPurchaseParityDetailsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods

        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT parity.*,tblProductItem.isNonCommercialItem,tbluserCreatedBy.userDisplayName as createdByName " +
                                  " FROM [tblParityDetailsPurchase] parity" +
                                  //" LEFT JOIN dimProdCat prodCat ON parity.prodCatId = prodCat.idProdCat" +
                                  //" LEFT JOIN dimProdSpec prodSpec ON parity.prodSpecId = prodSpec.idProdSpec" +
                                  //" LEFT JOIN tblMaterial material ON parity.prodItemPurchaseId=material.idMaterial" +
                                  " LEFT JOIN tblParitySummaryPurchase tblParitySummaryPurchase ON parity.parityPurchaseId=tblParitySummaryPurchase.idParityPurchase" +
                                  " LEFT JOIN tblProductItem tblProductItem ON  tblProductItem.idProdItem =  parity.prodItemId " +
                                  " LEFT JOIN tblUser tbluserCreatedBy on tbluserCreatedBy.idUser = tblParitySummaryPurchase.createdBy ";

            return sqlSelectQry;
        }

        #endregion

        #region Selection

        public List<TblPurchaseParityDetailsTO> SelectAllTblParityDetails(int parityId, Int32 prodSpecId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                if (prodSpecId == 0)
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE parityId=" + parityId;
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE parityId=" + parityId + " AND parity.prodSpecId=" + prodSpecId;

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToListNew(sqlReader);

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public List<TblPurchaseParityDetailsTO> SelectAllTblParityDetails(String parityIds, Int32 prodSpecId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                if (prodSpecId == 0)
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE parityId IN (" + parityIds + ")";
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE parityId IN (" + parityIds + ") AND parity.prodSpecId=" + prodSpecId;

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToListNew(sqlReader);

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        //public  List<TblParityDetailsTO> SelectAllParityDetailsListByIds(String parityDtlIds, SqlConnection conn, SqlTransaction tran)
        //{
        //    SqlCommand cmdSelect = new SqlCommand();
        //    SqlDataReader sqlReader = null;
        //    try
        //    {
        //        cmdSelect.CommandText = SqlSelectQuery() + " WHERE parity.idParityDtl In(" + parityDtlIds + ")";

        //        cmdSelect.Connection = conn;
        //        cmdSelect.Transaction = tran;
        //        cmdSelect.CommandType = System.Data.CommandType.Text;

        //        sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
        //        return ConvertDTToList(sqlReader);

        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        if (sqlReader != null)
        //            sqlReader.Dispose();
        //        cmdSelect.Dispose();
        //    }
        //}

        public List<TblPurchaseParityDetailsTO> SelectAllLatestParityDetails(Int32 stateId, Int32 prodSpecId, Int32 brandId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            String sqlQuery = string.Empty;
            try
            {
                if (prodSpecId == 0)
                {
                    sqlQuery = " SELECT  idProdItem as materialId,prodClassDesc,idParityDtlPurchase,latestParity.createdBy,latestParity.createdOn,parityAmt,b.idProdItem,b.itemName,b.itemDesc,b.prodClassId," +
                                           " [recovery],nonConfParityAmt,latestParity.remark,prodClassDesc as materialDesc,prodCatId,'' as prodCateDesc,prodSpecId,'' as prodSpecDesc," +
                                           " '0' as brandId,latestParity.idParityDtlPurchase,latestParity.parityPurchaseId" +
                                           " FROM tblProdClassification a " +
                                           " inner join  tblProductItem b " +
                                           " on a.idProdClass = b.prodClassId" +
                                           " LEFT JOIN " +
                                           " ( " +
                                           "     SELECT parityDtl.* FROM tblParityDetailsPurchase parityDtl " +
                                           "     INNER JOIN tblParitySummaryPurchase paritySum " +
                                           "     ON parityDtl.parityPurchaseId = paritySum.idParityPurchase " +
                                           "     WHERE paritySum.idParityPurchase = (SELECT MAX(idParityPurchase) FROM tblParitySummaryPurchase WHERE stateId=" + stateId + ") " +
                                           " ) latestParity " +
                                           " ON b.idProdItem = latestParity.prodItemId ";// +
                                                                                         //" WHERE a.itemProdCatId = '6'";

                }
                else
                {
                    sqlQuery = " SELECT  idProdItem as materialId,prodClassDesc,idParityDtlPurchase,latestParity.createdBy,latestParity.createdOn,parityAmt,b.idProdItem,b.itemName,b.itemDesc,b.prodClassId," +
                                           " [recovery],nonConfParityAmt,latestParity.remark,prodClassDesc as materialDesc,prodCatId,'' as prodCateDesc,prodSpecId,'' as prodSpecDesc," +
                                           " '0' as brandId,latestParity.idParityDtlPurchase,latestParity.parityPurchaseId " +
                                           " FROM tblProdClassification a " +
                                           " inner join  tblProductItem b " +
                                           " on a.idProdClass = b.prodClassId" +
                                           " LEFT JOIN " +
                                           " ( " +
                                           "     SELECT parityDtl.* FROM tblParityDetailsPurchase parityDtl " +
                                           "     INNER JOIN tblParitySummaryPurchase paritySum " +
                                           "     ON parityDtl.parityPurchaseId = paritySum.idParityPurchase " +
                                           "     WHERE paritySum.idParityPurchase = (SELECT MAX(idParityPurchase) FROM tblParitySummaryPurchase WHERE stateId=" + stateId + ") " +
                                           " ) latestParity " +
                                           " ON b.idProdItem = latestParity.prodItemId " +
                                           //" WHERE a.itemProdCatId = '6' AND a.idProdClass=" + prodSpecId;
                                           " WHERE  b.prodClassId=" + prodSpecId;

                }

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(sqlReader);

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public List<TblPurchaseParityDetailsTO> SelectAllLatestParityDetails(Int32 stateId, Int32 prodItemId)
        {
            SqlCommand cmdSelect = new SqlCommand();
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader sqlReader = null;
            String sqlQuery = string.Empty;
            try
            {

                {
                    sqlQuery = " SELECT  idProdItem as materialId,prodClassDesc,idParityDtlPurchase,latestParity.createdBy,latestParity.createdOn,parityAmt,b.idProdItem,b.itemName,b.itemDesc,b.prodClassId," +
                                           " [recovery],nonConfParityAmt,latestParity.remark,prodClassDesc as materialDesc,prodCatId,'' as prodCateDesc,prodSpecId,'' as prodSpecDesc," +
                                           " '0' as brandId,latestParity.idParityDtlPurchase,latestParity.parityPurchaseId" +
                                           " FROM tblProdClassification a " +
                                           " inner join  tblProductItem b " +
                                           " on a.idProdClass = b.prodClassId" +
                                           " LEFT JOIN " +
                                           " ( " +
                                           "     SELECT parityDtl.* FROM tblParityDetailsPurchase parityDtl " +
                                           "     INNER JOIN tblParitySummaryPurchase paritySum " +
                                           "     ON parityDtl.parityPurchaseId = paritySum.idParityPurchase " +
                                           "     WHERE paritySum.idParityPurchase = (SELECT MAX(idParityPurchase) FROM tblParitySummaryPurchase WHERE stateId=" + stateId + ") " +
                                           " ) latestParity " +
                                           " ON b.idProdItem = latestParity.prodItemId ";// +
                                                                                         //" WHERE a.itemProdCatId = '6'";

                }


                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(sqlReader);

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
                conn.Close();
            }
        }

        public List<TblPurchaseParityDetailsTO> GetBookingItemsParityDtls(Int32 prodItemId, DateTime createdOn)
        {
            SqlCommand cmdSelect = new SqlCommand();
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader sqlReader = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();
                sqlQuery = SqlSelectQuery() + " where prodItemId=" + prodItemId + " and tblParitySummaryPurchase.createdOn <= @date ORDER BY tblParitySummaryPurchase.createdOn  DESC";
                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@date", System.Data.SqlDbType.DateTime).Value = createdOn;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToListNew(sqlReader);

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
                conn.Close();
            }
        }
        public List<TblPurchaseParityDetailsTO> GetBookingItemsParityDtls(string prodItemIds, DateTime createdOn, Int32 stateId)
        {
            SqlCommand cmdSelect = new SqlCommand();
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader sqlReader = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();
                sqlQuery = SqlSelectQuery() + " where prodItemId in ( " + prodItemIds + " ) and tblParitySummaryPurchase.createdOn <= @date " +
                          " AND tblParitySummaryPurchase.stateId = " + stateId +
                         " ORDER BY tblParitySummaryPurchase.createdOn  DESC";
                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@date", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(createdOn);

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToListNew(sqlReader);

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
                conn.Close();
            }
        }

         public List<TblPurchaseParityDetailsTO> GetBookingItemsParityDtls(string prodItemIds, DateTime createdOn, Int32 stateId,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            String sqlQuery = string.Empty;
            try
            {

                sqlQuery = SqlSelectQuery() + " where prodItemId in ( " + prodItemIds + " ) and tblParitySummaryPurchase.createdOn <= @date " +
                          " AND tblParitySummaryPurchase.stateId = " + stateId +
                         " ORDER BY tblParitySummaryPurchase.createdOn  DESC";
                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@date", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(createdOn);

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToListNew(sqlReader);

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public List<TblPurchaseParityDetailsTO> ConvertDTToList(SqlDataReader tblPurchaseParityDetailsTODT)
        {
            List<TblPurchaseParityDetailsTO> tblParityDetailsTOList = new List<TblPurchaseParityDetailsTO>();
            if (tblPurchaseParityDetailsTODT != null)
            {
                if (tblPurchaseParityDetailsTODT.HasRows)
                {
                    while (tblPurchaseParityDetailsTODT.Read())
                    {
                        TblPurchaseParityDetailsTO tblParityDetailsTONew = new TblPurchaseParityDetailsTO();
                        if (tblPurchaseParityDetailsTODT["idParityDtlPurchase"] != DBNull.Value)
                            tblParityDetailsTONew.IdParityDtl = Convert.ToInt32(tblPurchaseParityDetailsTODT["idParityDtlPurchase"].ToString());
                        if (tblPurchaseParityDetailsTODT["parityPurchaseId"] != DBNull.Value)
                            tblParityDetailsTONew.ParityId = Convert.ToInt32(tblPurchaseParityDetailsTODT["parityPurchaseId"].ToString());
                        if (tblPurchaseParityDetailsTODT["prodItemId"] != DBNull.Value)
                            tblParityDetailsTONew.ProdItemId = Convert.ToInt32(tblPurchaseParityDetailsTODT["prodItemId"].ToString());
                        if (tblPurchaseParityDetailsTODT["createdBy"] != DBNull.Value)
                            tblParityDetailsTONew.CreatedBy = Convert.ToInt32(tblPurchaseParityDetailsTODT["createdBy"].ToString());
                        if (tblPurchaseParityDetailsTODT["createdOn"] != DBNull.Value)
                            tblParityDetailsTONew.CreatedOn = Convert.ToDateTime(tblPurchaseParityDetailsTODT["createdOn"].ToString());
                        if (tblPurchaseParityDetailsTODT["parityAmt"] != DBNull.Value)
                            tblParityDetailsTONew.ParityAmt = Convert.ToDouble(tblPurchaseParityDetailsTODT["parityAmt"].ToString());
                        if (tblPurchaseParityDetailsTODT["nonConfParityAmt"] != DBNull.Value)
                            tblParityDetailsTONew.NonConfParityAmt = Convert.ToDouble(tblPurchaseParityDetailsTODT["nonConfParityAmt"].ToString());
                        if (tblPurchaseParityDetailsTODT["recovery"] != DBNull.Value)
                            tblParityDetailsTONew.ParityRecoveryAmt = Convert.ToDouble(tblPurchaseParityDetailsTODT["recovery"].ToString());
                        if (tblPurchaseParityDetailsTODT["remark"] != DBNull.Value)
                            tblParityDetailsTONew.Remark = Convert.ToString(tblPurchaseParityDetailsTODT["remark"].ToString());
                        if (tblPurchaseParityDetailsTODT["prodClassId"] != DBNull.Value)
                            tblParityDetailsTONew.ProdCatId = Convert.ToInt32(tblPurchaseParityDetailsTODT["prodClassId"].ToString());
                        if (tblPurchaseParityDetailsTODT["itemDesc"] != DBNull.Value)
                            tblParityDetailsTONew.ProdCatDesc = Convert.ToString(tblPurchaseParityDetailsTODT["itemDesc"].ToString());
                        if (tblPurchaseParityDetailsTODT["materialDesc"] != DBNull.Value)
                            tblParityDetailsTONew.MaterialDesc = Convert.ToString(tblPurchaseParityDetailsTODT["materialDesc"].ToString());
                        if (tblPurchaseParityDetailsTODT["recovery"] != DBNull.Value)
                            tblParityDetailsTONew.Recovery = Convert.ToDouble(tblPurchaseParityDetailsTODT["recovery"].ToString());


                        tblParityDetailsTOList.Add(tblParityDetailsTONew);
                    }
                }

            }

            return tblParityDetailsTOList;
        }

        public List<TblPurchaseParityDetailsTO> ConvertDTToListNew(SqlDataReader tblPurchaseParityDetailsTODT)
        {
            List<TblPurchaseParityDetailsTO> tblParityDetailsTOList = new List<TblPurchaseParityDetailsTO>();
            if (tblPurchaseParityDetailsTODT != null)
            {
                //if (tblPurchaseParityDetailsTODT.HasRows)
                {
                    while (tblPurchaseParityDetailsTODT.Read())
                    {
                        TblPurchaseParityDetailsTO tblParityDetailsTONew = new TblPurchaseParityDetailsTO();
                        if (tblPurchaseParityDetailsTODT["idParityDtlPurchase"] != DBNull.Value)
                            tblParityDetailsTONew.IdParityDtl = Convert.ToInt32(tblPurchaseParityDetailsTODT["idParityDtlPurchase"].ToString());
                        if (tblPurchaseParityDetailsTODT["parityPurchaseId"] != DBNull.Value)
                            tblParityDetailsTONew.ParityId = Convert.ToInt32(tblPurchaseParityDetailsTODT["parityPurchaseId"].ToString());
                        if (tblPurchaseParityDetailsTODT["prodItemId"] != DBNull.Value)
                            tblParityDetailsTONew.ProdItemId = Convert.ToInt32(tblPurchaseParityDetailsTODT["prodItemId"].ToString());
                        if (tblPurchaseParityDetailsTODT["createdBy"] != DBNull.Value)
                            tblParityDetailsTONew.CreatedBy = Convert.ToInt32(tblPurchaseParityDetailsTODT["createdBy"].ToString());
                        if (tblPurchaseParityDetailsTODT["createdOn"] != DBNull.Value)
                            tblParityDetailsTONew.CreatedOn = Convert.ToDateTime(tblPurchaseParityDetailsTODT["createdOn"].ToString());
                        if (tblPurchaseParityDetailsTODT["parityAmt"] != DBNull.Value)
                            tblParityDetailsTONew.ParityAmt = Convert.ToDouble(tblPurchaseParityDetailsTODT["parityAmt"].ToString());
                        if (tblPurchaseParityDetailsTODT["nonConfParityAmt"] != DBNull.Value)
                            tblParityDetailsTONew.NonConfParityAmt = Convert.ToDouble(tblPurchaseParityDetailsTODT["nonConfParityAmt"].ToString());
                        if (tblPurchaseParityDetailsTODT["recovery"] != DBNull.Value)
                            tblParityDetailsTONew.ParityRecoveryAmt = Convert.ToDouble(tblPurchaseParityDetailsTODT["recovery"].ToString());
                        if (tblPurchaseParityDetailsTODT["recovery"] != DBNull.Value)
                            tblParityDetailsTONew.Recovery = Convert.ToDouble(tblPurchaseParityDetailsTODT["recovery"].ToString());
                        if (tblPurchaseParityDetailsTODT["isNonCommercialItem"] != DBNull.Value)
                            tblParityDetailsTONew.IsNonCommercialItem = Convert.ToInt32(tblPurchaseParityDetailsTODT["isNonCommercialItem"].ToString());
                        if (tblPurchaseParityDetailsTODT["createdByName"] != DBNull.Value)
                            tblParityDetailsTONew.CreatedByName = Convert.ToString(tblPurchaseParityDetailsTODT["createdByName"].ToString());


                        tblParityDetailsTOList.Add(tblParityDetailsTONew);
                    }
                }

            }

            return tblParityDetailsTOList;
        }


        #endregion

        #region Insertion

        public int InsertTblParityDetails(TblPurchaseParityDetailsTO tblParityDetailsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblParityDetailsTO, cmdInsert);
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

        public int InsertTblParityDetails(TblPurchaseParityDetailsTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblParityDetailsTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblPurchaseParityDetailsTO tblParityDetailsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblParityDetailsPurchase]( " +
                            "  [parityPurchaseId]" +
                            " ,[prodItemId]" +
                            " ,[parityAmt]" +
                            " ,[nonConfParityAmt]" +
                            " ,[recovery]" +
                            " ,[createdOn]" +
                            " ,[createdBy]" +
                            " )" +
                " VALUES (" +
                            "  @ParityId " +
                            " ,@ProdItemId " +
                            " ,@ParityAmt " +
                            " ,@NonConfParityAmt " +
                            " ,@recovery" +
                            " ,@CreatedOn " +
                            " ,@CreatedBy " +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdParityDtl", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.IdParityDtl;
            cmdInsert.Parameters.Add("@ParityId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.ParityId;
            cmdInsert.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.ProdItemId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblParityDetailsTO.CreatedOn;
            cmdInsert.Parameters.Add("@ParityAmt", System.Data.SqlDbType.NVarChar).Value = tblParityDetailsTO.ParityAmt;
            cmdInsert.Parameters.Add("@NonConfParityAmt", System.Data.SqlDbType.NVarChar).Value = tblParityDetailsTO.NonConfParityAmt;
            cmdInsert.Parameters.Add("@recovery", System.Data.SqlDbType.NVarChar).Value = tblParityDetailsTO.ParityRecoveryAmt;//Recovery;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblParityDetailsTO.IdParityDtl = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }


        public int InsertProductImgDetails(SaveProductImgTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            // String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            // //SqlConnection conn = new SqlConnection(sqlConnStr);
            // SqlCommand cmdInsert = new SqlCommand();
            // try
            // {
            //     if (conn.State == ConnectionState.Open) conn.Close();
            //     conn.Open();
            //     cmdInsert.Connection = conn;
            //     return ExecuteInsertionCommandNew(tblParityDetailsTO, cmdInsert);
            // }
            // catch (Exception ex)
            // {
            return -1;
            // }
            // finally
            // {
            //     conn.Close();
            //     cmdInsert.Dispose();
            // }
        }




        #endregion

        //#region Updation

        //public  int UpdateTblParityDetails(TblParityDetailsTO tblParityDetailsTO)
        //{
        //    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
        //    SqlConnection conn = new SqlConnection(sqlConnStr);
        //    SqlCommand cmdUpdate = new SqlCommand();
        //    try
        //    {
        //        conn.Open();
        //        cmdUpdate.Connection = conn;
        //        return ExecuteUpdationCommand(tblParityDetailsTO, cmdUpdate);
        //    }
        //    catch (Exception ex)
        //    {
        //        return -1;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //        cmdUpdate.Dispose();
        //    }
        //}

        //public  int UpdateTblParityDetails(TblParityDetailsTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran)
        //{
        //    SqlCommand cmdUpdate = new SqlCommand();
        //    try
        //    {
        //        cmdUpdate.Connection = conn;
        //        cmdUpdate.Transaction = tran;
        //        return ExecuteUpdationCommand(tblParityDetailsTO, cmdUpdate);
        //    }
        //    catch (Exception ex)
        //    {
        //        return -1;
        //    }
        //    finally
        //    {
        //        cmdUpdate.Dispose();
        //    }
        //}

        //public  int ExecuteUpdationCommand(TblParityDetailsTO tblParityDetailsTO, SqlCommand cmdUpdate)
        //{
        //    String sqlQuery = @" UPDATE [tblParityDetails] SET " +
        //                    "  [parityId]= @ParityId" +
        //                    " ,[materialId]= @MaterialId" +
        //                    " ,[createdBy]= @CreatedBy" +
        //                    " ,[createdOn]= @CreatedOn" +
        //                    " ,[parityAmt]= @ParityAmt" +
        //                    " ,[nonConfParityAmt]= @NonConfParityAmt" +
        //                    " ,[remark] = @Remark" +
        //                    " ,[prodCatId] = @prodCatId" +
        //                    " ,[prodSpecId] = @prodSpecId" +
        //                    " WHERE [idParityDtl] = @IdParityDtl ";

        //    cmdUpdate.CommandText = sqlQuery;
        //    cmdUpdate.CommandType = System.Data.CommandType.Text;

        //    cmdUpdate.Parameters.Add("@IdParityDtl", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.IdParityDtl;
        //    cmdUpdate.Parameters.Add("@ParityId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.ParityId;
        //    cmdUpdate.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.MaterialId;
        //    cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.CreatedBy;
        //    cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblParityDetailsTO.CreatedOn;
        //    cmdUpdate.Parameters.Add("@ParityAmt", System.Data.SqlDbType.NVarChar).Value = tblParityDetailsTO.ParityAmt;
        //    cmdUpdate.Parameters.Add("@NonConfParityAmt", System.Data.SqlDbType.NVarChar).Value = tblParityDetailsTO.NonConfParityAmt;
        //    cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.Remark);
        //    cmdUpdate.Parameters.Add("@prodCatId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.ProdCatId;
        //    cmdUpdate.Parameters.Add("@prodSpecId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.ProdSpecId;
        //    return cmdUpdate.ExecuteNonQuery();
        //}

        //#endregion

        //#region Deletion

        //public  int DeleteTblParityDetails(Int32 idParityDtl)
        //{
        //    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
        //    SqlConnection conn = new SqlConnection(sqlConnStr);
        //    SqlCommand cmdDelete = new SqlCommand();
        //    try
        //    {
        //        conn.Open();
        //        cmdDelete.Connection = conn;
        //        return ExecuteDeletionCommand(idParityDtl, cmdDelete);
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //        cmdDelete.Dispose();
        //    }
        //}

        //public  int DeleteTblParityDetails(Int32 idParityDtl, SqlConnection conn, SqlTransaction tran)
        //{
        //    SqlCommand cmdDelete = new SqlCommand();
        //    try
        //    {
        //        cmdDelete.Connection = conn;
        //        cmdDelete.Transaction = tran;
        //        return ExecuteDeletionCommand(idParityDtl, cmdDelete);
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }
        //    finally
        //    {
        //        cmdDelete.Dispose();
        //    }
        //}

        //public  int ExecuteDeletionCommand(Int32 idParityDtl, SqlCommand cmdDelete)
        //{
        //    cmdDelete.CommandText = " DELETE FROM [tblParityDetails] " +
        //                             " WHERE idParityDtl = " + idParityDtl + "";
        //    cmdDelete.CommandType = System.Data.CommandType.Text;

        //    //cmdDelete.Parameters.Add("@idParityDtl", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.IdParityDtl;
        //    return cmdDelete.ExecuteNonQuery();
        //}

        //#endregion

    }
}