using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;
using System.Linq;

namespace PurchaseTrackerAPI.BL
{
    public class TblVariablesBL : ITblVariablesBL
    {
        private readonly IConnectionString _iConnectionString;
        private readonly Icommondao _iCommonDAO;
        private readonly ITblVariablesDAO _iTblVariablesDAO;
        public TblVariablesBL(ITblVariablesDAO iTblVariablesDAO, IConnectionString iConnectionString, Icommondao icommondao)
        {
            _iConnectionString = iConnectionString;
            _iCommonDAO = icommondao;
            _iTblVariablesDAO = iTblVariablesDAO;
        }
        #region Selection
        public List<TblVariablesTO> SelectAllTblVariables(Int32 isActive)
        {
            return _iTblVariablesDAO.SelectAllTblVariables(isActive);
        }

        public List<TblVariablesTO> SelectAllTblVariablesList()
        {
            return _iTblVariablesDAO.SelectAllTblVariables();
        }

        public TblVariablesTO SelectTblVariablesTO(Int32 idVariable)
        {
            List<TblVariablesTO> tblVariablesTOList = _iTblVariablesDAO.SelectTblVariables(idVariable);
            if (tblVariablesTOList != null && tblVariablesTOList.Count == 1)
                return tblVariablesTOList[0];
            else
                return null;
        }
        public List<TblVariablesTO> SelectVariableCodeDtls(String variableCode, DateTime fromDate, DateTime toDate)
        {
            return _iTblVariablesDAO.SelectVariableCodeDtls(variableCode, fromDate,toDate);
        }
        public List<TblVariablesTO> GetHistoryOfVariablesbyUniqueNo(int uniqueTrackId)
        {
            return _iTblVariablesDAO.GetHistoryOfVariablesbyUniqueNo(uniqueTrackId);

        }
        public List<DropDownTO> SelectVariableList(int isProcessVar)
        {
            return _iTblVariablesDAO.SelectVariableList(isProcessVar);
        }

        public List<TblVariablesTO> SelectActiveVariablesList(SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVariablesDAO.SelectActiveVariablesList(conn, tran);
        }
        public List<TblVariablesTO> SelectAllTblVariables(SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVariablesDAO.SelectAllTblVariables(conn, tran);
        }
        public double GetProcessVariableValue(TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsLocalTO,List<TblVariablesTO> variableTOList)
        {
            double processVarValue = 0;

            if(variableTOList != null && variableTOList.Count > 0 && tblPurchaseVehicleDetailsLocalTO != null)
            {
                TblVariablesTO  variableTO = variableTOList.Where(a => a.IdVariable == tblPurchaseVehicleDetailsLocalTO.ProcessVarId).FirstOrDefault();
                if(variableTO != null)
                {
                    //processVarValue = (tblPurchaseVehicleDetailsLocalTO.Qty * variableTO.VariableValue);
                    processVarValue =  variableTO.VariableValue;
                    processVarValue = Math.Round(processVarValue, 3);
                 }
            }

            return processVarValue;
        }

        #endregion

        #region Insertion
        public int InsertTblVariables(TblVariablesTO tblVariablesTO)
        {
            return _iTblVariablesDAO.InsertTblVariables(tblVariablesTO);
        }

        public int InsertTblVariables(TblVariablesTO tblVariablesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVariablesDAO.InsertTblVariables(tblVariablesTO, conn, tran);
        }

        public int InsertTblVariablesEdit(TblVariablesTO tblVariablesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVariablesDAO.InsertTblVariablesEdit(tblVariablesTO, conn, tran);
        }


        //Deactive previous active variables change updated on,updated by
        //Post new record with active=1 and created by, created on

