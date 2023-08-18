using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class DashboardEntityController : Controller
    {
        private readonly ITblDashboardEntityBL _iTblDashboardEntityBL;
        private readonly ITblDashboardEntityHistoryBL _iTblDashboardEntityHistoryBL;
        public DashboardEntityController(ITblDashboardEntityBL iTblDashboardEntityBL,
                                            ITblDashboardEntityHistoryBL iTblDashboardEntityHistoryBL)
        {
            _iTblDashboardEntityBL = iTblDashboardEntityBL;
            _iTblDashboardEntityHistoryBL = iTblDashboardEntityHistoryBL;
        }


        #region GET
        [Route("GetAllDashboardEntityList")]
        [HttpGet]
        public List<TblDashboardEntityTO> GetAllDashboardEntityList(Int32 moduleId)
        {
            return _iTblDashboardEntityBL.SelectTblDashboardEntityListByModuleId(moduleId);

        }

        [Route("SelectAllDashboardEntityReportData")]
        [HttpGet]
        public List<TblDashboardEntityHistoryTO> SelectAllDashboardEntityReportData(string fromDate, string toDate)
        {
            return _iTblDashboardEntityHistoryBL.SelectAllDashboardEntityReportData(fromDate, toDate);

        }

        #endregion
        #region POST

        [Route("PostDashboardEntityDtls")]
        [HttpPost]
        public ResultMessage PostDashboardEntityDtls([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                List<TblDashboardEntityTO> tblDashboardEntityTOList = JsonConvert.DeserializeObject<List<TblDashboardEntityTO>>(data["dashboardEntityTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : Login User ID Found NULL";
                    return resultMessage;
                }

                if (tblDashboardEntityTOList == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : tblDashboardEntityTOList Found NULL";
                    return resultMessage;
                }

                resultMessage = _iTblDashboardEntityBL.PostDashboardEntityDtls(tblDashboardEntityTOList, Convert.ToInt32(loginUserId));

                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, " PostVehicleSpotEnteryDetails");
                return resultMessage;
            }
        }



        #endregion

    }
}