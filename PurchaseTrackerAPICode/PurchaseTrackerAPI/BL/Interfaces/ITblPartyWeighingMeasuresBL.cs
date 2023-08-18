using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPartyWeighingMeasuresBL
    {
        List<TblPartyWeighingMeasuresTO> SelectAllTblPartyWeighingMeasures();
        List<TblPartyWeighingMeasuresTO> SelectAllTblPartyWeighingMeasuresList();
        TblPartyWeighingMeasuresTO SelectTblPartyWeighingMeasuresTO(Int32 idPartyWeighingMeasures);
        TblPartyWeighingMeasuresTO SelectTblPartyWeighingMeasuresTOByPurSchedSummaryId(Int32 purchaseScheduleSummaryId,SqlConnection conn = null,SqlTransaction tran= null);
        int InsertTblPartyWeighingMeasures(TblPartyWeighingMeasuresTO tblPartyWeighingMeasuresTO);
        int InsertTblPartyWeighingMeasures(TblPartyWeighingMeasuresTO tblPartyWeighingMeasuresTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPartyWeighingMeasures(TblPartyWeighingMeasuresTO tblPartyWeighingMeasuresTO);
        int UpdateTblPartyWeighingMeasures(TblPartyWeighingMeasuresTO tblPartyWeighingMeasuresTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPartyWeighingMeasures(Int32 idPartyWeighingMeasures);
        int DeleteTblPartyWeighingMeasures(Int32 idPartyWeighingMeasures, SqlConnection conn, SqlTransaction tran);

        int DeleteAllPartyWeighingDtls(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran);

    }
}