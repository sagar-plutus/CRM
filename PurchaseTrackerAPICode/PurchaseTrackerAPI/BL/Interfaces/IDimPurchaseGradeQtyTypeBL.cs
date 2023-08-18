using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.BL.Interfaces
{
    public interface IDimPurchaseGradeQtyTypeBL
    {
        List<DimPurchaseGradeQtyTypeTO> SelectAllDimPurchaseGradeQtyType();
        List<DimPurchaseGradeQtyTypeTO> SelectAllDimPurchaseGradeQtyTypeList();
        List<DimPurchaseGradeQtyTypeTO> SelectDimPurchaseGradeQtyTypeTO(Int32 idPurchaseGradeQtyType);
        int InsertDimPurchaseGradeQtyType(DimPurchaseGradeQtyTypeTO dimPurchaseGradeQtyTypeTO);
        int InsertDimPurchaseGradeQtyType(DimPurchaseGradeQtyTypeTO dimPurchaseGradeQtyTypeTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimPurchaseGradeQtyType(DimPurchaseGradeQtyTypeTO dimPurchaseGradeQtyTypeTO);
        int UpdateDimPurchaseGradeQtyType(DimPurchaseGradeQtyTypeTO dimPurchaseGradeQtyTypeTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimPurchaseGradeQtyType(Int32 idPurchaseGradeQtyType);

        int DeleteDimPurchaseGradeQtyType(Int32 idPurchaseGradeQtyType, SqlConnection conn, SqlTransaction tran);


    }
}
