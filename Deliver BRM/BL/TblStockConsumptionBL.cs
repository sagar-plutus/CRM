using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using System.Linq;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.BL
{
    public class TblStockConsumptionBL
    {
        #region Selection
       
        public static List<TblStockConsumptionTO> SelectAllTblStockConsumptionList()
        {
            return  TblStockConsumptionDAO.SelectAllTblStockConsumption();
        }

        public static TblStockConsumptionTO SelectTblStockConsumptionTO(Int32 idStockConsumption)
        {
            return  TblStockConsumptionDAO.SelectTblStockConsumption(idStockConsumption);
        }

        public static List<TblStockConsumptionTO> SelectAllStockConsumptionList(Int32 loadingSlipExtId,Int32 txnOpTypeId,SqlConnection conn,SqlTransaction tran)
        {
            return TblStockConsumptionDAO.SelectAllStockConsumptionList(loadingSlipExtId,txnOpTypeId,conn,tran);
        }

        #endregion

        #region Insertion
        public static int InsertTblStockConsumption(TblStockConsumptionTO tblStockConsumptionTO)
        {
            return TblStockConsumptionDAO.InsertTblStockConsumption(tblStockConsumptionTO);
        }

        public static int InsertTblStockConsumption(TblStockConsumptionTO tblStockConsumptionTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblStockConsumptionDAO.InsertTblStockConsumption(tblStockConsumptionTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblStockConsumption(TblStockConsumptionTO tblStockConsumptionTO)
        {
            return TblStockConsumptionDAO.UpdateTblStockConsumption(tblStockConsumptionTO);
        }

        public static int UpdateTblStockConsumption(TblStockConsumptionTO tblStockConsumptionTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblStockConsumptionDAO.UpdateTblStockConsumption(tblStockConsumptionTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblStockConsumption(Int32 idStockConsumption)
        {
            return TblStockConsumptionDAO.DeleteTblStockConsumption(idStockConsumption);
        }

        public static int DeleteTblStockConsumption(Int32 idStockConsumption, SqlConnection conn, SqlTransaction tran)
        {
            return TblStockConsumptionDAO.DeleteTblStockConsumption(idStockConsumption, conn, tran);
        }

        #endregion
        
    }
}
