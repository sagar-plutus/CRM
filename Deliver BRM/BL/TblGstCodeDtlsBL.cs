using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using System.Linq;

namespace SalesTrackerAPI.BL
{
    public class TblGstCodeDtlsBL
    {
        #region Selection
       
        public static List<TblGstCodeDtlsTO> SelectAllTblGstCodeDtlsList()
        {
            return  TblGstCodeDtlsDAO.SelectAllTblGstCodeDtls();
        }

        public static TblGstCodeDtlsTO SelectTblGstCodeDtlsTO(Int32 idGstCode)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblGstCodeDtlsDAO.SelectTblGstCodeDtls(idGstCode, conn, tran);
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

        public static List<TblGstCodeDtlsTO> SelectTblGstCodeDtlsTOList(String idGstCodes)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblGstCodeDtlsBL.SelectTblGstCodeDtlsTOList(idGstCodes, conn, tran);
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

        public static TblGstCodeDtlsTO SelectTblGstCodeDtlsTO(Int32 idGstCode,SqlConnection conn,SqlTransaction tran)
        {
            return TblGstCodeDtlsDAO.SelectTblGstCodeDtls(idGstCode, conn, tran);
        }


        public static List<TblGstCodeDtlsTO> SelectTblGstCodeDtlsTOList(String idGstCodes, SqlConnection conn, SqlTransaction tran)
        {
            return TblGstCodeDtlsDAO.SelectTblGstCodeDtlsList(idGstCodes, conn, tran);
        }

        #endregion

        #region Insertion
        public static int InsertTblGstCodeDtls(TblGstCodeDtlsTO tblGstCodeDtlsTO)
        {
            return TblGstCodeDtlsDAO.InsertTblGstCodeDtls(tblGstCodeDtlsTO);
        }

        public static int InsertTblGstCodeDtls(TblGstCodeDtlsTO tblGstCodeDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblGstCodeDtlsDAO.InsertTblGstCodeDtls(tblGstCodeDtlsTO, conn, tran);
        }

