using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblTRSLADAO
    {
        DataTable SelectAllTblTRSLA();
        DataTable SelectTblTRSLA(Int32 idSLA);
        DataTable SelectAllTblTRSLA(SqlConnection conn, SqlTransaction tran);
        int InsertTblTRSLA(TblTRSLATO tblTRSLATO);
        int InsertTblTRSLA(TblTRSLATO tblTRSLATO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblTRSLA(TblTRSLATO tblTRSLATO);
        int UpdateTblTRSLA(TblTRSLATO tblTRSLATO, SqlConnection conn, SqlTransaction tran);
    }
}
