using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblGradeWiseTargetQtyDAO
    {
        String SqlSelectQuery();
          List<TblGradeWiseTargetQtyTO> SelectAllTblGradeWiseTargetQty();
        List<TblGradeWiseTargetQtyTO> SelectTblGradeWiseTargetQty(Int32 idGradeWiseTargetQty);
        List<TblGradeWiseTargetQtyTO> SelectGradeWiseTargetQtyDtls(Int32 rateBandPurchaseId, Int32 pmId);
        List<TblGradeWiseTargetQtyTO> SelectGradeWiseTargetQtyDtls(Int32 rateBandPurchaseId, Int32 pmId, SqlConnection conn, SqlTransaction tran);
        List<TblGradeWiseTargetQtyTO> SelectAllTblGradeWiseTargetQty(SqlConnection conn, SqlTransaction tran);
        List<TblGradeWiseTargetQtyTO> ConvertDTToList(SqlDataReader tblGradeWiseTargetQtyTODT);
        int InsertTblGradeWiseTargetQty(TblGradeWiseTargetQtyTO tblGradeWiseTargetQtyTO);
        int InsertTblGradeWiseTargetQty(TblGradeWiseTargetQtyTO tblGradeWiseTargetQtyTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblGradeWiseTargetQtyTO tblGradeWiseTargetQtyTO, SqlCommand cmdInsert);
        int UpdateTblGradeWiseTargetQty(TblGradeWiseTargetQtyTO tblGradeWiseTargetQtyTO);
        int UpdateTblGradeWiseTargetQty(TblGradeWiseTargetQtyTO tblGradeWiseTargetQtyTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblGradeWiseTargetQtyTO tblGradeWiseTargetQtyTO, SqlCommand cmdUpdate);
        int DeleteTblGradeWiseTargetQty(Int32 idGradeWiseTargetQty);
        int DeleteTblGradeWiseTargetQty(Int32 idGradeWiseTargetQty, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idGradeWiseTargetQty, SqlCommand cmdDelete);

    }
}