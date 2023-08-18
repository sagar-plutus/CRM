using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.StaticStuff;
namespace PurchaseTrackerAPI.BL
{
    public class ConnectionString : IConnectionString
    {
        private readonly HttpContext httpContext;
        public ConnectionString(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContext = httpContextAccessor.HttpContext;
        }
        public string GetSubDomain()
        {
            String url = this.httpContext.Request.Headers["apiurl"];
            if (url != null)
            {
                Uri fullPath = new Uri(url);
                string hostName = fullPath.Host;
                string[] domains = hostName.Split(new char[] { '.' });
                if (domains.Count() > 1)
                {
                    string subDomain = domains[0];
                    return subDomain;
                }
                else
                {
                    return StaticStuff.Constants.Local_URL;
                }
            }
            return null;
        }
        public string SetConnectionString(String ConfigName)
        {   
            //Added By Gokul discussed with Prajakta
            //if (Constants.Local_API == true)
            if (Startup.IsLocalAPI == true)
                {
                    if (Constants.CONNECTION_STRING == ConfigName)
                    return Startup.ConnectionString;
                if (Constants.REQUEST_ORIGIN_STRING == ConfigName)
                    return Startup.RequestOriginString;
                if (Constants.AZURE_CONNECTION_STRING == ConfigName)
                    return Startup.AzureConnectionStr;
                if (Constants.MIGRATION_CONNECTION_STRING == ConfigName)
                    return Startup.MIGRATION_CONNECTION_STRING;
            }
            else
            {
                String SubDomain = GetSubDomain();
                if (!String.IsNullOrEmpty(SubDomain))
                {
                    JObject o1 = JObject.Parse(System.IO.File.ReadAllText(@".\connection.json"));
                    return (string)o1[SubDomain][ConfigName];
                }

            }
            return string.Empty;
        }
        public string GetConnectionString(String ConfigName)
        {
            return SetConnectionString(ConfigName);
        }
    }
}
