using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface IDimQualitySampleTypeDAO
    {
        String SqlSelectQuery();
        List<DimQualitySampleTypeTO> SelectAllDimQualitySampleType();
        List<DimQualitySampleTypeTO> SelectDimQualitySampleType(Int32 idQualitySampleType);
        DataTable SelectAllDimQualitySampleType(SqlConnection conn, SqlTransaction tran);
        List<DimQualitySampleTypeTO> ConvertDTToList(SqlDataReader dimQualitySampleTypeTODT);
        int InsertDimQualitySampleType(DimQualitySampleTypeTO dimQualitySampleTypeTO);
        int InsertDimQualitySampleType(DimQualitySampleTypeTO dimQualitySampleTypeTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(DimQualitySampleTypeTO dimQualitySampleTypeTO, SqlCommand cmdInsert);
        int UpdateDimQualitySampleType(DimQualitySampleTypeTO dimQualitySampleTypeTO);
        int UpdateDimQualitySampleType(DimQualitySampleTypeTO dimQualitySampleTypeTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(DimQualitySampleTypeTO dimQualitySampleTypeTO, SqlCommand cmdUpdate);
        int DeleteDimQualitySampleType(Int32 idQualitySampleType);
        int DeleteDimQualitySampleType(Int32 idQualitySampleType, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idQualitySampleType, SqlCommand cmdDelete);

    }
}