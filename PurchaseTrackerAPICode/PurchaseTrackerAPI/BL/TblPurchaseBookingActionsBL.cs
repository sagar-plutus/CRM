using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PurchaseTrackerAPI.BL
{

    
    public class TblPurchaseBookingActionsBL : ITblPurchaseBookingActionsBL
    {
        private readonly IConnectionString _iConnectionString;
        private readonly ITblPurchaseBookingActionsDAO _iTblPurchaseBookingActionsDAO;
        private readonly ITblRateBandDeclarationPurchaseDAO _iTblRateBandDeclarationPurchaseDAO;
        private readonly INotification notify;
        public TblPurchaseBookingActionsBL(IConnectionString iConnectionString, ITblPurchaseBookingActionsDAO iTblPurchaseBookingActionsDAO, ITblRateBandDeclarationPurchaseDAO iTblRateBandDeclarationPurchaseDAO, INotification inotify)
        {
            _iConnectionString = iConnectionString;
            notify = inotify;
            _iTblRateBandDeclarationPurchaseDAO = iTblRateBandDeclarationPurchaseDAO;
            _iTblPurchaseBookingActionsDAO = iTblPurchaseBookingActionsDAO;
        }
        #region Selection
        /*
        public  List<TblPurchaseBookingActionsTO> SelectAllTblBookingActionsList()
        {
            return TblBookingActionsDAO.SelectAllTblBookingActions();
        }

        public  TblBookingActionsTO SelectTblBookingActionsTO(Int32 idBookingAction)
        {
            return TblBookingActionsDAO.SelectTblBookingActions(idBookingAction);
        }
        */

        public  TblPurchaseBookingActionsTO SelectLatestBookingActionTO(SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseBookingActionsDAO.SelectLatestBookingActionTO(conn, tran);
        }

        public  TblPurchaseBookingActionsTO SelectLatestBookingActionTO()
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                TblPurchaseBookingActionsTO tblBookingActionsTO = _iTblPurchaseBookingActionsDAO.SelectLatestBookingActionTO(conn, tran);
                tran.Commit();
                return tblBookingActionsTO;
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


        //public  TblBookingActionsTO SelectLatestBookingActionTO()
        //{
        //    SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
        //    SqlTransaction tran = null;
        //    try
        //    {
        //        conn.Open();
        //        tran = conn.BeginTransaction();
        //        TblBookingActionsTO tblBookingActionsTO = TblBookingActionsDAO.SelectLatestBookingActionTO(conn, tran);
        //        tran.Commit();
        //        return tblBookingActionsTO;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }

        //}

        #endregion

        #region Insertion

        public  int InsertTblBookingActions(TblPurchaseBookingActionsTO tblBookingActionsTO)
        {
            return _iTblPurchaseBookingActionsDAO.InsertTblBookingActions(tblBookingActionsTO);
        }

        public  int InsertTblBookingActions(TblPurchaseBookingActionsTO tblBookingActionsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseBookingActionsDAO.InsertTblBookingActions(tblBookingActionsTO, conn, tran);
        }

        public  ResultMessage SaveBookingActions(TblPurchaseBookingActionsTO tblBookingActionsTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                #region 1. Clear All Previous Quota i.e. Deactivate All Prev Quotas If Booking Status is CLOSE

                if (tblBookingActionsTO.BookingStatus == "CLOSE")
                {
                    result = _iTblRateBandDeclarationPurchaseDAO.DeactivateAllDeclaredQuota(tblBookingActionsTO.StatusBy, conn, tran);
                    if (result == -1)
                    {
                        tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error While DeactivateAllDeclaredQuota";
                        resultMessage.Tag = tblBookingActionsTO;
                        return resultMessage;
                    }
                }
                #endregion

                #region 2. Mark Booking Action Status

                result = InsertTblBookingActions(tblBookingActionsTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While InsertTblBookingActions";
                    resultMessage.Tag = tblBookingActionsTO;
                    return resultMessage;
                }

                #endregion

                #region 3. Notify All C&F For Booking Status. ---Pending

                List<TblSmsTO> smsTOList = new List<TblSmsTO>();

                TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.PURCHASE_BOOKINGS_CLOSED;//BOOKINGS_CLOSED;
                tblAlertInstanceTO.AlertAction = "PURCHASE_BOOKINGS_CLOSED";
                tblAlertInstanceTO.AlertComment = "Purchase Bookings closed.";
                tblAlertInstanceTO.EffectiveFromDate = tblBookingActionsTO.StatusDate;
                tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                tblAlertInstanceTO.IsActive = 1;
                tblAlertInstanceTO.SourceDisplayId = "PURCHASE_BOOKINGS_CLOSED";
                tblAlertInstanceTO.SourceEntityId = tblBookingActionsTO.IdBookingAction;
                tblAlertInstanceTO.RaisedBy = tblBookingActionsTO.StatusBy;
                tblAlertInstanceTO.RaisedOn = tblBookingActionsTO.StatusDate;
                tblAlertInstanceTO.IsAutoReset = 1;
                if (smsTOList != null)
                {
                    tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();
                    tblAlertInstanceTO.SmsTOList = smsTOList;
                }

                //Reset Prev alert
                tblAlertInstanceTO.AlertsToReset = new AlertsToReset();
                tblAlertInstanceTO.AlertsToReset.AlertDefIdList.Add((int)NotificationConstants.NotificationsE.PURCHASE_NEW_RATE_DECLARED);
                tblAlertInstanceTO.AlertsToReset.AlertDefIdList.Add((int)NotificationConstants.NotificationsE.PURCHASE_BOOKINGS_CLOSED);

                
                notify.SendNotificationToUsers(tblAlertInstanceTO);

                #endregion

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Booking Confirmed Sucessfully";
                resultMessage.Tag = tblBookingActionsTO;
                resultMessage.Result = 1;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Tag = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "Exception Error in Method SaveBookingActions";
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region Updation
        /*
        public  int UpdateTblBookingActions(TblBookingActionsTO tblBookingActionsTO)
        {
            return TblBookingActionsDAO.UpdateTblBookingActions(tblBookingActionsTO);
        }

        public  int UpdateTblBookingActions(TblBookingActionsTO tblBookingActionsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingActionsDAO.UpdateTblBookingActions(tblBookingActionsTO, conn, tran);
        }
        */
        #endregion

        #region Deletion
        /*
        public  int DeleteTblBookingActions(Int32 idBookingAction)
        {
            return TblBookingActionsDAO.DeleteTblBookingActions(idBookingAction);
        }

        public  int DeleteTblBookingActions(Int32 idBookingAction, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingActionsDAO.DeleteTblBookingActions(idBookingAction, conn, tran);
        }
        */
        #endregion

    }
}
