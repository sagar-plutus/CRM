using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SalesTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class LoadingSlipController : Controller
    {

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST api/values
        [Route("PostNewLoadingSlip")]
        [HttpPost]
        public int PostNewLoadingSlip([FromBody] JObject data)
        {
            try
            {

                TblLoadingTO tblLoadingSlipTO = JsonConvert.DeserializeObject<TblLoadingTO>(data["loadingSlipTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (tblLoadingSlipTO == null)
                {
                    return 0;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    return 0;
                }

                if (tblLoadingSlipTO.LoadingSlipList == null || tblLoadingSlipTO.LoadingSlipList.Count == 0)
                {
                    return 0;
                }              

                tblLoadingSlipTO.CreatedBy = Convert.ToInt32(loginUserId);
                tblLoadingSlipTO.TranStatusE = Constants.TranStatusE.LOADING_NEW;
                tblLoadingSlipTO.StatusDate = Constants.ServerDateTime;
                tblLoadingSlipTO.CreatedOn = Constants.ServerDateTime;
                tblLoadingSlipTO.StatusReason = "New - Considered For Loading";

                ResultMessage rMessage = new ResultMessage();
                rMessage = BL.TblLoadingBL.SaveNewLoadingSlip(tblLoadingSlipTO);
                if (rMessage.MessageType != ResultMessageE.Information)
                {
                    return 0;
                }
                else
                {
                    // loggerObj.LogInformation("Sucess");
                    return 1;
                }
            }
            catch (Exception ex)
            {
                //loggerObj.LogError(1, ex, "Exception Error in POstNewBooking", data);
                return -1;
            }
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


        [Route("GetTblGateList")]
        [HttpGet]
        public List<TblGateTO> GetTblGateList(TblLoadingTO tblloading)
        {
            //   int orgTypeId = (int)Constants.OrgTypeE.DEALER;
            try
            {
                List<TblGateTO> list = BL.TblLoadingBL.GetAllTblGate(tblloading);
                return list;
            }
        catch(Exception ex)
            {
                return null;
            }
            return null;
        }
         [Route("GetAllTblGateList")]
        [HttpGet]
        public List<TblGateTO> GetAllTblGateList()
        {
           
            try
            {
                List<TblGateTO> list = BL.TblLoadingBL.GetAllTblGate();
                return list;
            }
        catch(Exception ex)
            {
                return null;
            }
            return null;
        }


        [Route("GetEmptySizeAndProductListForLoadingAgainstSch")]
        [HttpGet]
        public List<TblBookingScheduleTO> GetEmptySizeAndProductListForLoadingAgainstSch(Int32 prodCatId, Int32 prodSpecId, Int32 boookingId, Int32 brandId)
        {
            List<TblBookingScheduleTO> list = BL.TblLoadingSlipExtBL.SelectEmptyLoadingSlipExtListAgainstSch(prodCatId, prodSpecId, boookingId, brandId);
            return list;
        }

    }
}
