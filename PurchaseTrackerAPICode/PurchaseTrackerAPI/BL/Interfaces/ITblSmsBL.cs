using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblSmsBL
    {
        List<TblSmsTO> SelectAllTblSmsList();
        TblSmsTO SelectTblSmsTO(Int32 idSms);
        int InsertTblSms(TblSmsTO tblSmsTO);
        int InsertTblSms(TblSmsTO tblSmsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblSms(TblSmsTO tblSmsTO);
        int UpdateTblSms(TblSmsTO tblSmsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblSms(Int32 idSms);
        int DeleteTblSms(Int32 idSms, SqlConnection conn, SqlTransaction tran);

    }
}