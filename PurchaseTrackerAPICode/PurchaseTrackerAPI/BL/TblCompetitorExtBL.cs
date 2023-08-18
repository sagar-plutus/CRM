using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System.Linq;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblCompetitorExtBL : ITblCompetitorExtBL
    {
        private readonly IConnectionString _iConnectionString;
        private readonly ITblCompetitorExtDAO _itblCompetitorExtDAO;
        public TblCompetitorExtBL(ITblCompetitorExtDAO itblCompetitorExtDAO, IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
            _itblCompetitorExtDAO = itblCompetitorExtDAO;

        }
        #region Selection

        public  List<TblCompetitorExtTO> SelectAllTblCompetitorExtList()
        {
            return _itblCompetitorExtDAO.SelectAllTblCompetitorExt();

        }

        public  TblCompetitorExtTO SelectTblCompetitorExtTO(Int32 idCompetitorExt)
        {
            return _itblCompetitorExtDAO.SelectTblCompetitorExt(idCompetitorExt);
        }

        public  List<DropDownTO> SelectCompetitorBrandNamesDropDownList(Int32 competitorOrgId)
        {
            return _itblCompetitorExtDAO.SelectCompetitorBrandNamesDropDownList(competitorOrgId);
        }

        public  List<TblCompetitorExtTO> SelectAllTblCompetitorExtList(Int32 orgId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return SelectAllTblCompetitorExtList(orgId, conn, tran);
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

        public  List<TblCompetitorExtTO> SelectAllTblCompetitorExtList(Int32 orgId, SqlConnection conn, SqlTransaction tran)
        {
            return _itblCompetitorExtDAO.SelectAllTblCompetitorExt(orgId, conn, tran);

        }
        

        

        #endregion

        #region Insertion
        public  int InsertTblCompetitorExt(TblCompetitorExtTO tblCompetitorExtTO)
        {
            return _itblCompetitorExtDAO.InsertTblCompetitorExt(tblCompetitorExtTO);
        }

        public  int InsertTblCompetitorExt(TblCompetitorExtTO tblCompetitorExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return _itblCompetitorExtDAO.InsertTblCompetitorExt(tblCompetitorExtTO, conn, tran);
        }

        #endregion

        #region Updation
        public  int UpdateTblCompetitorExt(TblCompetitorExtTO tblCompetitorExtTO)
        {
            return _itblCompetitorExtDAO.UpdateTblCompetitorExt(tblCompetitorExtTO);
        }

        public  int UpdateTblCompetitorExt(TblCompetitorExtTO tblCompetitorExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return _itblCompetitorExtDAO.UpdateTblCompetitorExt(tblCompetitorExtTO, conn, tran);
        }

        #endregion

        #region Deletion
        public  int DeleteTblCompetitorExt(Int32 idCompetitorExt)
        {
            return _itblCompetitorExtDAO.DeleteTblCompetitorExt(idCompetitorExt);
        }

        public  int DeleteTblCompetitorExt(Int32 idCompetitorExt, SqlConnection conn, SqlTransaction tran)
        {
            return _itblCompetitorExtDAO.DeleteTblCompetitorExt(idCompetitorExt, conn, tran);
        }

        #endregion

    }
}
