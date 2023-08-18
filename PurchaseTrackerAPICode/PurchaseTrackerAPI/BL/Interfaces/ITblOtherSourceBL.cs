using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblOtherSourceBL
    {
        List<TblOtherSourceTO> SelectAllTblOtherSourceList();
        TblOtherSourceTO SelectTblOtherSourceTO(Int32 idOtherSource);
        List<TblOtherSourceTO> SelectTblOtherSourceListFromDesc(string OtherSourceDesc);
        List<DropDownTO> SelectOtherSourceOfMarketTrendForDropDown();
        int InsertTblOtherSource(TblOtherSourceTO tblOtherSourceTO);
        int InsertTblOtherSource(TblOtherSourceTO tblOtherSourceTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblOtherSource(TblOtherSourceTO tblOtherSourceTO);
          int UpdateTblOtherSource(TblOtherSourceTO tblOtherSourceTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblOtherSource(Int32 idOtherSource);
        int DeleteTblOtherSource(Int32 idOtherSource, SqlConnection conn, SqlTransaction tran);

    }
}