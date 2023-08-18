using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblSmsDAO
    {
        String SqlSelectQuery();
        List<TblSmsTO> SelectAllTblSms();
        TblSmsTO SelectTblSms(Int32 idSms);
        List<TblSmsTO> ConvertDTToList(SqlDataReader tblSmsTODT);
        int InsertTblSms(TblSmsTO tblSmsTO);
        int InsertTblSms(TblSmsTO tblSmsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblSmsTO tblSmsTO, SqlCommand cmdInsert);
        int UpdateTblSms(TblSmsTO tblSmsTO);
        int UpdateTblSms(TblSmsTO tblSmsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblSmsTO tblSmsTO, SqlCommand cmdUpdate);
        int DeleteTblSms(Int32 idSms);
        int DeleteTblSms(Int32 idSms, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idSms, SqlCommand cmdDelete);

    }
}