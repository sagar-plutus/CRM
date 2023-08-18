using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblRateDeclareReasonsBL : ITblRateDeclareReasonsBL
    {
        private readonly ITblRateDeclareReasonsDAO _iTblRateDeclareReasonsDAO;
        public TblRateDeclareReasonsBL(ITblRateDeclareReasonsDAO iTblRateDeclareReasonsDAO)
        {   _iTblRateDeclareReasonsDAO = iTblRateDeclareReasonsDAO;
        }
        #region Selection
       
        public  List<TblRateDeclareReasonsTO> SelectAllTblRateDeclareReasonsList()
        {
            return _iTblRateDeclareReasonsDAO.SelectAllTblRateDeclareReasons();
        }

        public  TblRateDeclareReasonsTO SelectTblRateDeclareReasonsTO(Int32 idRateReason)
        {
            return _iTblRateDeclareReasonsDAO.SelectTblRateDeclareReasons(idRateReason);
        }

       

        #endregion
        
        #region Insertion
        public  int InsertTblRateDeclareReasons(TblRateDeclareReasonsTO tblRateDeclareReasonsTO)
        {
            return _iTblRateDeclareReasonsDAO.InsertTblRateDeclareReasons(tblRateDeclareReasonsTO);
        }

        public  int InsertTblRateDeclareReasons(TblRateDeclareReasonsTO tblRateDeclareReasonsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblRateDeclareReasonsDAO.InsertTblRateDeclareReasons(tblRateDeclareReasonsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblRateDeclareReasons(TblRateDeclareReasonsTO tblRateDeclareReasonsTO)
        {
            return _iTblRateDeclareReasonsDAO.UpdateTblRateDeclareReasons(tblRateDeclareReasonsTO);
        }

        public  int UpdateTblRateDeclareReasons(TblRateDeclareReasonsTO tblRateDeclareReasonsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblRateDeclareReasonsDAO.UpdateTblRateDeclareReasons(tblRateDeclareReasonsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblRateDeclareReasons(Int32 idRateReason)
        {
            return _iTblRateDeclareReasonsDAO.DeleteTblRateDeclareReasons(idRateReason);
        }

        public  int DeleteTblRateDeclareReasons(Int32 idRateReason, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblRateDeclareReasonsDAO.DeleteTblRateDeclareReasons(idRateReason, conn, tran);
        }

        #endregion
        
    }
}
