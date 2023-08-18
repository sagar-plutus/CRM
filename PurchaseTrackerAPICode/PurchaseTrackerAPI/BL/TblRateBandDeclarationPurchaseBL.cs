using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL
{
    public class TblRateBandDeclarationPurchaseBL : ITblRateBandDeclarationPurchaseBL
    {
        private readonly ITblPurchaseBookingActionsBL _iTblPurchaseBookingActionsBL;
        private readonly ITblPersonBL _iTblPersonBL;
        private readonly ITblBaseItemMetalCostBL _iTblBaseItemMetalCostBL;
        private readonly ITblGlobalRatePurchaseBL _iTblGlobalRatePurchaseBL;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly INotification notify;
        private readonly ITblRateBandDeclarationPurchaseDAO _iTblRateBandDeclarationPurchaseDAO;
        private readonly ITblPurchaseScheduleSummaryBL _iTblPurchaseScheduleSummaryBL;
        private readonly ITblGradeWiseTargetQtyBL _iTblGradeWiseTargetQtyBL;
        private readonly ITblGradeExpressionDtlsBL _iTblGradeExpressionDtlsBL;
        private readonly ITblVariablesBL _iTblVariablesBL;
        private readonly ITblProductItemBL _iTblProductItemBL;
        private readonly Icommondao _iCommonDAO;
        private readonly IConnectionString _iConnectionString;
        public TblRateBandDeclarationPurchaseBL(
            ITblPurchaseScheduleSummaryBL iTblPurchaseScheduleSummaryBL,
            ITblRateBandDeclarationPurchaseDAO iTblRateBandDeclarationPurchaseDAO, ITblGradeWiseTargetQtyBL iTblGradeWiseTargetQtyBL,
            ITblConfigParamsBL iTblConfigParamsBL,
            Icommondao icommondao,
            IConnectionString iConnectionString,
            ITblGlobalRatePurchaseBL iTblGlobalRatePurchaseBL
            , ITblBaseItemMetalCostBL iTblBaseItemMetalCostBL
            , ITblGradeExpressionDtlsBL iTblGradeExpressionDtlsBL
            , ITblPersonBL iTblPersonBL
            , ITblPurchaseBookingActionsBL iTblPurchaseBookingActionsBL
            , ITblVariablesBL iTblVariablesBL
            , ITblProductItemBL iTblProductItemBL, INotification inotify)
        {
            _iConnectionString = iConnectionString;
            _iCommonDAO = icommondao;
            notify = inotify;
            _iTblProductItemBL = iTblProductItemBL;
            _iTblVariablesBL = iTblVariablesBL;
            _iTblPurchaseBookingActionsBL = iTblPurchaseBookingActionsBL;
            _iTblPersonBL = iTblPersonBL;
            _iTblGradeExpressionDtlsBL = iTblGradeExpressionDtlsBL;
            _iTblBaseItemMetalCostBL = iTblBaseItemMetalCostBL;
            _iTblGlobalRatePurchaseBL = iTblGlobalRatePurchaseBL;
            _iTblConfigParamsBL = iTblConfigParamsBL;

            _iTblRateBandDeclarationPurchaseDAO = iTblRateBandDeclarationPurchaseDAO;
            _iTblPurchaseScheduleSummaryBL = iTblPurchaseScheduleSummaryBL;
            _iTblGradeWiseTargetQtyBL = iTblGradeWiseTargetQtyBL;
        }
        #region Selection

        /// <summary>
        /// swati pisal
        /// </summary>
        /// <param name="globalRatePurchaseId"></param>
        /// <returns></returns>
        public Dictionary<Int32, List<TblRateBandDeclarationPurchaseTO>> SelectAllTblRateBandDeclarationPurchaseList(Int32 globalRatePurchaseId)
        {
            try
            {
                List<TblRateBandDeclarationPurchaseTO> purchaseManagerList = SelectAllTblUserOfPurchase(); // DAL.TblRateBandDeclarationPurchaseDAO.SelectAllTblUserOfPurchase();
                List<TblRateBandDeclarationPurchaseTO> tblRateBandDeclarationPurchaseTO = _iTblRateBandDeclarationPurchaseDAO.SelectAllTblRateBandDeclarationPurchase(globalRatePurchaseId);
                Dictionary<Int32, List<TblRateBandDeclarationPurchaseTO>> purchaseWithBandDCT = new Dictionary<int, List<TblRateBandDeclarationPurchaseTO>>();

                if (purchaseManagerList != null)
                {

                    for (int i = 0; i < purchaseManagerList.Count; i++)
                    {
                        TblRateBandDeclarationPurchaseTO tblPurchaseTO = purchaseManagerList[i];
                        if (tblRateBandDeclarationPurchaseTO != null)
                        {
                            TblRateBandDeclarationPurchaseTO result = tblRateBandDeclarationPurchaseTO.Find(x => x.UserId == tblPurchaseTO.UserId);
                            if (result != null)
                                purchaseWithBandDCT.Add(tblPurchaseTO.UserId, tblRateBandDeclarationPurchaseTO);
                            else
                                purchaseWithBandDCT.Add(tblPurchaseTO.UserId, null);
                        }
                    }
                }

                return purchaseWithBandDCT;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }
        }

        public List<TblRateBandDeclarationPurchaseTO> SelectAllTblRateBandDeclarationPurchase(Int32 globalRatePurchaseId)
        {
            List<TblRateBandDeclarationPurchaseTO> tblRateBandDeclarationPurchaseTOList = _iTblRateBandDeclarationPurchaseDAO.SelectAllTblRateBandDeclarationPurchase(globalRatePurchaseId);
            return tblRateBandDeclarationPurchaseTOList;
        }
        public List<TblRateBandDeclarationPurchaseTO> GetPurchaseManagerWithBand(Int32 globalRatePurchaseId)
        {
            try
            {
                List<TblRateBandDeclarationPurchaseTO> purchaseManagerList = SelectAllTblUserOfPurchase();
                List<TblRateBandDeclarationPurchaseTO> tblRateBandDeclarationPurchaseTO = _iTblRateBandDeclarationPurchaseDAO.SelectAllTblRateBandDeclarationPurchase(globalRatePurchaseId);
                //List<TblRateBandDeclarationPurchaseTO> purchaseWithBandDCT = purchaseManagerList;
                if (purchaseManagerList != null)
                {
                    for (int i = 0; i < tblRateBandDeclarationPurchaseTO.Count; i++)
                    {
                        TblRateBandDeclarationPurchaseTO tblPurchaseTO = tblRateBandDeclarationPurchaseTO[i];
                        //TblRateBandDeclarationPurchaseTO result = tblRateBandDeclarationPurchaseTO.Find(x => x.UserId == tblPurchaseTO.UserId);

                        foreach (var purchaseManger in purchaseManagerList)
                        {
                            if (purchaseManger.UserId == tblPurchaseTO.UserId)
                            {
                                purchaseManger.RateBandCorrection = tblPurchaseTO.RateBandCorrection;
                                purchaseManger.RateBandCosting = tblPurchaseTO.RateBandCosting;
                                purchaseManger.ValidUpto = tblPurchaseTO.ValidUpto;
                                purchaseManger.IdRateBandDeclarationPurchase = tblPurchaseTO.IdRateBandDeclarationPurchase;
                                purchaseManger.GlobalRatePurchaseId = tblPurchaseTO.GlobalRatePurchaseId;

                                //Get Target Qty And Target Date Details
                                purchaseManger.GradeWiseTargetQtyTOList = new List<TblGradeWiseTargetQtyTO>();
                                purchaseManger.GradeWiseTargetQtyTOList = _iTblGradeWiseTargetQtyBL.SelectGradeWiseTargetQtyDtls(tblPurchaseTO.IdRateBandDeclarationPurchase, tblPurchaseTO.UserId);
                                if (purchaseManger.GradeWiseTargetQtyTOList == null)
                                {
                                    purchaseManger.GradeWiseTargetQtyTOList = new List<TblGradeWiseTargetQtyTO>();
                                }

                            }
                        }



                    }
                }

                return purchaseManagerList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }
        }

        public List<TblRateBandDeclarationPurchaseTO> SelectAllTblUserOfPurchase()
        {
            List<TblRateBandDeclarationPurchaseTO> purchaseManagerList = _iTblRateBandDeclarationPurchaseDAO.SelectAllTblUserOfPurchase();
            return purchaseManagerList;
        }

        public PurchaseTrackerAPI.DashboardModels.QuotaAndRateInfo SelectQuotaAndRateDashboardInfo(Int32 roleId, Int32 orgId, DateTime sysDate)
        {
            try
            {
                return _iTblRateBandDeclarationPurchaseDAO.SelectDashboardQuotaAndRateInfo(roleId, orgId, sysDate);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public TblRateBandDeclarationPurchaseTO SelectTblRateBandDeclaration(Int32 idRateBandDeclaration, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblRateBandDeclarationPurchaseDAO.SelectTblRateBandDeclaration(idRateBandDeclaration, conn, tran);

        }
        public TblRateBandDeclarationPurchaseTO SelectTblRateBandDeclaration(Int32 idRateBandDeclaration)
        {
            return _iTblRateBandDeclarationPurchaseDAO.SelectTblRateBandDeclaration(idRateBandDeclaration);

        }
        public List<TblRateBandDeclarationPurchaseTO> SelectLatestRateBandDeclarationPurchaseTOList(Int32 cnfId, DateTime date)
        {
            return _iTblRateBandDeclarationPurchaseDAO.SelectLatestRateBandDeclarationPurchaseTOList(cnfId, date);

        }


    

        public TblRateBandDeclarationPurchaseTO SelectOldRateTOList(Int32 cnfId, DateTime date)
        {
            return _iTblRateBandDeclarationPurchaseDAO.SelectOldRateTOList(cnfId, date);

        }
        public TblRateBandDeclarationPurchaseTO SelectLatestRateTOList(Int32 cnfId, DateTime date)
        {
            return _iTblRateBandDeclarationPurchaseDAO.SelectLatestRateTOList(cnfId, date);

        }

        public TblRateBandDeclarationPurchaseTO SelectPreviousTblRateDeclarationTO(Int32 idRateBandDeclarationPurchase, Int32 userId)
        {
            return _iTblRateBandDeclarationPurchaseDAO.SelectPreviousTblRateDeclarationTO(idRateBandDeclarationPurchase, userId);

        }
        public Boolean CheckForValidityAndReset(TblRateBandDeclarationPurchaseTO tblRateBandDeclarationPurchaseTO)
        {
            TblRateBandDeclarationPurchaseTO prevQuotaDeclaTO = SelectPreviousTblRateDeclarationTO(tblRateBandDeclarationPurchaseTO.IdRateBandDeclarationPurchase, tblRateBandDeclarationPurchaseTO.UserId);
            if (prevQuotaDeclaTO == null)
                return true;
            else
            {
                //swati pisal
                //need to confirm here we check createdon instead on quotaallocatedate
                DateTime timeToCheck = prevQuotaDeclaTO.CreatedOn.AddMinutes(tblRateBandDeclarationPurchaseTO.ValidUpto);
                DateTime serverDateTime = _iCommonDAO.ServerDateTime;
                if (timeToCheck < serverDateTime)
                {
                    tblRateBandDeclarationPurchaseTO.IsActive = 0;
                    TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_SYTEM_ADMIN_USER_ID);

                    tblRateBandDeclarationPurchaseTO.UpdatedBy = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
                    tblRateBandDeclarationPurchaseTO.UpdatedOn = serverDateTime;
                    int result = UpdateTblRateDeclaration(tblRateBandDeclarationPurchaseTO);
                    return false;
                }
                else return true;
            }
        }
        #endregion

        #region Insertion
        public int InsertTblRateBandDeclarationPurchase(TblRateBandDeclarationPurchaseTO tblRateBandDeclarationPurchaseTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblRateBandDeclarationPurchaseDAO.InsertTblRateBandDeclarationPurchase(tblRateBandDeclarationPurchaseTO, conn, tran);
        }

        /// <summary>
        /// modified by swati pisal for purchase
        /// </summary>
        /// <param name="tblGlobalRatePurchaseTOList"></param>
        /// <returns></returns>
        public ResultMessage SaveDeclaredRate(List<TblGlobalRatePurchaseTO> tblGlobalRatePurchaseTOList, Int32 loginUserId, DateTime serverDate)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                #region 1. Save the Declared Rate

                Boolean isRateAlreadyDeclare = _iTblGlobalRatePurchaseBL.IsRateAlreadyDeclaredForTheDate(tblGlobalRatePurchaseTOList[0].CreatedOn, conn, tran);
                String rateString = string.Empty;
                //This condition means if new declared quota is not found then new rate can not be declared
                if (tblGlobalRatePurchaseTOList != null && tblGlobalRatePurchaseTOList.Count > 0)
                {
                    #region 1.1 Deactivate All Previous Declared Quota

                    result = _iTblRateBandDeclarationPurchaseDAO.DeactivateAllDeclaredQuota(tblGlobalRatePurchaseTOList[0].CreatedBy, conn, tran);
                    if (result == -1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("DeactivateAllDeclaredQuota");
                        return resultMessage;
                    }

                    #endregion

                    for (int i = 0; i < tblGlobalRatePurchaseTOList.Count; i++)
                    {
                        TblGlobalRatePurchaseTO TblGlobalRatePurchaseTO = tblGlobalRatePurchaseTOList[i];
                        rateString += TblGlobalRatePurchaseTO.Rate + ",";
                        if (TblGlobalRatePurchaseTO.RateReasonDesc != "Other")
                            TblGlobalRatePurchaseTO.Comments = TblGlobalRatePurchaseTO.RateReasonDesc;

                        result = _iTblGlobalRatePurchaseBL.InsertTblGlobalRatePurchase(TblGlobalRatePurchaseTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While InsertTblGlobalRatePurchase");
                            return resultMessage;
                        }

                        #region 1.2. Save C&F Allocated Rate Band
                        if (TblGlobalRatePurchaseTO.RateBandDeclarationPurchaseTOList != null && TblGlobalRatePurchaseTO.RateBandDeclarationPurchaseTOList.Count > 0)
                        {
                            for (int qd = 0; qd < TblGlobalRatePurchaseTO.RateBandDeclarationPurchaseTOList.Count; qd++)
                            {
                                TblRateBandDeclarationPurchaseTO tblRateBandDeclarationPurchaseTO = new TblRateBandDeclarationPurchaseTO();
                                TblRateBandDeclarationPurchaseTO rateBandTO = TblGlobalRatePurchaseTO.RateBandDeclarationPurchaseTOList[qd];

                                tblRateBandDeclarationPurchaseTO.UserId = rateBandTO.UserId;
                                tblRateBandDeclarationPurchaseTO.GlobalRatePurchaseId = TblGlobalRatePurchaseTO.IdGlobalRatePurchase;
                                tblRateBandDeclarationPurchaseTO.RateBandCosting = rateBandTO.RateBandCosting;
                                tblRateBandDeclarationPurchaseTO.RateBandCorrection = rateBandTO.RateBandCorrection;
                                tblRateBandDeclarationPurchaseTO.ValidUpto = rateBandTO.ValidUpto;
                                tblRateBandDeclarationPurchaseTO.CalculatedRateCosting = rateBandTO.CalculatedRateCosting;
                                tblRateBandDeclarationPurchaseTO.CalculatedRateCorrection = rateBandTO.CalculatedRateCorrection;
                                tblRateBandDeclarationPurchaseTO.CreatedOn = rateBandTO.CreatedOn;
                                tblRateBandDeclarationPurchaseTO.CreatedBy = rateBandTO.CreatedBy;
                                tblRateBandDeclarationPurchaseTO.IsActive = 1;
                                tblRateBandDeclarationPurchaseTO.GradeWiseTargetQtyTOList = rateBandTO.GradeWiseTargetQtyTOList;

                                //tblRateBandDeclarationPurchaseTO.RateBandCorrection = TblGlobalRatePurchaseTO.Rate - tblRateBandDeclarationPurchaseTO.RateBandCorrection;

                                result = InsertTblRateBandDeclarationPurchase(tblRateBandDeclarationPurchaseTO, conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour("Error While InsertTblRateBandDeclarationPurchase");
                                    return resultMessage;
                                }
                                else
                                {
                                    #region Update Booking And Unloading Target Qty Details
                                    if (tblRateBandDeclarationPurchaseTO.GradeWiseTargetQtyTOList != null && tblRateBandDeclarationPurchaseTO.GradeWiseTargetQtyTOList.Count > 0)
                                    {
                                        for (int p = 0; p < tblRateBandDeclarationPurchaseTO.GradeWiseTargetQtyTOList.Count; p++)
                                        {
                                            tblRateBandDeclarationPurchaseTO.GradeWiseTargetQtyTOList[p].RateBandPurchaseId = tblRateBandDeclarationPurchaseTO.IdRateBandDeclarationPurchase;
                                            result = _iTblGradeWiseTargetQtyBL.InsertTblGradeWiseTargetQty(tblRateBandDeclarationPurchaseTO.GradeWiseTargetQtyTOList[p], conn, tran);
                                            if (result != 1)
                                            {
                                                tran.Rollback();
                                                resultMessage.DefaultBehaviour("Error While InsertTblGradeWiseTargetQty");
                                                return resultMessage;
                                            }
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                        #endregion

                        #region Save Base Item Cost details

                        TblConfigParamsTO isForBRMConfigTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_SCRAP_IS_FOR_BHAGYALAXMI, conn, tran);
                        if (isForBRMConfigTO != null)
                        {


                            if (isForBRMConfigTO.ConfigParamVal == "1")
                            {
                                Int32 defaultStateId;

                                TblConfigParamsTO defaultStateIdConfigTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_SCRAP_DEAULT_STATE_ID, conn, tran);
                                if (defaultStateIdConfigTO == null)
                                {
                                    throw new Exception("defaultStateIdConfigTO == NULL");
                                }

                                TblConfigParamsTO defaultRateAndRecConfigTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_SCRAP_DEFAULT_RATE_REC_VARIABLES, conn, tran);
                                if (defaultRateAndRecConfigTO == null)
                                {
                                    throw new Exception("defaultRateAndRecConfigTO == NULL");
                                }

                                List<TblVariablesTO> tblVariablesTOList = _iTblVariablesBL.SelectActiveVariablesList(conn, tran);
                                if (tblVariablesTOList == null || tblVariablesTOList.Count == 0)
                                {
                                    throw new Exception("Variables list not found");
                                }

                                double defaultRateForNC = 0;
                                double defaultRateForC = 0;
                                double defaultRec = 0;
                                //Int32 count = 2;
                                Int32 count = 3;

                                string configParamVal = defaultRateAndRecConfigTO.ConfigParamVal;
                                if (!string.IsNullOrEmpty(configParamVal))
                                {
                                    string[] arr = configParamVal.Split(',');

                                    if (arr != null && arr.Length > 0)
                                    {
                                        for (int q = 0; q < count; q++)
                                        {
                                            TblVariablesTO tempTO = tblVariablesTOList.Where(a => a.VariableCode == arr[q]).FirstOrDefault();
                                            if (tempTO != null)
                                            {
                                                if (q == 0)
                                                {
                                                    defaultRateForC = tempTO.VariableValue;
                                                }
                                                else if(q==1)
                                                {
                                                    defaultRateForNC = tempTO.VariableValue;
                                                }
                                                else
                                                {
                                                    defaultRec = tempTO.VariableValue;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    throw new Exception("Default Rate and Recovery variables not found");
                                }

                                defaultStateId = Convert.ToInt32(defaultStateIdConfigTO.ConfigParamVal.ToString());
                                Int32 isBaseItem = 1;

                                TblProductItemTO baseItemTO = _iTblProductItemBL.GetBaseItemRecovery(isBaseItem, defaultStateId, conn, tran);
                                if (baseItemTO == null)
                                {
                                    throw new Exception("baseItemTO == NULL");
                                }

                                //tblBaseItemMetalCostTO.BaseRecovery = baseItemTO.Recovery;

                                count = 2;
                                for (int k = 0; k < count; k++)
                                {
                                    TblPurchaseVehicleDetailsTO tempTO = new TblPurchaseVehicleDetailsTO();
                                    List<TblPurchaseVehicleDetailsTO> tempList = new List<TblPurchaseVehicleDetailsTO>();
                                    TblBaseItemMetalCostTO tblBaseItemMetalCostTO = new TblBaseItemMetalCostTO();

                                    tblBaseItemMetalCostTO.BaseRecovery = defaultRec;



                                    
                                    tblBaseItemMetalCostTO.CreatedBy = TblGlobalRatePurchaseTO.CreatedBy;
                                    tblBaseItemMetalCostTO.CreatedOn = TblGlobalRatePurchaseTO.CreatedOn;
                                    tblBaseItemMetalCostTO.UpdatedBy = loginUserId;
                                    tblBaseItemMetalCostTO.UpdatedOn = serverDate;

                                    tempTO.ProdItemId = baseItemTO.IdProdItem;
                                    tempTO.ProdClassId = baseItemTO.ProdClassId;
                                    tempTO.Qty = 1;
                                    tempTO.IsNonCommercialItem = baseItemTO.IsNonCommercialItem;

                                    // tempTO.Recovery = baseItemTO.Recovery;
                                    tempTO.Recovery = defaultRec;

                                    //Prajakta [2019-04-22] Commented
                                    if (k == 0)
                                    {
                                        //tempTO.Rate = TblGlobalRatePurchaseTO.Rate + baseItemTO.ParityAmt;
                                        //Prajakta[2019-05-06] Added to calculate base metal cost as per C or NC
                                        tempTO.CorNcId = (Int32)Constants.ConfirmTypeE.CONFIRM;
                                        tblBaseItemMetalCostTO.BaseRate = defaultRateForC;
                                    }
                                    else
                                    {
                                        //tempTO.Rate = TblGlobalRatePurchaseTO.Rate + baseItemTO.ParityAmt + baseItemTO.NonConfParityAmt;
                                        tempTO.CorNcId = (Int32)Constants.ConfirmTypeE.NONCONFIRM;
                                        tblBaseItemMetalCostTO.BaseRate = defaultRateForNC;
                                    }

                                    tempTO.Rate = tblBaseItemMetalCostTO.BaseRate;

                                    tblBaseItemMetalCostTO.COrNcId = tempTO.CorNcId;

                                    tempList.Add(tempTO);

                                    TblPurchaseScheduleSummaryTO tempScheduleTO = new TblPurchaseScheduleSummaryTO();
                                    tempScheduleTO.ProdClassId = baseItemTO.ProdClassId;
                                    tempScheduleTO.COrNcId = tempTO.CorNcId;


                                    resultMessage = _iTblPurchaseScheduleSummaryBL.CalculateItemDetails(tempList, tempScheduleTO, conn, tran);
                                    if (resultMessage.MessageType == ResultMessageE.Information)
                                    {
                                        if (k == 0)
                                        {
                                            tblBaseItemMetalCostTO.BaseMetalCostForC = tempList.Sum(p => p.TotalProduct);
                                        }
                                        else
                                        {
                                            tblBaseItemMetalCostTO.BaseMetalCostForNC = tempList.Sum(p => p.TotalProduct);
                                        }

                                        tblBaseItemMetalCostTO.GradeExpressionDtlsTOList.AddRange(tempList[0].GradeExpressionDtlsTOList);
                                    }
                                    else
                                    {
                                        throw new Exception("Error in TblPurchaseScheduleSummaryBL.CalculateItemDetails(tempList,baseItemTO.ProdClassId,conn,tran);");
                                    }

                                    if (tblBaseItemMetalCostTO != null)
                                    {

                                        if (tblBaseItemMetalCostTO.BaseRecovery == 0)
                                        {
                                            throw new Exception("Base Item Reovery zero.");
                                        }

                                        tblBaseItemMetalCostTO.GlobalRatePurchaseId = TblGlobalRatePurchaseTO.IdGlobalRatePurchase;

                                        result = _iTblBaseItemMetalCostBL.InsertTblBaseItemMetalCost(tblBaseItemMetalCostTO, conn, tran);
                                        if (result != 1)
                                        {
                                            tran.Rollback();
                                            resultMessage.DefaultBehaviour("Error While InsertTblBaseItemMetalCost");
                                            return resultMessage;
                                        }
                                        else
                                        {
                                            if (tblBaseItemMetalCostTO.GradeExpressionDtlsTOList != null && tblBaseItemMetalCostTO.GradeExpressionDtlsTOList.Count > 0)
                                            {
                                                tblBaseItemMetalCostTO.GradeExpressionDtlsTOList = tblBaseItemMetalCostTO.GradeExpressionDtlsTOList.Where(w => w.ExpressionDtlsId > 0).ToList();

                                                for (int q = 0; q < tblBaseItemMetalCostTO.GradeExpressionDtlsTOList.Count; q++)
                                                {
                                                    tblBaseItemMetalCostTO.GradeExpressionDtlsTOList[q].GlobleRatePurchaseId = TblGlobalRatePurchaseTO.IdGlobalRatePurchase;
                                                    tblBaseItemMetalCostTO.GradeExpressionDtlsTOList[q].BaseItemMetalCostId = tblBaseItemMetalCostTO.IdBaseItemMetalCost;
                                                    result = _iTblGradeExpressionDtlsBL.InsertTblGradeExpressionDtls(tblBaseItemMetalCostTO.GradeExpressionDtlsTOList[q], conn, tran);
                                                    if (result != 1)
                                                    {
                                                        tran.Rollback();
                                                        resultMessage.DefaultBehaviour("Error While InsertTblGradeExpressionDtls");
                                                        return resultMessage;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        #endregion
                    }

                }

                #endregion
                //added condition by swati if list will be null
                if (tblGlobalRatePurchaseTOList[0].RateBandDeclarationPurchaseTOList != null)
                {
                    #region 2. Prepare SMS List
                    List<TblSmsTO> smsTOList = new List<TblSmsTO>();
                    rateString = rateString.TrimEnd(',');
                    for (int i = 0; i < tblGlobalRatePurchaseTOList[0].RateBandDeclarationPurchaseTOList.Count; i++)
                    {
                        TblRateBandDeclarationPurchaseTO tblRateBandDeclarationPurchaseTO = tblGlobalRatePurchaseTOList[0].RateBandDeclarationPurchaseTOList[i];
                        TblPersonTO person = _iTblPersonBL.SelectAllPersonListByUser(tblRateBandDeclarationPurchaseTO.UserId);
                        if (person != null)
                        {
                            if (!string.IsNullOrEmpty(person.MobileNo))
                            {
                                TblSmsTO smsTO = new TblSmsTO();
                                smsTO.MobileNo = person.MobileNo;
                                smsTO.SourceTxnDesc = "Scrap Rate Declaration";
                                smsTO.SentOn = tblGlobalRatePurchaseTOList[0].CreatedOn;
                                String reasonDesc = tblGlobalRatePurchaseTOList[0].RateReasonDesc;
                                if (tblGlobalRatePurchaseTOList[0].RateReasonDesc == "Other")
                                    reasonDesc = tblGlobalRatePurchaseTOList[0].Comments;
                                if (isRateAlreadyDeclare)
                                    smsTO.SmsTxt = "New Rate is declared for scrap. Rate = " + rateString + " Rs/MT ";//, Reason : " + reasonDesc;
                                else
                                    smsTO.SmsTxt = "Today's Rate is declared for scrap. Rate = " + rateString + " Rs/MT ";//, Reason : " + reasonDesc;

                                smsTOList.Add(smsTO);
                            }
                        }
                    }
                    #endregion




                    #region 3. Send Notifications Via SMS Or Email To All C&F

                    TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                    tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.PURCHASE_NEW_RATE_DECLARED;
                    tblAlertInstanceTO.AlertAction = "PURCHASE_NEW_RATE_DECLARED";
                    rateString = rateString.TrimEnd(',');

                    if (!isRateAlreadyDeclare)
                        tblAlertInstanceTO.AlertComment = "Today's Rate is declared for scrap. Rate = " + rateString + " (Rs/MT)";
                    else
                        tblAlertInstanceTO.AlertComment = "New Rate is declared for scrap. Rate = " + rateString + " (Rs/MT)";

                    tblAlertInstanceTO.EffectiveFromDate = tblGlobalRatePurchaseTOList[0].CreatedOn;
                    tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                    tblAlertInstanceTO.IsActive = 1;
                    tblAlertInstanceTO.SourceDisplayId = "PURCHASE_NEW_RATE_DECLARED";
                    tblAlertInstanceTO.SourceEntityId = tblGlobalRatePurchaseTOList[0].IdGlobalRatePurchase;
                    tblAlertInstanceTO.RaisedBy = tblGlobalRatePurchaseTOList[0].CreatedBy;
                    tblAlertInstanceTO.RaisedOn = tblGlobalRatePurchaseTOList[0].CreatedOn;
                    tblAlertInstanceTO.IsAutoReset = 1;
                    if (smsTOList != null)
                    {
                        tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();
                        tblAlertInstanceTO.SmsTOList = smsTOList;
                    }

                    //Reset Prev alert
                    tblAlertInstanceTO.AlertsToReset = new AlertsToReset();
                    tblAlertInstanceTO.AlertsToReset.AlertDefIdList.Add((int)NotificationConstants.NotificationsE.PURCHASE_NEW_RATE_DECLARED);
                    tblAlertInstanceTO.AlertsToReset.AlertDefIdList.Add((int)NotificationConstants.NotificationsE.PURCHASE_BOOKINGS_CLOSED);

                    //Sanjay [21 sept 2018] Below code is commented and common notification API is called
                    //Notification notify = new Notification();
                    notify.SendNotificationToUsers(tblAlertInstanceTO);
                    //String alertDefIds = (int)NotificationConstants.NotificationsE.PURCHASE_NEW_RATE_DECLARED + "," + (int)NotificationConstants.NotificationsE.PURCHASE_BOOKINGS_CLOSED;
                    //result = BL.TblAlertInstanceBL.ResetAlertInstanceByDef(alertDefIds, conn, tran);
                    //if (result < 0)
                    //{
                    //    tran.Rollback();
                    //    resultMessage.DefaultBehaviour("Error While ResetAlertInstanceByDef");
                    //    return resultMessage;
                    //}

                    //ResultMessage rMessage = BL.TblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                    //if (rMessage.MessageType != ResultMessageE.Information)
                    //{
                    //    tran.Rollback();
                    //    resultMessage.DefaultBehaviour("Error While SaveNewAlertInstance");
                    //    return resultMessage;
                    //}

                    #endregion
                }

                #region 4. Update booking Status As OPEN
                TblPurchaseBookingActionsTO existinBookingActionsTO = _iTblPurchaseBookingActionsBL.SelectLatestBookingActionTO(conn, tran);
                if (existinBookingActionsTO == null || existinBookingActionsTO.BookingStatus == "CLOSE")
                {
                    TblPurchaseBookingActionsTO bookingActionTO = new TblPurchaseBookingActionsTO();
                    bookingActionTO.BookingStatus = "OPEN";
                    bookingActionTO.IsAuto = 1;
                    bookingActionTO.StatusBy = tblGlobalRatePurchaseTOList[0].CreatedBy;
                    bookingActionTO.StatusDate = tblGlobalRatePurchaseTOList[0].CreatedOn;

                    result = _iTblPurchaseBookingActionsBL.InsertTblBookingActions(bookingActionTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("InsertTblBookingActions");
                        return resultMessage;
                    }
                }
                #endregion

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SaveDeclaredRate");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region Updation
        public int UpdateTblRateDeclaration(TblRateBandDeclarationPurchaseTO tblRateBandDeclarationPurchaseTO)
        {
            return _iTblRateBandDeclarationPurchaseDAO.UpdateTblRateDeclaration(tblRateBandDeclarationPurchaseTO);
        }
        #endregion
    }
}
