using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblOverdueDtlDAO
    {
        String SqlSelectQuery();
        List<TblOverdueDtlTO> SelectAllTblOverdueDtl();
        List<TblOverdueDtlTO> SelectAllTblOverdueDtl(String dealerIds);
        TblOverdueDtlTO SelectTblOverdueDtl(Int32 idOverdueDtl);
        List<TblOverdueDtlTO> ConvertDTToList(SqlDataReader tblOverdueDtlTODT);
        List<TblOverdueDtlTO> SelectTblOverdueDtlList(Int32 dealerId);
        int InsertTblOverdueDtl(TblOverdueDtlTO tblOverdueDtlTO);
        int InsertTblOverdueDtl(TblOverdueDtlTO tblOverdueDtlTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblOverdueDtlTO tblOverdueDtlTO, SqlCommand cmdInsert);
        int UpdateTblOverdueDtl(TblOverdueDtlTO tblOverdueDtlTO);
        int UpdateTblOverdueDtl(TblOverdueDtlTO tblOverdueDtlTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblOverdueDtlTO tblOverdueDtlTO, SqlCommand cmdUpdate);
        int DeleteTblOverdueDtl(Int32 idOverdueDtl);
        int DeleteTblOverdueDtl(Int32 idOverdueDtl, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idOverdueDtl, SqlCommand cmdDelete);
        int DeleteTblOverdueDtl(SqlConnection conn, SqlTransaction tran);

    }
}