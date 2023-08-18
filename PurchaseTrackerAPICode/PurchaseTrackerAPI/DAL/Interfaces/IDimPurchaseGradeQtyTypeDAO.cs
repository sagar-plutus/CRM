using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface IDimPurchaseGradeQtyTypeDAO
    {
        List<DimPurchaseGradeQtyTypeTO> SelectAllDimPurchaseGradeQtyType();
        List<DimPurchaseGradeQtyTypeTO> SelectDimPurchaseGradeQtyType(Int32 idPurchaseGradeQtyType);
        DataTable SelectAllDimPurchaseGradeQtyType(SqlConnection conn, SqlTransaction tran);
        int InsertDimPurchaseGradeQtyType(DimPurchaseGradeQtyTypeTO dimPurchaseGradeQtyTypeTO);
        int InsertDimPurchaseGradeQtyType(DimPurchaseGradeQtyTypeTO dimPurchaseGradeQtyTypeTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimPurchaseGradeQtyType(DimPurchaseGradeQtyTypeTO dimPurchaseGradeQtyTypeTO);
        int UpdateDimPurchaseGradeQtyType(DimPurchaseGradeQtyTypeTO dimPurchaseGradeQtyTypeTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimPurchaseGradeQtyType(Int32 idPurchaseGradeQtyType);
        int DeleteDimPurchaseGradeQtyType(Int32 idPurchaseGradeQtyType, SqlConnection conn, SqlTransaction tran);
    }
}
