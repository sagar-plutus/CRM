using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace PurchaseTrackerAPI.BL
{
    public class TblPurchaseGradingDtlsBL : ITblPurchaseGradingDtlsBL
    {
        private readonly Icommondao _iCommonDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ITblPurchaseVehicleStageCntBL _iTblPurchaseVehicleStageCntBL;
        private readonly ITblQualityPhaseBL _iTblQualityPhaseBL;
        private readonly ITblPurchaseGradingDtlsDAO _iTblPurchaseGradingDtlsDAO;
        public TblPurchaseGradingDtlsBL(
            Icommondao icommondao,
            IConnectionString iConnectionString,
            ITblQualityPhaseBL iTblQualityPhaseBL
                , ITblPurchaseGradingDtlsDAO iTblPurchaseGradingDtlsDAO
                , ITblPurchaseVehicleStageCntBL iTblPurchaseVehicleStageCntBL
            )
        {
            _iCommonDAO = icommondao;

            _iConnectionString = iConnectionString;

            _iTblPurchaseVehicleStageCntBL = iTblPurchaseVehicleStageCntBL;
           _iTblQualityPhaseBL = iTblQualityPhaseBL;
            _iTblPurchaseGradingDtlsDAO = iTblPurchaseGradingDtlsDAO;
        }
        #region Selection
        public  List<TblPurchaseGradingDtlsTO> SelectAllTblPurchaseGradingDtls()
        {
            return _iTblPurchaseGradingDtlsDAO.SelectAllTblPurchaseGradingDtls();
        }

        public  List<TblPurchaseGradingDtlsTO> SelectAllTblPurchaseGradingDtlsList()
        {
            return _iTblPurchaseGradingDtlsDAO.SelectAllTblPurchaseGradingDtls();
            //return ConvertDTToList(tblPurchaseGradingDtlsTODT);
        }

        public  TblPurchaseGradingDtlsTO SelectTblPurchaseGradingDtlsTO(Int32 idGradingDtls)
        {
            List<TblPurchaseGradingDtlsTO> tblPurchaseGradingDtlsTOList = _iTblPurchaseGradingDtlsDAO.SelectTblPurchaseGradingDtls(idGradingDtls);
            //List<TblPurchaseGradingDtlsTO> tblPurchaseGradingDtlsTOList = ConvertDTToList(tblPurchaseGradingDtlsTODT);
            if (tblPurchaseGradingDtlsTOList != null && tblPurchaseGradingDtlsTOList.Count == 1)
                return tblPurchaseGradingDtlsTOList[0];
            else
                return null;
        }
        public  List<TblPurchaseGradingDtlsTO> SelectTblPurchaseGradingDtlsTOListByWeighingId(Int32 weighingStageId)
        {
            List<TblPurchaseGradingDtlsTO> tblPurchaseGradingDtlsTOList = _iTblPurchaseGradingDtlsDAO.SelectTblPurchaseGradingDtlsTOListByWeighingId(weighingStageId);
            return tblPurchaseGradingDtlsTOList;
            //List<TblPurchaseGradingDtlsTO> tblPurchaseGradingDtlsTOList = ConvertDTToList(tblPurchaseGradingDtlsTODT);
        }
        public  List<TblPurchaseGradingDtlsTO> SelectTblPurchaseGradingDtlsTOListByWeighingId(Int32 weighingStageId, SqlConnection conn, SqlTransaction tran)
        {
            List<TblPurchaseGradingDtlsTO> tblPurchaseGradingDtlsTOList = _iTblPurchaseGradingDtlsDAO.SelectTblPurchaseGradingDtlsTOListByWeighingId(weighingStageId, conn, tran);
            return tblPurchaseGradingDtlsTOList;
            //List<TblPurchaseGradingDtlsTO> tblPurchaseGradingDtlsTOList = ConvertDTToList(tblPurchaseGradingDtlsTODT);
        }

        public  List<TblPurchaseGradingDtlsTO> SelectTblPurchaseGradingDtlsTOListByGradingDtlsId(Int32 gradingDtlsId, SqlConnection conn, SqlTransaction tran)
        {
            List<TblPurchaseGradingDtlsTO> tblPurchaseGradingDtlsTOList = _iTblPurchaseGradingDtlsDAO.SelectTblPurchaseGradingDtlsTOListByGradingDtlsId(gradingDtlsId, conn, tran);
            return tblPurchaseGradingDtlsTOList;
            //List<TblPurchaseGradingDtlsTO> tblPurchaseGradingDtlsTOList = ConvertDTToList(tblPurchaseGradingDtlsTODT);
        }


        public  List<TblPurchaseGradingDtlsTO> SelectTblPurchaseGradingDtlsTOListByScheduleId(string purchaseScheduleId)
        {
            List<TblPurchaseGradingDtlsTO> tblPurchaseGradingDtlsTOList = _iTblPurchaseGradingDtlsDAO.SelectTblPurchaseGradingDtlsTOListByScheduleId(purchaseScheduleId);
            return tblPurchaseGradingDtlsTOList;
        }

        public  List<TblPurchaseGradingDtlsTO> SelectTblPurchaseGradingDtlsTOListByScheduleId(string purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            List<TblPurchaseGradingDtlsTO> tblPurchaseGradingDtlsTOList = _iTblPurchaseGradingDtlsDAO.SelectTblPurchaseGradingDtlsTOListByScheduleId(purchaseScheduleId, conn, tran);
            return tblPurchaseGradingDtlsTOList;
        }



        #endregion

        #region Insertion
        public  int InsertTblPurchaseGradingDtls(TblPurchaseGradingDtlsTO tblPurchaseGradingDtlsTO)
        {
            return _iTblPurchaseGradingDtlsDAO.InsertTblPurchaseGradingDtls(tblPurchaseGradingDtlsTO);
        }

        public  int InsertTblPurchaseGradingDtls(TblPurchaseGradingDtlsTO tblPurchaseGradingDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseGradingDtlsDAO.InsertTblPurchaseGradingDtls(tblPurchaseGradingDtlsTO, conn, tran);
        }


        public  ResultMessage SaveGradingMaterialDetails(List<TblPurchaseGradingDtlsTO> tblPurchaseGradingDtlsTOList, Int32 loginUserId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            Int32 result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                //Get First List deleted
                if (tblPurchaseGradingDtlsTOList != null && tblPurchaseGradingDtlsTOList.Count > 0)
                {

                    Int32 weightStageId = tblPurchaseGradingDtlsTOList[0].PurchaseWeighingStageId;

                    List<TblPurchaseGradingDtlsTO> tblPurchaseGradingDtlsTOListTemp = _iTblPurchaseGradingDtlsDAO.SelectTblPurchaseGradingDtlsTOListByWeighingId(weightStageId, conn, tran);
                    if (tblPurchaseGradingDtlsTOListTemp != null && tblPurchaseGradingDtlsTOListTemp.Count > 0)
                    {
                        for (int k = 0; k < tblPurchaseGradingDtlsTOListTemp.Count; k++)
                        {
                            result = _iTblPurchaseGradingDtlsDAO.DeleteTblPurchaseGradingDtls(tblPurchaseGradingDtlsTOListTemp[k].IdGradingDtls, conn, tran);
                            if (result <= 0)
                            {
                                tran.Rollback();
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Result = 0;
                                resultMessage.Text = "Error While Updating Material Details";
                                return resultMessage;
                            }

                        }
                    }

                    for (int i = 0; i < tblPurchaseGradingDtlsTOList.Count; i++)
                    {
                        DateTime createdDate =  _iCommonDAO.ServerDateTime;
                        tblPurchaseGradingDtlsTOList[i].CreatedOn = createdDate;
                        tblPurchaseGradingDtlsTOList[i].CreatedBy = loginUserId;

                        result = _iTblPurchaseGradingDtlsDAO.InsertTblPurchaseGradingDtls(tblPurchaseGradingDtlsTOList[i], conn, tran);
                        if (result <= 0)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Result = 0;
                            resultMessage.Text = "Error While Updating Material Details";
                            return resultMessage;
                        }

                    }
                    if (tblPurchaseGradingDtlsTOList[0].IsConfirmGrading > 0)
                    {

                        TblPurchaseScheduleSummaryTO ScheduleSummaryTO = new TblPurchaseScheduleSummaryTO();
                        ScheduleSummaryTO.RootScheduleId = tblPurchaseGradingDtlsTOList[0].PurchaseScheduleSummaryId;
                        resultMessage = _iTblPurchaseVehicleStageCntBL.InsertOrUpdateVehicleWtStageCount(ScheduleSummaryTO,null, null, null, tblPurchaseGradingDtlsTOList[0], conn, tran);
                        result = resultMessage.Result;

                    }
                }

                if (result >= 1)
                {
                    tran.Commit();
                    List<TblAlertUsersTO> AlertUsersTOList = new List<TblAlertUsersTO>();

                    string sourceEntityId = tblPurchaseGradingDtlsTOList[0].PurchaseScheduleSummaryId.ToString();
                    _iTblQualityPhaseBL.ResetAllPreviousNotification((int)NotificationConstants.NotificationsE.UNLOADING_COMPLETED, sourceEntityId);

                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Text = "Record Updated Successfully.";
                    resultMessage.Result = 1;
                    return resultMessage;
                }
                else
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While Updating Material Details";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

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

        #endregion

        #region Updation
        public  int UpdateTblPurchaseGradingDtls(TblPurchaseGradingDtlsTO tblPurchaseGradingDtlsTO)
        {
            return _iTblPurchaseGradingDtlsDAO.UpdateTblPurchaseGradingDtls(tblPurchaseGradingDtlsTO);
        }

        public  int UpdateTblPurchaseGradingDtls(TblPurchaseGradingDtlsTO tblPurchaseGradingDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseGradingDtlsDAO.UpdateTblPurchaseGradingDtls(tblPurchaseGradingDtlsTO, conn, tran);
        }

     
        #endregion

        #region Deletion
        public  int DeleteTblPurchaseGradingDtls(Int32 idGradingDtls)
        {
            return _iTblPurchaseGradingDtlsDAO.DeleteTblPurchaseGradingDtls(idGradingDtls);
        }

        public  int DeleteTblPurchaseGradingDtls(Int32 idGradingDtls, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseGradingDtlsDAO.DeleteTblPurchaseGradingDtls(idGradingDtls, conn, tran);
        }

        public int DeleteAllGradingDtls(Int32 scheduleSummaryId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseGradingDtlsDAO.DeleteAllGradingDtls(scheduleSummaryId, conn, tran);
        }


        public ResultMessage DeleteGradingDetails(List<TblPurchaseGradingDtlsTO> tblPurchaseGradingDtlsTOList)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            Int32 result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                //Get First List deleted
                if (tblPurchaseGradingDtlsTOList != null && tblPurchaseGradingDtlsTOList.Count > 0)
                {
                    for (int k = 0; k < tblPurchaseGradingDtlsTOList.Count; k++)
                    {
                        result = _iTblPurchaseGradingDtlsDAO.DeleteTblPurchaseGradingDtls(tblPurchaseGradingDtlsTOList[k].IdGradingDtls, conn, tran);
                        if (result <= 0)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Result = 0;
                            resultMessage.Text = "Error While Updating Material Details";
                            return resultMessage;
                        }

                    }
                }

                if (result >= 1)
                {
                    tran.Commit();
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Text = "Record Updated Successfully.";
                    resultMessage.Result = 1;
                    return resultMessage;
                }
                else
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While Updating Material Details";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

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

        public  int DeleteAllGradingDtlsAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseGradingDtlsDAO.DeleteAllGradingDtlsAgainstVehSchedule(purchaseScheduleId, conn, tran);
        }


        #endregion

    }
}
