using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseInvoiceAddrDAO
    {
        String SqlSelectQuery();
        List<TblPurchaseInvoiceAddrTO> SelectAllTblPurchaseInvoiceAddr();
        List<TblPurchaseInvoiceAddrTO> SelectAllTblPurchaseInvoiceAddr(Int64 purchaseInvoiceId);
        List<TblPurchaseInvoiceAddrTO> SelectAllTblPurchaseInvoiceAddr(Int64 purchaseInvoiceId, SqlConnection conn, SqlTransaction tran);
        TblPurchaseInvoiceAddrTO SelectTblPurchaseInvoiceAddr(Int64 idPurchaseInvoiceAddr);
        List<TblPurchaseInvoiceAddrTO> SelectAllTblPurchaseInvoiceAddr(SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseInvoiceAddrTO> ConvertDTToList(SqlDataReader tblPurchaseInvoiceAddrTODT);
        int InsertTblPurchaseInvoiceAddr(TblPurchaseInvoiceAddrTO tblPurchaseInvoiceAddrTO);
        int InsertTblPurchaseInvoiceAddr(TblPurchaseInvoiceAddrTO tblPurchaseInvoiceAddrTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPurchaseInvoiceAddrTO tblPurchaseInvoiceAddrTO, SqlCommand cmdInsert);
        int UpdateTblPurchaseInvoiceAddr(TblPurchaseInvoiceAddrTO tblPurchaseInvoiceAddrTO);
        int UpdateTblPurchaseInvoiceAddr(TblPurchaseInvoiceAddrTO tblPurchaseInvoiceAddrTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPurchaseInvoiceAddrTO tblPurchaseInvoiceAddrTO, SqlCommand cmdUpdate);
        int DeleteTblPurchaseInvoiceAddr(Int64 idPurchaseInvoiceAddr);
        int DeleteTblPurchaseInvoiceAddr(Int64 idPurchaseInvoiceAddr, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int64 idPurchaseInvoiceAddr, SqlCommand cmdDelete);

    }
}