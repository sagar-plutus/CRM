using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblEntityRangeDAO
    {
        String SqlSelectQuery();
        List<TblEntityRangeTO> SelectAllTblEntityRange();
        TblEntityRangeTO SelectTblEntityRange(Int32 idEntityRange);
        TblEntityRangeTO SelectEntityRangeFromInvoiceType(Int32 invoiceTypeId, int finYearId, SqlConnection conn, SqlTransaction tran);
        TblEntityRangeTO SelectEntityRangeFromInvoiceType(String entityName, int finYearId, SqlConnection conn, SqlTransaction tran);
        TblEntityRangeTO SelectTblEntityRangeByEntityName(string entityName);
        List<TblEntityRangeTO> ConvertDTToList(SqlDataReader tblEntityRangeTODT);
        TblEntityRangeTO SelectTblEntityRangeByEntityName(string entityName, int finYearId, SqlConnection conn, SqlTransaction tran);
        int InsertTblEntityRange(TblEntityRangeTO tblEntityRangeTO);
        int InsertTblEntityRange(TblEntityRangeTO tblEntityRangeTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblEntityRangeTO tblEntityRangeTO, SqlCommand cmdInsert);
        int UpdateTblEntityRange(TblEntityRangeTO tblEntityRangeTO);
        int UpdateTblEntityRange(TblEntityRangeTO tblEntityRangeTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblEntityRangeTO tblEntityRangeTO, SqlCommand cmdUpdate);
        int DeleteTblEntityRange(Int32 idEntityRange);
        int DeleteTblEntityRange(Int32 idEntityRange, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idEntityRange, SqlCommand cmdDelete);

    }
}