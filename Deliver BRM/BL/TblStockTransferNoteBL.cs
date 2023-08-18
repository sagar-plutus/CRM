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
    public class TblStockTransferNoteBL
    {
        #region Selection
       
        public static List<TblStockTransferNoteTO> SelectAllTblStockTransferNoteList()
        {
            return  TblStockTransferNoteDAO.SelectAllTblStockTransferNote();
        }

        public static TblStockTransferNoteTO SelectTblStockTransferNoteTO(Int32 idStkTransferNote)
        {
            return  TblStockTransferNoteDAO.SelectTblStockTransferNote(idStkTransferNote);
        }

        #endregion
        
        #region Insertion
        public static int InsertTblStockTransferNote(TblStockTransferNoteTO tblStockTransferNoteTO)
        {
            return TblStockTransferNoteDAO.InsertTblStockTransferNote(tblStockTransferNoteTO);
        }

        public static int InsertTblStockTransferNote(TblStockTransferNoteTO tblStockTransferNoteTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblStockTransferNoteDAO.InsertTblStockTransferNote(tblStockTransferNoteTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblStockTransferNote(TblStockTransferNoteTO tblStockTransferNoteTO)
        {
            return TblStockTransferNoteDAO.UpdateTblStockTransferNote(tblStockTransferNoteTO);
        }

        public static int UpdateTblStockTransferNote(TblStockTransferNoteTO tblStockTransferNoteTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblStockTransferNoteDAO.UpdateTblStockTransferNote(tblStockTransferNoteTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblStockTransferNote(Int32 idStkTransferNote)
        {
            return TblStockTransferNoteDAO.DeleteTblStockTransferNote(idStkTransferNote);
        }

        public static int DeleteTblStockTransferNote(Int32 idStkTransferNote, SqlConnection conn, SqlTransaction tran)
        {
            return TblStockTransferNoteDAO.DeleteTblStockTransferNote(idStkTransferNote, conn, tran);
        }

        #endregion
        
    }
}
