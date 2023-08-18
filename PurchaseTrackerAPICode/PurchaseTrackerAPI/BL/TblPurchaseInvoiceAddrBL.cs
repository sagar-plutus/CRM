using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL
{
    public class TblPurchaseInvoiceAddrBL : ITblPurchaseInvoiceAddrBL
    {

        private readonly ITblPurchaseInvoiceAddrDAO _iTblPurchaseInvoiceAddrDAO;
        public TblPurchaseInvoiceAddrBL(ITblPurchaseInvoiceAddrDAO iITblPurchaseInvoiceAddrDAO)
        {
            _iTblPurchaseInvoiceAddrDAO = iITblPurchaseInvoiceAddrDAO;
        }
        #region Selection
        public  List<TblPurchaseInvoiceAddrTO> SelectAllTblPurchaseInvoiceAddr()
        {
            return _iTblPurchaseInvoiceAddrDAO.SelectAllTblPurchaseInvoiceAddr();
        }

        public  List<TblPurchaseInvoiceAddrTO> SelectAllTblPurchaseInvoiceAddrList()
        {
            return _iTblPurchaseInvoiceAddrDAO.SelectAllTblPurchaseInvoiceAddr();
        }

        public  List<TblPurchaseInvoiceAddrTO> SelectAllTblPurchaseInvoiceAddrList(Int64 purchaseInvoiceId)
        {
            return _iTblPurchaseInvoiceAddrDAO.SelectAllTblPurchaseInvoiceAddr(purchaseInvoiceId);
        }

        public  List<TblPurchaseInvoiceAddrTO> SelectAllTblPurchaseInvoiceAddrList(Int64 purchaseInvoiceId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceAddrDAO.SelectAllTblPurchaseInvoiceAddr(purchaseInvoiceId, conn, tran);
        }

        public  TblPurchaseInvoiceAddrTO SelectTblPurchaseInvoiceAddrTO(Int64 idPurchaseInvoiceAddr)
        {
          return _iTblPurchaseInvoiceAddrDAO.SelectTblPurchaseInvoiceAddr(idPurchaseInvoiceAddr);
        }

    
        #endregion
        
        #region Insertion
        public  int InsertTblPurchaseInvoiceAddr(TblPurchaseInvoiceAddrTO tblPurchaseInvoiceAddrTO)
        {
            return _iTblPurchaseInvoiceAddrDAO.InsertTblPurchaseInvoiceAddr(tblPurchaseInvoiceAddrTO);
        }

        public  int InsertTblPurchaseInvoiceAddr(TblPurchaseInvoiceAddrTO tblPurchaseInvoiceAddrTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceAddrDAO.InsertTblPurchaseInvoiceAddr(tblPurchaseInvoiceAddrTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblPurchaseInvoiceAddr(TblPurchaseInvoiceAddrTO tblPurchaseInvoiceAddrTO)
        {
            return _iTblPurchaseInvoiceAddrDAO.UpdateTblPurchaseInvoiceAddr(tblPurchaseInvoiceAddrTO);
        }

        public  int UpdateTblPurchaseInvoiceAddr(TblPurchaseInvoiceAddrTO tblPurchaseInvoiceAddrTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceAddrDAO.UpdateTblPurchaseInvoiceAddr(tblPurchaseInvoiceAddrTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblPurchaseInvoiceAddr(Int64 idPurchaseInvoiceAddr)
        {
            return _iTblPurchaseInvoiceAddrDAO.DeleteTblPurchaseInvoiceAddr(idPurchaseInvoiceAddr);
        }

        public  int DeleteTblPurchaseInvoiceAddr(Int64 idPurchaseInvoiceAddr, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceAddrDAO.DeleteTblPurchaseInvoiceAddr(idPurchaseInvoiceAddr, conn, tran);
        }

        #endregion
        
    }
}
