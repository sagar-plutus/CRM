using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseVehFreightDtlsBL
    {
        #region Selection
        List<TblPurchaseVehFreightDtlsTO> SelectAllTblPurchaseVehFreightDtls();

        List<TblPurchaseVehFreightDtlsTO> SelectAllTblPurchaseVehFreightDtlsList();
        TblPurchaseVehFreightDtlsTO SelectTblPurchaseVehFreightDtlsTO(Int32 idPurchaseVehFreightDtls);

        List<TblPurchaseVehFreightDtlsTO> SelectFreightDtlsByPurchaseScheduleId(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseVehFreightDtlsTO> SelectFreightDtlsByPurchaseScheduleId(Int32 purchaseScheduleId);

        #endregion

        #region Insertion
        int InsertTblPurchaseVehFreightDtls(TblPurchaseVehFreightDtlsTO tblPurchaseVehFreightDtlsTO);
        int InsertTblPurchaseVehFreightDtls(TblPurchaseVehFreightDtlsTO tblPurchaseVehFreightDtlsTO, SqlConnection conn, SqlTransaction tran);


        #endregion

        #region Updation
        int UpdateTblPurchaseVehFreightDtls(TblPurchaseVehFreightDtlsTO tblPurchaseVehFreightDtlsTO);
        int UpdateTblPurchaseVehFreightDtls(TblPurchaseVehFreightDtlsTO tblPurchaseVehFreightDtlsTO, SqlConnection conn, SqlTransaction tran);

        #endregion

        #region Deletion
        int DeleteTblPurchaseVehFreightDtls(Int32 idPurchaseVehFreightDtls);
        int DeleteTblPurchaseVehFreightDtls(Int32 idPurchaseVehFreightDtls, SqlConnection conn, SqlTransaction tran);

        int DeletePurchaseVehFreightDtls(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran);

        #endregion
    }
}