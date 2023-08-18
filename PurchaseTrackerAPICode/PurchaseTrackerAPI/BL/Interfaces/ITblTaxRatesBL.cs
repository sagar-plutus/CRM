using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblTaxRatesBL
    {
        List<TblTaxRatesTO> SelectAllTblTaxRatesList();
        TblTaxRatesTO SelectTblTaxRatesTO();
        List<TblTaxRatesTO> SelectAllTblTaxRatesList(Int32 idGstCode);
        List<TblTaxRatesTO> SelectAllTblTaxRatesList(Int32 idGstCode, SqlConnection conn, SqlTransaction tran);
        int InsertTblTaxRates(TblTaxRatesTO tblTaxRatesTO);
        int InsertTblTaxRates(TblTaxRatesTO tblTaxRatesTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblTaxRates(TblTaxRatesTO tblTaxRatesTO);
        int UpdateTblTaxRates(TblTaxRatesTO tblTaxRatesTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblTaxRates(Int32 idTaxRate);
        int DeleteTblTaxRates(Int32 idTaxRate, SqlConnection conn, SqlTransaction tran);

    }
}