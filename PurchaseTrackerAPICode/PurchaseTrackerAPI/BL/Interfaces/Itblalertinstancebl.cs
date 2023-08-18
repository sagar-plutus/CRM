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
    public interface Itblalertinstancebl
    {

        ResultMessage AutoResetAndDeleteAlerts();
        List<TblAlertInstanceTO> SelectAllTblAlertInstanceList();
        TblAlertInstanceTO SelectTblAlertInstanceTO(Int32 idAlertInstance);
        List<TblAlertInstanceTO> SelectAllTblAlertInstanceList(Int32 userId, Int32 roleId);
        ResultMessage SaveNewAlertInstance(TblAlertInstanceTO alertInstanceTO, SqlConnection conn, SqlTransaction tran);
        int InsertTblAlertInstance(TblAlertInstanceTO tblAlertInstanceTO);
        int InsertTblAlertInstance(TblAlertInstanceTO tblAlertInstanceTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblAlertInstance(TblAlertInstanceTO tblAlertInstanceTO);
        int UpdateTblAlertInstance(TblAlertInstanceTO tblAlertInstanceTO, SqlConnection conn, SqlTransaction tran);
        int ResetAlertInstance(int alertDefId, int sourceEntityId, SqlConnection conn, SqlTransaction tran);
        int ResetAlertInstanceByDef(string alertDefIds, SqlConnection conn, SqlTransaction tran);
        int DeleteTblAlertInstance(Int32 idAlertInstance);
        int DeleteTblAlertInstance(Int32 idAlertInstance, SqlConnection conn, SqlTransaction tran);

    }
}