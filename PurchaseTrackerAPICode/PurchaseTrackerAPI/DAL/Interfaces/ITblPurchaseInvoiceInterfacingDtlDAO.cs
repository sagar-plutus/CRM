using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseInvoiceInterfacingDtlDAO
    {
        String SqlSelectQuery();
        List<TblPurchaseInvoiceInterfacingDtlTO> SelectAllTblPurchaseInvoiceInterfacingDtl();
        TblPurchaseInvoiceInterfacingDtlTO SelectTblPurchaseInvoiceInterfacingDtl(Int32 idPurInvInterfacingDtl);
        TblPurchaseInvoiceInterfacingDtlTO SelectTblPurchaseInvoiceInterfacingDtlTOAgainstPurInvId(Int64 PurInvId);

        TblPurchaseInvoiceInterfacingDtlTO SelectTblPurchaseInvoiceInterfacingDtlTOForReport(Int64 PurInvId);

        TblPurchaseInvoiceInterfacingDtlTO SelectTblPurchaseInvoiceInterfacingDtlTOAgainstPurInvId(Int64 PurInvId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseInvoiceInterfacingDtlTO> SelectAllTblPurchaseInvoiceInterfacingDtl(SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseInvoiceInterfacingDtlTO> ConvertDTToList(SqlDataReader tblPurchaseInvoiceInterfacingDtlTODT);
        int InsertTblPurchaseInvoiceInterfacingDtl(TblPurchaseInvoiceInterfacingDtlTO tblPurchaseInvoiceInterfacingDtlTO);
        int InsertTblPurchaseInvoiceInterfacingDtl(TblPurchaseInvoiceInterfacingDtlTO tblPurchaseInvoiceInterfacingDtlTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPurchaseInvoiceInterfacingDtlTO tblPurchaseInvoiceInterfacingDtlTO, SqlCommand cmdInsert);
        int UpdateTblPurchaseInvoiceInterfacingDtl(TblPurchaseInvoiceInterfacingDtlTO tblPurchaseInvoiceInterfacingDtlTO);
        int UpdateTblPurchaseInvoiceInterfacingDtl(TblPurchaseInvoiceInterfacingDtlTO tblPurchaseInvoiceInterfacingDtlTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPurchaseInvoiceInterfacingDtlTO tblPurchaseInvoiceInterfacingDtlTO, SqlCommand cmdUpdate);
        int DeleteTblPurchaseInvoiceInterfacingDtl(Int32 idPurInvInterfacingDtl);
        int DeleteTblPurchaseInvoiceInterfacingDtl(Int32 idPurInvInterfacingDtl, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idPurInvInterfacingDtl, SqlCommand cmdDelete);

    }
; }