using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblLocationBL
    {
        List<TblLocationTO> SelectAllTblLocationList();
        TblLocationTO SelectTblLocationTO(Int32 idLocation);
        int InsertTblLocation(TblLocationTO tblLocationTO);
        int InsertTblLocation(TblLocationTO tblLocationTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblLocation(TblLocationTO tblLocationTO);
        int UpdateTblLocation(TblLocationTO tblLocationTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblLocation(Int32 idLocation);
        int DeleteTblLocation(Int32 idLocation, SqlConnection conn, SqlTransaction tran);

    }
}