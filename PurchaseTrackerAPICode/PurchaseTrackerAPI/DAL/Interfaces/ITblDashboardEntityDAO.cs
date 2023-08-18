using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblDashboardEntityDAO
    {
        String SqlSelectQuery();
       
        #region Selection
        List<TblDashboardEntityTO> SelectAllTblDashboardEntity();
        
        List<TblDashboardEntityTO> SelectTblDashboardEntity(Int32 idDashboardEntity);

        List<TblDashboardEntityTO> SelectAllDashboardEntityListByModuleId(Int32 moduleId);

        List<TblDashboardEntityTO> SelectTblDashboardEntity(Int32 idDashboardEntity,SqlConnection conn,SqlTransaction tran);

        List<TblDashboardEntityTO> SelectAllTblDashboardEntity(SqlConnection conn, SqlTransaction tran);
       
        #endregion

        #region Insertion
        int InsertTblDashboardEntity(TblDashboardEntityTO tblDashboardEntityTO);
        
        int InsertTblDashboardEntity(TblDashboardEntityTO tblDashboardEntityTO, SqlConnection conn, SqlTransaction tran);

        int ExecuteInsertionCommand(TblDashboardEntityTO tblDashboardEntityTO, SqlCommand cmdInsert);
       
        #endregion

        #region Updation
        int UpdateTblDashboardEntity(TblDashboardEntityTO tblDashboardEntityTO);
        List<TblDashboardEntityTO> SelectAllDashboardEntityList(Int32 moduleId, int dashboardEntityId, DateTime fromDate, DateTime toDate);
        int UpdateTblDashboardEntity(TblDashboardEntityTO tblDashboardEntityTO, SqlConnection conn, SqlTransaction tran);

        int ExecuteUpdationCommand(TblDashboardEntityTO tblDashboardEntityTO, SqlCommand cmdUpdate);
      
        #endregion

        #region Deletion
        int DeleteTblDashboardEntity(Int32 idDashboardEntity);
        

        int DeleteTblDashboardEntity(Int32 idDashboardEntity, SqlConnection conn, SqlTransaction tran);
      
        int ExecuteDeletionCommand(Int32 idDashboardEntity, SqlCommand cmdDelete);
       
        #endregion

    }
}
