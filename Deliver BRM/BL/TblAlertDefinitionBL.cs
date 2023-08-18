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

namespace SalesTrackerAPI.BL
{
    public class TblAlertDefinitionBL
    {
        #region Selection
        public static List<TblAlertDefinitionTO> SelectAllTblAlertDefinitionList()
        {
            return  TblAlertDefinitionDAO.SelectAllTblAlertDefinition();
        }

        public static TblAlertDefinitionTO SelectTblAlertDefinitionTO(Int32 idAlertDef)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblAlertDefinitionBL.SelectTblAlertDefinitionTO(idAlertDef, conn, tran);
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

        public static TblAlertDefinitionTO SelectTblAlertDefinitionTO(Int32 idAlertDef,SqlConnection conn,SqlTransaction tran)
        {
            TblAlertDefinitionTO tblAlertDefinitionTO= TblAlertDefinitionDAO.SelectTblAlertDefinition(idAlertDef, conn, tran);
            if (tblAlertDefinitionTO != null)
                tblAlertDefinitionTO.AlertSubscribersTOList = BL.TblAlertSubscribersBL.SelectAllTblAlertSubscribersList(tblAlertDefinitionTO.IdAlertDef, conn, tran);

            return tblAlertDefinitionTO;

        }

        #endregion

        #region Insertion
        public static int InsertTblAlertDefinition(TblAlertDefinitionTO tblAlertDefinitionTO)
        {
            return TblAlertDefinitionDAO.InsertTblAlertDefinition(tblAlertDefinitionTO);
        }

        public static int InsertTblAlertDefinition(TblAlertDefinitionTO tblAlertDefinitionTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertDefinitionDAO.InsertTblAlertDefinition(tblAlertDefinitionTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblAlertDefinition(TblAlertDefinitionTO tblAlertDefinitionTO)
        {
            return TblAlertDefinitionDAO.UpdateTblAlertDefinition(tblAlertDefinitionTO);
        }

        public static int UpdateTblAlertDefinition(TblAlertDefinitionTO tblAlertDefinitionTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertDefinitionDAO.UpdateTblAlertDefinition(tblAlertDefinitionTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblAlertDefinition(Int32 idAlertDef)
        {
            return TblAlertDefinitionDAO.DeleteTblAlertDefinition(idAlertDef);
        }

        public static int DeleteTblAlertDefinition(Int32 idAlertDef, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertDefinitionDAO.DeleteTblAlertDefinition(idAlertDef, conn, tran);
        }

        #endregion
        
    }
}
