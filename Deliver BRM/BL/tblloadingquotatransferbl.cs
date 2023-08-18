using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;


namespace SalesTrackerAPI.BL
{
    public class TblLoadingQuotaTransferBL
    {
        #region Selection
      
        public static List<TblLoadingQuotaTransferTO> SelectAllTblLoadingQuotaTransferList()
        {
            return  TblLoadingQuotaTransferDAO.SelectAllTblLoadingQuotaTransfer();
        }

        public static TblLoadingQuotaTransferTO SelectTblLoadingQuotaTransferTO(Int32 idTransferNote)
        {
            return  TblLoadingQuotaTransferDAO.SelectTblLoadingQuotaTransfer(idTransferNote);
        }



        #endregion

        #region Insertion

        internal static ResultMessage SaveLoadingQuotaTransferNotes(List<TblLoadingQuotaTransferTO> loadingQuotaTransferTOList)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            resultMessage.Text = "Not Entered In The Loop";
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                if (loadingQuotaTransferTOList == null || loadingQuotaTransferTOList.Count == 0)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "loadingQuotaTransferTOList is found empty";
                    return resultMessage;
                }

                DateTime txnDate = loadingQuotaTransferTOList[0].CreatedOn;
                for (int i = 0; i < loadingQuotaTransferTOList.Count; i++)
                {
                    Int32 loadingQuotaId = loadingQuotaTransferTOList[i].AgainstLoadingQuotaId;
                    Double transferQty = loadingQuotaTransferTOList[i].TransferQty;
                    Int32 toCnfOrgId = loadingQuotaTransferTOList[i].ToCnfOrgId;
                    TblLoadingQuotaDeclarationTO loadingQuotaTO = BL.TblLoadingQuotaDeclarationBL.SelectTblLoadingQuotaDeclarationTO(loadingQuotaId, conn, tran);
                    if (loadingQuotaTO == null)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour();
                        resultMessage.Text = "From loadingQuotaTO is found empty";
                        return resultMessage;
                    }

