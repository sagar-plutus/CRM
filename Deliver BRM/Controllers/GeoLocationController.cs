using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SalesTrackerAPI.Models;
using System.Net;
using static SalesTrackerAPI.BL.GeoLocationAddressBL;
using SalesTrackerAPI.StaticStuff;
using Newtonsoft.Json.Linq;

namespace SalesTrackerAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/GeoLocation")]
    public class GeoLocationController : Controller
    {
        
        #region GET

        /// <summary>
        ///Kiran[13-08-2018] Get Location using Lat/Lng.  
        /// </summary>
        /// <returns></returns>
        [Route("myLocationAddress")]
        [HttpGet]
        public IActionResult myLocationAddress(string lat, string logn)
        {
            string addressResponse = BL.GeoLocationAddressBL.myLocationAddress(lat, logn);
            //For map my India

          //  dynamic obj = JsonConvert.DeserializeObject<dynamic>(addressResponse);
           // String addrStr = JsonConvert.SerializeObject(obj["results"][0]);
           // MapMyIndiaResponse addressObj = JsonConvert.DeserializeObject<MapMyIndiaResponse>(addrStr);

            //end
            //for Google API
               GoogleGeoCodeResponse addressObj = JsonConvert.DeserializeObject<GoogleGeoCodeResponse>(addressResponse);

            //for Google API    
             GeoLocationAddressTo addressTo = BL.GeoLocationAddressBL.convertToProperAddress(addressObj);

            //For map my India
            //GeoLocationAddressTo addressTo = new GeoLocationAddressTo();
            //addressTo.formatted_address = addressObj.Formatted_address;
            //addressTo.country = addressObj.Area;
            //addressTo.administrative_area_level_1 = addressObj.State;
            //addressTo.administrative_area_level_2 = addressObj.District.Split(" ")[0];
            //addressTo.locality = addressObj.SubDistrict;
            //addressTo.sublocality_level_1 = addressObj.Locality;
            //addressTo.sublocality_level_2 = addressObj.SubLocality;
            //addressTo.postal_code = addressObj.Pincode;
            //end

            if (addressTo != null)
            {
                var response = new ResponseWrapper<GeoLocationAddressTo>
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Address Fetch Successfully",
                    Body = addressTo
                };
                return Ok(response);
            }
            else
            {
                var response = new ResponseWrapper<int>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "",
                    Body = 0
                };
                return NotFound(response);
            }

        }



        [Route("GetRoutesOnLatLong")]
        [HttpGet]
        public IActionResult GetRoutesOnLatLong(String userLocStr)
        {
            try
            {

                List<TblUserLocationTO> userLocList = JsonConvert.DeserializeObject<List<TblUserLocationTO>>(userLocStr);
                String list = BL.TblUserLocationBL.getMatrixAPIUsingMapMyIndia(userLocList);

                if (list != null)
                {

                    return Ok(list);
                }
                else
                {
                    return NotFound(list);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

        }

        [Route("autoSearch")]
        [HttpGet]
        public IActionResult autoSerachMapMyindia(String query)
        {
            try
            {

                dynamic list = BL.TblUserLocationBL.autoSerachMapMyindia(query);

                if (list != null)
                {
                    autoSeachTermsTO termTO = JsonConvert.DeserializeObject<autoSeachTermsTO>(list);
                    return Ok(list);
                }
                else
                {
                    return NotFound(list);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

        }






        [Route("getAllCustomer")]
        [HttpGet]
        public IActionResult getAllCustomer(string lat, string logn)
        {
            string addressResponse = BL.GeoLocationAddressBL.myLocationAddress(lat, logn);
            GoogleGeoCodeResponse addressObj = JsonConvert.DeserializeObject<GoogleGeoCodeResponse>(addressResponse);
            GeoLocationAddressTo addressTo = BL.GeoLocationAddressBL.convertToProperAddress(addressObj);
            if (addressTo != null)
            {
                var response = new ResponseWrapper<GeoLocationAddressTo>
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Address Fetch Successfully",
                    Body = addressTo
                };
                return Ok(response);
            }
            else
            {
                var response = new ResponseWrapper<int>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "",
                    Body = 0
                };
                return NotFound(response);
            }

        }

        /// <summary>
        /// //Kiran[13-08-2018] Get Last Location 
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns>TblUserLocationTO</returns>
        [Route("getUserLastLocationListOnUserId")]
        [HttpGet]
        public List<TblUserLocationTO> getUserLastLocationListOnUserId(string userIds)
        {
            return BL.TblUserLocationBL.getUserLastLocationListOnUserId(userIds);
        }

        /// <summary>
        /// Sudhir[14-AUG-2018] Added For GetPlanRoutes 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="routeTypeE"></param>
        /// <returns></returns>
        [Route("GetPlanRoutes")]
        [HttpGet]
        public IActionResult GetPlanRoutes(int userId,Constants.RouteTypeE routeTypeE)
        {
            try
            {
                List<TblUserLocationTO> list = BL.TblUserLocationBL.SelectPlanRoute(userId, routeTypeE); 
                if (list != null)
                {
                    if (list.Count > 0)
                        return Ok(list);
                    else
                        return NoContent();
                }
                else
                {
                    return NotFound(list);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
            
        }

       //Sudhir[14-AUG-2018] Added For GetNearestDealerList 
       [Route("GetNearestDealerList")]
       [HttpGet]
       public IActionResult GetNearestDealerList(int distance,int cnfId, String userRoleTO)
       {
            try
            {
                TblUserRoleTO tblUserRoleTO = JsonConvert.DeserializeObject<TblUserRoleTO>(userRoleTO);
                List<nearBymeTo> list = BL.TblUserLocationBL.SelectNearByDealer(distance,cnfId, tblUserRoleTO);
                if (list != null)
                {
                    if (list.Count > 0)
                        return Ok(list);
                    else
                        return NoContent();
                }
                else
                {
                    return NotFound(list);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

        }


        #endregion

        #region POST
        /// <summary>
        /// //Kiran[13-08-2018] Get Last Near By customer 
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="siteType"></param>
        /// <returns>TblUserLocationTO</returns>
        [Route("getNearBycustomer")]
        [HttpGet]
        public List<nearBymeTo> getNearBycustomer(int distance, int siteType, string currentLat, string currentLng)
        {
            return BL.TblUserLocationBL.getNearBycustomer(distance, siteType, currentLat, currentLng);
        }

        [Route("PostLocationDetails")]
        [HttpPost]
        public ResultMessage PostLocationDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblUserLocationTO UserLocationTO = JsonConvert.DeserializeObject<TblUserLocationTO>(data["locationDetailsTo"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "loginUserId Found 0";
                }
                if (UserLocationTO != null)
                {
                    DateTime serverDate = Constants.ServerDateTime;
                    UserLocationTO.CurTime = serverDate;
                    UserLocationTO.UserId = Convert.ToInt32(loginUserId);
                    int result = BL.TblUserLocationBL.InsertTblUserLocation(UserLocationTO);
                    if (result != 1)
                    {
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        resultMessage.Text = "Error. Record Could Not Be Updated ";
                    }
                    else
                    {
                        resultMessage.MessageType = ResultMessageE.Information;
                        resultMessage.Result = 1;
                        resultMessage.Text = "Record Updated Successfully";
                    }
                }
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                resultMessage.Text = "Exception Error IN API Call PostLocationDetails";
                return resultMessage;
            }
        } 

        [Route("MigrationOfmyLocationAddress")]
        [HttpGet]
        public int MigrationOfmyLocationAddress()
        {
            List<newdata> migrationDataList = new List<newdata>();
            migrationDataList = BL.GeoLocationAddressBL.SelectAlllatlngData();
            if (migrationDataList != null)
            {
                foreach (var item in migrationDataList)
                {
                    string addressResponse = BL.GeoLocationAddressBL.myLocationAddress(item.Lat, item.Lng);
                    GoogleGeoCodeResponse addressObj = JsonConvert.DeserializeObject<GoogleGeoCodeResponse>(addressResponse);
                    if (addressObj.status == "OVER_QUERY_LIMIT")
                    {
                        return 0;
                    }
                    GeoLocationAddressTo addressTo = BL.GeoLocationAddressBL.convertToProperAddress(addressObj);
                    if (addressTo != null)
                    {
                        BL.GeoLocationAddressBL.insertAddress(addressTo, item.IdtblVisitDetails);
                    }
                }
            }
            return 1;

        }

        [Route("migrationLatlngdealerData")]
        [HttpGet]
        public int migrationLatlngdealerData()
        {
            var result = 0;
            List<TblAddressTO> migrationDataList = new List<TblAddressTO>();
            migrationDataList = BL.GeoLocationAddressBL.SelectAllAddress();
            if (migrationDataList != null)
            {

                foreach (var item in migrationDataList)
                {
                    var address = "";
                    if (item.PlotNo != null)
                    {
                        address += item.PlotNo + ", ";
                    }
                    if (item.StreetName != null)
                    {
                        address += item.StreetName + ", ";
                    }
                    if (item.AreaName != null)
                    {
                        address += item.AreaName + ", ";
                    }
                    if (item.VillageName != null)
                    {
                        address += item.VillageName + ", ";
                    }
                    if (item.TalukaName != null)
                    {
                        address += item.TalukaName + ", ";
                    }
                    if (item.StateName != null)
                    {
                        address += item.StateName;
                    }
                    string addressResponse = BL.GeoLocationAddressBL.myLatLngByAddress(address);
                    GoogleGeoCodeResponse addressObj = JsonConvert.DeserializeObject<GoogleGeoCodeResponse>(addressResponse);
                    if (addressObj.status == "OVER_QUERY_LIMIT" || addressObj.status == "ZERO_RESULTS")
                    {
                        //return 0;
                    }
                    else
                    {
                        GeoLocationAddressTo addressTo = BL.GeoLocationAddressBL.convertToProperAddress(addressObj);
                        if (addressTo != null)
                        {
                            result = BL.GeoLocationAddressBL.insertlatlong(addressTo, item.IdAddr);
                            if (result != 1)
                            {
                                return 0;
                            }
                        }
                    }

                }
                return 1;
            }
            return 0;
        }
        #endregion

    }
}