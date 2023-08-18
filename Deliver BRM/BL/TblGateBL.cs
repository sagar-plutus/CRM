using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.StaticStuff;
using System.Linq;

namespace SalesTrackerAPI.BL
{
    public class TblGateBL
    {
        #region Selection

        public static List<TblGateTO> SelectAllTblGateList(Constants.ActiveSelectionTypeE ActiveSelectionTypeE)
        {
            return TblGateDAO.SelectAllTblGate(ActiveSelectionTypeE);
        }

        public static TblGateTO SelectTblGateTO(Int32 idGate)
        {
            return TblGateDAO.SelectTblGate(idGate);
        }


        public static TblGateTO GetDefaultTblGateTO()
        {
            TblGateTO defaultTO = null;

            List<TblGateTO> tblGateTOList = SelectAllTblGateList(Constants.ActiveSelectionTypeE.Active);
            if (tblGateTOList != null && tblGateTOList.Count > 0)
            {
                defaultTO = tblGateTOList.Where(w => w.IsDefault == 1).FirstOrDefault();
                if (defaultTO == null)
                {
                    defaultTO = tblGateTOList[0];
                }
            }

            return defaultTO;

        }


        #endregion

        #region Insertion
        public static int InsertTblGate(TblGateTO tblGateTO)
        {
            return TblGateDAO.InsertTblGate(tblGateTO);
        }

        public static int InsertTblGate(TblGateTO tblGateTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblGateDAO.InsertTblGate(tblGateTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblGate(TblGateTO tblGateTO)
        {
            return TblGateDAO.UpdateTblGate(tblGateTO);
        }

        public static int UpdateTblGate(TblGateTO tblGateTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblGateDAO.UpdateTblGate(tblGateTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblGate(Int32 idGate)
        {
            return TblGateDAO.DeleteTblGate(idGate);
        }

        public static int DeleteTblGate(Int32 idGate, SqlConnection conn, SqlTransaction tran)
        {
            return TblGateDAO.DeleteTblGate(idGate, conn, tran);
        }

        #endregion
        
    }
}
