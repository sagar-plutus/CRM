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
    public interface ITblAddonsFunDtlsBL
    {
        List<TblAddonsFunDtlsTO> SelectAllTblAddonsFunDtlsList();
        TblAddonsFunDtlsTO SelectTblAddonsFunDtlsTO(int idAddonsfunDtls);
        List<TblAddonsFunDtlsTO> SelectTblAddonsFunDtlsByRootScheduleId(Int32 rootScheduleId, SqlConnection conn, SqlTransaction tran);
        List<TblAddonsFunDtlsTO> SelectTblAddonsFunDtlsByPurchaseInvoiceId(Int32 purchaseInvoiceId, SqlConnection conn, SqlTransaction tran);
        List<TblAddonsFunDtlsTO> SelectTblAddonsFunDtlsBySpotVehicleId(Int32 spotVehicleId, SqlConnection conn, SqlTransaction tran);
        List<TblAddonsFunDtlsTO> SelectAddonDetails(int transId, int ModuleId, String TransactionType, String PageElementId, String transIds);
        ResultMessage InsertTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO);
        ResultMessage InsertTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO);
        ResultMessage UpdateTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblAddonsFunDtls(int idAddonsfunDtls);
        int DeleteTblAddonsFunDtls(int idAddonsfunDtls, SqlConnection conn, SqlTransaction tran);
        int DeleteAllPhotoAgainstVehScheduleId(int rootScheduleId, SqlConnection conn, SqlTransaction tran);
        int DeleteAllPhotoAgainstVehInvoiceId(int purchaseInvoiceId, SqlConnection conn, SqlTransaction tran);
        int DeleteAllPhotoAgainstSpotVehId(int spotVehicleId, SqlConnection conn, SqlTransaction tran);

    }
}