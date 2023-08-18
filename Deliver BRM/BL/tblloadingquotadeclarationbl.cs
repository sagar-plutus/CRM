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
    public class TblLoadingQuotaDeclarationBL
    {
        #region Selection

        public static List<TblLoadingQuotaDeclarationTO> SelectAllTblLoadingQuotaDeclarationList()
        {
            return  TblLoadingQuotaDeclarationDAO.SelectAllTblLoadingQuotaDeclaration();
        }

        public static List<TblLoadingQuotaDeclarationTO> SelectAllTblLoadingQuotaDeclarationList(DateTime declarationDate)
        {
            return TblLoadingQuotaDeclarationDAO.SelectAllTblLoadingQuotaDeclaration(declarationDate);
        }

        public static Boolean IsLoadingQuotaDeclaredForTheDate(DateTime declarationDate, Int32 prodCatId, Int32 prodSpecId)
        {
            return TblLoadingQuotaDeclarationDAO.IsLoadingQuotaDeclaredForTheDate(declarationDate,prodCatId,prodSpecId);
        }

        public static Boolean IsLoadingQuotaDeclaredForTheDate(DateTime declarationDate)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblLoadingQuotaDeclarationDAO.IsLoadingQuotaDeclaredForTheDate(declarationDate, conn, tran);
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public static Boolean IsLoadingQuotaDeclaredForTheDate(DateTime declarationDate,SqlConnection conn,SqlTransaction tran)
        {
            return TblLoadingQuotaDeclarationDAO.IsLoadingQuotaDeclaredForTheDate(declarationDate,conn,tran);
        }

        public static List<TblLoadingQuotaDeclarationTO> SelectAvailableLoadingQuotaForCnf(int cnfId, DateTime declarationDate)
        {
            return TblLoadingQuotaDeclarationDAO.SelectAllTblLoadingQuotaDeclaration(cnfId,declarationDate);
        }

        public static List<TblLoadingQuotaDeclarationTO> SelectLatestCalculatedLoadingQuotaDeclarationList(DateTime stockDate, Int32 prodCatId, Int32 prodSpecId)
        {
            return TblLoadingQuotaDeclarationDAO.SelectLatestCalculatedLoadingQuotaDeclarationList(stockDate, prodCatId, prodSpecId);
        }

        public static List<TblLoadingQuotaDeclarationTO> SelectLatestCalculatedLoadingQuotaDeclarationList(DateTime stockDate,Int32 cnfOrgId, SqlConnection conn,SqlTransaction tran)
        {
            return TblLoadingQuotaDeclarationDAO.SelectLatestCalculatedLoadingQuotaDeclarationList(stockDate,cnfOrgId, conn, tran);
        }

        public static TblLoadingQuotaDeclarationTO SelectTblLoadingQuotaDeclarationTO(Int32 idLoadingQuota)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return SelectTblLoadingQuotaDeclarationTO(idLoadingQuota, conn, tran);
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

        public static TblLoadingQuotaDeclarationTO SelectTblLoadingQuotaDeclarationTO(Int32 idLoadingQuota,SqlConnection conn,SqlTransaction tran)
        {
           return  TblLoadingQuotaDeclarationDAO.SelectTblLoadingQuotaDeclaration(idLoadingQuota,conn,tran);
        }
        //Vijaymala [04-06-2018] changes the code to get loading quota for regular as well as isstockrequired item 

        public static TblLoadingQuotaDeclarationTO SelectTblLoadingQuotaDeclarationTO(Int32 cnfId,Int32 prodCatId,Int32 prodSpecId,Int32 materialId, Int32 prodItemId, DateTime quotaDate, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingQuotaDeclarationDAO.SelectLoadingQuotaDeclarationTO(cnfId, prodCatId, prodSpecId, materialId, prodItemId, quotaDate, conn, tran);
        }

        public static List<TblLoadingQuotaDeclarationTO> SelectLoadingQuotaListForCnfAndDate(Int32 cnfOrgId,DateTime quotaDate, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingQuotaDeclarationDAO.SelectLoadingQuotaDeclaredForCnfAndDate(cnfOrgId,quotaDate, conn, tran);
        }

        /// <summary>
        /// Sanjay [2017-04-05] To Get All Declared Loading Quota List against given Loading Slip Ext Ids
        /// These are the Ids of material against a loading slip. Required while confirming the loading slip
        /// </summary>
        /// <param name="loadingSlipExtId"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static List<TblLoadingQuotaDeclarationTO> SelectAllLoadingQuotaDeclListFromLoadingExt(String loadingSlipExtId,SqlConnection conn,SqlTransaction tran)
        {
            return TblLoadingQuotaDeclarationDAO.SelectAllLoadingQuotaDeclListFromLoadingExt(loadingSlipExtId,conn,tran);
        }
        #endregion

        #region Insertion
        public static int InsertTblLoadingQuotaDeclaration(TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO)
        {
            return TblLoadingQuotaDeclarationDAO.InsertTblLoadingQuotaDeclaration(tblLoadingQuotaDeclarationTO);
        }

        public static int InsertTblLoadingQuotaDeclaration(TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingQuotaDeclarationDAO.InsertTblLoadingQuotaDeclaration(tblLoadingQuotaDeclarationTO, conn, tran);
        }

        internal static ResultMessage SaveLoadingQuotaDeclaration(List<TblLoadingQuotaDeclarationTO> loadingQuotaDeclarationTOList)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                if (loadingQuotaDeclarationTOList == null || loadingQuotaDeclarationTOList.Count == 0)
                {
                    tran.Rollback();
                    resultMessage.Text = "competitorUpdatesTOList Found Null : SaveLoadingQuotaDeclaration";
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                #region 1. Mark All Previous Loading Quota As Inactive

                Int32 prodCatId = loadingQuotaDeclarationTOList[0].ProdCatId;
                Int32 prodSpecId = loadingQuotaDeclarationTOList[0].ProdSpecId;
                result = DAL.TblLoadingQuotaDeclarationDAO.DeactivateAllPrevLoadingQuota(loadingQuotaDeclarationTOList[0].CreatedBy,prodCatId,prodSpecId, conn, tran);
                if(result < 0)
                {
                    tran.Rollback();
                    resultMessage.Text = "Error While DeactivateAllPrevLoadingQuota : SaveLoadingQuotaDeclaration";
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                #endregion

                #region 2. Assign New Quota 

                for (int i = 0; i < loadingQuotaDeclarationTOList.Count; i++)
                {
                    result = InsertTblLoadingQuotaDeclaration(loadingQuotaDeclarationTOList[i], conn, tran);
                    if(result!=1)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Error While InsertTblLoadingQuotaDeclaration : SaveLoadingQuotaDeclaration";
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        return resultMessage;
                    }
                }

                #endregion


                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Saved Sucessfully";
                resultMessage.Result = 1;
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.Text = "Exception Error While Record Save : SaveLoadingQuotaDeclaration";
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region Updation
        public static int UpdateTblLoadingQuotaDeclaration(TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO)
        {
            return TblLoadingQuotaDeclarationDAO.UpdateTblLoadingQuotaDeclaration(tblLoadingQuotaDeclarationTO);
        }

        public static int UpdateTblLoadingQuotaDeclaration(TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingQuotaDeclarationDAO.UpdateTblLoadingQuotaDeclaration(tblLoadingQuotaDeclarationTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblLoadingQuotaDeclaration(Int32 idLoadingQuota)
        {
            return TblLoadingQuotaDeclarationDAO.DeleteTblLoadingQuotaDeclaration(idLoadingQuota);
        }

        public static int DeleteTblLoadingQuotaDeclaration(Int32 idLoadingQuota, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingQuotaDeclarationDAO.DeleteTblLoadingQuotaDeclaration(idLoadingQuota, conn, tran);
        }

       

        #endregion

    }
}
