using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseInvoiceInterfacingDtlBL
    {
        List<TblPurchaseInvoiceInterfacingDtlTO> SelectAllTblPurchaseInvoiceInterfacingDtl();
        List<TblPurchaseInvoiceInterfacingDtlTO> SelectAllTblPurchaseInvoiceInterfacingDtlList();
        TblPurchaseInvoiceInterfacingDtlTO SelectTblPurchaseInvoiceInterfacingDtlTO(Int32 idPurInvInterfacingDtl);
        TblPurchaseInvoiceInterfacingDtlTO SelectTblPurchaseInvoiceInterfacingDtlTOAgainstPurInvId(Int64 PurInvId);
        TblPurchaseInvoiceInterfacingDtlTO SelectTblPurchaseInvoiceInterfacingDtlTOAgainstPurInvId(Int64 PurInvId, SqlConnection conn, SqlTransaction tran);
        int InsertTblPurchaseInvoiceInterfacingDtl(TblPurchaseInvoiceInterfacingDtlTO tblPurchaseInvoiceInterfacingDtlTO);
        int InsertTblPurchaseInvoiceInterfacingDtl(TblPurchaseInvoiceInterfacingDtlTO tblPurchaseInvoiceInterfacingDtlTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseInvoiceInterfacingDtl(TblPurchaseInvoiceInterfacingDtlTO tblPurchaseInvoiceInterfacingDtlTO);
        int UpdateTblPurchaseInvoiceInterfacingDtl(TblPurchaseInvoiceInterfacingDtlTO tblPurchaseInvoiceInterfacingDtlTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseInvoiceInterfacingDtl(Int32 idPurInvInterfacingDtl);
        int DeleteTblPurchaseInvoiceInterfacingDtl(Int32 idPurInvInterfacingDtl, SqlConnection conn, SqlTransaction tran);

    }
}