using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Z.Expressions;


namespace PurchaseTrackerAPI.BL
{

    
    public class TblPurchaseInvoiceInterfacingDtlBL : ITblPurchaseInvoiceInterfacingDtlBL
    {
        private readonly ITblPurchaseInvoiceInterfacingDtlDAO _iTblPurchaseInvoiceInterfacingDtlDAO;
        public TblPurchaseInvoiceInterfacingDtlBL(ITblPurchaseInvoiceInterfacingDtlDAO iTblPurchaseInvoiceInterfacingDtlDAO)
        {
            _iTblPurchaseInvoiceInterfacingDtlDAO = iTblPurchaseInvoiceInterfacingDtlDAO;
        }
        #region Selection
        public  List<TblPurchaseInvoiceInterfacingDtlTO> SelectAllTblPurchaseInvoiceInterfacingDtl()
        {
            return _iTblPurchaseInvoiceInterfacingDtlDAO.SelectAllTblPurchaseInvoiceInterfacingDtl();
        }

        public  List<TblPurchaseInvoiceInterfacingDtlTO> SelectAllTblPurchaseInvoiceInterfacingDtlList()
        {
            return _iTblPurchaseInvoiceInterfacingDtlDAO.SelectAllTblPurchaseInvoiceInterfacingDtl();
           
        }

        public  TblPurchaseInvoiceInterfacingDtlTO SelectTblPurchaseInvoiceInterfacingDtlTO(Int32 idPurInvInterfacingDtl)
        {
            return _iTblPurchaseInvoiceInterfacingDtlDAO.SelectTblPurchaseInvoiceInterfacingDtl(idPurInvInterfacingDtl);
        }

        public  TblPurchaseInvoiceInterfacingDtlTO SelectTblPurchaseInvoiceInterfacingDtlTOAgainstPurInvId(Int64 PurInvId)
        {
            return _iTblPurchaseInvoiceInterfacingDtlDAO.SelectTblPurchaseInvoiceInterfacingDtlTOAgainstPurInvId(PurInvId);
        }

        public  TblPurchaseInvoiceInterfacingDtlTO SelectTblPurchaseInvoiceInterfacingDtlTOAgainstPurInvId(Int64 PurInvId, SqlConnection conn , SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceInterfacingDtlDAO.SelectTblPurchaseInvoiceInterfacingDtlTOAgainstPurInvId(PurInvId, conn, tran);
        }


        #endregion

        #region Insertion
        public  int InsertTblPurchaseInvoiceInterfacingDtl(TblPurchaseInvoiceInterfacingDtlTO tblPurchaseInvoiceInterfacingDtlTO)
        {
            return _iTblPurchaseInvoiceInterfacingDtlDAO.InsertTblPurchaseInvoiceInterfacingDtl(tblPurchaseInvoiceInterfacingDtlTO);
        }

        public  int InsertTblPurchaseInvoiceInterfacingDtl(TblPurchaseInvoiceInterfacingDtlTO tblPurchaseInvoiceInterfacingDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceInterfacingDtlDAO.InsertTblPurchaseInvoiceInterfacingDtl(tblPurchaseInvoiceInterfacingDtlTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblPurchaseInvoiceInterfacingDtl(TblPurchaseInvoiceInterfacingDtlTO tblPurchaseInvoiceInterfacingDtlTO)
        {
            return _iTblPurchaseInvoiceInterfacingDtlDAO.UpdateTblPurchaseInvoiceInterfacingDtl(tblPurchaseInvoiceInterfacingDtlTO);
        }

        public  int UpdateTblPurchaseInvoiceInterfacingDtl(TblPurchaseInvoiceInterfacingDtlTO tblPurchaseInvoiceInterfacingDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceInterfacingDtlDAO.UpdateTblPurchaseInvoiceInterfacingDtl(tblPurchaseInvoiceInterfacingDtlTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblPurchaseInvoiceInterfacingDtl(Int32 idPurInvInterfacingDtl)
        {
            return _iTblPurchaseInvoiceInterfacingDtlDAO.DeleteTblPurchaseInvoiceInterfacingDtl(idPurInvInterfacingDtl);
        }

        public  int DeleteTblPurchaseInvoiceInterfacingDtl(Int32 idPurInvInterfacingDtl, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceInterfacingDtlDAO.DeleteTblPurchaseInvoiceInterfacingDtl(idPurInvInterfacingDtl, conn, tran);
        }

        #endregion
        
    }
}
