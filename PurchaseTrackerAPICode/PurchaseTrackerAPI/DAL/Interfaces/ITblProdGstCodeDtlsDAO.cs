using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblProdGstCodeDtlsDAO
    {
        String SqlSelectQuery();
        List<TblProdGstCodeDtlsTO> SelectAllTblProdGstCodeDtls(Int32 gstCodeId, SqlConnection conn, SqlTransaction tran);
        TblProdGstCodeDtlsTO SelectTblProdGstCodeDtls(Int32 idProdGstCode, SqlConnection conn, SqlTransaction tran);
        List<TblProdGstCodeDtlsTO> SelectTblProdGstCodeDtls(String idProdGstCodes, SqlConnection conn, SqlTransaction tran);
        TblProdGstCodeDtlsTO SelectTblProdGstCodeDtls(Int32 prodCatId, Int32 prodSpecId, Int32 materialId, Int32 prodItemId, SqlConnection conn, SqlTransaction tran);
        List<TblProdGstCodeDtlsTO> ConvertDTToList(SqlDataReader tblProdGstCodeDtlsTODT);
        int InsertTblProdGstCodeDtls(TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO);
        int InsertTblProdGstCodeDtls(TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO, SqlCommand cmdInsert);
        int UpdateTblProdGstCodeDtls(TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO);
        int UpdateTblProdGstCodeDtls(TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO, SqlCommand cmdUpdate);
        int DeleteTblProdGstCodeDtls(Int32 idProdGstCode);
        int DeleteTblProdGstCodeDtls(Int32 idProdGstCode, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idProdGstCode, SqlCommand cmdDelete);

    }
}