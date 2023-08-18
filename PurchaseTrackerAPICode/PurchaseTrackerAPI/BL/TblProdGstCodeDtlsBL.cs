using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblProdGstCodeDtlsBL : ITblProdGstCodeDtlsBL
    {
        private readonly ITblProdGstCodeDtlsDAO _iTblProdGstCodeDtlsDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly Icommondao _iCommonDAO;
        public TblProdGstCodeDtlsBL(IConnectionString iConnectionString, Icommondao icommondao, ITblProdGstCodeDtlsDAO iTblProdGstCodeDtlsDAO)
        {
            _iCommonDAO = icommondao;
            _iTblProdGstCodeDtlsDAO = iTblProdGstCodeDtlsDAO;
            _iConnectionString = iConnectionString;
        }
        #region Selection
        public  List<TblProdGstCodeDtlsTO> SelectAllTblProdGstCodeDtlsList(Int32 gstCodeId = 0)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblProdGstCodeDtlsDAO.SelectAllTblProdGstCodeDtls(gstCodeId,conn,tran);
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

        public  List<TblProdGstCodeDtlsTO> SelectAllTblProdGstCodeDtlsList(Int32 gstCodeId ,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblProdGstCodeDtlsDAO.SelectAllTblProdGstCodeDtls(gstCodeId, conn, tran);
        }

        public  TblProdGstCodeDtlsTO SelectTblProdGstCodeDtlsTO(Int32 idProdGstCode)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblProdGstCodeDtlsDAO.SelectTblProdGstCodeDtls(idProdGstCode, conn, tran);
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

        public  TblProdGstCodeDtlsTO SelectTblProdGstCodeDtlsTO(Int32 idProdGstCode, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProdGstCodeDtlsDAO.SelectTblProdGstCodeDtls(idProdGstCode, conn, tran);
        }


        public  List<TblProdGstCodeDtlsTO> SelectTblProdGstCodeDtlsTOList(String idProdGstCodes)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return SelectTblProdGstCodeDtlsTOList(idProdGstCodes, conn, tran);
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


        public  List<TblProdGstCodeDtlsTO> SelectTblProdGstCodeDtlsTOList(String idProdGstCodes, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProdGstCodeDtlsDAO.SelectTblProdGstCodeDtls(idProdGstCodes, conn, tran);
        }

        public  TblProdGstCodeDtlsTO SelectTblProdGstCodeDtlsTO(Int32 prodCatId, Int32 prodSpecId,Int32 materialId, Int32 prodItemId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblProdGstCodeDtlsDAO.SelectTblProdGstCodeDtls(prodCatId, prodSpecId, materialId, prodItemId, conn, tran);
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

        public  TblProdGstCodeDtlsTO SelectTblProdGstCodeDtlsTO(Int32 prodCatId,Int32 prodSpecId,Int32 materialId, Int32 prodItemId,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblProdGstCodeDtlsDAO.SelectTblProdGstCodeDtls(prodCatId,prodSpecId, materialId,prodItemId, conn,tran);
        }

        #endregion

        #region Insertion
        public  int InsertTblProdGstCodeDtls(TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO)
        {
            return _iTblProdGstCodeDtlsDAO.InsertTblProdGstCodeDtls(tblProdGstCodeDtlsTO);
        }

        public  int InsertTblProdGstCodeDtls(TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProdGstCodeDtlsDAO.InsertTblProdGstCodeDtls(tblProdGstCodeDtlsTO, conn, tran);
        }

        #endregion

        #region Updation

          ResultMessage UpdateProductGstCode(List<TblProdGstCodeDtlsTO> prodGstCodeDtlsTOList, int loginUserId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                int result = 0;
                DateTime serverDate =  _iCommonDAO.ServerDateTime;
                for (int i = 0; i < prodGstCodeDtlsTOList.Count; i++)
                {

                    TblProdGstCodeDtlsTO prodGstCodeDtlsTO = prodGstCodeDtlsTOList[i];
                    TblProdGstCodeDtlsTO existingProdGstCodeDtlsTO = SelectTblProdGstCodeDtlsTO(prodGstCodeDtlsTO.ProdCatId, prodGstCodeDtlsTO.ProdSpecId, prodGstCodeDtlsTO.MaterialId, prodGstCodeDtlsTO.ProdItemId, conn, tran);
                    if (existingProdGstCodeDtlsTO != null)
                    {
                        //Update and Deactivate the Linkage
                        existingProdGstCodeDtlsTO.EffectiveTodt = serverDate;
                        existingProdGstCodeDtlsTO.IsActive = prodGstCodeDtlsTO.IsActive;
                        existingProdGstCodeDtlsTO.UpdatedBy = loginUserId;
                        existingProdGstCodeDtlsTO.UpdatedOn = serverDate;
                        result = UpdateTblProdGstCodeDtls(existingProdGstCodeDtlsTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While UpdateTblProdGstCodeDtls");
                            return resultMessage;
                        }
                    }
                    else
                    {
                        prodGstCodeDtlsTO.CreatedBy = loginUserId;
                        prodGstCodeDtlsTO.CreatedOn = serverDate;
                        prodGstCodeDtlsTO.IsActive = prodGstCodeDtlsTO.IsActive;
                        prodGstCodeDtlsTO.EffectiveFromDt = serverDate.AddSeconds(1);
                        prodGstCodeDtlsTO.EffectiveTodt = DateTime.MinValue;
                        result = InsertTblProdGstCodeDtls(prodGstCodeDtlsTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While InsertTblProdGstCodeDtls");
                            return resultMessage;
                        }
                    }
                }

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateProductGstCode");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public  int UpdateTblProdGstCodeDtls(TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO)
        {
            return _iTblProdGstCodeDtlsDAO.UpdateTblProdGstCodeDtls(tblProdGstCodeDtlsTO);
        }

        public  int UpdateTblProdGstCodeDtls(TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProdGstCodeDtlsDAO.UpdateTblProdGstCodeDtls(tblProdGstCodeDtlsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblProdGstCodeDtls(Int32 idProdGstCode)
        {
            return _iTblProdGstCodeDtlsDAO.DeleteTblProdGstCodeDtls(idProdGstCode);
        }

        public  int DeleteTblProdGstCodeDtls(Int32 idProdGstCode, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProdGstCodeDtlsDAO.DeleteTblProdGstCodeDtls(idProdGstCode, conn, tran);
        }

       

        #endregion

    }
}
