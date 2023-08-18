using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;

namespace PurchaseTrackerAPI.BL
{
    public interface ITblTRLoadingHistoryBL 
    {
        DataTable SelectAllTblTRLoadingHistory();
         List<TblTRLoadingHistoryTO> SelectAllTblTRLoadingHistoryList();
          TblTRLoadingHistoryTO SelectTblTRLoadingHistoryTO(Int32 idLoadingHistory);
          List<TblTRLoadingHistoryTO> ConvertDTToList(DataTable tblTRLoadingHistoryTODT );
          int InsertTblTRLoadingHistory(TblTRLoadingHistoryTO tblTRLoadingHistoryTO);
          int InsertTblTRLoadingHistory(TblTRLoadingHistoryTO tblTRLoadingHistoryTO, SqlConnection conn, SqlTransaction tran);
          int UpdateTblTRLoadingHistory(TblTRLoadingHistoryTO tblTRLoadingHistoryTO);
         int UpdateTblTRLoadingHistory(TblTRLoadingHistoryTO tblTRLoadingHistoryTO, SqlConnection conn, SqlTransaction tran);
          int DeleteTblTRLoadingHistory(Int32 idLoadingHistory);
         int DeleteTblTRLoadingHistory(Int32 idLoadingHistory, SqlConnection conn, SqlTransaction tran);
       
        
    }
}
