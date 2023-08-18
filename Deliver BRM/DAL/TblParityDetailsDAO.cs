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
    public class TblParityDetailsDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT parity.* ,  prodCat.prodCateDesc ,prodSpec.prodSpecDesc, material.materialSubType as materialDesc " +
                                  " FROM[tblParityDetails] parity" +
                                  " LEFT JOIN dimProdCat prodCat ON parity.prodCatId = prodCat.idProdCat" +
                                  " LEFT JOIN dimProdSpec prodSpec ON parity.prodSpecId = prodSpec.idProdSpec" +
                                  " LEFT JOIN tblMaterial material ON parity.materialId=material.idMaterial";

            return sqlSelectQry;
        }


        //Sudhir[20-March-2018] Simple Select Query
        public static String SqlSimpleSelectQuery()
        {
            String sqlSelectQry = "SELECT parityDtl.*,material.materialSubType AS materialDesc,prodCat.prodCateDesc As prodCateDesc " +
                " ,prodSpec.prodSpecDesc As prodSpecDesc,itemName='',displayName='' FROM  tblParityDetails parityDtl  LEFT JOIN tblMaterial material " +
                " ON parityDtl.materialId = material.idMaterial LEFT JOIN dimProdCat prodCat ON parityDtl.prodCatId = prodCat.idProdCat" +
                " LEFT JOIN dimProdSpec prodSpec ON parityDtl.prodSpecId = prodSpec.idProdSpec ";

            return sqlSelectQry;
        }

        #endregion

        #region Selection
        public static List<TblParityDetailsTO> SelectAllTblParityDetails(int parityId, Int32 prodSpecId, SqlConnection conn, SqlTransaction tran)
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

        public static List<TblParityDetailsTO> SelectAllParityDetailsListByIds(String parityDtlIds, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE parity.idParityDtl In(" + parityDtlIds + ")";

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

        public static List<TblParityDetailsTO> SelectAllLatestParityDetails(Int32 stateId, Int32 prodSpecId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            String sqlQuery = string.Empty;
            try
            {
                if (prodSpecId == 0)
                {
                    sqlQuery = " SELECT material.idMaterial materialId , material.materialSubType AS materialDesc, dimProdCat.idProdCat AS prodCatId , " +
                                            " dimProdCat.prodCateDesc ,dimProdSpec.idProdSpec AS prodSpecId , dimProdSpec.prodSpecDesc, latestParity.idParityDtl , latestParity.parityId,latestParity.parityAmt,latestParity.nonConfParityAmt, " +
                                            " latestParity.remark,latestParity.createdOn,latestParity.createdBy " +
                                            " FROM tblMaterial material " +
                                            " FULL OUTER JOIN dimProdCat " +
                                            " ON 1 = 1 " +
                                            " FULL OUTER JOIN dimProdSpec " +
                                            " ON 1 = 1 " +
                                            " LEFT JOIN " +
                                            " ( " +
                                            "     SELECT parityDtl.* FROM tblParityDetails parityDtl " +
                                            "     INNER JOIN tblParitySummary paritySum " +
                                            "     ON parityDtl.parityId = paritySum.idParity " +
                                            "     WHERE paritySum.idParity = (SELECT MAX(idParity) FROM tblParitySummary WHERE stateId=" + stateId + ") " +
                                            " ) latestParity " +
                                            " ON material.idMaterial = latestParity.materialId " +
                                            " AND dimProdCat.idProdCat = latestParity.prodCatId" +
                                            " AND dimProdSpec.idProdSpec = latestParity.prodSpecId";

                }
                else
                {
                    sqlQuery = " SELECT material.idMaterial materialId , material.materialSubType AS materialDesc, dimProdCat.idProdCat AS prodCatId , " +
                                           " dimProdCat.prodCateDesc ,dimProdSpec.idProdSpec AS prodSpecId , dimProdSpec.prodSpecDesc, latestParity.idParityDtl , latestParity.parityId,latestParity.parityAmt,latestParity.nonConfParityAmt, " +
                                           " latestParity.remark,latestParity.createdOn,latestParity.createdBy " +
                                           " FROM tblMaterial material " +
                                           " FULL OUTER JOIN dimProdCat " +
                                           " ON 1 = 1 " +
                                           " FULL OUTER JOIN dimProdSpec " +
                                           " ON 1 = 1 " +
                                           " LEFT JOIN " +
                                           " ( " +
                                           "     SELECT parityDtl.* FROM tblParityDetails parityDtl " +
                                           "     INNER JOIN tblParitySummary paritySum " +
                                           "     ON parityDtl.parityId = paritySum.idParity " +
                                           "     WHERE parityDtl.prodSpecId=" + prodSpecId + " AND paritySum.idParity = (SELECT MAX(idParity) FROM tblParitySummary WHERE stateId=" + stateId + ") " +
                                           " ) latestParity " +
                                           " ON material.idMaterial = latestParity.materialId " +
                                           " AND dimProdCat.idProdCat = latestParity.prodCatId" +
                                           " AND dimProdSpec.idProdSpec = latestParity.prodSpecId" +
                                           " WHERE dimProdSpec.idProdSpec=" + prodSpecId;
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

        public static TblParityDetailsTO SelectTblParityDetails(Int32 idParityDtl)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idParityDtl = " + idParityDtl + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblParityDetailsTO> list = ConvertDTToList(sqlReader);
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
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblParityDetailsTO> SelectAllParityDetails()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSimpleSelectQuery() + " WHERE parityDtl.isActive=1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblParityDetailsTO> list = ConvertDTToListV2(sqlReader);
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
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        public static List<TblParityDetailsTO> SelectAllParityDetails(SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.CommandText = SqlSimpleSelectQuery() ;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblParityDetailsTO> list = ConvertDTToListV2(sqlReader);
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
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        ///// <summary>
        ///// Sudhir[20-MARCH-2018] Added for get List Based on productItemId for other Item Parity Details
        ///// </summary>
        ///// <returns></returns>
        //public static List<TblParityDetailsTO> SelectAllParityDetailsOnProductItemId(Int32 productItemId,  Int32 prodCatId, Int32 prodSpecId, Int32 materialId)
        //{
        //    String sqlConnStr = Startup.ConnectionString;
        //    SqlConnection conn = new SqlConnection(sqlConnStr);
        //    SqlCommand cmdSelect = new SqlCommand();
        //    SqlDataReader sqlReader = null;
        //    try
        //    {
        //        conn.Open();
        //        cmdSelect.CommandText = SqlSimpleSelectQuery() + " WHERE ISNULL(parityDtl.prodItemId,0)=" + productItemId 
        //                                + " AND ISNULL(parityDtl.prodCatId,0)=" + prodCatId + " AND ISNULL(parityDtl.prodSpecId,0)=" + prodSpecId
        //                                + " AND ISNULL(parityDtl.materialId,0)=" + materialId + "AND parityDtl.isActive=" + 1;
        //        cmdSelect.Connection = conn;
        //        cmdSelect.CommandType = System.Data.CommandType.Text;

        //        sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
        //        List<TblParityDetailsTO> list = ConvertDTToListV2(sqlReader);
        //        if (list != null)
        //            return list;
        //        else return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        if (sqlReader != null)
        //            sqlReader.Dispose();
        //        conn.Close();
        //        cmdSelect.Dispose();
        //    }
        //}

        /// <summary>
        /// Priyanka [14-09-2018] Added for get List Based on productItemId for other Item Parity Details
        /// </summary>
        /// <returns></returns>
        public static List<TblParityDetailsTO> SelectAllParityDetailsOnProductItemId(Int32 productItemId, Int32 prodCatId, Int32 stateId, Int32 currencyId, Int32 productSpecInfoListTo)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                if (productSpecInfoListTo == 0)
                {
                    cmdSelect.CommandText = " SELECT latestParity.idParityDtl, latestParity.parityId, latestParity.parityAmt,latestParity.nonConfParityAmt, "+
                                            " latestParity.remark,latestParity.createdOn, latestParity.createdBy, latestParity.baseValCorAmt, "+
                                            " latestParity.freightAmt, latestParity.expenseAmt, latestParity.otherAmt, latestParity.prodItemId, "+
                                            " latestParity.isActive, latestParity.currencyId  ,material.idMaterial As materialId, "+
                                            " material.materialSubType AS materialDesc,prodcat.idProdCat As prodCatId, itemName = '', "+
                                            " displayName = '', prodCat.prodCateDesc As prodCateDesc , prodspec.idProdSpec As prodSpecId,"+
                                            " prodSpec.prodSpecDesc As prodSpecDesc, stateName.idState As stateId "+
                                            " FROM tblMaterial material "+
                                            " FULL OUTER JOIN dimProdCat prodCat ON 1 = 1 and prodCat.idProdCat = "+ prodCatId + " and prodCat.isActive = 1 "+
                                            " FULL OUTER JOIN dimProdSpec prodSpec ON 1 = 1  and prodSpec.isActive = 1 "+
                                            " FULL OUTER JOIN dimState stateName ON 1 = 1 and stateName.idState = "+ stateId +
                                            " FULL OUTER JOIN dimCurrency currency ON 1 = 1 and currency.idCurrency = "+ currencyId +
                                            " LEFT JOIN(select* from tblParityDetails where stateId= "+stateId +" and isActive = 1 " +
                                            " and prodCatId = "+ prodCatId +" and currencyId = "+ currencyId + ") latestParity "+
                                            " ON material.idMaterial = latestParity.materialId AND prodCat.idProdCat = latestParity.prodCatId "+
                                            " AND prodSpec.idProdSpec = latestParity.prodSpecId AND stateName.idState = latestParity.stateId "+
                                            " AND currency.idCurrency = latestParity.currencyId" +
                                            " where idProdCat = " + prodCatId + " order by prodSpec.displaySequence ,materialId";
                }
                else
                {
                    cmdSelect.CommandText = " Select  latestParity.idParityDtl, latestParity.parityId, latestParity.parityAmt,latestParity.nonConfParityAmt, " +
                                                           " latestParity.remark,latestParity.createdOn, latestParity.createdBy, latestParity.baseValCorAmt, " +
                                                           " latestParity.freightAmt, latestParity.expenseAmt, latestParity.materialId ,materialDesc = ''," +
                                                           " prodCateDesc ='', prodSpecDesc ='',latestParity.prodSpecId," +
                                                           " latestParity.prodCatId ,latestParity.otherAmt, " +
                                                           " latestParity.isActive, currency.idCurrency  As currencyId,prodClass.displayName ,prodItem.itemName ," +
                                                           " prodItem.idProdItem as prodItemId,stateName.idState As stateId from tblProductItem prodItem " +
                                                           " LEft Join tblProdClassification prodClass On prodClass.idProdClass = prodItem.prodClassId " +
                                                           " LEFT JOIN(select * from tblParityDetails where stateId= " + stateId + " and isActive = 1 " +
                                                           " and currencyId = " + currencyId + ") latestParity ON prodItem.idProdItem = latestParity.prodItemId " +
                                                           " FULL outer join dimCurrency currency ON 1 = 1 and currency.idCurrency =  " + currencyId +
                                                           " FULL outer join dimState stateName ON 1 = 1 and stateName.idState =  " + stateId +
                                                           " where prodItem.prodClassId = " + productSpecInfoListTo + "and isParity = 1";

                }
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblParityDetailsTO> list = ConvertDTToListV2(sqlReader);
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
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        /// <summary>
        /// Sudhir[23-MARCH-2018] Added for Get ParityDetail List based on Booking DateTime and Other Combination
        /// </summary>
        /// <returns></returns>
        public static List<TblParityDetailsTO> SelectParityDetailToListOnBooking(Int32 materialId, Int32 prodCatId, Int32 prodSpecId, Int32 productItemId, Int32 stateId, DateTime boookingDate)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSimpleSelectQuery() + " WHERE ISNULL(parityDtl.prodItemId,0)=" + productItemId 
                                        + " AND ISNULL(parityDtl.prodCatId,0)=" + prodCatId + " AND ISNULL(parityDtl.prodSpecId,0)=" + prodSpecId
                                        + " AND ISNULL(parityDtl.materialId,0)=" + materialId
                                        + " AND  parityDtl.stateId=" + stateId
                                        + "  AND parityDtl.createdOn <=  @BookingDate order by parityDtl.createdOn DESC";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@BookingDate", System.Data.SqlDbType.DateTime).Value = boookingDate;//.ToString(Constants.AzureDateFormat);
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblParityDetailsTO> list = ConvertDTToListV2(sqlReader);
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
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }



        public static List<TblParityDetailsTO> ConvertDTToList(SqlDataReader tblParityDetailsTODT)
        {
            List<TblParityDetailsTO> tblParityDetailsTOList = new List<TblParityDetailsTO>();
            if (tblParityDetailsTODT != null)
            {
                while (tblParityDetailsTODT.Read())
                {
                    TblParityDetailsTO tblParityDetailsTONew = new TblParityDetailsTO();
                    if (tblParityDetailsTODT["idParityDtl"] != DBNull.Value)
                        tblParityDetailsTONew.IdParityDtl = Convert.ToInt32(tblParityDetailsTODT["idParityDtl"].ToString());
                    if (tblParityDetailsTODT["parityId"] != DBNull.Value)
                        tblParityDetailsTONew.ParityId = Convert.ToInt32(tblParityDetailsTODT["parityId"].ToString());
                    if (tblParityDetailsTODT["materialId"] != DBNull.Value)
                        tblParityDetailsTONew.MaterialId = Convert.ToInt32(tblParityDetailsTODT["materialId"].ToString());
                    if (tblParityDetailsTODT["createdBy"] != DBNull.Value)
                        tblParityDetailsTONew.CreatedBy = Convert.ToInt32(tblParityDetailsTODT["createdBy"].ToString());
                    if (tblParityDetailsTODT["createdOn"] != DBNull.Value)
                        tblParityDetailsTONew.CreatedOn = Convert.ToDateTime(tblParityDetailsTODT["createdOn"].ToString());
                    if (tblParityDetailsTODT["parityAmt"] != DBNull.Value)
                        tblParityDetailsTONew.ParityAmt = Convert.ToDouble(tblParityDetailsTODT["parityAmt"].ToString());
                    if (tblParityDetailsTODT["nonConfParityAmt"] != DBNull.Value)
                        tblParityDetailsTONew.NonConfParityAmt = Convert.ToDouble(tblParityDetailsTODT["nonConfParityAmt"].ToString());
                    if (tblParityDetailsTODT["remark"] != DBNull.Value)
                        tblParityDetailsTONew.Remark = Convert.ToString(tblParityDetailsTODT["remark"].ToString());
                    if (tblParityDetailsTODT["prodCatId"] != DBNull.Value)
                        tblParityDetailsTONew.ProdCatId = Convert.ToInt32(tblParityDetailsTODT["prodCatId"].ToString());
                    if (tblParityDetailsTODT["prodCateDesc"] != DBNull.Value)
                        tblParityDetailsTONew.ProdCatDesc = Convert.ToString(tblParityDetailsTODT["prodCateDesc"].ToString());
                    if (tblParityDetailsTODT["materialDesc"] != DBNull.Value)
                        tblParityDetailsTONew.MaterialDesc = Convert.ToString(tblParityDetailsTODT["materialDesc"].ToString());
                    if (tblParityDetailsTODT["prodSpecId"] != DBNull.Value)
                        tblParityDetailsTONew.ProdSpecId = Convert.ToInt32(tblParityDetailsTODT["prodSpecId"].ToString());
                    if (tblParityDetailsTODT["prodSpecDesc"] != DBNull.Value)
                        tblParityDetailsTONew.ProdSpecDesc = Convert.ToString(tblParityDetailsTODT["prodSpecDesc"].ToString());
                    tblParityDetailsTOList.Add(tblParityDetailsTONew);
                }
            }
            return tblParityDetailsTOList;
        }

        //Sudhir
        public static List<TblParityDetailsTO> ConvertDTToListV2(SqlDataReader tblParityDetailsTODT)
        {
            List<TblParityDetailsTO> tblParityDetailsTOList = new List<TblParityDetailsTO>();
            if (tblParityDetailsTODT != null)
            {
                while (tblParityDetailsTODT.Read())
                {
                    TblParityDetailsTO tblParityDetailsTONew = new TblParityDetailsTO();
                    if (tblParityDetailsTODT["idParityDtl"] != DBNull.Value)
                        tblParityDetailsTONew.IdParityDtl = Convert.ToInt32(tblParityDetailsTODT["idParityDtl"].ToString());
                    if (tblParityDetailsTODT["parityId"] != DBNull.Value)
                        tblParityDetailsTONew.ParityId = Convert.ToInt32(tblParityDetailsTODT["parityId"].ToString());
                    if (tblParityDetailsTODT["materialId"] != DBNull.Value)
                        tblParityDetailsTONew.MaterialId = Convert.ToInt32(tblParityDetailsTODT["materialId"].ToString());
                    if (tblParityDetailsTODT["createdBy"] != DBNull.Value)
                        tblParityDetailsTONew.CreatedBy = Convert.ToInt32(tblParityDetailsTODT["createdBy"].ToString());
                    if (tblParityDetailsTODT["createdOn"] != DBNull.Value)
                        tblParityDetailsTONew.CreatedOn = Convert.ToDateTime(tblParityDetailsTODT["createdOn"].ToString());
                    if (tblParityDetailsTODT["parityAmt"] != DBNull.Value)
                        tblParityDetailsTONew.ParityAmt = Convert.ToDouble(tblParityDetailsTODT["parityAmt"].ToString());
                    if (tblParityDetailsTODT["nonConfParityAmt"] != DBNull.Value)
                        tblParityDetailsTONew.NonConfParityAmt = Convert.ToDouble(tblParityDetailsTODT["nonConfParityAmt"].ToString());
                    if (tblParityDetailsTODT["remark"] != DBNull.Value)
                        tblParityDetailsTONew.Remark = Convert.ToString(tblParityDetailsTODT["remark"].ToString());
                    if (tblParityDetailsTODT["prodCatId"] != DBNull.Value)
                        tblParityDetailsTONew.ProdCatId = Convert.ToInt32(tblParityDetailsTODT["prodCatId"].ToString());
                    if (tblParityDetailsTODT["prodCateDesc"] != DBNull.Value)
                        tblParityDetailsTONew.ProdCatDesc = Convert.ToString(tblParityDetailsTODT["prodCateDesc"].ToString());
                    if (tblParityDetailsTODT["materialDesc"] != DBNull.Value)
                        tblParityDetailsTONew.MaterialDesc = Convert.ToString(tblParityDetailsTODT["materialDesc"].ToString());
                    if (tblParityDetailsTODT["prodSpecId"] != DBNull.Value)
                        tblParityDetailsTONew.ProdSpecId = Convert.ToInt32(tblParityDetailsTODT["prodSpecId"].ToString());
                    if (tblParityDetailsTODT["prodSpecDesc"] != DBNull.Value)
                        tblParityDetailsTONew.ProdSpecDesc = Convert.ToString(tblParityDetailsTODT["prodSpecDesc"].ToString());
                    if (tblParityDetailsTODT["stateId"] != DBNull.Value)
                        tblParityDetailsTONew.StateId = Convert.ToInt32(tblParityDetailsTODT["stateId"].ToString());
                    if (tblParityDetailsTODT["baseValCorAmt"] != DBNull.Value)
                        tblParityDetailsTONew.BaseValCorAmt = Convert.ToDouble(tblParityDetailsTODT["baseValCorAmt"].ToString());
                    if (tblParityDetailsTODT["freightAmt"] != DBNull.Value)
                        tblParityDetailsTONew.FreightAmt = Convert.ToDouble(tblParityDetailsTODT["freightAmt"].ToString());
                    if (tblParityDetailsTODT["expenseAmt"] != DBNull.Value)
                        tblParityDetailsTONew.ExpenseAmt = Convert.ToDouble(tblParityDetailsTODT["expenseAmt"].ToString());
                    if (tblParityDetailsTODT["otherAmt"] != DBNull.Value)
                        tblParityDetailsTONew.OtherAmt = Convert.ToDouble(tblParityDetailsTODT["otherAmt"].ToString());
                    if (tblParityDetailsTODT["prodItemId"] != DBNull.Value)
                        tblParityDetailsTONew.ProdItemId = Convert.ToInt32(tblParityDetailsTODT["prodItemId"].ToString());
                    if (tblParityDetailsTODT["isActive"] != DBNull.Value)
                        tblParityDetailsTONew.IsActive = Convert.ToInt32(tblParityDetailsTODT["isActive"].ToString());

                    //Priyanka [14-09-2018] : Added
                    if (tblParityDetailsTODT["currencyId"] != DBNull.Value)
                        tblParityDetailsTONew.CurrencyId = Convert.ToInt32(tblParityDetailsTODT["currencyId"].ToString());
                    if (tblParityDetailsTODT["itemName"] != DBNull.Value)
                        tblParityDetailsTONew.ItemName = Convert.ToString(tblParityDetailsTODT["itemName"].ToString());
                    if (tblParityDetailsTODT["displayName"] != DBNull.Value)
                        tblParityDetailsTONew.DisplayName = Convert.ToString(tblParityDetailsTODT["displayName"].ToString());
                    if (tblParityDetailsTONew.ProdItemId > 0)
                    {
                        tblParityDetailsTONew.DisplayName = tblParityDetailsTONew.DisplayName + "/" + tblParityDetailsTONew.ItemName;
                    }
                    else
                    {
                        tblParityDetailsTONew.DisplayName = tblParityDetailsTONew.MaterialDesc + "-" + tblParityDetailsTONew.ProdCatDesc + "-" + tblParityDetailsTONew.ProdSpecDesc;
                    }

                    tblParityDetailsTOList.Add(tblParityDetailsTONew);
                }
            }
            return tblParityDetailsTOList;
        }

        /// <summary>
        /// Vijaymala[19-06-2018] Added for Get ParityDetail List based on Booking DateTime and state id
        /// </summary>
        /// <returns></returns>
        public static List<TblParityDetailsTO> SelectParityDetailToListOnBooking( Int32 stateId, DateTime boookingDate)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSimpleSelectQuery() + " WHERE parityDtl.createdOn <=  @BookingDate";

                if (stateId > 0)
                {
                    cmdSelect.CommandText += " AND parityDtl.stateId=" + stateId;
                }

                cmdSelect.CommandText +=  "order by parityDtl.createdOn DESC";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@BookingDate", System.Data.SqlDbType.DateTime).Value = boookingDate;//.ToString(Constants.AzureDateFormat);
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblParityDetailsTO> list = ConvertDTToListV2(sqlReader);
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
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        #endregion

        #region Insertion
        public static int InsertTblParityDetails(TblParityDetailsTO tblParityDetailsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
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

        public static int InsertTblParityDetails(TblParityDetailsTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran)
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

        public static int ExecuteInsertionCommand(TblParityDetailsTO tblParityDetailsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblParityDetails]( " +
                            "  [parityId]" +
                            " ,[materialId]" +
                            " ,[createdBy]" +
                            " ,[createdOn]" +
                            " ,[parityAmt]" +
                            " ,[nonConfParityAmt]" +
                            " ,[remark]" +
                            " ,[prodCatId]" +
                            " ,[prodSpecId]" +
                            " ,[stateId]" +
                            " ,[prodItemId]" +
                            " ,[isActive]" +
                            " ,[baseValCorAmt]" +
                            " ,[freightAmt]" +
                            " ,[expenseAmt]" +
                            " ,[otherAmt]" +
                            " ,[currencyId]" +
                            " )" +
                " VALUES (" +
                            "  @ParityId " +
                            " ,@MaterialId " +
                            " ,@CreatedBy " +
                            " ,@CreatedOn " +
                            " ,@ParityAmt " +
                            " ,@NonConfParityAmt " +
                            " ,@Remark " +
                            " ,@prodCatId " +
                            " ,@prodSpecId " +
                            " ,@StateId " +
                            " ,@ProdItemId " +
                            " ,@IsActive " +
                            " ,@BaseValCorAmt " +
                            " ,@FreightAmt " +
                            " ,@ExpenseAmt " +
                            " ,@OtherAmt " +
                            " ,@currencyId" +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdParityDtl", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.IdParityDtl;
            cmdInsert.Parameters.Add("@ParityId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.ParityId);
            cmdInsert.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.MaterialId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblParityDetailsTO.CreatedOn;
            cmdInsert.Parameters.Add("@ParityAmt", System.Data.SqlDbType.NVarChar).Value = tblParityDetailsTO.ParityAmt;
            cmdInsert.Parameters.Add("@NonConfParityAmt", System.Data.SqlDbType.NVarChar).Value = tblParityDetailsTO.NonConfParityAmt;
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.Remark);
            cmdInsert.Parameters.Add("@prodCatId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.ProdCatId);
            cmdInsert.Parameters.Add("@prodSpecId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.ProdSpecId);
            cmdInsert.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.ProdItemId);
            cmdInsert.Parameters.Add("@StateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.StateId);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.IsActive);
            cmdInsert.Parameters.Add("@BaseValCorAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.BaseValCorAmt);
            cmdInsert.Parameters.Add("@FreightAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.FreightAmt);
            cmdInsert.Parameters.Add("@ExpenseAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.ExpenseAmt);
            cmdInsert.Parameters.Add("@OtherAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.OtherAmt);
            cmdInsert.Parameters.Add("@CurrencyId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.CurrencyId);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblParityDetailsTO.IdParityDtl = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation
        public static int UpdateTblParityDetails(TblParityDetailsTO tblParityDetailsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblParityDetailsTO, cmdUpdate);
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

        public static int UpdateTblParityDetails(TblParityDetailsTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblParityDetailsTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblParityDetailsTO tblParityDetailsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblParityDetails] SET " +
                            "  [parityId]= @ParityId" +
                            " ,[materialId]= @MaterialId" +
                            " ,[createdBy]= @CreatedBy" +
                            " ,[createdOn]= @CreatedOn" +
                            " ,[parityAmt]= @ParityAmt" +
                            " ,[nonConfParityAmt]= @NonConfParityAmt" +
                            " ,[remark] = @Remark" +
                            " ,[prodCatId] = @prodCatId" +
                            " ,[prodSpecId] = @prodSpecId" +
                            " ,[stateId]= @StateId" +
                            " ,[prodItemId]= @ProdItemId" +
                            " ,[isActive]= @IsActive" +
                            " ,[baseValCorAmt]= @BaseValCorAmt" +
                            " ,[freightAmt]= @FreightAmt" +
                            " ,[expenseAmt]= @ExpenseAmt" +
                            " ,[otherAmt]= @OtherAmt" +
                            " ,[currencyId] @CurrencyId" +
                            " WHERE [idParityDtl] = @IdParityDtl ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdParityDtl", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.IdParityDtl;
            cmdUpdate.Parameters.Add("@ParityId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.ParityId);
            cmdUpdate.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.MaterialId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblParityDetailsTO.CreatedOn;
            cmdUpdate.Parameters.Add("@ParityAmt", System.Data.SqlDbType.NVarChar).Value = tblParityDetailsTO.ParityAmt;
            cmdUpdate.Parameters.Add("@NonConfParityAmt", System.Data.SqlDbType.NVarChar).Value = tblParityDetailsTO.NonConfParityAmt;
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.Remark);
            cmdUpdate.Parameters.Add("@prodCatId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.ProdCatId;
            cmdUpdate.Parameters.Add("@prodSpecId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.ProdSpecId;
            cmdUpdate.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.ProdItemId);
            cmdUpdate.Parameters.Add("@StateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.StateId);
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.IsActive);
            cmdUpdate.Parameters.Add("@BaseValCorAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.BaseValCorAmt);
            cmdUpdate.Parameters.Add("@FreightAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.FreightAmt);
            cmdUpdate.Parameters.Add("@ExpenseAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.ExpenseAmt);
            cmdUpdate.Parameters.Add("@OtherAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.OtherAmt);
            cmdUpdate.Parameters.Add("@CurrencyId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParityDetailsTO.CurrencyId);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblParityDetails(Int32 idParityDtl)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idParityDtl, cmdDelete);
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

        public static int DeleteTblParityDetails(Int32 idParityDtl, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idParityDtl, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idParityDtl, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = " DELETE FROM [tblParityDetails] " +
                                     " WHERE idParityDtl = " + idParityDtl + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idParityDtl", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.IdParityDtl;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

        #region Deactivation

        public static int DeactivateAllParityDetails(TblParityDetailsTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblParityDetails] SET " +
                                    " [isActive]= @IsActive" +
                                    " WHERE ISNULL(prodItemId,0)=@ProductItemId" +
                                    " AND ISNULL(materialId,0)=@MaterialId" +
                                    " AND ISNULL(prodSpecId,0)=@ProdSpecId" +
                                    " AND ISNULL(prodCatId,0)=@ProdCatId " +
                                    " AND ISNULL(stateId,0)=@StateId " +
                                    " AND ISNULL(prodItemId,0)=@ProdItemId " +
                                    " AND isActive=1";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;
                cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = 0;
                cmdUpdate.Parameters.Add("@ProductItemId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.ProdItemId;
                cmdUpdate.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.MaterialId;
                cmdUpdate.Parameters.Add("@ProdSpecId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.ProdSpecId;
                cmdUpdate.Parameters.Add("@ProdCatId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.ProdCatId;
                cmdUpdate.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.ProdItemId;
                cmdUpdate.Parameters.Add("@StateId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.StateId;

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

        /// <summary>
        /// Sudhir[21-MARCH-2018] Added for Deactivating Related Parity Details Record  
        /// </summary>
        /// <param name="brandId"></param>
        /// <param name="productItemId"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static int DeactivateAllParityDetailsForUpdate(TblParityDetailsTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblParityDetails] SET " +
                                   " [isActive]= @IsActive" +
                                   " WHERE ISNULL(prodItemId,0)=@ProductItemId" +
                                   " AND ISNULL(stateId,0)=@StateId" +
                                   " AND ISNULL(materialId,0)=@MaterialId" +
                                   " AND ISNULL(prodSpecId,0)=@ProdSpecId" +
                                   " AND ISNULL(prodCatId,0)=@ProdCatId " +
                                   " AND isActive=1";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;
                cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = 0;
                cmdUpdate.Parameters.Add("@ProductItemId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.ProdItemId;
                cmdUpdate.Parameters.Add("@StateId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.StateId;
                cmdUpdate.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.MaterialId;
                cmdUpdate.Parameters.Add("@ProdSpecId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.ProdSpecId;
                cmdUpdate.Parameters.Add("@ProdCatId", System.Data.SqlDbType.Int).Value = tblParityDetailsTO.ProdCatId;

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

        #endregion
    }
}
