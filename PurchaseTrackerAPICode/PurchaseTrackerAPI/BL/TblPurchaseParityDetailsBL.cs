using PurchaseTrackerAPI.BL.Interfaces;
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
    public class TblPurchaseParityDetailsBL : ITblPurchaseParityDetailsBL
    {

        #region Selection
        private readonly ITblPurchaseParityDetailsDAO _iTblPurchaseParityDetailsDAO;
        private readonly IConnectionString _iConnectionString;

        public TblPurchaseParityDetailsBL(ITblPurchaseParityDetailsDAO iTblPurchaseParityDetailsDAO, IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
            _iTblPurchaseParityDetailsDAO = iTblPurchaseParityDetailsDAO;
        }
        public  List<TblPurchaseParityDetailsTO> SelectAllTblParityDetailsList(Int32 parityId, Int32 prodSpecId, Int32 stateId, Int32 brandId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                List<TblPurchaseParityDetailsTO> list = null;
                if (parityId == 0)
                    list = _iTblPurchaseParityDetailsDAO.SelectAllLatestParityDetails(stateId, prodSpecId, brandId, conn, tran);
                else
                {
                    list = _iTblPurchaseParityDetailsDAO.SelectAllTblParityDetails(parityId, prodSpecId, conn, tran);
                }

                return list;
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

        public  List<TblPurchaseParityDetailsTO> SelectAllEmptyParityDetailsList(Int32 prodSpecId, Int32 stateId, Int32 brandId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                List<TblPurchaseParityDetailsTO> list = null;
                list = _iTblPurchaseParityDetailsDAO.SelectAllLatestParityDetails(stateId, prodSpecId, brandId, conn, tran);
                return list;
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

        public  List<TblPurchaseParityDetailsTO> SelectAllTblParityDetailsList(String parityIds, int prodSpecId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseParityDetailsDAO.SelectAllTblParityDetails(parityIds, prodSpecId, conn, tran);
        }
        //Prajakta [2018 dec 03 ] Added to get booking parity details
        public  List<TblPurchaseParityDetailsTO> GetBookingItemsParityDtls(Int32 prodItemId, DateTime createdOn)
        {
            return _iTblPurchaseParityDetailsDAO.GetBookingItemsParityDtls(prodItemId, createdOn);
        }
        public  List<TblPurchaseParityDetailsTO> GetBookingItemsParityDtls(string prodItemIds, DateTime createdOn,Int32 stateId,SqlConnection conn = null,SqlTransaction tran = null)
        {
            if(conn != null && tran != null)
                return _iTblPurchaseParityDetailsDAO.GetBookingItemsParityDtls(prodItemIds, createdOn,stateId,conn,tran);
            else
                return _iTblPurchaseParityDetailsDAO.GetBookingItemsParityDtls(prodItemIds, createdOn,stateId);
        }
        public  List<TblPurchaseParityDetailsTO> SelectAllTblParityDetailsList(Int32 parityId, int prodSpecId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseParityDetailsDAO.SelectAllTblParityDetails(parityId, prodSpecId, conn, tran);
        }

        public  int InsertTblParityDetails(TblPurchaseParityDetailsTO tblParityDetailsTO)
        {
            return _iTblPurchaseParityDetailsDAO.InsertTblParityDetails(tblParityDetailsTO);
        }

        public  int SaveProductImgSettings(SaveProductImgTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseParityDetailsDAO.InsertProductImgDetails(tblParityDetailsTO, conn, tran);
        }

        public  int InsertTblParityDetails(TblPurchaseParityDetailsTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseParityDetailsDAO.InsertTblParityDetails(tblParityDetailsTO, conn, tran);
        }

        //public  List<TblParityDetailsTO> SelectAllTblParityDetailsList(Int32 parityId, int prodSpecId, SqlConnection conn, SqlTransaction tran)
        //{
        //    return TblParityDetailsDAO.SelectAllTblParityDetails(parityId, prodSpecId, conn, tran);
        //}

        //public  List<TblParityDetailsTO> SelectAllTblParityDetailsList(String parityIds, int prodSpecId, SqlConnection conn, SqlTransaction tran)
        //{
        //    return TblParityDetailsDAO.SelectAllTblParityDetails(parityIds, prodSpecId, conn, tran);
        //}

        //public  TblParityDetailsTO SelectTblParityDetailsTO(Int32 idParityDtl)
        //{
        //    return TblParityDetailsDAO.SelectTblParityDetails(idParityDtl);
        //}

        //public  List<TblParityDetailsTO> SelectAllParityDetailsListByIds(String parityDtlIds, SqlConnection conn, SqlTransaction tran)
        //{
        //    return TblParityDetailsDAO.SelectAllParityDetailsListByIds(parityDtlIds, conn, tran);
        //}

        #endregion

        //#region Insertion
        //public  int InsertTblParityDetails(TblParityDetailsTO tblParityDetailsTO)
        //{
        //    return TblParityDetailsDAO.InsertTblParityDetails(tblParityDetailsTO);
        //}

        //public  int InsertTblParityDetails(TblParityDetailsTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran)
        //{
        //    return TblParityDetailsDAO.InsertTblParityDetails(tblParityDetailsTO, conn, tran);
        //}

        //#endregion

        //#region Updation
        //public  int UpdateTblParityDetails(TblParityDetailsTO tblParityDetailsTO)
        //{
        //    return TblParityDetailsDAO.UpdateTblParityDetails(tblParityDetailsTO);
        //}

        //public  int UpdateTblParityDetails(TblParityDetailsTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran)
        //{
        //    return TblParityDetailsDAO.UpdateTblParityDetails(tblParityDetailsTO, conn, tran);
        //}

        //#endregion

        //#region Deletion
        //public  int DeleteTblParityDetails(Int32 idParityDtl)
        //{
        //    return TblParityDetailsDAO.DeleteTblParityDetails(idParityDtl);
        //}

        //public  int DeleteTblParityDetails(Int32 idParityDtl, SqlConnection conn, SqlTransaction tran)
        //{
        //    return TblParityDetailsDAO.DeleteTblParityDetails(idParityDtl, conn, tran);
        //}

        //#endregion
    }
}
