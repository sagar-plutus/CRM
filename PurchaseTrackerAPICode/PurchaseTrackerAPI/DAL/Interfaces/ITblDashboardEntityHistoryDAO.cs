using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblDashboardEntityHistoryDAO
    {

        String SqlSelectQuery();
        

        #region Selection
        List<TblDashboardEntityHistoryTO> SelectAllTblDashboardEntityHistory();
      

        List<TblDashboardEntityHistoryTO> SelectTblDashboardEntityHistory(Int32 idDashboardEntityHistoryId);
        
        List<TblDashboardEntityHistoryTO> SelectAllTblDashboardEntityHistory(SqlConnection conn, SqlTransaction tran);

        List<TblDashboardEntityHistoryTO> SelectAllDashboardEntityList(Int32 moduleId,int dashboardEntityId, DateTime fromDate, DateTime toDate);
        #endregion

        #region Insertion
        int InsertTblDashboardEntityHistory(TblDashboardEntityHistoryTO tblDashboardEntityHistoryTO);
      

        int InsertTblDashboardEntityHistory(TblDashboardEntityHistoryTO tblDashboardEntityHistoryTO, SqlConnection conn, SqlTransaction tran);
      

        int ExecuteInsertionCommand(TblDashboardEntityHistoryTO tblDashboardEntityHistoryTO, SqlCommand cmdInsert);
        
        #endregion

        #region Updation
        int UpdateTblDashboardEntityHistory(TblDashboardEntityHistoryTO tblDashboardEntityHistoryTO);
       

        int UpdateTblDashboardEntityHistory(TblDashboardEntityHistoryTO tblDashboardEntityHistoryTO, SqlConnection conn, SqlTransaction tran);
       

        int ExecuteUpdationCommand(TblDashboardEntityHistoryTO tblDashboardEntityHistoryTO, SqlCommand cmdUpdate);
       
        #endregion

        #region Deletion
        int DeleteTblDashboardEntityHistory(Int32 idDashboardEntityHistoryId);
      

        int DeleteTblDashboardEntityHistory(Int32 idDashboardEntityHistoryId, SqlConnection conn, SqlTransaction tran);
      
        int ExecuteDeletionCommand(Int32 idDashboardEntityHistoryId, SqlCommand cmdDelete);
    
        #endregion
    }
}
