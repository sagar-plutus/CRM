using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using System.Linq;

namespace SalesTrackerAPI.BL
{
    public class TblStockSummaryBL
    {
        #region Selection
       
        public static List<TblStockSummaryTO> SelectAllTblStockSummaryList()
        {
            return TblStockSummaryDAO.SelectAllTblStockSummary();
        }

        public static String SelectLastStockUpdatedDateTime(Int32 compartmentId, Int32 prodCatId)
        {
            return TblStockSummaryDAO.SelectLastStockUpdatedDateTime(compartmentId, prodCatId);
        }

        public static TblStockSummaryTO SelectTblStockSummaryTO(Int32 idStockSummary)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblStockSummaryDAO.SelectTblStockSummary(idStockSummary,conn,tran);

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

        public static TblStockSummaryTO SelectTblStockSummaryTO(Int32 idStockSummary,SqlConnection conn,SqlTransaction tran)
        {
            return TblStockSummaryDAO.SelectTblStockSummary(idStockSummary, conn, tran);

        }

        public static TblStockSummaryTO SelectTblStockSummaryTO(DateTime stocDate)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblStockSummaryDAO.SelectTblStockSummary(stocDate, conn, tran);
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

        public static SalesTrackerAPI.DashboardModels.StockUpdateInfo SelectDashboardStockUpdateInfo(DateTime sysDate)
        {
            return TblStockSummaryDAO.SelectDashboardStockUpdateInfo(sysDate);

        }
        #endregion

        #region Insertion
        public static int InsertTblStockSummary(TblStockSummaryTO tblStockSummaryTO)
        {
            return TblStockSummaryDAO.InsertTblStockSummary(tblStockSummaryTO);
        }

        public static int InsertTblStockSummary(TblStockSummaryTO tblStockSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblStockSummaryDAO.InsertTblStockSummary(tblStockSummaryTO, conn, tran);
        }

