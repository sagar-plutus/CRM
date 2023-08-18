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
    public class TblRateDeclareReasonsBL
    {
        #region Selection
       
        public static List<TblRateDeclareReasonsTO> SelectAllTblRateDeclareReasonsList()
        {
            return TblRateDeclareReasonsDAO.SelectAllTblRateDeclareReasons();
        }

        public static TblRateDeclareReasonsTO SelectTblRateDeclareReasonsTO(Int32 idRateReason)
        {
            return TblRateDeclareReasonsDAO.SelectTblRateDeclareReasons(idRateReason);
        }

       

        #endregion
        
        #region Insertion
        public static int InsertTblRateDeclareReasons(TblRateDeclareReasonsTO tblRateDeclareReasonsTO)
        {
            return TblRateDeclareReasonsDAO.InsertTblRateDeclareReasons(tblRateDeclareReasonsTO);
        }

        public static int InsertTblRateDeclareReasons(TblRateDeclareReasonsTO tblRateDeclareReasonsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblRateDeclareReasonsDAO.InsertTblRateDeclareReasons(tblRateDeclareReasonsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblRateDeclareReasons(TblRateDeclareReasonsTO tblRateDeclareReasonsTO)
        {
            return TblRateDeclareReasonsDAO.UpdateTblRateDeclareReasons(tblRateDeclareReasonsTO);
        }

        public static int UpdateTblRateDeclareReasons(TblRateDeclareReasonsTO tblRateDeclareReasonsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblRateDeclareReasonsDAO.UpdateTblRateDeclareReasons(tblRateDeclareReasonsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblRateDeclareReasons(Int32 idRateReason)
        {
            return TblRateDeclareReasonsDAO.DeleteTblRateDeclareReasons(idRateReason);
        }

        public static int DeleteTblRateDeclareReasons(Int32 idRateReason, SqlConnection conn, SqlTransaction tran)
        {
            return TblRateDeclareReasonsDAO.DeleteTblRateDeclareReasons(idRateReason, conn, tran);
        }

        #endregion
        
    }
}
