using PurchaseTrackerAPI.BL;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;
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
    public class TblPurchaseVehicleStatusHistoryBL : ITblPurchaseVehicleStatusHistoryBL
    {

        public readonly ITblPurchaseVehicleStatusHistoryDAO _iTblPurchaseVehicleStatusHistoryDAO ;

        public TblPurchaseVehicleStatusHistoryBL(ITblPurchaseVehicleStatusHistoryDAO iTblPurchaseVehicleStatusHistoryDAO)
        {
            _iTblPurchaseVehicleStatusHistoryDAO = iTblPurchaseVehicleStatusHistoryDAO;
        }

        #region Selection
        public  List<TblPurchaseVehicleStatusHistoryTO> SelectAllTblPurchaseVehicleStatusHistory()
        {
            return _iTblPurchaseVehicleStatusHistoryDAO.SelectAllTblPurchaseVehicleStatusHistory();
        }

        public  List<TblPurchaseVehicleStatusHistoryTO> SelectAllTblPurchaseVehicleStatusHistoryList()
        {
            return _iTblPurchaseVehicleStatusHistoryDAO.SelectAllTblPurchaseVehicleStatusHistory();
        }

        public  TblPurchaseVehicleStatusHistoryTO SelectTblPurchaseVehicleStatusHistoryTO(Int32 idPurVehStatusHistory)
        {
            List<TblPurchaseVehicleStatusHistoryTO> tblPurchaseVehicleStatusHistoryTOList = _iTblPurchaseVehicleStatusHistoryDAO.SelectTblPurchaseVehicleStatusHistory(idPurVehStatusHistory);
            if(tblPurchaseVehicleStatusHistoryTOList != null && tblPurchaseVehicleStatusHistoryTOList.Count == 1)
                return tblPurchaseVehicleStatusHistoryTOList[0];
            else
                return null;
        }

     
        #endregion
        
        #region Insertion
        public  int InsertTblPurchaseVehicleStatusHistory(TblPurchaseVehicleStatusHistoryTO tblPurchaseVehicleStatusHistoryTO)
        {
            return _iTblPurchaseVehicleStatusHistoryDAO.InsertTblPurchaseVehicleStatusHistory(tblPurchaseVehicleStatusHistoryTO);
        }

        public  int InsertTblPurchaseVehicleStatusHistory(TblPurchaseVehicleStatusHistoryTO tblPurchaseVehicleStatusHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehicleStatusHistoryDAO.InsertTblPurchaseVehicleStatusHistory(tblPurchaseVehicleStatusHistoryTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblPurchaseVehicleStatusHistory(TblPurchaseVehicleStatusHistoryTO tblPurchaseVehicleStatusHistoryTO)
        {
            return _iTblPurchaseVehicleStatusHistoryDAO.UpdateTblPurchaseVehicleStatusHistory(tblPurchaseVehicleStatusHistoryTO);
        }

        public  int UpdateTblPurchaseVehicleStatusHistory(TblPurchaseVehicleStatusHistoryTO tblPurchaseVehicleStatusHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehicleStatusHistoryDAO.UpdateTblPurchaseVehicleStatusHistory(tblPurchaseVehicleStatusHistoryTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblPurchaseVehicleStatusHistory(Int32 idPurVehStatusHistory)
        {
            return _iTblPurchaseVehicleStatusHistoryDAO.DeleteTblPurchaseVehicleStatusHistory(idPurVehStatusHistory);
        }

        public  int DeleteTblPurchaseVehicleStatusHistory(Int32 idPurVehStatusHistory, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehicleStatusHistoryDAO.DeleteTblPurchaseVehicleStatusHistory(idPurVehStatusHistory, conn, tran);
        }

        #endregion
        
    }
}
