using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPageElementsDAO
    {
        String SqlSelectQuery();
          List<TblPageElementsTO> SelectAllTblPageElements();
        List<TblPageElementsTO> SelectAllTblPageElements(int pageId);
        TblPageElementsTO SelectTblPageElements(Int32 idPageElement);
        List<TblPageElementsTO> ConvertDTToList(SqlDataReader tblPageElementsTODT);
        int InsertTblPageElements(TblPageElementsTO tblPageElementsTO);
        int InsertTblPageElements(TblPageElementsTO tblPageElementsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPageElementsTO tblPageElementsTO, SqlCommand cmdInsert);
          int UpdateTblPageElements(TblPageElementsTO tblPageElementsTO);
        int UpdateTblPageElements(TblPageElementsTO tblPageElementsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPageElementsTO tblPageElementsTO, SqlCommand cmdUpdate);
        int DeleteTblPageElements(Int32 idPageElement);
        int DeleteTblPageElements(Int32 idPageElement, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idPageElement, SqlCommand cmdDelete);

    }
}