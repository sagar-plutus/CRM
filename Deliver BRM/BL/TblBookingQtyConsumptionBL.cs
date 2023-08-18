using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;

namespace SalesTrackerAPI.BL
{
    public class TblBookingQtyConsumptionBL
    {
        #region Selection

        public static List<TblBookingQtyConsumptionTO> SelectAllTblBookingQtyConsumptionList()
        {
            return  TblBookingQtyConsumptionDAO.SelectAllTblBookingQtyConsumption();
        }

        public static TblBookingQtyConsumptionTO SelectTblBookingQtyConsumptionTO(Int32 idBookQtyConsuption)
        {
            return  TblBookingQtyConsumptionDAO.SelectTblBookingQtyConsumption(idBookQtyConsuption);
        }

        

        #endregion
        
        #region Insertion
        public static int InsertTblBookingQtyConsumption(TblBookingQtyConsumptionTO tblBookingQtyConsumptionTO)
        {
            return TblBookingQtyConsumptionDAO.InsertTblBookingQtyConsumption(tblBookingQtyConsumptionTO);
        }

        public static int InsertTblBookingQtyConsumption(TblBookingQtyConsumptionTO tblBookingQtyConsumptionTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingQtyConsumptionDAO.InsertTblBookingQtyConsumption(tblBookingQtyConsumptionTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblBookingQtyConsumption(TblBookingQtyConsumptionTO tblBookingQtyConsumptionTO)
        {
            return TblBookingQtyConsumptionDAO.UpdateTblBookingQtyConsumption(tblBookingQtyConsumptionTO);
        }

        public static int UpdateTblBookingQtyConsumption(TblBookingQtyConsumptionTO tblBookingQtyConsumptionTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingQtyConsumptionDAO.UpdateTblBookingQtyConsumption(tblBookingQtyConsumptionTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblBookingQtyConsumption(Int32 idBookQtyConsuption)
        {
            return TblBookingQtyConsumptionDAO.DeleteTblBookingQtyConsumption(idBookQtyConsuption);
        }

        public static int DeleteTblBookingQtyConsumption(Int32 idBookQtyConsuption, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingQtyConsumptionDAO.DeleteTblBookingQtyConsumption(idBookQtyConsuption, conn, tran);
        }

        #endregion
        
    }
}
