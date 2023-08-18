using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseParityDetailsBL
    {
        List<TblPurchaseParityDetailsTO> SelectAllTblParityDetailsList(Int32 parityId, Int32 prodSpecId, Int32 stateId, Int32 brandId);
        List<TblPurchaseParityDetailsTO> SelectAllEmptyParityDetailsList(Int32 prodSpecId, Int32 stateId, Int32 brandId);
        List<TblPurchaseParityDetailsTO> SelectAllTblParityDetailsList(String parityIds, int prodSpecId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseParityDetailsTO> GetBookingItemsParityDtls(Int32 prodItemId, DateTime createdOn);
        List<TblPurchaseParityDetailsTO> GetBookingItemsParityDtls(string prodItemIds, DateTime createdOn, Int32 stateId,SqlConnection conn = null,SqlTransaction tran = null);
        List<TblPurchaseParityDetailsTO> SelectAllTblParityDetailsList(Int32 parityId, int prodSpecId, SqlConnection conn, SqlTransaction tran);
        int InsertTblParityDetails(TblPurchaseParityDetailsTO tblParityDetailsTO);
        int SaveProductImgSettings(SaveProductImgTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran);
        int InsertTblParityDetails(TblPurchaseParityDetailsTO tblParityDetailsTO, SqlConnection conn, SqlTransaction tran);

    }
}