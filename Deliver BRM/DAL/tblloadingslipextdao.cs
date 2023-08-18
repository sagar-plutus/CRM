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
    public class TblLoadingSlipExtDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT loadDtl.* ,loadLayers.layerDesc,material.materialSubType " +
                                  " , prodCat.prodCateDesc AS prodCatDesc ,prodSpec.prodSpecDesc " +
                                  ",category.prodClassDesc as categoryName ,subCategory.prodClassDesc as subCategoryName," +
                                  "specification.prodClassDesc as specificationName,item.itemName "+
                                  "  FROM tempLoadingSlipExt loadDtl " +
                                  "  LEFT JOIN dimLoadingLayers loadLayers " +
                                  "  ON loadDtl.loadingLayerid = loadLayers.idLoadingLayer " +
                                  "  LEFT JOIN tblMaterial material " +
                                  "  ON material.idMaterial = loadDtl.materialId " +
                                  " LEFT JOIN  dimProdCat prodCat ON prodCat.idProdCat=loadDtl.prodCatId" +
                                  " LEFT JOIN  dimProdSpec prodSpec ON prodSpec.idProdSpec=loadDtl.prodSpecId" +
                                  " LEFT JOIN tblProductItem item ON item.idProdItem = loadDtl.prodItemId " +
                                  " LEFT JOIN tblProdClassification specification ON item.prodClassId = specification.idProdClass " +
                                  " LEFT JOIN tblProdClassification subCategory ON specification.parentProdClassId = subCategory.idProdClass " +
                                  " LEFT JOIN tblProdClassification category ON subCategory.parentProdClassId = category.idProdClass "+
                                  // Vaibhav [20-Nov-2017] Added to select from finalLoadingSlipExt

                                  " UNION ALL "+
                                  " SELECT loadDtl.* ,loadLayers.layerDesc,material.materialSubType " +
                                  " , prodCat.prodCateDesc AS prodCatDesc ,prodSpec.prodSpecDesc " +
                                  ",category.prodClassDesc as categoryName ,subCategory.prodClassDesc as subCategoryName," +
                                  "specification.prodClassDesc as specificationName,item.itemName " +
                                  "  FROM finalLoadingSlipExt loadDtl " +
                                  "  LEFT JOIN dimLoadingLayers loadLayers " +
                                  "  ON loadDtl.loadingLayerid = loadLayers.idLoadingLayer " +
                                  "  LEFT JOIN tblMaterial material " +
                                  "  ON material.idMaterial = loadDtl.materialId " +
                                  " LEFT JOIN tblProductItem item ON item.idProdItem = loadDtl.prodItemId  "+
                                  " LEFT JOIN  dimProdCat prodCat ON prodCat.idProdCat=loadDtl.prodCatId" +
                                  " LEFT JOIN  dimProdSpec prodSpec ON prodSpec.idProdSpec=loadDtl.prodSpecId"+
                                  "  LEFT JOIN tblProdClassification specification ON item.prodClassId = specification.idProdClass " +
                                  "  LEFT JOIN tblProdClassification subCategory ON specification.parentProdClassId = subCategory.idProdClass " +
                                  "  LEFT JOIN tblProdClassification category ON subCategory.parentProdClassId = category.idProdClass ";

            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblLoadingSlipExtTO> SelectAllTblLoadingSlipExt()
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
                List<TblLoadingSlipExtTO> list = ConvertDTToList(sqlReader);
                return list;
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

        public static List<TblLoadingSlipExtTO> SelectAllLoadingSlipExtListFromLoadingId(String loadingIds, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE loadingSlipId IN (SELECT idLoadingSlip FROM tempLoadingSlip WHERE loadingId IN(" + loadingIds + ") )" +

                                        // Vaibhav [20-Nov-2017] Added to select from finalLoadingSlip
                                        " UNION ALL " + " SELECT * FROM ("+ SqlSelectQuery() + ")sq2 WHERE loadingSlipId IN (SELECT idLoadingSlip FROM finalLoadingSlip WHERE loadingId IN(" + loadingIds + ") )";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipExtTO> list = ConvertDTToList(sqlReader);
                return list;
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

        public static Dictionary<Int32, Double> SelectLoadingQuotaWiseApprovedLoadingQtyDCT(String loadingQuotaIds, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            String statusIds = (int)Constants.TranStatusE.LOADING_CONFIRM + "," + (int)Constants.TranStatusE.LOADING_COMPLETED + "," +
                         (int)Constants.TranStatusE.LOADING_DELIVERED + "," + (int)Constants.TranStatusE.LOADING_GATE_IN + "," +
                         (int)Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING;
            try
            {
                cmdSelect.CommandText = " SELECT ext.loadingQuotaId, SUM(loadingQty) approvedLoadingQty " +
                                        " FROM tempLoadingSlipExt ext " +
                                        " LEFT JOIN tempLoadingSlip slip ON slip.idLoadingSlip = ext.loadingSlipId " +
                                        " LEFT JOIN tempLoading loading ON loading.idLoading = slip.loadingId " +
                                        " WHERE ext.loadingQuotaId IN(" + loadingQuotaIds + ") AND loading.statusId IN(" + statusIds + ") " +
                                        " GROUP BY ext.loadingQuotaId";//+

                                        //// Vaibhav [20-Nov-2017] Added to select from finalLoading and finalLoadingSlip and finalLoadingSlipExt

                                        //" UNION ALL " +
                                        //" SELECT ext.loadingQuotaId, SUM(loadingQty) approvedLoadingQty " +
                                        //" FROM finalLoadingSlipExt ext " +
                                        //" LEFT JOIN finalLoadingSlip slip ON slip.idLoadingSlip = ext.loadingSlipId " +
                                        //" LEFT JOIN finalLoading loading ON loading.idLoading = slip.loadingId " +
                                        //" WHERE ext.loadingQuotaId IN(" + loadingQuotaIds + ") AND loading.statusId IN(" + statusIds + ") " +
                                        //" GROUP BY ext.loadingQuotaId";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (sqlReader != null)
                {
                    Dictionary<Int32, Double> DCT = new Dictionary<int, double>();
                    while (sqlReader.Read())
                    {
                        Int32 loadingQuotaId = 0;
                        Double approvedQty = 0;

                        if (sqlReader["loadingQuotaId"] != DBNull.Value)
                            loadingQuotaId = Convert.ToInt32(sqlReader["loadingQuotaId"].ToString());
                        if (sqlReader["approvedLoadingQty"] != DBNull.Value)
                            approvedQty = Convert.ToDouble(sqlReader["approvedLoadingQty"].ToString());

                        if (loadingQuotaId > 0 && approvedQty > 0)
                            DCT.Add(loadingQuotaId, approvedQty);
                    }

                    return DCT;
                }

                return null;
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

        public static List<TblLoadingSlipExtTO> SelectEmptyLoadingSlipExt(Int32 prodCatId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT tblMaterial.idMaterial AS materialId, tblMaterial.materialSubType , " +
                                        " dimProdCat.idProdCat AS prodCatId,dimProdCat.prodCateDesc prodCatDesc, " +
                                        " dimProdSpec.idProdSpec AS prodSpecId ,dimProdSpec.prodSpecDesc " +
                                        " FROM tblMaterial " +
                                        " FULL OUTER JOIN dimProdCat ON 1 = 1 " +
                                        " FULL OUTER JOIN dimProdSpec ON 1 = 1" +
                                        " WHERE idProdCat=" + prodCatId;


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblLoadingSlipExtTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = new List<Models.TblLoadingSlipExtTO>();

                while (tblLoadingSlipExtTODT.Read())
                {
                    TblLoadingSlipExtTO tblLoadingSlipExtTONew = new TblLoadingSlipExtTO();
                    if (tblLoadingSlipExtTODT["materialId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.MaterialId = Convert.ToInt32(tblLoadingSlipExtTODT["materialId"].ToString());
                    if (tblLoadingSlipExtTODT["materialSubType"] != DBNull.Value)
                        tblLoadingSlipExtTONew.MaterialDesc = Convert.ToString(tblLoadingSlipExtTODT["materialSubType"].ToString());
                    if (tblLoadingSlipExtTODT["prodCatId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdCatId = Convert.ToInt32(tblLoadingSlipExtTODT["prodCatId"].ToString());
                    if (tblLoadingSlipExtTODT["prodSpecId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdSpecId = Convert.ToInt32(tblLoadingSlipExtTODT["prodSpecId"].ToString());
                    if (tblLoadingSlipExtTODT["prodCatDesc"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdCatDesc = Convert.ToString(tblLoadingSlipExtTODT["prodCatDesc"].ToString());
                    if (tblLoadingSlipExtTODT["prodSpecDesc"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdSpecDesc = Convert.ToString(tblLoadingSlipExtTODT["prodSpecDesc"].ToString());

                    tblLoadingSlipExtTOList.Add(tblLoadingSlipExtTONew);
                }

                return tblLoadingSlipExtTOList;
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

        public static List<TblLoadingSlipExtTO> SelectEmptyLoadingSlipExt(Int32 prodCatId,int prodSpecId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String whereCond = string.Empty;
            try
            {
                conn.Open();
                if (prodCatId > 0 && prodSpecId > 0)
                {
                    whereCond = "WHERE idProdCat=" + prodCatId + " AND idProdSpec=" + prodSpecId;
                }
                cmdSelect.CommandText = " SELECT tblMaterial.idMaterial AS materialId, tblMaterial.materialSubType , " +
                                        " dimProdCat.idProdCat AS prodCatId,dimProdCat.prodCateDesc prodCatDesc, " +
                                        " dimProdSpec.idProdSpec AS prodSpecId ,dimProdSpec.prodSpecDesc " +
                                        " FROM tblMaterial " +
                                        " FULL OUTER JOIN dimProdCat ON 1 = 1 " +
                                        " FULL OUTER JOIN dimProdSpec ON 1 = 1" +
                                        whereCond;


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblLoadingSlipExtTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = new List<Models.TblLoadingSlipExtTO>();

                while (tblLoadingSlipExtTODT.Read())
                {
                    TblLoadingSlipExtTO tblLoadingSlipExtTONew = new TblLoadingSlipExtTO();
                    if (tblLoadingSlipExtTODT["materialId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.MaterialId = Convert.ToInt32(tblLoadingSlipExtTODT["materialId"].ToString());
                    if (tblLoadingSlipExtTODT["materialSubType"] != DBNull.Value)
                        tblLoadingSlipExtTONew.MaterialDesc = Convert.ToString(tblLoadingSlipExtTODT["materialSubType"].ToString());
                    if (tblLoadingSlipExtTODT["prodCatId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdCatId = Convert.ToInt32(tblLoadingSlipExtTODT["prodCatId"].ToString());
                    if (tblLoadingSlipExtTODT["prodSpecId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdSpecId = Convert.ToInt32(tblLoadingSlipExtTODT["prodSpecId"].ToString());
                    if (tblLoadingSlipExtTODT["prodCatDesc"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdCatDesc = Convert.ToString(tblLoadingSlipExtTODT["prodCatDesc"].ToString());
                    if (tblLoadingSlipExtTODT["prodSpecDesc"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdSpecDesc = Convert.ToString(tblLoadingSlipExtTODT["prodSpecDesc"].ToString());

                    if (tblLoadingSlipExtTONew.ProdCatDesc != String.Empty && tblLoadingSlipExtTONew.ProdSpecDesc != String.Empty
                            && tblLoadingSlipExtTONew.MaterialDesc != String.Empty)
                    {
                        tblLoadingSlipExtTONew.DisplayName = tblLoadingSlipExtTONew.MaterialDesc+ "-" + tblLoadingSlipExtTONew.ProdCatDesc + "-" + tblLoadingSlipExtTONew.ProdSpecDesc ;
                    }

                    tblLoadingSlipExtTOList.Add(tblLoadingSlipExtTONew);
                }

                return tblLoadingSlipExtTOList;
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

        public static List<TblLoadingSlipExtTO> SelectAllTblLoadingSlipExt(Int32 loadingSlipId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                // Vaibhav modify
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE loadingSlipId=" + loadingSlipId + "ORDER BY weighingSequenceNumber ASC" ;
                //cmdSelect.CommandText = SqlSelectQuery() + " WHERE loadingSlipId=" + loadingSlipId + "ORDER BY loadDtl.weighingSequenceNumber ASC";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipExtTO> list = ConvertDTToList(sqlReader);
                return list;
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
        ///  Sanjay [2017-12-18] It ll return record count against each loading. For the status of Vehicle IN i.e Gate In
        /// </summary>
        /// <returns></returns>
        public static Dictionary<Int32, string> SelectLoadingWiseExtRecordCountDCT()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT idLoading,COUNT(*) AS recordCount, COUNT(loadedWeight) AS loadedWt  " +
                                        " FROM tempLoadingSlipExt " +
                                        " INNER JOIN tempLoadingSlip ON idLoadingSlip = loadingSlipId " +
                                        " INNER JOIN tempLoading ON idLoading = loadingId " +
                                        " WHERE tempLoading.statusId = " + (Int32)Constants.TranStatusE.LOADING_GATE_IN +
                                        " GROUP BY idLoading" +

                                        // Vaibhav [17-Jan-2018] To select from final tables

                                        " UNION ALL " +
                                        " SELECT idLoading,COUNT(*) AS recordCount, COUNT(loadedWeight) AS loadedWt  " +
                                        " FROM finalLoadingSlipExt " +
                                        " INNER JOIN finalLoadingSlip ON idLoadingSlip = loadingSlipId " +
                                        " INNER JOIN finalLoading ON idLoading = loadingId " +
                                        " WHERE finalLoading.statusId = " + (Int32)Constants.TranStatusE.LOADING_GATE_IN +
                                        " GROUP BY idLoading";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                Dictionary<Int32, string> loadingRecordsDCT = new Dictionary<int, string>();
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (sqlReader != null)
                {
                    while (sqlReader.Read())
                    {
                        Int32 loadingId = 0;
                        Int32 recordCount = 0;
                        Int32 loadedCount = 0;
                        String wtStr = string.Empty;
                        if (sqlReader["idLoading"] != DBNull.Value)
                            loadingId = Convert.ToInt32(sqlReader["idLoading"].ToString());
                        if (sqlReader["recordCount"] != DBNull.Value)
                            recordCount = Convert.ToInt32(sqlReader["recordCount"].ToString());
                        if (sqlReader["loadedWt"] != DBNull.Value)
                            loadedCount = Convert.ToInt32(sqlReader["loadedWt"].ToString());
                        wtStr = recordCount + "|" + loadedCount;
                        loadingRecordsDCT.Add(loadingId, wtStr);
                    }
                }

                return loadingRecordsDCT;
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


        public static List<TblLoadingSlipExtTO> SelectAllTblLoadingSlipExt(Int32 loadingSlipId,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                // Vaibhav modify
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE loadingSlipId=" + loadingSlipId;
                //cmdSelect.CommandText = SqlSelectQuery() + " WHERE loadingSlipId=" + loadingSlipId + "ORDER BY weighingSequenceNumber";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction= tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipExtTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null) sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static TblLoadingSlipExtTO SelectTblLoadingSlipExt(Int32 idLoadingSlipExt)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
           
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return SelectTblLoadingSlipExt(idLoadingSlipExt, conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();                
            }
        }
        public static TblLoadingSlipExtTO SelectTblLoadingSlipExt(Int32 idLoadingSlipExt, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText =  " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE idLoadingSlipExt = " + idLoadingSlipExt + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipExtTO> list = ConvertDTToList(sqlReader);
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
                if (sqlReader != null) sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }
        public static List<TblLoadingSlipExtTO> ConvertDTToList(SqlDataReader tblLoadingSlipExtTODT)
        {
            List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = new List<TblLoadingSlipExtTO>();
            if (tblLoadingSlipExtTODT != null)
            {
                while (tblLoadingSlipExtTODT.Read())
                {
                    TblLoadingSlipExtTO tblLoadingSlipExtTONew = new TblLoadingSlipExtTO();
                    if (tblLoadingSlipExtTODT["idLoadingSlipExt"] != DBNull.Value)
                        tblLoadingSlipExtTONew.IdLoadingSlipExt = Convert.ToInt32(tblLoadingSlipExtTODT["idLoadingSlipExt"].ToString());
                    if (tblLoadingSlipExtTODT["bookingId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.BookingId = Convert.ToInt32(tblLoadingSlipExtTODT["bookingId"].ToString());
                    if (tblLoadingSlipExtTODT["loadingSlipId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.LoadingSlipId = Convert.ToInt32(tblLoadingSlipExtTODT["loadingSlipId"].ToString());
                    if (tblLoadingSlipExtTODT["loadingLayerid"] != DBNull.Value)
                        tblLoadingSlipExtTONew.LoadingLayerid = Convert.ToInt32(tblLoadingSlipExtTODT["loadingLayerid"].ToString());
                    if (tblLoadingSlipExtTODT["materialId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.MaterialId = Convert.ToInt32(tblLoadingSlipExtTODT["materialId"].ToString());
                    if (tblLoadingSlipExtTODT["bookingExtId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.BookingExtId = Convert.ToInt32(tblLoadingSlipExtTODT["bookingExtId"].ToString());
                    if (tblLoadingSlipExtTODT["loadingQty"] != DBNull.Value)
                        tblLoadingSlipExtTONew.LoadingQty = Convert.ToDouble(tblLoadingSlipExtTODT["loadingQty"].ToString());
                    if (tblLoadingSlipExtTODT["layerDesc"] != DBNull.Value)
                        tblLoadingSlipExtTONew.LoadingLayerDesc = Convert.ToString(tblLoadingSlipExtTODT["layerDesc"].ToString());
                    if (tblLoadingSlipExtTODT["materialSubType"] != DBNull.Value)
                        tblLoadingSlipExtTONew.MaterialDesc = Convert.ToString(tblLoadingSlipExtTODT["materialSubType"].ToString());

                    if (tblLoadingSlipExtTODT["quotaBforeLoading"] != DBNull.Value)
                        tblLoadingSlipExtTONew.QuotaBforeLoading = Convert.ToDouble(tblLoadingSlipExtTODT["quotaBforeLoading"].ToString());
                    if (tblLoadingSlipExtTODT["quotaAfterLoading"] != DBNull.Value)
                        tblLoadingSlipExtTONew.QuotaAfterLoading = Convert.ToDouble(tblLoadingSlipExtTODT["quotaAfterLoading"].ToString());

                    if (tblLoadingSlipExtTODT["prodCatId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdCatId = Convert.ToInt32(tblLoadingSlipExtTODT["prodCatId"].ToString());
                    if (tblLoadingSlipExtTODT["prodSpecId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdSpecId = Convert.ToInt32(tblLoadingSlipExtTODT["prodSpecId"].ToString());
                    if (tblLoadingSlipExtTODT["prodCatDesc"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdCatDesc = Convert.ToString(tblLoadingSlipExtTODT["prodCatDesc"].ToString());
                    if (tblLoadingSlipExtTODT["prodSpecDesc"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdSpecDesc = Convert.ToString(tblLoadingSlipExtTODT["prodSpecDesc"].ToString());
                    if (tblLoadingSlipExtTODT["loadingQuotaId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.LoadingQuotaId = Convert.ToInt32(tblLoadingSlipExtTODT["loadingQuotaId"].ToString());

                    if (tblLoadingSlipExtTODT["bundles"] != DBNull.Value)
                        tblLoadingSlipExtTONew.Bundles = Convert.ToDouble(tblLoadingSlipExtTODT["bundles"].ToString());
                    if (tblLoadingSlipExtTODT["parityDtlId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ParityDtlId = Convert.ToInt32(tblLoadingSlipExtTODT["parityDtlId"].ToString());
                    if (tblLoadingSlipExtTODT["ratePerMT"] != DBNull.Value)
                        tblLoadingSlipExtTONew.RatePerMT = Convert.ToDouble(tblLoadingSlipExtTODT["ratePerMT"].ToString());

                    if (tblLoadingSlipExtTODT["rateCalcDesc"] != DBNull.Value)
                        tblLoadingSlipExtTONew.RateCalcDesc = Convert.ToString(tblLoadingSlipExtTODT["rateCalcDesc"].ToString());

                    if (tblLoadingSlipExtTODT["loadedWeight"] != DBNull.Value)
                        tblLoadingSlipExtTONew.LoadedWeight = Convert.ToDouble(tblLoadingSlipExtTODT["loadedWeight"]);
                    if (tblLoadingSlipExtTODT["loadedBundles"] != DBNull.Value)
                        tblLoadingSlipExtTONew.LoadedBundles = Convert.ToDouble(tblLoadingSlipExtTODT["loadedBundles"]);
                    if (tblLoadingSlipExtTODT["calcTareWeight"] != DBNull.Value)
                        tblLoadingSlipExtTONew.CalcTareWeight = Convert.ToDouble(tblLoadingSlipExtTODT["calcTareWeight"]);
                    if (tblLoadingSlipExtTODT["weightMeasureId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.WeightMeasureId = Convert.ToInt32(tblLoadingSlipExtTODT["weightMeasureId"]);
                    if (tblLoadingSlipExtTODT["isAllowWeighingMachine"] != DBNull.Value)
                        tblLoadingSlipExtTONew.IsAllowWeighingMachine = Convert.ToInt32(tblLoadingSlipExtTODT["isAllowWeighingMachine"]);
                    if (tblLoadingSlipExtTODT["updatedBy"] != DBNull.Value)
                        tblLoadingSlipExtTONew.UpdatedBy = Convert.ToInt32(tblLoadingSlipExtTODT["updatedBy"]);
                    if (tblLoadingSlipExtTODT["updatedOn"] != DBNull.Value)
                        tblLoadingSlipExtTONew.UpdatedOn = Convert.ToDateTime(tblLoadingSlipExtTODT["updatedOn"]);
                    if (tblLoadingSlipExtTODT["cdStructureId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.CdStructureId = Convert.ToInt32(tblLoadingSlipExtTODT["cdStructureId"]);
                    if (tblLoadingSlipExtTODT["cdStructure"] != DBNull.Value)
                        tblLoadingSlipExtTONew.CdStructure = Convert.ToDouble(tblLoadingSlipExtTODT["cdStructure"]);
                    if (tblLoadingSlipExtTODT["prodItemDesc"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdItemDesc = Convert.ToString(tblLoadingSlipExtTODT["prodItemDesc"]);
                    if (tblLoadingSlipExtTODT["prodItemId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdItemId = Convert.ToInt32(tblLoadingSlipExtTODT["prodItemId"]);
                    if (tblLoadingSlipExtTODT["taxableRateMT"] != DBNull.Value)
                        tblLoadingSlipExtTONew.TaxableRateMT = Convert.ToDouble(tblLoadingSlipExtTODT["taxableRateMT"]);

                    if (tblLoadingSlipExtTODT["freExpOtherAmt"] != DBNull.Value)
                        tblLoadingSlipExtTONew.FreExpOtherAmt = Convert.ToDouble(tblLoadingSlipExtTODT["freExpOtherAmt"]);
                    if (tblLoadingSlipExtTODT["cdApplicableAmt"] != DBNull.Value)
                        tblLoadingSlipExtTONew.CdApplicableAmt = Convert.ToDouble(tblLoadingSlipExtTODT["cdApplicableAmt"]);
                    if (tblLoadingSlipExtTODT["weighingSequenceNumber"] != DBNull.Value)
                        tblLoadingSlipExtTONew.WeighingSequenceNumber = Convert.ToInt32(tblLoadingSlipExtTODT["weighingSequenceNumber"]);


                    if (tblLoadingSlipExtTODT["categoryName"] != DBNull.Value)
                        tblLoadingSlipExtTONew.CategoryName = Convert.ToString(tblLoadingSlipExtTODT["categoryName"].ToString());
                    if (tblLoadingSlipExtTODT["subCategoryName"] != DBNull.Value)
                        tblLoadingSlipExtTONew.SubCategoryName = Convert.ToString(tblLoadingSlipExtTODT["subCategoryName"].ToString());
                    if (tblLoadingSlipExtTODT["specificationName"] != DBNull.Value)
                        tblLoadingSlipExtTONew.SpecificationName = Convert.ToString(tblLoadingSlipExtTODT["specificationName"].ToString());
                    if (tblLoadingSlipExtTODT["itemName"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ItemName = Convert.ToString(tblLoadingSlipExtTODT["itemName"].ToString());

                    if (tblLoadingSlipExtTODT["modbusRefId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ModbusRefId = Convert.ToInt32(tblLoadingSlipExtTODT["modbusRefId"]);

                    if (tblLoadingSlipExtTONew.ProdItemId > 0)
                    {
                        tblLoadingSlipExtTONew.DisplayName = tblLoadingSlipExtTONew.CategoryName + "-" + tblLoadingSlipExtTONew.SubCategoryName + "-" + tblLoadingSlipExtTONew.SpecificationName + "-" + tblLoadingSlipExtTONew.ItemName;
                    }
                    else
                    {
                        tblLoadingSlipExtTONew.DisplayName =  tblLoadingSlipExtTONew.MaterialDesc + "-" + tblLoadingSlipExtTONew.ProdCatDesc + "-" + tblLoadingSlipExtTONew.ProdSpecDesc;
                    }

                    tblLoadingSlipExtTOList.Add(tblLoadingSlipExtTONew);
                }
            }
            return tblLoadingSlipExtTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblLoadingSlipExt(TblLoadingSlipExtTO tblLoadingSlipExtTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblLoadingSlipExtTO, cmdInsert);
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

        public static int InsertTblLoadingSlipExt(TblLoadingSlipExtTO tblLoadingSlipExtTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblLoadingSlipExtTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblLoadingSlipExtTO tblLoadingSlipExtTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tempLoadingSlipExt]( " +
                            "  [bookingId]" +
                            " ,[loadingSlipId]" +
                            " ,[loadingLayerid]" +
                            " ,[materialId]" +
                            " ,[bookingExtId]" +
                            " ,[loadingQty]" +
                            " ,[prodCatId]" +
                            " ,[prodSpecId]" +
                            " ,[quotaBforeLoading]" +
                            " ,[quotaAfterLoading]" +
                            " ,[loadingQuotaId]" +
                            " ,[bundles]" +
                            " ,[parityDtlId]" +
                            " ,[ratePerMT]" +
                            " ,[rateCalcDesc]" +
                            " ,[loadedWeight]" +
                            " ,[loadedBundles]" +
                            " ,[calcTareWeight]" +
                            " ,[weightMeasureId]" +
                            " ,[updatedBy]" +
                            " ,[updatedOn]" +
                            " ,[cdStructureId]" +
                            " ,[cdStructure]" +
                            ", [isAllowWeighingMachine] " +
                            " ,[prodItemDesc]" +
                            " ,[prodItemId]" +
                            " ,[taxableRateMT]" +
                            " ,[freExpOtherAmt]" +
                            " ,[cdApplicableAmt]" +
                            ",[modbusRefId]"+
                            " )" +
                " VALUES (" +
                            "  @BookingId " +
                            " ,@LoadingSlipId " +
                            " ,@LoadingLayerid " +
                            " ,@MaterialId " +
                            " ,@BookingExtId " +
                            " ,@LoadingQty " +
                            " ,@prodCatId " +
                            " ,@prodSpecId " +
                            " ,@quotaBforeLoading " +
                            " ,@quotaAfterLoading " +
                            " ,@loadingQuotaId " +
                            " ,@bundles " +
                            " ,@parityDtlId " +
                            " ,@ratePerMT " +
                            " ,@rateCalcDesc " +
                            " ,@loadedWeight " +
                            " ,@loadedBundles " +
                            " ,@calcTareWeight " +
                            " ,@weightMeasureId " +
                            " ,@updatedBy " +
                            " ,@updatedOn " +
                            " ,@cdStructureId " +
                            " ,@cdStructure " +
                            " ,@isAllowWeighingMachine" +
                            " ,@prodItemDesc " +
                            " ,@prodItemId " +
                            " ,@taxableRateMT " +
                            " ,@freExpOtherAmt " +
                            " ,@cdApplicableAmt " +
                            " ,@modbusRefId" +

                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            // cmdInsert.Parameters.Add("@IdLoadingSlipExt", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtTO.IdLoadingSlipExt;
            cmdInsert.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.BookingId);
            cmdInsert.Parameters.Add("@LoadingSlipId", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtTO.LoadingSlipId;
            cmdInsert.Parameters.Add("@LoadingLayerid", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtTO.LoadingLayerid;
            cmdInsert.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.MaterialId);
            cmdInsert.Parameters.Add("@BookingExtId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.BookingExtId);
            cmdInsert.Parameters.Add("@LoadingQty", System.Data.SqlDbType.NVarChar).Value = tblLoadingSlipExtTO.LoadingQty;
            cmdInsert.Parameters.Add("@prodCatId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.ProdCatId);
            cmdInsert.Parameters.Add("@prodSpecId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.ProdSpecId);
            cmdInsert.Parameters.Add("@quotaBforeLoading", System.Data.SqlDbType.NVarChar).Value = tblLoadingSlipExtTO.QuotaBforeLoading;
            cmdInsert.Parameters.Add("@quotaAfterLoading", System.Data.SqlDbType.NVarChar).Value = tblLoadingSlipExtTO.QuotaAfterLoading;
            cmdInsert.Parameters.Add("@loadingQuotaId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.LoadingQuotaId);
            cmdInsert.Parameters.Add("@bundles", System.Data.SqlDbType.NVarChar).Value = tblLoadingSlipExtTO.Bundles;
            cmdInsert.Parameters.Add("@parityDtlId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.ParityDtlId);
            cmdInsert.Parameters.Add("@ratePerMT", System.Data.SqlDbType.Decimal).Value = tblLoadingSlipExtTO.RatePerMT;
            cmdInsert.Parameters.Add("@rateCalcDesc", System.Data.SqlDbType.NVarChar, 256).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.RateCalcDesc);
            cmdInsert.Parameters.Add("@loadedWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.LoadedWeight);
            cmdInsert.Parameters.Add("@loadedBundles", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.LoadedBundles);
            cmdInsert.Parameters.Add("@calcTareWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.CalcTareWeight);
            cmdInsert.Parameters.Add("@weightMeasureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.WeightMeasureId);
            cmdInsert.Parameters.Add("@updatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.UpdatedBy);
            cmdInsert.Parameters.Add("@updatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.UpdatedOn);
            cmdInsert.Parameters.Add("@cdStructureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.CdStructureId);
            cmdInsert.Parameters.Add("@cdStructure", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.CdStructure);
            cmdInsert.Parameters.Add("@isAllowWeighingMachine", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.IsAllowWeighingMachine);
            cmdInsert.Parameters.Add("@prodItemDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.ProdItemDesc);
            cmdInsert.Parameters.Add("@prodItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.ProdItemId);
            cmdInsert.Parameters.Add("@taxableRateMT", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.TaxableRateMT);
            cmdInsert.Parameters.Add("@freExpOtherAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.FreExpOtherAmt);
            cmdInsert.Parameters.Add("@cdApplicableAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.CdApplicableAmt);
            cmdInsert.Parameters.Add("@modbusRefId", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.ModbusRefId);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblLoadingSlipExtTO.IdLoadingSlipExt = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation
        public static int UpdateTblLoadingSlipExt(TblLoadingSlipExtTO tblLoadingSlipExtTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblLoadingSlipExtTO, cmdUpdate);
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

        public static int UpdateTblLoadingSlipExt(TblLoadingSlipExtTO tblLoadingSlipExtTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblLoadingSlipExtTO, cmdUpdate);
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

        public static int UpdateFinalLoadingSlipExtCalTareWt(TblLoadingSlipExtTO tblLoadingSlipExtTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                cmdUpdate.CommandText = @" UPDATE [finalLoadingSlipExt] SET " +
                            " [calcTareWeight] = @calcTareWeight " +
                            " WHERE [idLoadingSlipExt] = @IdLoadingSlipExt ";

                cmdUpdate.Parameters.Add("@calcTareWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.CalcTareWeight);
                cmdUpdate.Parameters.Add("@IdLoadingSlipExt", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtTO.IdLoadingSlipExt;
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


        public static int UpdateLoadingSlipExtSeqNumber(TblLoadingSlipExtTO tblLoadingSlipExtTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                cmdUpdate.CommandText = @" UPDATE [tempLoadingSlipExt] SET " +
                            " [weighingSequenceNumber] = @weighingSequenceNumber " +
                            " WHERE [idLoadingSlipExt] = @IdLoadingSlipExt ";

                cmdUpdate.Parameters.Add("@weighingSequenceNumber", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.WeighingSequenceNumber);
                cmdUpdate.Parameters.Add("@IdLoadingSlipExt", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtTO.IdLoadingSlipExt;
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

        public static int ExecuteUpdationCommand(TblLoadingSlipExtTO tblLoadingSlipExtTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tempLoadingSlipExt] SET " +
                            "  [bookingId]= @BookingId" +
                            " ,[loadingSlipId]= @LoadingSlipId" +
                            " ,[loadingLayerid]= @LoadingLayerid" +
                            " ,[materialId]= @MaterialId" +
                            //" ,[bookingExtId]= @BookingExtId" +
                            " ,[loadingQty] = @LoadingQty" +
                            " ,[prodCatId] = @prodCatId" +
                            " ,[prodSpecId] = @prodSpecId" +
                            " ,[quotaBforeLoading] = @quotaBforeLoading" +
                            " ,[quotaAfterLoading] = @quotaAfterLoading" +
                            " ,[loadingQuotaId] = @loadingQuotaId" +
                            " ,[bundles] = @bundles" +
                            " ,[parityDtlId] = @parityDtlId" +
                            " ,[ratePerMT] = @ratePerMT" +
                            " ,[rateCalcDesc] = @rateCalcDesc" +
                            " ,[loadedWeight] = @loadedWeight " +
                            " ,[loadedBundles] = @loadedBundles " +
                            " ,[calcTareWeight] = @calcTareWeight " +
                            " ,[weightMeasureId] = @weightMeasureId " +
                            " ,[updatedBy] = @updatedBy " +
                            " ,[updatedOn] = @updatedOn " +
                            " ,[isAllowWeighingMachine] = @isAllowWeighingMachine " +
                            " ,[cdStructureId] = @cdStructureId " +
                            " ,[cdStructure] = @cdStructure " +
                            " ,[prodItemDesc] = @prodItemDesc " +
                            " ,[prodItemId] = @prodItemId " +
                            " ,[taxableRateMT] = @taxableRateMT " +
                            " ,[freExpOtherAmt] = @freExpOtherAmt " +
                            " ,[cdApplicableAmt] = @cdApplicableAmt " +
                            " ,[weighingSequenceNumber] = @weighingSequenceNumber " +
                            " ,[modbusRefId] = @ModbusRefId " +
                            
                            " WHERE [idLoadingSlipExt] = @IdLoadingSlipExt ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdLoadingSlipExt", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtTO.IdLoadingSlipExt;
            cmdUpdate.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.BookingId);
            cmdUpdate.Parameters.Add("@LoadingSlipId", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtTO.LoadingSlipId;
            cmdUpdate.Parameters.Add("@LoadingLayerid", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtTO.LoadingLayerid;
            cmdUpdate.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.MaterialId);
            //cmdUpdate.Parameters.Add("@BookingExtId", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtTO.BookingExtId;
            cmdUpdate.Parameters.Add("@LoadingQty", System.Data.SqlDbType.NVarChar).Value = tblLoadingSlipExtTO.LoadingQty;
            cmdUpdate.Parameters.Add("@prodCatId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.ProdCatId);
            cmdUpdate.Parameters.Add("@prodSpecId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.ProdSpecId);
            cmdUpdate.Parameters.Add("@quotaBforeLoading", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.QuotaBforeLoading);
            cmdUpdate.Parameters.Add("@quotaAfterLoading", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.QuotaAfterLoading);
            cmdUpdate.Parameters.Add("@loadingQuotaId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.LoadingQuotaId);
            cmdUpdate.Parameters.Add("@bundles", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.Bundles);
            cmdUpdate.Parameters.Add("@parityDtlId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.ParityDtlId);
            cmdUpdate.Parameters.Add("@ratePerMT", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue( tblLoadingSlipExtTO.RatePerMT);
            cmdUpdate.Parameters.Add("@rateCalcDesc", System.Data.SqlDbType.NVarChar,256).Value = Constants.GetSqlDataValueNullForBaseValue( tblLoadingSlipExtTO.RateCalcDesc);
            cmdUpdate.Parameters.Add("@loadedWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.LoadedWeight);
            cmdUpdate.Parameters.Add("@loadedBundles", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.LoadedBundles);
            cmdUpdate.Parameters.Add("@calcTareWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.CalcTareWeight);
            cmdUpdate.Parameters.Add("@weightMeasureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.WeightMeasureId);
            cmdUpdate.Parameters.Add("@updatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.UpdatedBy);
            cmdUpdate.Parameters.Add("@updatedOn", System.Data.SqlDbType.DateTime).Value = Constants.ServerDateTime;
            cmdUpdate.Parameters.Add("@isAllowWeighingMachine", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtTO.IsAllowWeighingMachine;
            cmdUpdate.Parameters.Add("@cdStructureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.CdStructureId);
            cmdUpdate.Parameters.Add("@cdStructure", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.CdStructure);
            cmdUpdate.Parameters.Add("@prodItemDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.ProdItemDesc);
            cmdUpdate.Parameters.Add("@prodItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.ProdItemId);
            cmdUpdate.Parameters.Add("@taxableRateMT", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.TaxableRateMT);
            cmdUpdate.Parameters.Add("@freExpOtherAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.FreExpOtherAmt);
            cmdUpdate.Parameters.Add("@cdApplicableAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.CdApplicableAmt);
            cmdUpdate.Parameters.Add("@weighingSequenceNumber", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.WeighingSequenceNumber);
            cmdUpdate.Parameters.Add("@ModbusRefId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipExtTO.ModbusRefId);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblLoadingSlipExt(Int32 idLoadingSlipExt)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idLoadingSlipExt, cmdDelete);
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

        public static int DeleteTblLoadingSlipExt(Int32 idLoadingSlipExt, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idLoadingSlipExt, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idLoadingSlipExt, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tempLoadingSlipExt] " +
            " WHERE idLoadingSlipExt = " + idLoadingSlipExt + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idLoadingSlipExt", System.Data.SqlDbType.Int).Value = tblLoadingSlipExtTO.IdLoadingSlipExt;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
