using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.BL;
using Newtonsoft.Json.Linq;
using SalesTrackerAPI.StaticStuff;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SalesTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class WeighingController : Controller
    {

        #region GET

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("GetAllWeighingMachines")]
        [HttpGet]
        public List<TblWeighingMachineTO> GetAllWeighingMachines()
        {
            List<TblWeighingMachineTO> list = BL.TblWeighingMachineBL.SelectAllTblWeighingMachineList();
            return list;
        }

        [Route("GetWeighingMachinesDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetWeighingMachinesDropDownList()
        {
            List<DropDownTO> list = BL.TblWeighingMachineBL.SelectTblWeighingMachineDropDownList();
            return list;
        }

        [Route("GetAllWeighingMeasuresByLoadingId")]
        [HttpGet]
        public List<TblWeighingMeasuresTO> GetAllWeighingMeasuresByLoadingId(int loadingId)
        {
            List<TblWeighingMeasuresTO> list = BL.TblWeighingMeasuresBL.SelectAllTblWeighingMeasuresListByLoadingId(loadingId);
            return list;
        }

        [Route("GetAllWeighingMeasuresByLoadingIds")]
        [HttpGet]
        public List<TblWeighingMeasuresTO> GetAllWeighingMeasuresByLoadingId(string loadingIds, Boolean isUnloading)
        {
            List<TblWeighingMeasuresTO> list = BL.TblWeighingMeasuresBL.SelectAllTblWeighingMeasuresListByLoadingId(loadingIds, isUnloading);
            return list;
        }

        [Route("GetAllWeighingMeasuresByUnLoadingId")]
        [HttpGet]
        public List<TblWeighingMeasuresTO> GetAllWeighingMeasuresByUnLoadingId(string unLoadingId)
        {
            List<TblWeighingMeasuresTO> list = BL.TblWeighingMeasuresBL.SelectAllTblWeighingMeasuresListByLoadingId(unLoadingId, true);
            return list;
        }

        [Route("GetAllWeighingMeasuresByVehicleNo")]
        [HttpGet]
        public List<TblWeighingMeasuresTO> GetAllWeighingMeasuresByVehicleNo(string vehicleNo)
        {
            List<TblWeighingMeasuresTO> list = BL.TblWeighingMeasuresBL.SelectAllTblWeighingMeasuresListByVehicleNo(vehicleNo);
            return list;
        }

        [Route("GetLatestWeightByMachineIp")]
        [HttpGet]
        public TblWeighingTO GetLatestWeightByMachineIp(string ipAddr)
        {
            return BL.TblWeighingBL.SelectTblWeighingByMachineIp(ipAddr);
        }


        [Route("GetLatestCalibrationValByMachineId")]
        [HttpGet]
        public TblMachineCalibrationTO GetLatestCalibrationValByMachineId(int weighingMachineId)
        {
            TblMachineCalibrationTO tblMachineCalibrationTO = BL.TblMachineCalibrationBL.SelectTblMachineCalibrationTOByWeighingMachineId(weighingMachineId);
            return tblMachineCalibrationTO;
        }

        #endregion

        #region POST


        [Route("PostNewWeighingMachine")]
        [HttpPost]
        public ResultMessage PostNewWeighingMachine([FromBody] JObject data)
        {
            ResultMessage resMsg = new ResultMessage();
            try
            {
                TblWeighingMachineTO weighingMachineTO = JsonConvert.DeserializeObject<TblWeighingMachineTO>(data["weighingMachineTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (weighingMachineTO == null)
                {
                    resMsg.DefaultBehaviour();
                    resMsg.Text = "weighingMachineTO found null";
                    resMsg.DisplayMessage = Constants.DefaultErrorMsg;
                    return resMsg;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resMsg.DefaultBehaviour();
                    resMsg.Text = "loginUserId found null";
                    resMsg.DisplayMessage = Constants.DefaultErrorMsg;
                    return resMsg;
                }

                DateTime serverDate = Constants.ServerDateTime;
                Int32 userId = Convert.ToInt32(loginUserId);
                weighingMachineTO.CreatedBy = userId;
                weighingMachineTO.CreatedOn = serverDate;
                int result = BL.TblWeighingMachineBL.InsertTblWeighingMachine(weighingMachineTO);
                if (result == 1)
                {
                    resMsg.DefaultSuccessBehaviour();
                    //resMsg.MessageType = ResultMessageE.Information;
                    //resMsg.DisplayMessage = Constants.DefaultSuccessMsg;
                    //resMsg.Text = Constants.DefaultSuccessMsg;
                    //resMsg.Result = 1;
                }
                else
                {
                    resMsg.DefaultBehaviour();
                    resMsg.DisplayMessage = Constants.DefaultErrorMsg;
                    resMsg.Text = "Error While InsertTblWeighingMachine";
                    resMsg.Result = 0;
                }
                return resMsg;
            }
            catch (Exception ex)
            {
                resMsg.MessageType = ResultMessageE.Error;
                resMsg.Result = -1;
                resMsg.Exception = ex;
                resMsg.Text = "Exception Error IN API Call PostNewWeighingMachine :" + ex;
                resMsg.DisplayMessage = Constants.DefaultErrorMsg;
                return resMsg;
            }
        }

        [Route("PostUpdateWeighingMachine")]
        [HttpPost]
        public ResultMessage PostUpdateWeighingMachine([FromBody] JObject data)
        {
            ResultMessage resMsg = new ResultMessage();
            try
            {
                TblWeighingMachineTO weighingMachineTO = JsonConvert.DeserializeObject<TblWeighingMachineTO>(data["weighingMachineTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (weighingMachineTO == null)
                {
                    resMsg.DefaultBehaviour();
                    resMsg.Text = "weighingMachineTO found null";
                    resMsg.DisplayMessage = Constants.DefaultErrorMsg;
                    return resMsg;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resMsg.DefaultBehaviour();
                    resMsg.Text = "loginUserId found null";
                    resMsg.DisplayMessage = Constants.DefaultErrorMsg;
                    return resMsg;
                }

                DateTime serverDate = Constants.ServerDateTime;
                Int32 userId = Convert.ToInt32(loginUserId);
                weighingMachineTO.UpdatedBy = userId;
                weighingMachineTO.UpdatedOn = serverDate;
                int result = BL.TblWeighingMachineBL.UpdateTblWeighingMachine(weighingMachineTO);
                if (result == 1)
                {
                    resMsg.MessageType = ResultMessageE.Information;
                    resMsg.DisplayMessage = Constants.DefaultSuccessMsg;
                    resMsg.Text = Constants.DefaultSuccessMsg;
                    resMsg.Result = 1;
                }
                else
                {
                    resMsg.DefaultBehaviour();
                    resMsg.DisplayMessage = Constants.DefaultErrorMsg;
                    resMsg.Text = "Error While UpdateTblWeighingMachine";
                    resMsg.Result = 0;
                }
                return resMsg;
            }
            catch (Exception ex)
            {
                resMsg.MessageType = ResultMessageE.Error;
                resMsg.Result = -1;
                resMsg.Exception = ex;
                resMsg.Text = "Exception Error IN API Call PostUpdateWeighingMachine :" + ex;
                resMsg.DisplayMessage = Constants.DefaultErrorMsg;
                return resMsg;
            }
        }

        [Route("PostNewWeighingMeasurement")]
        [HttpPost]
        public ResultMessage PostNewWeighingMeasurement([FromBody] JObject data)
        {
            ResultMessage resMsg = new ResultMessage();
            try
            {
                List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = new List<TblLoadingSlipExtTO>();
                TblWeighingMeasuresTO tblWeighingMeasuresTO = new TblWeighingMeasuresTO();
                tblLoadingSlipExtTOList = JsonConvert.DeserializeObject<List<TblLoadingSlipExtTO>>(data["tblLoadingSlipExtTOList"].ToString());
                tblWeighingMeasuresTO = JsonConvert.DeserializeObject<TblWeighingMeasuresTO>(data["tblWeighingMeasuresTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                tblWeighingMeasuresTO.CreatedBy = Convert.ToInt32(loginUserId);
                tblWeighingMeasuresTO.CreatedOn = Constants.ServerDateTime;

                resMsg = BL.TblWeighingMeasuresBL.SaveNewWeighinMachineMeasurement(tblWeighingMeasuresTO, tblLoadingSlipExtTOList);
                resMsg.Tag = IoT.IotCommunication.GetDateToTimestap();
                resMsg.Tag = resMsg.Tag + "|" + tblWeighingMeasuresTO.IsLoadingCompleted;


                return resMsg;
            }
            catch (Exception ex)
            {
                resMsg.DefaultExceptionBehaviour(ex, "Exception Error IN API Call PostNewWeighingMeasurement :");
                return resMsg;
            }

        }

        [Route("PostIsAllowWeighingMachineToWt")]
        [HttpPost]
        public ResultMessage PostIsAllowWeighingMachineToWt([FromBody] JObject data)
        {
            ResultMessage resMsg = new ResultMessage();
            try
            {
                List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = new List<TblLoadingSlipExtTO>();
                TblLoadingSlipExtTO tblLoadingSlipExtTO = new TblLoadingSlipExtTO();
                tblLoadingSlipExtTO = JsonConvert.DeserializeObject<TblLoadingSlipExtTO>(data["tblLoadingSlipExtTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                tblLoadingSlipExtTO.UpdatedBy = Convert.ToInt32(loginUserId);
                tblLoadingSlipExtTO.UpdatedOn = Constants.ServerDateTime;

                resMsg = BL.TblWeighingMeasuresBL.UpdateLoadingSlipExtTo(tblLoadingSlipExtTO);

                return resMsg;
            }
            catch (Exception ex)
            {
                resMsg.MessageType = ResultMessageE.Error;
                resMsg.Result = -1;
                resMsg.Exception = ex;
                resMsg.Text = "Exception Error IN API Call PostNewWeighingMeasurement :" + ex;
                resMsg.DisplayMessage = Constants.DefaultErrorMsg;
                return resMsg;
            }
        }

        [HttpGet("RestoreOnDB")]
        public IActionResult RestoreOnDB()
        {
            int result = TblWeighingBL.FetchFromIotAndWrite();
            if (result == 1)
            {
                return Ok("Data store on cloud successfully");
            }
            return Ok("Failed Or Data Not Found..!!");
        }

        [HttpGet("RestoreToIOT")]
        public IActionResult RestoreToIOT()
        {
            ResultMessage r = TblWeighingBL.RestoreToIOT();

            return Ok(r);
        }


        [Route("PostIsAllowWeighingMachineToWtAll")]
        [HttpPost]
        public ResultMessage PostIsAllowWeighingMachineToWtAll([FromBody] JObject data)
        {
            ResultMessage resMsg = new ResultMessage();
            try
            {
                //List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = new List<TblLoadingSlipExtTO>();
                //TblLoadingSlipExtTO tblLoadingSlipExtTO = new TblLoadingSlipExtTO();
                //tblLoadingSlipExtTO = JsonConvert.DeserializeObject<TblLoadingSlipExtTO>(data["tblLoadingSlipExtTO"].ToString());
                var loginUserId = Convert.ToInt32(data["loginUserId"].ToString());
                var loadingId = Convert.ToInt32(data["loadingId"].ToString());

                //tblLoadingSlipExtTO.UpdatedBy = Convert.ToInt32(loginUserId);
                //tblLoadingSlipExtTO.UpdatedOn = Constants.ServerDateTime;

                resMsg = BL.TblWeighingMeasuresBL.MarkAllLoadingSlipExtIsAllowTareWeightAgainstLoading((Int32)loadingId, (Int32)loginUserId);

                return resMsg;
            }
            catch (Exception ex)
            {
                resMsg.MessageType = ResultMessageE.Error;
                resMsg.Result = -1;
                resMsg.Exception = ex;
                resMsg.Text = "Exception Error IN API Call PostNewWeighingMeasurement :" + ex;
                resMsg.DisplayMessage = Constants.DefaultErrorMsg;
                return resMsg;
            }
        }

        [Route("PostMachineCalibration")]
        [HttpPost]
        public ResultMessage PostMachineCalibration([FromBody] JObject data)
        {
            ResultMessage resMsg = new ResultMessage();
            int result = 0;
            try
            {
                List<TblMachineCalibrationTO> tblLoadingSlipExtTOList = new List<TblMachineCalibrationTO>();
                TblMachineCalibrationTO tblMachineCalibrationTO = new TblMachineCalibrationTO();
                tblMachineCalibrationTO = JsonConvert.DeserializeObject<TblMachineCalibrationTO>(data["tblMachineCalibrationTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                //tblLoadingSlipExtTO.UpdatedBy = Convert.ToInt32(loginUserId);
                //tblLoadingSlipExtTO.UpdatedOn = Constants.ServerDateTime;
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resMsg.DefaultBehaviour("loginUserId found null");
                    return resMsg;
                }
                if (tblMachineCalibrationTO == null)
                {
                    resMsg.DefaultBehaviour("tblMachineCalibrationTO found null");
                    return resMsg;
                }

                tblMachineCalibrationTO.CreatedBy = Convert.ToInt32(loginUserId);
                tblMachineCalibrationTO.CreatedOn = Constants.ServerDateTime;
                return BL.TblMachineCalibrationBL.InsertTblMachineCalibration(tblMachineCalibrationTO);
            }
            catch (Exception ex)
            {
                resMsg.DefaultExceptionBehaviour(ex, "PostMachineCalibration");
                return resMsg;
            }
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        #endregion

        #region PUT

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        #endregion

        #region DELETE

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        #endregion

    }
}
