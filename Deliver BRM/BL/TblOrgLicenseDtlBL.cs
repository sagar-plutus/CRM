using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;


namespace SalesTrackerAPI.BL
{
    public class TblOrgLicenseDtlBL
    {
        #region Selection

        public static List<TblOrgLicenseDtlTO> SelectAllTblOrgLicenseDtlList(Int32 orgId)
        {
           return TblOrgLicenseDtlDAO.SelectAllTblOrgLicenseDtl(orgId);
        }

        public static List<TblOrgLicenseDtlTO> SelectAllTblOrgLicenseDtlList(Int32 orgId,SqlConnection conn,SqlTransaction tran)
        {
            return TblOrgLicenseDtlDAO.SelectAllTblOrgLicenseDtl(orgId,conn,tran);
        }

        public static TblOrgLicenseDtlTO SelectTblOrgLicenseDtlTO(Int32 idOrgLicense)
        {
           return  TblOrgLicenseDtlDAO.SelectTblOrgLicenseDtl(idOrgLicense);
        }

        public static List<TblOrgLicenseDtlTO> SelectAllTblOrgLicenseDtlList(Int32 orgId,Int32 licenseId,String licenseVal)
        {
            return TblOrgLicenseDtlDAO.SelectAllTblOrgLicenseDtl(orgId,licenseId,licenseVal);
        }

        #endregion

        #region Insertion
        public static int InsertTblOrgLicenseDtl(TblOrgLicenseDtlTO tblOrgLicenseDtlTO)
        {
            return TblOrgLicenseDtlDAO.InsertTblOrgLicenseDtl(tblOrgLicenseDtlTO);
        }

        public static int InsertTblOrgLicenseDtl(TblOrgLicenseDtlTO tblOrgLicenseDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblOrgLicenseDtlDAO.InsertTblOrgLicenseDtl(tblOrgLicenseDtlTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblOrgLicenseDtl(TblOrgLicenseDtlTO tblOrgLicenseDtlTO)
        {
            return TblOrgLicenseDtlDAO.UpdateTblOrgLicenseDtl(tblOrgLicenseDtlTO);
        }

        public static int UpdateTblOrgLicenseDtl(TblOrgLicenseDtlTO tblOrgLicenseDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblOrgLicenseDtlDAO.UpdateTblOrgLicenseDtl(tblOrgLicenseDtlTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblOrgLicenseDtl(Int32 idOrgLicense)
        {
            return TblOrgLicenseDtlDAO.DeleteTblOrgLicenseDtl(idOrgLicense);
        }

        public static int DeleteTblOrgLicenseDtl(Int32 idOrgLicense, SqlConnection conn, SqlTransaction tran)
        {
            return TblOrgLicenseDtlDAO.DeleteTblOrgLicenseDtl(idOrgLicense, conn, tran);
        }

        #endregion
        
    }
}
