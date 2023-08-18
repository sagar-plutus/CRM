using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Linq;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblRecycleDocumentBL : ITblRecycleDocumentBL
    {
        private readonly Icommondao _iCommonDAO;
        private readonly IConnectionString _iConnectionString;

        private readonly ITblDocumentDetailsBL _iTblDocumentDetailsBL;
        private readonly ITblRecycleDocumentDAO _iTblRecycleDocumentDAO;

        public TblRecycleDocumentBL(
            ITblRecycleDocumentDAO iTblRecycleDocumentDAO
            , IConnectionString iConnectionString,
            Icommondao icommondao,
            ITblDocumentDetailsBL iTblDocumentDetailsBL
            )
        {
            _iConnectionString = iConnectionString;
            _iCommonDAO = icommondao;
            _iTblDocumentDetailsBL = iTblDocumentDetailsBL;
            _iTblRecycleDocumentDAO = iTblRecycleDocumentDAO;
        }
        #region Selection
        public  List<TblRecycleDocumentTO> SelectAllTblRecycleDocument()
        {
            return _iTblRecycleDocumentDAO.SelectAllTblRecycleDocument();
        }

        public  List<TblRecycleDocumentTO> SelectAllTblRecycleDocumentList()
        {
            return _iTblRecycleDocumentDAO.SelectAllTblRecycleDocument();
        }

        public  TblRecycleDocumentTO SelectTblRecycleDocumentTO(Int32 idRecycleDocument)
        {
            List<TblRecycleDocumentTO> tblRecycleDocumentTOList = _iTblRecycleDocumentDAO.SelectTblRecycleDocument(idRecycleDocument);
            if (tblRecycleDocumentTOList != null && tblRecycleDocumentTOList.Count == 1)
                return tblRecycleDocumentTOList[0];
            else
                return null;
        }

        public  List<TblRecycleDocumentTO> SelectRecycleDocumentList(string txnId, Int32 txnTypeId,Int32 isActive, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblRecycleDocumentDAO.SelectRecycleDocumentList(txnId, txnTypeId,isActive, conn, tran);
        }

        public  List<TblDocumentDetailsTO> SelectAllRecycleDocumentList(string txnId, Int32 txnTypeId,Int32 isActive)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            List<TblDocumentDetailsTO> documentDetailsTOList = new List<TblDocumentDetailsTO>();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                List<TblRecycleDocumentTO> tblRecycleDocumentTOList = _iTblRecycleDocumentDAO.SelectRecycleDocumentList(txnId, txnTypeId,isActive, conn, tran);
                if (tblRecycleDocumentTOList != null && tblRecycleDocumentTOList.Count > 0)
                {
                    string documentIds = string.Join(", ", tblRecycleDocumentTOList.Select(i => i.DocumentId));
                    if (!string.IsNullOrEmpty(documentIds))
                    {
                        documentDetailsTOList = _iTblDocumentDetailsBL.SelectAllTblDocumentDetails(documentIds, conn, tran);
                    }
                }

                return documentDetailsTOList;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }


        #endregion

        #region Insertion
        public  int InsertTblRecycleDocument(TblRecycleDocumentTO tblRecycleDocumentTO)
        {
            return _iTblRecycleDocumentDAO.InsertTblRecycleDocument(tblRecycleDocumentTO);
        }

        public  int InsertTblRecycleDocument(TblRecycleDocumentTO tblRecycleDocumentTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblRecycleDocumentDAO.InsertTblRecycleDocument(tblRecycleDocumentTO, conn, tran);
        }

        #endregion

        #region Updation
        public  int UpdateTblRecycleDocument(TblRecycleDocumentTO tblRecycleDocumentTO)
        {
            return _iTblRecycleDocumentDAO.UpdateTblRecycleDocument(tblRecycleDocumentTO);
        }

        public  int UpdateTblRecycleDocument(TblRecycleDocumentTO tblRecycleDocumentTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblRecycleDocumentDAO.UpdateTblRecycleDocument(tblRecycleDocumentTO, conn, tran);
        }

        #endregion

        #region Deletion
        public  int DeleteTblRecycleDocument(Int32 idRecycleDocument)
        {
            return _iTblRecycleDocumentDAO.DeleteTblRecycleDocument(idRecycleDocument);
        }

        public  int DeleteTblRecycleDocument(Int32 idRecycleDocument, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblRecycleDocumentDAO.DeleteTblRecycleDocument(idRecycleDocument, conn, tran);
        }

        #endregion


        public  int SaveUploadedImages(List<TblRecycleDocumentTO> tblRecycleDocumentTOList, Int32 txnId, DateTime currentdate, SqlConnection conn, SqlTransaction tran)
        {
            Int32 result = 0;
            if (tblRecycleDocumentTOList != null && tblRecycleDocumentTOList.Count > 0)
            {
                for (int p = 0; p < tblRecycleDocumentTOList.Count; p++)
                {
                    tblRecycleDocumentTOList[p].TxnId = txnId;
                    tblRecycleDocumentTOList[p].CreatedOn = currentdate;
                    result = InsertTblRecycleDocument(tblRecycleDocumentTOList[p], conn, tran);
                    if (result != 1)
                    {
                        return 0;
                    }
                }
            }
            return result;
        }

        public  ResultMessage PostUploadedImages(List<TblRecycleDocumentTO> tblRecycleDocumentTOList, Int32 loginUserId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            Int32 result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            DateTime currentDate =  _iCommonDAO.ServerDateTime;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                if (tblRecycleDocumentTOList != null && tblRecycleDocumentTOList.Count > 0)
                {

                    //Mark isActive false to already saved images
                    Int32 txnId = tblRecycleDocumentTOList[0].TxnId;
                    Int32 txnTypeId = tblRecycleDocumentTOList[0].TxnTypeId;
                    Int32 isActive=0;

                    List<TblRecycleDocumentTO> tblRecycleDocumentLocalTOList = SelectRecycleDocumentList(txnId.ToString(), txnTypeId,isActive, conn, tran);
                    if (tblRecycleDocumentLocalTOList != null && tblRecycleDocumentLocalTOList.Count > 0)
                    {
                        for (int i = 0; i < tblRecycleDocumentLocalTOList.Count; i++)
                        {
                            tblRecycleDocumentLocalTOList[i].IsActive = 0;
                            tblRecycleDocumentLocalTOList[i].UpdatedBy = loginUserId;
                            tblRecycleDocumentLocalTOList[i].UpdatedOn = currentDate;
                            result = UpdateTblRecycleDocument(tblRecycleDocumentLocalTOList[i], conn, tran);
                            if (result <= 0)
                            {
                                tran.Rollback();
                                throw new Exception("Error while uploading images.");
                            }
                        }
                    }

                    for (int k = 0; k < tblRecycleDocumentTOList.Count; k++)
                    {
                        tblRecycleDocumentTOList[k].CreatedBy = loginUserId;
                        tblRecycleDocumentTOList[k].CreatedOn = currentDate;
                        tblRecycleDocumentTOList[k].IsActive = 1;
                        result = InsertTblRecycleDocument(tblRecycleDocumentTOList[k], conn, tran);
                        if (result <= 0)
                        {
                            tran.Rollback();
                            throw new Exception("Error while uploading images.");
                        }

                    }
                }

                if (result >= 1)
                {
                    tran.Commit();
                    resultMessage.DefaultSuccessBehaviour("Images Uploaded Successfully");

                }
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "PostUploadedImages");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }

        }

    }
}
