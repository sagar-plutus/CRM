using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblGradeExpressionDtlsBL : ITblGradeExpressionDtlsBL
    {
        private readonly ITblGradeExpressionDtlsDAO _itblGradeExpressionDtlsDAO;
        private readonly ITblBaseItemMetalCostBL _iTblBaseItemMetalCostBL;
        public TblGradeExpressionDtlsBL(ITblGradeExpressionDtlsDAO iTblGradeExpressionDtlsDAO, ITblBaseItemMetalCostBL iTblBaseItemMetalCostBL)
        {
            _itblGradeExpressionDtlsDAO = iTblGradeExpressionDtlsDAO;
            _iTblBaseItemMetalCostBL = iTblBaseItemMetalCostBL;
        }
        #region Selection
        public List<TblGradeExpressionDtlsTO> SelectAllTblGradeExpressionDtls()
        {
            return _itblGradeExpressionDtlsDAO.SelectAllTblGradeExpressionDtls();
        }

        public List<TblGradeExpressionDtlsTO> SelectAllTblGradeExpressionDtlsList()
        {
            return _itblGradeExpressionDtlsDAO.SelectAllTblGradeExpressionDtls();
        }
        public List<TblGradeExpressionDtlsTO> SelectAllTblGradeExpDtlsByGlobalRateId(string globleRatePurchaseIds, SqlConnection conn, SqlTransaction tran)
        {
            return _itblGradeExpressionDtlsDAO.SelectAllTblGradeExpDtlsByGlobalRateId(globleRatePurchaseIds, conn, tran);
        }
        public List<TblGradeExpressionDtlsTO> SelectGradeExpressionDtls(string enquiryDetailsId, SqlConnection conn, SqlTransaction tran)
        {
            return _itblGradeExpressionDtlsDAO.SelectGradeExpressionDtls(enquiryDetailsId, conn, tran);
        }
        public List<TblGradeExpressionDtlsTO> SelectGradeExpressionDtls(string enquiryDetailsId)
        {
            return _itblGradeExpressionDtlsDAO.SelectGradeExpressionDtls(enquiryDetailsId);
        }

        public List<TblGradeExpressionDtlsTO> SelectGradeExpressionDtlsByScheduleId(string scheduleDtlsId)
        {
            return _itblGradeExpressionDtlsDAO.SelectGradeExpressionDtlsByScheduleId(scheduleDtlsId);
        }

        public List<TblGradeExpressionDtlsTO> SelectGradeExpreDtlsByBaseMetalId(Int32 baseItemMetalCostId)
        {
            return _itblGradeExpressionDtlsDAO.SelectGradeExpreDtlsByBaseMetalId(baseItemMetalCostId);
        }

        public TblBaseItemMetalCostTO GetBaseItemGradeExpreDtls(Int32 globalRatePurchaseId, Int32 cOrNcId)
        {
            TblBaseItemMetalCostTO tblBaseItemMetalCostTO = null;
            List<TblBaseItemMetalCostTO> tblBaseItemMetalCostTOList = _iTblBaseItemMetalCostBL.SelectBaseItemMetalCostByGlobalRateId(globalRatePurchaseId, cOrNcId);
            if (tblBaseItemMetalCostTOList != null && tblBaseItemMetalCostTOList.Count == 1)
            {
                tblBaseItemMetalCostTO = tblBaseItemMetalCostTOList[0];
                tblBaseItemMetalCostTO.GradeExpressionDtlsTOList = SelectGradeExpreDtlsByBaseMetalId(tblBaseItemMetalCostTO.IdBaseItemMetalCost);
            }
            return tblBaseItemMetalCostTO;
        }



        // public  void SelectGradeExpDtlsList(List<TblPurchaseVehicleDetailsTO> scheduleItemDtlsList)
        // {
        //     if (scheduleItemDtlsList != null && scheduleItemDtlsList.Count > 0)
        //     {
        //         string scheduleDtlsIds = string.Join(", ", scheduleItemDtlsList.Select(i => i.IdVehiclePurchase));
        //         List<TblGradeExpressionDtlsTO> tblGradeExpressionDtlsTOList = _itblGradeExpressionDtlsDAO.SelectGradeExpressionDtlsByScheduleId(scheduleDtlsIds);

        //         if (tblGradeExpressionDtlsTOList != null && tblGradeExpressionDtlsTOList.Count > 0)
        //         {
        //             for (int i = 0; i < scheduleItemDtlsList.Count; i++)
        //             {
        //                 TblPurchaseVehicleDetailsTO vehicleDtlsTO = scheduleItemDtlsList[i];

        //                 List<TblGradeExpressionDtlsTO> tempGradeExpList = tblGradeExpressionDtlsTOList.Where(a => a.PurchaseScheduleDtlsId == vehicleDtlsTO.IdVehiclePurchase).ToList();
        //                 if (tempGradeExpList != null && tempGradeExpList.Count > 0)
        //                 {
        //                     vehicleDtlsTO.GradeExpressionDtlsTOList = tempGradeExpList;
        //                 }
        //             }
        //         }
        //     }
        // }
        public void SelectGradeExpDtlsList(List<TblPurchaseVehicleDetailsTO> scheduleItemDtlsList, SqlConnection conn = null, SqlTransaction tran = null)
        {
            if (scheduleItemDtlsList != null && scheduleItemDtlsList.Count > 0)
            {
                string scheduleDtlsIds = string.Join(", ", scheduleItemDtlsList.Select(i => i.IdVehiclePurchase));
                List<TblGradeExpressionDtlsTO> tblGradeExpressionDtlsTOList = null;

                if (conn != null && tran != null)
                {
                    tblGradeExpressionDtlsTOList = new List<TblGradeExpressionDtlsTO>();
                    tblGradeExpressionDtlsTOList = _itblGradeExpressionDtlsDAO.SelectGradeExpressionDtlsByScheduleId(scheduleDtlsIds, conn, tran);
                }
                else
                {
                    tblGradeExpressionDtlsTOList = new List<TblGradeExpressionDtlsTO>();
                    tblGradeExpressionDtlsTOList = _itblGradeExpressionDtlsDAO.SelectGradeExpressionDtlsByScheduleId(scheduleDtlsIds);
                }

                if (tblGradeExpressionDtlsTOList != null && tblGradeExpressionDtlsTOList.Count > 0)
                {
                    for (int i = 0; i < scheduleItemDtlsList.Count; i++)
                    {
                        TblPurchaseVehicleDetailsTO vehicleDtlsTO = scheduleItemDtlsList[i];

                        List<TblGradeExpressionDtlsTO> tempGradeExpList = tblGradeExpressionDtlsTOList.Where(a => a.PurchaseScheduleDtlsId == vehicleDtlsTO.IdVehiclePurchase).ToList();
                        if (tempGradeExpList != null && tempGradeExpList.Count > 0)
                        {
                            vehicleDtlsTO.GradeExpressionDtlsTOList = tempGradeExpList;
                        }
                    }
                }
            }
        }

        // public  void SelectGradeExpDtlsList(List<TblPurchaseEnquiryDetailsTO> enquiryItemDtlsList)
        // {
        //     if (enquiryItemDtlsList != null && enquiryItemDtlsList.Count > 0)
        //     {
        //         string scheduleDtlsIds = string.Join(", ", enquiryItemDtlsList.Select(i => i.IdPurchaseEnquiryDetails));
        //         List<TblGradeExpressionDtlsTO> tblGradeExpressionDtlsTOList = _itblGradeExpressionDtlsDAO.SelectGradeExpressionDtls(scheduleDtlsIds);

        //         if (tblGradeExpressionDtlsTOList != null && tblGradeExpressionDtlsTOList.Count > 0)
        //         {
        //             for (int i = 0; i < enquiryItemDtlsList.Count; i++)
        //             {
        //                 TblPurchaseEnquiryDetailsTO vehicleDtlsTO = enquiryItemDtlsList[i];

        //                 List<TblGradeExpressionDtlsTO> tempGradeExpList = tblGradeExpressionDtlsTOList.Where(a => a.PurchaseEnquiryDtlsId == vehicleDtlsTO.IdPurchaseEnquiryDetails).ToList();
        //                 if (tempGradeExpList != null && tempGradeExpList.Count > 0)
        //                 {
        //                     vehicleDtlsTO.GradeExpressionDtlsTOList = tempGradeExpList;
        //                 }
        //             }
        //         }
        //     }
        // }
        public void SelectGradeExpDtlsList(List<TblPurchaseEnquiryDetailsTO> enquiryItemDtlsList, SqlConnection conn = null, SqlTransaction tran = null)
        {
            if (enquiryItemDtlsList != null && enquiryItemDtlsList.Count > 0)
            {
                string enquiryItemDtlsIds = string.Join(", ", enquiryItemDtlsList.Select(i => i.IdPurchaseEnquiryDetails));
                List<TblGradeExpressionDtlsTO> tblGradeExpressionDtlsTOList = null;

                if (conn != null && tran != null)
                {
                    tblGradeExpressionDtlsTOList = new List<TblGradeExpressionDtlsTO>();
                    tblGradeExpressionDtlsTOList = _itblGradeExpressionDtlsDAO.SelectGradeExpressionDtls(enquiryItemDtlsIds, conn, tran);
                }
                else
                {
                    tblGradeExpressionDtlsTOList = _itblGradeExpressionDtlsDAO.SelectGradeExpressionDtls(enquiryItemDtlsIds);
                }

                if (tblGradeExpressionDtlsTOList != null && tblGradeExpressionDtlsTOList.Count > 0)
                {
                    for (int i = 0; i < enquiryItemDtlsList.Count; i++)
                    {
                        TblPurchaseEnquiryDetailsTO vehicleDtlsTO = enquiryItemDtlsList[i];

                        List<TblGradeExpressionDtlsTO> tempGradeExpList = tblGradeExpressionDtlsTOList.Where(a => a.PurchaseEnquiryDtlsId == vehicleDtlsTO.IdPurchaseEnquiryDetails).ToList();
                        if (tempGradeExpList != null && tempGradeExpList.Count > 0)
                        {
                            vehicleDtlsTO.GradeExpressionDtlsTOList = tempGradeExpList;
                        }
                    }
                }
            }
        }


        public List<TblGradeExpressionDtlsTO> SelectGradeExpressionDtlsByScheduleId(string scheduleDtlsId, SqlConnection conn, SqlTransaction tran)
        {
            return _itblGradeExpressionDtlsDAO.SelectGradeExpressionDtlsByScheduleId(scheduleDtlsId, conn, tran);
        }
        public TblGradeExpressionDtlsTO SelectTblGradeExpressionDtlsTO(Int32 idGradeExpressionDtls)
        {
            List<TblGradeExpressionDtlsTO> tblGradeExpressionDtlsTOList = _itblGradeExpressionDtlsDAO.SelectTblGradeExpressionDtls(idGradeExpressionDtls);
            if (tblGradeExpressionDtlsTOList != null && tblGradeExpressionDtlsTOList.Count == 1)
                return tblGradeExpressionDtlsTOList[0];
            else
                return null;
        }



        #endregion

        #region Insertion
        public int InsertTblGradeExpressionDtls(TblGradeExpressionDtlsTO tblGradeExpressionDtlsTO)
        {
            return _itblGradeExpressionDtlsDAO.InsertTblGradeExpressionDtls(tblGradeExpressionDtlsTO);
        }

        public int InsertTblGradeExpressionDtls(TblGradeExpressionDtlsTO tblGradeExpressionDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _itblGradeExpressionDtlsDAO.InsertTblGradeExpressionDtls(tblGradeExpressionDtlsTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblGradeExpressionDtls(TblGradeExpressionDtlsTO tblGradeExpressionDtlsTO)
        {
            return _itblGradeExpressionDtlsDAO.UpdateTblGradeExpressionDtls(tblGradeExpressionDtlsTO);
        }

        public int UpdateTblGradeExpressionDtls(TblGradeExpressionDtlsTO tblGradeExpressionDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _itblGradeExpressionDtlsDAO.UpdateTblGradeExpressionDtls(tblGradeExpressionDtlsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblGradeExpressionDtls(Int32 idGradeExpressionDtls)
        {
            return _itblGradeExpressionDtlsDAO.DeleteTblGradeExpressionDtls(idGradeExpressionDtls);
        }

        public int DeleteTblGradeExpressionDtls(Int32 idGradeExpressionDtls, SqlConnection conn, SqlTransaction tran)
        {
            return _itblGradeExpressionDtlsDAO.DeleteTblGradeExpressionDtls(idGradeExpressionDtls, conn, tran);
        }

        public int DeleteAllGradeExpDtlsAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            return _itblGradeExpressionDtlsDAO.DeleteAllGradeExpDtlsAgainstVehSchedule(purchaseScheduleId, conn, tran);
        }


        #endregion

    }
}
