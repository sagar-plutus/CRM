using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL.Interfaces
{
    public interface ITblPurchaseSchTcDtlsBL
    {
        List<TblPurchaseSchTcDtlsTO> SelectAllTblPurchaseSchTcDtls();
        List<TblPurchaseSchTcDtlsTO> SelectAllTblPurchaseSchTcDtlsList();
        TblPurchaseSchTcDtlsTO SelectTblPurchaseSchTcDtlsTO(Int32 idPurchasseSchTcDtls);
        List<TblPurchaseSchTcDtlsTO> SelectScheTcDtlsByRootScheduleId(String rootScheduleIds);
        int InsertTblPurchaseSchTcDtls(TblPurchaseSchTcDtlsTO tblPurchaseSchTcDtlsTO);
        int InsertTblPurchaseSchTcDtls(TblPurchaseSchTcDtlsTO tblPurchaseSchTcDtlsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseSchTcDtls(TblPurchaseSchTcDtlsTO tblPurchaseSchTcDtlsTO);
        int UpdateTblPurchaseSchTcDtls(TblPurchaseSchTcDtlsTO tblPurchaseSchTcDtlsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseSchTcDtls(Int32 idPurchasseSchTcDtls);
        int DeleteTblPurchaseSchTcDtls(Int32 idPurchasseSchTcDtls, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseSchTcDtlsTO> SelectAllScheTcDtls(String rootScheduleId);

        int UpdateIsActiveAgainstSch(Int32 rootScheduleId, Int32 isActive, SqlConnection conn, SqlTransaction tran);
    }
}
