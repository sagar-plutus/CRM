using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblUserExtBL
    {
        List<TblUserExtTO> SelectAllTblUserExtList();
        TblUserExtTO SelectTblUserExtTO(Int32 userId);
        int InsertTblUserExt(TblUserExtTO tblUserExtTO);
        int InsertTblUserExt(TblUserExtTO tblUserExtTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblUserExt(TblUserExtTO tblUserExtTO);
        int UpdateTblUserExt(TblUserExtTO tblUserExtTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblUserExt();
        int DeleteTblUserExt(SqlConnection conn, SqlTransaction tran);

    }
}