using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblRateDeclareReasonsDAO
    {
        String SqlSelectQuery();
        List<TblRateDeclareReasonsTO> SelectAllTblRateDeclareReasons();
        TblRateDeclareReasonsTO SelectTblRateDeclareReasons(Int32 idRateReason);
        List<TblRateDeclareReasonsTO> ConvertDTToList(SqlDataReader tblRateDeclareReasonsTODT);
        int InsertTblRateDeclareReasons(TblRateDeclareReasonsTO tblRateDeclareReasonsTO);
        int InsertTblRateDeclareReasons(TblRateDeclareReasonsTO tblRateDeclareReasonsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblRateDeclareReasonsTO tblRateDeclareReasonsTO, SqlCommand cmdInsert);
        int UpdateTblRateDeclareReasons(TblRateDeclareReasonsTO tblRateDeclareReasonsTO);
        int UpdateTblRateDeclareReasons(TblRateDeclareReasonsTO tblRateDeclareReasonsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblRateDeclareReasonsTO tblRateDeclareReasonsTO, SqlCommand cmdUpdate);
        int DeleteTblRateDeclareReasons(Int32 idRateReason);
        int DeleteTblRateDeclareReasons(Int32 idRateReason, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idRateReason, SqlCommand cmdDelete);

    }
}