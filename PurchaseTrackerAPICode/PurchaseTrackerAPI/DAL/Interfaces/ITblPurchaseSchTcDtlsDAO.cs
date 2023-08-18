using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseSchTcDtlsDAO
    {
        String SqlSelectQuery();
        List<TblPurchaseSchTcDtlsTO> SelectAllTblPurchaseSchTcDtls();
        List<TblPurchaseSchTcDtlsTO> SelectTblPurchaseSchTcDtls(Int32 idPurchasseSchTcDtls);

        List<TblPurchaseSchTcDtlsTO> SelectScheTcDtlsByRootScheduleId(String rootScheduleIds);
        List<TblPurchaseSchTcDtlsTO> SelectAllTblPurchaseSchTcDtls(SqlConnection conn, SqlTransaction tran);
        int InsertTblPurchaseSchTcDtls(TblPurchaseSchTcDtlsTO tblPurchaseSchTcDtlsTO);
        int InsertTblPurchaseSchTcDtls(TblPurchaseSchTcDtlsTO tblPurchaseSchTcDtlsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPurchaseSchTcDtlsTO tblPurchaseSchTcDtlsTO, SqlCommand cmdInsert);
        int UpdateTblPurchaseSchTcDtls(TblPurchaseSchTcDtlsTO tblPurchaseSchTcDtlsTO);
        int UpdateTblPurchaseSchTcDtls(TblPurchaseSchTcDtlsTO tblPurchaseSchTcDtlsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPurchaseSchTcDtlsTO tblPurchaseSchTcDtlsTO, SqlCommand cmdUpdate);
        int DeleteTblPurchaseSchTcDtls(Int32 idPurchasseSchTcDtls);
        int DeleteTblPurchaseSchTcDtls(Int32 idPurchasseSchTcDtls, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idPurchasseSchTcDtls, SqlCommand cmdDelete);
        int UpdateIsActiveAgainstSch(Int32 rootScheduleId, Int32 isActive, SqlConnection conn, SqlTransaction tran);
    }
}
