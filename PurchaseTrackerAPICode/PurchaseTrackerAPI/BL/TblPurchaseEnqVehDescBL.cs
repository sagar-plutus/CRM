using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL
{
    public class TblPurchaseEnqVehDescBL : ITblPurchaseEnqVehDescBL
    {
        private readonly ITblPurchaseEnqVehDescDAO _iTblPurchaseEnqVehDescDAO;

        public TblPurchaseEnqVehDescBL(ITblPurchaseEnqVehDescDAO iTblPurchaseEnqVehDescDAO)
        {
            _iTblPurchaseEnqVehDescDAO = iTblPurchaseEnqVehDescDAO;
        }
        #region Selection
        public List<TblPurchaseEnqVehDescTO> SelectAllPurchaseEnqVehDesc()
        {
            return _iTblPurchaseEnqVehDescDAO.SelectAllTblPurchaseEnqVehDesc();
        }

        public List<TblPurchaseEnqVehDescTO> SelectAllTblPurchaseEnqVehDescList()
        {
            return _iTblPurchaseEnqVehDescDAO.SelectAllTblPurchaseEnqVehDesc();
        }

        public List<TblPurchaseEnqVehDescTO> SelectAllTblPurchaseEnqVehDesc(SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnqVehDescDAO.SelectAllTblPurchaseEnqVehDesc(conn, tran);
        }
        public List<TblPurchaseEnqVehDescTO> SelectAllTblPurchaseEnqVehDesc(Int32 purchaseEnqId, SqlConnection conn = null, SqlTransaction tran = null)
{
            if(conn != null && tran != null)
                return _iTblPurchaseEnqVehDescDAO.SelectAllTblPurchaseEnqVehDesc(purchaseEnqId, conn, tran);
            else
                return _iTblPurchaseEnqVehDescDAO.SelectAllTblPurchaseEnqVehDesc(purchaseEnqId);
        }

        public TblPurchaseEnqVehDescTO SelectTblPurchaseEnqVehDescTO(Int32 idVehTypeDesc)
        {
            List <TblPurchaseEnqVehDescTO> tblPurchaseEnqVehDescTOList = _iTblPurchaseEnqVehDescDAO.SelectTblPurchaseEnqVehDesc(idVehTypeDesc);
            if(tblPurchaseEnqVehDescTOList != null && tblPurchaseEnqVehDescTOList.Count == 1)
                return tblPurchaseEnqVehDescTOList[0];
            else
                return null;
        }

      
        #endregion
        
        #region Insertion
        public int InsertTblPurchaseEnqVehDesc(TblPurchaseEnqVehDescTO tblPurchaseEnqVehDescTO)
        {
            return _iTblPurchaseEnqVehDescDAO.InsertTblPurchaseEnqVehDesc(tblPurchaseEnqVehDescTO);
        }

        public  int InsertTblPurchaseEnqVehDesc(TblPurchaseEnqVehDescTO tblPurchaseEnqVehDescTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnqVehDescDAO.InsertTblPurchaseEnqVehDesc(tblPurchaseEnqVehDescTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblPurchaseEnqVehDesc(TblPurchaseEnqVehDescTO tblPurchaseEnqVehDescTO)
        {
            return _iTblPurchaseEnqVehDescDAO.UpdateTblPurchaseEnqVehDesc(tblPurchaseEnqVehDescTO);
        }

        public  int UpdateTblPurchaseEnqVehDesc(TblPurchaseEnqVehDescTO tblPurchaseEnqVehDescTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnqVehDescDAO.UpdateTblPurchaseEnqVehDesc(tblPurchaseEnqVehDescTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblPurchaseEnqVehDesc(Int32 idVehTypeDesc)
        {
            return _iTblPurchaseEnqVehDescDAO.DeleteTblPurchaseEnqVehDesc(idVehTypeDesc);
        }

        public  int DeleteTblPurchaseEnqVehDesc(Int32 idVehTypeDesc, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnqVehDescDAO.DeleteTblPurchaseEnqVehDesc(idVehTypeDesc, conn, tran);
        }

        public int DeletePurchVehDesc(Int32 purchaseEnqId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnqVehDescDAO.DeletePurchVehDesc(purchaseEnqId, conn, tran);
        }

        #endregion

    }
}
