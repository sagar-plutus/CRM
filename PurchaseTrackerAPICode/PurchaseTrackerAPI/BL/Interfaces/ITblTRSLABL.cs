using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.Models;

namespace PurchaseTrackerAPI.BL.Interfaces
{
     public  interface  ITblTRSLABL
    {
        DataTable SelectAllTblTRSLA();
        List<TblTRSLATO> SelectAllTblTRSLAList();
        TblTRSLATO SelectTblTRSLATO(Int32 idSLA);
        int InsertTblTRSLA(TblTRSLATO tblTRSLATO);
        int InsertTblTRSLA(TblTRSLATO tblTRSLATO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblTRSLA(TblTRSLATO tblTRSLATO);
        int UpdateTblTRSLA(TblTRSLATO tblTRSLATO, SqlConnection conn, SqlTransaction tran);
    }
}
