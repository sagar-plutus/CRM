using PurchaseTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
     public interface ITblScheduleDensityDAO
    {

        #region Selection
         List<TblScheduleDensityTO> SelectAllTblScheduleDensity();

         TblScheduleDensityTO SelectTblScheduleDensity(Int32 idScheduleDensity);
         List<TblScheduleDensityTO> SelectAllTblScheduleDensity(SqlConnection conn, SqlTransaction tran);
        #endregion

        #region Insertion
         int InsertTblScheduleDensity(TblScheduleDensityTO tblScheduleDensityTO);

         int InsertTblScheduleDensity(TblScheduleDensityTO tblScheduleDensityTO, SqlConnection conn, SqlTransaction tran);

         #endregion

        #region Updation
         int UpdateTblScheduleDensity(TblScheduleDensityTO tblScheduleDensityTO);

         int UpdateTblScheduleDensity(TblScheduleDensityTO tblScheduleDensityTO, SqlConnection conn, SqlTransaction tran);

        #endregion

        #region Deletion
         int DeleteTblScheduleDensity(Int32 idScheduleDensity);

         int DeleteTblScheduleDensity(Int32 idScheduleDensity, SqlConnection conn, SqlTransaction tran);

        int DeletePurchaseVehDensityDtls(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran);

        #endregion

    }
}
