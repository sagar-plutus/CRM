using PurchaseTrackerAPI.BL;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TO;

namespace PurchaseTrackerAPI.BL.Interfaces
{
    public interface ITblPurchaseVehLinkSaudaBL
    {
        List<TblPurchaseVehLinkSaudaTO> SelectAllTblPurchaseVehLinkSauda();
        List<TblPurchaseVehLinkSaudaTO> SelectAllTblPurchaseVehLinkSaudaList();
        TblPurchaseVehLinkSaudaTO SelectTblPurchaseVehLinkSaudaTO(Int32 idVehLinkSauda);
        int InsertTblPurchaseVehLinkSauda(TblPurchaseVehLinkSaudaTO tblPurchaseVehLinkSaudaTO);
        int InsertTblPurchaseVehLinkSauda(TblPurchaseVehLinkSaudaTO tblPurchaseVehLinkSaudaTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseVehLinkSauda(TblPurchaseVehLinkSaudaTO tblPurchaseVehLinkSaudaTO);
        int UpdateTblPurchaseVehLinkSauda(TblPurchaseVehLinkSaudaTO tblPurchaseVehLinkSaudaTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseVehLinkSauda(Int32 idVehLinkSauda);
        int DeleteTblPurchaseVehLinkSauda(Int32 idVehLinkSauda, SqlConnection conn, SqlTransaction tran);

        ResultMessage SavePurchaseVehLinkSaudaDtls(TblPurchaseScheduleSummaryTO scheduleSummaryTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage DeactivatePreviousLinkSauda(TblPurchaseScheduleSummaryTO scheduleSummaryTO, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseVehLinkSaudaTO> SelectTblPurchaseVehLinkSauda(Int32 rootScheduleId, SqlConnection conn = null, SqlTransaction tran = null);
    }
}
