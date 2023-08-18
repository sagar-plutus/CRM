using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblAlertActionDtlBL
    {
        List<TblAlertActionDtlTO> SelectAllTblAlertActionDtlList();
        TblAlertActionDtlTO SelectTblAlertActionDtlTO(Int32 idAlertActionDtl);
        TblAlertActionDtlTO SelectTblAlertActionDtlTO(Int32 alertInstanceId, Int32 userId);
        TblAlertActionDtlTO SelectTblAlertActionDtlTO(Int32 alertInstanceId, Int32 userId, SqlConnection conn, SqlTransaction tran);
        List<TblAlertActionDtlTO> SelectAllTblAlertActionDtlList(Int32 userId);
        int InsertTblAlertActionDtl(TblAlertActionDtlTO tblAlertActionDtlTO);
        int InsertTblAlertActionDtl(TblAlertActionDtlTO tblAlertActionDtlTO, SqlConnection conn, SqlTransaction tran);
          int UpdateTblAlertActionDtl(TblAlertActionDtlTO tblAlertActionDtlTO);
        int UpdateTblAlertActionDtl(TblAlertActionDtlTO tblAlertActionDtlTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage ResetAllAlerts(int loginUserId, List<TblAlertUsersTO> list, int result);
        int DeleteTblAlertActionDtl(Int32 idAlertActionDtl);
        int DeleteTblAlertActionDtl(Int32 idAlertActionDtl, SqlConnection conn, SqlTransaction tran);

    }
}