        /// <summary>
        /// Sanjay [2017-03-18] To Update Daily Stock Entries. If it is for the first time for that day
        /// then records will be inserted otherwise records will be updated.
        /// </summary>
        /// <param name="tblStockSummaryTO"></param>
        /// <returns></returns>
        public static ResultMessage UpdateDailyStock(TblStockSummaryTO tblStockSummaryTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            Int32 updateOrCreatedUser = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                #region Check For Existed Or Not

                //Check if Todays Stock Summary Record is inserted or not
                // If Inserted then its update request else insert request
                DateTime stockDate = Constants.ServerDateTime.Date;
                TblStockSummaryTO todaysStockSummaryTO = DAL.TblStockSummaryDAO.SelectTblStockSummary(stockDate, conn, tran);
                if (todaysStockSummaryTO == null)
                {
                    tblStockSummaryTO.StockDate = stockDate;
                    result = InsertTblStockSummary(tblStockSummaryTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.Result = 0;
                        resultMessage.Text = "Error While InsertTblStockSummary : MEthod UpdateDailyStock";
                        resultMessage.DisplayMessage = "Error.. Records could not be saved";
                        resultMessage.MessageType = ResultMessageE.Error;
                        return resultMessage;
                    }
                }
                else
                {
                    if (todaysStockSummaryTO.ConfirmedOn != DateTime.MinValue)
                    {
                        tran.Rollback();
                        resultMessage.Result = 0;
                        resultMessage.Text = "Stock For the Date :" + todaysStockSummaryTO.StockDate.Date.ToString(Constants.DefaultDateFormat) + " is already confirmed. You can not update the stock now";
                        resultMessage.DisplayMessage = resultMessage.Text;
                        resultMessage.MessageType = ResultMessageE.Error;
                        return resultMessage;
                    }

                    todaysStockSummaryTO.StockDetailsTOList = tblStockSummaryTO.StockDetailsTOList;
                    tblStockSummaryTO = todaysStockSummaryTO;
                }

                #endregion

                #region Based On Step 1 Calculate and Update Otherwise Insert New Records
                // To Compare Against Existing and Update
                List<TblStockDetailsTO> existingStockList = BL.TblStockDetailsBL.SelectAllTblStockDetailsList(tblStockSummaryTO.IdStockSummary, conn, tran);

                // For weight and Stock in MT calculations
                List<TblProductInfoTO> productList = BL.TblProductInfoBL.SelectAllTblProductInfoList(conn, tran);
                // Insert All New Records
                if (tblStockSummaryTO.StockDetailsTOList != null && tblStockSummaryTO.StockDetailsTOList.Count > 0)
                {
                    updateOrCreatedUser = tblStockSummaryTO.StockDetailsTOList[0].CreatedBy;

                    for (int i = 0; i < tblStockSummaryTO.StockDetailsTOList.Count; i++)
                    {
                        tblStockSummaryTO.StockDetailsTOList[i].StockSummaryId = tblStockSummaryTO.IdStockSummary;

                        Boolean isExist = false;
                        if (existingStockList != null && existingStockList.Count > 0)
                        {
                            var existingStocDtlTO = existingStockList.Where(e => e.LocationId == tblStockSummaryTO.StockDetailsTOList[i].LocationId
                                                                            && e.MaterialId == tblStockSummaryTO.StockDetailsTOList[i].MaterialId
                                                                            && e.ProdCatId == tblStockSummaryTO.StockDetailsTOList[i].ProdCatId
                                                                            && e.ProdSpecId == tblStockSummaryTO.StockDetailsTOList[i].ProdSpecId
                                                                            && e.ProdItemId== tblStockSummaryTO.StockDetailsTOList[i].ProdItemId).FirstOrDefault();
                            if (existingStocDtlTO != null)
                            {
                                isExist = true;
                                tblStockSummaryTO.StockDetailsTOList[i].IdStockDtl = existingStocDtlTO.IdStockDtl;
                            }
                        }

                        if (productList != null)
                        {
                            var productInfo = productList.Where(p => p.MaterialId == tblStockSummaryTO.StockDetailsTOList[i].MaterialId
                                                                && p.ProdCatId == tblStockSummaryTO.StockDetailsTOList[i].ProdCatId
                                                                && p.ProdSpecId == tblStockSummaryTO.StockDetailsTOList[i].ProdSpecId).OrderByDescending(d => d.CreatedOn).FirstOrDefault();

                            //Sudhir[05-APR-2018] Added Condition For Other Item.
                            if(tblStockSummaryTO.StockDetailsTOList[i].OtherItem != 1 && productInfo != null)
                            {
                                if (productInfo != null)
                                {
                                    Double totalStkInMT = tblStockSummaryTO.StockDetailsTOList[i].NoOfBundles * productInfo.NoOfPcs * productInfo.AvgSecWt * productInfo.StdLength;
                                    tblStockSummaryTO.StockDetailsTOList[i].TotalStock = totalStkInMT / 1000;
                                    tblStockSummaryTO.StockDetailsTOList[i].BalanceStock = tblStockSummaryTO.StockDetailsTOList[i].TotalStock;
                                    tblStockSummaryTO.StockDetailsTOList[i].TodaysStock = tblStockSummaryTO.StockDetailsTOList[i].TotalStock;
                                    tblStockSummaryTO.StockDetailsTOList[i].ProductId = productInfo.IdProduct;
                                }

                                else
                                {
                                    tran.Rollback();
                                    resultMessage.Result = 0;
                                    resultMessage.Text = "Error Product Configuration Not Found : Method UpdateDailyStock";
                                    resultMessage.DisplayMessage = "Error. Record Could not be saved. Product Configuration Not Found For " + tblStockSummaryTO.StockDetailsTOList[i].MaterialDesc + " " + tblStockSummaryTO.StockDetailsTOList[i].ProdCatDesc + "-" + tblStockSummaryTO.StockDetailsTOList[i].ProdSpecDesc;
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    return resultMessage;
                                }
                            }
                            else
                            {
                                tblStockSummaryTO.StockDetailsTOList[i].TotalStock = tblStockSummaryTO.StockDetailsTOList[i].TotalStock;
                                tblStockSummaryTO.StockDetailsTOList[i].BalanceStock = tblStockSummaryTO.StockDetailsTOList[i].TotalStock;
                                tblStockSummaryTO.StockDetailsTOList[i].TodaysStock = tblStockSummaryTO.StockDetailsTOList[i].TotalStock;
                                //tblStockSummaryTO.StockDetailsTOList[i].ProductId = productInfo.IdProduct;
                            }
                            
                        }

                        if (isExist)
                        {
                            //Update Existing Records 
                            tblStockSummaryTO.StockDetailsTOList[i].UpdatedOn = Constants.ServerDateTime;
                            result = BL.TblStockDetailsBL.UpdateTblStockDetails(tblStockSummaryTO.StockDetailsTOList[i], conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.Result = 0;
                                resultMessage.Text = "Error While UpdateTblStockDetails : Method UpdateDailyStock";
                                resultMessage.DisplayMessage = "Error.. Records could not be saved";
                                resultMessage.MessageType = ResultMessageE.Error;
                                return resultMessage;
                            }
                        }
                        else
                        {
                            // Insert New Stock Entry
                            tblStockSummaryTO.StockDetailsTOList[i].UpdatedOn = DateTime.MinValue;
                            tblStockSummaryTO.StockDetailsTOList[i].UpdatedBy = 0;
                            result = BL.TblStockDetailsBL.InsertTblStockDetails(tblStockSummaryTO.StockDetailsTOList[i], conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.Text = "Error While InsertTblStockDetails : Method UpdateDailyStock";
                                resultMessage.DisplayMessage = "Error.. Records could not be saved";
                                resultMessage.Result = 0;
                                resultMessage.MessageType = ResultMessageE.Error;
                                return resultMessage;
                            }
                        }
                    }
                }
                else
                {
                    tran.Rollback();
                    resultMessage.Text = "Error,StockDetailsTOList Found NULL : Method UpdateDailyStock";
                    resultMessage.DisplayMessage = "Error.. Records could not be saved";
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                #endregion

                #region Update Total No Of Bundels and Stock Qty in MT

                // Consolidate All No Of Bundles and Total Stock Qty
                List<TblStockDetailsTO> totalStockList = BL.TblStockDetailsBL.SelectAllTblStockDetailsList(tblStockSummaryTO.IdStockSummary, conn, tran);
                Double totalNoOfBundles = 0;
                Double totalStockMT = 0;

                if (totalStockList != null && totalStockList.Count > 0)
                {
                    totalNoOfBundles = totalStockList.Sum(t => t.NoOfBundles);
                    totalStockMT = totalStockList.Sum(t => t.TotalStock);

                    tblStockSummaryTO.NoOfBundles = totalNoOfBundles;
                    tblStockSummaryTO.TotalStock = totalStockMT;
                    tblStockSummaryTO.UpdatedBy = updateOrCreatedUser;
                    tblStockSummaryTO.UpdatedOn = Constants.ServerDateTime;

                    result = UpdateTblStockSummary(tblStockSummaryTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Error,While UpdateTblStockSummary : Method UpdateDailyStock";
                        resultMessage.DisplayMessage = "Error.. Records could not be saved";
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        return resultMessage;
                    }
                }
                else
                {
                    tran.Rollback();
                    resultMessage.Text = "Error,StockDetailsTOList Found NULL For Final Summation : Method UpdateDailyStock";
                    resultMessage.DisplayMessage = "Error.. Records could not be saved";
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                #endregion

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Saved Sucessfully";
                resultMessage.Result = 1;
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.Text = "Exception Error While Record Save : UpdateDailyStock";
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.DisplayMessage = "Error.. Records could not be saved";
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Sanjay [2017-06-13]
        /// </summary>
        /// <param name="sizeSpecWiseStockTOList"></param>
        /// <returns></returns>
        internal static ResultMessage ConfirmStockSummary(List<SizeSpecWiseStockTO> sizeSpecWiseStockTOList)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            Int32 updateOrCreatedUser = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                if (sizeSpecWiseStockTOList == null || sizeSpecWiseStockTOList.Count == 0)
                {
                    tran.Rollback();
                    resultMessage.Text = "Error,sizeSpecWiseStockTOList Found NULL : Method ConfirmStockSummary";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                #region 0.1 Check For Yesterddays Pending Loading Slip and Reduce Stock Qty
                List<TblStockDetailsTO> stockList = BL.TblStockDetailsBL.SelectAllTblStockDetailsList(sizeSpecWiseStockTOList[0].StockSummaryId, conn, tran);
                Boolean canStockDeclare = true;
                String notDeclaReasons = string.Empty;
                Boolean isStockRequie = false;
                updateOrCreatedUser = sizeSpecWiseStockTOList[0].ConfirmedBy;

                //Sudhir[05-APR-2018] Added for ProductItem List Who Require Stock.
                List<TblProductItemTO> stockRequireProductItemList = TblProductItemBL.SelectProductItemListStockUpdateRequire(1);


                List<TblLoadingSlipExtTO> postponeList = BL.TblLoadingSlipExtBL.SelectCnfWiseLoadingMaterialToPostPoneList(conn, tran);
                if (postponeList != null)
                {
                    var groupItemsList = postponeList.GroupBy(x => new { x.ProdCatId, x.ProdCatDesc, x.ProdSpecId, x.ProdSpecDesc, x.MaterialId, x.MaterialDesc,x.ProdItemId,x.DisplayName}).ToList();

                    //Filter List



                    for (int g = 0; g < groupItemsList.Count; g++)
                    {
                            //var tempList = postponeList.Where(p => p.MaterialId == groupItemsList[g].Key.MaterialId && p.ProdCatId == groupItemsList[g].Key.ProdCatId && p.ProdCatId == groupItemsList[g].Key.ProdSpecId).ToList();
                            //Sudhir[05-APR-2018] Sudhir Added for get StockRequire Flag for Other Item and then proceed.
                            if(groupItemsList[g].Key.ProdItemId > 0)
                            {
                                isStockRequie = stockRequireProductItemList.Where(ele => ele.IdProdItem == groupItemsList[g].Key.ProdItemId).
                                                                            Select(x => x.IsStockRequire == 1).FirstOrDefault();
                            //Sudhir[05-APR-2018] Added for Checking IsStock Require for Each ProductItemId.
                                     if (!isStockRequie)
                                             continue;
                            }


                            var tempList = postponeList.Where(p => p.MaterialId == groupItemsList[g].Key.MaterialId && p.ProdCatId == groupItemsList[g].Key.ProdCatId && p.ProdSpecId == groupItemsList[g].Key.ProdSpecId
                                                        && p.ProdItemId== groupItemsList[g].Key.ProdItemId).ToList(); //Sudhir[05-APR-2018] Added Product Item Id .

                            Double loadingQty = tempList.Sum(x => x.LoadingQty);

                            var curStockList = stockList.Where(p => p.MaterialId == groupItemsList[g].Key.MaterialId && p.ProdCatId == groupItemsList[g].Key.ProdCatId && p.ProdSpecId == groupItemsList[g].Key.ProdSpecId
                                                             && p.ProdItemId == groupItemsList[g].Key.ProdItemId).ToList(); //Sudhir[05-APR-2018] Added Product Item Id .

                        Double itemStockQty = 0;
                            if (curStockList != null)
                                itemStockQty = curStockList.Sum(a => a.TotalStock);

                            if (itemStockQty < loadingQty)
                            {
                                canStockDeclare = false;
                                if(groupItemsList[g].Key.ProdItemId > 0)
                                     notDeclaReasons += groupItemsList[g].Key.DisplayName+ " Qty :" + loadingQty + " ,";
                                 else
                                    notDeclaReasons += groupItemsList[g].Key.MaterialDesc + " " + groupItemsList[g].Key.ProdCatDesc + "-" + groupItemsList[g].Key.ProdSpecDesc + " Qty :" + loadingQty + " ,";
                            }
                            else
                            {
                                Double qtyToRemove = loadingQty;
                                for (int si = 0; si < curStockList.Count; si++)
                                {
                                    if (qtyToRemove > 0)
                                    {
                                        TblStockDetailsTO stockDtlTO = curStockList[si];
                                        if (stockDtlTO.TotalStock >= qtyToRemove)
                                        {
                                            stockDtlTO.RemovedStock = qtyToRemove;
                                            qtyToRemove = 0;
                                        }
                                        else
                                        {
                                            stockDtlTO.RemovedStock = stockDtlTO.TotalStock;
                                            qtyToRemove = qtyToRemove - stockDtlTO.TotalStock;
                                        }

                                        stockDtlTO.UpdatedBy = updateOrCreatedUser;
                                        stockDtlTO.UpdatedOn = sizeSpecWiseStockTOList[0].ConfirmedOn;
                                        result = BL.TblStockDetailsBL.UpdateTblStockDetails(stockDtlTO, conn, tran);
                                        if (result != 1)
                                        {
                                            tran.Rollback();
                                            resultMessage.Text = "Error,While UpdateTblStockDetails : Method ConfirmStockSummary";
                                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                            resultMessage.MessageType = ResultMessageE.Error;
                                            resultMessage.Result = 0;
                                            return resultMessage;
                                        }
                                    }
                                    else break;
                                }
                            }
                    }
                }

                if (!canStockDeclare)
                {
                    tran.Rollback();
                    resultMessage.Text = "Error,While UpdateTblStockDetails : Method ConfirmStockSummary";
                    notDeclaReasons = "Final stock is less than 0 for below items. Please cofirm stock from yesterdays Vehicle In ," + Environment.NewLine + notDeclaReasons;
                    resultMessage.DisplayMessage = notDeclaReasons;
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                #endregion

                #region 1. Mark Balance Stock = Total Stock i.e Make this stock available to C&F View

                Double totalNoOfBundles = 0;
                Double totalStock = 0;
                for (int i = 0; i < stockList.Count; i++)
                {
                    //Sanjay [2017-06-13] As yesterdays loading slips stock reduces from current stock
                    //stockList[i].BalanceStock = stockList[i].TotalStock;
                    stockList[i].BalanceStock = stockList[i].TotalStock - stockList[i].RemovedStock;
                    stockList[i].UpdatedBy = updateOrCreatedUser;
                    stockList[i].UpdatedOn = sizeSpecWiseStockTOList[0].ConfirmedOn;
                    result = BL.TblStockDetailsBL.UpdateTblStockDetails(stockList[i], conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Error,While UpdateTblStockDetails : Method ConfirmStockSummary";
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.Result = 0;
                        return resultMessage;
                    }

                    totalNoOfBundles += stockList[i].NoOfBundles;
                    totalStock += stockList[i].TotalStock;

                }
                #endregion

                #region 2. Mark Stock Summary As Confirmed. After Confirmation Do Not Allow Stock Edit

                TblStockSummaryTO stockSummaryTO = SelectTblStockSummaryTO(sizeSpecWiseStockTOList[0].StockSummaryId, conn, tran);
                if (stockSummaryTO != null)
                {
                    stockSummaryTO.TotalStock = totalStock;
                    stockSummaryTO.NoOfBundles = totalNoOfBundles;
                    stockSummaryTO.ConfirmedBy = updateOrCreatedUser;
                    stockSummaryTO.ConfirmedOn = sizeSpecWiseStockTOList[0].ConfirmedOn;
                    stockSummaryTO.UpdatedBy = updateOrCreatedUser;
                    stockSummaryTO.UpdatedOn = sizeSpecWiseStockTOList[0].ConfirmedOn;

                    result = UpdateTblStockSummary(stockSummaryTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Error,While UpdateTblStockSummary : Method ConfirmStockSummary";
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        return resultMessage;
                    }
                }
                else
                {
                    tran.Rollback();
                    resultMessage.Text = "Error,stockSummaryTO Found NULL : Method ConfirmStockSummary";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                #endregion

                #region 2.1 If AutoDeclare Loading Quota Setting = 1 Then Declare Loading Quota to All C&F

                TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_AUTO_DECLARE_LOADING_QUOTA_ON_STOCK_CONFIRMATION, conn, tran);

                if (tblConfigParamsTO != null && Convert.ToInt32(tblConfigParamsTO.ConfigParamVal) == 1)
                {
                    //Declare Loading Quota 
                    DateTime stockDate = sizeSpecWiseStockTOList[0].ConfirmedOn;

                    List<TblLoadingQuotaDeclarationTO> list = BL.TblLoadingQuotaDeclarationBL.SelectLatestCalculatedLoadingQuotaDeclarationList(stockDate, 0, conn, tran);
                    if (list == null)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Error,C&F Quota list found empty ";
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.Result = 0;
                        return resultMessage;
                    }

                    #region 1. Mark All Previous Loading Quota As Inactive

                    result = DAL.TblLoadingQuotaDeclarationDAO.DeactivateAllPrevLoadingQuota(sizeSpecWiseStockTOList[0].ConfirmedBy, conn, tran);
                    if (result < 0)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Error While DeactivateAllPrevLoadingQuota : SaveLoadingQuotaDeclaration";
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.Result = 0;
                        return resultMessage;
                    }

                    #endregion

                    #region 2. Assign New Quota 

                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].CreatedOn = sizeSpecWiseStockTOList[0].ConfirmedOn;
                        list[i].CreatedBy = sizeSpecWiseStockTOList[0].ConfirmedBy;

                        result = BL.TblLoadingQuotaDeclarationBL.InsertTblLoadingQuotaDeclaration(list[i], conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.Text = "Error While InsertTblLoadingQuotaDeclaration : ConfirmStockSummary";
                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Result = 0;
                            return resultMessage;
                        }
                    }

                    #endregion
                }

                #endregion

                #region 3.1 Notification to Directors and account person on stock confirmation

                TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.TODAYS_STOCK_CONFIRMED;
                tblAlertInstanceTO.AlertAction = "TODAYS_STOCK_CONFIRMED";
                tblAlertInstanceTO.AlertComment = "Today's Stock is Confirmed";// . Total Stock(In MT) is :" + stockSummaryTO.TotalStock;
                tblAlertInstanceTO.EffectiveFromDate = stockSummaryTO.ConfirmedOn;
                tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                tblAlertInstanceTO.IsActive = 1;
                tblAlertInstanceTO.SourceDisplayId = "TODAYS_STOCK_CONFIRMED";
                tblAlertInstanceTO.SourceEntityId = stockSummaryTO.IdStockSummary;
                tblAlertInstanceTO.RaisedBy = stockSummaryTO.ConfirmedBy;
                tblAlertInstanceTO.RaisedOn = stockSummaryTO.ConfirmedOn;
                tblAlertInstanceTO.IsAutoReset = 1;

                ResultMessage rMessage = BL.TblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                if (rMessage.MessageType != ResultMessageE.Information)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    resultMessage.Text = "Error While SaveNewAlertInstance";
                    resultMessage.Tag = tblAlertInstanceTO;
                    return resultMessage;
                }

