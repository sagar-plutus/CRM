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
    public class TblPurchaseInvoiceItemDetailsBL : ITblPurchaseInvoiceItemDetailsBL
    {

        private readonly ITblPurchaseInvoiceItemDetailsDAO _iTblPurchaseInvoiceItemDetailsDAO;
        public TblPurchaseInvoiceItemDetailsBL(ITblPurchaseInvoiceItemDetailsDAO iTblPurchaseInvoiceItemDetailsDAO)
        {
            _iTblPurchaseInvoiceItemDetailsDAO = iTblPurchaseInvoiceItemDetailsDAO;
        }
        #region Selection
        public  List<TblPurchaseInvoiceItemDetailsTO> SelectAllTblPurchaseInvoiceItemDetails()
        {
            return _iTblPurchaseInvoiceItemDetailsDAO.SelectAllTblPurchaseInvoiceItemDetails();
        }

        public  List<TblPurchaseInvoiceItemDetailsTO> SelectAllTblPurchaseInvoiceItemDetailsList()
        {
           return _iTblPurchaseInvoiceItemDetailsDAO.SelectAllTblPurchaseInvoiceItemDetails();
        }

        public  List<TblPurchaseInvoiceItemDetailsTO> SelectAllTblPurchaseInvoiceItemDetailsList(Int64 purchaseInvoiceId)
        {
            return _iTblPurchaseInvoiceItemDetailsDAO.SelectAllTblPurchaseInvoiceItemDetails(purchaseInvoiceId);
        }

        public  List<TblPurchaseInvoiceItemDetailsTO> SelectAllTblPurchaseInvoiceItemDetailsList(Int64 purchaseInvoiceId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceItemDetailsDAO.SelectAllTblPurchaseInvoiceItemDetails(purchaseInvoiceId, conn, tran);
        }
         public  List<TblPurchaseInvoiceItemDetailsTO> SelectPurchaseInvoiceItemDtlsForOtherTaxId(Int64 purchaseInvoiceId,Int32 otherTaxTypeId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceItemDetailsDAO.SelectPurchaseInvoiceItemDtlsForOtherTaxId(purchaseInvoiceId,otherTaxTypeId, conn, tran);
        }

        public  TblPurchaseInvoiceItemDetailsTO SelectTblPurchaseInvoiceItemDetailsTO(Int64 idPurchaseInvoiceItem)
        {
           return _iTblPurchaseInvoiceItemDetailsDAO.SelectTblPurchaseInvoiceItemDetails(idPurchaseInvoiceItem);
        }

  
        #endregion
        
        #region Insertion
        public  int InsertTblPurchaseInvoiceItemDetails(TblPurchaseInvoiceItemDetailsTO tblPurchaseInvoiceItemDetailsTO)
        {
            return _iTblPurchaseInvoiceItemDetailsDAO.InsertTblPurchaseInvoiceItemDetails(tblPurchaseInvoiceItemDetailsTO);
        }

        public  int InsertTblPurchaseInvoiceItemDetails(TblPurchaseInvoiceItemDetailsTO tblPurchaseInvoiceItemDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceItemDetailsDAO.InsertTblPurchaseInvoiceItemDetails(tblPurchaseInvoiceItemDetailsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblPurchaseInvoiceItemDetails(TblPurchaseInvoiceItemDetailsTO tblPurchaseInvoiceItemDetailsTO)
        {
            return _iTblPurchaseInvoiceItemDetailsDAO.UpdateTblPurchaseInvoiceItemDetails(tblPurchaseInvoiceItemDetailsTO);
        }

        public  int UpdateTblPurchaseInvoiceItemDetails(TblPurchaseInvoiceItemDetailsTO tblPurchaseInvoiceItemDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceItemDetailsDAO.UpdateTblPurchaseInvoiceItemDetails(tblPurchaseInvoiceItemDetailsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblPurchaseInvoiceItemDetails(Int64 idPurchaseInvoiceItem)
        {
            return _iTblPurchaseInvoiceItemDetailsDAO.DeleteTblPurchaseInvoiceItemDetails(idPurchaseInvoiceItem);
        }

        public  int DeleteTblPurchaseInvoiceItemDetails(Int64 idPurchaseInvoiceItem, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceItemDetailsDAO.DeleteTblPurchaseInvoiceItemDetails(idPurchaseInvoiceItem, conn, tran);
        }

        #endregion
        
    }
}
