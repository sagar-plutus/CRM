using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblSysElementsDAO
    {
        String SqlSelectQuery();
        List<TblSysElementsTO> SelectAllTblSysElements(int menuPageId);
        TblSysElementsTO SelectTblSysElements(Int32 idSysElement);
        List<TblSysElementsTO> ConvertDTToList(SqlDataReader tblSysElementsTODT);
        int InsertTblSysElements(TblSysElementsTO tblSysElementsTO);
          int InsertTblSysElements(TblSysElementsTO tblSysElementsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblSysElementsTO tblSysElementsTO, SqlCommand cmdInsert);
        int UpdateTblSysElements(TblSysElementsTO tblSysElementsTO);
        int UpdateTblSysElements(TblSysElementsTO tblSysElementsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblSysElementsTO tblSysElementsTO, SqlCommand cmdUpdate);
        int DeleteTblSysElements(Int32 idSysElement);
        int DeleteTblSysElements(Int32 idSysElement, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idSysElement, SqlCommand cmdDelete);

    }
}