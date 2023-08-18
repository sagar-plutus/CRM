using PurchaseTrackerAPI.BL;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TO;

namespace PurchaseTrackerAPI.BL
{
    public class TblPurchaseVehicleStageCntBL : ITblPurchaseVehicleStageCntBL
    {
        private readonly ITblPurchaseVehicleStageCntDAO _iTblPurchaseVehicleStageCntDAO;

        public TblPurchaseVehicleStageCntBL(ITblPurchaseVehicleStageCntDAO iTblPurchaseVehicleStageCntDAO)
        {
            _iTblPurchaseVehicleStageCntDAO = iTblPurchaseVehicleStageCntDAO;
        }

        #region Selection
        public List<TblPurchaseVehicleStageCntTO> SelectAllTblPurchaseVehicleStageCnt()
        {
            return _iTblPurchaseVehicleStageCntDAO.SelectAllTblPurchaseVehicleStageCnt();
        }

        public List<TblPurchaseVehicleStageCntTO> SelectAllTblPurchaseVehicleStageCntList()
        {
            return _iTblPurchaseVehicleStageCntDAO.SelectAllTblPurchaseVehicleStageCnt();
        }

        public TblPurchaseVehicleStageCntTO SelectTblPurchaseVehicleStageCntTO(Int32 idPurchaseVehicleStageCnt)
        {
            List<TblPurchaseVehicleStageCntTO> tblPurchaseVehicleStageCntTOList = _iTblPurchaseVehicleStageCntDAO.SelectTblPurchaseVehicleStageCnt(idPurchaseVehicleStageCnt);
            if (tblPurchaseVehicleStageCntTOList != null && tblPurchaseVehicleStageCntTOList.Count == 1)
                return tblPurchaseVehicleStageCntTOList[0];
            else
                return null;
        }

        public TblPurchaseVehicleStageCntTO SelectPurchaseVehicleStageCntByRootScheduleId(Int32 rootScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            List<TblPurchaseVehicleStageCntTO> tblPurchaseVehicleStageCntTOList = _iTblPurchaseVehicleStageCntDAO.SelectPurchaseVehicleStageCntByRootScheduleId(rootScheduleId, conn, tran);
            if (tblPurchaseVehicleStageCntTOList != null && tblPurchaseVehicleStageCntTOList.Count == 1)
                return tblPurchaseVehicleStageCntTOList[0];
            else
                return null;
        }


        #endregion

        #region Insertion
        public int InsertTblPurchaseVehicleStageCnt(TblPurchaseVehicleStageCntTO tblPurchaseVehicleStageCntTO)
        {
            return _iTblPurchaseVehicleStageCntDAO.InsertTblPurchaseVehicleStageCnt(tblPurchaseVehicleStageCntTO);
        }

