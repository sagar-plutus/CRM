using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseCompetitorExtBL
    {
        List<TblPurchaseCompetitorExtTO> SelectAllTblPurchaseCompetitorExt();
        List<TblPurchaseCompetitorExtTO> SelectAllTblPurchaseCompetitorExtList();
        List<TblPurchaseCompetitorExtTO> GetMaterialListByCompetitorId(Int32 competitorId);
        TblPurchaseCompetitorExtTO SelectTblPurchaseCompetitorExtTO(Int32 idPurCompetitorExt);
        int InsertTblPurchaseCompetitorExt(TblPurchaseCompetitorExtTO tblPurchaseCompetitorExtTO);
        int InsertTblPurchaseCompetitorExt(TblPurchaseCompetitorExtTO tblPurchaseCompetitorExtTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseCompetitorExt(TblPurchaseCompetitorExtTO tblPurchaseCompetitorExtTO);
        int UpdateTblPurchaseCompetitorExt(TblPurchaseCompetitorExtTO tblPurchaseCompetitorExtTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseCompetitorExt(Int32 idPurCompetitorExt);
        int DeleteTblPurchaseCompetitorExt(Int32 idPurCompetitorExt, SqlConnection conn, SqlTransaction tran);

    }
}