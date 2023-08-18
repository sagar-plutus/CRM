using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System.Linq;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblBaseItemMetalCostBL : ITblBaseItemMetalCostBL
    {
        readonly private ITblBaseItemMetalCostDAO _iTblBaseItemMetalCostDAO;
        readonly private ITblPurchaseScheduleSummaryBL _iTblPurchaseScheduleSummaryBL;
        public TblBaseItemMetalCostBL(ITblBaseItemMetalCostDAO iTblBaseItemMetalCostDAO)
        {
            _iTblBaseItemMetalCostDAO = iTblBaseItemMetalCostDAO;
        }
        #region Selection
        public  List<TblBaseItemMetalCostTO> SelectAllTblBaseItemMetalCost()
        {
            return _iTblBaseItemMetalCostDAO.SelectAllTblBaseItemMetalCost();
        }

        public  List<TblBaseItemMetalCostTO> SelectAllTblBaseItemMetalCostList()
        {
            return _iTblBaseItemMetalCostDAO.SelectAllTblBaseItemMetalCost();
        }

        public  TblBaseItemMetalCostTO SelectTblBaseItemMetalCostTO(Int32 idBaseItemMetalCost)
        {
            List<TblBaseItemMetalCostTO> tblBaseItemMetalCostTOList = _iTblBaseItemMetalCostDAO.SelectTblBaseItemMetalCost(idBaseItemMetalCost);
            if (tblBaseItemMetalCostTOList != null && tblBaseItemMetalCostTOList.Count == 1)
                return tblBaseItemMetalCostTOList[0];
            else
                return null;
        }

        public  List<TblBaseItemMetalCostTO> SelectLatestBaseItemMetalCost(Int32 globalRatePurchaseId)
        {
            List<TblBaseItemMetalCostTO> tblBaseItemMetalCostTOList = _iTblBaseItemMetalCostDAO.SelectLatestBaseItemMetalCost(globalRatePurchaseId);
            if (tblBaseItemMetalCostTOList != null && tblBaseItemMetalCostTOList.Count > 0)
                return tblBaseItemMetalCostTOList;
            else
                return null;
        }

        public  List<TblBaseItemMetalCostTO> SelectLatestBaseItemMetalCost(Int32 globalRatePurchaseId,SqlConnection conn,SqlTransaction tran)
        {
            List<TblBaseItemMetalCostTO> tblBaseItemMetalCostTOList = _iTblBaseItemMetalCostDAO.SelectLatestBaseItemMetalCost(globalRatePurchaseId,conn,tran);
            if (tblBaseItemMetalCostTOList != null && tblBaseItemMetalCostTOList.Count > 0)
                return tblBaseItemMetalCostTOList;
            else
                return null;
        }

        public  List<TblBaseItemMetalCostTO> SelectBaseItemMetalCostByGlobalRateId(Int32 globalRatePurchaseId,Int32 cOrNcId)
        {
            List<TblBaseItemMetalCostTO> tblBaseItemMetalCostTOList = _iTblBaseItemMetalCostDAO.SelectBaseItemMetalCostByGlobalRateId(globalRatePurchaseId,cOrNcId);
            if (tblBaseItemMetalCostTOList != null && tblBaseItemMetalCostTOList.Count > 0)
                return tblBaseItemMetalCostTOList;
            else
                return null;
        }

        #endregion

        #region Insertion
        public  int InsertTblBaseItemMetalCost(TblBaseItemMetalCostTO tblBaseItemMetalCostTO)
        {
            return _iTblBaseItemMetalCostDAO.InsertTblBaseItemMetalCost(tblBaseItemMetalCostTO);
        }

        public  int InsertTblBaseItemMetalCost(TblBaseItemMetalCostTO tblBaseItemMetalCostTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBaseItemMetalCostDAO.InsertTblBaseItemMetalCost(tblBaseItemMetalCostTO, conn, tran);
        }

        #endregion

        #region Updation
        public  int UpdateTblBaseItemMetalCost(TblBaseItemMetalCostTO tblBaseItemMetalCostTO)
        {
            return _iTblBaseItemMetalCostDAO.UpdateTblBaseItemMetalCost(tblBaseItemMetalCostTO);
        }

        public  int UpdateTblBaseItemMetalCost(TblBaseItemMetalCostTO tblBaseItemMetalCostTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBaseItemMetalCostDAO.UpdateTblBaseItemMetalCost(tblBaseItemMetalCostTO, conn, tran);
        }

        #endregion

        #region Deletion
        public  int DeleteTblBaseItemMetalCost(Int32 idBaseItemMetalCost)
        {
            return _iTblBaseItemMetalCostDAO.DeleteTblBaseItemMetalCost(idBaseItemMetalCost);
        }

        public  int DeleteTblBaseItemMetalCost(Int32 idBaseItemMetalCost, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBaseItemMetalCostDAO.DeleteTblBaseItemMetalCost(idBaseItemMetalCost, conn, tran);
        }

        #endregion

    }
}
