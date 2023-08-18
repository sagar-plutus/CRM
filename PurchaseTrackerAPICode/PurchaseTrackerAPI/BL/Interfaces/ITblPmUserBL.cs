using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPmUserBL
    {
        List<TblPmUserTO> SelectAllTblPmUser();
        List<TblPmUserTO> SelectAllTblPmUserList();
        TblPmUserTO SelectTblPmUserTO(Int32 idPmUser);
        int InsertTblPmUser(TblPmUserTO tblPmUserTO);
        int InsertTblPmUser(TblPmUserTO tblPmUserTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPmUser(TblPmUserTO tblPmUserTO);
        int UpdateTblPmUser(TblPmUserTO tblPmUserTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPmUser(Int32 idPmUser);
        int DeleteTblPmUser(Int32 idPmUser, SqlConnection conn, SqlTransaction tran);

    }
}