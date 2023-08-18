using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblUserExtDAO
    {
        String SqlSelectQuery();
        List<TblUserExtTO> SelectAllTblUserExt();
        TblUserExtTO SelectTblUserExt(Int32 userId);
        List<TblUserExtTO> ConvertDTToList(SqlDataReader tblUserExtTODT);
        int InsertTblUserExt(TblUserExtTO tblUserExtTO);
        int InsertTblUserExt(TblUserExtTO tblUserExtTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblUserExtTO tblUserExtTO, SqlCommand cmdInsert);
        int UpdateTblUserExt(TblUserExtTO tblUserExtTO);
        int UpdateTblUserExt(TblUserExtTO tblUserExtTO, SqlConnection conn, SqlTransaction tran);
          int ExecuteUpdationCommand(TblUserExtTO tblUserExtTO, SqlCommand cmdUpdate);
        int DeleteTblUserExt();
        int DeleteTblUserExt(SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(SqlCommand cmdDelete);

    }
}