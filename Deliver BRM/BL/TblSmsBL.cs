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
    public class TblSmsBL
    {
        #region Selection
        public static List<TblSmsTO> SelectAllTblSmsList()
        {
            return  TblSmsDAO.SelectAllTblSms();
        }

        public static TblSmsTO SelectTblSmsTO(Int32 idSms)
        {
            return  TblSmsDAO.SelectTblSms(idSms);
        }

        #endregion
        
        #region Insertion
        public static int InsertTblSms(TblSmsTO tblSmsTO)
        {
            return TblSmsDAO.InsertTblSms(tblSmsTO);
        }

        public static int InsertTblSms(TblSmsTO tblSmsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblSmsDAO.InsertTblSms(tblSmsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblSms(TblSmsTO tblSmsTO)
        {
            return TblSmsDAO.UpdateTblSms(tblSmsTO);
        }

        public static int UpdateTblSms(TblSmsTO tblSmsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblSmsDAO.UpdateTblSms(tblSmsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblSms(Int32 idSms)
        {
            return TblSmsDAO.DeleteTblSms(idSms);
        }

        public static int DeleteTblSms(Int32 idSms, SqlConnection conn, SqlTransaction tran)
        {
            return TblSmsDAO.DeleteTblSms(idSms, conn, tran);
        }

        #endregion
        
    }
}
