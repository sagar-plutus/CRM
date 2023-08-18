using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblQualityPhaseDtlsBL
    {
         int DeleteAllQualityPhaseDtlsAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran);
            List<TblQualityPhaseDtlsTO> SelectAllTblQualityPhaseDtls();
        List<TblQualityPhaseDtlsTO> SelectAllTblQualityPhaseDtlsList();
        List<TblQualityPhaseDtlsTO> SelectAllTblQualityPhaseDtlsList(Int32 tblQualityPhaseId);
        TblQualityPhaseDtlsTO SelectTblQualityPhaseDtlsTO(Int32 idTblQualityPhaseDtls);
        int InsertTblQualityPhaseDtls(TblQualityPhaseDtlsTO tblQualityPhaseDtlsTO);
        int InsertTblQualityPhaseDtls(TblQualityPhaseDtlsTO tblQualityPhaseDtlsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblQualityPhaseDtls(TblQualityPhaseDtlsTO tblQualityPhaseDtlsTO);
        int UpdateTblQualityPhaseDtls(TblQualityPhaseDtlsTO tblQualityPhaseDtlsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblQualityPhaseDtls(Int32 idTblQualityPhaseDtls);
        int DeleteTblQualityPhaseDtls(Int32 idTblQualityPhaseDtls, SqlConnection conn, SqlTransaction tran);

    }
}