﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using SalesTrackerAPI.BL;
using Newtonsoft.Json.Linq;
using TO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SalesTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    public class VersionController : Controller
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

        [Route("GetLatestVersion")]
        [HttpGet]
        public TblVersionTO GetLatestVersionDetails()
        {
            try
            {
                TblVersionTO tblVersionTO = BL.TblVersionBL.SelectLatestVersionTO();
                //tblVersionTO.TblVerReleaseNotesTOList = BL.TblVerReleaseNotesBL.SelectTblVerReleaseNotesTOByVerId(tblVersionTO.IdVersion);
                return tblVersionTO;
            }
            catch (Exception ex)
            {
                return null;                
            }
           

        }


        [Route("GetAllReleasedVersion")]
        [HttpGet]
        public List<TblVersionTO> GetAllReleasedVersion()
        {
            try
            {
                List<TblVersionTO> tblVersionTOList = BL.TblVersionBL.SelectAllTblVersionList();
                if (tblVersionTOList != null && tblVersionTOList.Count > 0)
                {
                    foreach (var tblVersionTO in tblVersionTOList)
                    {
                        tblVersionTO.TblVerReleaseNotesTOList = BL.TblVerReleaseNotesBL.SelectTblVerReleaseNotesTOByVerId(tblVersionTO.IdVersion);
                    }
                }

                return tblVersionTOList;
            }
            catch (Exception ex)
            {
                return null;
            }


        }

        [Route("GetRleaseNotesByVersion")]
        [HttpGet]
        public List<TblVerReleaseNotesTO> GetRleaseNotesByVersion(int versionId)
        {
            List<TblVerReleaseNotesTO> tblVerReleaseNotesTOList = new List<TblVerReleaseNotesTO>();
            try
            {
                tblVerReleaseNotesTOList = BL.TblVerReleaseNotesBL.SelectTblVerReleaseNotesTOByVerId(versionId);
            }
            catch (Exception)
            {

                return null;
            }
            return tblVerReleaseNotesTOList;
        }

        [Route("PostUserRelVer")]
        [HttpPost]
        public ResultMessage PostUserRelVer([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            TblUserVerTO tblUserVerTo = new TblUserVerTO();
            try
            {
                tblUserVerTo.VersionId = Convert.ToInt32( data["VersionId"].ToString());
                tblUserVerTo.ImeiNumber = Convert.ToString( data["DeviceId"].ToString());
                int userId = TblUserBL.SelectUserByImeiNumber(tblUserVerTo.ImeiNumber);
                if (userId != 0 && userId != -1)
                {
                    tblUserVerTo.UserId = userId;
                    tblUserVerTo.CreatedOn = Constants.ServerDateTime;
                    tblUserVerTo.CreatedBy = userId;
                    int result = TblUserVerBL.InsertTblUserVer(tblUserVerTo);
                    if (result == 1)
                    {
                        resultMessage.MessageType = ResultMessageE.Information;
                        resultMessage.Result = 1;
                        resultMessage.Text = "Success... Record saved";
                        resultMessage.DisplayMessage = Constants.DefaultSuccessMsg;
                        return resultMessage;

                    }
                }                
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = 0;
                resultMessage.Text = "Error In Method SaveUserVer";
                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                resultMessage.Text = "Exception Error In API";
                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                return resultMessage;
            }
            
        }
    }
}
