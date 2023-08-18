using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblProductItemDAO : ITblProductItemDAO
    {

        private readonly IConnectionString _iConnectionString;

        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO ;
        public TblProductItemDAO(IConnectionString iConnectionString,ITblConfigParamsDAO iTblConfigParamsDAO)
        {
            _iConnectionString = iConnectionString;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
        }
        #region Methods

        public String SqlSelectQuery()
        {
            String sqlSelectQry = " select tblProductItem.*,tblProdClassification.prodClassDesc,abc.* from tblProductItem tblProductItem " +
                                  " left join tblProdClassification tblProdClassification on tblProdClassification.idProdClass=tblProductItem.prodClassId ";
            return sqlSelectQry;
        }

        public String SqlQuery()
        {
            String sqlSelectQry = " select tblProductItem.*,tblProdClassification.prodClassDesc from tblProductItem tblProductItem " +
                                  " left join tblProdClassification tblProdClassification on tblProdClassification.idProdClass=tblProductItem.prodClassId ";
            return sqlSelectQry;
        }

        #endregion

        #region Selection

        public List<TblProductItemTO> SelectAllTblProductItem(Int32 specificationId = 0)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING); ;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {


                conn.Open();
                if (specificationId == 0)
                    cmdSelect.CommandText = SqlQuery() + " WHERE tblProductItem.isActive=1";
                else
                    cmdSelect.CommandText = SqlQuery() + " WHERE tblProductItem.isActive=1 AND tblProductItem.prodClassId=" + specificationId;

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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblProductItemTO> SelectAllTblProductItemListWithParityAndRecovery(Int32 specificationId, Int32 stateId = 0)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING); ;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " LEFT JOIN tblParityDetailsPurchase" +
                                                           " ON tblParityDetailsPurchase.prodItemId = tblProductItem.idProdItem " +
                                                           " LEFT JOIN tblParitySummaryPurchase" +
                                                           " ON tblParitySummaryPurchase.idParityPurchase = tblParityDetailsPurchase.parityPurchaseId " +
                                                           " AND tblParitySummaryPurchase.stateId=" + stateId +
                                                           " WHERE [tblProductItem].prodClassId=" + specificationId + " AND tblParitySummaryPurchase.isActive = 1" +
                                                           " ORDER BY tblParitySummaryPurchase.createdOn desc";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                Boolean isParityDetails = true;
                List<TblProductItemTO> list = ConvertDTToList(reader, isParityDetails);

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

        public TblProductItemTO GetBaseItemRecovery(Int32 isBaseItem = 0, Int32 stateId = 0)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING); ;

            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();


                cmdSelect.CommandText = SqlSelectQuery() + " LEFT JOIN " +
                                                      " ( SELECT tblParityDetailsPurchase.* FROM tblParityDetailsPurchase LEFT JOIN tblParitySummaryPurchase " +
                                                      " ON tblParityDetailsPurchase.parityPurchaseId = tblParitySummaryPurchase.idParityPurchase where tblParitySummaryPurchase.stateId= " + stateId + " and tblParitySummaryPurchase.isActive = 1) as abc " +
                                                      " ON abc.prodItemId = tblProductItem.idProdItem " +
                                                      " where tblProductItem.isBaseItemForRate =" + isBaseItem + " and tblProductItem.isActive = 1 order by displaySequanceNo asc";


                // cmdSelect.CommandText = SqlSelectQuery() + " LEFT JOIN tblParityDetailsPurchase" +
                //                                             " ON tblParityDetailsPurchase.prodItemId = tblProductItem.idProdItem " +
                //                                             " LEFT JOIN tblParitySummaryPurchase" +
                //                                             " ON tblParitySummaryPurchase.idParityPurchase = tblParityDetailsPurchase.parityPurchaseId " +
                //                                             " AND tblParitySummaryPurchase.stateId=" + stateId +
                //                                             " WHERE ((tblParitySummaryPurchase.isActive = 1 OR tblParitySummaryPurchase.isActive IS NULL))" +
                //                                             " AND tblParitySummaryPurchase.stateId=" + stateId + "and tblProductItem.isBaseItemForRate= " + isBaseItem +
                //                                             " ORDER BY tblParitySummaryPurchase.createdOn desc";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                Boolean isParityDetails = true;
                List<TblProductItemTO> list = ConvertDTToList(reader, isParityDetails);
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public TblProductItemTO GetBaseItemRecovery(Int32 isBaseItem, Int32 stateId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {


                cmdSelect.CommandText = SqlSelectQuery() + " LEFT JOIN " +
                                                    " ( SELECT tblParityDetailsPurchase.* FROM tblParityDetailsPurchase LEFT JOIN tblParitySummaryPurchase " +
                                                    " ON tblParityDetailsPurchase.parityPurchaseId = tblParitySummaryPurchase.idParityPurchase where tblParitySummaryPurchase.stateId= " + stateId + " and tblParitySummaryPurchase.isActive = 1) as abc " +
                                                    " ON abc.prodItemId = tblProductItem.idProdItem " +
                                                    " where tblProductItem.isBaseItemForRate =" + isBaseItem + " and tblProductItem.isActive = 1 order by displaySequanceNo asc";


                // cmdSelect.CommandText = SqlSelectQuery() + " LEFT JOIN tblParityDetailsPurchase" +
                //                                             " ON tblParityDetailsPurchase.prodItemId = tblProductItem.idProdItem " +
                //                                             " LEFT JOIN tblParitySummaryPurchase" +
                //                                             " ON tblParitySummaryPurchase.idParityPurchase = tblParityDetailsPurchase.parityPurchaseId " +
                //                                             " AND tblParitySummaryPurchase.stateId=" + stateId +
                //                                             " WHERE ((tblParitySummaryPurchase.isActive = 1 OR tblParitySummaryPurchase.isActive IS NULL))" +
                //                                             " AND tblParitySummaryPurchase.stateId=" + stateId + "and tblProductItem.isBaseItemForRate= " + isBaseItem +
                //                                             " ORDER BY tblParitySummaryPurchase.createdOn desc";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                Boolean isParityDetails = true;
                List<TblProductItemTO> list = ConvertDTToList(reader, isParityDetails);
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
        public List<TblProductItemTO> SelectAllTblProductItemListWithParityAndRecoveryNew(Int32 specificationId, Int32 stateId = 0)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING); ;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                // cmdSelect.CommandText = SqlSelectQuery() + " LEFT JOIN tblParityDetailsPurchase" +
                //                                            " ON tblParityDetailsPurchase.prodItemId = tblProductItem.idProdItem " +
                //                                            " LEFT JOIN tblParitySummaryPurchase" +
                //                                            " ON tblParitySummaryPurchase.idParityPurchase = tblParityDetailsPurchase.parityPurchaseId " +
                //                                            " AND tblParitySummaryPurchase.stateId=" + stateId +
                //                                            " WHERE tblParitySummaryPurchase.prodClassId=" + specificationId + " AND ((tblParitySummaryPurchase.isActive = 1 OR tblParitySummaryPurchase.isActive IS NULL))" +
                //                                            //" AND tblParitySummaryPurchase.stateId=" + stateId +
                //                                            " ORDER BY tblParitySummaryPurchase.createdOn desc";

                cmdSelect.CommandText = SqlSelectQuery() + " LEFT JOIN " +
                                        " ( SELECT tblParityDetailsPurchase.* FROM tblParityDetailsPurchase LEFT JOIN tblParitySummaryPurchase " +
                                        " ON tblParityDetailsPurchase.parityPurchaseId = tblParitySummaryPurchase.idParityPurchase where tblParitySummaryPurchase.stateId= " + stateId + " and tblParitySummaryPurchase.isActive = 1) as abc " +
                                        " ON abc.prodItemId = tblProductItem.idProdItem " +
                                        " where tblProductItem.prodClassId =" + specificationId + " and tblProductItem.isActive = 1 order by displaySequanceNo asc";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                Boolean isParityDetails = true;
                List<TblProductItemTO> list = ConvertDTToList(reader, isParityDetails);
                if (list.Count == 0)
                {
                    if (reader != null) reader.Dispose();
                    cmdSelect.Dispose();
                    cmdSelect.CommandText = "SELECT idProdItem,prodClassId,'0' as createdBy,'0' as updatedBy,'0001-01-01' as createdOn,'0001-01-01' as updatedOn,itemName,itemDesc," +
                        "'' as remark,'0' as isActive,weightMeasureUnitId,conversionUnitOfMeasure,conversionFactor,isStockRequire,'0' as parityAmt,'0' as nonConfParityAmt," +
                        "'0' as 'recovery' , '0' as 'isBaseItemForRate'  From [tblProductItem] where prodClassId = " + specificationId + " order by displaySequanceNo asc";

                    cmdSelect.Connection = conn;
                    cmdSelect.CommandType = System.Data.CommandType.Text;

                    reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                    isParityDetails = true;
                    list = ConvertDTToList(reader, isParityDetails);
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        public List<TblProductItemTO> GetGradeBookingParityDtls(DateTime saudaCreatedOn,Int32 specificationId, Int32 stateId = 0,Boolean isGetlatestParity = false)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING); ;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            
            try
            {
                //Prajakta][2019-07-29] Added to get latest parity details for sauda is of open qty
                TblConfigParamsTO openQtySaudaConfigParam = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_SCRAP_IS_OPEN_QTY_SAUDA);
                if(openQtySaudaConfigParam != null)
                {
                    if(openQtySaudaConfigParam.ConfigParamVal == "1")
                        isGetlatestParity = true;
                }

                conn.Open();

                
                if (isGetlatestParity)
                {
                    cmdSelect.CommandText = SqlSelectQuery() + " LEFT JOIN " +
                                                          " ( SELECT tblParityDetailsPurchase.* FROM tblParityDetailsPurchase LEFT JOIN tblParitySummaryPurchase " +
                                                          " ON tblParityDetailsPurchase.parityPurchaseId = tblParitySummaryPurchase.idParityPurchase where tblParitySummaryPurchase.stateId= " + stateId + " and tblParitySummaryPurchase.isActive = 1) as abc " +
                                                          " ON abc.prodItemId = tblProductItem.idProdItem " +
                                                          " where tblProductItem.prodClassId =" + specificationId + " and tblProductItem.isActive = 1 order by displaySequanceNo asc";
                    cmdSelect.Connection = conn;
                    cmdSelect.CommandType = System.Data.CommandType.Text;

                }
                else
                {
                    cmdSelect.CommandText = SqlSelectQuery() + " LEFT JOIN " +
                                                           " ( SELECT tblParityDetailsPurchase.* FROM tblParityDetailsPurchase LEFT JOIN tblParitySummaryPurchase " +
                                                           " ON tblParityDetailsPurchase.parityPurchaseId = tblParitySummaryPurchase.idParityPurchase WHERE tblParitySummaryPurchase.stateId= " + stateId
                                                           + " AND tblParitySummaryPurchase.idParityPurchase = (SELECT MAX(idParityPurchase) FROM " +
                                                           " tblParitySummaryPurchase WHERE tblParitySummaryPurchase.createdOn  <= @date " +
                                                            " AND tblParitySummaryPurchase.prodClassId = " + specificationId + " AND tblParitySummaryPurchase.stateId= " + stateId + " ) ) as abc " +
                                                           " ON abc.prodItemId = tblProductItem.idProdItem " +
                                                           " WHERE tblProductItem.prodClassId =" + specificationId + " AND tblProductItem.isActive = 1 order by displaySequanceNo asc";

                    cmdSelect.Connection = conn;
                    cmdSelect.CommandType = System.Data.CommandType.Text;

                    cmdSelect.Parameters.Add("@date", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(saudaCreatedOn);


                }


                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                Boolean isParityDetails = true;
                List<TblProductItemTO> list = ConvertDTToList(reader, isParityDetails);
                if (list.Count == 0)
                {
                    if (reader != null) reader.Dispose();
                    cmdSelect.Dispose();
                    cmdSelect.CommandText = "SELECT idProdItem,prodClassId,'0' as createdBy,'0' as updatedBy,'0001-01-01' as createdOn,'0001-01-01' as updatedOn,itemName,itemDesc," +
                        "'' as remark,'0' as isActive,weightMeasureUnitId,conversionUnitOfMeasure,conversionFactor,isStockRequire,'0' as parityAmt,'0' as nonConfParityAmt," +
                        "'0' as 'recovery' , '0' as 'isBaseItemForRate'  From [tblProductItem] where prodClassId = " + specificationId + " order by displaySequanceNo asc";

                    cmdSelect.Connection = conn;
                    cmdSelect.CommandType = System.Data.CommandType.Text;

                    reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                    isParityDetails = true;
                    list = ConvertDTToList(reader, isParityDetails);
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblProductItemTO> SelectAllTblProductItemListByProdItemId(Int32 prodItemId, Int32 stateId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING); ;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                // cmdSelect.CommandText = SqlSelectQuery() + " LEFT JOIN tblParityDetailsPurchase" +
                //                                            " ON tblParityDetailsPurchase.prodItemId = tblProductItem.idProdItem " +
                //                                            " LEFT JOIN tblParitySummaryPurchase" +
                //                                            " ON tblParitySummaryPurchase.idParityPurchase = tblParityDetailsPurchase.parityPurchaseId " +
                //                                            " AND tblParitySummaryPurchase.stateId=" + stateId +
                //                                            " WHERE tblParitySummaryPurchase.prodClassId=" + specificationId + " AND ((tblParitySummaryPurchase.isActive = 1 OR tblParitySummaryPurchase.isActive IS NULL))" +
                //                                            //" AND tblParitySummaryPurchase.stateId=" + stateId +
                //                                            " ORDER BY tblParitySummaryPurchase.createdOn desc";

                cmdSelect.CommandText = SqlSelectQuery() + " LEFT JOIN " +
                                        " ( SELECT tblParityDetailsPurchase.* FROM tblParityDetailsPurchase LEFT JOIN tblParitySummaryPurchase " +
                                        " ON tblParityDetailsPurchase.parityPurchaseId = tblParitySummaryPurchase.idParityPurchase where tblParitySummaryPurchase.stateId= " + stateId + " and tblParitySummaryPurchase.isActive = 1) as abc " +
                                        " ON abc.prodItemId = tblProductItem.idProdItem " +
                                        " where tblProductItem.idProdItem =" + prodItemId + " and tblProductItem.isActive = 1";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                Boolean isParityDetails = true;
                List<TblProductItemTO> list = ConvertDTToList(reader, isParityDetails);
                if (list.Count == 0)
                {
                    if (reader != null) reader.Dispose();
                    cmdSelect.Dispose();
                    cmdSelect.CommandText = "SELECT idProdItem,prodClassId,'0' as createdBy,'0' as updatedBy,'0001-01-01' as createdOn,'0001-01-01' as updatedOn,itemName,itemDesc," +
                        "'' as remark,'0' as isActive,weightMeasureUnitId,conversionUnitOfMeasure,conversionFactor,isStockRequire,'0' as parityAmt,'0' as nonConfParityAmt," +
                        "'0' as 'recovery' , '0' as 'isBaseItemForRate'  From [tblProductItem] where idProdItem = " + prodItemId;

                    cmdSelect.Connection = conn;
                    cmdSelect.CommandType = System.Data.CommandType.Text;

                    reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                    isParityDetails = true;
                    list = ConvertDTToList(reader, isParityDetails);
                    reader.Close();
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

         public List<TblProductItemTO> SelectAllTblProductItemListByProdItemId(Int32 prodItemId, Int32 stateId,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                // cmdSelect.CommandText = SqlSelectQuery() + " LEFT JOIN tblParityDetailsPurchase" +
                //                                            " ON tblParityDetailsPurchase.prodItemId = tblProductItem.idProdItem " +
                //                                            " LEFT JOIN tblParitySummaryPurchase" +
                //                                            " ON tblParitySummaryPurchase.idParityPurchase = tblParityDetailsPurchase.parityPurchaseId " +
                //                                            " AND tblParitySummaryPurchase.stateId=" + stateId +
                //                                            " WHERE tblParitySummaryPurchase.prodClassId=" + specificationId + " AND ((tblParitySummaryPurchase.isActive = 1 OR tblParitySummaryPurchase.isActive IS NULL))" +
                //                                            //" AND tblParitySummaryPurchase.stateId=" + stateId +
                //                                            " ORDER BY tblParitySummaryPurchase.createdOn desc";

                cmdSelect.CommandText = SqlSelectQuery() + " LEFT JOIN " +
                                        " ( SELECT tblParityDetailsPurchase.* FROM tblParityDetailsPurchase LEFT JOIN tblParitySummaryPurchase " +
                                        " ON tblParityDetailsPurchase.parityPurchaseId = tblParitySummaryPurchase.idParityPurchase where tblParitySummaryPurchase.stateId= " + stateId + " and tblParitySummaryPurchase.isActive = 1) as abc " +
                                        " ON abc.prodItemId = tblProductItem.idProdItem " +
                                        " where tblProductItem.idProdItem =" + prodItemId + " and tblProductItem.isActive = 1";

                cmdSelect.Connection = conn;
            cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                Boolean isParityDetails = true;
                List<TblProductItemTO> list = ConvertDTToList(reader, isParityDetails);
                if (list.Count == 0)
                {
                    if (reader != null) reader.Dispose();
                    cmdSelect.Dispose();
                    cmdSelect.CommandText = "SELECT idProdItem,prodClassId,'0' as createdBy,'0' as updatedBy,'0001-01-01' as createdOn,'0001-01-01' as updatedOn,itemName,itemDesc," +
                        "'' as remark,'0' as isActive,weightMeasureUnitId,conversionUnitOfMeasure,conversionFactor,isStockRequire,'0' as parityAmt,'0' as nonConfParityAmt," +
                        "'0' as 'recovery' , '0' as 'isBaseItemForRate'  From [tblProductItem] where idProdItem = " + prodItemId;

                    cmdSelect.Connection = conn;
                    cmdSelect.CommandType = System.Data.CommandType.Text;

                    reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                    isParityDetails = true;
                    list = ConvertDTToList(reader, isParityDetails);
                    reader.Close();
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
            }
        }


        public List<TblProductItemTO> SelectAllTblProductGraidList(Int32 specificationId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING); ;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " LEFT JOIN tblParityDetailsPurchase" +
                                                           " ON tblParityDetailsPurchase.prodItemId = tblProductItem.idProdItem " +
                                                           " LEFT JOIN tblParitySummaryPurchase" +
                                                           " ON tblParitySummaryPurchase.idParityPurchase = tblParityDetailsPurchase.parityPurchaseId " +

                                                           " WHERE tblParitySummaryPurchase.prodClassId=" + specificationId + " AND ((tblParitySummaryPurchase.isActive = 1 OR tblParitySummaryPurchase.isActive IS NULL))" +
                                                           //" AND tblParitySummaryPurchase.stateId=" + stateId +
                                                           " ORDER BY tblParitySummaryPurchase.createdOn desc";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                Boolean isParityDetails = true;
                List<TblProductItemTO> list = ConvertDTToList(reader, isParityDetails);
                if (list.Count == 0)
                {
                    if (reader != null) reader.Dispose();
                    cmdSelect.Dispose();
                    cmdSelect.CommandText = "SELECT idProdItem,prodClassId,'0' as createdBy,'0' as updatedBy,'0001-01-01' as createdOn,'0001-01-01' as updatedOn,itemName,itemDesc," +
                        "'' as remark,'0' as isActive,weightMeasureUnitId,conversionUnitOfMeasure,conversionFactor,isStockRequire,'0' as parityAmt,'0' as nonConfParityAmt," +
                        "'0' as 'recovery' , '0' as 'isBaseItemForRate'  From [tblProductItem] where prodClassId = " + specificationId;

                    cmdSelect.Connection = conn;
                    cmdSelect.CommandType = System.Data.CommandType.Text;

                    reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                    isParityDetails = true;
                    list = ConvertDTToList(reader, isParityDetails);
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }



        public List<TblProductItemTO> SelectAllTblProductGraidList(string specificationId = "0")
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING); ;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {


                conn.Open();
                if (specificationId == "0")
                    cmdSelect.CommandText = SqlQuery() + " WHERE tblProductItem.isActive=1 order by displaySequanceNo asc";
                else
                    cmdSelect.CommandText = SqlQuery() + " WHERE tblProductItem.isActive=1 AND tblProductItem.prodClassId in(" + specificationId + ") order by displaySequanceNo asc";

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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public TblProductItemTO SelectTblProductItem(Int32 idProdItem, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlQuery() + " WHERE tblProductItem.idProdItem = " + idProdItem + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductItemTO> list = ConvertDTToList(reader);
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

        public List<TblProductItemTO> ConvertDTToList(SqlDataReader tblProductItemTODT, Boolean isParityDetails = false)
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
                    {
                        tblProductItemTONew.ItemName = Convert.ToString(tblProductItemTODT["itemName"].ToString());
                        tblProductItemTONew.ItemDesc = Convert.ToString(tblProductItemTODT["itemName"].ToString());
                    }
                    //if (tblProductItemTODT["itemDesc"] != DBNull.Value)
                    //    tblProductItemTONew.ItemDesc = Convert.ToString(tblProductItemTODT["itemDesc"].ToString());
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

                    if (tblProductItemTODT["isNonCommercialItem"] != DBNull.Value)
                        tblProductItemTONew.IsNonCommercialItem = Convert.ToInt32(tblProductItemTODT["isNonCommercialItem"].ToString());

                    if (tblProductItemTODT["isBaseItemForRate"] != DBNull.Value)
                        tblProductItemTONew.IsBaseItemForRate = Convert.ToInt32(tblProductItemTODT["isBaseItemForRate"].ToString());


                    if (tblProductItemTODT["prodClassDesc"] != DBNull.Value)
                        tblProductItemTONew.ProdClassDisplayName = Convert.ToString(tblProductItemTODT["prodClassDesc"].ToString());

                    if (isParityDetails == true)
                    {
                        if (tblProductItemTODT["parityAmt"] != DBNull.Value)
                            tblProductItemTONew.ParityAmt = Convert.ToDouble(tblProductItemTODT["parityAmt"].ToString());
                        if (tblProductItemTODT["nonConfParityAmt"] != DBNull.Value)
                            tblProductItemTONew.NonConfParityAmt = Convert.ToDouble(tblProductItemTODT["nonConfParityAmt"].ToString());
                        if (tblProductItemTODT["recovery"] != DBNull.Value)
                            tblProductItemTONew.Recovery = Convert.ToDouble(tblProductItemTODT["recovery"].ToString());

                    }
                    tblProductItemTOList.Add(tblProductItemTONew);
                }
            }
            return tblProductItemTOList;
        }

        public List<TblProductItemTO> ConvertDTToListForUpdate(SqlDataReader tblProductItemTODT)
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

                    tblProductItemTOList.Add(tblProductItemTONew);
                }
            }
            return tblProductItemTOList;
        }

        //sudhir[12-jan-2018] added for getlistof items whose stockupdate is require.
        public List<TblProductItemTO> SelectProductItemListStockUpdateRequire(int isStockRequire)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING); ;
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

        #endregion

        #region Insertion

        public int InsertTblProductItem(TblProductItemTO tblProductItemTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING); ;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblProductItemTO, cmdInsert);
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

        public int InsertTblProductItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblProductItemTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblProductItemTO tblProductItemTO, SqlCommand cmdInsert)
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
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblProductItemTO.IdProdItem = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }

            return 0;
        }

        #endregion

        #region Updation

        public int UpdateTblProductItem(TblProductItemTO tblProductItemTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING); ;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblProductItemTO, cmdUpdate);
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

        public int UpdateTblProductItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblProductItemTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblProductItemTO tblProductItemTO, SqlCommand cmdUpdate)
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
            cmdUpdate.Parameters.Add("@isStockRequire", System.Data.SqlDbType.Int).Value = tblProductItemTO.IsStockRequire;
            return cmdUpdate.ExecuteNonQuery();
        }

        #endregion

        #region Deletion

        public int DeleteTblProductItem(Int32 idProdItem)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING); ;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idProdItem, cmdDelete);
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

        public int DeleteTblProductItem(Int32 idProdItem, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idProdItem, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idProdItem, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblProductItem] " +
            " WHERE idProdItem = " + idProdItem + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idProdItem", System.Data.SqlDbType.Int).Value = tblProductItemTO.IdProdItem;
            return cmdDelete.ExecuteNonQuery();
        }

        #endregion

    }
}
