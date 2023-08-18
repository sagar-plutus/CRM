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
    public class TblLoadingQuotaDeclarationDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT loadingQuota.* ,org.firmName as cnfOrgName, material.materialSubType as materialDesc " +
                                  " ,prodCat.prodCateDesc AS prodCatDesc, prodSpec.prodSpecDesc, (prodclass.displayName+'/'+prodItem.itemName) as displayName " +
                                  " FROM tblLoadingQuotaDeclaration loadingQuota " +
                                  " LEFT JOIN tblOrganization org " +
                                  " ON loadingQuota.cnfOrgId = org.idOrganization " +
                                  " LEFT JOIN tblMaterial material " +
                                  " ON material.idMaterial = loadingQuota.materialId " +
                                  " LEFT JOIN dimProdCat prodCat ON prodCat.idProdCat=loadingQuota.prodCatId" +
                                  " LEFT JOIN dimProdSpec prodSpec ON prodSpec.idProdSpec=loadingQuota.prodSpecId"+
                                    " LEFT JOIN tblProductItem prodItem ON prodItem.idProdItem=loadingQuota.prodItemId " +
                                    " LEFT JOIN tblProdClassification prodclass ON prodclass.idProdClass = prodItem.prodClassId ";

            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblLoadingQuotaDeclarationTO> SelectAllTblLoadingQuotaDeclaration()
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
                List<TblLoadingQuotaDeclarationTO> list = ConvertDTToList(sqlReader);
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

        public static List<TblLoadingQuotaDeclarationTO> SelectAllTblLoadingQuotaDeclaration(DateTime declarationDate)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE DAY(loadingQuota.createdOn)=" + declarationDate.Day + " AND MONTH(loadingQuota.createdOn)=" + declarationDate.Month + " AND YEAR(loadingQuota.createdOn)=" + declarationDate.Year;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingQuotaDeclarationTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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

        public static Boolean IsLoadingQuotaDeclaredForTheDate(DateTime declarationDate, Int32 prodCatId, Int32 prodSpecId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT ISNULL(COUNT(*),0) AS recordCnt FROM tblLoadingQuotaDeclaration " + " WHERE DAY(createdOn)=" + declarationDate.Day + " AND MONTH(createdOn)=" + declarationDate.Month + " AND YEAR(createdOn)=" + declarationDate.Year +
                                        " AND prodCatId=" + prodCatId + " AND prodSpecId=" + prodSpecId;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                int recordCnt = 0;
                while (sqlReader.Read())
                {
                    if (sqlReader["recordCnt"] != DBNull.Value)
                    {
                        recordCnt = Convert.ToInt32(sqlReader["recordCnt"].ToString());
                    }
                }

                sqlReader.Dispose();
                if (recordCnt == 0)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static Boolean IsLoadingQuotaDeclaredForTheDate(DateTime declarationDate, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = " SELECT ISNULL(COUNT(*),0) AS recordCnt FROM tblLoadingQuotaDeclaration " + " WHERE DAY(createdOn)=" + declarationDate.Day + " AND MONTH(createdOn)=" + declarationDate.Month + " AND YEAR(createdOn)=" + declarationDate.Year;

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                int recordCnt = 0;
                while (sqlReader.Read())
                {
                    if (sqlReader["recordCnt"] != DBNull.Value)
                    {
                        recordCnt = Convert.ToInt32(sqlReader["recordCnt"].ToString());
                    }
                }

                if (recordCnt == 0)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static List<TblLoadingQuotaDeclarationTO> SelectAllTblLoadingQuotaDeclaration(int cnfId , DateTime declarationDate)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE cnfOrgId=" + cnfId + " AND loadingQuota.isActive=1 AND DAY(loadingQuota.createdOn)=" + declarationDate.Day + " AND MONTH(loadingQuota.createdOn)=" + declarationDate.Month + " AND YEAR(loadingQuota.createdOn)=" + declarationDate.Year +
                                       " ORDER BY prodSpec.displaySequence";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingQuotaDeclarationTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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

        public static List<TblLoadingQuotaDeclarationTO> SelectLatestCalculatedLoadingQuotaDeclarationList(DateTime stockDate, Int32 prodCatId, Int32 prodSpecId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "  SELECT 0 AS idLoadingQuota,0 AS updatedBy , null as updatedOn , cnfList.firmName cnfOrgName ,stockConfig.* " +
                                        "  FROM tblOrganization cnfList " +
                                        "  INNER JOIN " +
                                        "  ( " +
                                        "    SELECT allConfig.idLoadQuotaConfig AS loadQuotaConfigId ,allConfig.*, material.materialSubType AS materialDesc, stockDtl.totalStock, " +
                                        "    CASE WHEN ISNULL(stockDtl.totalStock, 0) > 0 THEN " +
                                        "    CAST( stockDtl.totalStock * ((ISNULL(allocPct, 0) * stockDtl.totalStock) / stockDtl.totalStock) / 100 AS INT)   " +   //To Round Down the values. Suggested By Nitin K [Meeting Ref 31-03-2017]
                                        "    ELSE 0 END AS allocQuota, " +
                                        "    CASE WHEN ISNULL(stockDtl.totalStock, 0) > 0 THEN " +
                                        "    CAST( stockDtl.totalStock * ((ISNULL(allocPct, 0) * stockDtl.totalStock) / stockDtl.totalStock) / 100 AS INT)   " +
                                        "    ELSE 0 END AS balanceQuota " +
                                        "    ,prodCat.prodCateDesc AS prodCatDesc" +
                                        "    ,prodSpec.prodSpecDesc AS prodSpecDesc" +
                                        "    ,0 AS transferedQuota" +
                                        "    ,0 AS receivedQuota" +
                                        "    FROM tblLoadingQuotaConfig allConfig " +
                                        "    INNER JOIN " +
                                        "    ( " +
                                        "        SELECT cnfOrgId, prodCatId ,prodSpecId,materialId, MAX(createdOn)createdOn " +
                                        "        FROM tblLoadingQuotaConfig " +
                                        "        WHERE prodCatId=" + prodCatId + " AND prodSpecId=" + prodSpecId +
                                        "        GROUP BY cnfOrgId, prodCatId,prodSpecId,materialId " +
                                        "    ) latestConfig " +
                                        "    ON allConfig.cnfOrgId = latestConfig.cnfOrgId " +
                                        "    AND allConfig.materialId = latestConfig.materialId " +
                                        "    AND allConfig.createdOn = latestConfig.createdOn " +
                                        "    AND allConfig.prodCatId = latestConfig.prodCatId " +
                                        "    AND allConfig.prodSpecId = latestConfig.prodSpecId " +
                                        "    LEFT JOIN " +
                                        "    ( " +
                                        "        SELECT stockSummaryId, prodCatId,prodSpecId,materialId, SUM(dtl.noOfBundles)noOfBundles, SUM(dtl.totalStock) totalStock " +
                                        "        FROM tblStockDetails dtl " +
                                        "        INNER JOIN tblStockSummary summary " +
                                        "        ON dtl.stockSummaryId = summary.idStockSummary " +
                                        "        WHERE DAY(stockDate)=" + stockDate.Day +
                                        "        AND MONTH(stockDate)=" + stockDate.Month + " AND YEAR(stockDate) = " + stockDate.Year +
                                        "        AND prodCatId=" + prodCatId + " AND prodSpecId=" + prodSpecId +
                                        "        GROUP BY stockSummaryId,prodCatId,prodSpecId, materialId " +
                                        "    ) stockDtl " +
                                        "    ON stockDtl.materialId = allConfig.materialId " +
                                        "    AND allConfig.prodSpecId = stockDtl.prodSpecId " +
                                        "    AND allConfig.prodCatId = stockDtl.prodCatId " +
                                        "    LEFT JOIN tblMaterial material " +
                                        "    ON material.idMaterial = allConfig.materialId " +
                                        "    LEFT JOIN dimProdCat prodCat " +
                                        "    ON prodCat.idProdCat = allConfig.prodCatId " +
                                        "    LEFT JOIN dimProdSpec prodSpec " +
                                        "    ON prodSpec.idProdSpec = allConfig.prodSpecId " +
                                        "    WHERE allConfig.prodCatId=" + prodCatId + " AND allConfig.prodSpecId=" + prodSpecId +
                                        " ) stockConfig " +
                                        " ON cnfList.idOrganization = stockConfig.cnfOrgId " +
                                        " WHERE orgTypeId = " + (int)Constants.OrgTypeE.C_AND_F_AGENT +
                                        " ORDER BY materialId ASC";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@stockDate", System.Data.SqlDbType.DateTime).Value = stockDate.Date;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingQuotaDeclarationTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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

        public static List<TblLoadingQuotaDeclarationTO> SelectLatestCalculatedLoadingQuotaDeclarationList(DateTime stockDate,Int32 cnfId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            String whereCond = string.Empty;
            try
            {
                if(cnfId >0)
                {
                    whereCond = " AND cnfList.idOrganization=" + cnfId;
                }
                //cmdSelect.CommandText = "  SELECT 0 AS idLoadingQuota,0 AS updatedBy , null as updatedOn , cnfList.firmName cnfOrgName ,stockConfig.* " +
                //                        "  FROM tblOrganization cnfList " +
                //                        "  INNER JOIN " +
                //                        "  ( " +
                //                        "    SELECT allConfig.idLoadQuotaConfig AS loadQuotaConfigId ,allConfig.*, material.materialSubType AS materialDesc, stockDtl.totalStock, " +
                //                        "    CASE WHEN ISNULL(stockDtl.totalStock, 0) > 0 THEN " +
                //                        "    CAST( stockDtl.totalStock * ((ISNULL(allocPct, 0) * stockDtl.totalStock) / stockDtl.totalStock) / 100 AS INT)   " +   //To Round Down the values. Suggested By Nitin K [Meeting Ref 31-03-2017]
                //                        "    ELSE 0 END AS allocQuota, " +
                //                        "    CASE WHEN ISNULL(stockDtl.totalStock, 0) > 0 THEN " +
                //                        "    CAST( stockDtl.totalStock * ((ISNULL(allocPct, 0) * stockDtl.totalStock) / stockDtl.totalStock) / 100 AS INT)   " +
                //                        "    ELSE 0 END AS balanceQuota " +
                //                        "    ,prodCat.prodCateDesc AS prodCatDesc" +
                //                        "    ,prodSpec.prodSpecDesc AS prodSpecDesc" +
                //                        "    ,0 AS transferedQuota" +
                //                        "    ,0 AS receivedQuota" +
                //                        "    FROM tblLoadingQuotaConfig allConfig " +
                //                        "    INNER JOIN " +
                //                        "    ( " +
                //                        "        SELECT cnfOrgId, prodCatId ,prodSpecId,materialId, MAX(createdOn)createdOn " +
                //                        "        FROM tblLoadingQuotaConfig " +
                //                        "        GROUP BY cnfOrgId, prodCatId,prodSpecId,materialId " +
                //                        "    ) latestConfig " +
                //                        "    ON allConfig.cnfOrgId = latestConfig.cnfOrgId " +
                //                        "    AND allConfig.materialId = latestConfig.materialId " +
                //                        "    AND allConfig.createdOn = latestConfig.createdOn " +
                //                        "    AND allConfig.prodCatId = latestConfig.prodCatId " +
                //                        "    AND allConfig.prodSpecId = latestConfig.prodSpecId " +
                //                        "    LEFT JOIN " +
                //                        "    ( " +
                //                        "        SELECT stockSummaryId, prodCatId,prodSpecId,materialId, SUM(dtl.noOfBundles)noOfBundles, SUM(dtl.totalStock) - SUM(dtl.removedStock) AS totalStock " +
                //                        "        FROM tblStockDetails dtl " +
                //                        "        INNER JOIN tblStockSummary summary " +
                //                        "        ON dtl.stockSummaryId = summary.idStockSummary " +
                //                        "        WHERE DAY(stockDate)=" + stockDate.Day +
                //                        "        AND MONTH(stockDate)=" + stockDate.Month + " AND YEAR(stockDate) = " + stockDate.Year +
                //                        "        GROUP BY stockSummaryId,prodCatId,prodSpecId, materialId " +
                //                        "    ) stockDtl " +
                //                        "    ON stockDtl.materialId = allConfig.materialId " +
                //                        "    AND allConfig.prodSpecId = stockDtl.prodSpecId " +
                //                        "    AND allConfig.prodCatId = stockDtl.prodCatId " +
                //                        "    LEFT JOIN tblMaterial material " +
                //                        "    ON material.idMaterial = allConfig.materialId " +
                //                        "    LEFT JOIN dimProdCat prodCat " +
                //                        "    ON prodCat.idProdCat = allConfig.prodCatId " +
                //                        "    LEFT JOIN dimProdSpec prodSpec " +
                //                        "    ON prodSpec.idProdSpec = allConfig.prodSpecId " +
                //                        " ) stockConfig " +
                //                        " ON cnfList.idOrganization = stockConfig.cnfOrgId " +
                //                        " WHERE orgTypeId = " + (int)Constants.OrgTypeE.C_AND_F_AGENT + whereCond +
                //                        " ORDER BY materialId ASC";

                cmdSelect.CommandText = "  SELECT 0 AS idLoadingQuota,0 AS updatedBy , null as updatedOn , cnfList.firmName cnfOrgName ,stockConfig.* " +
                                        "  FROM tblOrganization cnfList " +
                                        "  INNER JOIN " +
                                        "  ( " +
                                        "    SELECT allConfig.idLoadQuotaConfig AS loadQuotaConfigId ,allConfig.*, material.materialSubType AS materialDesc, stockDtl.totalStock, " +
                                        "    CASE WHEN ISNULL(stockDtl.totalStock, 0) > 0 THEN " +
                                        "    CAST( stockDtl.totalStock * ((ISNULL(allocPct, 0) * stockDtl.totalStock) / stockDtl.totalStock) / 100 AS INT)   " +   //To Round Down the values. Suggested By Nitin K [Meeting Ref 31-03-2017]
                                        "    ELSE 0 END AS allocQuota, " +
                                        "    CASE WHEN ISNULL(stockDtl.totalStock, 0) > 0 THEN " +
                                        "    CAST( stockDtl.totalStock * ((ISNULL(allocPct, 0) * stockDtl.totalStock) / stockDtl.totalStock) / 100 AS INT)   " +
                                        "    ELSE 0 END AS balanceQuota " +
                                        "    ,prodCat.prodCateDesc AS prodCatDesc" +
                                        "    ,prodSpec.prodSpecDesc AS prodSpecDesc" +
                                        "    ,(prodClass.displayName+'/'+productItem.itemName) as displayName "+
                                        "    ,0 AS transferedQuota" +
                                        "    ,0 AS receivedQuota" +
                                        "    FROM tblLoadingQuotaConfig allConfig " +
                                        "    INNER JOIN " +
                                        "    ( " +
                                        "        SELECT cnfOrgId, prodCatId ,prodSpecId,materialId,prodItemId, MAX(createdOn)createdOn " +
                                        "        FROM tblLoadingQuotaConfig " +
                                        "        GROUP BY cnfOrgId, prodCatId,prodSpecId,materialId,prodItemId " +
                                        "    ) latestConfig " +
                                        "    ON ISNULL(allConfig.cnfOrgId,0) = ISNULL(latestConfig.cnfOrgId,0) " +
                                        "    AND ISNULL(allConfig.materialId,0) = ISNULL(latestConfig.materialId,0) " +
                                        "    AND allConfig.createdOn = latestConfig.createdOn " +
                                        "    AND ISNULL(allConfig.prodCatId,0) = ISNULL(latestConfig.prodCatId,0) " +
                                        "    AND ISNULL(allConfig.prodSpecId,0) = ISNULL(latestConfig.prodSpecId,0) " +
                                        "    AND ISNULL(allConfig.prodItemId,0) = ISNULL(latestConfig.prodItemId,0) "+
                                        "    LEFT JOIN " +
                                        "    ( " +
                                        "        SELECT stockSummaryId, prodCatId,prodSpecId,materialId, SUM(dtl.noOfBundles)noOfBundles, SUM(dtl.totalStock) - SUM(dtl.removedStock) AS totalStock " +
                                        "        ,prodItemId FROM tblStockDetails dtl " +
                                        "        INNER JOIN tblStockSummary summary " +
                                        "        ON dtl.stockSummaryId = summary.idStockSummary " +
                                        "        WHERE DAY(stockDate)=" + stockDate.Day +
                                        "        AND MONTH(stockDate)=" + stockDate.Month + " AND YEAR(stockDate) = " + stockDate.Year +
                                        "        GROUP BY stockSummaryId,prodCatId,prodSpecId, materialId ,prodItemId " +
                                        "    ) stockDtl " +
                                        "    ON ISNULL(stockDtl.materialId,0) = ISNULL(allConfig.materialId,0) " +
                                        "    AND ISNULL(allConfig.prodSpecId,0) = ISNULL(stockDtl.prodSpecId,0) " +
                                        "    AND ISNULL(allConfig.prodCatId,0) = ISNULL(stockDtl.prodCatId,0) " +
                                        "    AND ISNULL(allConfig.prodItemId,0) = ISNULL(stockDtl.prodItemId,0) "+
                                        "    LEFT JOIN tblMaterial material " +
                                        "    ON ISNULL(material.idMaterial,0) = ISNULL(allConfig.materialId,0) " +
                                        "    LEFT JOIN dimProdCat prodCat " +
                                        "    ON ISNULL(prodCat.idProdCat,0) = ISNULL(allConfig.prodCatId,0) " +
                                        "    LEFT JOIN dimProdSpec prodSpec " +
                                        "    ON ISNULL(prodSpec.idProdSpec,0) = ISNULL(allConfig.prodSpecId,0) " +
                                        "    LEFT JOIN tblProductItem productItem  "+
                                        "    ON ISNULL(productItem.idProdItem,0)=ISNULL(allConfig.prodItemId,0) "+
                                        "    LEFT JOIN tblProdClassification prodClass "+
                                        "    ON  prodClass.idProdClass=productItem.prodClassId "+
                                        " ) stockConfig " +
                                        " ON cnfList.idOrganization = stockConfig.cnfOrgId " +
                                        " WHERE orgTypeId = " + (int)Constants.OrgTypeE.C_AND_F_AGENT + whereCond +
                                        " ORDER BY materialId ASC";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@stockDate", System.Data.SqlDbType.DateTime).Value = stockDate.Date;
                cmdSelect.CommandTimeout = 100;  //Vijaymala added [05-06-2018] to change default timeout.
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingQuotaDeclarationTO> list = ConvertDTToListV2(sqlReader); //Sudhir[09-APR-2018] change Default ConvertDT to V2.
                sqlReader.Dispose();
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

        public static TblLoadingQuotaDeclarationTO SelectTblLoadingQuotaDeclaration(Int32 idLoadingQuota, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {

                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idLoadingQuota = " + idLoadingQuota + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingQuotaDeclarationTO> list = ConvertDTToList(reader);
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
                if (reader != null)
                    reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        //Vijaymala [04-06-2018] changes the code to save loading quota for regular as well as isstockrequired item 
        
        public static TblLoadingQuotaDeclarationTO SelectLoadingQuotaDeclarationTO(Int32 cnfId, Int32 prodCatId, Int32 prodSpecId, Int32 materialId,Int32 prodItemId, DateTime quotaDate, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                //Vijaymala[19-01-2018]Added isActive condition as per discussion with Saket 
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE cnfOrgId = " + cnfId + " AND ISNULL(prodCatId,0)=" + prodCatId + " AND ISNULL(prodSpecId,0)=" + prodSpecId + " AND ISNULL(materialId,0)=" + materialId + " AND ISNULL(prodItemId,0)=" + prodItemId +
                                        " AND loadingQuota.isActive=1 AND DAY(loadingQuota.createdOn)=" + quotaDate.Day + " AND MONTH(loadingQuota.createdOn)=" + quotaDate.Month + " AND YEAR(loadingQuota.createdOn)=" + quotaDate.Year ;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingQuotaDeclarationTO> list = ConvertDTToList(reader);
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
                if (reader != null)
                    reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static List<TblLoadingQuotaDeclarationTO> SelectLoadingQuotaDeclaredForCnfAndDate(Int32 cnfOrgId, DateTime quotaDate, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE loadingQuota.isActive=1 AND cnfOrgId=" + cnfOrgId + " AND DAY(loadingQuota.createdOn)=" + quotaDate.Day + " AND MONTH(loadingQuota.createdOn)=" + quotaDate.Month + " AND YEAR(loadingQuota.createdOn)=" + quotaDate.Year;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingQuotaDeclarationTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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

        public static List<TblLoadingQuotaDeclarationTO> SelectAllLoadingQuotaDeclListFromLoadingExt(String loadingSlipExtId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            int txnOpTypeId = (int)Constants.TxnOperationTypeE.OUT;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE loadingQuota.idLoadingQuota IN(SELECT loadingQuotaId FROM tblLoadingQuotaConsumption where loadingSlipExtId IN(" + loadingSlipExtId + ") AND txnOpTypeId=" + txnOpTypeId + ")";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingQuotaDeclarationTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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

        public static List<TblLoadingQuotaDeclarationTO> ConvertDTToList(SqlDataReader tblLoadingQuotaDeclarationTODT)
        {
            List<TblLoadingQuotaDeclarationTO> tblLoadingQuotaDeclarationTOList = new List<TblLoadingQuotaDeclarationTO>();
            if (tblLoadingQuotaDeclarationTODT != null)
            {
                while (tblLoadingQuotaDeclarationTODT.Read())
                {
                    TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTONew = new TblLoadingQuotaDeclarationTO();
                    if (tblLoadingQuotaDeclarationTODT["idLoadingQuota"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.IdLoadingQuota = Convert.ToInt32(tblLoadingQuotaDeclarationTODT["idLoadingQuota"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["cnfOrgId"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.CnfOrgId = Convert.ToInt32(tblLoadingQuotaDeclarationTODT["cnfOrgId"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["materialId"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.MaterialId = Convert.ToInt32(tblLoadingQuotaDeclarationTODT["materialId"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["loadQuotaConfigId"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.LoadQuotaConfigId = Convert.ToInt32(tblLoadingQuotaDeclarationTODT["loadQuotaConfigId"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["isActive"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.IsActive = Convert.ToInt32(tblLoadingQuotaDeclarationTODT["isActive"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["createdBy"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.CreatedBy = Convert.ToInt32(tblLoadingQuotaDeclarationTODT["createdBy"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["updatedBy"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.UpdatedBy = Convert.ToInt32(tblLoadingQuotaDeclarationTODT["updatedBy"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["createdOn"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.CreatedOn = Convert.ToDateTime(tblLoadingQuotaDeclarationTODT["createdOn"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["updatedOn"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.UpdatedOn = Convert.ToDateTime(tblLoadingQuotaDeclarationTODT["updatedOn"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["allocQuota"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.AllocQuota = Convert.ToDouble(tblLoadingQuotaDeclarationTODT["allocQuota"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["balanceQuota"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.BalanceQuota = Convert.ToDouble(tblLoadingQuotaDeclarationTODT["balanceQuota"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["remark"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.Remark = Convert.ToString(tblLoadingQuotaDeclarationTODT["remark"].ToString());

                    if (string.IsNullOrEmpty(tblLoadingQuotaDeclarationTONew.Remark))
                        tblLoadingQuotaDeclarationTONew.Remark = "New Quota Declaration";

                    if (tblLoadingQuotaDeclarationTODT["cnfOrgName"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.CnfOrgName = Convert.ToString(tblLoadingQuotaDeclarationTODT["cnfOrgName"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["materialDesc"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.MaterialDesc = Convert.ToString(tblLoadingQuotaDeclarationTODT["materialDesc"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["prodCatId"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.ProdCatId = Convert.ToInt32(tblLoadingQuotaDeclarationTODT["prodCatId"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["prodSpecId"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.ProdSpecId = Convert.ToInt32(tblLoadingQuotaDeclarationTODT["prodSpecId"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["prodCatDesc"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.ProdCatDesc = Convert.ToString(tblLoadingQuotaDeclarationTODT["prodCatDesc"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["prodSpecDesc"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.ProdSpecDesc = Convert.ToString(tblLoadingQuotaDeclarationTODT["prodSpecDesc"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["transferedQuota"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.TransferedQuota = Convert.ToDouble(tblLoadingQuotaDeclarationTODT["transferedQuota"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["receivedQuota"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.ReceivedQuota = Convert.ToDouble(tblLoadingQuotaDeclarationTODT["receivedQuota"].ToString());

                    if (tblLoadingQuotaDeclarationTODT["prodItemId"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.ProdItemId = Convert.ToInt32(tblLoadingQuotaDeclarationTODT["prodItemId"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["displayName"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.DisplayName = Convert.ToString(tblLoadingQuotaDeclarationTODT["displayName"].ToString());
                    if(tblLoadingQuotaDeclarationTONew.ProdItemId > 0)
                    {
                        tblLoadingQuotaDeclarationTONew.MaterialDesc = tblLoadingQuotaDeclarationTONew.DisplayName;
                    }
                    //Not Required at this level. Shifted to Stock Detail Level
                    //if (tblLoadingQuotaDeclarationTODT["removedQuota"] != DBNull.Value)
                    //    tblLoadingQuotaDeclarationTONew.RemovedQuota = Convert.ToDouble(tblLoadingQuotaDeclarationTODT["removedQuota"].ToString());
                    tblLoadingQuotaDeclarationTOList.Add(tblLoadingQuotaDeclarationTONew);
                }
            }
            return tblLoadingQuotaDeclarationTOList;
        }

        //Sudhir[09-APR-2018] Added for the new req. of other Item added coloumn prodItemId and display Name.
        public static List<TblLoadingQuotaDeclarationTO> ConvertDTToListV2(SqlDataReader tblLoadingQuotaDeclarationTODT)
        {
            List<TblLoadingQuotaDeclarationTO> tblLoadingQuotaDeclarationTOList = new List<TblLoadingQuotaDeclarationTO>();
            if (tblLoadingQuotaDeclarationTODT != null)
            {
                while (tblLoadingQuotaDeclarationTODT.Read())
                {
                    TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTONew = new TblLoadingQuotaDeclarationTO();
                    if (tblLoadingQuotaDeclarationTODT["idLoadingQuota"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.IdLoadingQuota = Convert.ToInt32(tblLoadingQuotaDeclarationTODT["idLoadingQuota"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["cnfOrgId"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.CnfOrgId = Convert.ToInt32(tblLoadingQuotaDeclarationTODT["cnfOrgId"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["materialId"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.MaterialId = Convert.ToInt32(tblLoadingQuotaDeclarationTODT["materialId"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["loadQuotaConfigId"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.LoadQuotaConfigId = Convert.ToInt32(tblLoadingQuotaDeclarationTODT["loadQuotaConfigId"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["isActive"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.IsActive = Convert.ToInt32(tblLoadingQuotaDeclarationTODT["isActive"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["createdBy"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.CreatedBy = Convert.ToInt32(tblLoadingQuotaDeclarationTODT["createdBy"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["updatedBy"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.UpdatedBy = Convert.ToInt32(tblLoadingQuotaDeclarationTODT["updatedBy"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["createdOn"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.CreatedOn = Convert.ToDateTime(tblLoadingQuotaDeclarationTODT["createdOn"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["updatedOn"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.UpdatedOn = Convert.ToDateTime(tblLoadingQuotaDeclarationTODT["updatedOn"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["allocQuota"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.AllocQuota = Convert.ToDouble(tblLoadingQuotaDeclarationTODT["allocQuota"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["balanceQuota"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.BalanceQuota = Convert.ToDouble(tblLoadingQuotaDeclarationTODT["balanceQuota"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["remark"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.Remark = Convert.ToString(tblLoadingQuotaDeclarationTODT["remark"].ToString());

                    if (string.IsNullOrEmpty(tblLoadingQuotaDeclarationTONew.Remark))
                        tblLoadingQuotaDeclarationTONew.Remark = "New Quota Declaration";

                    if (tblLoadingQuotaDeclarationTODT["cnfOrgName"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.CnfOrgName = Convert.ToString(tblLoadingQuotaDeclarationTODT["cnfOrgName"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["materialDesc"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.MaterialDesc = Convert.ToString(tblLoadingQuotaDeclarationTODT["materialDesc"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["prodCatId"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.ProdCatId = Convert.ToInt32(tblLoadingQuotaDeclarationTODT["prodCatId"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["prodSpecId"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.ProdSpecId = Convert.ToInt32(tblLoadingQuotaDeclarationTODT["prodSpecId"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["prodCatDesc"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.ProdCatDesc = Convert.ToString(tblLoadingQuotaDeclarationTODT["prodCatDesc"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["prodSpecDesc"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.ProdSpecDesc = Convert.ToString(tblLoadingQuotaDeclarationTODT["prodSpecDesc"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["transferedQuota"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.TransferedQuota = Convert.ToDouble(tblLoadingQuotaDeclarationTODT["transferedQuota"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["receivedQuota"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.ReceivedQuota = Convert.ToDouble(tblLoadingQuotaDeclarationTODT["receivedQuota"].ToString());

                    if (tblLoadingQuotaDeclarationTODT["prodItemId"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.ProdItemId = Convert.ToInt32(tblLoadingQuotaDeclarationTODT["prodItemId"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["displayName"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.DisplayName = Convert.ToString(tblLoadingQuotaDeclarationTODT["displayName"].ToString());
                    //Not Required at this level. Shifted to Stock Detail Level
                    //if (tblLoadingQuotaDeclarationTODT["removedQuota"] != DBNull.Value)
                    //    tblLoadingQuotaDeclarationTONew.RemovedQuota = Convert.ToDouble(tblLoadingQuotaDeclarationTODT["removedQuota"].ToString());
                    tblLoadingQuotaDeclarationTOList.Add(tblLoadingQuotaDeclarationTONew);
                }
            }
            return tblLoadingQuotaDeclarationTOList;
        }



        #endregion

        #region Insertion
        public static int InsertTblLoadingQuotaDeclaration(TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblLoadingQuotaDeclarationTO, cmdInsert);
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

        public static int InsertTblLoadingQuotaDeclaration(TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblLoadingQuotaDeclarationTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblLoadingQuotaDeclaration]( " +
                                "  [cnfOrgId]" +
                                " ,[materialId]" +
                                " ,[loadQuotaConfigId]" +
                                " ,[isActive]" +
                                " ,[createdBy]" +
                                " ,[updatedBy]" +
                                " ,[createdOn]" +
                                " ,[updatedOn]" +
                                " ,[allocQuota]" +
                                " ,[balanceQuota]" +
                                " ,[remark]" +
                                " ,[prodCatId]" +
                                " ,[prodSpecId]" +
                                " ,[transferedQuota]" +
                                " ,[receivedQuota]" +
                                " ,[prodItemId]"+
                                " )" +
                    " VALUES (" +
                                "  @CnfOrgId " +
                                " ,@MaterialId " +
                                " ,@LoadQuotaConfigId " +
                                " ,@IsActive " +
                                " ,@CreatedBy " +
                                " ,@UpdatedBy " +
                                " ,@CreatedOn " +
                                " ,@UpdatedOn " +
                                " ,@AllocQuota " +
                                " ,@BalanceQuota " +
                                " ,@Remark " +
                                " ,@prodCatId " +
                                " ,@prodSpecId " +
                                " ,@transferedQuota " +
                                " ,@receivedQuota " +
                                " ,@ProdItemId"+
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdLoadingQuota", System.Data.SqlDbType.Int).Value = tblLoadingQuotaDeclarationTO.IdLoadingQuota;
            cmdInsert.Parameters.Add("@CnfOrgId", System.Data.SqlDbType.Int).Value = tblLoadingQuotaDeclarationTO.CnfOrgId;
            cmdInsert.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingQuotaDeclarationTO.MaterialId);
            cmdInsert.Parameters.Add("@LoadQuotaConfigId", System.Data.SqlDbType.Int).Value = tblLoadingQuotaDeclarationTO.LoadQuotaConfigId;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblLoadingQuotaDeclarationTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLoadingQuotaDeclarationTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingQuotaDeclarationTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingQuotaDeclarationTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingQuotaDeclarationTO.UpdatedOn);
            cmdInsert.Parameters.Add("@AllocQuota", System.Data.SqlDbType.NVarChar).Value = tblLoadingQuotaDeclarationTO.AllocQuota;
            cmdInsert.Parameters.Add("@BalanceQuota", System.Data.SqlDbType.NVarChar).Value = tblLoadingQuotaDeclarationTO.BalanceQuota;
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value =Constants.GetSqlDataValueNullForBaseValue(tblLoadingQuotaDeclarationTO.Remark);
            cmdInsert.Parameters.Add("@prodCatId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingQuotaDeclarationTO.ProdCatId);
            cmdInsert.Parameters.Add("@prodSpecId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingQuotaDeclarationTO.ProdSpecId);
            cmdInsert.Parameters.Add("@transferedQuota", System.Data.SqlDbType.NVarChar).Value = tblLoadingQuotaDeclarationTO.TransferedQuota;
            cmdInsert.Parameters.Add("@receivedQuota", System.Data.SqlDbType.NVarChar).Value = tblLoadingQuotaDeclarationTO.ReceivedQuota;
            cmdInsert.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblLoadingQuotaDeclarationTO.ProdItemId);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblLoadingQuotaDeclarationTO.IdLoadingQuota = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblLoadingQuotaDeclaration(TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblLoadingQuotaDeclarationTO, cmdUpdate);
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

        public static int UpdateTblLoadingQuotaDeclaration(TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblLoadingQuotaDeclarationTO, cmdUpdate);
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

        public static int DeactivateAllPrevLoadingQuota(Int32 updatedBy,int prodCatId,Int32 prodSpecId,  SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [tblLoadingQuotaDeclaration] SET " +
                                "  [isActive]= @IsActive" +
                                " ,[updatedBy]= @UpdatedBy" +
                                " ,[updatedOn]= @UpdatedOn" +
                                " ,[balanceQuota]= @BalanceQuota" +
                                " ,[remark] = @Remark" +
                                "  WHERE isActive=1 AND prodCatId=" + prodCatId + " AND prodSpecId=" + prodSpecId;

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = 0;
                cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = updatedBy;
                cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.ServerDateTime;
                cmdUpdate.Parameters.Add("@BalanceQuota", System.Data.SqlDbType.NVarChar).Value = 0;
                cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = "Old Loading Quota Deactivated";
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

        public static int DeactivateAllPrevLoadingQuota(Int32 updatedBy, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [tblLoadingQuotaDeclaration] SET " +
                                "  [isActive]= @IsActive" +
                                " ,[updatedBy]= @UpdatedBy" +
                                " ,[updatedOn]= @UpdatedOn" +
                                " ,[balanceQuota]= @BalanceQuota" +
                                " ,[remark] = @Remark" +
                                "  WHERE isActive=1 ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = 0;
                cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = updatedBy;
                cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.ServerDateTime;
                cmdUpdate.Parameters.Add("@BalanceQuota", System.Data.SqlDbType.NVarChar).Value = 0;
                cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = "Old Loading Quota Deactivated";
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

        public static int ExecuteUpdationCommand(TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblLoadingQuotaDeclaration] SET " +
                            "  [cnfOrgId]= @CnfOrgId" +
                            " ,[materialId]= @MaterialId" +
                            " ,[loadQuotaConfigId]= @LoadQuotaConfigId" +
                            " ,[isActive]= @IsActive" +
                            " ,[updatedBy]= @UpdatedBy" +
                            " ,[updatedOn]= @UpdatedOn" +
                            " ,[allocQuota]= @AllocQuota" +
                            " ,[balanceQuota]= @BalanceQuota" +
                            " ,[remark] = @Remark" +
                            " ,[prodCatId] = @prodCatId" +
                            " ,[prodSpecId] = @prodSpecId" +
                            " ,[transferedQuota] = @transferedQuota" +
                            " ,[receivedQuota] = @receivedQuota" +
                            " ,[prodItemId]=@ProdItemId"+
                            " WHERE [idLoadingQuota] = @IdLoadingQuota";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdLoadingQuota", System.Data.SqlDbType.Int).Value = tblLoadingQuotaDeclarationTO.IdLoadingQuota;
            cmdUpdate.Parameters.Add("@CnfOrgId", System.Data.SqlDbType.Int).Value = tblLoadingQuotaDeclarationTO.CnfOrgId;
            cmdUpdate.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingQuotaDeclarationTO.MaterialId);
            cmdUpdate.Parameters.Add("@LoadQuotaConfigId", System.Data.SqlDbType.Int).Value = tblLoadingQuotaDeclarationTO.LoadQuotaConfigId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblLoadingQuotaDeclarationTO.IsActive;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblLoadingQuotaDeclarationTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingQuotaDeclarationTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@AllocQuota", System.Data.SqlDbType.NVarChar).Value = tblLoadingQuotaDeclarationTO.AllocQuota;
            cmdUpdate.Parameters.Add("@BalanceQuota", System.Data.SqlDbType.NVarChar).Value = tblLoadingQuotaDeclarationTO.BalanceQuota;
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingQuotaDeclarationTO.Remark);
            cmdUpdate.Parameters.Add("@prodCatId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingQuotaDeclarationTO.ProdCatId);
            cmdUpdate.Parameters.Add("@prodSpecId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingQuotaDeclarationTO.ProdSpecId);
            cmdUpdate.Parameters.Add("@transferedQuota", System.Data.SqlDbType.NVarChar).Value = tblLoadingQuotaDeclarationTO.TransferedQuota;
            cmdUpdate.Parameters.Add("@receivedQuota", System.Data.SqlDbType.NVarChar).Value = tblLoadingQuotaDeclarationTO.ReceivedQuota;
            cmdUpdate.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingQuotaDeclarationTO.ProdItemId);

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblLoadingQuotaDeclaration(Int32 idLoadingQuota)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idLoadingQuota, cmdDelete);
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

        public static int DeleteTblLoadingQuotaDeclaration(Int32 idLoadingQuota, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idLoadingQuota, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idLoadingQuota, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblLoadingQuotaDeclaration] " +
            " WHERE idLoadingQuota = " + idLoadingQuota +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idLoadingQuota", System.Data.SqlDbType.Int).Value = tblLoadingQuotaDeclarationTO.IdLoadingQuota;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
