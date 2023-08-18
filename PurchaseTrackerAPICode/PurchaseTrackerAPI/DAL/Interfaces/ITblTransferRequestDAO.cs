using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblTransferRequestDAO
    {
        TblTransferRequestTO SelectTblTransferRequest(Int32 idTransferRequest);
        TblTransferRequestTO SelectTblTransferRequest(Int32 idTransferRequest,SqlConnection conn,SqlTransaction tran);

        DataTable SelectAllTblTransferRequest(SqlConnection conn, SqlTransaction tran);
        int InsertTblTransferRequest(TblTransferRequestTO tblTransferRequestTO);
        int InsertTblTransferRequest(TblTransferRequestTO tblTransferRequestTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblTransferRequest(TblTransferRequestTO tblTransferRequestTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblTransferRequestScheduleQtyNStatus(TblTransferRequestTO tblTransferRequestTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblTransferRequestScheduleQty(TblTransferRequestTO tblTransferRequestTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblTransferRequest(TblTransferRequestTO tblTransferRequestTO);
        List<TblTransferRequestTO> GetTransferRequestDtlList(InternalTransferFilterTO InternalTransferFilterTO);
        List<TblTransferRequestTO> SelectTblTransferRequest(String idTransferRequestStr);
    }
}
