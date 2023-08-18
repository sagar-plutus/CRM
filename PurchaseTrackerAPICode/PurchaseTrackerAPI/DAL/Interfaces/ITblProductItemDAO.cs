using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblProductItemDAO
    {
        String SqlSelectQuery();
          String SqlQuery();
        List<TblProductItemTO> SelectAllTblProductItem(Int32 specificationId = 0);
        List<TblProductItemTO> SelectAllTblProductItemListWithParityAndRecovery(Int32 specificationId, Int32 stateId = 0);
        TblProductItemTO GetBaseItemRecovery(Int32 isBaseItem = 0, Int32 stateId = 0);
        TblProductItemTO GetBaseItemRecovery(Int32 isBaseItem, Int32 stateId, SqlConnection conn, SqlTransaction tran);
        List<TblProductItemTO> SelectAllTblProductItemListWithParityAndRecoveryNew(Int32 specificationId, Int32 stateId = 0);

        List<TblProductItemTO> GetGradeBookingParityDtls(DateTime saudaCreatedOn,Int32 specificationId = 0, Int32 stateId = 0,Boolean isGetlatestParity = false);
        List<TblProductItemTO> SelectAllTblProductItemListByProdItemId(Int32 prodItemId, Int32 stateId);
        List<TblProductItemTO> SelectAllTblProductGraidList(Int32 specificationId);
        List<TblProductItemTO> SelectAllTblProductGraidList(string specificationId = "0");
        TblProductItemTO SelectTblProductItem(Int32 idProdItem, SqlConnection conn, SqlTransaction tran);
        List<TblProductItemTO> ConvertDTToList(SqlDataReader tblProductItemTODT, Boolean isParityDetails = false);
        List<TblProductItemTO> ConvertDTToListForUpdate(SqlDataReader tblProductItemTODT);
        List<TblProductItemTO> SelectProductItemListStockUpdateRequire(int isStockRequire);
        int InsertTblProductItem(TblProductItemTO tblProductItemTO);
        int InsertTblProductItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblProductItemTO tblProductItemTO, SqlCommand cmdInsert);
        int UpdateTblProductItem(TblProductItemTO tblProductItemTO);
        int UpdateTblProductItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblProductItemTO tblProductItemTO, SqlCommand cmdUpdate);
        int DeleteTblProductItem(Int32 idProdItem);
        int DeleteTblProductItem(Int32 idProdItem, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idProdItem, SqlCommand cmdDelete);
        List<TblProductItemTO> SelectAllTblProductItemListByProdItemId(Int32 prodItemId, Int32 stateId,SqlConnection conn,SqlTransaction tran);

    }
}