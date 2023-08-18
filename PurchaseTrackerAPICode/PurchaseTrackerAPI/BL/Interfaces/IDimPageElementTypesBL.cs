using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface IDimPageElementTypesBL
    {
        List<DimPageElementTypesTO> SelectAllDimPageElementTypesList();
        DimPageElementTypesTO SelectDimPageElementTypesTO(Int32 idPageEleType);
        int InsertDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO);
        int InsertDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO);
        int UpdateDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimPageElementTypes(Int32 idPageEleType);
        int DeleteDimPageElementTypes(Int32 idPageEleType, SqlConnection conn, SqlTransaction tran);

    }
}