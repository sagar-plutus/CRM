using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblTaxRatesDAO
    {
        String SqlSelectQuery();
        List<TblTaxRatesTO> SelectAllTblTaxRates();
        List<TblTaxRatesTO> SelectAllTblTaxRates(int idGstCode, SqlConnection conn, SqlTransaction tran);
        TblTaxRatesTO SelectTblTaxRates();
        List<TblTaxRatesTO> ConvertDTToList(SqlDataReader tblTaxRatesTODT);
        int InsertTblTaxRates(TblTaxRatesTO tblTaxRatesTO);
        int InsertTblTaxRates(TblTaxRatesTO tblTaxRatesTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblTaxRatesTO tblTaxRatesTO, SqlCommand cmdInsert);
        int UpdateTblTaxRates(TblTaxRatesTO tblTaxRatesTO);
        int UpdateTblTaxRates(TblTaxRatesTO tblTaxRatesTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblTaxRatesTO tblTaxRatesTO, SqlCommand cmdUpdate);
        int DeleteTblTaxRates(Int32 idTaxRate);
        int DeleteTblTaxRates(Int32 idTaxRate, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idTaxRate, SqlCommand cmdDelete);

    }
}