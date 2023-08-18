using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblProductInfoBL
    {

        ResultMessage SaveProductInformation(List<TblProductInfoTO> productInfoTOList);


        List<TblProductInfoTO> SelectAllTblProductInfoList();
        List<TblProductInfoTO> SelectAllTblProductInfoList(SqlConnection conn, SqlTransaction tran);
        List<TblProductInfoTO> SelectAllTblProductInfoListLatest();
        TblProductInfoTO SelectTblProductInfoTO(Int32 idProduct);
        List<TblProductInfoTO> SelectAllEmptyProductInfoList(int prodCatId);
        List<TblProductInfoTO> SelectProductInfoListByLoadingSlipExtIds(string strLoadingSlipExtIds);
        int InsertTblProductInfo(TblProductInfoTO tblProductInfoTO);
        int InsertTblProductInfo(TblProductInfoTO tblProductInfoTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblProductInfo(TblProductInfoTO tblProductInfoTO);
          int UpdateTblProductInfo(TblProductInfoTO tblProductInfoTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblProductInfo(Int32 idProduct);
        int DeleteTblProductInfo(Int32 idProduct, SqlConnection conn, SqlTransaction tran);

    }
}