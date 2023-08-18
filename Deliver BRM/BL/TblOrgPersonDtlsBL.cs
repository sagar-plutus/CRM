using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SalesTrackerAPI.BL
{
    public class TblOrgPersonDtlsBL
    {
        #region Selection
        public static List<TblOrgPersonDtlsTO> SelectAllTblOrgPersonDtls()
        {
            return TblOrgPersonDtlsDAO.SelectAllTblOrgPersonDtls();
        }

        public static List<TblOrgPersonDtlsTO> SelectAllTblOrgPersonDtlsList()
        {
            List<TblOrgPersonDtlsTO> tblOrgPersonDtlsTODT = TblOrgPersonDtlsDAO.SelectAllTblOrgPersonDtls();
            if (tblOrgPersonDtlsTODT != null && tblOrgPersonDtlsTODT.Count > 0)
                return tblOrgPersonDtlsTODT;
            else
                return null;
        }

        public static TblOrgPersonDtlsTO SelectTblOrgPersonDtlsTO(Int32 idOrgPersonDtl)
        {
            TblOrgPersonDtlsTO tblOrgPersonDtlsTODT = TblOrgPersonDtlsDAO.SelectTblOrgPersonDtls(idOrgPersonDtl);
            if (tblOrgPersonDtlsTODT != null )
                return tblOrgPersonDtlsTODT;
            else
                return null;
        }
        #endregion

        #region Insertion
        public static int InsertTblOrgPersonDtls(TblOrgPersonDtlsTO tblOrgPersonDtlsTO)
        {
            return TblOrgPersonDtlsDAO.InsertTblOrgPersonDtls(tblOrgPersonDtlsTO);
        }

        public static int InsertTblOrgPersonDtls(TblOrgPersonDtlsTO tblOrgPersonDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblOrgPersonDtlsDAO.InsertTblOrgPersonDtls(tblOrgPersonDtlsTO, conn, tran);
        }

        #endregion

        #region Updation
        public static int UpdateTblOrgPersonDtls(TblOrgPersonDtlsTO tblOrgPersonDtlsTO)
        {
            return TblOrgPersonDtlsDAO.UpdateTblOrgPersonDtls(tblOrgPersonDtlsTO);
        }

        public static int UpdateTblOrgPersonDtls(TblOrgPersonDtlsTO tblOrgPersonDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblOrgPersonDtlsDAO.UpdateTblOrgPersonDtls(tblOrgPersonDtlsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblOrgPersonDtls(Int32 idOrgPersonDtl)
        {
            return TblOrgPersonDtlsDAO.DeleteTblOrgPersonDtls(idOrgPersonDtl);
        }

        public static int DeleteTblOrgPersonDtls(Int32 idOrgPersonDtl, SqlConnection conn, SqlTransaction tran)
        {
            return TblOrgPersonDtlsDAO.DeleteTblOrgPersonDtls(idOrgPersonDtl, conn, tran);
        }

        #endregion

    }
}
