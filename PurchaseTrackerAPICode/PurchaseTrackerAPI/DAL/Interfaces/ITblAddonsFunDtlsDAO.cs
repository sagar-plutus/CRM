using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblAddonsFunDtlsDAO
    {
        String SqlSelectQuery();
        List<TblAddonsFunDtlsTO> SelectAllTblAddonsFunDtls();
        TblAddonsFunDtlsTO SelectTblAddonsFunDtls(int idAddonsfunDtls);
        List<TblAddonsFunDtlsTO> SelectTblAddonsFunDtlsByRootScheduleId(Int32 rootScheduleId, SqlConnection conn, SqlTransaction tran);
        List<TblAddonsFunDtlsTO> SelectTblAddonsFunDtlsByPurchaseInvoiceId(Int32 purchaseInvoiceId, SqlConnection conn, SqlTransaction tran);
        List<TblAddonsFunDtlsTO> SelectTblAddonsFunDtlsBySpotVehicleId(Int32 spotVehicleId, SqlConnection conn, SqlTransaction tran);
        List<TblAddonsFunDtlsTO> SelectAddonDetailsList(int transId, int ModuleId, String TransactionType, String PageElementId, String transIds);
        List<TblAddonsFunDtlsTO> SelectAllTblAddonsFunDtls(SqlConnection conn, SqlTransaction tran);
        int InsertTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO);
        int InsertTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblAddonsFunDtlsTO tblAddonsFunDtlsTO, SqlCommand cmdInsert);
        int UpdateTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO);
        int UpdateTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblAddonsFunDtlsTO tblAddonsFunDtlsTO, SqlCommand cmdUpdate);
        int DeleteTblAddonsFunDtls(int idAddonsfunDtls);
        int DeleteTblAddonsFunDtls(int idAddonsfunDtls, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(int idAddonsfunDtls, SqlCommand cmdDelete);
        int DeleteAllPhotoAgainstVehScheduleId(Int32 rootScheduleId, SqlConnection conn, SqlTransaction tran);
        int DeleteAllPhotoAgainstVehInvoiceId(Int32 purchaseInvoiceId, SqlConnection conn, SqlTransaction tran);
        int DeleteAllPhotoAgainstSpotVehId(Int32 spotVehicleId, SqlConnection conn, SqlTransaction tran);
        List<TblAddonsFunDtlsTO> ConvertDTToList(SqlDataReader tblAddonsFunDtlsTODT);
        List<TblAddonsFunDtlsTO> SelectAddonDetailsList(String transIds, int ModuleId, String TransactionType, String PageElementId);
        int DeleteTblAddonsFunDtls(String idAddonsfunDtlsStr, SqlConnection conn, SqlTransaction tran);

    }
}