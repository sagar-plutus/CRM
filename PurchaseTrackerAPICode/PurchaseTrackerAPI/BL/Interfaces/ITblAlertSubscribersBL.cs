using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblAlertSubscribersBL
    {
        List<TblAlertSubscribersTO> SelectAllTblAlertSubscribersList();
        TblAlertSubscribersTO SelectTblAlertSubscribersTO(Int32 idSubscription);
        List<TblAlertSubscribersTO> SelectAllTblAlertSubscribersList(Int32 alertDefId, SqlConnection conn, SqlTransaction tran);
        int InsertTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO);
        int InsertTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO);
        int UpdateTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblAlertSubscribers(Int32 idSubscription);
        int DeleteTblAlertSubscribers(Int32 idSubscription, SqlConnection conn, SqlTransaction tran);

    }
}