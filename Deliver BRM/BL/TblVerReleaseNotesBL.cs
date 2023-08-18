using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using TO;
using SalesTrackerAPI.DAL;

namespace SalesTrackerAPI.BL
{
    public class TblVerReleaseNotesBL
    {
        #region Selection
        public static List<TblVerReleaseNotesTO> SelectAllTblVerReleaseNotes()
        {
            return TblVerReleaseNotesDAO.SelectAllTblVerReleaseNotes();
        }

        public static List<TblVerReleaseNotesTO> SelectAllTblVerReleaseNotesList()
        {
            List<TblVerReleaseNotesTO> tblVerReleaseNotesTOList = TblVerReleaseNotesDAO.SelectAllTblVerReleaseNotes();
            return tblVerReleaseNotesTOList;
        }

        public static TblVerReleaseNotesTO SelectTblVerReleaseNotesTO(Int32 idReleaseNote)
        {
            List<TblVerReleaseNotesTO> tblVerReleaseNotesTODT = TblVerReleaseNotesDAO.SelectTblVerReleaseNotes(idReleaseNote);
            if(tblVerReleaseNotesTODT != null && tblVerReleaseNotesTODT.Count == 1)
                return tblVerReleaseNotesTODT[0];
            else
                return null;
        }

        public static List<TblVerReleaseNotesTO> SelectTblVerReleaseNotesTOByVerId(Int32 idVersion)
        {
            List<TblVerReleaseNotesTO> tblVerReleaseNotesTODT = TblVerReleaseNotesDAO.SelectTblVerReleaseNotesByVerId(idVersion);
            if (tblVerReleaseNotesTODT != null && tblVerReleaseNotesTODT.Count > 0)
                return tblVerReleaseNotesTODT;
            else
                return null;
        }


        #endregion

        #region Insertion
        public static int InsertTblVerReleaseNotes(TblVerReleaseNotesTO tblVerReleaseNotesTO)
        {
            return TblVerReleaseNotesDAO.InsertTblVerReleaseNotes(tblVerReleaseNotesTO);
        }

        public static int InsertTblVerReleaseNotes(TblVerReleaseNotesTO tblVerReleaseNotesTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblVerReleaseNotesDAO.InsertTblVerReleaseNotes(tblVerReleaseNotesTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblVerReleaseNotes(TblVerReleaseNotesTO tblVerReleaseNotesTO)
        {
            return TblVerReleaseNotesDAO.UpdateTblVerReleaseNotes(tblVerReleaseNotesTO);
        }

        public static int UpdateTblVerReleaseNotes(TblVerReleaseNotesTO tblVerReleaseNotesTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblVerReleaseNotesDAO.UpdateTblVerReleaseNotes(tblVerReleaseNotesTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblVerReleaseNotes(Int32 idReleaseNote)
        {
            return TblVerReleaseNotesDAO.DeleteTblVerReleaseNotes(idReleaseNote);
        }

        public static int DeleteTblVerReleaseNotes(Int32 idReleaseNote, SqlConnection conn, SqlTransaction tran)
        {
            return TblVerReleaseNotesDAO.DeleteTblVerReleaseNotes(idReleaseNote, conn, tran);
        }

        #endregion
        
    }
}
