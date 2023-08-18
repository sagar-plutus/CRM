using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblWeighingMeasuresDAO
    {
        String SqlSelectQuery();
        List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasures();
        List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasuresListByLoadingId(int loadingId);
        List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasuresListByLoadingId(int loadingId, SqlConnection conn, SqlTransaction tran);
        List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasuresListByLoadingIds(string loadingIds, Boolean isUnloading);
        List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasuresListByVehicleNo(string vehicleNo);
        TblWeighingMeasuresTO SelectTblWeighingMeasures(Int32 idWeightMeasure);
        List<TblWeighingMeasuresTO> ConvertDTToList(SqlDataReader tblWeighingMeasuresTODT);
        int InsertTblWeighingMeasures(TblWeighingMeasuresTO tblWeighingMeasuresTO);
        int InsertTblWeighingMeasures(TblWeighingMeasuresTO tblWeighingMeasuresTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblWeighingMeasuresTO tblWeighingMeasuresTO, SqlCommand cmdInsert);
        int UpdateTblWeighingMeasures(TblWeighingMeasuresTO tblWeighingMeasuresTO);
        int UpdateTblWeighingMeasures(TblWeighingMeasuresTO tblWeighingMeasuresTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblWeighingMeasuresTO tblWeighingMeasuresTO, SqlCommand cmdUpdate);
        int DeleteTblWeighingMeasures(Int32 idWeightMeasure);
        int DeleteTblWeighingMeasures(Int32 idWeightMeasure, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idWeightMeasure, SqlCommand cmdDelete);

    }
}