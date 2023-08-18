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
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.BL
{
    public interface ITblTRLoadingBL
    {

        DataTable SelectAllTblTRLoading();

        List<TblTRLoadingTO> SelectAllTblTRLoadingList();

        TblTRLoadingTO SelectTblTRLoadingTO(Int32 idLoading);

        int SelectNextLoadingId();

        TblTRLoadingTO SelectTblTRLoadingTO(Int32 idLoading, SqlConnection conn, SqlTransaction tran);

        List<TblTRLoadingTO> SelectAllTblTRLoadingList(Int32 statusId);

        List<TblTRLoadingTO> SelectAllTblTRLoadingList(String statusIds);

        List<TblTRLoadingTO> SelectAllTblTRLoadingList(String statusIds, DateTime fromDate, DateTime toDate);

        List<TblTRLoadingTO> SelectAllTblTRLoadingList(String statusIds, DateTime fromDate, DateTime toDate, TRLoadingFilterE tRLoadingFilterE);

        List<TblTRLoadingTO> SelectAllTblTRLoadingList(DateTime fromDate, DateTime toDate);

        List<TblTRLoadingTO> ConvertDTToList(DataTable tblTRLoadingTODT);

        int InsertTblTRLoading(TblTRLoadingTO tblTRLoadingTO);

        int InsertTblTRLoading(TblTRLoadingTO tblTRLoadingTO, int userId, ref string errorMsg);


        int InsertTblTRLoading(TblTRLoadingTO tblTRLoadingTO, SqlConnection conn, SqlTransaction tran);

        int UpdateTblTRLoading(TblTRLoadingTO tblTRLoadingTO);

        int UpdateTblTRLoading(TblTRLoadingTO tblTRLoadingTO, int userId, ref string errorMsg);

        int UpdateTblTRLoading(TblTRLoadingTO tblTRLoadingTO, SqlConnection conn, SqlTransaction tran);

        int UpdateTblTRLoadingStatus(TblTRLoadingTO tblTRLoadingTO, int userId, ref string errorMsg);

        int DeleteTblTRLoading(TblTRLoadingTO tblTRLoadingTO, int userId);

        int DeleteTblTRLoading(TblTRLoadingTO tblTRLoadingTO, SqlConnection conn, SqlTransaction tran);


    }
}
