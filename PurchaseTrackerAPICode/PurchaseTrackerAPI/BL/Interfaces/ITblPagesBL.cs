using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPagesBL
    {
        List<TblPagesTO> SelectAllTblPagesList(int moduleId);
        List<TblPagesTO> SelectAllTblPagesList();
        TblPagesTO SelectTblPagesTO(Int32 idPage);
        int InsertTblPages(TblPagesTO tblPagesTO);
        int InsertTblPages(TblPagesTO tblPagesTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPages(TblPagesTO tblPagesTO);
        int UpdateTblPages(TblPagesTO tblPagesTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPages(Int32 idPage);
        int DeleteTblPages(Int32 idPage, SqlConnection conn, SqlTransaction tran);

    }
}