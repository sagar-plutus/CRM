using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using System.Linq;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.BL
{
    public class TblStockDetailsBL
    {
        #region Selection

        public static int SelectStockDetailsID(int CNFId1, int Bundles, int ProdCatId1, int SizeId, int ProdSpecId)
        {
            int id = 0;
            id = TblStockDetailsDAO.SelectIdStockDetail(CNFId1, Bundles, ProdCatId1, SizeId, ProdSpecId);
            return id;
        }

        public static List<TblStockDetailsTO> SelectAllTblStockDetailsList()
        {
            return TblStockDetailsDAO.SelectAllTblStockDetails();
        }

        public static List<TblStockDetailsTO> SelectStockDetailsListByProdCatgAndSpec(Int32 prodCatId, Int32 prodSpecId, DateTime stockDate)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblStockDetailsDAO.SelectAllTblStockDetails(prodCatId, prodSpecId, stockDate, conn, tran);
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

        public static List<TblStockDetailsTO> SelectStockDetailsListByProdCatgSpecAndMaterial(Int32 prodCatId, Int32 prodSpecId, Int32 materialId, DateTime stockDate)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblStockDetailsDAO.SelectAllTblStockDetails(prodCatId, prodSpecId, materialId, stockDate, conn, tran);
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

        public static List<TblStockDetailsTO> SelectStockDetailsListByProdCatgSpecAndMaterial(Int32 prodCatId, Int32 prodSpecId, Int32 materialId, DateTime stockDate, SqlConnection conn, SqlTransaction tran)
        {
            return TblStockDetailsDAO.SelectAllTblStockDetails(prodCatId, prodSpecId, materialId, stockDate, conn, tran);
        }

        public static List<TblStockDetailsTO> SelectAllTblStockDetailsList(Int32 stockSummaryId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return SelectAllTblStockDetailsList(stockSummaryId, conn, tran);
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

        public static List<TblStockDetailsTO> SelectAllTblStockDetailsList(Int32 stockSummaryId, SqlConnection conn, SqlTransaction tran)
        {
            return TblStockDetailsDAO.SelectAllTblStockDetails(stockSummaryId, conn, tran);
        }

        public static TblStockDetailsTO SelectTblStockDetailsTO(Int32 idStockDtl)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblStockDetailsDAO.SelectTblStockDetails(idStockDtl, conn, tran);
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

        public static TblStockDetailsTO SelectTblStockDetailsTO(Int32 idStockDtl, SqlConnection conn, SqlTransaction tran)
        {
            return TblStockDetailsDAO.SelectTblStockDetails(idStockDtl, conn, tran);
        }

        public static TblStockDetailsTO SelectTblStockDetailsTO(TblRunningSizesTO runningSizeTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblStockDetailsDAO.SelectTblStockDetails(runningSizeTO, conn, tran);
        }

        public static List<TblStockDetailsTO> SelectAllEmptyStockTemplateList(int prodCatId, int locationId)
        {
            return TblStockDetailsDAO.SelectEmptyStockDetailsTemplate(prodCatId, locationId);
        }

        public static List<TblStockDetailsTO> SelectAllTblStockDetailsList(int locationId, int prodCatId, DateTime stockDate)
        {
            List<TblStockDetailsTO> emptyStkTemplateList = TblStockDetailsDAO.SelectEmptyStockDetailsTemplate(prodCatId, locationId);
            List<TblStockDetailsTO> existingList = TblStockDetailsDAO.SelectAllTblStockDetails(locationId, prodCatId, stockDate);
            if (emptyStkTemplateList != null && emptyStkTemplateList.Count > 0)
            {
                if (existingList != null && existingList.Count > 0)
                {
                    for (int i = 0; i < emptyStkTemplateList.Count; i++)
                    {
                        TblStockDetailsTO existingStockDetailsTO = existingList.Where(a => a.ProdCatId == emptyStkTemplateList[i].ProdCatId && a.ProdSpecId == emptyStkTemplateList[i].ProdSpecId && a.MaterialId == emptyStkTemplateList[i].MaterialId && a.LocationId == locationId).FirstOrDefault();
                        if (existingStockDetailsTO != null)
                        {
                            emptyStkTemplateList[i].LocationId = locationId;
                            emptyStkTemplateList[i].StockSummaryId = existingStockDetailsTO.StockSummaryId;
                            emptyStkTemplateList[i].IdStockDtl = existingStockDetailsTO.IdStockDtl;
                            emptyStkTemplateList[i].NoOfBundles = existingStockDetailsTO.NoOfBundles;
                            emptyStkTemplateList[i].TotalStock = existingStockDetailsTO.TotalStock;
                            emptyStkTemplateList[i].LoadedStock = existingStockDetailsTO.LoadedStock;
                            emptyStkTemplateList[i].BalanceStock = existingStockDetailsTO.BalanceStock;
                            emptyStkTemplateList[i].CreatedBy = existingStockDetailsTO.CreatedBy;
                            emptyStkTemplateList[i].CreatedOn = existingStockDetailsTO.CreatedOn;
                            emptyStkTemplateList[i].UpdatedBy = existingStockDetailsTO.UpdatedBy;
                            emptyStkTemplateList[i].UpdatedOn = existingStockDetailsTO.UpdatedOn;
                            emptyStkTemplateList[i].ProductId = existingStockDetailsTO.ProductId;

                        }
                    }
                }

                #region For Other Item
                //sudhir[04-APR-2018] Added for otheritemStock if found then add else create empty.
                List<TblProductItemTO> productItemTOList = TblProductItemBL.SelectProductItemListStockUpdateRequire(1);
                if (productItemTOList != null && productItemTOList.Count > 0)
                {
                    for (int i = 0; i < productItemTOList.Count; i++)
                    {
                        TblStockDetailsTO StockDetailsTO = existingList.Where(x => x.ProdItemId == productItemTOList[i].IdProdItem && x.LocationId == locationId).FirstOrDefault();
                        if (StockDetailsTO != null)
                        {
                            StockDetailsTO.MaterialDesc = productItemTOList[i].ProdClassDisplayName + "/" + productItemTOList[i].ItemDesc;
                            StockDetailsTO.OtherItem = 1;
                            emptyStkTemplateList.Add(StockDetailsTO);

                        }
                        else //Add Empty Stock 
                        {
                            TblStockDetailsTO emptyItemStockTO = new TblStockDetailsTO();
                            emptyItemStockTO.ProdItemId = productItemTOList[i].IdProdItem;
                            emptyItemStockTO.MaterialDesc = productItemTOList[i].ProdClassDisplayName + "/" + productItemTOList[i].ItemDesc;
                            emptyItemStockTO.LocationId = locationId;
                            emptyItemStockTO.OtherItem = 1;
                            //emptyItemStockTO.ProdCatId = prodCatId;
                            emptyStkTemplateList.Add(emptyItemStockTO);
                        }
                    }
                }
                #endregion
                return emptyStkTemplateList;
            }
            return null;
        }

        public static List<SizeSpecWiseStockTO> SelectSizeAndSpecWiseStockSummary(DateTime stockDate)
        {
            return DAL.TblStockDetailsDAO.SelectSizeAndSpecWiseStockSummary(stockDate);
        }


        #endregion

        #region Insertion
        public static int InsertTblStockDetails(TblStockDetailsTO tblStockDetailsTO)
        {
            return TblStockDetailsDAO.InsertTblStockDetails(tblStockDetailsTO);
        }

        public static int InsertTblStockDetails(TblStockDetailsTO tblStockDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblStockDetailsDAO.InsertTblStockDetails(tblStockDetailsTO, conn, tran);
        }

        #endregion

        #region Updation
        public static int UpdateTblStockDetails(TblStockDetailsTO tblStockDetailsTO)
        {
            int result = TblStockDetailsDAO.UpdateTblStockDetails(tblStockDetailsTO);

            return result;
        }

        public static int UpdateTblStockDetails(TblStockDetailsTO tblStockDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblStockDetailsDAO.UpdateTblStockDetails(tblStockDetailsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblStockDetails(Int32 idStockDtl)
        {
            return TblStockDetailsDAO.DeleteTblStockDetails(idStockDtl);
        }

        public static int DeleteTblStockDetails(Int32 idStockDtl, SqlConnection conn, SqlTransaction tran)
        {
            return TblStockDetailsDAO.DeleteTblStockDetails(idStockDtl, conn, tran);
        }

        #endregion


        #region  Stock Up by Deepali
        internal static int SaveStockUp(TblStockDetailsTO stockDetailsTOList, string loginUserId)
        {
            int Result = 1;

            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;

            conn.Open();
            tran = conn.BeginTransaction();


            // For weight and Stock in MT calculations
            List<TblProductInfoTO> productList = BL.TblProductInfoBL.SelectAllTblProductInfoList(conn, tran);


            DateTime confirmedDate = Constants.ServerDateTime;
            DateTime stockDate = confirmedDate.Date;

            TblStockDetailsTO obj = new TblStockDetailsTO();
            TblStockDetailsTO objStockTO = new TblStockDetailsTO();

            obj = stockDetailsTOList;

            List<TblStockDetailsTO> tblStockDetailsTO = BL.TblStockDetailsBL.SelectStockDetailsListByProdCatgSpecAndMaterial(obj.ProdCatId1, obj.ProdSpecId, obj.MaterialId, stockDate);

            if (productList != null)
            {
                var productInfo = productList.Where(p => p.MaterialId == obj.MaterialId
                                                    && p.ProdCatId == obj.ProdCatId1
                                                    && p.ProdSpecId == obj.ProdSpecId).OrderByDescending(d => d.CreatedOn).FirstOrDefault();

                if (productInfo != null && tblStockDetailsTO.Count > 0)
                {
                    if (obj.Bundles != 0)
                    {
                        Double totalStkInMT = obj.Bundles * productInfo.NoOfPcs * productInfo.AvgSecWt * productInfo.StdLength;
                        Double TotalStock = totalStkInMT / 1000;
                        tblStockDetailsTO[0].TotalStock =TotalStock;
                    }
                    else if (obj.StockInMT != 0)
                    {
                        Double Bundle = obj.StockInMT * 1000;
                        Double BundleTotal = Bundle / productInfo.NoOfPcs / productInfo.AvgSecWt / productInfo.StdLength;
                        obj.Bundles = BundleTotal;
                         tblStockDetailsTO[0].TotalStock = obj.StockInMT ;

                    }
                      
                    tblStockDetailsTO[0].BalanceStock = tblStockDetailsTO[0].TotalStock;
                    tblStockDetailsTO[0].TodaysStock = tblStockDetailsTO[0].TotalStock;
                    tblStockDetailsTO[0].ProductId = productInfo.IdProduct;
                    tblStockDetailsTO[0].NoOfBundles = tblStockDetailsTO[0].NoOfBundles + obj.Bundles;

                    tblStockDetailsTO[0].UpdatedBy = Convert.ToInt32(loginUserId);
                    tblStockDetailsTO[0].UpdatedOn = Constants.ServerDateTime;

                    tblStockDetailsTO[0].CNFId1 = obj.CNFId1;

                    objStockTO = tblStockDetailsTO[0];
                }

                else
                {
                    tran.Rollback();
                }

                if (tblStockDetailsTO.Count > 0)
                {
                    // tblStockDetailsTO[0].UpdatedBy = Convert.ToInt32(loginUserId);
                    // tblStockDetailsTO[0].UpdatedOn = Constants.ServerDateTime;
                    // tblStockDetailsTO[0].NoOfBundles = tblStockDetailsTO[0].NoOfBundles + obj.Bundles;
                    // tblStockDetailsTO[0].CNFId1 = obj.CNFId1;
                    // tblStockDetailsTO[0].BalanceStock = tblStockDetailsTO[0].TotalStock + obj.StockInMT;
                    // tblStockDetailsTO[0].TotalStock = tblStockDetailsTO[0].TotalStock + obj.StockInMT;
                    // tblStockDetailsTO[0].TodaysStock = tblStockDetailsTO[0].TodaysStock + obj.StockInMT;
                    // objStockTO = tblStockDetailsTO[0];

                    Result = BL.TblStockDetailsBL.UpdateTblStockDetails(objStockTO, conn, tran);

                    if (Result != -1)
                    {
                        TblLoadingQuotaDeclarationTO LoadingQuotaDeclarationTO = new TblLoadingQuotaDeclarationTO();
                        LoadingQuotaDeclarationTO = BL.TblLoadingQuotaDeclarationBL.SelectTblLoadingQuotaDeclarationTO(objStockTO.CNFId1, objStockTO.ProdCatId, objStockTO.ProdSpecId, objStockTO.MaterialId, objStockTO.ProdItemId, objStockTO.UpdatedOn, conn, tran);
                        if (LoadingQuotaDeclarationTO != null)
                        {
                            Double totalStkInMT;
                             Double TotalStock = 0;
                            if (obj.Bundles != 0)
                            {
                                 totalStkInMT = obj.Bundles * productInfo.NoOfPcs * productInfo.AvgSecWt * productInfo.StdLength;
                                 TotalStock = totalStkInMT / 1000;
                            }      
                            else
                            {
                                TotalStock = obj.StockInMT;
                            }                     

                            // LoadingQuotaDeclarationTO.BalanceQuota = LoadingQuotaDeclarationTO.BalanceQuota + obj.StockInMT;
                            // LoadingQuotaDeclarationTO.AllocQuota = LoadingQuotaDeclarationTO.AllocQuota + obj.StockInMT;

                            LoadingQuotaDeclarationTO.BalanceQuota = LoadingQuotaDeclarationTO.BalanceQuota + TotalStock;
                            LoadingQuotaDeclarationTO.AllocQuota = LoadingQuotaDeclarationTO.AllocQuota + TotalStock;

                            LoadingQuotaDeclarationTO.UpdatedOn = stockDate;
                            LoadingQuotaDeclarationTO.UpdatedBy = Convert.ToInt32(loginUserId);
                            Result = BL.TblLoadingQuotaDeclarationBL.UpdateTblLoadingQuotaDeclaration(LoadingQuotaDeclarationTO, conn, tran);

                        }
                        else
                        {
                            tran.Rollback();
                        }
                        tran.Commit();
                        conn.Close();
                    }
                    else
                    {
                        tran.Rollback();
                    }

                }
            }
            return Result;

        }


        #endregion
    }
}
