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

namespace PurchaseTrackerAPI.BL.Interfaces
{
    public interface ITblTransferRequestBL
    {
        ResultMessage InsertTblTransferRequestData(TblTransferRequestTO tblTransferRequestTO); 
        TblTransferRequestTO SelectTblTransferRequestTO(Int32 idTransferRequest);
        int InsertTblTransferRequest(TblTransferRequestTO tblTransferRequestTO);
        int InsertTblTransferRequest(TblTransferRequestTO tblTransferRequestTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblTransferRequest(TblTransferRequestTO tblTransferRequestTO);
        int UpdateTblTransferRequest(TblTransferRequestTO tblTransferRequestTO, SqlConnection conn, SqlTransaction tran);
        List<TblTransferRequestTO> GetTransferRequestDtlList(InternalTransferFilterTO InternalTransferFilterTO);
        int UpdateTblTransferRequestScheduleQtyNStatus(TblTransferRequestTO tblTransferRequestTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblTransferRequestScheduleQty(TblTransferRequestTO tblTransferRequestTO, SqlConnection conn, SqlTransaction tran);

        ResultMessage InsertTblTransferRequestData(TblTransferRequestTO tblTransferRequestTO, SqlConnection conn, SqlTransaction tran);

        TblTransferRequestTO SelectTblTransferRequestTO(Int32 idTransferRequest, SqlConnection conn, SqlTransaction tran);

    }
}
