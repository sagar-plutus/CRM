using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblAlertActionDtlDAO
    {
        String SqlSelectQuery();
        List<TblAlertActionDtlTO> SelectAllTblAlertActionDtl();
        TblAlertActionDtlTO SelectTblAlertActionDtl(Int32 idAlertActionDtl);
        TblAlertActionDtlTO SelectTblAlertActionDtl(Int32 alertInstanceId, Int32 userId, SqlConnection conn, SqlTransaction tran);
        List<TblAlertActionDtlTO> SelectAllTblAlertActionDtl(Int32 userId);
        List<TblAlertActionDtlTO> ConvertDTToList(SqlDataReader tblAlertActionDtlTODT);
        int InsertTblAlertActionDtl(TblAlertActionDtlTO tblAlertActionDtlTO);
        int InsertTblAlertActionDtl(TblAlertActionDtlTO tblAlertActionDtlTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblAlertActionDtlTO tblAlertActionDtlTO, SqlCommand cmdInsert);
        int UpdateTblAlertActionDtl(TblAlertActionDtlTO tblAlertActionDtlTO);
        int UpdateTblAlertActionDtl(TblAlertActionDtlTO tblAlertActionDtlTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblAlertActionDtlTO tblAlertActionDtlTO, SqlCommand cmdUpdate);
        int DeleteTblAlertActionDtl(Int32 idAlertActionDtl);
        int DeleteTblAlertActionDtl(Int32 idAlertActionDtl, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idAlertActionDtl, SqlCommand cmdDelete);

    }
}