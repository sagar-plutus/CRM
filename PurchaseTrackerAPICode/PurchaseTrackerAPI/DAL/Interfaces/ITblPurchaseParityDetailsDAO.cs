using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseParityDetailsDAO
    {
        String SqlSelectQuery();
        List<TblPurchaseParityDetailsTO> SelectAllTblParityDetails(int parityId, Int32 prodSpecId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseParityDetailsTO> SelectAllTblParityDetails(String parityIds, Int32 prodSpecId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseParityDetailsTO> SelectAllLatestParityDetails(Int32 stateId, Int32 prodSpecId, Int32 brandId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseParityDetailsTO> SelectAllLatestParityDetails(Int32 stateId, Int32 prodItemId);
        List<TblPurchaseParityDetailsTO> GetBookingItemsParityDtls(Int32 prodItemId, DateTime createdOn);
        List<TblPurchaseParityDetailsTO> GetBookingItemsParityDtls(string prodItemIds, DateTime createdOn,  Int32 stateId);
        List<TblPurchaseParityDetailsTO> ConvertDTToList(SqlDataReader tblPurchaseParityDetailsTODT);
        List<TblPurchaseParityDetailsTO> ConvertDTToListNew(SqlDataReader tblPurchaseParityDetailsTODT);
        int InsertTblParityDetails(TblPurchaseParityDetailsTO tblParityDetailsTO);
        int InsertTblParityDetails(TblPurchaseParityDetailsTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPurchaseParityDetailsTO tblParityDetailsTO, SqlCommand cmdInsert);
        int InsertProductImgDetails(SaveProductImgTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseParityDetailsTO> GetBookingItemsParityDtls(string prodItemIds, DateTime createdOn, Int32 stateId,SqlConnection conn,SqlTransaction tran);

    }
}