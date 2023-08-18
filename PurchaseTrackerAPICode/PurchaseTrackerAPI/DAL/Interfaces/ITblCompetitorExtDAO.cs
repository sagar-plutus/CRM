using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblCompetitorExtDAO
    {
        String SqlSelectQuery();
        List<TblCompetitorExtTO> SelectAllTblCompetitorExt();
        List<TblCompetitorExtTO> SelectAllTblCompetitorExt(Int32 orgId, SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> SelectCompetitorBrandNamesDropDownList(Int32 competitorOrgId);
        TblCompetitorExtTO SelectTblCompetitorExt(Int32 idCompetitorExt);
        List<TblCompetitorExtTO> ConvertDTToList(SqlDataReader tblCompetitorExtTODT);
        int InsertTblCompetitorExt(TblCompetitorExtTO tblCompetitorExtTO);
        int InsertTblCompetitorExt(TblCompetitorExtTO tblCompetitorExtTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblCompetitorExtTO tblCompetitorExtTO, SqlCommand cmdInsert);
        int UpdateTblCompetitorExt(TblCompetitorExtTO tblCompetitorExtTO);
        int UpdateTblCompetitorExt(TblCompetitorExtTO tblCompetitorExtTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblCompetitorExtTO tblCompetitorExtTO, SqlCommand cmdUpdate);
        int DeleteTblCompetitorExt(Int32 idCompetitorExt);
        int DeleteTblCompetitorExt(Int32 idCompetitorExt, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idCompetitorExt, SqlCommand cmdDelete);
    }
}