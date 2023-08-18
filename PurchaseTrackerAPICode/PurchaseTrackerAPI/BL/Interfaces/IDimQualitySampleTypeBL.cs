using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface IDimQualitySampleTypeBL
    {
        List<DimQualitySampleTypeTO> SelectAllDimQualitySampleType();
        List<DimQualitySampleTypeTO> SelectAllDimQualitySampleTypeList();
          DimQualitySampleTypeTO SelectDimQualitySampleTypeTO(Int32 idQualitySampleType);
        int InsertDimQualitySampleType(DimQualitySampleTypeTO dimQualitySampleTypeTO);
        int InsertDimQualitySampleType(DimQualitySampleTypeTO dimQualitySampleTypeTO, SqlConnection conn, SqlTransaction tran);
          int UpdateDimQualitySampleType(DimQualitySampleTypeTO dimQualitySampleTypeTO);
        int UpdateDimQualitySampleType(DimQualitySampleTypeTO dimQualitySampleTypeTO, SqlConnection conn, SqlTransaction tran);
          int DeleteDimQualitySampleType(Int32 idQualitySampleType);
          int DeleteDimQualitySampleType(Int32 idQualitySampleType, SqlConnection conn, SqlTransaction tran);

    }
}