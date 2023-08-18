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
    public class TblUnLoadingBL
    {
        #region Selection

        public static List<TblUnLoadingTO> SelectAllTblUnLoadingList(DateTime startDate, DateTime endDate)
        {
            //startDate = Constants.GetStartDateTime(startDate);
            //endDate = Constants.GetEndDateTime(endDate);
            startDate = Convert.ToDateTime(startDate);
            endDate = Convert.ToDateTime(endDate);
            return TblUnLoadingDAO.SelectAllTblUnLoading(startDate,endDate);
        }

        public static TblUnLoadingTO SelectTblUnLoadingTO(Int32 idUnLoading)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                // Get all perticular unloading slip details with respecive item details
                TblUnLoadingTO tblUnLoadingTO = TblUnLoadingDAO.SelectTblUnLoading(idUnLoading);
                if (tblUnLoadingTO != null)
                    tblUnLoadingTO.UnLoadingItemDetTOList = TblUnLoadingItemDetBL.SelectAllUnLoadingItemDetailsList(idUnLoading);

                return tblUnLoadingTO;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectTblUnLoadingTO");
                return null;
            }
        }


        public static List<UnloadingRptTO> SelectAllUnLoadingListForReport(DateTime startDate, DateTime endDate)
        {
            return TblUnLoadingDAO.SelectAllUnLoadingListForReport(startDate, endDate);
        }

        //Added by minal 27 May 2021 For Dropbox
        public static List<TblUnLoadingTO> SelectAllUnLoadingForDropbox(SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_MIGRATE_BEFORE_DAYS, conn, tran);
            if (tblConfigParamsTO == null)
            {
                resultMessage.DefaultBehaviour("Error Booking Unloading TO is null");
                return null;
            }

            DateTime statusDate = Constants.ServerDateTime.AddDays(-Convert.ToInt32(tblConfigParamsTO.ConfigParamVal));

            try
            {
                return TblUnLoadingDAO.SelectAllUnLoadingForDropbox(statusDate);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllTempLoading");
                return null;
            }
        }

        #endregion

        #region Insertion
        public static int InsertTblUnLoading(TblUnLoadingTO tblUnLoadingTO)
        {
            return TblUnLoadingDAO.InsertTblUnLoading(tblUnLoadingTO);
        }

        public static int InsertTblUnLoading(TblUnLoadingTO tblUnLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblUnLoadingDAO.InsertTblUnLoading(tblUnLoadingTO, conn, tran);
        }

        // Vaibhav [13-Sep-2017] save unloading slip
        public static ResultMessage SaveNewUnLoadingSlipDetails(TblUnLoadingTO tblUnLoadingTO)
        {

            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                result = InsertTblUnLoading(tblUnLoadingTO, conn, tran);

                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While InsertTblUnLoading");
                    return resultMessage;
                }

                if (tblUnLoadingTO.UnLoadingItemDetTOList.Count != 0)
                {
                    for (int i = 0; i < tblUnLoadingTO.UnLoadingItemDetTOList.Count; i++)
                    {
                        tblUnLoadingTO.UnLoadingItemDetTOList[i].CreatedBy = tblUnLoadingTO.CreatedBy;
                        tblUnLoadingTO.UnLoadingItemDetTOList[i].CreatedOn = tblUnLoadingTO.CreatedOn;
                        tblUnLoadingTO.UnLoadingItemDetTOList[i].UnLoadingId = tblUnLoadingTO.IdUnLoading;

                        result = BL.TblUnLoadingItemDetBL.InsertTblUnLoadingItemDet(tblUnLoadingTO.UnLoadingItemDetTOList[i], conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While InsertTblUnLoadingItemDet");
                            return resultMessage;
                        }
                    }

                    // Vaibhav [14-Sep-2017] Calculate total unload qty from itemdetails
                    tblUnLoadingTO.TotalUnLoadingQty = tblUnLoadingTO.UnLoadingItemDetTOList.Sum(i => i.UnLoadingQty);

                    result = UpdateUnloadingQuantity(tblUnLoadingTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Error While UpdateUnloadingQuantity");
                        return resultMessage;
                    }
                }

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "SaveNewUnLoadingSlipDetails");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region Updation
        public static int UpdateTblUnLoading(TblUnLoadingTO tblUnLoadingTO)
        {
            return TblUnLoadingDAO.UpdateTblUnLoading(tblUnLoadingTO);
        }


        public static int UpdateTblUnLoading(TblUnLoadingTO tblUnLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblUnLoadingDAO.UpdateTblUnLoading(tblUnLoadingTO, conn, tran);
        }

        // Vaibhav [14-Sep-2017] Update unloading slip details
        public static ResultMessage UpdateUnLoadingSlipDetails(TblUnLoadingTO tblUnLoadingTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                result = UpdateTblUnLoading(tblUnLoadingTO, conn, tran);

                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While UpdateTblUnLoading");
                    return resultMessage;
                }

                if (tblUnLoadingTO.UnLoadingItemDetTOList.Count != 0)
                {
                    for (int i = 0; i < tblUnLoadingTO.UnLoadingItemDetTOList.Count; i++)
                    {
                        tblUnLoadingTO.UnLoadingItemDetTOList[i].UpdatedBy = tblUnLoadingTO.UpdatedBy;
                        tblUnLoadingTO.UnLoadingItemDetTOList[i].UpdatedOn = tblUnLoadingTO.UpdatedOn;
                        tblUnLoadingTO.UnLoadingItemDetTOList[i].UnLoadingId = tblUnLoadingTO.IdUnLoading;

                        result = BL.TblUnLoadingItemDetBL.UpdateTblUnLoadingItemDet(tblUnLoadingTO.UnLoadingItemDetTOList[i], conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While UpdateTblUnLoadingItemDet");
                            return resultMessage;
                        }
                    }

                    // Vaibhav [14-Sep-2017] Calculate total unload qty from itemdetails                   
                    tblUnLoadingTO.TotalUnLoadingQty = tblUnLoadingTO.UnLoadingItemDetTOList.Sum(i => i.UnLoadingQty);

                    result = UpdateUnloadingQuantity(tblUnLoadingTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Error While UpdateUnloadingQuantity");
                        return resultMessage;
                    }
                }
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateUnLoadingSlipDetails");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        // Vaibhav [14-Sep-2017] Update total unloading qty for perticular unloading transaction
        public static int UpdateUnloadingQuantity(TblUnLoadingTO tblUnLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                return TblUnLoadingDAO.UpdateUnLoadingQty(tblUnLoadingTO, conn, tran);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateUnloadingQuantity");
                return 0;
            }
        }


        // Vaibhav [12-oct-2017] added to deactivate unloading slip
        public static ResultMessage DeactivateUnLoadingSlip(TblUnLoadingTO tblUnLoadingTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                conn.Open();
                result =  TblUnLoadingDAO.DeactivateUnLoadingSlip(tblUnLoadingTO, conn, tran);

                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While DeactivateUnLoadingSlip");
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "DeactivateUnLoadingSlip");
                return resultMessage;
            }
        }

        #endregion

        #region Deletion
        public static int DeleteTblUnLoading(Int32 idUnLoading)
        {
            return TblUnLoadingDAO.DeleteTblUnLoading(idUnLoading);
        }

        public static int DeleteTblUnLoading(Int32 idUnLoading, SqlConnection conn, SqlTransaction tran)
        {
            return TblUnLoadingDAO.DeleteTblUnLoading(idUnLoading, conn, tran);
        }

        #endregion

    }
}
