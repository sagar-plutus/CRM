using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.DashboardModels;

namespace PurchaseTrackerAPI.BL.Interfaces
{
    public interface ITblGradeQtyDtlsBL
    {
        List<TblGradeQtyDtlsTO> SelectAllTblGradeQtyDtls();
        List<TblGradeQtyDtlsTO> SelectTblGradeQtyDtls(Int32 idGradeQtyDtls);
        List<TblGradeQtyDtlsTO> SelectAllTblGradeQtyDtls(SqlConnection conn, SqlTransaction tran);
        int InsertTblGradeQtyDtls(TblGradeQtyDtlsTO tblGradeQtyDtlsTO);
        int InsertTblGradeQtyDtls(TblGradeQtyDtlsTO tblGradeQtyDtlsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblGradeQtyDtls(TblGradeQtyDtlsTO tblGradeQtyDtlsTO);
        int UpdateTblGradeQtyDtls(TblGradeQtyDtlsTO tblGradeQtyDtlsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblGradeQtyDtls(Int32 idGradeQtyDtls);
        int DeleteTblGradeQtyDtls(Int32 idGradeQtyDtls, SqlConnection conn, SqlTransaction tran);
        ResultMessage SavePurchaseGradeQtyDtls(List<TblGradeQtyDtlsTO> tblGradeQtyDtlsTOList, Int32 loginUserId);

        List<dynamic> SelectGradeQtyDetails(TblReportsTO tblReportsTO);


    }
}
