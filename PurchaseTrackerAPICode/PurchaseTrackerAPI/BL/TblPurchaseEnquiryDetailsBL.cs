using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.BL
{
    public class TblPurchaseEnquiryDetailsBL : ITblPurchaseEnquiryDetailsBL
    {

        private readonly ITblGradeExpressionDtlsBL _iTblGradeExpressionDtlsBL;
        private readonly ITblPurchaseEnquiryDetailsDAO _iTblPurchaseEnquiryDetailsDAO;
        private readonly ITblPurchaseVehicleDetailsBL _iTblPurchaseVehicleDetailsBL;
        public TblPurchaseEnquiryDetailsBL(ITblPurchaseEnquiryDetailsDAO iTblPurchaseEnquiryDetailsDAO, ITblGradeExpressionDtlsBL iTblGradeExpressionDtlsBL, ITblPurchaseVehicleDetailsBL iTblPurchaseVehicleDetailsBL)
        {
            _iTblGradeExpressionDtlsBL = iTblGradeExpressionDtlsBL;
            _iTblPurchaseEnquiryDetailsDAO = iTblPurchaseEnquiryDetailsDAO;
            _iTblPurchaseVehicleDetailsBL = iTblPurchaseVehicleDetailsBL;
        }

        #region Selection

        public List<TblPurchaseEnquiryDetailsTO> SelectAllTblEnquiryDetailsListByEnquiryId(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran)
        {
            List<TblPurchaseEnquiryDetailsTO> tblPurchaseEnquiryDetailsTOList = _iTblPurchaseEnquiryDetailsDAO.SelectAllTblEnquiryDetailsListByEnquiryId(purchaseEnquiryId, conn, tran);
            if (tblPurchaseEnquiryDetailsTOList != null && tblPurchaseEnquiryDetailsTOList.Count > 0)
            {
                // for (int i = 0; i < tblPurchaseEnquiryDetailsTOList.Count; i++)
                // {
                //     tblPurchaseEnquiryDetailsTOList[i].GradeExpressionDtlsTOList = BL.TblGradeExpressionDtlsBL.SelectGradeExpressionDtls(tblPurchaseEnquiryDetailsTOList[i].IdPurchaseEnquiryDetails.ToString());
                //     if (tblPurchaseEnquiryDetailsTOList[i].GradeExpressionDtlsTOList == null)
                //         tblPurchaseEnquiryDetailsTOList[i].GradeExpressionDtlsTOList = new List<TblGradeExpressionDtlsTO>();

                // }

                _iTblGradeExpressionDtlsBL.SelectGradeExpDtlsList(tblPurchaseEnquiryDetailsTOList, conn, tran);
            }
            return tblPurchaseEnquiryDetailsTOList;
        }

        public List<TblPurchaseEnquiryDetailsTO> SelectAllTblEnquiryDetailsList(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnquiryDetailsDAO.SelectAllTblEnquiryDetailsList(purchaseEnquiryId, conn, tran);
        }

        public List<TblPurchaseEnquiryDetailsTO> SelectTblEnquiryDetailsList(Int32 purchaseEnquiryId,SqlConnection conn = null,SqlTransaction tran = null)
        {
            if(conn != null && tran != null)
                return _iTblPurchaseEnquiryDetailsDAO.SelectTblEnquiryDetailsList(purchaseEnquiryId,conn,tran);
            else
                return _iTblPurchaseEnquiryDetailsDAO.SelectTblEnquiryDetailsList(purchaseEnquiryId);
        }

        #endregion

        #region Insertion
        public int InsertTblPurchaseEnquiryDetails(TblPurchaseEnquiryDetailsTO tblPurchaseEnquiryDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnquiryDetailsDAO.InsertTblPurchaseEnquiryDetails(tblPurchaseEnquiryDetailsTO, conn, tran);
        }
        #endregion

        #region Updation
        public int UpdateTblPurchaseEnquiryDetails(TblPurchaseEnquiryDetailsTO tblPurchaseEnquiryDetailsTO)
        {
            return _iTblPurchaseEnquiryDetailsDAO.UpdateTblPurchaseEnquiryDetails(tblPurchaseEnquiryDetailsTO);
        }

        public int UpdateTblPurchaseEnquiryDetails(TblPurchaseEnquiryDetailsTO tblPurchaseEnquiryDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnquiryDetailsDAO.UpdateTblPurchaseEnquiryDetails(tblPurchaseEnquiryDetailsTO, conn, tran);
        }

        public int UpdateEnquiryItemPendingQty(TblPurchaseEnquiryDetailsTO tblPurchaseEnquiryDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnquiryDetailsDAO.UpdateEnquiryItemPendingQty(tblPurchaseEnquiryDetailsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblPurchaseEnquiryDetails(Int32 idPurchaseEnquiryDetails, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnquiryDetailsDAO.DeleteTblPurchaseEnquiryDetails(idPurchaseEnquiryDetails, conn, tran);
        }
        #endregion


        public ResultMessage UpdateEnquiryItemsPendingQty(TblPurchaseScheduleSummaryTO scheduleTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            Boolean isUpdateEnquiryDtls = false;
            Int32 result = 0;
            try
            {
                if (scheduleTO == null)
                {
                    throw new Exception("scheduleTO == null");
                }

                if (scheduleTO.PurchaseScheduleSummaryDetailsTOList == null || scheduleTO.PurchaseScheduleSummaryDetailsTOList.Count == 0)
                {
                    //throw new Exception("scheduleTO.PurchaseScheduleSummaryDetailsTOList == null");
                    scheduleTO.PurchaseScheduleSummaryDetailsTOList = new List<TblPurchaseVehicleDetailsTO>();
                    scheduleTO.PurchaseScheduleSummaryDetailsTOList = _iTblPurchaseVehicleDetailsBL.SelectAllTblPurchaseVehicleDetailsList(scheduleTO.IdPurchaseScheduleSummary, false, conn, tran);
                    //Prajakta[2019-11-12] Commented to save vehicle schedule with no item details
                    //if (scheduleTO.PurchaseScheduleSummaryDetailsTOList == null || scheduleTO.PurchaseScheduleSummaryDetailsTOList.Count == 0)
                    //{
                    //    throw new Exception("scheduleTO.PurchaseScheduleSummaryDetailsTOList == null");
                    //}
                }
                //Get enquiry details
                List<TblPurchaseEnquiryDetailsTO> tblPurchaseEnquiryDetailsTOList = SelectAllTblEnquiryDetailsListByEnquiryId(scheduleTO.PurchaseEnquiryId, conn, tran);
                // if (tblPurchaseEnquiryDetailsTOList == null || tblPurchaseEnquiryDetailsTOList.Count == 0)
                // {
                //     throw new Exception("tblPurchaseEnquiryDetailsTOList == null");
                // }

                if (tblPurchaseEnquiryDetailsTOList != null && tblPurchaseEnquiryDetailsTOList.Count > 0)
                {
                    if(scheduleTO.PurchaseScheduleSummaryDetailsTOList != null && scheduleTO.PurchaseScheduleSummaryDetailsTOList.Count > 0)
                    {
                        for (int i = 0; i < scheduleTO.PurchaseScheduleSummaryDetailsTOList.Count; i++)
                        {

                            TblPurchaseVehicleDetailsTO scheduleItemDtlsTO = scheduleTO.PurchaseScheduleSummaryDetailsTOList[i];

                            var res = tblPurchaseEnquiryDetailsTOList.Where(a => a.ProdItemId == scheduleItemDtlsTO.ProdItemId).FirstOrDefault();
                            if (res != null)
                            {
                                if (scheduleTO.StatusId == (Int32)Constants.TranStatusE.New)
                                {
                                    isUpdateEnquiryDtls = true;
                                    scheduleItemDtlsTO.PurchaseEnqDtlsId = res.IdPurchaseEnquiryDetails;
                                    res.PendingQty = res.PendingQty + scheduleItemDtlsTO.PreviousQty;
                                    res.PendingQty = res.PendingQty - scheduleItemDtlsTO.Qty;
                                    // if (res.PendingQty < 0)
                                    // {
                                    //     res.PendingQty = 0;
                                    // }
                                }

                                if (scheduleTO.StatusId == (Int32)Constants.TranStatusE.DELETE_VEHICLE || scheduleTO.StatusId == (Int32)Constants.TranStatusE.VEHICLE_REJECTED_AFTER_WEIGHING ||
                                    scheduleTO.StatusId == (Int32)Constants.TranStatusE.VEHICLE_REJECTED_BEFORE_WEIGHING
                                    || scheduleTO.StatusId == (Int32)Constants.TranStatusE.VEHICLE_REJECTED_AFTER_GROSS_WEIGHT)
                                {
                                    isUpdateEnquiryDtls = true;

                                    if (scheduleItemDtlsTO.Qty > res.Qty)
                                    {
                                        res.PendingQty = res.Qty;
                                    }
                                    else
                                    {
                                        res.PendingQty = res.PendingQty + scheduleItemDtlsTO.Qty;
                                    }
                                }
                            }
                        }
                    }
         

                }

                //Update pendingQty for enquiry item details
                if (isUpdateEnquiryDtls)
                {
                    for (int k = 0; k < tblPurchaseEnquiryDetailsTOList.Count; k++)
                    {
                        result = UpdateEnquiryItemPendingQty(tblPurchaseEnquiryDetailsTOList[k], conn, tran);
                        if (result == -1)
                        {
                            throw new Exception("Error in UpdateEnquiryItemPendingQty(tblPurchaseEnquiryDetailsTOList[k],conn,tran);");
                        }
                    }
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (System.Exception ex)
            {

                resultMessage.DefaultExceptionBehaviour(ex, "Error in UpdateEnquiryItemsPendingQty(TblPurchaseScheduleSummaryTO scheduleTO,SqlConnection conn,SqlTransaction tran)");
                return resultMessage;
            }

        }

        
        public  int DeleteAllGradeDetailsForEnquiry(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnquiryDetailsDAO.DeleteAllGradeDetailsForEnquiry(purchaseEnquiryId, conn, tran);
        }

    }
}
