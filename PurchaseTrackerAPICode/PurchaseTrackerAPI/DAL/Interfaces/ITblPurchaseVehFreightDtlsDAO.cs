using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseVehFreightDtlsDAO
    {
        String SqlSelectQuery();

        #region Selection
        List<TblPurchaseVehFreightDtlsTO> SlectAllTblPurchaseVehFreightDtls();
       
        List<TblPurchaseVehFreightDtlsTO> SelectTblPurchaseVehFreightDtls(Int32 idPurchaseVehFreightDtls);

        List<TblPurchaseVehFreightDtlsTO> SelectAllTblPurchaseVehFreightDtls(SqlConnection conn, SqlTransaction tran);
       
        List<TblPurchaseVehFreightDtlsTO> ConvertDTToList(SqlDataReader tblPurchaseVehFreightDtlsTODT);

        List<TblPurchaseVehFreightDtlsTO> SelectFreightDtlsByPurchaseScheduleId(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseVehFreightDtlsTO> SelectFreightDtlsByPurchaseScheduleId(Int32 purchaseScheduleId);

        #endregion

        #region Insertion
        int InsertTblPurchaseVehFreightDtls(TblPurchaseVehFreightDtlsTO tblPurchaseVehFreightDtlsTO);

        int InsertTblPurchaseVehFreightDtls(TblPurchaseVehFreightDtlsTO tblPurchaseVehFreightDtlsTO, SqlConnection conn, SqlTransaction tran);

        int ExecuteInsertionCommand(TblPurchaseVehFreightDtlsTO tblPurchaseVehFreightDtlsTO, SqlCommand cmdInsert);
        
        #endregion

        #region Updation
        int UpdateTblPurchaseVehFreightDtls(TblPurchaseVehFreightDtlsTO tblPurchaseVehFreightDtlsTO);

        int UpdateTblPurchaseVehFreightDtls(TblPurchaseVehFreightDtlsTO tblPurchaseVehFreightDtlsTO, SqlConnection conn, SqlTransaction tran);

        int ExecuteUpdationCommand(TblPurchaseVehFreightDtlsTO tblPurchaseVehFreightDtlsTO, SqlCommand cmdUpdate);
       
        #endregion

        #region Deletion
        int DeleteTblPurchaseVehFreightDtls(Int32 idPurchaseVehFreightDtls);

        int DeleteTblPurchaseVehFreightDtls(Int32 idPurchaseVehFreightDtls, SqlConnection conn, SqlTransaction tran);

        int ExecuteDeletionCommand(Int32 idPurchaseVehFreightDtls, SqlCommand cmdDelete);

        int DeletePurchaseVehFreightDtls(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran);

        #endregion

    }
}