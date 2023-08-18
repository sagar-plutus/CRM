using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseCompetitorUpdatesBL
    {
         ResultMessage SaveMarketUpdate(List<TblPurchaseCompetitorUpdatesTO> competitorUpdatesTOList);
        List<DropDownTO> SelectCompetitorBrandNamesDropDownList(Int32 competitorOrgId);
        List<DropDownTO> SelectCompetitorMaterialGradeDropDownList(Int32 MaterialId, Int32 competitorOrgId);
        TblPurchaseCompetitorUpdatesTO SelectLastPriceForCompetitorAndBrand(Int32 brandId);
     //   List<TblCompetitorUpdatesTO> SelectAllTblCompetitorUpdatesList();
       // List<TblCompetitorUpdatesTO> SelectAllTblCompetitorUpdatesList(Int32 competitorId, Int32 enteredBy, DateTime fromDate, DateTime toDate);
      //  TblCompetitorUpdatesTO SelectTblCompetitorUpdatesTO(Int32 idCompeUpdate);
      //  List<DropDownTO> SelectCompeUpdateUserDropDown();
        int InsertTblCompetitorUpdates(TblPurchaseCompetitorUpdatesTO tblCompetitorUpdatesTO);
        int InsertTblCompetitorUpdates(TblPurchaseCompetitorUpdatesTO tblCompetitorUpdatesTO, SqlConnection conn, SqlTransaction tran);
        //int UpdateTblCompetitorUpdates(TblCompetitorUpdatesTO tblCompetitorUpdatesTO);
        //int UpdateTblCompetitorUpdates(TblCompetitorUpdatesTO tblCompetitorUpdatesTO, SqlConnection conn, SqlTransaction tran);
        //int DeleteTblCompetitorUpdates(Int32 idCompeUpdate);
        //int DeleteTblCompetitorUpdates(Int32 idCompeUpdate, SqlConnection conn, SqlTransaction tran);

    }
}