                    if (loadingQuotaTO.BalanceQuota < transferQty)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour();
                        resultMessage.Text = "Can't Continue , " + loadingQuotaTO.MaterialDesc + " " + loadingQuotaTO.ProdCatDesc + "-" + loadingQuotaTO.ProdSpecDesc + " has balance quota +" + loadingQuotaTO.BalanceQuota + " is less then transfer qty :" + transferQty;
                        return resultMessage;
                    }

                    //Vijaymala [04-06-2018] changes the code to get loading quota for regular as well as isstockrequired item 

                    TblLoadingQuotaDeclarationTO toCnfLoadingQuotaTO = BL.TblLoadingQuotaDeclarationBL.SelectTblLoadingQuotaDeclarationTO(toCnfOrgId, loadingQuotaTO.ProdCatId, loadingQuotaTO.ProdSpecId, loadingQuotaTO.MaterialId,loadingQuotaTO.ProdItemId, txnDate, conn, tran);
                    if (toCnfLoadingQuotaTO == null)
                    {
                        // Need to Insert New Record of Quota
                        TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO = new TblLoadingQuotaDeclarationTO();
                        tblLoadingQuotaDeclarationTO.BalanceQuota = transferQty;
                        tblLoadingQuotaDeclarationTO.CnfOrgId = toCnfOrgId;
                        tblLoadingQuotaDeclarationTO.CreatedBy = loadingQuotaTransferTOList[0].CreatedBy;
                        tblLoadingQuotaDeclarationTO.CreatedOn = loadingQuotaTransferTOList[0].CreatedOn;
                        tblLoadingQuotaDeclarationTO.IsActive = 1;
                        tblLoadingQuotaDeclarationTO.ReceivedQuota = transferQty;
                        tblLoadingQuotaDeclarationTO.ProdCatId = loadingQuotaTO.ProdCatId;
                        tblLoadingQuotaDeclarationTO.ProdSpecId = loadingQuotaTO.ProdSpecId;
                        tblLoadingQuotaDeclarationTO.MaterialId = loadingQuotaTO.MaterialId;
                        tblLoadingQuotaDeclarationTO.Remark = "Quota Transfered From C&F :" + loadingQuotaTransferTOList[0].FromCnfOrgName;

                        result = BL.TblLoadingQuotaDeclarationBL.InsertTblLoadingQuotaDeclaration(tblLoadingQuotaDeclarationTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour();
                            resultMessage.Text = "Error While InsertTblLoadingQuotaDeclaration For New Loaing Quota";
                            return resultMessage;
                        }

                        resultMessage = ProcessQuotaTransferNote(loadingQuotaTransferTOList[i], loadingQuotaTO, tblLoadingQuotaDeclarationTO, transferQty, true,false, conn, tran);
                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            tran.Rollback();
                            return resultMessage;
                        }

                    }
                    else
                    {
                        // Need to Update Existing Record for Quota
                        resultMessage = ProcessQuotaTransferNote(loadingQuotaTransferTOList[i], loadingQuotaTO, toCnfLoadingQuotaTO, transferQty, false,false, conn, tran);
                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            tran.Rollback();
                            return resultMessage;
                        }
                    }

                }

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Result = 1;
                return resultMessage;
            }
            catch (Exception ex)
            {
                if (tran.Connection.State == ConnectionState.Open)
                    tran.Rollback();

                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In Method SaveLoadingQuotaTransferNotes";
                resultMessage.Tag = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        internal static ResultMessage SaveMaterialTransferNotes(List<TblLoadingQuotaTransferTO> loadingQuotaTransferTOList)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            resultMessage.Text = "Not Entered In The Loop";
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                if (loadingQuotaTransferTOList == null || loadingQuotaTransferTOList.Count == 0)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "loadingQuotaTransferTOList is found empty";
                    return resultMessage;
                }

                DateTime txnDate = loadingQuotaTransferTOList[0].CreatedOn;
                String alertMsg = "New St To Bend X- Request" + Environment.NewLine;
                for (int i = 0; i < loadingQuotaTransferTOList.Count; i++)
                {
                    Int32 loadingQuotaId = loadingQuotaTransferTOList[i].AgainstLoadingQuotaId;
                    Double transferQty = loadingQuotaTransferTOList[i].TransferQty;
                    Int32 toCnfOrgId = loadingQuotaTransferTOList[i].ToCnfOrgId;

                    TblLoadingQuotaDeclarationTO loadingQuotaTO = BL.TblLoadingQuotaDeclarationBL.SelectTblLoadingQuotaDeclarationTO(loadingQuotaId, conn, tran);
                    if (loadingQuotaTO == null)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour();
                        resultMessage.Text = "From loadingQuotaTO is found empty";
                        return resultMessage;
                    }

                    alertMsg += loadingQuotaTO.MaterialDesc + " " + loadingQuotaTO.ProdCatDesc + " - " + transferQty + " MT" + Environment.NewLine;
                    if (loadingQuotaTO.BalanceQuota < transferQty)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour();
                        resultMessage.Text = "Can't Continue , " + loadingQuotaTO.MaterialDesc + " " + loadingQuotaTO.ProdCatDesc + "-" + loadingQuotaTO.ProdSpecDesc + " has balance quota +" + loadingQuotaTO.BalanceQuota + " is less then transfer qty :" + transferQty;
                        return resultMessage;
                    }

                    //Vijaymala [04-06-2018] changes the code to get loading quota for regular as well as isstockrequired item 

                    TblLoadingQuotaDeclarationTO toCnfLoadingQuotaTO = BL.TblLoadingQuotaDeclarationBL.SelectTblLoadingQuotaDeclarationTO(toCnfOrgId, loadingQuotaTO.ProdCatId, (int)Constants.ProductSpecE.BEND, loadingQuotaTO.MaterialId,loadingQuotaTO.ProdItemId, txnDate, conn, tran);
                    if (toCnfLoadingQuotaTO == null)
                    {
                        // Need to Insert New Record of Quota
                        TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO = new TblLoadingQuotaDeclarationTO();
                        tblLoadingQuotaDeclarationTO.BalanceQuota = transferQty;
                        tblLoadingQuotaDeclarationTO.CnfOrgId = toCnfOrgId;
                        tblLoadingQuotaDeclarationTO.CreatedBy = loadingQuotaTransferTOList[0].CreatedBy;
                        tblLoadingQuotaDeclarationTO.CreatedOn = loadingQuotaTransferTOList[0].CreatedOn;
                        tblLoadingQuotaDeclarationTO.IsActive = 1;
                        tblLoadingQuotaDeclarationTO.ReceivedQuota = transferQty;
                        tblLoadingQuotaDeclarationTO.ProdCatId = loadingQuotaTO.ProdCatId;
                        tblLoadingQuotaDeclarationTO.ProdSpecId = loadingQuotaTO.ProdSpecId;
                        tblLoadingQuotaDeclarationTO.MaterialId = loadingQuotaTO.MaterialId;
                        tblLoadingQuotaDeclarationTO.Remark = "Quota Transfered From C&F :" + loadingQuotaTransferTOList[0].FromCnfOrgName;

                        result = BL.TblLoadingQuotaDeclarationBL.InsertTblLoadingQuotaDeclaration(tblLoadingQuotaDeclarationTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour();
                            resultMessage.Text = "Error While InsertTblLoadingQuotaDeclaration For New Loaing Quota";
                            return resultMessage;
                        }

                        resultMessage = ProcessQuotaTransferNote(loadingQuotaTransferTOList[i], loadingQuotaTO, tblLoadingQuotaDeclarationTO, transferQty, true,true, conn, tran);
                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            tran.Rollback();
                            return resultMessage;
                        }

                    }
                    else
                    {
                        // Need to Update Existing Record for Quota
                        resultMessage = ProcessQuotaTransferNote(loadingQuotaTransferTOList[i], loadingQuotaTO, toCnfLoadingQuotaTO, transferQty, false, true,conn, tran);
                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            tran.Rollback();
                            return resultMessage;
                        }
                    }

                }


                #region 3.2 Notifications of Loading Quota Declaration To All C&F

                TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.STRAIGHT_TO_BEND_TRANSFER_REQUEST;
                tblAlertInstanceTO.AlertAction = "STRAIGHT_TO_BEND_TRANSFER_REQUEST";
                tblAlertInstanceTO.AlertComment = alertMsg;

                tblAlertInstanceTO.EffectiveFromDate = loadingQuotaTransferTOList[0].CreatedOn;
                tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                tblAlertInstanceTO.IsActive = 1;
                tblAlertInstanceTO.SourceDisplayId = "STRAIGHT_TO_BEND_TRANSFER_REQUEST";
                tblAlertInstanceTO.SourceEntityId = loadingQuotaTransferTOList[0].IdTransferNote;
                tblAlertInstanceTO.RaisedBy = loadingQuotaTransferTOList[0].CreatedBy;
                tblAlertInstanceTO.RaisedOn = loadingQuotaTransferTOList[0].CreatedOn;
                tblAlertInstanceTO.IsAutoReset = 1;

                resultMessage = BL.TblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                if (resultMessage.MessageType != ResultMessageE.Information)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While SaveNewAlertInstance";
                    resultMessage.Tag = tblAlertInstanceTO;
                    return resultMessage;
                }
                #endregion

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Saved Successfully";
                resultMessage.Result = 1;
                return resultMessage;
            }
            catch (Exception ex)
            {
                if (tran.Connection.State == ConnectionState.Open)
                    tran.Rollback();

                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In Method SaveLoadingQuotaTransferNotes";
                resultMessage.Tag = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        private static ResultMessage ProcessQuotaTransferNote(TblLoadingQuotaTransferTO tblLoadingQuotaTransferTO, TblLoadingQuotaDeclarationTO loadingQuotaTO, TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO, Double transferQty,Boolean isNewRecordForToCnf, Boolean doStockProcessing, SqlConnection conn, SqlTransaction tran)
        {
            //Save the Note
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();

            result = InsertTblLoadingQuotaTransfer(tblLoadingQuotaTransferTO, conn, tran);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour();
                resultMessage.Text = "Error While InsertTblLoadingQuotaTransfer For New Loading Quota";
                return resultMessage;
            }

            // Update From C&F Quota 
            Double balanceQuota = loadingQuotaTO.BalanceQuota;
            loadingQuotaTO.BalanceQuota = loadingQuotaTO.BalanceQuota - transferQty;
            loadingQuotaTO.TransferedQuota = transferQty;
            loadingQuotaTO.UpdatedBy = tblLoadingQuotaTransferTO.CreatedBy;
            loadingQuotaTO.UpdatedOn = tblLoadingQuotaTransferTO.CreatedOn;
            result = BL.TblLoadingQuotaDeclarationBL.UpdateTblLoadingQuotaDeclaration(loadingQuotaTO, conn, tran);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour();
                resultMessage.Text = "Error While UpdateTblLoadingQuotaDeclaration For New Loading Quota";
                return resultMessage;
            }

            //History Record For From C&F Loading Quota consumptions
            TblLoadingQuotaConsumptionTO consumptionTO = new Models.TblLoadingQuotaConsumptionTO();
            consumptionTO.AvailableQuota = balanceQuota;
            consumptionTO.BalanceQuota = loadingQuotaTO.BalanceQuota;
            consumptionTO.CreatedBy = tblLoadingQuotaTransferTO.CreatedBy;
            consumptionTO.CreatedOn = tblLoadingQuotaTransferTO.CreatedOn;
            consumptionTO.LoadingQuotaId = loadingQuotaTO.IdLoadingQuota;
            consumptionTO.TransferNoteId = tblLoadingQuotaTransferTO.IdTransferNote;
            consumptionTO.TxnOpTypeId = (int)Constants.TxnOperationTypeE.OUT;
            consumptionTO.QuotaQty = -transferQty;
            consumptionTO.Remark = "Quota Transfered  From C&F :" + tblLoadingQuotaTransferTO.FromCnfOrgName + " to " + tblLoadingQuotaTransferTO.ToCnfOrgName;
            result = BL.TblLoadingQuotaConsumptionBL.InsertTblLoadingQuotaConsumption(consumptionTO, conn, tran);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour();
                resultMessage.Text = "Error : While InsertTblLoadingQuotaConsumption Against LoadingSlip";
                return resultMessage;
            }


            // Update to C&F Quota 
            Double balanceToQuota = 0;
            if (!isNewRecordForToCnf)
            {
                balanceToQuota = tblLoadingQuotaDeclarationTO.BalanceQuota;
                tblLoadingQuotaDeclarationTO.BalanceQuota += transferQty;
                tblLoadingQuotaDeclarationTO.ReceivedQuota += transferQty;
                tblLoadingQuotaDeclarationTO.UpdatedBy = tblLoadingQuotaTransferTO.CreatedBy;
                tblLoadingQuotaDeclarationTO.UpdatedOn = tblLoadingQuotaTransferTO.CreatedOn;
                result = BL.TblLoadingQuotaDeclarationBL.UpdateTblLoadingQuotaDeclaration(tblLoadingQuotaDeclarationTO, conn, tran);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "Error While UpdateTblLoadingQuotaDeclaration For New Loading Quota";
                    return resultMessage;
                }
            }

            //History Record For To C&F Loading Quota consumptions
            TblLoadingQuotaConsumptionTO toCnfConsumptionTO = new Models.TblLoadingQuotaConsumptionTO();
            if (isNewRecordForToCnf)
            {
                toCnfConsumptionTO.AvailableQuota = 0;
                toCnfConsumptionTO.BalanceQuota = transferQty;
            }
            else
            {
                toCnfConsumptionTO.AvailableQuota = balanceToQuota;
                toCnfConsumptionTO.BalanceQuota = tblLoadingQuotaDeclarationTO.BalanceQuota;
            }

            toCnfConsumptionTO.CreatedBy = tblLoadingQuotaTransferTO.CreatedBy;
            toCnfConsumptionTO.CreatedOn = tblLoadingQuotaTransferTO.CreatedOn;
            toCnfConsumptionTO.LoadingQuotaId = tblLoadingQuotaDeclarationTO.IdLoadingQuota;
            toCnfConsumptionTO.TransferNoteId = tblLoadingQuotaTransferTO.IdTransferNote;
            toCnfConsumptionTO.TxnOpTypeId = (int)Constants.TxnOperationTypeE.IN;
            toCnfConsumptionTO.QuotaQty = transferQty;
            toCnfConsumptionTO.Remark = "Quota Received  From C&F :" + tblLoadingQuotaTransferTO.FromCnfOrgName;
            result = BL.TblLoadingQuotaConsumptionBL.InsertTblLoadingQuotaConsumption(toCnfConsumptionTO, conn, tran);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour();
                resultMessage.Text = "Error : While InsertTblLoadingQuotaConsumption Against LoadingSlip";
                return resultMessage;
            }

            if (doStockProcessing)
            {
                //Reduce From Stock Qty
                List<TblStockDetailsTO> fromStockDetailsTOList = BL.TblStockDetailsBL.SelectStockDetailsListByProdCatgSpecAndMaterial(loadingQuotaTO.ProdCatId, loadingQuotaTO.ProdSpecId, loadingQuotaTO.MaterialId, loadingQuotaTO.CreatedOn, conn, tran);
                if (fromStockDetailsTOList != null)
                {
                    Double stkQty = transferQty;

                    for (int si = 0; si < fromStockDetailsTOList.Count; si++)
                    {
                        TblStockDetailsTO fromTblStockDetailsTO = fromStockDetailsTOList[si];

                        if (stkQty > 0)
                        {
                            if (fromTblStockDetailsTO.BalanceStock <= 0)
                                continue;

                            Double txnQty = 0;

                            if (fromTblStockDetailsTO.BalanceStock >= stkQty)
                            {
                                txnQty = stkQty;
                                stkQty = 0;
                            }
                            else
                            {
                                txnQty = fromTblStockDetailsTO.BalanceStock;
                                stkQty = stkQty - fromTblStockDetailsTO.BalanceStock;
                            }

                            // Insert From Stock Consumption Record
                            TblStockConsumptionTO fromStockConsumptionTO = new TblStockConsumptionTO();
                            fromStockConsumptionTO.BeforeStockQty = fromTblStockDetailsTO.BalanceStock;
                            fromStockConsumptionTO.AfterStockQty = fromTblStockDetailsTO.BalanceStock - txnQty;
                            fromStockConsumptionTO.CreatedBy = tblLoadingQuotaTransferTO.CreatedBy;
                            fromStockConsumptionTO.CreatedOn = tblLoadingQuotaTransferTO.CreatedOn;
                            fromStockConsumptionTO.StockDtlId = fromTblStockDetailsTO.IdStockDtl;
                            fromStockConsumptionTO.TxnQty = -txnQty;
                            fromStockConsumptionTO.TxnOpTypeId = (int)Constants.TxnOperationTypeE.OUT;
                            fromStockConsumptionTO.TransferNoteId = tblLoadingQuotaTransferTO.IdTransferNote;
                            fromStockConsumptionTO.Remark = "Internal Stock Transfer";

                            result = BL.TblStockConsumptionBL.InsertTblStockConsumption(fromStockConsumptionTO, conn, tran);
                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour();
                                resultMessage.Text = "Error : While InsertTblStockConsumption Against LoadingSlip";
                                return resultMessage;
                            }


                            //Update Stock Details

                            fromTblStockDetailsTO.BalanceStock = fromTblStockDetailsTO.BalanceStock - txnQty;
                            fromTblStockDetailsTO.TotalStock = fromTblStockDetailsTO.TotalStock - txnQty;
                            fromTblStockDetailsTO.UpdatedBy = tblLoadingQuotaTransferTO.CreatedBy;
                            fromTblStockDetailsTO.UpdatedOn = tblLoadingQuotaTransferTO.CreatedOn;

                            result = BL.TblStockDetailsBL.UpdateTblStockDetails(fromTblStockDetailsTO, conn, tran);
                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour();
                                resultMessage.Text = "Error : While UpdateTblStockDetails Against LoadingSlip";
                                return resultMessage;
                            }


                            //From Stock Transfer Note
                            TblStockTransferNoteTO stkTrnsferNoteTO = new TblStockTransferNoteTO();
                            stkTrnsferNoteTO.CreatedBy = tblLoadingQuotaTransferTO.CreatedBy;
                            stkTrnsferNoteTO.CreatedOn = tblLoadingQuotaTransferTO.CreatedOn;
                            stkTrnsferNoteTO.LocationId = fromTblStockDetailsTO.LocationId;
                            stkTrnsferNoteTO.MaterialId = fromTblStockDetailsTO.MaterialId;
                            stkTrnsferNoteTO.ProdCatId = fromTblStockDetailsTO.ProdCatId;
                            stkTrnsferNoteTO.ProdSpecId = fromTblStockDetailsTO.ProdSpecId;
                            stkTrnsferNoteTO.StockQtyMT = txnQty;
                            stkTrnsferNoteTO.TxnOpTypeId = (int)Constants.TxnOperationTypeE.OUT;
                            stkTrnsferNoteTO.Remark = tblLoadingQuotaTransferTO.TransferDesc;

                            result = BL.TblStockTransferNoteBL.InsertTblStockTransferNote(stkTrnsferNoteTO, conn, tran);
                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour();
                                resultMessage.Text = "Error : While InsertTblStockTransferNote Against LoadingSlip";
                                return resultMessage;
                            }

                            TblStockDetailsTO toTblStockDetailsTO = null;
                            List<TblStockDetailsTO> toStockDetailsTOList = BL.TblStockDetailsBL.SelectStockDetailsListByProdCatgSpecAndMaterial(tblLoadingQuotaDeclarationTO.ProdCatId, tblLoadingQuotaDeclarationTO.ProdSpecId, tblLoadingQuotaDeclarationTO.MaterialId, tblLoadingQuotaDeclarationTO.CreatedOn, conn, tran);
                            if (toStockDetailsTOList != null && toStockDetailsTOList.Count > 0)
                            {
                                toTblStockDetailsTO = toStockDetailsTOList[0];
                            }
                            else
                            {
                                //Insert New Stock Details
                                toTblStockDetailsTO = new TblStockDetailsTO();
                                toTblStockDetailsTO.LocationId = fromTblStockDetailsTO.LocationId;
                                toTblStockDetailsTO.MaterialId = tblLoadingQuotaDeclarationTO.MaterialId;
                                toTblStockDetailsTO.ProdCatId = tblLoadingQuotaDeclarationTO.ProdCatId;
                                toTblStockDetailsTO.ProdSpecId = tblLoadingQuotaDeclarationTO.ProdSpecId;
                                toTblStockDetailsTO.CreatedBy = tblLoadingQuotaTransferTO.CreatedBy;
                                toTblStockDetailsTO.CreatedOn = tblLoadingQuotaTransferTO.CreatedOn;
                                toTblStockDetailsTO.BalanceStock = 0;
                                toTblStockDetailsTO.TotalStock = 0;
                                toTblStockDetailsTO.StockSummaryId = fromTblStockDetailsTO.StockSummaryId;
                                result = BL.TblStockDetailsBL.InsertTblStockDetails(toTblStockDetailsTO, conn, tran);
                                if (result != 1)
                                {
                                    resultMessage.DefaultBehaviour();
                                    resultMessage.Text = "Error : While InsertTblStockDetails For transfered TO Stock Details";
                                    return resultMessage;
                                }
                            }

                            // Insert to Stock Consumption Record
                            TblStockConsumptionTO toStockConsumptionTO = new TblStockConsumptionTO();
                            toStockConsumptionTO.BeforeStockQty = toTblStockDetailsTO.BalanceStock;
                            toStockConsumptionTO.AfterStockQty = toTblStockDetailsTO.BalanceStock + txnQty;
                            toStockConsumptionTO.CreatedBy = tblLoadingQuotaTransferTO.CreatedBy;
                            toStockConsumptionTO.CreatedOn = tblLoadingQuotaTransferTO.CreatedOn;
                            toStockConsumptionTO.StockDtlId = toTblStockDetailsTO.IdStockDtl;
                            toStockConsumptionTO.TxnQty = txnQty;
                            toStockConsumptionTO.TxnOpTypeId = (int)Constants.TxnOperationTypeE.IN;
                            toStockConsumptionTO.TransferNoteId = tblLoadingQuotaTransferTO.IdTransferNote;
                            toStockConsumptionTO.Remark = "Internal Stock Transfer";

                            result = BL.TblStockConsumptionBL.InsertTblStockConsumption(toStockConsumptionTO, conn, tran);
                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour();
                                resultMessage.Text = "Error : While InsertTblStockConsumption Against LoadingSlip";
                                return resultMessage;
                            }


                            //Update to Stock Details

                            toTblStockDetailsTO.BalanceStock = toTblStockDetailsTO.BalanceStock + txnQty;
                            toTblStockDetailsTO.TotalStock = toTblStockDetailsTO.TotalStock + txnQty;
                            toTblStockDetailsTO.UpdatedBy = tblLoadingQuotaTransferTO.CreatedBy;
                            toTblStockDetailsTO.UpdatedOn = tblLoadingQuotaTransferTO.CreatedOn;
                            stkQty = 0;

                            result = BL.TblStockDetailsBL.UpdateTblStockDetails(toTblStockDetailsTO, conn, tran);
                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour();
                                resultMessage.Text = "Error : While to UpdateTblStockDetails Against LoadingSlip";
                                return resultMessage;
                            }


                            //to Stock Transfer Note
                            TblStockTransferNoteTO toStkTrnsferNoteTO = new TblStockTransferNoteTO();
                            toStkTrnsferNoteTO.CreatedBy = tblLoadingQuotaTransferTO.CreatedBy;
                            toStkTrnsferNoteTO.CreatedOn = tblLoadingQuotaTransferTO.CreatedOn;
                            toStkTrnsferNoteTO.LocationId = toTblStockDetailsTO.LocationId;
                            toStkTrnsferNoteTO.MaterialId = toTblStockDetailsTO.MaterialId;
                            toStkTrnsferNoteTO.ProdCatId = toTblStockDetailsTO.ProdCatId;
                            toStkTrnsferNoteTO.ProdSpecId = toTblStockDetailsTO.ProdSpecId;
                            toStkTrnsferNoteTO.StockQtyMT = txnQty;
                            toStkTrnsferNoteTO.TxnOpTypeId = (int)Constants.TxnOperationTypeE.IN;
                            toStkTrnsferNoteTO.Remark = tblLoadingQuotaTransferTO.TransferDesc;

                            result = BL.TblStockTransferNoteBL.InsertTblStockTransferNote(toStkTrnsferNoteTO, conn, tran);
                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour();
                                resultMessage.Text = "Error : While to InsertTblStockTransferNote Against LoadingSlip";
                                return resultMessage;
                            }

                        }
                        else break;
                    }
                }
            }

            resultMessage.Text = "Transfer Note Processed Successfully";
            resultMessage.MessageType = ResultMessageE.Information;
            resultMessage.Result = 1;
            return resultMessage;
        }


        

        public static int InsertTblLoadingQuotaTransfer(TblLoadingQuotaTransferTO tblLoadingQuotaTransferTO)
        {
            return TblLoadingQuotaTransferDAO.InsertTblLoadingQuotaTransfer(tblLoadingQuotaTransferTO);
        }

        public static int InsertTblLoadingQuotaTransfer(TblLoadingQuotaTransferTO tblLoadingQuotaTransferTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingQuotaTransferDAO.InsertTblLoadingQuotaTransfer(tblLoadingQuotaTransferTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblLoadingQuotaTransfer(TblLoadingQuotaTransferTO tblLoadingQuotaTransferTO)
        {
            return TblLoadingQuotaTransferDAO.UpdateTblLoadingQuotaTransfer(tblLoadingQuotaTransferTO);
        }

        public static int UpdateTblLoadingQuotaTransfer(TblLoadingQuotaTransferTO tblLoadingQuotaTransferTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingQuotaTransferDAO.UpdateTblLoadingQuotaTransfer(tblLoadingQuotaTransferTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblLoadingQuotaTransfer(Int32 idTransferNote)
        {
            return TblLoadingQuotaTransferDAO.DeleteTblLoadingQuotaTransfer(idTransferNote);
        }

        public static int DeleteTblLoadingQuotaTransfer(Int32 idTransferNote, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingQuotaTransferDAO.DeleteTblLoadingQuotaTransfer(idTransferNote, conn, tran);
        }

       

        #endregion

    }
}
