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
    public interface ITblDashboardEntityBL
    {

        #region Selection
        List<TblDashboardEntityTO> SelectAllTblDashboardEntity();

        List<TblDashboardEntityTO> SelectAllTblDashboardEntityList();

        TblDashboardEntityTO SelectTblDashboardEntityTO(Int32 idDashboardEntity);

        List<TblDashboardEntityTO> SelectTblDashboardEntityListByModuleId(Int32 moduleId);

        List<TblDashboardEntityTO> SelectAllDashboardEntityList(Int32 moduleId, int dashboardEntityId, DateTime fromDate, DateTime toDate);

        #endregion

        #region Insertion
        int InsertTblDashboardEntity(TblDashboardEntityTO tblDashboardEntityTO);

        int InsertTblDashboardEntity(TblDashboardEntityTO tblDashboardEntityTO, SqlConnection conn, SqlTransaction tran);
       
        #endregion

        #region Updation
        int UpdateTblDashboardEntity(TblDashboardEntityTO tblDashboardEntityTO);

        int UpdateTblDashboardEntity(TblDashboardEntityTO tblDashboardEntityTO, SqlConnection conn, SqlTransaction tran);
      

        #endregion

        #region Deletion
        int DeleteTblDashboardEntity(Int32 idDashboardEntity);

        int DeleteTblDashboardEntity(Int32 idDashboardEntity, SqlConnection conn, SqlTransaction tran);

        #endregion

        ResultMessage PostDashboardEntityDtls(List<TblDashboardEntityTO> tblDashboardEntityTOList, Int32 loginUserId);

    }
}
