using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL
{
    public class TblGateBL: ITblGateBL
    {
        private readonly ITblGateDAO _iTblGateDAO;
        public TblGateBL(ITblGateDAO iTblGateDAO)
        {
            _iTblGateDAO = iTblGateDAO;
        }
        #region Selection

        public  List<TblGateTO> SelectAllTblGateList(StaticStuff.Constants.ActiveSelectionTypeE ActiveSelectionTypeE)
        {
            return _iTblGateDAO.SelectAllTblGate(ActiveSelectionTypeE);
        }

        public  TblGateTO SelectTblGateTO(Int32 idGate)
        {
            return _iTblGateDAO.SelectTblGate(idGate);
        }


        public  TblGateTO GetDefaultTblGateTO()
        {
            TblGateTO defaultTO = null;

            List<TblGateTO> tblGateTOList = SelectAllTblGateList(StaticStuff.Constants.ActiveSelectionTypeE.Active);
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
        public  int InsertTblGate(TblGateTO tblGateTO)
        {
            return _iTblGateDAO.InsertTblGate(tblGateTO);
        }

        public  int InsertTblGate(TblGateTO tblGateTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblGateDAO.InsertTblGate(tblGateTO, conn, tran);
        }

        #endregion

        #region Updation
        public  int UpdateTblGate(TblGateTO tblGateTO)
        {
            return _iTblGateDAO.UpdateTblGate(tblGateTO);
        }

        public  int UpdateTblGate(TblGateTO tblGateTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblGateDAO.UpdateTblGate(tblGateTO, conn, tran);
        }

        #endregion

        #region Deletion
        public  int DeleteTblGate(Int32 idGate)
        {
            return _iTblGateDAO.DeleteTblGate(idGate);
        }

        public  int DeleteTblGate(Int32 idGate, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblGateDAO.DeleteTblGate(idGate, conn, tran);
        }

        #endregion

    }
}
