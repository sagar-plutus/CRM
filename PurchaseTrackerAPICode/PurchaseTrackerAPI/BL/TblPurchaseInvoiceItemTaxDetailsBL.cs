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
    public class TblPurchaseInvoiceItemTaxDetailsBL : ITblPurchaseInvoiceItemTaxDetailsBL
    {
        private readonly ITblPurchaseInvoiceItemTaxDetailsDAO _iTblPurchaseInvoiceItemTaxDetailsDAO;
        public TblPurchaseInvoiceItemTaxDetailsBL(ITblPurchaseInvoiceItemTaxDetailsDAO iTblPurchaseInvoiceItemTaxDetailsDAO)
        {
            _iTblPurchaseInvoiceItemTaxDetailsDAO = iTblPurchaseInvoiceItemTaxDetailsDAO;
        }
        #region Selection
        public  List<TblPurchaseInvoiceItemTaxDetailsTO> SelectAllTblPurchaseInvoiceItemTaxDetails()
        {
            return _iTblPurchaseInvoiceItemTaxDetailsDAO.SelectAllTblPurchaseInvoiceItemTaxDetails();
        }

        public  List<TblPurchaseInvoiceItemTaxDetailsTO> SelectAllTblPurchaseInvoiceItemTaxDetailsList()
        {
            return _iTblPurchaseInvoiceItemTaxDetailsDAO.SelectAllTblPurchaseInvoiceItemTaxDetails();
        }

        public  List<TblPurchaseInvoiceItemTaxDetailsTO> SelectAllTblPurchaseInvoiceItemTaxDetailsList(Int64 purchaseInvoiceItemId)
        {
            return _iTblPurchaseInvoiceItemTaxDetailsDAO.SelectAllTblPurchaseInvoiceItemTaxDetails(purchaseInvoiceItemId);
        }

        public  List<TblPurchaseInvoiceItemTaxDetailsTO> SelectAllTblPurchaseInvoiceItemTaxDetailsList(Int64 purchaseInvoiceItemId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceItemTaxDetailsDAO.SelectAllTblPurchaseInvoiceItemTaxDetails(purchaseInvoiceItemId, conn, tran);
        }

        public  TblPurchaseInvoiceItemTaxDetailsTO SelectTblPurchaseInvoiceItemTaxDetailsTO(Int64 idPurchaseInvItemTaxDtl)
        {
            return _iTblPurchaseInvoiceItemTaxDetailsDAO.SelectTblPurchaseInvoiceItemTaxDetails(idPurchaseInvItemTaxDtl);
        }

  
        #endregion
        
        #region Insertion
        public  int InsertTblPurchaseInvoiceItemTaxDetails(TblPurchaseInvoiceItemTaxDetailsTO tblPurchaseInvoiceItemTaxDetailsTO)
        {
            return _iTblPurchaseInvoiceItemTaxDetailsDAO.InsertTblPurchaseInvoiceItemTaxDetails(tblPurchaseInvoiceItemTaxDetailsTO);
        }

        public  int InsertTblPurchaseInvoiceItemTaxDetails(TblPurchaseInvoiceItemTaxDetailsTO tblPurchaseInvoiceItemTaxDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceItemTaxDetailsDAO.InsertTblPurchaseInvoiceItemTaxDetails(tblPurchaseInvoiceItemTaxDetailsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblPurchaseInvoiceItemTaxDetails(TblPurchaseInvoiceItemTaxDetailsTO tblPurchaseInvoiceItemTaxDetailsTO)
        {
            return _iTblPurchaseInvoiceItemTaxDetailsDAO.UpdateTblPurchaseInvoiceItemTaxDetails(tblPurchaseInvoiceItemTaxDetailsTO);
        }

        public  int UpdateTblPurchaseInvoiceItemTaxDetails(TblPurchaseInvoiceItemTaxDetailsTO tblPurchaseInvoiceItemTaxDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceItemTaxDetailsDAO.UpdateTblPurchaseInvoiceItemTaxDetails(tblPurchaseInvoiceItemTaxDetailsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblPurchaseInvoiceItemTaxDetails(Int64 idPurchaseInvItemTaxDtl)
        {
            return _iTblPurchaseInvoiceItemTaxDetailsDAO.DeleteTblPurchaseInvoiceItemTaxDetails(idPurchaseInvItemTaxDtl);
        }

        public  int DeleteTblPurchaseInvoiceItemTaxDetails(Int64 idPurchaseInvItemTaxDtl, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceItemTaxDetailsDAO.DeleteTblPurchaseInvoiceItemTaxDetails(idPurchaseInvItemTaxDtl, conn, tran);
        }

        public  int DeleteTblPurchaseInvoiceItemTaxDetailsByPurchaseInvoiceId(Int64 purchaseInvoiceId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceItemTaxDetailsDAO.DeleteTblPurchaseInvoiceItemTaxDetailsByPurchaseInvoiceId(purchaseInvoiceId, conn, tran);
        }

        #endregion

    }
}
