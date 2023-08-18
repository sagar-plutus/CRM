using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.BL
{
    public class TblProdGstCodeDtlsBL
    {
        #region Selection
        public static List<TblProdGstCodeDtlsTO> SelectAllTblProdGstCodeDtlsList(Int32 gstCodeId = 0)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblProdGstCodeDtlsDAO.SelectAllTblProdGstCodeDtls(gstCodeId,conn,tran);
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

        public static List<TblProdGstCodeDtlsTO> SelectAllTblProdGstCodeDtlsList(Int32 gstCodeId ,SqlConnection conn,SqlTransaction tran)
        {
            return TblProdGstCodeDtlsDAO.SelectAllTblProdGstCodeDtls(gstCodeId, conn, tran);
        }

        public static TblProdGstCodeDtlsTO SelectTblProdGstCodeDtlsTO(Int32 idProdGstCode)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblProdGstCodeDtlsDAO.SelectTblProdGstCodeDtls(idProdGstCode, conn, tran);
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

        public static TblProdGstCodeDtlsTO SelectTblProdGstCodeDtlsTO(Int32 idProdGstCode, SqlConnection conn, SqlTransaction tran)
        {
            return TblProdGstCodeDtlsDAO.SelectTblProdGstCodeDtls(idProdGstCode, conn, tran);
        }


        public static List<TblProdGstCodeDtlsTO> SelectTblProdGstCodeDtlsTOList(String idProdGstCodes)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblProdGstCodeDtlsBL.SelectTblProdGstCodeDtlsTOList(idProdGstCodes, conn, tran);
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


        public static List<TblProdGstCodeDtlsTO> SelectTblProdGstCodeDtlsTOList(String idProdGstCodes, SqlConnection conn, SqlTransaction tran)
        {
            return TblProdGstCodeDtlsDAO.SelectTblProdGstCodeDtls(idProdGstCodes, conn, tran);
        }

        public static TblProdGstCodeDtlsTO SelectTblProdGstCodeDtlsTO(Int32 prodCatId, Int32 prodSpecId,Int32 materialId, Int32 prodItemId, Int32 prodClassId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblProdGstCodeDtlsDAO.SelectTblProdGstCodeDtls(prodCatId, prodSpecId, materialId, prodItemId, prodClassId, conn, tran);
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

        public static TblProdGstCodeDtlsTO SelectTblProdGstCodeDtlsTO(Int32 prodCatId,Int32 prodSpecId,Int32 materialId, Int32 prodItemId, Int32 prodClassId, SqlConnection conn,SqlTransaction tran)
        {
            return TblProdGstCodeDtlsDAO.SelectTblProdGstCodeDtls(prodCatId, prodSpecId, materialId, prodItemId, prodClassId, conn, tran);
        }

        #endregion

        #region Insertion
        public static int InsertTblProdGstCodeDtls(TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO)
        {
            return TblProdGstCodeDtlsDAO.InsertTblProdGstCodeDtls(tblProdGstCodeDtlsTO);
        }

        public static int InsertTblProdGstCodeDtls(TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblProdGstCodeDtlsDAO.InsertTblProdGstCodeDtls(tblProdGstCodeDtlsTO, conn, tran);
        }

        #endregion

        #region Updation

        internal static ResultMessage UpdateProductGstCode(List<TblProdGstCodeDtlsTO> prodGstCodeDtlsTOList, int loginUserId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                int result = 0;
                DateTime serverDate = Constants.ServerDateTime;
                for (int i = 0; i < prodGstCodeDtlsTOList.Count; i++)
                {

                    TblProdGstCodeDtlsTO prodGstCodeDtlsTO = prodGstCodeDtlsTOList[i];
                    TblProdGstCodeDtlsTO existingProdGstCodeDtlsTO = SelectTblProdGstCodeDtlsTO(prodGstCodeDtlsTO.ProdCatId, prodGstCodeDtlsTO.ProdSpecId, prodGstCodeDtlsTO.MaterialId, prodGstCodeDtlsTO.ProdItemId, prodGstCodeDtlsTO.ProdClassId, conn, tran);
                 
                    //if (existingProdGstCodeDtlsTO != null && prodGstCodeDtlsTO.ProdClassId != 0)
                    //{
                    //    if (existingProdGstCodeDtlsTO.ProdClassId != prodGstCodeDtlsTO.ProdClassId)
                    //    {
                    //        existingProdGstCodeDtlsTO = null;
                    //    }
                    //}
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

        public static int UpdateTblProdGstCodeDtls(TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO)
        {
            return TblProdGstCodeDtlsDAO.UpdateTblProdGstCodeDtls(tblProdGstCodeDtlsTO);
        }

        public static int UpdateTblProdGstCodeDtls(TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblProdGstCodeDtlsDAO.UpdateTblProdGstCodeDtls(tblProdGstCodeDtlsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblProdGstCodeDtls(Int32 idProdGstCode)
        {
            return TblProdGstCodeDtlsDAO.DeleteTblProdGstCodeDtls(idProdGstCode);
        }

        public static int DeleteTblProdGstCodeDtls(Int32 idProdGstCode, SqlConnection conn, SqlTransaction tran)
        {
            return TblProdGstCodeDtlsDAO.DeleteTblProdGstCodeDtls(idProdGstCode, conn, tran);
        }

       

        #endregion

    }
}
