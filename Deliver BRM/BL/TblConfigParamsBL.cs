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
    public class TblConfigParamsBL
    {
        #region Selection
       
        public static List<TblConfigParamsTO> SelectAllTblConfigParamsList()
        {
            return TblConfigParamsDAO.SelectAllTblConfigParams();
        }
        /// <summary>
        /// GJ@20170810 : Get the Configuration value by Name 
        /// </summary>
        /// <param name="configParamName"></param>
        /// <returns></returns>
        public static TblConfigParamsTO SelectTblConfigParamsValByName(string configParamName)
        {
            return TblConfigParamsDAO.SelectTblConfigParamsValByName(configParamName);
        }

        public static TblConfigParamsTO SelectTblConfigParamsTO(Int32 idConfigParam)
        {
            return TblConfigParamsDAO.SelectTblConfigParams(idConfigParam);
        }

        public static TblConfigParamsTO SelectTblConfigParamsTO(String configParamName)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblConfigParamsDAO.SelectTblConfigParams(configParamName, conn, tran);
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

        public static TblConfigParamsTO SelectTblConfigParamsTO(string configParamName,SqlConnection conn,SqlTransaction tran)
        {
            return TblConfigParamsDAO.SelectTblConfigParams(configParamName,conn,tran);
        }

        #endregion

        #region Insertion
        public static int InsertTblConfigParams(TblConfigParamsTO tblConfigParamsTO)
        {
            return TblConfigParamsDAO.InsertTblConfigParams(tblConfigParamsTO);
        }

        public static int InsertTblConfigParams(TblConfigParamsTO tblConfigParamsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblConfigParamsDAO.InsertTblConfigParams(tblConfigParamsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblConfigParams(TblConfigParamsTO tblConfigParamsTO)
        {
            return TblConfigParamsDAO.UpdateTblConfigParams(tblConfigParamsTO);
        }

        internal static ResultMessage UpdateConfigParamsWithHistory(TblConfigParamsTO configParamsTO,Int32 updatedByUserId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            DateTime serverDate = Constants.ServerDateTime;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                TblConfigParamsTO existingTblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(configParamsTO.ConfigParamName, conn, tran);
                if(existingTblConfigParamsTO==null)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "Error While SelectTblConfigParamsTO. existingTblConfigParamsTO found NULL ";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return resultMessage;
                }

                TblConfigParamHistoryTO historyTO = new TblConfigParamHistoryTO();
                historyTO.ConfigParamId = configParamsTO.IdConfigParam;
                historyTO.ConfigParamName = configParamsTO.ConfigParamName;
                historyTO.ConfigParamOldVal = existingTblConfigParamsTO.ConfigParamVal;
                historyTO.ConfigParamNewVal = configParamsTO.ConfigParamVal;
                historyTO.CreatedBy = updatedByUserId;
                historyTO.CreatedOn = serverDate;

                int result = BL.TblConfigParamHistoryBL.InsertTblConfigParamHistory(historyTO, conn, tran);
                if(result!=1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "Error While InsertTblConfigParamHistory";
                    return resultMessage;
                }

                result = UpdateTblConfigParams(configParamsTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "Error While UpdateTblConfigParams";
                    return resultMessage;
                }

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateConfigParamsWithHistory");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public static int UpdateTblConfigParams(TblConfigParamsTO tblConfigParamsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblConfigParamsDAO.UpdateTblConfigParams(tblConfigParamsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblConfigParams(Int32 idConfigParam)
        {
            return TblConfigParamsDAO.DeleteTblConfigParams(idConfigParam);
        }

        public static int DeleteTblConfigParams(Int32 idConfigParam, SqlConnection conn, SqlTransaction tran)
        {
            return TblConfigParamsDAO.DeleteTblConfigParams(idConfigParam, conn, tran);
        }

        #endregion
        
    }
}
