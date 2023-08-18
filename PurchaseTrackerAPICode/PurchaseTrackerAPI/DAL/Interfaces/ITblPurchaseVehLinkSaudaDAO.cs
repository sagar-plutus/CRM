using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseVehLinkSaudaDAO
    {
        String SqlSelectQuery();
        List<TblPurchaseVehLinkSaudaTO> SelectAllTblPurchaseVehLinkSauda();
        List<TblPurchaseVehLinkSaudaTO> SelectTblPurchaseVehLinkSauda(Int32 rootScheduleId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseVehLinkSaudaTO> SelectTblPurchaseVehLinkSauda(Int32 idVehLinkSauda);
        List<TblPurchaseVehLinkSaudaTO> SelectPurchaseVehLinkSauda(Int32 rootScheduleId);
        List<TblPurchaseVehLinkSaudaTO> SelectAllTblPurchaseVehLinkSauda(SqlConnection conn, SqlTransaction tran);
        int InsertTblPurchaseVehLinkSauda(TblPurchaseVehLinkSaudaTO tblPurchaseVehLinkSaudaTO);
        int InsertTblPurchaseVehLinkSauda(TblPurchaseVehLinkSaudaTO tblPurchaseVehLinkSaudaTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPurchaseVehLinkSaudaTO tblPurchaseVehLinkSaudaTO, SqlCommand cmdInsert);
        int UpdateTblPurchaseVehLinkSauda(TblPurchaseVehLinkSaudaTO tblPurchaseVehLinkSaudaTO);
        int UpdateTblPurchaseVehLinkSauda(TblPurchaseVehLinkSaudaTO tblPurchaseVehLinkSaudaTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPurchaseVehLinkSaudaTO tblPurchaseVehLinkSaudaTO, SqlCommand cmdUpdate);
        int DeleteTblPurchaseVehLinkSauda(Int32 idVehLinkSauda);
        int DeleteTblPurchaseVehLinkSauda(Int32 idVehLinkSauda, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idVehLinkSauda, SqlCommand cmdDelete);
        int DeletePurchaseVehLinkSaudaDtls(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran);
    }
    
}
