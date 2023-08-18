using Newtonsoft.Json;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.Models
{
    public class TblEInvoiceSessionApiResponseTO
    {
        #region Declarations

        Int32 idResponse;
        Int32 apiId;
        string accessToken;
        DateTime tokenExpiresAt;
        string responseStatus;
        string response;
        Int32 createdBy;
        DateTime createdOn;
        Int32 orgId;

        #endregion

        #region Constructor

        public TblEInvoiceSessionApiResponseTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdResponse
        {
            get { return idResponse; }
            set { idResponse = value; }
        }

        public Int32 ApiId
        {
            get { return apiId; }
            set { apiId = value; }
        }
        public string AccessToken
        {
            get { return accessToken; }
            set { accessToken = value; }
        }
        public string ResponseStatus
        {
            get { return responseStatus; }
            set { responseStatus = value; }
        }
        public string Response
        {
            get { return response; }
            set { response = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public Int32 OrgId
        {
            get { return orgId; }
            set { orgId = value; }
        }

        #endregion

    }
}
