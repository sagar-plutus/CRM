using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces; 
using PurchaseTrackerAPI.DAL;

namespace PurchaseTrackerAPI.BL
{
    public class TblTRLoadingHistoryBL : ITblTRLoadingHistoryBL
    {

        ITblTRLoadingHistoryDAO _iTblTRLoadingHistoryDAO;
        public TblTRLoadingHistoryBL(ITblTRLoadingHistoryDAO iTblTRLoadingHistoryDAO)
        {
            _iTblTRLoadingHistoryDAO = iTblTRLoadingHistoryDAO;
        }

        #region Selection
        public  DataTable SelectAllTblTRLoadingHistory()
        {
            return _iTblTRLoadingHistoryDAO.SelectAllTblTRLoadingHistory();
        }

        public  List<TblTRLoadingHistoryTO> SelectAllTblTRLoadingHistoryList()
        {
            DataTable tblTRLoadingHistoryTODT = _iTblTRLoadingHistoryDAO.SelectAllTblTRLoadingHistory();
            return ConvertDTToList(tblTRLoadingHistoryTODT);
        }

        public  TblTRLoadingHistoryTO SelectTblTRLoadingHistoryTO(Int32 idLoadingHistory)
        {
            DataTable tblTRLoadingHistoryTODT = _iTblTRLoadingHistoryDAO.SelectTblTRLoadingHistory(idLoadingHistory);
            List<TblTRLoadingHistoryTO> tblTRLoadingHistoryTOList = ConvertDTToList(tblTRLoadingHistoryTODT);
            if(tblTRLoadingHistoryTOList != null && tblTRLoadingHistoryTOList.Count == 1)
                return tblTRLoadingHistoryTOList[0];
            else
                return null;
        }

        public  List<TblTRLoadingHistoryTO> ConvertDTToList(DataTable tblTRLoadingHistoryTODT )
        {
            List<TblTRLoadingHistoryTO> tblTRLoadingHistoryTOList = new List<TblTRLoadingHistoryTO>();
            if (tblTRLoadingHistoryTODT != null)
            {
                for (int rowCount = 0; rowCount < tblTRLoadingHistoryTODT.Rows.Count; rowCount++)
                {
                    TblTRLoadingHistoryTO tblTRLoadingHistoryTONew = new TblTRLoadingHistoryTO();
                    if(tblTRLoadingHistoryTODT.Rows[rowCount]["idLoadingHistory"] != DBNull.Value)
                        tblTRLoadingHistoryTONew.IdLoadingHistory = Convert.ToInt32( tblTRLoadingHistoryTODT.Rows[rowCount]["idLoadingHistory"].ToString());
                    if(tblTRLoadingHistoryTODT.Rows[rowCount]["loadingId"] != DBNull.Value)
                        tblTRLoadingHistoryTONew.LoadingId = Convert.ToInt32( tblTRLoadingHistoryTODT.Rows[rowCount]["loadingId"].ToString());
                    if(tblTRLoadingHistoryTODT.Rows[rowCount]["statusId"] != DBNull.Value)
                        tblTRLoadingHistoryTONew.StatusId = Convert.ToInt32( tblTRLoadingHistoryTODT.Rows[rowCount]["statusId"].ToString());
                    if(tblTRLoadingHistoryTODT.Rows[rowCount]["statusBy"] != DBNull.Value)
                        tblTRLoadingHistoryTONew.StatusBy = Convert.ToInt32( tblTRLoadingHistoryTODT.Rows[rowCount]["statusBy"].ToString());
                    if(tblTRLoadingHistoryTODT.Rows[rowCount]["statusOn"] != DBNull.Value)
                        tblTRLoadingHistoryTONew.StatusOn = Convert.ToDateTime( tblTRLoadingHistoryTODT.Rows[rowCount]["statusOn"].ToString());
                    tblTRLoadingHistoryTOList.Add(tblTRLoadingHistoryTONew);
                }
            }
            return tblTRLoadingHistoryTOList;
        }

        #endregion
        
        #region Insertion
        public  int InsertTblTRLoadingHistory(TblTRLoadingHistoryTO tblTRLoadingHistoryTO)
        {
            return _iTblTRLoadingHistoryDAO.InsertTblTRLoadingHistory(tblTRLoadingHistoryTO);
        }

        public  int InsertTblTRLoadingHistory(TblTRLoadingHistoryTO tblTRLoadingHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblTRLoadingHistoryDAO.InsertTblTRLoadingHistory(tblTRLoadingHistoryTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblTRLoadingHistory(TblTRLoadingHistoryTO tblTRLoadingHistoryTO)
        {
            return _iTblTRLoadingHistoryDAO.UpdateTblTRLoadingHistory(tblTRLoadingHistoryTO);
        }

        public  int UpdateTblTRLoadingHistory(TblTRLoadingHistoryTO tblTRLoadingHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblTRLoadingHistoryDAO.UpdateTblTRLoadingHistory(tblTRLoadingHistoryTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblTRLoadingHistory(Int32 idLoadingHistory)
        {
            return _iTblTRLoadingHistoryDAO.DeleteTblTRLoadingHistory(idLoadingHistory);
        }

        public  int DeleteTblTRLoadingHistory(Int32 idLoadingHistory, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblTRLoadingHistoryDAO.DeleteTblTRLoadingHistory(idLoadingHistory, conn, tran);
        }

        #endregion
        
    }
}
