using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PurchaseTrackerAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DashboardModels;
using System.Globalization;
using PurchaseTrackerAPI.DAL.Interfaces;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PurchaseTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class ConfigurationController : Controller
    {
        private readonly ITblRecyclePreferenceBL _iTblRecyclePreferenceBL;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        public ConfigurationController(ITblRecyclePreferenceBL iTblRecyclePreferenceBL,
            ITblConfigParamsBL iTblConfigParamsBL)
        {
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iTblRecyclePreferenceBL = iTblRecyclePreferenceBL;

        }
        [Route("GetSystemConfigValue")]
        [HttpGet]
        public TblRecyclePreferenceTO GetSystemConfigValue(string settingKey)
        {
            return _iTblRecyclePreferenceBL.SelectTblRecyclePreferenceValByName(settingKey);
        }

        [Route("GetSystemConfigParamValByName")]
        [HttpGet]
        public TblConfigParamsTO GetSystemConfigParamValByName(string configParamName)
        {
            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName(configParamName);
            return tblConfigParamsTO;
        }

        
        [Route("GetAllSystemConfigList")]
        [HttpGet]
        public List<TblConfigParamsTO> GetAllSystemConfigList()
        {
            List<TblConfigParamsTO> tblConfigParamsTOList = _iTblConfigParamsBL.SelectAllTblConfigParamsList();
            return tblConfigParamsTOList;
        }

        [Route("SaveBirimMachineQty")]
        [HttpPost]
        public ResultMessage SaveBirimMachineQty([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                double birimMachineQty = Convert.ToDouble(data["birimMachineQty"].ToString());
                Int32 loginUserId = Convert.ToInt32(data["loginUserId"].ToString());

                if (loginUserId <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : Login User ID Found NULL";
                    return resultMessage;
                }

                return  _iTblConfigParamsBL.SaveBirimMachineQty(birimMachineQty, loginUserId);

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in SaveBirimMachineQty(birimMachineQty, loginUserId);");
                return resultMessage;
            }
        }

    }

}
