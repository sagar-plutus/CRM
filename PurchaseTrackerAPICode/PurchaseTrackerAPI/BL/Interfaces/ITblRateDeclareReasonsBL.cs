using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblRateDeclareReasonsBL
    {
        List<TblRateDeclareReasonsTO> SelectAllTblRateDeclareReasonsList();
        TblRateDeclareReasonsTO SelectTblRateDeclareReasonsTO(Int32 idRateReason);
        int InsertTblRateDeclareReasons(TblRateDeclareReasonsTO tblRateDeclareReasonsTO);
        int InsertTblRateDeclareReasons(TblRateDeclareReasonsTO tblRateDeclareReasonsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblRateDeclareReasons(TblRateDeclareReasonsTO tblRateDeclareReasonsTO);
          int UpdateTblRateDeclareReasons(TblRateDeclareReasonsTO tblRateDeclareReasonsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblRateDeclareReasons(Int32 idRateReason);
        int DeleteTblRateDeclareReasons(Int32 idRateReason, SqlConnection conn, SqlTransaction tran);

    }
}