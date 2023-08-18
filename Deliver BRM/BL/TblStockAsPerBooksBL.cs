using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using System.Linq;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.BL
{
    public class TblStockAsPerBooksBL
    {
        #region Selection
       
        public static List<TblStockAsPerBooksTO> SelectAllTblStockAsPerBooksList()
        {
            return  TblStockAsPerBooksDAO.SelectAllTblStockAsPerBooks();
        }

        public static TblStockAsPerBooksTO SelectTblStockAsPerBooksTO(Int32 idStockAsPerBooks)
        {
            return TblStockAsPerBooksDAO.SelectTblStockAsPerBooks(idStockAsPerBooks);
        }

        public static TblStockAsPerBooksTO SelectTblStockAsPerBooksTO(DateTime stockDate)
        {
            return TblStockAsPerBooksDAO.SelectTblStockAsPerBooks(stockDate);
        }

        #endregion

        #region Insertion
        public static int InsertTblStockAsPerBooks(TblStockAsPerBooksTO tblStockAsPerBooksTO)
        {
            return TblStockAsPerBooksDAO.InsertTblStockAsPerBooks(tblStockAsPerBooksTO);
        }

        public static ResultMessage SaveStockAsPerBooks(TblStockAsPerBooksTO tblStockAsPerBooksTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                #region 1. Save the Stock as per books

                int result = TblStockAsPerBooksDAO.InsertTblStockAsPerBooks(tblStockAsPerBooksTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While InsertTblStockAsPerBooks";
                    resultMessage.Tag = tblStockAsPerBooksTO;
                    return resultMessage;
                }

                #endregion

                #region 2. Notification to Directors and account person on stock confirmation

                TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.TODAYS_STOCK_AS_PER_ACCOUNTANT;
                tblAlertInstanceTO.AlertAction = "TODAYS_STOCK_AS_PER_ACCOUNTANT";
                tblAlertInstanceTO.AlertComment = "Stock Ready for quota declaration . Stock(In MT) as per books is :" + tblStockAsPerBooksTO.StockInMT;
                tblAlertInstanceTO.EffectiveFromDate = tblStockAsPerBooksTO.CreatedOn;
                tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                tblAlertInstanceTO.IsActive = 1;
                tblAlertInstanceTO.SourceDisplayId = "TODAYS_STOCK_AS_PER_ACCOUNTANT";
                tblAlertInstanceTO.SourceEntityId = tblStockAsPerBooksTO.IdStockAsPerBooks;
                tblAlertInstanceTO.RaisedBy = tblStockAsPerBooksTO.CreatedBy;
                tblAlertInstanceTO.RaisedOn = tblStockAsPerBooksTO.CreatedOn;
                tblAlertInstanceTO.IsAutoReset = 1;

                ResultMessage rMessage = BL.TblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                if (rMessage.MessageType != ResultMessageE.Information)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While SaveNewAlertInstance";
                    resultMessage.Tag = tblAlertInstanceTO;
                    return resultMessage;
                }

                #endregion

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Stock Saved Sucessfully";
                resultMessage.Result = 1;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception While SaveStockAsPerBooks";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public static int InsertTblStockAsPerBooks(TblStockAsPerBooksTO tblStockAsPerBooksTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblStockAsPerBooksDAO.InsertTblStockAsPerBooks(tblStockAsPerBooksTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblStockAsPerBooks(TblStockAsPerBooksTO tblStockAsPerBooksTO)
        {
            return TblStockAsPerBooksDAO.UpdateTblStockAsPerBooks(tblStockAsPerBooksTO);
        }

        public static int UpdateTblStockAsPerBooks(TblStockAsPerBooksTO tblStockAsPerBooksTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblStockAsPerBooksDAO.UpdateTblStockAsPerBooks(tblStockAsPerBooksTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblStockAsPerBooks(Int32 idStockAsPerBooks)
        {
            return TblStockAsPerBooksDAO.DeleteTblStockAsPerBooks(idStockAsPerBooks);
        }

        public static int DeleteTblStockAsPerBooks(Int32 idStockAsPerBooks, SqlConnection conn, SqlTransaction tran)
        {
            return TblStockAsPerBooksDAO.DeleteTblStockAsPerBooks(idStockAsPerBooks, conn, tran);
        }

        #endregion
        
    }
}
