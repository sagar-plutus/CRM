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
    public class TblPurchaseCompetitorExtBL
    {
        #region Selection
       
        public static List<TblPurchaseCompetitorExtTO> SelectAllTblPurchaseCompetitorExtList()
        {
            return  TblPurchaseCompetitorExtDAO.SelectAllTblPurchaseCompetitorExt();
        }

        public static TblPurchaseCompetitorExtTO SelectTblPurchaseCompetitorExtTO(Int32 idPurCompetitorExt)
        {
            return  TblPurchaseCompetitorExtDAO.SelectTblPurchaseCompetitorExt(idPurCompetitorExt);
        }

        #endregion
        
        #region Insertion
        public static int InsertTblPurchaseCompetitorExt(TblPurchaseCompetitorExtTO tblPurchaseCompetitorExtTO)
        {
            return TblPurchaseCompetitorExtDAO.InsertTblPurchaseCompetitorExt(tblPurchaseCompetitorExtTO);
        }

        public static int InsertTblPurchaseCompetitorExt(TblPurchaseCompetitorExtTO tblPurchaseCompetitorExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblPurchaseCompetitorExtDAO.InsertTblPurchaseCompetitorExt(tblPurchaseCompetitorExtTO, conn, tran);
        }

        /// <summary>
        ///  Priyanka [16-02-18]: Added to get Purchase Competitor Details
        /// </summary>
        /// <returns></returns>
        public static List<TblPurchaseCompetitorExtTO> SelectAllTblPurchaseCompetitorExtList(Int32 organizationId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return SelectAllTblPurchaseCompetitorExtList(organizationId, conn, tran);
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

        public static List<TblPurchaseCompetitorExtTO> SelectAllTblPurchaseCompetitorExtList(Int32 organizationId, SqlConnection conn, SqlTransaction tran)
        {
            return TblPurchaseCompetitorExtDAO.SelectAllTblPurchaseCompetitorExt(organizationId, conn, tran);

        }

        #endregion

        #region Updation
        public static int UpdateTblPurchaseCompetitorExt(TblPurchaseCompetitorExtTO tblPurchaseCompetitorExtTO)
        {
            return TblPurchaseCompetitorExtDAO.UpdateTblPurchaseCompetitorExt(tblPurchaseCompetitorExtTO);
        }

        public static int UpdateTblPurchaseCompetitorExt(TblPurchaseCompetitorExtTO tblPurchaseCompetitorExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblPurchaseCompetitorExtDAO.UpdateTblPurchaseCompetitorExt(tblPurchaseCompetitorExtTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblPurchaseCompetitorExt(Int32 idPurCompetitorExt)
        {
            return TblPurchaseCompetitorExtDAO.DeleteTblPurchaseCompetitorExt(idPurCompetitorExt);
        }

        public static int DeleteTblPurchaseCompetitorExt(Int32 idPurCompetitorExt, SqlConnection conn, SqlTransaction tran)
        {
            return TblPurchaseCompetitorExtDAO.DeleteTblPurchaseCompetitorExt(idPurCompetitorExt, conn, tran);
        }

        #endregion
        
    }
}
