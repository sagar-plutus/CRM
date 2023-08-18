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

   
    public class TblPurchaseDocToVerifyBL : ITblPurchaseDocToVerifyBL
    {
        private readonly ITblPurchaseDocToVerifyDAO _iTblPurchaseDocToVerifyDAO;
        public TblPurchaseDocToVerifyBL(ITblPurchaseDocToVerifyDAO iTblPurchaseDocToVerifyDAO)
        {
            _iTblPurchaseDocToVerifyDAO = iTblPurchaseDocToVerifyDAO;
        }
        #region Selection
        public  List<TblPurchaseDocToVerifyTO> SelectAllTblPurchaseDocToVerify()
        {
            return _iTblPurchaseDocToVerifyDAO.SelectAllTblPurchaseDocToVerify();
        }

        public  List<TblPurchaseDocToVerifyTO> SelectAllTblPurchaseDocToVerifyList()
        {
            return  _iTblPurchaseDocToVerifyDAO.SelectAllTblPurchaseDocToVerify();
        }

        public  TblPurchaseDocToVerifyTO SelectTblPurchaseDocToVerifyTO(Int32 idPurchaseDocType)
        {
           return _iTblPurchaseDocToVerifyDAO.SelectTblPurchaseDocToVerify(idPurchaseDocType);
        }

       
        #endregion
        
        #region Insertion
        public  int InsertTblPurchaseDocToVerify(TblPurchaseDocToVerifyTO tblPurchaseDocToVerifyTO)
        {
            return _iTblPurchaseDocToVerifyDAO.InsertTblPurchaseDocToVerify(tblPurchaseDocToVerifyTO);
        }

        public  int InsertTblPurchaseDocToVerify(TblPurchaseDocToVerifyTO tblPurchaseDocToVerifyTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseDocToVerifyDAO.InsertTblPurchaseDocToVerify(tblPurchaseDocToVerifyTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblPurchaseDocToVerify(TblPurchaseDocToVerifyTO tblPurchaseDocToVerifyTO)
        {
            return _iTblPurchaseDocToVerifyDAO.UpdateTblPurchaseDocToVerify(tblPurchaseDocToVerifyTO);
        }

        public  int UpdateTblPurchaseDocToVerify(TblPurchaseDocToVerifyTO tblPurchaseDocToVerifyTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseDocToVerifyDAO.UpdateTblPurchaseDocToVerify(tblPurchaseDocToVerifyTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblPurchaseDocToVerify(Int32 idPurchaseDocType)
        {
            return _iTblPurchaseDocToVerifyDAO.DeleteTblPurchaseDocToVerify(idPurchaseDocType);
        }

        public  int DeleteTblPurchaseDocToVerify(Int32 idPurchaseDocType, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseDocToVerifyDAO.DeleteTblPurchaseDocToVerify(idPurchaseDocType, conn, tran);
        }

        #endregion
        
    }
}