        public ResultMessage PostVariableDetails(List<TblVariablesTO> tblVariablesTOList, Int32 loginUserId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            Int32 result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            DateTime currentDate = _iCommonDAO.ServerDateTime;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                //Deactive previous active variables change updated on,updated by
                List<TblVariablesTO> tblVariablesDtlsTOList = SelectActiveVariablesList(conn, tran);
                if (tblVariablesDtlsTOList != null && tblVariablesDtlsTOList.Count > 0)
                {
                    for (int k = 0; k < tblVariablesDtlsTOList.Count; k++)
                    {
                        tblVariablesDtlsTOList[k].IsActive = 0;
                        tblVariablesDtlsTOList[k].UpdatedBy = loginUserId;
                        tblVariablesDtlsTOList[k].UpdatedOn = currentDate;

                        result = UpdateTblVariables(tblVariablesDtlsTOList[k], conn, tran);
                        if (result <= 0)
                        {
                            tran.Rollback();
                            throw new Exception("Error while Updating active status of variable");
                        }

                    }
                }

                //Insert New records
                if (tblVariablesTOList != null && tblVariablesTOList.Count > 0)
                {
                    for (int i = 0; i < tblVariablesTOList.Count; i++)
                    {
                        if(i == 60)
                        {

                        }

                        tblVariablesTOList[i].IsActive = 1;
                        tblVariablesTOList[i].CreatedBy = loginUserId;
                        tblVariablesTOList[i].CreatedOn = currentDate;
                        tblVariablesTOList[i].UpdatedBy = loginUserId;
                        tblVariablesTOList[i].UpdatedOn = currentDate;
                        result = InsertTblVariables(tblVariablesTOList[i], conn, tran);
                        if (result <= 0)
                        {
                            tran.Rollback();
                            throw new Exception("Error while importing variable details");
                        }
                    }
                }


                if (result >= 1)
                {
                    tran.Commit();
                    resultMessage.DefaultSuccessBehaviour("Variables Imported Successfully.");
                    return resultMessage;
                }
                else
                {
                    tran.Rollback();
                    throw new Exception("Error while importing variable details");
                }

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostVariableDetails");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public ResultMessage EditVariableDetails(TblVariablesTO tblVariablesTO, Int32 loginUserId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            Int32 result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            DateTime currentDate = _iCommonDAO.ServerDateTime;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                //Deactive previous active variables change updated on,updated by
                if (tblVariablesTO != null)
                {
                    tblVariablesTO.IsActive = 0;
                    tblVariablesTO.UpdatedBy = loginUserId;
                    tblVariablesTO.UpdatedOn = currentDate;

                    result = UpdateTblVariablesEdit(tblVariablesTO, conn, tran);
                    if (result <= 0)
                    {
                        tran.Rollback();
                        throw new Exception("Error while Updating active status of variable");
                    }

                }

                //Insert New records
                if (tblVariablesTO != null)
                {
                    tblVariablesTO.IsActive = 1;
                    tblVariablesTO.CreatedBy = loginUserId;
                    tblVariablesTO.CreatedOn = currentDate;
                    tblVariablesTO.UpdatedBy = loginUserId;
                    tblVariablesTO.UpdatedOn = currentDate;
                    result = InsertTblVariablesEdit(tblVariablesTO, conn, tran);
                    if (result <= 0)
                    {
                        tran.Rollback();
                        throw new Exception("Error while updating variable details");
                    }
                }

                if (result >= 1)
                {
                    tran.Commit();
                    resultMessage.DefaultSuccessBehaviour("Variables updated Successfully.");
                    return resultMessage;
                }
                else
                {
                    tran.Rollback();
                    throw new Exception("Error while importing variable details");
                }

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostVariableDetails");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }



        #endregion

        #region Updation
        public int UpdateTblVariables(TblVariablesTO tblVariablesTO)
        {
            return _iTblVariablesDAO.UpdateTblVariables(tblVariablesTO);
        }

        public int UpdateTblVariables(TblVariablesTO tblVariablesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVariablesDAO.UpdateTblVariables(tblVariablesTO, conn, tran);
        }
        public int UpdateTblVariablesEdit(TblVariablesTO tblVariablesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVariablesDAO.UpdateTblVariablesEdit(tblVariablesTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblVariables(Int32 idVariable)
        {
            return _iTblVariablesDAO.DeleteTblVariables(idVariable);
        }

        public int DeleteTblVariables(Int32 idVariable, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVariablesDAO.DeleteTblVariables(idVariable, conn, tran);
        }

        #endregion

    }
}