        public int InsertTblPurchaseVehicleStageCnt(TblPurchaseVehicleStageCntTO tblPurchaseVehicleStageCntTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehicleStageCntDAO.InsertTblPurchaseVehicleStageCnt(tblPurchaseVehicleStageCntTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblPurchaseVehicleStageCnt(TblPurchaseVehicleStageCntTO tblPurchaseVehicleStageCntTO)
        {
            return _iTblPurchaseVehicleStageCntDAO.UpdateTblPurchaseVehicleStageCnt(tblPurchaseVehicleStageCntTO);
        }

        public int UpdateTblPurchaseVehicleStageCntForWeighing(Int32 rootScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehicleStageCntDAO.UpdateTblPurchaseVehicleStageCntForWeighing(rootScheduleId, conn, tran);
        }
        public int UpdateTblPurchaseVehicleStageCnt(TblPurchaseVehicleStageCntTO tblPurchaseVehicleStageCntTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehicleStageCntDAO.UpdateTblPurchaseVehicleStageCnt(tblPurchaseVehicleStageCntTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblPurchaseVehicleStageCnt(Int32 idPurchaseVehicleStageCnt)
        {
            return _iTblPurchaseVehicleStageCntDAO.DeleteTblPurchaseVehicleStageCnt(idPurchaseVehicleStageCnt);
        }

        public int DeleteTblPurchaseVehicleStageCnt(Int32 idPurchaseVehicleStageCnt, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehicleStageCntDAO.DeleteTblPurchaseVehicleStageCnt(idPurchaseVehicleStageCnt, conn, tran);
        }

        #endregion

        //Prajakta[2019-03-19] Added to insert or update the weighing stage count for respective phases
        public ResultMessage InsertOrUpdateVehicleWtStageCount(TblPurchaseScheduleSummaryTO scheduleSummaryTO, TblPurchaseWeighingStageSummaryTO weighingStageSummaryTO, TblPurchaseWeighingStageSummaryTO weighingStageSummaryTOForRec, TblPurchaseUnloadingDtlTO unloadingDtlTO, TblPurchaseGradingDtlsTO gradingDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                int res = 0;
                TblPurchaseVehicleStageCntTO tblPurchaseVehicleStageCntTO = SelectPurchaseVehicleStageCntByRootScheduleId(scheduleSummaryTO.ActualRootScheduleId, conn, tran);
                if (tblPurchaseVehicleStageCntTO != null)
                {
                    // if (weighingStageSummaryTO != null)
                    // {
                    //     if (weighingStageSummaryTO.IsRecConfirm > 0)
                    //     {
                    //         tblPurchaseVehicleStageCntTO.RecoveryCompCnt = weighingStageSummaryTO.WeightStageId;
                    //     }
                    //     else
                    //     {
                    //         tblPurchaseVehicleStageCntTO.WtStageCompCnt = weighingStageSummaryTO.WeightStageId;
                    //     }
                    // }

                    if (weighingStageSummaryTO != null)
                    {
                        tblPurchaseVehicleStageCntTO.WtStageCompCnt = weighingStageSummaryTO.WeightStageId;
                    }

                    if (weighingStageSummaryTOForRec != null)
                    {
                        tblPurchaseVehicleStageCntTO.RecoveryCompCnt = weighingStageSummaryTOForRec.WeightStageId;
                    }

                    if (unloadingDtlTO != null)
                    {
                        tblPurchaseVehicleStageCntTO.UnloadingCompCnt = unloadingDtlTO.WeighingStageId;
                    }
                    if (gradingDtlsTO != null)
                    {
                        tblPurchaseVehicleStageCntTO.GradingCompCnt = gradingDtlsTO.WeighingStageId;
                    }
                    res = UpdateTblPurchaseVehicleStageCnt(tblPurchaseVehicleStageCntTO, conn, tran);
                    //Update count
                }
                else
                {
                    //Insert count

                    if (weighingStageSummaryTO != null)
                    {
                        tblPurchaseVehicleStageCntTO = new TblPurchaseVehicleStageCntTO();
                        tblPurchaseVehicleStageCntTO.RootScheduleId = scheduleSummaryTO.ActualRootScheduleId;
                        tblPurchaseVehicleStageCntTO.WtStageCompCnt = weighingStageSummaryTO.WeightStageId;
                        res = InsertTblPurchaseVehicleStageCnt(tblPurchaseVehicleStageCntTO, conn, tran);
                    }
                }
                if (res > 0)
                {
                    resultMessage.DefaultSuccessBehaviour();
                }
                else
                {
                    resultMessage.Result = 0;
                    resultMessage.Text = "error while insert update TblPurchaseVehicleStageCntTO";
                    resultMessage.DisplayMessage = "error while insert update TblPurchaseVehicleStageCntTO";
                }
                return resultMessage;
            }
            catch (System.Exception ex)
            {

                resultMessage.DefaultExceptionBehaviour(ex, "Error in InsertOrUpdateVehicleWtStageCount(TblPurchaseScheduleSummaryTO scheduleSummaryTO,TblPurchaseWeighingStageSummaryTO weighingStageSummaryTO,TblPurchaseUnloadingDtlTO unloadingDtlTO,TblPurchaseGradingDtlsTO gradingDtlsTO)");
                return resultMessage;
            }
        }

        public int DeleteAllStageCntAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehicleStageCntDAO.DeleteAllStageCntAgainstVehSchedule(purchaseScheduleId, conn, tran);
        }


    }
}
