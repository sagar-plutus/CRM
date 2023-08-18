using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.BL
{
    public class TblOtherDesignationsBL
    {
        #region Selection
        public static List<TblOtherDesignationsTO> SelectAllTblOtherDesignations()
        {
            return TblOtherDesignationsDAO.SelectAllTblOtherDesignations();
        }

        public static List<TblOtherDesignationsTO> SelectAllTblOtherDesignationsList()
        {
            List<TblOtherDesignationsTO> tblOtherDesignationsTODT = TblOtherDesignationsDAO.SelectAllTblOtherDesignations();
            return tblOtherDesignationsTODT;
        }

        public static TblOtherDesignationsTO SelectTblOtherDesignationsTO(Int32 idOtherDesignation)
        {
            TblOtherDesignationsTO tblOtherDesignationsTO = TblOtherDesignationsDAO.SelectTblOtherDesignations(idOtherDesignation);
            if (tblOtherDesignationsTO != null)
                return tblOtherDesignationsTO;
            else
                return null;
        }



        #endregion

        #region Insertion
        public static int InsertTblOtherDesignations(TblOtherDesignationsTO tblOtherDesignationsTO)
        {
            return TblOtherDesignationsDAO.InsertTblOtherDesignations(tblOtherDesignationsTO);
        }

        public static int InsertTblOtherDesignations(TblOtherDesignationsTO tblOtherDesignationsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblOtherDesignationsDAO.InsertTblOtherDesignations(tblOtherDesignationsTO, conn, tran);
        }

        #endregion

        #region Updation
        public static int UpdateTblOtherDesignations(TblOtherDesignationsTO tblOtherDesignationsTO)
        {
            return TblOtherDesignationsDAO.UpdateTblOtherDesignations(tblOtherDesignationsTO);
        }

        public static int UpdateTblOtherDesignations(TblOtherDesignationsTO tblOtherDesignationsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblOtherDesignationsDAO.UpdateTblOtherDesignations(tblOtherDesignationsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblOtherDesignations(Int32 idOtherDesignation)
        {
            return TblOtherDesignationsDAO.DeleteTblOtherDesignations(idOtherDesignation);
        }

        public static int DeleteTblOtherDesignations(Int32 idOtherDesignation, SqlConnection conn, SqlTransaction tran)
        {
            return TblOtherDesignationsDAO.DeleteTblOtherDesignations(idOtherDesignation, conn, tran);
        }

        #endregion
    }
}
