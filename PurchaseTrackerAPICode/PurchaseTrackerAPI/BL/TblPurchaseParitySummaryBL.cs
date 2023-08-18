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
    public class TblPurchaseParitySummaryBL : ITblPurchaseParitySummaryBL
    {
        private readonly ITblPurchaseParityDetailsBL _iTblPurchaseParityDetailsBL;
        private readonly ITblPurchaseParitySummaryDAO _iTblPurchaseParitySummaryDAO;
        private readonly IConnectionString _iConnectionString;
        public TblPurchaseParitySummaryBL(IConnectionString iConnectionString, ITblPurchaseParitySummaryDAO iTblPurchaseParitySummaryDAO, ITblPurchaseParityDetailsBL iTblPurchaseParityDetailsBL)
        {
            _iConnectionString = iConnectionString;
            _iTblPurchaseParityDetailsBL = iTblPurchaseParityDetailsBL;
            _iTblPurchaseParitySummaryDAO = iTblPurchaseParitySummaryDAO;
        }
        #region Selection

        //public  List<TblParitySummaryTO> SelectAllTblParitySummaryList()
        //{
        //    return TblParitySummaryDAO.SelectAllTblParitySummary();
        //}

        //public  TblParitySummaryTO SelectTblParitySummaryTO(Int32 idParity, SqlConnection conn, SqlTransaction tran)
        //{
        //    return TblParitySummaryDAO.SelectTblParitySummary(idParity, conn, tran);
        //}

        //public  TblParitySummaryTO SelectParitySummaryTOFromParityDtlId(Int32 parityDtlId, SqlConnection conn, SqlTransaction tran)
        //{
        //    return TblParitySummaryDAO.SelectParitySummaryFromParityDtlId(parityDtlId, conn, tran);
        //}

        public  TblPurchaseParitySummaryTO SelectStatesActiveParitySummaryTO(Int32 stateId, Int32 brandId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblPurchaseParitySummaryDAO.SelectStatesActiveParitySummary(stateId, brandId, conn, tran);
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

        public  TblPurchaseParitySummaryTO SelectStatesActiveParitySummaryTO(Int32 stateId, Int32 brandId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseParitySummaryDAO.SelectStatesActiveParitySummary(stateId, brandId, conn, tran);
        }

        public  List<TblPurchaseParitySummaryTO> SelectAllPurchaseCompetitorMaterialList(Int32 organizationId, DateTime fromDate, DateTime toDate)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblPurchaseParitySummaryDAO.SelectAllActivePurchaseCompetitorMaterialList(organizationId, fromDate, toDate, conn, tran);
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

        public  List<TblPurchaseParitySummaryTO> SelectAllMaterialReasonsList(Int32 stateId)
        {
            return _iTblPurchaseParitySummaryDAO.SelectAllMaterialReasonsList(stateId);
        }

        ///// <summary>
        ///// Sanjay [2017-04-21] To Get Active Parity Id
        ///// </summary>
        ///// <param name="conn"></param>
        ///// <param name="tran"></param>
        ///// <returns></returns>
        //public  List<TblParitySummaryTO> SelectActiveParitySummaryTOList(int dealerId, SqlConnection conn, SqlTransaction tran)
        //{
        //    return TblParitySummaryDAO.SelectActiveParitySummaryTOList(dealerId, conn, tran);
        //}

        #endregion

        #region Insertion

        public  ResultMessage SaveParitySettings(TblPurchaseParitySummaryTO tblParitySummaryTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                #region 1. Deactivate All Prev Parity Summary Details
                result = _iTblPurchaseParitySummaryDAO.DeactivateAllParitySummary(tblParitySummaryTO.StateId, tblParitySummaryTO.ProdClassId, conn, tran);
                if (result < 0)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "Error While Deactivating Prev Parity Summary Details";
                    return resultMessage;
                }
                #endregion

                #region 2. Save New Parity Summary

                result = InsertTblParitySummary(tblParitySummaryTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "Error While Inserting New Parity Summary Details";
                    return resultMessage;
                }

                #endregion

                #region 3. Save Parity Details

                if (tblParitySummaryTO.ParityDetailList == null || tblParitySummaryTO.ParityDetailList.Count == 0)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "ParityDetailList Found NULL";
                    return resultMessage;
                }

                for (int i = 0; i < tblParitySummaryTO.ParityDetailList.Count; i++)
                {
                    tblParitySummaryTO.ParityDetailList[i].ParityId = tblParitySummaryTO.IdParity;
                    tblParitySummaryTO.ParityDetailList[i].CreatedBy = tblParitySummaryTO.CreatedBy;
                    tblParitySummaryTO.ParityDetailList[i].CreatedOn = tblParitySummaryTO.CreatedOn;
                    result = _iTblPurchaseParityDetailsBL.InsertTblParityDetails(tblParitySummaryTO.ParityDetailList[i], conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour();
                        resultMessage.Text = "Error While Inserting New Parity Sizewise Details";
                        return resultMessage;
                    }
                }

                #endregion

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Result = 1;
                resultMessage.Text = "Parity Settings Updated Successfully.";
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.Text = "Exception Error While Record Save in BL : SaveParitySettings";
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }


        public  ResultMessage SaveProductImgSettings(SaveProductImgTO saveProductImgTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                #region Save Product Details

                result = _iTblPurchaseParityDetailsBL.SaveProductImgSettings(saveProductImgTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "Error While Inserting New Parity Sizewise Details";
                    return resultMessage;
                }

                #endregion

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Result = 1;
                resultMessage.Text = "Parity Settings Updated Successfully.";
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.Text = "Exception Error While Record Save in BL : SaveParitySettings";
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }


        public  int InsertTblParitySummary(TblPurchaseParitySummaryTO tblParitySummaryTO)
        {
            return _iTblPurchaseParitySummaryDAO.InsertTblParitySummary(tblParitySummaryTO);
        }

        public  int InsertTblParitySummary(TblPurchaseParitySummaryTO tblParitySummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseParitySummaryDAO.InsertTblParitySummary(tblParitySummaryTO, conn, tran);
        }

        #endregion

    }
}