                #endregion

                #region 3.2 Notifications of Loading Quota Declaration To All C&F

                tblAlertInstanceTO = new TblAlertInstanceTO();
                tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.LOADING_QUOTA_DECLARED;
                tblAlertInstanceTO.AlertAction = "LOADING_QUOTA_DECLARED";
                tblAlertInstanceTO.AlertComment = "Today's Loading Quota is Declared";

                tblAlertInstanceTO.EffectiveFromDate = stockSummaryTO.ConfirmedOn;
                tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                tblAlertInstanceTO.IsActive = 1;
                tblAlertInstanceTO.SourceDisplayId = "LOADING_QUOTA_DECLARED";
                tblAlertInstanceTO.SourceEntityId = stockSummaryTO.IdStockSummary;
                tblAlertInstanceTO.RaisedBy = stockSummaryTO.ConfirmedBy;
                tblAlertInstanceTO.RaisedOn = stockSummaryTO.ConfirmedOn;
                tblAlertInstanceTO.IsAutoReset = 1;

                String alertDefIds = (int)NotificationConstants.NotificationsE.LOADING_QUOTA_DECLARED + "," + (int)NotificationConstants.NotificationsE.LOADING_STOPPED;
                result = BL.TblAlertInstanceBL.ResetAlertInstanceByDef(alertDefIds, conn, tran);
                if (result < 0)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While ResetAlertInstanceByDef";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    resultMessage.Tag = tblAlertInstanceTO;
                    return resultMessage;
                }

                rMessage = BL.TblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                if (rMessage.MessageType != ResultMessageE.Information)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While SaveNewAlertInstance";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    resultMessage.Tag = tblAlertInstanceTO;
                    return resultMessage;
                }
                #endregion

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Saved Sucessfully";
                resultMessage.DisplayMessage = "Record Saved Sucessfully";
                resultMessage.Result = 1;
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.Text = "Exception Error While Record Save : ConfirmStockSummary";
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region Updation
        public static int UpdateTblStockSummary(TblStockSummaryTO tblStockSummaryTO)
        {
            return TblStockSummaryDAO.UpdateTblStockSummary(tblStockSummaryTO);
        }

        public static int UpdateTblStockSummary(TblStockSummaryTO tblStockSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblStockSummaryDAO.UpdateTblStockSummary(tblStockSummaryTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblStockSummary(Int32 idStockSummary)
        {
            return TblStockSummaryDAO.DeleteTblStockSummary(idStockSummary);
        }

        public static int DeleteTblStockSummary(Int32 idStockSummary, SqlConnection conn, SqlTransaction tran)
        {
            return TblStockSummaryDAO.DeleteTblStockSummary(idStockSummary, conn, tran);
        }

        #endregion
        
    }
}
