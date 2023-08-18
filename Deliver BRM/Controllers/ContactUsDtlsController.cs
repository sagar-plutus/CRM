using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesTrackerAPI.BL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SalesTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class ContactUsDtlsController : Controller
    {
        // Select contacts on condition - Tejaswini
        [Route("GetContactUsDtls/IsActive/{IsActive}")]
        [HttpGet]
        public List<IGrouping<int, TblContactUsDtls>> GetContactUsDtlsList(int IsActive)
        {
            try
            {
                TblContactUsDtlsBL tblContactUsDtlsBL = new TblContactUsDtlsBL();
                return tblContactUsDtlsBL.SelectContactUsDtls(IsActive);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }

}