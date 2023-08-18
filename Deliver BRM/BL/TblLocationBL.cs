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
    public class TblLocationBL
    {
        #region Selection
     
        public static List<TblLocationTO> SelectAllTblLocationList()
        {
           return TblLocationDAO.SelectAllTblLocation();
        }

        public static List<TblLocationTO> SelectAllCompartmentLocationList(Int32 parentLocationId)
        {
            return TblLocationDAO.SelectAllTblLocation(parentLocationId);
        }

        public static List<TblLocationTO> SelectAllParentLocation()
        {
            return TblLocationDAO.SelectAllParentLocation();
        }

        public static TblLocationTO SelectTblLocationTO(Int32 idLocation)
        {
            return  TblLocationDAO.SelectTblLocation(idLocation);
        }

        /// <summary>
        /// Sanjay [2017-05-03] To Get All the compartment whose stock for the given date is not taken
        /// </summary>
        /// <param name="stockDate"></param>
        /// <returns></returns>
        public static List<TblLocationTO> SelectStkNotTakenCompartmentList(DateTime stockDate)
        {
            return TblLocationDAO.SelectStkNotTakenCompartmentList(stockDate);

        }


        #endregion

        #region Insertion
        public static int InsertTblLocation(TblLocationTO tblLocationTO)
        {
            return TblLocationDAO.InsertTblLocation(tblLocationTO);
        }

        public static int InsertTblLocation(TblLocationTO tblLocationTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblLocationDAO.InsertTblLocation(tblLocationTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblLocation(TblLocationTO tblLocationTO)
        {
            return TblLocationDAO.UpdateTblLocation(tblLocationTO);
        }

        public static int UpdateTblLocation(TblLocationTO tblLocationTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblLocationDAO.UpdateTblLocation(tblLocationTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblLocation(Int32 idLocation)
        {
            return TblLocationDAO.DeleteTblLocation(idLocation);
        }

        public static int DeleteTblLocation(Int32 idLocation, SqlConnection conn, SqlTransaction tran)
        {
            return TblLocationDAO.DeleteTblLocation(idLocation, conn, tran);
        }

        #endregion
        
    }
}
