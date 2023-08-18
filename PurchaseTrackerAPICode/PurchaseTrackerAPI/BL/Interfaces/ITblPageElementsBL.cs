using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPageElementsBL
    {
        List<TblPageElementsTO> SelectAllTblPageElementsList(int pageId);
        List<TblPageElementsTO> SelectAllTblPageElementsList();
        TblPageElementsTO SelectTblPageElementsTO(Int32 idPageElement);
        int InsertTblPageElements(TblPageElementsTO tblPageElementsTO);
        int InsertTblPageElements(TblPageElementsTO tblPageElementsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPageElements(TblPageElementsTO tblPageElementsTO);
          int UpdateTblPageElements(TblPageElementsTO tblPageElementsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPageElements(Int32 idPageElement);
        int DeleteTblPageElements(Int32 idPageElement, SqlConnection conn, SqlTransaction tran);

    }
}