using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.StaticStuff;
namespace PurchaseTrackerAPI.BL
{
    public class TblProductItemBL : ITblProductItemBL
    {
        #region Selection
        private readonly ITblGlobalRatePurchaseDAO _iTblGlobalRatePurchaseDAO;
        private readonly ITblProductItemDAO _iTblProductItemDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly Icommondao _iCommonDAO;

        public TblProductItemBL(ITblProductItemDAO iTblProductItemDAO, ITblGlobalRatePurchaseDAO iTblGlobalRatePurchaseDAO
                                , IConnectionString iConnectionString,Icommondao icommondao
)
        {
            _iCommonDAO = icommondao;
            _iTblGlobalRatePurchaseDAO = iTblGlobalRatePurchaseDAO;
            _iTblProductItemDAO = iTblProductItemDAO;
        }

        public  List<TblProductItemTO> SelectAllTblProductItemList(Int32 specificationId = 0)
        {
            return  _iTblProductItemDAO.SelectAllTblProductItem(specificationId);
        }
        public  List<TblProductItemTO> SelectAllTblProductItemListWithParityAndRecovery(Int32 specificationId = 0, Int32 stateId = 0)
        {
            return _iTblProductItemDAO.SelectAllTblProductItemListWithParityAndRecovery(specificationId, stateId);
        }
         public  TblProductItemTO GetBaseItemRecovery(Int32 isBaseItem = 0,Int32 stateId=0)
        {
            TblProductItemTO tblProductItemTO= _iTblProductItemDAO.GetBaseItemRecovery(isBaseItem,stateId);
            if(tblProductItemTO!=null)
            {
                tblProductItemTO.GlobalRatePurchaseTO=new TblGlobalRatePurchaseTO();
                DateTime sysDate= _iCommonDAO.ServerDateTime;
                List<TblGlobalRatePurchaseTO> tblGlobalRatePurchaseTOList= _iTblGlobalRatePurchaseDAO.SelectLatestRateOfPurchaseDCT(sysDate);
                if(tblGlobalRatePurchaseTOList!=null && tblGlobalRatePurchaseTOList.Count>0)
                    tblProductItemTO.GlobalRatePurchaseTO=tblGlobalRatePurchaseTOList[0];
            }

            return tblProductItemTO;
            
        }
          public  TblProductItemTO GetBaseItemRecovery(Int32 isBaseItem,Int32 stateId,SqlConnection conn,SqlTransaction tran)
        {
            TblProductItemTO tblProductItemTO= _iTblProductItemDAO.GetBaseItemRecovery(isBaseItem,stateId,conn,tran);
            return tblProductItemTO;
            
        }
        public  List<TblProductItemTO> SelectAllTblProductItemListWithParityAndRecoveryNew(Int32 specificationId = 0, Int32 stateId = 0)
        {
            return _iTblProductItemDAO.SelectAllTblProductItemListWithParityAndRecoveryNew(specificationId, stateId);
        }
        public  List<TblProductItemTO> GetGradeBookingParityDtls(DateTime saudaCreatedOn,Int32 specificationId = 0, Int32 stateId = 0)
        {
            return _iTblProductItemDAO.GetGradeBookingParityDtls(saudaCreatedOn,specificationId, stateId);
        }
         public  List<TblProductItemTO> SelectAllTblProductItemListByProdItemId(Int32 prodItemId, Int32 stateId,SqlConnection conn = null,SqlTransaction tran = null)
        {
            if(conn != null && tran != null)
                return _iTblProductItemDAO.SelectAllTblProductItemListByProdItemId(prodItemId, stateId,conn,tran);
            else
                return _iTblProductItemDAO.SelectAllTblProductItemListByProdItemId(prodItemId, stateId);
        }
        public  List<TblProductItemTO> SelectAllTblProductGraidList(Int32 specificationId = 0 )
        {
            return _iTblProductItemDAO.SelectAllTblProductGraidList(specificationId);
        }
        public  List<TblProductItemTO> SelectAllTblProductGraidList(string specificationId = "0" )
        {
            return _iTblProductItemDAO.SelectAllTblProductGraidList(specificationId);
        }

        //
        public  TblProductItemTO SelectTblProductItemTO(Int32 idProdItem)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblProductItemDAO.SelectTblProductItem(idProdItem,conn,tran);

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

        public  TblProductItemTO SelectTblProductItemTO(Int32 idProdItem,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblProductItemDAO.SelectTblProductItem(idProdItem, conn, tran);
        }

        /// <summary>
        /// Sudhir[11-Jan-2017] Added for Get List of Items Based on isStockRequire Flag
        /// </summary>
        /// <param name="isStockRequire"></param>
        /// <returns></returns>
        public  List<TblProductItemTO> SelectProductItemListStockUpdateRequire(int isStockRequire)
        {
            return _iTblProductItemDAO.SelectProductItemListStockUpdateRequire(isStockRequire);
        }

        #endregion

        #region Insertion
        public  int InsertTblProductItem(TblProductItemTO tblProductItemTO)
        {
            return _iTblProductItemDAO.InsertTblProductItem(tblProductItemTO);
        }

        public  int InsertTblProductItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProductItemDAO.InsertTblProductItem(tblProductItemTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblProductItem(TblProductItemTO tblProductItemTO)
        {
            return _iTblProductItemDAO.UpdateTblProductItem(tblProductItemTO);
        }

        public  int UpdateTblProductItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProductItemDAO.UpdateTblProductItem(tblProductItemTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblProductItem(Int32 idProdItem)
        {
            return _iTblProductItemDAO.DeleteTblProductItem(idProdItem);
        }

        public  int DeleteTblProductItem(Int32 idProdItem, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProductItemDAO.DeleteTblProductItem(idProdItem, conn, tran);
        }

        #endregion
        
    }
}
