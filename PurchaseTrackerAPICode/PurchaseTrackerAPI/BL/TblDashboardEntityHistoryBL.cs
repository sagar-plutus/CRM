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
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblDashboardEntityHistoryBL : ITblDashboardEntityHistoryBL
    {
        private readonly IConnectionString _iConnectionString;
        private readonly Icommondao _iCommonDAO;
        private readonly ITblDashboardEntityHistoryDAO _iTblDashboardEntityHistoryDAO;
        private readonly ITblDashboardEntityDAO _iTblDashboardEntityDAO;

        public TblDashboardEntityHistoryBL(ITblDashboardEntityHistoryDAO iTblDashboardEntityHistoryDAO
                                            , ITblDashboardEntityDAO iTblDashboardEntityDAO)
        {
            _iTblDashboardEntityHistoryDAO = iTblDashboardEntityHistoryDAO;
            _iTblDashboardEntityDAO = iTblDashboardEntityDAO;
        }

        #region Selection
        public List<TblDashboardEntityHistoryTO>  SelectAllTblDashboardEntityHistory()
        {
            return _iTblDashboardEntityHistoryDAO.SelectAllTblDashboardEntityHistory();
        }
        public List<TblDashboardEntityHistoryTO> SelectAllDashboardEntityList(Int32 moduleId, int dashboardEntityId, DateTime fromDate, DateTime toDate)
        {
            return _iTblDashboardEntityHistoryDAO.SelectAllDashboardEntityList(moduleId, dashboardEntityId,fromDate, toDate);
        }
        public List<TblDashboardEntityHistoryTO> SelectAllDashboardEntityReportData(string fromDate, string toDate)
        {
            List<TblDashboardEntityHistoryTO> tblDashboardEntityHistoryTOList = new List<TblDashboardEntityHistoryTO>();
            try
            {
                int modelId = 5;
                int dashboardEntityId = (int)Constants.DashboardEntityE.BirimMakina;
                DateTime _fromDate = Convert.ToDateTime(fromDate);
                DateTime _toDate = Convert.ToDateTime(toDate);
                _fromDate = Constants.GetStartDateTime(_fromDate);
                _toDate = Constants.GetEndDateTime(_toDate);
                tblDashboardEntityHistoryTOList = _iTblDashboardEntityHistoryDAO.SelectAllDashboardEntityList(modelId, dashboardEntityId, _fromDate, _toDate);
                List<TblDashboardEntityTO> tblDashboardEntityTOList = _iTblDashboardEntityDAO.SelectAllDashboardEntityList(modelId, dashboardEntityId, _fromDate, _toDate);
                if (tblDashboardEntityTOList != null && tblDashboardEntityTOList.Count > 0)
                {
                    if (tblDashboardEntityHistoryTOList == null)
                        tblDashboardEntityHistoryTOList = new List<TblDashboardEntityHistoryTO>();
                    TblDashboardEntityTO tblDashboardEntityTO = tblDashboardEntityTOList[0];
                    TblDashboardEntityHistoryTO tblDashboardEntityHistoryTO = new TblDashboardEntityHistoryTO();
                    tblDashboardEntityHistoryTO.CreatedBy = tblDashboardEntityTO.CreatedBy;
                    tblDashboardEntityHistoryTO.CreatedOn = tblDashboardEntityTO.CreatedOn;
                    tblDashboardEntityHistoryTO.DashboardEntityId = tblDashboardEntityTO.IdDashboardEntity;
                    tblDashboardEntityHistoryTO.EntityName = tblDashboardEntityTO.EntityName;
                    tblDashboardEntityHistoryTO.EntityValue = tblDashboardEntityTO.EntityValue;
                    tblDashboardEntityHistoryTO.ModuleId = tblDashboardEntityTO.ModuleId;
                    tblDashboardEntityHistoryTOList.Add(tblDashboardEntityHistoryTO);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return tblDashboardEntityHistoryTOList;
        }

        public  List<TblDashboardEntityHistoryTO> SelectAllTblDashboardEntityHistoryList()
        {
            return _iTblDashboardEntityHistoryDAO.SelectAllTblDashboardEntityHistory();
        }

        public TblDashboardEntityHistoryTO SelectTblDashboardEntityHistoryTO(Int32 idDashboardEntityHistoryId)
        {
            List<TblDashboardEntityHistoryTO> tblDashboardEntityHistoryTOList = _iTblDashboardEntityHistoryDAO.SelectTblDashboardEntityHistory(idDashboardEntityHistoryId);
            if(tblDashboardEntityHistoryTOList != null && tblDashboardEntityHistoryTOList.Count == 1)
                return tblDashboardEntityHistoryTOList[0];
            else
                return null;
        }

     
        #endregion
        
        #region Insertion
        public int InsertTblDashboardEntityHistory(TblDashboardEntityHistoryTO tblDashboardEntityHistoryTO)
        {
            return _iTblDashboardEntityHistoryDAO.InsertTblDashboardEntityHistory(tblDashboardEntityHistoryTO);
        }

        public  int InsertTblDashboardEntityHistory(TblDashboardEntityHistoryTO tblDashboardEntityHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblDashboardEntityHistoryDAO.InsertTblDashboardEntityHistory(tblDashboardEntityHistoryTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblDashboardEntityHistory(TblDashboardEntityHistoryTO tblDashboardEntityHistoryTO)
        {
            return _iTblDashboardEntityHistoryDAO.UpdateTblDashboardEntityHistory(tblDashboardEntityHistoryTO);
        }

        public  int UpdateTblDashboardEntityHistory(TblDashboardEntityHistoryTO tblDashboardEntityHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblDashboardEntityHistoryDAO.UpdateTblDashboardEntityHistory(tblDashboardEntityHistoryTO, conn, tran);
        }



        #endregion
        
        #region Deletion
        public  int DeleteTblDashboardEntityHistory(Int32 idDashboardEntityHistoryId)
        {
            return _iTblDashboardEntityHistoryDAO.DeleteTblDashboardEntityHistory(idDashboardEntityHistoryId);
        }

        public  int DeleteTblDashboardEntityHistory(Int32 idDashboardEntityHistoryId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblDashboardEntityHistoryDAO.DeleteTblDashboardEntityHistory(idDashboardEntityHistoryId, conn, tran);
        }

        #endregion
        
    }
}
