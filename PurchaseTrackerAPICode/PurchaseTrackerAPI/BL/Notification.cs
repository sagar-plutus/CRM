using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL
{
    public class Notification : INotification
    {
        private readonly IConnectionString _iConnectionString;

        public Notification(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        public void SendNotificationToUsers(TblAlertInstanceTO tblAlertInstanceTO)
        {

            try
            {
                var values = new JObject();
                values.Add("tblAlertInstanceTO", JsonConvert.SerializeObject(tblAlertInstanceTO));

                ApiData data = new ApiData();
                data.tblAlertInstanceTO = tblAlertInstanceTO;

                MemoryStream ms = new MemoryStream();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ApiData));
                ser.WriteObject(ms, data);
                byte[] json = ms.ToArray();
                ms.Close();

                String notifyUrl = "Notify/PostNewAlert";

                String url = Startup.CommonUrl + notifyUrl;
                object result;
                StreamWriter myWriter = null;
                WebRequest request = WebRequest.Create(url);
                request.Headers.Add("apiurl", _iConnectionString.GetConnectionString(Constants.REQUEST_ORIGIN_STRING));
                request.Method = "Post";
                request.ContentType = "application/json";

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(json, 0, json.Length);
                    requestStream.Close();
                }

                WebResponse objResponse = request.GetResponseAsync().Result;
                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    sr.Dispose();
                }

            }
            catch (Exception exc)
            {
               // throw exc;
            }
        }
    }

    public class ApiData
    {
        public TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
    }
}
