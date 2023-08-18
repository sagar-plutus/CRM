using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseUnloadingDtlDAO
    {
        String SqlSelectQuery();
          List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtl();
        List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtl(Int32 purchaseWeighingStageId,Int32 isGradingBeforeUnld = 0);
        List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtlListByScheduleId(Int32 purchaseScheduleId,Int32 isGradingBeforeUnloading);
        List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtl(Int32 purchaseWeighingStageId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseUnloadingDtlTO> SelectTblPurchaseUnloadingDtl(Int32 idPurchaseUnloadingDtl);
        List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtl(SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseUnloadingDtlTO> ConvertDTToList(SqlDataReader tblPurchaseUnloadingDtlTODT);
        List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtl(string purchaseWeighingStageIdStr);
          int InsertTblPurchaseUnloadingDtl(TblPurchaseUnloadingDtlTO tblPurchaseUnloadingDtlTO);
        int InsertTblPurchaseUnloadingDtl(TblPurchaseUnloadingDtlTO tblPurchaseUnloadingDtlTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPurchaseUnloadingDtlTO tblPurchaseUnloadingDtlTO, SqlCommand cmdInsert);
        int UpdateTblPurchaseUnloadingDtl(TblPurchaseUnloadingDtlTO tblPurchaseUnloadingDtlTO);
        int UpdateTblPurchaseUnloadingDtl(TblPurchaseUnloadingDtlTO tblPurchaseUnloadingDtlTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPurchaseUnloadingDtlTO tblPurchaseUnloadingDtlTO, SqlCommand cmdUpdate);
        int DeleteTblPurchaseUnloadingDtl(Int32 idPurchaseUnloadingDtl);
        int DeleteTblPurchaseUnloadingDtl(Int32 idPurchaseUnloadingDtl, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idPurchaseUnloadingDtl, SqlCommand cmdDelete);
        int DeleteAllUnloadingDtlsAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtlListByScheduleId(Int32 purchaseScheduleId,Int32 isGradingBeforeUnloading,SqlConnection conn,SqlTransaction tran);

    }
}