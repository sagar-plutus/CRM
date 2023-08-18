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
    public interface ITblPurchaseUnloadingDtlBL
    {
        List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtl();
        List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtlList();
        List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtlList(Int32 purchaseWeighingStageId );
        List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtlList(Int32 purchaseWeighingStageId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtlListByScheduleId(Int32 purchaseScheduleId,Int32 isGradingBeforeUnloading,SqlConnection conn = null,SqlTransaction tran = null);
        int InsertTblPurchaseUnloadingDtl(TblPurchaseUnloadingDtlTO tblPurchaseUnloadingDtlTO);
        int InsertTblPurchaseUnloadingDtl(TblPurchaseUnloadingDtlTO tblPurchaseUnloadingDtlTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveUnloadingMaterialDetails(List<TblPurchaseUnloadingDtlTO> tblPurchaseUnloadingDtlTOList, Boolean isSendNotification,Boolean isDeletePrevious);
        ResultMessage DeleteUnloadingDetails(List<TblPurchaseUnloadingDtlTO> tblPurchaseUnloadingDtlTOList);
        int UpdateTblPurchaseUnloadingDtl(TblPurchaseUnloadingDtlTO tblPurchaseUnloadingDtlTO);
        int UpdateTblPurchaseUnloadingDtl(TblPurchaseUnloadingDtlTO tblPurchaseUnloadingDtlTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseUnloadingDtl(Int32 idPurchaseUnloadingDtl);
        int DeleteTblPurchaseUnloadingDtl(Int32 idPurchaseUnloadingDtl, SqlConnection conn, SqlTransaction tran);
        int DeleteAllUnloadingDtlsAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtlList(Int32 purchaseWeighingStageId, Int32 isGradingBeforeUnld = 0);
        List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtl(string purchaseWeighingStageIdStr);
    }
}