using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblUserVerDAO
    {
        String SqlSelectQuery();
        List<TblUserVerTO> SelectAllTblUserVer();
        List<TblUserVerTO> SelectTblUserVer(Int32 idUserVer);
        int InsertTblUserVer(TblUserVerTO tblUserVerTO);
        int InsertTblUserVer(TblUserVerTO tblUserVerTO, SqlConnection conn, SqlTransaction tran);
          int ExecuteInsertionCommand(TblUserVerTO tblUserVerTO, SqlCommand cmdInsert);
        int UpdateTblUserVer(TblUserVerTO tblUserVerTO);
        int UpdateTblUserVer(TblUserVerTO tblUserVerTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblUserVerTO tblUserVerTO, SqlCommand cmdUpdate);
        int DeleteTblUserVer(Int32 idUserVer);
        int DeleteTblUserVer(Int32 idUserVer, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idUserVer, SqlCommand cmdDelete);

    }
}