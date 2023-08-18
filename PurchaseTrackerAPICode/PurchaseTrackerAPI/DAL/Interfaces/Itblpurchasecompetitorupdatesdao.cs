using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface Itblpurchasecompetitorupdatesdao
    {
        String SqlSelectQuery();
        List<DropDownTO> SelectCompetitorBrandNamesDropDownList(Int32 competitorOrgId);
        List<DropDownTO> SelectCompetitorMaterialGradeDropDownList(Int32 materialid, Int32 competitorOrgId);
      TblPurchaseCompetitorUpdatesTO SelectLastPriceForCompetitorAndBrand(Int32 brandId);
        List<TblPurchaseCompetitorUpdatesTO> ConvertDTToList(SqlDataReader tblCompetitorUpdatesTODT);
       //   List<TblCompetitorUpdatesTO> SelectAllTblCompetitorUpdates();
      //  List<TblCompetitorUpdatesTO> SelectAllTblCompetitorUpdates(Int32 competitorId, Int32 enteredBy, DateTime fromDate, DateTime toDate);
    //    TblCompetitorUpdatesTO SelectTblCompetitorUpdates(Int32 idCompeUpdate);
      //  List<DropDownTO> SelectCompeUpdateUserDropDown();
        int InsertTblCompetitorUpdates(TblPurchaseCompetitorUpdatesTO tblCompetitorUpdatesTO);
        int InsertTblCompetitorUpdates(TblPurchaseCompetitorUpdatesTO tblCompetitorUpdatesTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPurchaseCompetitorUpdatesTO tblCompetitorUpdatesTO, SqlCommand cmdInsert);
        //int UpdateTblCompetitorUpdates(TblCompetitorUpdatesTO tblCompetitorUpdatesTO);
        //  int UpdateTblCompetitorUpdates(TblCompetitorUpdatesTO tblCompetitorUpdatesTO, SqlConnection conn, SqlTransaction tran);
        //  int ExecuteUpdationCommand(TblCompetitorUpdatesTO tblCompetitorUpdatesTO, SqlCommand cmdUpdate);
        //  int DeleteTblCompetitorUpdates(Int32 idCompeUpdate);
        //int DeleteTblCompetitorUpdates(Int32 idCompeUpdate, SqlConnection conn, SqlTransaction tran);
        //int ExecuteDeletionCommand(Int32 idCompeUpdate, SqlCommand cmdDelete);

    }
}