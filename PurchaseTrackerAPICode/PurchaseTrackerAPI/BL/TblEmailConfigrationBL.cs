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
using ODLMSWebAPI.Models;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblEmailConfigrationBL : ITblEmailConfigrationBL
    {
        private readonly ITblEmailConfigrationDAO _iTblEmailConfigrationDAO;
        public TblEmailConfigrationBL(ITblEmailConfigrationDAO iTblEmailConfigrationDAO)
        {
            _iTblEmailConfigrationDAO = iTblEmailConfigrationDAO;
        }
        #region Selection
        public  List<TblEmailConfigrationTO> SelectAllDimEmailConfigration()
        {
            return _iTblEmailConfigrationDAO.SelectAllDimEmailConfigration();
        }

        public  List<TblEmailConfigrationTO> SelectAllDimEmailConfigrationList()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<TblEmailConfigrationTO> list = _iTblEmailConfigrationDAO.SelectAllDimEmailConfigration();
                if (list != null)
                    return list;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllDimEmailConfigrationList");
                return null;
            }
        }

        public  TblEmailConfigrationTO SelectDimEmailConfigrationTO()
        {
            TblEmailConfigrationTO dimEmailConfigrationTODT = _iTblEmailConfigrationDAO.SelectDimEmailConfigrationIsActive();
            if(dimEmailConfigrationTODT !=null)
            {
                return dimEmailConfigrationTODT;
            }
            else
            {
                return null;
            }
        }

        #endregion
        
        #region Insertion
        public  int InsertDimEmailConfigration(TblEmailConfigrationTO dimEmailConfigrationTO)
        {
            return _iTblEmailConfigrationDAO.InsertDimEmailConfigration(dimEmailConfigrationTO);
        }

        public  int InsertDimEmailConfigration(TblEmailConfigrationTO dimEmailConfigrationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblEmailConfigrationDAO.InsertDimEmailConfigration(dimEmailConfigrationTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateDimEmailConfigration(TblEmailConfigrationTO dimEmailConfigrationTO)
        {
            return _iTblEmailConfigrationDAO.UpdateDimEmailConfigration(dimEmailConfigrationTO);
        }

        public  int UpdateDimEmailConfigration(TblEmailConfigrationTO dimEmailConfigrationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblEmailConfigrationDAO.UpdateDimEmailConfigration(dimEmailConfigrationTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteDimEmailConfigration(Int32 idEmailConfig)
        {
            return _iTblEmailConfigrationDAO.DeleteDimEmailConfigration(idEmailConfig);
        }

        public  int DeleteDimEmailConfigration(Int32 idEmailConfig, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblEmailConfigrationDAO.DeleteDimEmailConfigration(idEmailConfig, conn, tran);
        }

        #endregion
        
    }
}
