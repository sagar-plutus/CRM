using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;

using PurchaseTrackerAPI.StaticStuff;

using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblLoginBL
    {
        List<TblLoginTO> SelectAllTblLoginList();
        TblLoginTO SelectTblLoginTO(Int32 idLogin);
        int InsertTblLogin(TblLoginTO tblLoginTO);
        int InsertTblLogin(TblLoginTO tblLoginTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage LogIn(TblUserTO tblUserTO);
          ResultMessage LogOut(TblUserTO tblUserTO);
          int UpdateTblLogin(TblLoginTO tblLoginTO);
          int UpdateTblLogin(TblLoginTO tblLoginTO, SqlConnection conn, SqlTransaction tran);
          int DeleteTblLogin(Int32 idLogin);
        int DeleteTblLogin(Int32 idLogin, SqlConnection conn, SqlTransaction tran);

    }
}