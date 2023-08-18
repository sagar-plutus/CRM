using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblProdGstCodeDtlsBL
    {
        List<TblProdGstCodeDtlsTO> SelectAllTblProdGstCodeDtlsList(Int32 gstCodeId = 0);
        List<TblProdGstCodeDtlsTO> SelectAllTblProdGstCodeDtlsList(Int32 gstCodeId, SqlConnection conn, SqlTransaction tran);
        TblProdGstCodeDtlsTO SelectTblProdGstCodeDtlsTO(Int32 idProdGstCode);
        TblProdGstCodeDtlsTO SelectTblProdGstCodeDtlsTO(Int32 idProdGstCode, SqlConnection conn, SqlTransaction tran);
        List<TblProdGstCodeDtlsTO> SelectTblProdGstCodeDtlsTOList(String idProdGstCodes);
        List<TblProdGstCodeDtlsTO> SelectTblProdGstCodeDtlsTOList(String idProdGstCodes, SqlConnection conn, SqlTransaction tran);
        TblProdGstCodeDtlsTO SelectTblProdGstCodeDtlsTO(Int32 prodCatId, Int32 prodSpecId, Int32 materialId, Int32 prodItemId);
        TblProdGstCodeDtlsTO SelectTblProdGstCodeDtlsTO(Int32 prodCatId, Int32 prodSpecId, Int32 materialId, Int32 prodItemId, SqlConnection conn, SqlTransaction tran);
        int InsertTblProdGstCodeDtls(TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO);
        int InsertTblProdGstCodeDtls(TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblProdGstCodeDtls(TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO);
        int UpdateTblProdGstCodeDtls(TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblProdGstCodeDtls(Int32 idProdGstCode);
        int DeleteTblProdGstCodeDtls(Int32 idProdGstCode, SqlConnection conn, SqlTransaction tran);

    }
}