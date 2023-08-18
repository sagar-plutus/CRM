using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPartyWeighingMeasuresDAO
    {
        String SqlSelectQuery();
        List<TblPartyWeighingMeasuresTO> SelectAllTblPartyWeighingMeasures();
        TblPartyWeighingMeasuresTO SelectTblPartyWeighingMeasures(Int32 idPartyWeighingMeasures);
        TblPartyWeighingMeasuresTO SelectTblPartyWeighingMeasuresTOByPurSchedSummaryId(Int32 purchaseScheduleSummaryId);

        TblPartyWeighingMeasuresTO SelectTblPartyWeighingMeasuresTOByPurSchedSummaryId(Int32 purchaseScheduleSummaryId,SqlConnection conn,SqlTransaction tran);
        List<TblPartyWeighingMeasuresTO> SelectAllTblPartyWeighingMeasures(SqlConnection conn, SqlTransaction tran);
        List<TblPartyWeighingMeasuresTO> ConvertDTToList(SqlDataReader tblPartyWeighingMeasuresTODT);
        int InsertTblPartyWeighingMeasures(TblPartyWeighingMeasuresTO tblPartyWeighingMeasuresTO);
        int InsertTblPartyWeighingMeasures(TblPartyWeighingMeasuresTO tblPartyWeighingMeasuresTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPartyWeighingMeasuresTO tblPartyWeighingMeasuresTO, SqlCommand cmdInsert);
        int UpdateTblPartyWeighingMeasures(TblPartyWeighingMeasuresTO tblPartyWeighingMeasuresTO);
        int UpdateTblPartyWeighingMeasures(TblPartyWeighingMeasuresTO tblPartyWeighingMeasuresTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPartyWeighingMeasuresTO tblPartyWeighingMeasuresTO, SqlCommand cmdUpdate);
        int DeleteTblPartyWeighingMeasures(Int32 idPartyWeighingMeasures);
        int DeleteTblPartyWeighingMeasures(Int32 idPartyWeighingMeasures, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idPartyWeighingMeasures, SqlCommand cmdDelete);
        int DeleteAllPartyWeighingDtls(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran);


    }
}