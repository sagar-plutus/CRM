using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblProductInfoDAO
    {
        String SqlSelectQuery();
        List<TblProductInfoTO> SelectAllTblProductInfo();
        List<TblProductInfoTO> SelectAllTblProductInfo(SqlConnection conn, SqlTransaction tran);
          List<TblProductInfoTO> SelectAllLatestProductInfo(SqlConnection conn, SqlTransaction tran);
        List<TblProductInfoTO> SelectTblProductInfoLatest();
        TblProductInfoTO SelectTblProductInfo(Int32 idProduct);
        List<TblProductInfoTO> SelectEmptyProductDetailsTemplate(int prodCatId);
        List<TblProductInfoTO> SelectProductInfoListByLoadingSlipExtIds(string strLoadingSlipExtIds);
        List<TblProductInfoTO> ConvertReaderToList(SqlDataReader tblStockDetailsTODT);
        List<TblProductInfoTO> ConvertDTToList(SqlDataReader tblProductInfoTODT);
        int InsertTblProductInfo(TblProductInfoTO tblProductInfoTO);
        int InsertTblProductInfo(TblProductInfoTO tblProductInfoTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblProductInfoTO tblProductInfoTO, SqlCommand cmdInsert);
        int UpdateTblProductInfo(TblProductInfoTO tblProductInfoTO);
        int UpdateTblProductInfo(TblProductInfoTO tblProductInfoTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblProductInfoTO tblProductInfoTO, SqlCommand cmdUpdate);
        int DeleteTblProductInfo(Int32 idProduct);
        int DeleteTblProductInfo(Int32 idProduct, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idProduct, SqlCommand cmdDelete);

        

    }
}