using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblOtherTaxesDAO
    {
        String SqlSelectQuery();
        List<TblOtherTaxesTO> SelectAllTblOtherTaxes();
        TblOtherTaxesTO SelectTblOtherTaxes(Int32 idOtherTax);
        List<TblOtherTaxesTO> ConvertDTToList(SqlDataReader tblOtherTaxesTODT);
        int InsertTblOtherTaxes(TblOtherTaxesTO tblOtherTaxesTO);
        int InsertTblOtherTaxes(TblOtherTaxesTO tblOtherTaxesTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblOtherTaxesTO tblOtherTaxesTO, SqlCommand cmdInsert);
        int UpdateTblOtherTaxes(TblOtherTaxesTO tblOtherTaxesTO);
        int UpdateTblOtherTaxes(TblOtherTaxesTO tblOtherTaxesTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblOtherTaxesTO tblOtherTaxesTO, SqlCommand cmdUpdate);
        int DeleteTblOtherTaxes(Int32 idOtherTax);
        int DeleteTblOtherTaxes(Int32 idOtherTax, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idOtherTax, SqlCommand cmdDelete);

    }
}