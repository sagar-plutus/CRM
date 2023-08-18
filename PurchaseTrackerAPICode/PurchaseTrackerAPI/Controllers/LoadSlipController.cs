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
    public class LoadSlipController : Controller
    {
        private readonly Itblloadingbl _iTblLoadingBL;
        public LoadSlipController(Itblloadingbl iTblLoadingBL)
        {
            _iTblLoadingBL = iTblLoadingBL;
        }

        [Route("GetVehicleNumberList")]
        [HttpGet]
        public List<VehicleNumber> GetVehicleNumberList()
        {
            return _iTblLoadingBL.SelectAllVehicles();
        }

    }
}
