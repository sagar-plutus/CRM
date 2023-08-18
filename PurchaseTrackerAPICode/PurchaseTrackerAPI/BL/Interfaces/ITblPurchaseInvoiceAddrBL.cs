using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseInvoiceAddrBL
    {
        List<TblPurchaseInvoiceAddrTO> SelectAllTblPurchaseInvoiceAddr();
        List<TblPurchaseInvoiceAddrTO> SelectAllTblPurchaseInvoiceAddrList();
        List<TblPurchaseInvoiceAddrTO> SelectAllTblPurchaseInvoiceAddrList(Int64 purchaseInvoiceId);
        List<TblPurchaseInvoiceAddrTO> SelectAllTblPurchaseInvoiceAddrList(Int64 purchaseInvoiceId, SqlConnection conn, SqlTransaction tran);
        TblPurchaseInvoiceAddrTO SelectTblPurchaseInvoiceAddrTO(Int64 idPurchaseInvoiceAddr);
        int InsertTblPurchaseInvoiceAddr(TblPurchaseInvoiceAddrTO tblPurchaseInvoiceAddrTO);
        int InsertTblPurchaseInvoiceAddr(TblPurchaseInvoiceAddrTO tblPurchaseInvoiceAddrTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseInvoiceAddr(TblPurchaseInvoiceAddrTO tblPurchaseInvoiceAddrTO);
        int UpdateTblPurchaseInvoiceAddr(TblPurchaseInvoiceAddrTO tblPurchaseInvoiceAddrTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseInvoiceAddr(Int64 idPurchaseInvoiceAddr);
        int DeleteTblPurchaseInvoiceAddr(Int64 idPurchaseInvoiceAddr, SqlConnection conn, SqlTransaction tran);

    }
}