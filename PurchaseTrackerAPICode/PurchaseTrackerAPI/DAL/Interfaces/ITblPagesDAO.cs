using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPagesDAO
    {
        String SqlSelectQuery();
        List<TblPagesTO> SelectAllTblPages();
        List<TblPagesTO> SelectAllTblPages(int moduleId);
        TblPagesTO SelectTblPages(Int32 idPage);
        List<TblPagesTO> ConvertDTToList(SqlDataReader tblPagesTODT);
        int InsertTblPages(TblPagesTO tblPagesTO);
        int InsertTblPages(TblPagesTO tblPagesTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPagesTO tblPagesTO, SqlCommand cmdInsert);
        int UpdateTblPages(TblPagesTO tblPagesTO);
        int UpdateTblPages(TblPagesTO tblPagesTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPagesTO tblPagesTO, SqlCommand cmdUpdate);
        int DeleteTblPages(Int32 idPage);
        int DeleteTblPages(Int32 idPage, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idPage, SqlCommand cmdDelete);

    }
}