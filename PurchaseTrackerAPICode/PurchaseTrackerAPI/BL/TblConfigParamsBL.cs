using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System.Linq;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblConfigParamsBL: ITblConfigParamsBL
    {
        private readonly IConnectionString _iConnectionString;
        private readonly ITblConfigParamHistoryBL _iTblConfigParamHistoryBL;
        private readonly Idimensionbl _idimensionbl;
        private readonly ITblConfigParamsDAO _itblConfigParamsDAO;
        private readonly Icommondao _iCommonDAO;
        private readonly ITblVariablesBL _iTblVariablesBL;

        public TblConfigParamsBL( ITblConfigParamsDAO itblConfigParamsDAO, ITblVariablesBL iTblVariablesBL, Icommondao icommondao, Idimensionbl idimensionBL, ITblConfigParamHistoryBL iTblConfigParamHistoryBL, IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
            _iCommonDAO = icommondao;
            _iTblConfigParamHistoryBL = iTblConfigParamHistoryBL;
            _itblConfigParamsDAO = itblConfigParamsDAO;
            _idimensionbl = idimensionBL;
            _iTblVariablesBL = iTblVariablesBL;
        }
        #region Selection

        public  List<TblConfigParamsTO> SelectAllTblConfigParamsList()
        {
            return _itblConfigParamsDAO.SelectAllTblConfigParams();
        }
        /// <summary>
        /// GJ@20170810 : Get the Configuration value by Name 
        /// </summary>
        /// <param name="configParamName"></param>
        /// <returns></returns>
        public  TblConfigParamsTO SelectTblConfigParamsValByName(string configParamName)
        {
            return _itblConfigParamsDAO.SelectTblConfigParamsValByName(configParamName);
        }

        public  TblConfigParamsTO SelectTblConfigParamsValByName(string configParamName, SqlConnection conn, SqlTransaction tran)
        {
            return _itblConfigParamsDAO.SelectTblConfigParams(configParamName, conn, tran);
        }
        public  TblConfigParamsTO SelectTblConfigParamsTO(Int32 idConfigParam)
        {
            return _itblConfigParamsDAO.SelectTblConfigParams(idConfigParam);
        }

        public  TblConfigParamsTO SelectTblConfigParamsTO(String configParamName)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _itblConfigParamsDAO.SelectTblConfigParams(configParamName, conn, tran);
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

        //Prajakta [2018 dec 12] Added
        public  List<Int32> SelectDefaultPmRoleIds()
        {
            List<Int32> pmRoleIds = new List<Int32>();
            string defaultPMRoleIds = string.Empty;

            //TblConfigParamsTO tblConfigParamsTempTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_PURCHASE_MANAGER_ROLE_ID);
            //if (tblConfigParamsTempTO != null)
            //{
            //    defaultPMRoleIds = tblConfigParamsTempTO.ConfigParamVal;
            //}

            defaultPMRoleIds = _idimensionbl.GetRoleIdsStrFromRoleTypeId(Convert.ToInt32(Constants.SystemRoleTypeE.PURCHASE_MANAGER));


            if (!String.IsNullOrEmpty(defaultPMRoleIds))
            {
                pmRoleIds = defaultPMRoleIds.Split(',').Select(n => Convert.ToInt32(n)).ToList();
            }
            return pmRoleIds;
        }
        public  TblConfigParamsTO SelectTblConfigParamsTO(string configParamName, SqlConnection conn, SqlTransaction tran)
        {
            return _itblConfigParamsDAO.SelectTblConfigParams(configParamName, conn, tran);
        }

        public  Int32 GetStockConfigIsConsolidate()
        {
            TblConfigParamsTO tblConfigParamsTO = SelectTblConfigParamsTO(Constants.CONSOLIDATE_STOCK);
            Int32 isConsolidateStk = 0;

            if (tblConfigParamsTO != null)
                isConsolidateStk = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);

            return isConsolidateStk;
        }


        public String SelectTblConfigParamsValByNameString(string configParamName)
        {
            TblConfigParamsTO tblConfigParamsTO = SelectTblConfigParamsTO(configParamName);
            String value = String.Empty;

            if (tblConfigParamsTO != null)
                value = Convert.ToString(tblConfigParamsTO.ConfigParamVal);

            return value;
        }

        public double GetCurrentValueOfV8RefVar(String variableName)
        {
            TblConfigParamsTO refRateVar = SelectTblConfigParamsValByName(variableName);
            double V48Val = 0;
            if (refRateVar != null)
            {
                int isactive = 1;
                List<TblVariablesTO> tblVariablesTOList = _iTblVariablesBL.SelectAllTblVariables(isactive);
                TblVariablesTO varTo = tblVariablesTOList.Where(w => w.VariableCode == refRateVar.ConfigParamVal).FirstOrDefault();
                if (varTo != null)
                {
                    V48Val = varTo.VariableValue;
                    return V48Val;
                }
            }
            return V48Val;
        }


        #endregion

        #region Insertion
        public int InsertTblConfigParams(TblConfigParamsTO tblConfigParamsTO)
        {
            return _itblConfigParamsDAO.InsertTblConfigParams(tblConfigParamsTO);
        }

        public  int InsertTblConfigParams(TblConfigParamsTO tblConfigParamsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _itblConfigParamsDAO.InsertTblConfigParams(tblConfigParamsTO, conn, tran);
        }

        #endregion

        #region Updation
        public  int UpdateTblConfigParams(TblConfigParamsTO tblConfigParamsTO)
        {
            return _itblConfigParamsDAO.UpdateTblConfigParams(tblConfigParamsTO);
        }

        internal  ResultMessage UpdateConfigParamsWithHistory(TblConfigParamsTO configParamsTO, Int32 updatedByUserId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            DateTime serverDate =  _iCommonDAO.ServerDateTime;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                TblConfigParamsTO existingTblConfigParamsTO =SelectTblConfigParamsTO(configParamsTO.ConfigParamName, conn, tran);
                if (existingTblConfigParamsTO == null)
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

                int result = _iTblConfigParamHistoryBL.InsertTblConfigParamHistory(historyTO, conn, tran);
                if (result != 1)
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

        public  int UpdateTblConfigParams(TblConfigParamsTO tblConfigParamsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _itblConfigParamsDAO.UpdateTblConfigParams(tblConfigParamsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public  int DeleteTblConfigParams(Int32 idConfigParam)
        {
            return _itblConfigParamsDAO.DeleteTblConfigParams(idConfigParam);
        }

        public  int DeleteTblConfigParams(Int32 idConfigParam, SqlConnection conn, SqlTransaction tran)
        {
            return _itblConfigParamsDAO.DeleteTblConfigParams(idConfigParam, conn, tran);
        }

        #endregion

        public ResultMessage SaveBirimMachineQty(double birimMachineQty, Int32 loginUserId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            DateTime currentDate = _iCommonDAO.ServerDateTime;
            Int32 result = 0;
            string oldVal = String.Empty;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                string configParamName = Constants.CP_SCRAP_BIRIM_MACHINE_QTY;

                TblConfigParamsTO tblConfigParamsTO = SelectTblConfigParamsValByName(configParamName, conn, tran);
                if (tblConfigParamsTO == null)
                {
                    tblConfigParamsTO = new TblConfigParamsTO();
                    tblConfigParamsTO.ConfigParamName = configParamName;
                    tblConfigParamsTO.ConfigParamValType = 1;
                    tblConfigParamsTO.ConfigParamVal = birimMachineQty.ToString();
                    oldVal = birimMachineQty.ToString();
                    tblConfigParamsTO.CreatedOn = currentDate;
                    tblConfigParamsTO.IsActive = 1;
                    tblConfigParamsTO.ConfigParamDisplayVal = configParamName;
                    tblConfigParamsTO.ModuleId = 5;

                    result = InsertTblConfigParams(tblConfigParamsTO, conn, tran);
                    if(result != 1)
                    {
                        throw new Exception("Error in InsertTblConfigParams(tblConfigParamsTO, conn, tran);");
                    }
                }
                else
                {
                    oldVal = tblConfigParamsTO.ConfigParamVal;
                    tblConfigParamsTO.ConfigParamVal = birimMachineQty.ToString();
                    result = UpdateTblConfigParams(tblConfigParamsTO, conn, tran);
                    if(result == -1)
                    {
                        throw new Exception("Error in UpdateTblConfigParams(tblConfigParamsTO, conn, tran);");
                    }
                }

                TblConfigParamHistoryTO tblConfigParamHistoryTO = new TblConfigParamHistoryTO();

                tblConfigParamHistoryTO.ConfigParamId = tblConfigParamsTO.IdConfigParam;
                tblConfigParamHistoryTO.ConfigParamName = tblConfigParamsTO.ConfigParamName;
                tblConfigParamHistoryTO.ConfigParamOldVal = oldVal;
                tblConfigParamHistoryTO.ConfigParamNewVal = tblConfigParamsTO.ConfigParamVal;
                tblConfigParamHistoryTO.CreatedBy = loginUserId;
                tblConfigParamHistoryTO.CreatedOn = currentDate;


                result = _iTblConfigParamHistoryBL.InsertTblConfigParamHistory(tblConfigParamHistoryTO, conn, tran);
                if(result != 1)
                {
                    throw new Exception("Error in InsertTblConfigParamHistory(tblConfigParamHistoryTO, conn, tran);");
                }

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in SaveBirimMachineQty(double birimMachineQty, Int32 loginUserId)");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

    }
}
