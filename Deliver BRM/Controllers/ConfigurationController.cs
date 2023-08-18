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
    public class ConfigurationController : Controller
    {
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

        [Route("GetAllSystemConfigList")]
        [HttpGet]
        public List<TblConfigParamsTO> GetAllSystemConfigList()
        {
            List<TblConfigParamsTO> list = BL.TblConfigParamsBL.SelectAllTblConfigParamsList();
            return list;
        }

        /// <summary>
        /// GJ@20170810 : Get the Configuration value by Name 
        /// </summary>
        /// <param name="configParamName"></param>
        /// <returns></returns>
        [Route("GetSystemConfigParamValByName")]
        [HttpGet]
        public TblConfigParamsTO GetSystemConfigParamValByName(string configParamName)
        {
            TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsValByName(configParamName);
            return tblConfigParamsTO;
        }

        [Route("PostConfigurationUpdate")]
        [HttpPost]
        public ResultMessage PostConfigurationUpdate([FromBody] JObject data)
        {
            ResultMessage resMsg = new ResultMessage();
            try
            {
                TblConfigParamsTO configParamsTO = JsonConvert.DeserializeObject<TblConfigParamsTO>(data["configParamsTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (configParamsTO == null)
                {
                    resMsg.DefaultBehaviour();
                    resMsg.Text = "configParamsTO found null";
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

                return BL.TblConfigParamsBL.UpdateConfigParamsWithHistory(configParamsTO, Convert.ToInt32(loginUserId));
            }
            catch (Exception ex)
            {
                resMsg.MessageType = ResultMessageE.Error;
                resMsg.Result = -1;
                resMsg.Exception = ex;
                resMsg.Text = "Exception Error IN API Call PostConfigurationUpdate :" + ex;
                resMsg.DisplayMessage = Constants.DefaultErrorMsg;
                return resMsg;
            }
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
