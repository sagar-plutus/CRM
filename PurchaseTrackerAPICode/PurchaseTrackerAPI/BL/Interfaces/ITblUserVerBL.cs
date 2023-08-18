using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblUserVerBL
    {
        List<TblUserVerTO> SelectAllTblUserVer();
        List<TblUserVerTO> SelectAllTblUserVerList();
        TblUserVerTO SelectTblUserVerTO(Int32 idUserVer);
        int InsertTblUserVer(TblUserVerTO tblUserVerTO);
        int InsertTblUserVer(TblUserVerTO tblUserVerTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblUserVer(TblUserVerTO tblUserVerTO);
        int UpdateTblUserVer(TblUserVerTO tblUserVerTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblUserVer(Int32 idUserVer);
        int DeleteTblUserVer(Int32 idUserVer, SqlConnection conn, SqlTransaction tran);

    }
}