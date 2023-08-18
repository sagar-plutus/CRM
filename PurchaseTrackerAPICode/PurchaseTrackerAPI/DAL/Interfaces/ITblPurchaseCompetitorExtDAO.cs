using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseCompetitorExtDAO
    {
        String SqlSelectQuery();
        List<TblPurchaseCompetitorExtTO> SelectAllTblPurchaseCompetitorExt();
        List<TblPurchaseCompetitorExtTO> GetMaterialListByCompetitorId(Int32 competitorId);
        List<TblPurchaseCompetitorExtTO> SelectTblPurchaseCompetitorExt(Int32 idPurCompetitorExt);
        List<TblPurchaseCompetitorExtTO> SelectAllTblPurchaseCompetitorExt(SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseCompetitorExtTO> ConvertDTToList(SqlDataReader tblPurchaseCompetitorExtTODT);
        int InsertTblPurchaseCompetitorExt(TblPurchaseCompetitorExtTO tblPurchaseCompetitorExtTO);
        int InsertTblPurchaseCompetitorExt(TblPurchaseCompetitorExtTO tblPurchaseCompetitorExtTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPurchaseCompetitorExtTO tblPurchaseCompetitorExtTO, SqlCommand cmdInsert);
        int UpdateTblPurchaseCompetitorExt(TblPurchaseCompetitorExtTO tblPurchaseCompetitorExtTO);
        int UpdateTblPurchaseCompetitorExt(TblPurchaseCompetitorExtTO tblPurchaseCompetitorExtTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPurchaseCompetitorExtTO tblPurchaseCompetitorExtTO, SqlCommand cmdUpdate);
        int DeleteTblPurchaseCompetitorExt(Int32 idPurCompetitorExt);
        int DeleteTblPurchaseCompetitorExt(Int32 idPurCompetitorExt, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idPurCompetitorExt, SqlCommand cmdDelete);

    }
}