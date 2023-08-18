using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;


namespace SalesTrackerAPI.BL
{
    public class TblLoadingQuotaConsumptionBL
    {
        #region Selection

        public static List<TblLoadingQuotaConsumptionTO> SelectAllTblLoadingQuotaConsumptionList()
        {
           return  TblLoadingQuotaConsumptionDAO.SelectAllTblLoadingQuotaConsumption();
        }

        public static TblLoadingQuotaConsumptionTO SelectTblLoadingQuotaConsumptionTO()
        {
            return  TblLoadingQuotaConsumptionDAO.SelectTblLoadingQuotaConsumption();
        }

        

        #endregion
        
        #region Insertion
        public static int InsertTblLoadingQuotaConsumption(TblLoadingQuotaConsumptionTO tblLoadingQuotaConsumptionTO)
        {
            return TblLoadingQuotaConsumptionDAO.InsertTblLoadingQuotaConsumption(tblLoadingQuotaConsumptionTO);
        }

        public static int InsertTblLoadingQuotaConsumption(TblLoadingQuotaConsumptionTO tblLoadingQuotaConsumptionTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingQuotaConsumptionDAO.InsertTblLoadingQuotaConsumption(tblLoadingQuotaConsumptionTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblLoadingQuotaConsumption(TblLoadingQuotaConsumptionTO tblLoadingQuotaConsumptionTO)
        {
            return TblLoadingQuotaConsumptionDAO.UpdateTblLoadingQuotaConsumption(tblLoadingQuotaConsumptionTO);
        }

        public static int UpdateTblLoadingQuotaConsumption(TblLoadingQuotaConsumptionTO tblLoadingQuotaConsumptionTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingQuotaConsumptionDAO.UpdateTblLoadingQuotaConsumption(tblLoadingQuotaConsumptionTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblLoadingQuotaConsumption()
        {
            return TblLoadingQuotaConsumptionDAO.DeleteTblLoadingQuotaConsumption();
        }

        public static int DeleteTblLoadingQuotaConsumption(SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingQuotaConsumptionDAO.DeleteTblLoadingQuotaConsumption(conn, tran);
        }

        #endregion
        
    }
}
