using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL
{
    public class TblPurchaseInvoiceDocumentsBL : ITblPurchaseInvoiceDocumentsBL
    {

        private readonly ITblPurchaseDocToVerifyBL _iTblPurchaseDocToVerifyBL;
        private readonly ITblPurchaseInvoiceDocumentsDAO _iTblPurchaseInvoiceDocumentsDAO;
        public TblPurchaseInvoiceDocumentsBL(ITblPurchaseInvoiceDocumentsDAO iTblPurchaseInvoiceDocumentsDAO, ITblPurchaseDocToVerifyBL iTblPurchaseDocToVerifyBL)
        {
            _iTblPurchaseDocToVerifyBL = iTblPurchaseDocToVerifyBL;
            _iTblPurchaseInvoiceDocumentsDAO = iTblPurchaseInvoiceDocumentsDAO;
        }
        #region Selection
        public  List<TblPurchaseInvoiceDocumentsTO> SelectAllTblPurchaseInvoiceDocuments()
        {
            return _iTblPurchaseInvoiceDocumentsDAO.SelectAllTblPurchaseInvoiceDocuments();
        }
         public  List<TblPurchaseInvoiceDocumentsTO> SelectAllTblPurchaseInvoiceDocuments(Int64 purchaseInvoiceId)
        {
            return _iTblPurchaseInvoiceDocumentsDAO.SelectAllTblPurchaseInvoiceDocuments(purchaseInvoiceId);
        }
        //Priyanka [06-02-19]
        public  List<TblPurchaseInvoiceDocumentsTO> SelectAllTblPurchaseDocToVerifyWithDocDtls()
        {

            List<TblPurchaseInvoiceDocumentsTO> tblPurchaseInvoiceDocumentsTOList = new List<TblPurchaseInvoiceDocumentsTO>();
            List<TblPurchaseDocToVerifyTO> tblPurchaseDocToVerifyTOList = _iTblPurchaseDocToVerifyBL.SelectAllTblPurchaseDocToVerify();
            if(tblPurchaseDocToVerifyTOList != null || tblPurchaseDocToVerifyTOList.Count> 0)
            {
                
                for (int i=0;i < tblPurchaseDocToVerifyTOList.Count; i++)
                {
                    TblPurchaseInvoiceDocumentsTO tblPurchaseInvoiceDocumentsTO = new TblPurchaseInvoiceDocumentsTO();
                    tblPurchaseInvoiceDocumentsTO.DocumentTypeId = tblPurchaseDocToVerifyTOList[i].IdPurchaseDocType;
                    tblPurchaseInvoiceDocumentsTO.DocumentTypeValue = tblPurchaseDocToVerifyTOList[i].PurchaseDocType;
                    tblPurchaseInvoiceDocumentsTO.MasterId = tblPurchaseDocToVerifyTOList[i].MasterId;
                    tblPurchaseInvoiceDocumentsTO.IsFromMaster = tblPurchaseDocToVerifyTOList[i].IsFromMaster;
                    tblPurchaseInvoiceDocumentsTOList.Add(tblPurchaseInvoiceDocumentsTO);
                }
            }
            return tblPurchaseInvoiceDocumentsTOList;
        }


        public  List<TblPurchaseInvoiceDocumentsTO> SelectAllTblPurchaseInvoiceDocumentsList()
        {
            return _iTblPurchaseInvoiceDocumentsDAO.SelectAllTblPurchaseInvoiceDocuments();
        }

        public  TblPurchaseInvoiceDocumentsTO SelectTblPurchaseInvoiceDocumentsTO(Int64 idPurchaseInvDocument)
        {
           return _iTblPurchaseInvoiceDocumentsDAO.SelectTblPurchaseInvoiceDocuments(idPurchaseInvDocument);
        }

        //Priyanka [07-02-2019]
        public  List<TblPurchaseInvoiceDocumentsTO> SelecTblPurDocToVerifyWithDocDtlsAgainstPurInvId(Int64 purchaseInvoiceId)
        {
            return _iTblPurchaseInvoiceDocumentsDAO.SelecTblPurDocToVerifyWithDocDtlsAgainstPurInvId(purchaseInvoiceId);
        }

        #endregion

        #region Insertion
        public  int InsertTblPurchaseInvoiceDocuments(TblPurchaseInvoiceDocumentsTO tblPurchaseInvoiceDocumentsTO)
        {
            return _iTblPurchaseInvoiceDocumentsDAO.InsertTblPurchaseInvoiceDocuments(tblPurchaseInvoiceDocumentsTO);
        }

        public  int InsertTblPurchaseInvoiceDocuments(TblPurchaseInvoiceDocumentsTO tblPurchaseInvoiceDocumentsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceDocumentsDAO.InsertTblPurchaseInvoiceDocuments(tblPurchaseInvoiceDocumentsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblPurchaseInvoiceDocuments(TblPurchaseInvoiceDocumentsTO tblPurchaseInvoiceDocumentsTO)
        {
            return _iTblPurchaseInvoiceDocumentsDAO.UpdateTblPurchaseInvoiceDocuments(tblPurchaseInvoiceDocumentsTO);
        }

        public  int UpdateTblPurchaseInvoiceDocuments(TblPurchaseInvoiceDocumentsTO tblPurchaseInvoiceDocumentsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceDocumentsDAO.UpdateTblPurchaseInvoiceDocuments(tblPurchaseInvoiceDocumentsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblPurchaseInvoiceDocuments(Int64 idPurchaseInvDocument)
        {
            return _iTblPurchaseInvoiceDocumentsDAO.DeleteTblPurchaseInvoiceDocuments(idPurchaseInvDocument);
        }

        public int DeleteTblPurchaseInvoiceDocuments(Int64 idPurchaseInvDocument, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseInvoiceDocumentsDAO.DeleteTblPurchaseInvoiceDocuments(idPurchaseInvDocument, conn, tran);
        }

        #endregion
        
    }
}
