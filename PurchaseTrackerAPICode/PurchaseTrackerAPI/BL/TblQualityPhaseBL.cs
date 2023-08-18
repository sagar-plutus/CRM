using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL
{
    public class TblQualityPhaseBL : ITblQualityPhaseBL
    {
        private readonly Icommondao _iCommonDAO;
        private readonly INotification notify;
        private readonly IDimVehiclePhaseBL _iDimVehiclePhaseBL;
        private readonly IDimQualitySampleTypeBL _iDimQualitySampleTypeBL;
        private readonly ITblQualityPhaseDAO _iTblQualityPhaseDAO;
        private readonly ITblQualityPhaseDtlsBL _iTblQualityPhaseDtlsBL;
        private readonly ITblQualityPhaseDtlsDAO _iTblQualityPhaseDtlsDAO;
        private readonly IConnectionString _iConnectionString;
        public TblQualityPhaseBL( IDimQualitySampleTypeBL iDimQualitySampleTypeBL, INotification inotify, IDimVehiclePhaseBL iDimVehiclePhaseBL, ITblQualityPhaseDAO iTblQualityPhaseDAO
                                    , ITblQualityPhaseDtlsDAO iTblQualityPhaseDtlsDAO
                                    , ITblQualityPhaseDtlsBL iTblQualityPhaseDtlsBL
                                    , IConnectionString iConnectionString
                                    , Icommondao icommondao)
        {
             notify = inotify;
            _iCommonDAO = icommondao;
            _iConnectionString = iConnectionString;
            _iTblQualityPhaseDtlsBL = iTblQualityPhaseDtlsBL;
            _iTblQualityPhaseDtlsDAO = iTblQualityPhaseDtlsDAO;
            _iTblQualityPhaseDAO = iTblQualityPhaseDAO;
            _iDimVehiclePhaseBL = iDimVehiclePhaseBL;
           _iDimQualitySampleTypeBL = iDimQualitySampleTypeBL;
        }


        #region Selection
        public  List<TblQualityPhaseTO> SelectAllTblQualityPhase()
        {
            return _iTblQualityPhaseDAO.SelectAllTblQualityPhase();
        }

        public  List<TblQualityPhaseTO> SelectAllTblQualityPhaseList(int PurchaseScheduleSummaryId)
        {
            return _iTblQualityPhaseDAO.SelectAllTblQualityPhase(PurchaseScheduleSummaryId);
            //return ConvertDTToList(tblQualityPhaseTODT);
        }
        public  List<TblQualityPhaseTO> SelectAllTblQualityPhaseList(Int32 purchaseScheduleSummaryId, Int32 isActive)
        {
            return _iTblQualityPhaseDAO.SelectTblQualityPhaseList(purchaseScheduleSummaryId, isActive);
            //return ConvertDTToList(tblQualityPhaseTODT);
        }

        public  TblQualityPhaseTO SelectTblQualityPhaseTO(Int32 idTblQualityPhase)
        {
            List<TblQualityPhaseTO> tblQualityPhaseTOList = _iTblQualityPhaseDAO.SelectTblQualityPhase(idTblQualityPhase);
            //List<TblQualityPhaseTO> tblQualityPhaseTOList = ConvertDTToList(tblQualityPhaseTODT);
            if (tblQualityPhaseTOList != null && tblQualityPhaseTOList.Count == 1)
                return tblQualityPhaseTOList[0];
            else
                return null;
        }


        #endregion

        #region Insertion
        public  int InsertTblQualityPhase(TblQualityPhaseTO tblQualityPhaseTO)
        {
            return _iTblQualityPhaseDAO.InsertTblQualityPhase(tblQualityPhaseTO);
        }

        public  int InsertTblQualityPhase(TblQualityPhaseTO tblQualityPhaseTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblQualityPhaseDAO.InsertTblQualityPhase(tblQualityPhaseTO, conn, tran);
        }

        #endregion

        #region Updation
        public  int UpdateTblQualityPhase(TblQualityPhaseTO tblQualityPhaseTO)
        {
            return _iTblQualityPhaseDAO.UpdateTblQualityPhase(tblQualityPhaseTO);
        }

        public  int UpdateTblQualityPhase(TblQualityPhaseTO tblQualityPhaseTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblQualityPhaseDAO.UpdateTblQualityPhase(tblQualityPhaseTO, conn, tran);
        }

        public  int UpdateTblQualityPhaseDeAct(TblQualityPhaseTO tblQualityPhaseTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblQualityPhaseDAO.UpdateTblQualityPhaseDeact(tblQualityPhaseTO, conn, tran);
        }

        #endregion

        #region Deletion
        public  int DeleteTblQualityPhase(Int32 idTblQualityPhase)
        {
            return _iTblQualityPhaseDAO.DeleteTblQualityPhase(idTblQualityPhase);
        }

        public  int DeleteTblQualityPhase(Int32 idTblQualityPhase, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblQualityPhaseDAO.DeleteTblQualityPhase(idTblQualityPhase, conn, tran);
        }

        public  int DeleteAllQualityPhaseAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblQualityPhaseDAO.DeleteAllQualityPhaseAgainstVehSchedule(purchaseScheduleId, conn, tran);
        }


        //Deepali 24-12-2018 for Quality CheckList
        public  List<TblQualityPhaseTO> GetQualityFlagCheckLists(int idPurchaseScheduleSummary, int userId, int qualityFormTypeE, int flagTypeId)
        {

            List<TblQualityPhaseTO> tblQualityPhaseTOListForReturn = new List<TblQualityPhaseTO>();
            // if (flagTypeId == 0)
            // {
            if (qualityFormTypeE == Convert.ToInt32(Constants.QualityFormTypeE.RAISE))
            {
                List<TblQualityPhaseTO> tblQualityPhaseTOList = new List<TblQualityPhaseTO>();
                //List<TblQualityPhaseTO> tblQualityPhaseTOTemp = new List<TblQualityPhaseTO>();
                //TblQualityPhaseTO tblQualityPhaseTO;
                // tblQualityPhaseTOListForReturn = new List<TblQualityPhaseTO>();
                List<DimQualitySampleTypeTO> dimQualitySampleTypeTOListAll = _iDimQualitySampleTypeBL.SelectAllDimQualitySampleTypeList();
                List<DimVehiclePhaseTO> VehiclePhaseList = _iDimVehiclePhaseBL.SelectAllDimVehiclePhaseList(0);

                tblQualityPhaseTOList = SelectAllTblQualityPhaseList(idPurchaseScheduleSummary);
                for (int j = 0; j < tblQualityPhaseTOList.Count; j++)
                {
                    tblQualityPhaseTOList[j].QualityPhaseDtlsTOList = _iTblQualityPhaseDtlsBL.SelectAllTblQualityPhaseDtlsList(tblQualityPhaseTOList[j].IdTblQualityPhase);
                }

                for (int i = 0; i < VehiclePhaseList.Count; i++)
                {
                    DimVehiclePhaseTO dimVehiclePhaseTO = VehiclePhaseList[i];
                    List<DimQualitySampleTypeTO> DimQualitySampleTypeTOList = dimQualitySampleTypeTOListAll.Where(w => w.PhaseId == dimVehiclePhaseTO.IdVehiclePhase).ToList();
                    if (DimQualitySampleTypeTOList == null || DimQualitySampleTypeTOList.Count == 0)
                    {
                        continue;
                    }

                    //TblQualityPhaseTO tblQualityPhaseTO = new TblQualityPhaseTO();
                    TblQualityPhaseTO tblQualityPhaseTO = tblQualityPhaseTOList.Where(w => w.VehiclePhaseId == VehiclePhaseList[i].IdVehiclePhase && w.FlagTypeId == flagTypeId).FirstOrDefault();
                    if (tblQualityPhaseTO != null)
                    {
                        tblQualityPhaseTOListForReturn.Add(tblQualityPhaseTO);
                    }
                    else
                    {
                        tblQualityPhaseTO = new TblQualityPhaseTO();
                        tblQualityPhaseTO.PurchaseScheduleSummaryId = idPurchaseScheduleSummary;
                        tblQualityPhaseTO.VehiclePhaseId = VehiclePhaseList[i].IdVehiclePhase;
                        tblQualityPhaseTO.VehiclePhaseName = VehiclePhaseList[i].PhaseName;
                        tblQualityPhaseTO.VehiclePhaseSequanceNo = VehiclePhaseList[i].SequanceNo;
                        tblQualityPhaseTO.CreatedBy = userId;
                        tblQualityPhaseTO.CreatedOn =  _iCommonDAO.ServerDateTime;
                        tblQualityPhaseTOListForReturn.Add(tblQualityPhaseTO);
                    }


                    for (int Sample = 0; Sample < DimQualitySampleTypeTOList.Count; Sample++)
                    {
                        DimQualitySampleTypeTO dimQualitySampleTypeTO = DimQualitySampleTypeTOList[Sample];

                        // if (dimQualitySampleTypeTO.FlagTypeId == flagTypeId)
                        // {
                        if (tblQualityPhaseTO.QualityPhaseDtlsTOList == null)
                        {
                            tblQualityPhaseTO.QualityPhaseDtlsTOList = new List<TblQualityPhaseDtlsTO>();
                        }

                        TblQualityPhaseDtlsTO existingSampleTypeTO = tblQualityPhaseTO.QualityPhaseDtlsTOList.Where(w => w.QualitySampleTypeId == dimQualitySampleTypeTO.IdQualitySampleType).FirstOrDefault();

                        if (existingSampleTypeTO != null)
                        {
                            existingSampleTypeTO.VehiclePhaseName = tblQualityPhaseTO.VehiclePhaseName;
                            existingSampleTypeTO.IsSelected = 1;
                            existingSampleTypeTO.IsChecked = true;
                            if (existingSampleTypeTO.FlagStatusId > 0)
                            {
                                existingSampleTypeTO.FlagStatusIdBool = true;
                            }
                            existingSampleTypeTO.FlagTypeId = dimQualitySampleTypeTO.FlagTypeId;
                            existingSampleTypeTO.QualitySampleTypeName = dimQualitySampleTypeTO.SampleTypeName;
                            existingSampleTypeTO.QualitySampleTypeParentId = dimQualitySampleTypeTO.ParentSampleTypeId;
                            //existingSampleTypeTO  //Selected Property true.
                        }
                        else
                        {
                            existingSampleTypeTO = new TblQualityPhaseDtlsTO();

                            existingSampleTypeTO.VehiclePhaseName = tblQualityPhaseTO.VehiclePhaseName;
                            existingSampleTypeTO.FlagTypeId = dimQualitySampleTypeTO.FlagTypeId;
                            existingSampleTypeTO.QualitySampleTypeId = dimQualitySampleTypeTO.IdQualitySampleType;
                            existingSampleTypeTO.QualitySampleTypeName = dimQualitySampleTypeTO.SampleTypeName;
                            existingSampleTypeTO.QualitySampleTypeParentId = dimQualitySampleTypeTO.ParentSampleTypeId;
                            existingSampleTypeTO.IsSelected = 0;
                            existingSampleTypeTO.FlagStatusIdBool = false;
                            existingSampleTypeTO.IsChecked = false;
                            existingSampleTypeTO.CreatedBy = userId;
                            existingSampleTypeTO.CreatedOn =  _iCommonDAO.ServerDateTime;
                            tblQualityPhaseTO.QualityPhaseDtlsTOList.Add(existingSampleTypeTO);

                        }
                        // }
                    }
                }
            }
            else if (qualityFormTypeE == Convert.ToInt32(Constants.QualityFormTypeE.COMPLETE))
            {
                List<TblQualityPhaseTO> tblQualityPhaseTOList = new List<TblQualityPhaseTO>();
                //List<TblQualityPhaseTO> tblQualityPhaseTOTemp = new List<TblQualityPhaseTO>();
                //TblQualityPhaseTO tblQualityPhaseTO;
                // tblQualityPhaseTOListForReturn = new List<TblQualityPhaseTO>();
                List<DimQualitySampleTypeTO> dimQualitySampleTypeTOListAll = _iDimQualitySampleTypeBL.SelectAllDimQualitySampleTypeList();
                List<DimVehiclePhaseTO> VehiclePhaseList = _iDimVehiclePhaseBL.SelectAllDimVehiclePhaseList(0);

                tblQualityPhaseTOList = SelectAllTblQualityPhaseList(idPurchaseScheduleSummary);
                for (int j = 0; j < tblQualityPhaseTOList.Count; j++)
                {
                    tblQualityPhaseTOList[j].QualityPhaseDtlsTOList = _iTblQualityPhaseDtlsBL.SelectAllTblQualityPhaseDtlsList(tblQualityPhaseTOList[j].IdTblQualityPhase);
                }

                for (int i = 0; i < VehiclePhaseList.Count; i++)
                {
                    DimVehiclePhaseTO dimVehiclePhaseTO = VehiclePhaseList[i];
                    List<DimQualitySampleTypeTO> DimQualitySampleTypeTOList = dimQualitySampleTypeTOListAll.Where(w => w.PhaseId == dimVehiclePhaseTO.IdVehiclePhase).ToList();
                    if (DimQualitySampleTypeTOList == null || DimQualitySampleTypeTOList.Count == 0)
                    {
                        continue;
                    }

                    //TblQualityPhaseTO tblQualityPhaseTO = new TblQualityPhaseTO();
                    TblQualityPhaseTO tblQualityPhaseTO = tblQualityPhaseTOList.Where(w => w.VehiclePhaseId == VehiclePhaseList[i].IdVehiclePhase && w.FlagTypeId == flagTypeId).FirstOrDefault();
                    if (tblQualityPhaseTO != null)
                    {
                        tblQualityPhaseTOListForReturn.Add(tblQualityPhaseTO);
                    }
                    else
                    {
                        tblQualityPhaseTO = new TblQualityPhaseTO();
                        tblQualityPhaseTO.PurchaseScheduleSummaryId = idPurchaseScheduleSummary;
                        tblQualityPhaseTO.VehiclePhaseId = VehiclePhaseList[i].IdVehiclePhase;
                        tblQualityPhaseTO.VehiclePhaseName = VehiclePhaseList[i].PhaseName;
                        tblQualityPhaseTO.VehiclePhaseSequanceNo = VehiclePhaseList[i].SequanceNo;
                        tblQualityPhaseTO.CreatedBy = userId;
                        tblQualityPhaseTO.CreatedOn =  _iCommonDAO.ServerDateTime;
                        tblQualityPhaseTOListForReturn.Add(tblQualityPhaseTO);
                    }


                    for (int Sample = 0; Sample < DimQualitySampleTypeTOList.Count; Sample++)
                    {
                        DimQualitySampleTypeTO dimQualitySampleTypeTO = DimQualitySampleTypeTOList[Sample];
                        if (tblQualityPhaseTO.QualityPhaseDtlsTOList == null)
                        {
                            tblQualityPhaseTO.QualityPhaseDtlsTOList = new List<TblQualityPhaseDtlsTO>();
                        }

                        TblQualityPhaseDtlsTO existingSampleTypeTO = tblQualityPhaseTO.QualityPhaseDtlsTOList.Where(w => w.QualitySampleTypeId == dimQualitySampleTypeTO.IdQualitySampleType).FirstOrDefault();

                        if (existingSampleTypeTO != null)
                        {
                            existingSampleTypeTO.IsSelected = 1;
                            existingSampleTypeTO.IsChecked = true;
                            if (existingSampleTypeTO.FlagStatusId > 0)
                            {
                                existingSampleTypeTO.FlagStatusIdBool = true;
                            }
                            existingSampleTypeTO.VehiclePhaseName = tblQualityPhaseTO.VehiclePhaseName;

                            existingSampleTypeTO.FlagTypeId = dimQualitySampleTypeTO.FlagTypeId;
                            existingSampleTypeTO.QualitySampleTypeName = dimQualitySampleTypeTO.SampleTypeName;
                            existingSampleTypeTO.QualitySampleTypeParentId = dimQualitySampleTypeTO.ParentSampleTypeId;
                            //existingSampleTypeTO  //Selected Property true.
                        }
                    }
                }

                List<TblQualityPhaseTO> tblQualityPhaseTOListTemp = new List<TblQualityPhaseTO>();
                tblQualityPhaseTOListTemp = tblQualityPhaseTOListForReturn;
                tblQualityPhaseTOListForReturn = new List<TblQualityPhaseTO>(); ;
                foreach (var item in tblQualityPhaseTOListTemp)
                {
                    if (item.QualityPhaseDtlsTOList.Count > 0)
                    {
                        tblQualityPhaseTOListForReturn.Add(item);
                    }
                }
            }

            if (flagTypeId != 0)
            {
                List<TblQualityPhaseTO> tblQualityPhaseTOListNew = new List<TblQualityPhaseTO>();
                tblQualityPhaseTOListNew = tblQualityPhaseTOListForReturn;
                tblQualityPhaseTOListForReturn = new List<TblQualityPhaseTO>();
                foreach (var item in tblQualityPhaseTOListNew)
                {

                    List<TblQualityPhaseDtlsTO> tblQualityPhaseDtlsTOListNew = new List<TblQualityPhaseDtlsTO>();
                    tblQualityPhaseDtlsTOListNew = item.QualityPhaseDtlsTOList.Where(w => w.FlagTypeId == flagTypeId).ToList();
                    if (tblQualityPhaseDtlsTOListNew != null && tblQualityPhaseDtlsTOListNew.Count > 0)
                    {
                        item.FlagTypeId = flagTypeId;
                        item.QualityPhaseDtlsTOList = tblQualityPhaseDtlsTOListNew;
                        tblQualityPhaseTOListForReturn.Add(item);
                    }
                }
            }
            return tblQualityPhaseTOListForReturn;
        }

       public  List<DropDownTO> GetAllIdsForSampleType(int purchaseScheduleSummaryId, int vehiclePhaseId, int flagTypeId, int QualitySampleTypeId)
        {
            return _iTblQualityPhaseDAO.GetAllIdsForSampleType(purchaseScheduleSummaryId, vehiclePhaseId, flagTypeId, QualitySampleTypeId);
        }

        public ResultMessage SavePhaseSampleListsagainstPurrchaseScheduleSummaryID(List<TblQualityPhaseTO> tblQualityPhaseTOList, int loginUserId)
        {
            ResultMessage ResultMessage = new ResultMessage();
            DateTime createdDate =  _iCommonDAO.ServerDateTime;
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            DateTime serverDateTime =  _iCommonDAO.ServerDateTime;
            try
            {
                int result = 0;
                if (tblQualityPhaseTOList != null && tblQualityPhaseTOList.Count > 0)
                {
                    conn.Open();
                    tran = conn.BeginTransaction();
                    for (int e = 0; e < tblQualityPhaseTOList.Count; e++)
                    {
                        TblQualityPhaseTO tblQualityPhaseTO1 = tblQualityPhaseTOList[e];
                        if (tblQualityPhaseTO1.PurchaseScheduleSummaryId > 0)
                        {
                            result = _iTblQualityPhaseDAO.SelectFromDtlsAndQuality(tblQualityPhaseTO1.PurchaseScheduleSummaryId, tblQualityPhaseTO1.VehiclePhaseId, tblQualityPhaseTO1.FlagTypeId, conn, tran);
                            if (result > 0)
                            {
                                tblQualityPhaseTO1.UpdatedOn = serverDateTime;
                                tblQualityPhaseTO1.UpdatedBy = loginUserId;
                                tblQualityPhaseTO1.IsActive = 0;
                                result = UpdateTblQualityPhaseDeAct(tblQualityPhaseTO1, conn, tran);
                                if (result > 0)
                                { }
                                else
                                {
                                    tran.Rollback();
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Text = "Error While InsertTblQualityPhase";
                                    return resultMessage;
                                }
                            }
                        }
                    }
                    // if ()
                    {
                        foreach (var tblQualityPhaseTO in tblQualityPhaseTOList)
                        {
                            foreach (var QualityPhaseDtlsTO in tblQualityPhaseTO.QualityPhaseDtlsTOList)
                            {
                                List<TblAlertUsersTO> AlertUsersTOList = new List<TblAlertUsersTO>();


                                int defId = GetAlertDefinationIDForSampleType(QualityPhaseDtlsTO.QualitySampleTypeId, Convert.ToInt32(Constants.TranStatusE.FLAG_RAISED), conn, tran);
                                ResetAllPreviousNotification(defId, QualityPhaseDtlsTO.IdTblQualityPhaseDtls.ToString());

                                int defIdC = GetAlertDefinationIDForSampleType(QualityPhaseDtlsTO.QualitySampleTypeId, Convert.ToInt32(Constants.TranStatusE.QUALITY_FLAG_COMPLETED), conn, tran);
                                ResetAllPreviousNotification(defIdC, QualityPhaseDtlsTO.IdTblQualityPhaseDtls.ToString());

                            }
                        }
                    }

                    for (int j = 0; j < tblQualityPhaseTOList.Count; j++)
                    {
                        TblQualityPhaseTO tblQualityPhaseTO = tblQualityPhaseTOList[j];
                        tblQualityPhaseTO.CreatedBy = loginUserId;
                        tblQualityPhaseTO.UpdatedBy = loginUserId;
                        tblQualityPhaseTO.CreatedOn = createdDate;
                        tblQualityPhaseTO.UpdatedOn = createdDate;
                        tblQualityPhaseTO.IsActive = 1;

                        // ResultMessage = DeactivateIfAlreadyExistsAndInsertNew(tblQualityPhaseTOList[i]);

                        if (tblQualityPhaseTO != null)
                        {
                            if (tblQualityPhaseTO.PurchaseScheduleSummaryId > 0)
                            {
                                int count = 0;
                                count = tblQualityPhaseTO.QualityPhaseDtlsTOList.Where(w => w.IsChecked == true).ToList().Count();
                                int res = 0;
                                if (count > 0)
                                {
                                    res = InsertTblQualityPhase(tblQualityPhaseTO, conn, tran);
                                    if (res > 0)
                                    {
                                        List<TblQualityPhaseDtlsTO> dtlsList = tblQualityPhaseTO.QualityPhaseDtlsTOList;
                                        for (int i = 0; i < dtlsList.Count; i++)
                                        {
                                            if (dtlsList[i].IsChecked == false)
                                            {
                                                dtlsList[i].IsSelected = 0;
                                            }
                                            if (dtlsList[i].IsSelected == 1)
                                            {
                                                int result2 = 0;
                                                dtlsList[i].TblQualityPhaseId = tblQualityPhaseTO.IdTblQualityPhase;
                                                // dtlsList[i].FlagStatusId = tblQualityPhaseTO.FlagStatusId;
                                                dtlsList[i].CreatedOn = tblQualityPhaseTO.CreatedOn;
                                                dtlsList[i].UpdatedOn = tblQualityPhaseTO.UpdatedOn;
                                                result2 = _iTblQualityPhaseDtlsBL.InsertTblQualityPhaseDtls(dtlsList[i], conn, tran);
                                                if (result2 > 0)
                                                {
                                                    if (dtlsList[i].FlagStatusId != (Int32)Constants.TranStatusE.QUALITY_FLAG_COMPLETED)
                                                    {
                                                        #region Send Notification
                                                        TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                                                        List<TblAlertUsersTO> AlertUsersTOList = new List<TblAlertUsersTO>();
                                                        int defId = GetAlertDefinationIDForSampleType(dtlsList[i].QualitySampleTypeId, Convert.ToInt32(Constants.TranStatusE.FLAG_RAISED), conn, tran);
                                                        ResetAllPreviousNotification(defId, dtlsList[i].IdTblQualityPhaseDtls.ToString());

                                                        if (defId > 0)
                                                        {
                                                            string VehicleName = GetVehicleNameForNotification(tblQualityPhaseTO.PurchaseScheduleSummaryId, conn, tran);

                                                            tblAlertInstanceTO.AlertDefinitionId = defId;
                                                            tblAlertInstanceTO.AlertAction = "FLAG_RAISED_AGAINST_VEHICLE";
                                                            tblAlertInstanceTO.AlertComment = dtlsList[i].QualitySampleTypeName + " flag raised against vehicle No: " + VehicleName + " For " + tblQualityPhaseTO.VehiclePhaseName;
                                                            // tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                                                            tblAlertInstanceTO.EffectiveFromDate = tblQualityPhaseTO.CreatedOn;
                                                            tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                                                            tblAlertInstanceTO.IsActive = 1;
                                                            tblAlertInstanceTO.SourceDisplayId = "FLAG_RAISED";
                                                            tblAlertInstanceTO.SourceEntityId = dtlsList[i].IdTblQualityPhaseDtls;
                                                            tblAlertInstanceTO.RaisedBy = tblQualityPhaseTO.CreatedBy;
                                                            tblAlertInstanceTO.RaisedOn = tblQualityPhaseTO.CreatedOn;
                                                            tblAlertInstanceTO.IsAutoReset = 1;

                                                        //    Notification notify = new Notification();
                                                            notify.SendNotificationToUsers(tblAlertInstanceTO);
                                                        }
                                                        #endregion
                                                    }

                                                }
                                                else
                                                {
                                                    tran.Rollback();
                                                    resultMessage.MessageType = ResultMessageE.Error;
                                                    resultMessage.Text = "Error While InsertTblQualityPhase";
                                                    return resultMessage;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        tran.Rollback();
                                        resultMessage.MessageType = ResultMessageE.Error;
                                        resultMessage.Text = "Error While InsertTblQualityPhase";
                                        return resultMessage;
                                    }
                                }
                            }
                        }

                    }
                    tran.Commit();
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Text = resultMessage.DisplayMessage = "Record Saved Sucessfully";
                    resultMessage.Result = 1;
                    return resultMessage;

                }

                return ResultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Tag = ex;
                resultMessage.Text = "Exception Error in Method DeactivateIfAlreadyExists";
                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }

        }


        public  void ResetAllPreviousNotification(int defId, string SourceEntityId)
        {
            string[] arr = SourceEntityId.Split(',');
            TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
            AlertsToReset alertsToReset = new AlertsToReset();
            alertsToReset.ResetAlertInstanceTOList = new List<ResetAlertInstanceTO>();
            foreach (var item in arr)
            {
                ResetAlertInstanceTO resetAlertInstanceTO = new ResetAlertInstanceTO();
                resetAlertInstanceTO.AlertDefinitionId = defId;
                resetAlertInstanceTO.SourceEntityTxnId = Convert.ToInt32(item);
                alertsToReset.ResetAlertInstanceTOList.Add(resetAlertInstanceTO);
            }

            tblAlertInstanceTO.AlertsToReset = alertsToReset;

            tblAlertInstanceTO.EffectiveFromDate =  _iCommonDAO.ServerDateTime;
            tblAlertInstanceTO.EffectiveToDate =  _iCommonDAO.ServerDateTime;

            tblAlertInstanceTO.RaisedOn =  _iCommonDAO.ServerDateTime;

            //tblAlertInstanceTO. =  _iCommonDAO.ServerDateTime;

            ResetAllAlerts(tblAlertInstanceTO);
        }

        public  void ResetAllAlerts(TblAlertInstanceTO tblAlertInstanceTO)
        {

            try
            {
                var values = new JObject();
                values.Add("tblAlertInstanceTO", JsonConvert.SerializeObject(tblAlertInstanceTO));

                apiData data = new apiData();
                data.tblAlertInstanceTO = tblAlertInstanceTO;

                MemoryStream ms = new MemoryStream();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(apiData));
                ser.WriteObject(ms, data);
                byte[] json = ms.ToArray();
                ms.Close();

                String notifyUrl = "Notify/PostResetOldAlerts";

                String url = Startup.CommonUrl + notifyUrl;
                object result;
                StreamWriter myWriter = null;
                WebRequest request = WebRequest.Create(url);
                request.Headers.Add("apiurl", _iConnectionString.GetConnectionString(Constants.REQUEST_ORIGIN_STRING));
                request.Method = "Post";
                request.ContentType = "application/json";

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(json, 0, json.Length);
                    requestStream.Close();
                }

                WebResponse objResponse = request.GetResponseAsync().Result;
                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    sr.Dispose();
                }

            }
            catch (Exception exc)
            {
                throw exc;
            }
        }



        public  string GetVehicleNameForNotification(int purchaseScheduleSummaryId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblQualityPhaseDtlsDAO.GetVehicleNameForNotification(purchaseScheduleSummaryId, conn, tran);

        }

        public  int GetAlertDefinationIDForSampleType(int qualitySampleTypeId, int Flag, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblQualityPhaseDtlsDAO.SelectAlertDefinationID(qualitySampleTypeId, Flag, conn, tran);

        }

        public  ResultMessage SaveCompletedPhaseSampleListsagainstPurrchaseScheduleSummaryID(List<TblQualityPhaseDtlsTO> tblQualityPhaseDtlsTOList, int loginUserId)
        {
            ResultMessage ResultMessage = new ResultMessage();
            DateTime createdDate =  _iCommonDAO.ServerDateTime;
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            DateTime serverDateTime =  _iCommonDAO.ServerDateTime;
            try
            {
                if (tblQualityPhaseDtlsTOList != null && tblQualityPhaseDtlsTOList.Count > 0)
                {
                    conn.Open();
                    tran = conn.BeginTransaction();

                    for (int j = 0; j < tblQualityPhaseDtlsTOList.Count; j++)
                    {
                        TblQualityPhaseDtlsTO tblQualityPhaseDtlsTO = tblQualityPhaseDtlsTOList[j];
                        tblQualityPhaseDtlsTO.StatusBy = loginUserId;
                        tblQualityPhaseDtlsTO.UpdatedBy = loginUserId;
                        tblQualityPhaseDtlsTO.StatusOn = serverDateTime;
                        tblQualityPhaseDtlsTO.UpdatedOn = serverDateTime;
                        int result2 = 0;
                        result2 = _iTblQualityPhaseDtlsBL.UpdateTblQualityPhaseDtls(tblQualityPhaseDtlsTO, conn, tran);
                        if (result2 > 0)
                        {
                            #region Send Notification

                            TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();

                            int defId = GetAlertDefinationIDForSampleType(tblQualityPhaseDtlsTO.QualitySampleTypeId, Convert.ToInt32(Constants.TranStatusE.QUALITY_FLAG_COMPLETED), conn, tran);
                            if (defId > 0)
                            {
                                List<TblAlertUsersTO> AlertUsersTOList = new List<TblAlertUsersTO>();

                                ResetAllPreviousNotification(defId, tblQualityPhaseDtlsTO.IdTblQualityPhaseDtls.ToString());

                                int defIdC = GetAlertDefinationIDForSampleType(tblQualityPhaseDtlsTO.QualitySampleTypeId, Convert.ToInt32(Constants.TranStatusE.FLAG_RAISED), conn, tran);
                                ResetAllPreviousNotification(defIdC, tblQualityPhaseDtlsTO.IdTblQualityPhaseDtls.ToString());



                                TblQualityPhaseTO PhaseTo = SelectTblQualityPhaseTO(tblQualityPhaseDtlsTO.TblQualityPhaseId);
                                string VehicleName = GetVehicleNameForNotification(PhaseTo.PurchaseScheduleSummaryId, conn, tran);

                                tblAlertInstanceTO.AlertDefinitionId = defId;
                                tblAlertInstanceTO.AlertAction = "FLAG_COMPLETED_AGAINST_VEHICLE";
                                tblAlertInstanceTO.AlertComment = tblQualityPhaseDtlsTO.QualitySampleTypeName + " flag completed against vehicle No: " + VehicleName + " For " + tblQualityPhaseDtlsTO.VehiclePhaseName;
                                // tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                                tblAlertInstanceTO.EffectiveFromDate = tblQualityPhaseDtlsTO.CreatedOn;
                                tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                                tblAlertInstanceTO.IsActive = 1;
                                tblAlertInstanceTO.SourceDisplayId = "FLAG_COMPLETED";
                                tblAlertInstanceTO.SourceEntityId = tblQualityPhaseDtlsTO.IdTblQualityPhaseDtls;
                                tblAlertInstanceTO.RaisedBy = tblQualityPhaseDtlsTO.CreatedBy;
                                tblAlertInstanceTO.RaisedOn = tblQualityPhaseDtlsTO.CreatedOn;
                                tblAlertInstanceTO.IsAutoReset = 1;

                                TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                                tblAlertInstanceTO.AlertUsersTOList = new List<TblAlertUsersTO>();
                                tblAlertUsersTO.UserId = tblQualityPhaseDtlsTO.CreatedBy;
                                tblAlertUsersTO.AlertDefinitionId = tblAlertInstanceTO.AlertDefinitionId;
                                tblAlertUsersTO.RaisedOn = createdDate;
                                tblAlertInstanceTO.AlertUsersTOList.Add(tblAlertUsersTO);

                              
                                notify.SendNotificationToUsers(tblAlertInstanceTO);
                            }
                            #endregion

                        }
                        else
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While InsertTblQualityPhase";
                            return resultMessage;
                        }

                    }
                    tran.Commit();
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Text = resultMessage.DisplayMessage = "Record Saved Sucessfully";
                    resultMessage.Result = 1;
                    return resultMessage;

                }
                return ResultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Tag = ex;
                resultMessage.Text = "Exception Error in Method DeactivateIfAlreadyExists";
                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }

        }

        private  ResultMessage DeactivateIfAlreadyExistsAndInsertNew(TblQualityPhaseTO tblQualityPhaseTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            DateTime serverDateTime =  _iCommonDAO.ServerDateTime;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                if (tblQualityPhaseTO != null)
                {
                    int result = 0;
                    int result2 = 0;
                    result = _iTblQualityPhaseDAO.SelectFromDtlsAndQuality(tblQualityPhaseTO.PurchaseScheduleSummaryId, tblQualityPhaseTO.VehiclePhaseId, tblQualityPhaseTO.FlagTypeId, conn, tran);
                    if (result > 0)
                    {
                        tblQualityPhaseTO.IsActive = 0;
                        UpdateTblQualityPhase(tblQualityPhaseTO, conn, tran);
                    }
                    else
                    {
                        int res = InsertTblQualityPhase(tblQualityPhaseTO, conn, tran);
                        if (res > 0)
                        {
                            List<TblQualityPhaseDtlsTO> dtlsList = tblQualityPhaseTO.QualityPhaseDtlsTOList;
                            for (int i = 0; i < dtlsList.Count; i++)
                            {
                                if (dtlsList[i].IsSelected == 1)
                                {
                                    dtlsList[i].TblQualityPhaseId = tblQualityPhaseTO.IdTblQualityPhase;
                                    dtlsList[i].FlagStatusId = tblQualityPhaseTO.FlagStatusId;
                                    dtlsList[i].CreatedOn = tblQualityPhaseTO.CreatedOn;
                                    dtlsList[i].UpdatedOn = tblQualityPhaseTO.UpdatedOn;
                                    result2 = _iTblQualityPhaseDtlsBL.InsertTblQualityPhaseDtls(dtlsList[i], conn, tran);
                                }
                            }
                        }
                        else
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While InsertTblQualityPhase";
                            return resultMessage;
                        }
                    }

                }

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = resultMessage.DisplayMessage = "Record Saved Sucessfully";
                resultMessage.Result = 1;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Tag = ex;
                resultMessage.Text = "Exception Error in Method DeactivateIfAlreadyExists";
                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

       public  List<DropDownTO> GetFlagType()
        {
            return _iTblQualityPhaseDAO.GetFlagType();
        }

        #endregion

    }
}
