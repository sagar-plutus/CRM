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
    public class TblAlertActionDtlBL : ITblAlertActionDtlBL
    {
        private readonly ITblAlertActionDtlDAO _itblAlertActionDtlDAO;
        private readonly Icommondao _iCommonDAO;
        private readonly IConnectionString _iConnectionString;



        public TblAlertActionDtlBL(ITblAlertActionDtlDAO itblAlertActionDtlDAO, Icommondao icommondao, IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
            _itblAlertActionDtlDAO = itblAlertActionDtlDAO;
            _iCommonDAO = icommondao;
        }
        #region Selection

        public  List<TblAlertActionDtlTO> SelectAllTblAlertActionDtlList()
        {
            return  _itblAlertActionDtlDAO.SelectAllTblAlertActionDtl();
        }

        public  TblAlertActionDtlTO SelectTblAlertActionDtlTO(Int32 idAlertActionDtl)
        {
            return  _itblAlertActionDtlDAO.SelectTblAlertActionDtl(idAlertActionDtl);
        }

        public  TblAlertActionDtlTO SelectTblAlertActionDtlTO(Int32 alertInstanceId,Int32 userId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _itblAlertActionDtlDAO.SelectTblAlertActionDtl(alertInstanceId, userId,conn,tran);
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

        public  TblAlertActionDtlTO SelectTblAlertActionDtlTO(Int32 alertInstanceId, Int32 userId,SqlConnection conn,SqlTransaction tran)
        {
            return _itblAlertActionDtlDAO.SelectTblAlertActionDtl(alertInstanceId, userId,conn,tran);
        }

        public  List<TblAlertActionDtlTO> SelectAllTblAlertActionDtlList(Int32 userId)
        {
            return _itblAlertActionDtlDAO.SelectAllTblAlertActionDtl(userId);
        }
        #endregion

        #region Insertion
        public  int InsertTblAlertActionDtl(TblAlertActionDtlTO tblAlertActionDtlTO)
        {
            return _itblAlertActionDtlDAO.InsertTblAlertActionDtl(tblAlertActionDtlTO);
        }

        public  int InsertTblAlertActionDtl(TblAlertActionDtlTO tblAlertActionDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return _itblAlertActionDtlDAO.InsertTblAlertActionDtl(tblAlertActionDtlTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblAlertActionDtl(TblAlertActionDtlTO tblAlertActionDtlTO)
        {
            return _itblAlertActionDtlDAO.UpdateTblAlertActionDtl(tblAlertActionDtlTO);
        }

        public  int UpdateTblAlertActionDtl(TblAlertActionDtlTO tblAlertActionDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return _itblAlertActionDtlDAO.UpdateTblAlertActionDtl(tblAlertActionDtlTO, conn, tran);
        }


        public  ResultMessage ResetAllAlerts(int loginUserId, List<TblAlertUsersTO> list, int result)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            ResultMessage resultMessage = new ResultMessage();
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                for (int i = 0; i < list.Count; i++)
                {

                    TblAlertUsersTO alertUsersTO = list[i];
                    TblAlertActionDtlTO tblAlertActionDtlTO = new TblAlertActionDtlTO();

                    alertUsersTO.IsReseted = 1;
                    if (alertUsersTO.IsReseted == 1)
                    {
                        //Check For Existence
                        TblAlertActionDtlTO existingAlertActionDtlTO = SelectTblAlertActionDtlTO(alertUsersTO.AlertInstanceId, Convert.ToInt32(loginUserId), conn, tran);
                        if (existingAlertActionDtlTO != null)
                        {
                            existingAlertActionDtlTO.ResetDate =  _iCommonDAO.ServerDateTime;
                            result = UpdateTblAlertActionDtl(existingAlertActionDtlTO, conn, tran);
                            if (result != 1)
                            {
                                resultMessage.Text = "Error While UpdateTblAlertActionDtl";
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Result = 0;
                                return resultMessage;
                            }
                        }
                        else
                        {
                            tblAlertActionDtlTO.ResetDate =  _iCommonDAO.ServerDateTime;
                            goto xxx;
                        }
                    }

                    xxx:
                    tblAlertActionDtlTO.UserId = loginUserId;
                    tblAlertActionDtlTO.AcknowledgedOn =  _iCommonDAO.ServerDateTime;
                    tblAlertActionDtlTO.AlertInstanceId = alertUsersTO.AlertInstanceId;
                    result =InsertTblAlertActionDtl(tblAlertActionDtlTO);
                    if (result != 1)
                    {
                        resultMessage.Text = "Error While InsertTblAlertActionDtl";
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        return resultMessage;

                    }

                }

                tran.Commit();
                resultMessage.Text = "All Alert Reseted";
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Result = 1;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.Text = "Error While InsertTblAlertActionDtl";
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

        #region Deletion
        public  int DeleteTblAlertActionDtl(Int32 idAlertActionDtl)
        {
            return _itblAlertActionDtlDAO.DeleteTblAlertActionDtl(idAlertActionDtl);
        }

        public  int DeleteTblAlertActionDtl(Int32 idAlertActionDtl, SqlConnection conn, SqlTransaction tran)
        {
            return _itblAlertActionDtlDAO.DeleteTblAlertActionDtl(idAlertActionDtl, conn, tran);
        }

        #endregion
        
    }
}
