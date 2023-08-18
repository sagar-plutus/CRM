using Newtonsoft.Json;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.Models
{
    public class TblEInvoiceApiTO
    {
        #region Declarations
        Int32 idApi;
        string apiName;
        string apiMethod;
        string apiBaseUri;
        string apiFunctionName;
        string headerParam;
        string bodyParam;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Int32 isActive;
        Int32 isSession;
        string accessToken;
        DateTime sessionExpiresAt;
        #endregion

        #region Constructor
        public TblEInvoiceApiTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdApi
        {
            get { return idApi; }
            set { idApi = value; }
        }
        public string ApiName
        {
            get { return apiName; }
            set { apiName = value; }
        }
        public string ApiMethod
        {
            get { return apiMethod; }
            set { apiMethod = value; }
        }
        public string ApiBaseUri
        {
            get { return apiBaseUri; }
            set { apiBaseUri = value; }
        }
        public string ApiFunctionName
        {
            get { return apiFunctionName; }
            set { apiFunctionName = value; }
        }
        public string HeaderParam
        {
            get { return headerParam; }
            set { headerParam = value; }
        }
        public string BodyParam
        {
            get { return bodyParam; }
            set { bodyParam = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public Int32 IsSession
        {
            get { return isSession; }
            set { isSession = value; }
        }
        public string AccessToken
        {
            get { return accessToken; }
            set { accessToken = value; }
        }
        public DateTime SessionExpiresAt
        {
            get { return sessionExpiresAt; }
            set { sessionExpiresAt = value; }
        }

        #endregion

    }
}
