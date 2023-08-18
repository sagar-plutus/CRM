using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblLoginDAO
    {
        String SqlSelectQuery();
        List<TblLoginTO> SelectAllTblLogin();
        TblLoginTO SelectTblLogin(Int32 idLogin);
        List<TblLoginTO> ConvertDTToList(SqlDataReader tblLoginTODT);
        int InsertTblLogin(TblLoginTO tblLoginTO);
        int InsertTblLogin(TblLoginTO tblLoginTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblLoginTO tblLoginTO, SqlCommand cmdInsert);
        int UpdateTblLogin(TblLoginTO tblLoginTO);
        int UpdateTblLogin(TblLoginTO tblLoginTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblLoginTO tblLoginTO, SqlCommand cmdUpdate);
        int DeleteTblLogin(Int32 idLogin);
        int DeleteTblLogin(Int32 idLogin, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idLogin, SqlCommand cmdDelete);

    }
}