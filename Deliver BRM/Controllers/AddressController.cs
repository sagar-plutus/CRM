using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using System.Net;
using System.Net.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SalesTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class AddressController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("GetOrgAddressDetails")]
        [HttpGet]
        public ActionResult GetOrgAddressDetails(Int32 orgId, Int32 addrTypeId=1)
        {
            Constants.AddressTypeE addressTypeE = (Constants.AddressTypeE)Enum.Parse(typeof(Constants.AddressTypeE), addrTypeId.ToString());
            TblAddressTO tblAddressTO =BL.TblAddressBL.SelectOrgAddressWrtAddrType(orgId, addressTypeE);
            if(tblAddressTO==null)
            {
                return NoContent();
            }
            else 
            return new ObjectResult(tblAddressTO);
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
