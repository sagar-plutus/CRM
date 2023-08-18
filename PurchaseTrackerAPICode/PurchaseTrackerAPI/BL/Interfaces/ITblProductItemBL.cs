using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblProductItemBL
    {
        List<TblProductItemTO> SelectAllTblProductItemList(Int32 specificationId = 0);
        List<TblProductItemTO> SelectAllTblProductItemListWithParityAndRecovery(Int32 specificationId = 0, Int32 stateId = 0);
        TblProductItemTO GetBaseItemRecovery(Int32 isBaseItem = 0, Int32 stateId = 0);
        TblProductItemTO GetBaseItemRecovery(Int32 isBaseItem, Int32 stateId, SqlConnection conn, SqlTransaction tran);
        List<TblProductItemTO> SelectAllTblProductItemListWithParityAndRecoveryNew(Int32 specificationId = 0, Int32 stateId = 0);

        List<TblProductItemTO> GetGradeBookingParityDtls(DateTime saudaCreatedOn,Int32 specificationId = 0, Int32 stateId = 0);
        List<TblProductItemTO> SelectAllTblProductItemListByProdItemId(Int32 prodItemId, Int32 stateId,SqlConnection conn = null,SqlTransaction tran = null);
        List<TblProductItemTO> SelectAllTblProductGraidList(Int32 specificationId = 0);
        List<TblProductItemTO> SelectAllTblProductGraidList(string specificationId = "0");
        TblProductItemTO SelectTblProductItemTO(Int32 idProdItem);
        TblProductItemTO SelectTblProductItemTO(Int32 idProdItem, SqlConnection conn, SqlTransaction tran);
        List<TblProductItemTO> SelectProductItemListStockUpdateRequire(int isStockRequire);
        int InsertTblProductItem(TblProductItemTO tblProductItemTO);
        int InsertTblProductItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblProductItem(TblProductItemTO tblProductItemTO);
        int UpdateTblProductItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblProductItem(Int32 idProdItem);
        int DeleteTblProductItem(Int32 idProdItem, SqlConnection conn, SqlTransaction tran);

    }
}