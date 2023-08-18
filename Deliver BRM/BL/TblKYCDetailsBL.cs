using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using System.Linq;
using static SalesTrackerAPI.StaticStuff.Constants;


namespace SalesTrackerAPI.BL
{
    public class TblKYCDetailsBL
    {
        #region Selection
        public static List<TblKYCDetailsTO> SelectAllTblKYCDetails()
        {
            return TblKYCDetailsDAO.SelectAllTblKYCDetails();
        }

        public static List<TblKYCDetailsTO> SelectTblKYCDetailsTOByOrgId(Int32 organizationId)
        {
            return TblKYCDetailsDAO.SelectTblKYCDetailsTOByOrgId(organizationId);
        }
        public static TblKYCDetailsTO SelectTblKYCDetailsTO(Int32 idKYCDetails)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblKYCDetailsDAO.SelectTblKYCDetails(idKYCDetails, conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        public static TblKYCDetailsTO SelectTblKYCDetailsTOByOrg(Int32 organizationId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblKYCDetailsDAO.SelectTblKYCDetailsTOByOrgId(organizationId, conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region Insertion
        public static int InsertTblKYCDetails(TblKYCDetailsTO tblKYCDetailsTO)
        {
            return TblKYCDetailsDAO.InsertTblKYCDetails(tblKYCDetailsTO);
        }

        public static int InsertTblKYCDetails(TblKYCDetailsTO tblKYCDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblKYCDetailsDAO.InsertTblKYCDetails(tblKYCDetailsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblKYCDetails(TblKYCDetailsTO tblKYCDetailsTO)
        {
            return TblKYCDetailsDAO.UpdateTblKYCDetails(tblKYCDetailsTO);
        }

        public static int UpdateTblKYCDetails(TblKYCDetailsTO tblKYCDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblKYCDetailsDAO.UpdateTblKYCDetails(tblKYCDetailsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblKYCDetails(Int32 idKYCDetails)
        {
            return TblKYCDetailsDAO.DeleteTblKYCDetails(idKYCDetails);
        }

        public static int DeleteTblKYCDetails(Int32 idKYCDetails, SqlConnection conn, SqlTransaction tran)
        {
            return TblKYCDetailsDAO.DeleteTblKYCDetails(idKYCDetails, conn, tran);
        }

        #endregion
        
    }
}
