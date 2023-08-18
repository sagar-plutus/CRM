using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblOtherSourceDAO
    {
        String SqlSelectQuery();
        List<TblOtherSourceTO> SelectAllTblOtherSource();
        TblOtherSourceTO SelectTblOtherSource(Int32 idOtherSource);
        List<TblOtherSourceTO> SelectTblOtherSourceListFromDesc(string OtherSourceDesc);
        List<DropDownTO> SelectOtherSourceOfMarketTrendForDropDown();
        List<TblOtherSourceTO> ConvertDTToList(SqlDataReader tblOtherSourceTODT);
        int InsertTblOtherSource(TblOtherSourceTO tblOtherSourceTO);
        int InsertTblOtherSource(TblOtherSourceTO tblOtherSourceTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblOtherSourceTO tblOtherSourceTO, SqlCommand cmdInsert);
        int UpdateTblOtherSource(TblOtherSourceTO tblOtherSourceTO);
        int UpdateTblOtherSource(TblOtherSourceTO tblOtherSourceTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblOtherSourceTO tblOtherSourceTO, SqlCommand cmdUpdate);
        int DeleteTblOtherSource(Int32 idOtherSource);
        int DeleteTblOtherSource(Int32 idOtherSource, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idOtherSource, SqlCommand cmdDelete);

    }
}