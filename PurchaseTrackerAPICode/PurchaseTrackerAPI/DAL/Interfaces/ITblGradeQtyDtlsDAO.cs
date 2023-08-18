using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblGradeQtyDtlsDAO
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
        List<TblGradeQtyDtlsTO> SelectExistingGradeQtyDtls(TblGradeQtyDtlsTO tblGradeQtyDtlsTO, SqlConnection conn, SqlTransaction tran);

        DataTable SelectGradeQtyDetails(TblReportsTO tblReportsTO);
        DataTable GetUnloadingQty(DateTime fromDate, DateTime toDate, Int32 prodClassId, Int32 prodItemId, Int32 supervisorId);
    }
}
