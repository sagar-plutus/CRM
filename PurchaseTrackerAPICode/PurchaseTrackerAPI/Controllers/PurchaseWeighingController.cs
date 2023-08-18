using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;

using System.Text;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class PurchaseWeighingController : Controller
    {
        private readonly Icommondao _iCommonDAO;
        private readonly ITblPurchaseWeighingStageSummaryBL _iTblPurchaseWeighingStageSummaryBL;
        private readonly ITblPurchaseUnloadingDtlBL _iTblPurchaseUnloadingDtlBL;
        private readonly ITblPurchaseGradingDtlsBL _iTblPurchaseGradingDtlsBL;
        private readonly ITblWeighingMachineBL _iTblWeighingMachineBL;
        private readonly ITblWeighingBL _iTblWeighingBL;
        private readonly ITblMachineCalibrationBL _iTblMachineCalibrationBL;
        private readonly ITblRecycleDocumentBL _iTblRecycleDocumentBL;

        private readonly ITblPurchaseScheduleSummaryBL _iTblPurchaseScheduleSummaryBL;

        public PurchaseWeighingController(ITblPurchaseWeighingStageSummaryBL iTblPurchaseWeighingStageSummaryBL
            , Icommondao icommondao
           , ITblPurchaseUnloadingDtlBL iTblPurchaseUnloadingDtlBL, ITblPurchaseGradingDtlsBL iTblPurchaseGradingDtlsBL
           , ITblWeighingMachineBL iTblWeighingMachineBL, ITblWeighingBL iTblWeighingBL
           , ITblMachineCalibrationBL iTblMachineCalibrationBL
           , ITblRecycleDocumentBL iTblRecycleDocumentBL
           , ITblPurchaseScheduleSummaryBL iTblPurchaseScheduleSummaryBL)
        {
            _iCommonDAO = icommondao;
            _iTblRecycleDocumentBL = iTblRecycleDocumentBL;
            _iTblMachineCalibrationBL = iTblMachineCalibrationBL;
            _iTblWeighingBL = iTblWeighingBL;
            _iTblWeighingMachineBL = iTblWeighingMachineBL;
            _iTblPurchaseUnloadingDtlBL = iTblPurchaseUnloadingDtlBL;
            _iTblPurchaseGradingDtlsBL = iTblPurchaseGradingDtlsBL;
            _iTblPurchaseWeighingStageSummaryBL = iTblPurchaseWeighingStageSummaryBL;
            _iTblPurchaseScheduleSummaryBL = iTblPurchaseScheduleSummaryBL;
        }

        #region Get

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("UnlodingTimeRport")]
        [HttpGet]
        public List<UnloadedTimeReportTO> UnlodingTimeRport(string fromDate, string toDate, Int32 isForWeighingPointWise,String purchaseManagerIds)
        {
            return _iTblPurchaseWeighingStageSummaryBL.UnlodingTimeRport(fromDate, toDate, isForWeighingPointWise, false, purchaseManagerIds);
        }

        [Route("GetVehicleWeighingDetailsBySchduleId")]
        [HttpGet]
        public List<TblPurchaseWeighingStageSummaryTO> GetVehicleWeighingDetailsBySchduleId(Int32 purchaseScheduleId)
        {
            bool fromWeighing = true;
            List<TblPurchaseWeighingStageSummaryTO> tblPurchaseWeighingStageSummaryTO = _iTblPurchaseWeighingStageSummaryBL.GetVehicleWeighingDetailsBySchduleId(purchaseScheduleId, fromWeighing);
            return tblPurchaseWeighingStageSummaryTO;
        }

        //Added by minal for weighing Report (Display Unloading Supervisor in report)
        [Route("GetVehicleWeighingDetailsBySchduleIdForWeighingReport")]
        [HttpGet]
        public List<TblPurchaseWeighingStageSummaryTO> GetVehicleWeighingDetailsBySchduleIdForWeighingReport(Int32 purchaseScheduleId)
        {
            bool fromWeighing = true;
            List<TblPurchaseWeighingStageSummaryTO> tblPurchaseWeighingStageSummaryTO = _iTblPurchaseWeighingStageSummaryBL.GetVehicleWeighingDetailsBySchduleIdForWeighingReport(purchaseScheduleId, fromWeighing);
            return tblPurchaseWeighingStageSummaryTO;
        }


        [Route("GetVehicleTareWeightDetails")]
        [HttpGet]
        public List<TblPurchaseWeighingStageSummaryTO> GetVehicleTareWeightDetails(Int32 purchaseScheduleId, string weightTypeId)
        {
            List<TblPurchaseWeighingStageSummaryTO> tblPurchaseWeighingStageSummaryTOList = _iTblPurchaseWeighingStageSummaryBL.GetVehicleWeightDetails(purchaseScheduleId, weightTypeId);
            return tblPurchaseWeighingStageSummaryTOList;
        }
        [Route("GetLatestWeightByMachineIp")]
        [HttpGet]
        public TblWeighingTO GetLatestWeightByMachineIp(string ipAddr, int machineId = 0)
        {
            return _iTblWeighingBL.SelectTblWeighingByMachineIp(ipAddr, machineId);
        }

        [Route("GetMachineDtlsByMachineIp")]
        [HttpGet]
        public TblWeighingMachineTO GetMachineDtlsByMachineIp(string ipAddr)
        {
            return _iTblWeighingMachineBL.SelectTblWeighingMachineTO(ipAddr);
        }


        [Route("GetAllWeighingMachines")]
        [HttpGet]
        public List<TblWeighingMachineTO> GetAllWeighingMachines()
        {
            List<TblWeighingMachineTO> list = _iTblWeighingMachineBL.SelectAllTblWeighingMachineList();
            return list;
        }

        // [Route("GetVehicleWeightAndUnloadingMaterialDetails")]
        // [HttpGet]
        // public List<TblPurchaseWeighingStageSummaryTO> GetVehicleWeightAndUnloadingMaterialDetails(Int32 purchaseScheduleId)
        // {
        //     List<TblPurchaseWeighingStageSummaryTO> tblPurchaseWeighingStageSummaryTOList = _iTblPurchaseWeighingStageSummaryBL.GetVehicleWeighingDetailsBySchduleId(purchaseScheduleId);
        //     if (tblPurchaseWeighingStageSummaryTOList != null && tblPurchaseWeighingStageSummaryTOList.Count > 0)
        //     {
        //         for (int i = 0; i < tblPurchaseWeighingStageSummaryTOList.Count; i++)
        //         {
        //             List<TblPurchaseUnloadingDtlTO> tblPurchaseUnloadingDtlTOList = BL.TblPurchaseUnloadingDtlBL.SelectAllTblPurchaseUnloadingDtlList(tblPurchaseWeighingStageSummaryTOList[i].IdPurchaseWeighingStage);
        //             if (tblPurchaseUnloadingDtlTOList != null && tblPurchaseUnloadingDtlTOList.Count > 0)
        //             {
        //                 tblPurchaseWeighingStageSummaryTOList[i].PurchaseUnloadingDtlTOList = new List<TblPurchaseUnloadingDtlTO>();
        //                 tblPurchaseWeighingStageSummaryTOList[i].PurchaseUnloadingDtlTOList = tblPurchaseUnloadingDtlTOList;
        //             }

        //             List<TblPurchaseGradingDtlsTO> tblPurchaseGradingDtlsTOList = BL.TblPurchaseGradingDtlsBL.SelectTblPurchaseGradingDtlsTOListByWeighingId(tblPurchaseWeighingStageSummaryTOList[i].IdPurchaseWeighingStage);
        //             if (tblPurchaseGradingDtlsTOList != null && tblPurchaseGradingDtlsTOList.Count > 0)
        //             {
        //                 tblPurchaseWeighingStageSummaryTOList[i].PurchaseGradingDtlsTOList = new List<TblPurchaseGradingDtlsTO>();
        //                 tblPurchaseWeighingStageSummaryTOList[i].PurchaseGradingDtlsTOList = tblPurchaseGradingDtlsTOList;
        //             }


        //         }


        //     }
        //     return tblPurchaseWeighingStageSummaryTOList;
        // }
        [Route("GetVehicleWeightAndUnloadingMaterialDetails")]
        [HttpGet]
        public List<TblPurchaseWeighingStageSummaryTO> GetVehicleWeightAndUnloadingMaterialDetails(Int32 purchaseScheduleId, Int32 formTypeE)
        {
            return _iTblPurchaseWeighingStageSummaryBL.GetVehicleWeightAndUnloadingMaterialDetails(purchaseScheduleId, formTypeE);
        }
        //chetan[28-feb-2020] added 
        [Route("GetVehicleWeightAndUnloadingMaterialDetailsTOList")]
        [HttpGet]
        public List<TblPurchaseWeighingStageSummaryTO> GetVehicleWeightAndUnloadingMaterialDetailsTOList(Int32 purchaseScheduleId)
        {
            return _iTblPurchaseWeighingStageSummaryBL.GetVehicleWeightAndUnloadingMaterialDetailsTOList(purchaseScheduleId);
        }
        [Route("GradeInfoTOListForSudharReport")]
        [HttpGet]
        public List<GradeInfoTO> GradeInfoTOListForSudharReport(Int32 purchaseScheduleId)
        {
            return _iTblPurchaseWeighingStageSummaryBL.GradeInfoTOListForSudharReport(purchaseScheduleId);
        }

        [Route("GetWeighingMachinesDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetWeighingMachinesDropDownList()
        {
            List<DropDownTO> list = _iTblWeighingMachineBL.SelectTblWeighingMachineDropDownList();
            return list;
        }

        [Route("GetLatestCalibrationValByMachineId")]
        [HttpGet]
        public TblMachineCalibrationTO GetLatestCalibrationValByMachineId(int weighingMachineId)
        {
            TblMachineCalibrationTO tblMachineCalibrationTO = _iTblMachineCalibrationBL.SelectTblMachineCalibrationTOByWeighingMachineId(weighingMachineId);
            return tblMachineCalibrationTO;
        }
        /// <summary>
        /// Priyanka [22-03-2019] : Added to get the unloading stage wise record from purchaseWeighingStageId.
        /// </summary>
        /// <param name="purchaseWeighingStageId"></param>
        /// <returns></returns>
        [Route("GetAllTblPurchaseUnloadingDtlList")]
        [HttpGet]
        public List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtlList(Int32 purchaseWeighingStageId)
        {
            return _iTblPurchaseUnloadingDtlBL.SelectAllTblPurchaseUnloadingDtlList(purchaseWeighingStageId);
        }
        #endregion

        #region Post

        [Route("PostUpdateRecoveryDtls")]
        [HttpPost]
        public ResultMessage PostUpdateRecoveryDtls([FromBody] TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                if (tblPurchaseWeighingStageSummaryTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.DisplayMessage = "API : tblPurchaseWeighingStageSummaryTO Found NULL";
                    return resultMessage;
                }

                if (tblPurchaseWeighingStageSummaryTO.RecoveryPer < 1)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.DisplayMessage = "Recovery percentage is not valid.";
                    return resultMessage;
                }
                else if (tblPurchaseWeighingStageSummaryTO.RecoveryPer > 100)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.DisplayMessage = "Recovery percentage is greater than 100";
                    return resultMessage;
                }

                // if (tblPurchaseWeighingStageSummaryTO.RecoveryPer < 0)
                // {
                //     resultMessage.DefaultBehaviour();
                //     resultMessage.DisplayMessage = "Recovery percentage is less than zero";
                //     return resultMessage;
                // }
                // else if (tblPurchaseWeighingStageSummaryTO.RecoveryPer == 0)
                // {
                //     resultMessage.DefaultBehaviour();
                //     resultMessage.DisplayMessage = "Recovery percentage is equal to zero";
                //     return resultMessage;
                // }
                // else if (tblPurchaseWeighingStageSummaryTO.RecoveryPer > 100)
                // {
                //     resultMessage.DefaultBehaviour();
                //     resultMessage.DisplayMessage = "Recovery percentage is greater than 100";
                //     return resultMessage;
                // }


                resultMessage = _iTblPurchaseWeighingStageSummaryBL.UpdateTblPurchaseWeighingStageSummaryRecoveryDtls(tblPurchaseWeighingStageSummaryTO);
                // if (resultMessage.MessageType != ResultMessageE.Information)
                // {
                //     resultMessage.DefaultBehaviour();
                // }
                // else
                // {
                //     resultMessage.DefaultSuccessBehaviour();
                // }

                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "API : Exception Error In Method PostVehicleWeighingDetails";
                return resultMessage;
            }
        }
        [Route("UpdateUnlodingStartTime")]
        [HttpPost]
        public ResultMessage UpdateUnlodingStartTime([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO = JsonConvert.DeserializeObject<TblPurchaseWeighingStageSummaryTO>(data["purchaseWeighingStageSummaryTO"].ToString());
                tblPurchaseWeighingStageSummaryTO.GradingStartTime = _iCommonDAO.ServerDateTime;
                int result = _iTblPurchaseWeighingStageSummaryBL.UpdateUnlodingStartTime(tblPurchaseWeighingStageSummaryTO);
                if(result ==0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "API : Exception Error In Method UpdateUnlodingStartTime";
                    return resultMessage;
                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Result = 1;
                    resultMessage.Text = "Update Unloding time sucessfully";
                    return resultMessage;
                }
            }
            catch(Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = 0;
                resultMessage.Text = "API : Exception Error In Method PostVehicleWeighingDetails";
                return resultMessage;
            }
            return resultMessage;
            }
        [Route("PostVehicleWeighingDetails")]
        [HttpPost]
        public ResultMessage PostVehicleWeighingDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO = JsonConvert.DeserializeObject<TblPurchaseWeighingStageSummaryTO>(data["PurchaseWeighingStageSummaryTO"].ToString());
                Int32 loginUserId = Convert.ToInt32(data["loginUserId"].ToString());

                if (tblPurchaseWeighingStageSummaryTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : tblPurchaseWeighingStageSummaryTO Found NULL";
                    return resultMessage;
                }

                DateTime createdDate =  _iCommonDAO.ServerDateTime;
                tblPurchaseWeighingStageSummaryTO.CreatedOn = createdDate;
                tblPurchaseWeighingStageSummaryTO.CreatedBy = Convert.ToInt32(loginUserId);
                tblPurchaseWeighingStageSummaryTO.UpdatedOn = createdDate;
                tblPurchaseWeighingStageSummaryTO.UpdatedBy = Convert.ToInt32(loginUserId);

                return resultMessage = _iTblPurchaseScheduleSummaryBL.InsertWeighingDetails(tblPurchaseWeighingStageSummaryTO);

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "API : Exception Error In Method PostVehicleWeighingDetails";
                return resultMessage;
            }
        }

        [Route("PostUnloadingMaterialDetails")]
        [HttpPost]
        public ResultMessage PostUnloadingMaterialDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                List<TblPurchaseUnloadingDtlTO> tblPurchaseUnloadingDtlTOList = JsonConvert.DeserializeObject<List<TblPurchaseUnloadingDtlTO>>(data["PurchaseUnloadingDtlTOList"].ToString());
                //Int32 loginUserId = Convert.ToInt32(data["loginUserId"].ToString());
                Boolean isSendNotification = Convert.ToBoolean(data["isSendNotification"].ToString());
                Boolean isDeletePrevious = Convert.ToBoolean(data["isDeletePrevious"].ToString());
                

                if (tblPurchaseUnloadingDtlTOList == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : tblPurchaseUnloadingDtlTOList Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                resultMessage = _iTblPurchaseUnloadingDtlBL.SaveUnloadingMaterialDetails(tblPurchaseUnloadingDtlTOList, isSendNotification,isDeletePrevious);
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "API : Exception Error In Method PostUnloadingMaterialDetails";
                return resultMessage;
            }
        }


        [Route("PostGradingMaterialDetails")]
        [HttpPost]
        public ResultMessage PostGradingMaterialDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                List<TblPurchaseGradingDtlsTO> tblPurchaseGradingDtlsTOList = JsonConvert.DeserializeObject<List<TblPurchaseGradingDtlsTO>>(data["PurchaseGradingDtlsTOList"].ToString());
                Int32 loginUserId = Convert.ToInt32(data["loginUserId"].ToString());

                if (tblPurchaseGradingDtlsTOList == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : tblPurchaseGradingDtlsTOList Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                resultMessage = _iTblPurchaseGradingDtlsBL.SaveGradingMaterialDetails(tblPurchaseGradingDtlsTOList, loginUserId);
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "API : Exception Error In Method PostGradingMaterialDetails";
                return resultMessage;
            }
        }

        [Route("DeleteUnloadingDetails")]
        [HttpPost]
        public ResultMessage DeleteUnloadingDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                List<TblPurchaseUnloadingDtlTO> tblPurchaseUnloadingDtlTOList = JsonConvert.DeserializeObject<List<TblPurchaseUnloadingDtlTO>>(data["PurchaseUnloadingDtlTOList"].ToString());

                if (tblPurchaseUnloadingDtlTOList == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : tblPurchaseUnloadingDtlTOList Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                resultMessage = _iTblPurchaseUnloadingDtlBL.DeleteUnloadingDetails(tblPurchaseUnloadingDtlTOList);
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "API : Exception Error In Method DeleteUnloadingDetails";
                return resultMessage;
            }
        }
        [Route("DeleteGradingDetails")]
        [HttpPost]
        public ResultMessage DeleteGradingDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                List<TblPurchaseGradingDtlsTO> tblPurchaseGradingDtlsTOList = JsonConvert.DeserializeObject<List<TblPurchaseGradingDtlsTO>>(data["PurchaseGradingDtlsTOList"].ToString());

                if (tblPurchaseGradingDtlsTOList == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : tblPurchaseGradingDtlsTOList Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                resultMessage = _iTblPurchaseGradingDtlsBL.DeleteGradingDetails(tblPurchaseGradingDtlsTOList);
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "API : Exception Error In Method DeleteGradingDetails";
                return resultMessage;
            }
        }

        [Route("PostUploadedImages")]
        [HttpPost]
        public ResultMessage PostUploadedImages([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                List<TblRecycleDocumentTO> tblRecycleDocumentTOList = JsonConvert.DeserializeObject<List<TblRecycleDocumentTO>>(data["RecycleDocumentTOList"].ToString());
                Int32 loginUserId = Convert.ToInt32(data["loginUserId"].ToString());
                if (tblRecycleDocumentTOList == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : PostUploadedImages Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                resultMessage = _iTblRecycleDocumentBL.PostUploadedImages(tblRecycleDocumentTOList, loginUserId);
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "API : Exception Error In Method DeleteUnloadingDetails";
                return resultMessage;
            }
        }


        [Route("PostUpdateRecoveryEngineerId")]
        [HttpPost]
        public ResultMessage PostUpdateRecoveryEngineerId([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                Int32 loginUserId = Convert.ToInt32(data["loginUserId"].ToString());
                Int32 PurchaseScheduleSummaryId = Convert.ToInt32(data["PurchaseScheduleSummaryId"].ToString());

                resultMessage = _iTblPurchaseWeighingStageSummaryBL.UpdateRecoveryEngineerId(loginUserId, PurchaseScheduleSummaryId);
                if (resultMessage.MessageType != ResultMessageE.Information)
                {
                    resultMessage.DefaultBehaviour();
                }
                else
                {
                    resultMessage.DefaultSuccessBehaviour();
                }

                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "API : Exception Error In Method PostVehicleWeighingDetails";
                return resultMessage;
            }
        }


        [Route("PostUpdateGraderId")]
        [HttpPost]
        public ResultMessage PostUpdateGraderId([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                Int32 loginUserId = Convert.ToInt32(data["loginUserId"].ToString());
                Int32 PurchaseScheduleSummaryId = Convert.ToInt32(data["PurchaseScheduleSummaryId"].ToString());

                resultMessage = _iTblPurchaseWeighingStageSummaryBL.UpdateGraderId(loginUserId, PurchaseScheduleSummaryId);
                if (resultMessage.MessageType != ResultMessageE.Information)
                {
                    resultMessage.DefaultBehaviour();
                }
                else
                {
                    resultMessage.DefaultSuccessBehaviour();
                }

                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "API : Exception Error In Method PostVehicleWeighingDetails";
                return resultMessage;
            }
        }

        [Route("PostUpdatePhotographerId")]
        [HttpPost]
        public ResultMessage PostUpdatePhotographerId([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                Int32 loginUserId = Convert.ToInt32(data["loginUserId"].ToString());
                Int32 PurchaseScheduleSummaryId = Convert.ToInt32(data["PurchaseScheduleSummaryId"].ToString());

                resultMessage = _iTblPurchaseWeighingStageSummaryBL.PostUpdatePhotographerId(loginUserId, PurchaseScheduleSummaryId);
                if (resultMessage.MessageType != ResultMessageE.Information)
                {
                    resultMessage.DefaultBehaviour();
                }
                else
                {
                    resultMessage.DefaultSuccessBehaviour();
                }

                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "API : Exception Error In Method PostVehicleWeighingDetails";
                return resultMessage;
            }
        }


        [Route("PostUpdateSupervisorId")]
        [HttpPost]
        public ResultMessage PostUpdateSupervisorId([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                //Int32 supervisorId = Convert.ToInt32(data["supervisorId"].ToString());
                //Int32 purchaseScheduleSummaryId = Convert.ToInt32(data["PurchaseScheduleSummaryId"].ToString());

                Int32 loginUserId = Convert.ToInt32(data["loginUserId"].ToString());
                TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO = JsonConvert.DeserializeObject<TblPurchaseScheduleSummaryTO>(data["purchaseScheduleSummaryTO"].ToString());

                resultMessage = _iTblPurchaseScheduleSummaryBL.UpdateSupervisorId(loginUserId, tblPurchaseScheduleSummaryTO);
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in PostUpdateSupervisorId()");
                return resultMessage;
            }
        }


        #endregion


        [HttpGet("RestoreOnDB")]
        public IActionResult RestoreOnDB()
        {
            int result = _iTblWeighingBL.FetchFromIotAndWriteDB();
            if (result == 1)
            {
                return Ok("Data store on cloud successfully");
            }
            return Ok("Failed Or Data Not Found..!!");
        }

        [HttpGet("RestoreToIOT")]
        public IActionResult RestoreToIOT()
        {
            ResultMessage r = _iTblWeighingBL.RestoreToIOT();

            return Ok(r);
        }
    }
}
