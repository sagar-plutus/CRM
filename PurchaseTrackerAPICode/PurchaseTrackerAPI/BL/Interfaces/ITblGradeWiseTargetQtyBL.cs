using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblGradeWiseTargetQtyBL
    {
        List<TblGradeWiseTargetQtyTO> SelectAllTblGradeWiseTargetQty();
        List<TblGradeWiseTargetQtyTO> SelectAllTblGradeWiseTargetQtyList();
        TblGradeWiseTargetQtyTO SelectTblGradeWiseTargetQtyTO(Int32 idGradeWiseTargetQty);
        List<TblGradeWiseTargetQtyTO> SelectGradeWiseTargetQtyDtls(Int32 rateBandPurchaseId, Int32 pmId);
        List<TblGradeWiseTargetQtyTO> SelectGradeWiseTargetQtyDtls(Int32 rateBandPurchaseId, Int32 pmId, SqlConnection conn, SqlTransaction tran);
          int InsertTblGradeWiseTargetQty(TblGradeWiseTargetQtyTO tblGradeWiseTargetQtyTO);
        int InsertTblGradeWiseTargetQty(TblGradeWiseTargetQtyTO tblGradeWiseTargetQtyTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblGradeWiseTargetQty(TblGradeWiseTargetQtyTO tblGradeWiseTargetQtyTO);
        int UpdateTblGradeWiseTargetQty(TblGradeWiseTargetQtyTO tblGradeWiseTargetQtyTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblGradeWiseTargetQty(Int32 idGradeWiseTargetQty);
          int DeleteTblGradeWiseTargetQty(Int32 idGradeWiseTargetQty, SqlConnection conn, SqlTransaction tran);

    }
}