        internal static ResultMessage SaveNewGSTCodeDetails(TblGstCodeDtlsTO gstCodeDtlsTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMsg = new ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                int result = InsertTblGstCodeDtls(gstCodeDtlsTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMsg.DefaultBehaviour("Error While InsertTblGstCodeDtls");
                    return resultMsg;
                }

                if (gstCodeDtlsTO.TaxRatesTOList == null)
                {
                    tran.Rollback();
                    resultMsg.DefaultBehaviour("TaxRatesTOList Found NULL");
                    return resultMsg;
                }

                for (int i = 0; i < gstCodeDtlsTO.TaxRatesTOList.Count; i++)
                {
                    gstCodeDtlsTO.TaxRatesTOList[i].CreatedBy = gstCodeDtlsTO.CreatedBy;
                    gstCodeDtlsTO.TaxRatesTOList[i].CreatedOn = gstCodeDtlsTO.CreatedOn;
                    gstCodeDtlsTO.TaxRatesTOList[i].IsActive = 1;
                    gstCodeDtlsTO.TaxRatesTOList[i].GstCodeId = gstCodeDtlsTO.IdGstCode;
                    result = BL.TblTaxRatesBL.InsertTblTaxRates(gstCodeDtlsTO.TaxRatesTOList[i], conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMsg.DefaultBehaviour("Error While InsertTblTaxRates");
                        return resultMsg;
                    }
                }

                tran.Commit();
                resultMsg.DefaultSuccessBehaviour();
                return resultMsg;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMsg.DefaultExceptionBehaviour(ex, "SaveNewGSTCodeDetails");
                return resultMsg;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region Updation
        public static int UpdateTblGstCodeDtls(TblGstCodeDtlsTO tblGstCodeDtlsTO)
        {
            return TblGstCodeDtlsDAO.UpdateTblGstCodeDtls(tblGstCodeDtlsTO);
        }

        public static int UpdateTblGstCodeDtls(TblGstCodeDtlsTO tblGstCodeDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblGstCodeDtlsDAO.UpdateTblGstCodeDtls(tblGstCodeDtlsTO, conn, tran);
        }

        internal static ResultMessage UpdateNewGSTCodeDetails(TblGstCodeDtlsTO gstCodeDtlsTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMsg = new ResultMessage();
            try
            {
                TblGstCodeDtlsTO existingGstCodeDtlsTO = SelectTblGstCodeDtlsTO(gstCodeDtlsTO.IdGstCode);

                conn.Open();
                tran = conn.BeginTransaction();

                if(existingGstCodeDtlsTO==null)
                {
                    tran.Rollback();
                    resultMsg.DefaultBehaviour("existingGstCodeDtlsTO found NULL");
                    return resultMsg;
                }
                int result = 0;
                List<TblTaxRatesTO> list = BL.TblTaxRatesBL.SelectAllTblTaxRatesList(existingGstCodeDtlsTO.IdGstCode, conn, tran);
                List<TblProdGstCodeDtlsTO> prodItemList = BL.TblProdGstCodeDtlsBL.SelectAllTblProdGstCodeDtlsList(existingGstCodeDtlsTO.IdGstCode, conn, tran);
                if (existingGstCodeDtlsTO.TaxPct != gstCodeDtlsTO.TaxPct)
                {
                    existingGstCodeDtlsTO.EffectiveToDt = gstCodeDtlsTO.UpdatedOn;
                    existingGstCodeDtlsTO.IsActive = 0;
                    existingGstCodeDtlsTO.UpdatedBy = gstCodeDtlsTO.UpdatedBy;
                    existingGstCodeDtlsTO.UpdatedOn = gstCodeDtlsTO.UpdatedOn;

                    result = UpdateTblGstCodeDtls(existingGstCodeDtlsTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMsg.DefaultBehaviour("Error While InsertTblGstCodeDtls");
                        return resultMsg;
                    }

                    //Deactivate all taxes

                    for (int t = 0; t < list.Count; t++)
                    {
                        list[t].IsActive = 0;
                        list[t].EffectiveToDt = existingGstCodeDtlsTO.EffectiveToDt;
                        list[t].UpdatedBy = existingGstCodeDtlsTO.UpdatedBy;
                        list[t].UpdatedOn = existingGstCodeDtlsTO.UpdatedOn;
                        result = BL.TblTaxRatesBL.UpdateTblTaxRates(list[t], conn, tran);

                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMsg.DefaultBehaviour("Error While UpdateTblTaxRates");
                            return resultMsg;
                        }
                    }

                    for (int pi = 0; pi < prodItemList.Count; pi++)
                    {
                        prodItemList[pi].IsActive = 0;
                        prodItemList[pi].EffectiveTodt= existingGstCodeDtlsTO.EffectiveToDt;
                        prodItemList[pi].UpdatedOn= existingGstCodeDtlsTO.UpdatedOn;
                        prodItemList[pi].UpdatedBy = existingGstCodeDtlsTO.UpdatedBy;
                        result = BL.TblProdGstCodeDtlsBL.UpdateTblProdGstCodeDtls(prodItemList[pi], conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMsg.DefaultBehaviour("Error While UpdateTblTaxRates");
                            return resultMsg;
                        }
                    }

                    // Insert New 
                    gstCodeDtlsTO.IsActive = 1;
                    gstCodeDtlsTO.CreatedBy = gstCodeDtlsTO.UpdatedBy;
                    gstCodeDtlsTO.CreatedOn = gstCodeDtlsTO.UpdatedOn;
                    gstCodeDtlsTO.EffectiveFromDt = gstCodeDtlsTO.UpdatedOn;
                    gstCodeDtlsTO.EffectiveToDt = DateTime.MinValue;
                    result = InsertTblGstCodeDtls(gstCodeDtlsTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMsg.DefaultBehaviour("Error While InsertTblGstCodeDtls");
                        return resultMsg;
                    }

                    if (gstCodeDtlsTO.TaxRatesTOList == null)
                    {
                        tran.Rollback();
                        resultMsg.DefaultBehaviour("TaxRatesTOList Found NULL");
                        return resultMsg;
                    }

                    for (int i = 0; i < gstCodeDtlsTO.TaxRatesTOList.Count; i++)
                    {
                        gstCodeDtlsTO.TaxRatesTOList[i].GstCodeId = gstCodeDtlsTO.IdGstCode;
                        gstCodeDtlsTO.TaxRatesTOList[i].CreatedBy = gstCodeDtlsTO.CreatedBy;
                        gstCodeDtlsTO.TaxRatesTOList[i].CreatedOn = gstCodeDtlsTO.CreatedOn;
                        gstCodeDtlsTO.TaxRatesTOList[i].EffectiveFromDt = gstCodeDtlsTO.EffectiveFromDt;
                        gstCodeDtlsTO.TaxRatesTOList[i].EffectiveToDt = DateTime.MinValue;
                        result = BL.TblTaxRatesBL.InsertTblTaxRates(gstCodeDtlsTO.TaxRatesTOList[i], conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMsg.DefaultBehaviour("Error While InsertTblTaxRates");
                            return resultMsg;
                        }
                    }

                    for (int pi = 0; pi < prodItemList.Count; pi++)
                    {
                        prodItemList[pi].IsActive = 1;
                        prodItemList[pi].EffectiveFromDt = gstCodeDtlsTO.EffectiveFromDt;
                        prodItemList[pi].CreatedOn = gstCodeDtlsTO.CreatedOn;
                        prodItemList[pi].CreatedBy = gstCodeDtlsTO.CreatedBy;
                        prodItemList[pi].EffectiveTodt = DateTime.MinValue;
                        prodItemList[pi].GstCodeId = gstCodeDtlsTO.IdGstCode;
                        result = BL.TblProdGstCodeDtlsBL.InsertTblProdGstCodeDtls(prodItemList[pi], conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMsg.DefaultBehaviour("Error While InsertTblProdGstCodeDtls");
                            return resultMsg;
                        }
                    }
                }
                else
                {

                    result = UpdateTblGstCodeDtls(gstCodeDtlsTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMsg.DefaultBehaviour("Error While UpdateTblGstCodeDtls");
                        return resultMsg;
                    }

                    if (gstCodeDtlsTO.TaxRatesTOList == null)
                    {
                        tran.Rollback();
                        resultMsg.DefaultBehaviour("TaxRatesTOList Found NULL");
                        return resultMsg;
                    }

                    for (int i = 0; i < gstCodeDtlsTO.TaxRatesTOList.Count; i++)
                    {

                        var existingTaxRateTO = list.Where(a => a.TaxTypeId == gstCodeDtlsTO.TaxRatesTOList[i].TaxTypeId && a.IsActive==1).FirstOrDefault();
                        if (existingTaxRateTO != null)
                        {
                            if (existingTaxRateTO.TaxPct != gstCodeDtlsTO.TaxRatesTOList[i].TaxPct)
                            {
                                existingTaxRateTO.IsActive = 0;
                                existingTaxRateTO.UpdatedBy = gstCodeDtlsTO.UpdatedBy;
                                existingTaxRateTO.UpdatedOn = gstCodeDtlsTO.UpdatedOn;
                                existingTaxRateTO.EffectiveToDt = gstCodeDtlsTO.UpdatedOn;

                                result = BL.TblTaxRatesBL.UpdateTblTaxRates(existingTaxRateTO, conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMsg.DefaultBehaviour("Error While UpdateTblTaxRates");
                                    return resultMsg;
                                }

                                //Insert New One
                                gstCodeDtlsTO.TaxRatesTOList[i].EffectiveFromDt = gstCodeDtlsTO.UpdatedOn;
                                gstCodeDtlsTO.TaxRatesTOList[i].CreatedBy = gstCodeDtlsTO.UpdatedBy;
                                gstCodeDtlsTO.TaxRatesTOList[i].CreatedOn = gstCodeDtlsTO.UpdatedOn;
                                result = BL.TblTaxRatesBL.InsertTblTaxRates(gstCodeDtlsTO.TaxRatesTOList[i], conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMsg.DefaultBehaviour("Error While InsertTblTaxRates");
                                    return resultMsg;
                                }
                            }
                            else
                            {
                                gstCodeDtlsTO.TaxRatesTOList[i].UpdatedBy = gstCodeDtlsTO.UpdatedBy;
                                gstCodeDtlsTO.TaxRatesTOList[i].UpdatedOn = gstCodeDtlsTO.UpdatedOn;
                                result = BL.TblTaxRatesBL.UpdateTblTaxRates(gstCodeDtlsTO.TaxRatesTOList[i], conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMsg.DefaultBehaviour("Error While UpdateTblTaxRates");
                                    return resultMsg;
                                }
                            }
                        }
                        else
                        {
                            gstCodeDtlsTO.TaxRatesTOList[i].CreatedBy = gstCodeDtlsTO.UpdatedBy;
                            gstCodeDtlsTO.TaxRatesTOList[i].CreatedOn = gstCodeDtlsTO.UpdatedOn;
                            result = BL.TblTaxRatesBL.InsertTblTaxRates(gstCodeDtlsTO.TaxRatesTOList[i], conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMsg.DefaultBehaviour("Error While InsertTblTaxRates");
                                return resultMsg;
                            }
                        }
                    }
                }

                tran.Commit();
                resultMsg.DefaultSuccessBehaviour();
                return resultMsg;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMsg.DefaultExceptionBehaviour(ex, "UpdateNewGSTCodeDetails");
                return resultMsg;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region Deletion
        public static int DeleteTblGstCodeDtls(Int32 idGstCode)
        {
            return TblGstCodeDtlsDAO.DeleteTblGstCodeDtls(idGstCode);
        }

        public static int DeleteTblGstCodeDtls(Int32 idGstCode, SqlConnection conn, SqlTransaction tran)
        {
            return TblGstCodeDtlsDAO.DeleteTblGstCodeDtls(idGstCode, conn, tran);
        }

       

        #endregion

    }
}
