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
    public class TblAlertDefinitionBL : ITblAlertDefinitionBL
    {
        private readonly IConnectionString _iConnectionString;
        private readonly ITblAlertDefinitionDAO _itblAlertDefinitionDAO;
        private readonly ITblAlertSubscribersBL _iTblAlertSubscribersBL;
        public TblAlertDefinitionBL(ITblAlertDefinitionDAO itblAlertDefinitionDAO , IConnectionString iConnectionString
                                    , ITblAlertSubscribersBL iTblAlertSubscribersBL) {
            _itblAlertDefinitionDAO = itblAlertDefinitionDAO;
            _iConnectionString = iConnectionString;
            _iTblAlertSubscribersBL = iTblAlertSubscribersBL;
        }
        #region Selection
        public  List<TblAlertDefinitionTO> SelectAllTblAlertDefinitionList()
        {
            return  _itblAlertDefinitionDAO.SelectAllTblAlertDefinition();
        }

        public  TblAlertDefinitionTO SelectTblAlertDefinitionTO(Int32 idAlertDef)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _itblAlertDefinitionDAO.SelectTblAlertDefinition(idAlertDef, conn, tran);
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

        public  TblAlertDefinitionTO SelectTblAlertDefinitionTO(Int32 idAlertDef,SqlConnection conn,SqlTransaction tran)
        {
            TblAlertDefinitionTO tblAlertDefinitionTO= _itblAlertDefinitionDAO.SelectTblAlertDefinition(idAlertDef, conn, tran);
            if (tblAlertDefinitionTO != null)
                tblAlertDefinitionTO.AlertSubscribersTOList = _iTblAlertSubscribersBL.SelectAllTblAlertSubscribersList(tblAlertDefinitionTO.IdAlertDef, conn, tran);

            return tblAlertDefinitionTO;

        }

        #endregion

        #region Insertion
        public  int InsertTblAlertDefinition(TblAlertDefinitionTO tblAlertDefinitionTO)
        {
            return _itblAlertDefinitionDAO.InsertTblAlertDefinition(tblAlertDefinitionTO);
        }

        public  int InsertTblAlertDefinition(TblAlertDefinitionTO tblAlertDefinitionTO, SqlConnection conn, SqlTransaction tran)
        {
            return _itblAlertDefinitionDAO.InsertTblAlertDefinition(tblAlertDefinitionTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblAlertDefinition(TblAlertDefinitionTO tblAlertDefinitionTO)
        {
            return _itblAlertDefinitionDAO.UpdateTblAlertDefinition(tblAlertDefinitionTO);
        }

        public  int UpdateTblAlertDefinition(TblAlertDefinitionTO tblAlertDefinitionTO, SqlConnection conn, SqlTransaction tran)
        {
            return _itblAlertDefinitionDAO.UpdateTblAlertDefinition(tblAlertDefinitionTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblAlertDefinition(Int32 idAlertDef)
        {
            return _itblAlertDefinitionDAO.DeleteTblAlertDefinition(idAlertDef);
        }

        public  int DeleteTblAlertDefinition(Int32 idAlertDef, SqlConnection conn, SqlTransaction tran)
        {
            return _itblAlertDefinitionDAO.DeleteTblAlertDefinition(idAlertDef, conn, tran);
        }

        #endregion
        
    }
}
