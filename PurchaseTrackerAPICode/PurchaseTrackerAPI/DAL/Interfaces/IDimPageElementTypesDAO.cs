using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface IDimPageElementTypesDAO
    {
        String SqlSelectQuery();
          List<DimPageElementTypesTO> SelectAllDimPageElementTypes();
        DimPageElementTypesTO SelectDimPageElementTypes(Int32 idPageEleType);
        List<DimPageElementTypesTO> ConvertDTToList(SqlDataReader dimPageElementTypesTODT);
        int InsertDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO);
        int InsertDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(DimPageElementTypesTO dimPageElementTypesTO, SqlCommand cmdInsert);
        int UpdateDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO);
        int UpdateDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(DimPageElementTypesTO dimPageElementTypesTO, SqlCommand cmdUpdate);
        int DeleteDimPageElementTypes(Int32 idPageEleType);
        int DeleteDimPageElementTypes(Int32 idPageEleType, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idPageEleType, SqlCommand cmdDelete);

    }
}