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
    public class TblSmsBL : ITblSmsBL
    {
        private readonly ITblSmsDAO _iTblSmsDAO;
        public TblSmsBL(ITblSmsDAO iTblSmsDAO)
        {
            _iTblSmsDAO = iTblSmsDAO;
        }
        #region Selection
        public  List<TblSmsTO> SelectAllTblSmsList()
        {
            return  _iTblSmsDAO.SelectAllTblSms();
        }

        public  TblSmsTO SelectTblSmsTO(Int32 idSms)
        {
            return  _iTblSmsDAO.SelectTblSms(idSms);
        }

        #endregion
        
        #region Insertion
        public  int InsertTblSms(TblSmsTO tblSmsTO)
        {
            return _iTblSmsDAO.InsertTblSms(tblSmsTO);
        }

        public  int InsertTblSms(TblSmsTO tblSmsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSmsDAO.InsertTblSms(tblSmsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblSms(TblSmsTO tblSmsTO)
        {
            return _iTblSmsDAO.UpdateTblSms(tblSmsTO);
        }

        public  int UpdateTblSms(TblSmsTO tblSmsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSmsDAO.UpdateTblSms(tblSmsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblSms(Int32 idSms)
        {
            return _iTblSmsDAO.DeleteTblSms(idSms);
        }

        public  int DeleteTblSms(Int32 idSms, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSmsDAO.DeleteTblSms(idSms, conn, tran);
        }

        #endregion
        
    }
}
