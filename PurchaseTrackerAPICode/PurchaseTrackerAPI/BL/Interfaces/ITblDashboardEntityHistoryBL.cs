using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
using PurchaseTrackerAPI.StaticStuff;
namespace PurchaseTrackerAPI.BL.Interfaces
{
    public interface ITblDashboardEntityHistoryBL
    {

        #region Selection
        List<TblDashboardEntityHistoryTO> SelectAllTblDashboardEntityHistory();
        

        List<TblDashboardEntityHistoryTO> SelectAllTblDashboardEntityHistoryList();
       
        TblDashboardEntityHistoryTO SelectTblDashboardEntityHistoryTO(Int32 idDashboardEntityHistoryId);
        
        #endregion

        #region Insertion
        int InsertTblDashboardEntityHistory(TblDashboardEntityHistoryTO tblDashboardEntityHistoryTO);
        

        int InsertTblDashboardEntityHistory(TblDashboardEntityHistoryTO tblDashboardEntityHistoryTO, SqlConnection conn, SqlTransaction tran);
       

        #endregion

        #region Updation
        int UpdateTblDashboardEntityHistory(TblDashboardEntityHistoryTO tblDashboardEntityHistoryTO);

        List<TblDashboardEntityHistoryTO> SelectAllDashboardEntityList(Int32 moduleId, int dashboardEntityId, DateTime fromDate, DateTime toDate);
        int UpdateTblDashboardEntityHistory(TblDashboardEntityHistoryTO tblDashboardEntityHistoryTO, SqlConnection conn, SqlTransaction tran);
        List<TblDashboardEntityHistoryTO> SelectAllDashboardEntityReportData(string fromDate, string toDate);
        #endregion

        #region Deletion
        int DeleteTblDashboardEntityHistory(Int32 idDashboardEntityHistoryId);
        

        int DeleteTblDashboardEntityHistory(Int32 idDashboardEntityHistoryId, SqlConnection conn, SqlTransaction tran);
        
        #endregion
    }
}
