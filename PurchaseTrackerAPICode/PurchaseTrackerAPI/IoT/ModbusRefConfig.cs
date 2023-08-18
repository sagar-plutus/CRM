using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.IoT {
    class ModbusRefConfig : IModbusRefConfig {
        private readonly IConnectionString _iConnectionString;

        public ModbusRefConfig (IConnectionString iConnectionString) {

            _iConnectionString = iConnectionString;
        }
        
        private static Dictionary<string, List<int>> modbusRefDict;
        //hrushikesh
        //on startup create client wise Lists
        //public static void setModbusRef () {
        //    if (!Constants.Local_API) {
        //        JObject o1 = JObject.Parse (System.IO.File.ReadAllText (@".\connection.json"));
        //        //Thread is created so main thread can continue on starting API at startup
        //        //Hrushikesh
        //        Thread thread = new Thread(delegate ()
        //        {
        //        foreach (var property in o1) {
        //            string key = property.Key;
        //            List<int> modbusRefList = getModbusRefList ((string) o1[key][Constants.REQUEST_ORIGIN_STRING]);
        //            if(modbusRefDict == null)
        //            {
        //                modbusRefDict = new Dictionary<string,List<int>>();
        //            }                   
        //            modbusRefDict.Add (key, modbusRefList);
        //        }
        //        });
        //        thread.Start();

        //    } else {
        //        Startup.AvailableModbusRefList = DimensionDAO.GeModRefMaxDataNonMulti();
        //    }
        //}


        //Hrushikesh
        //added to set list on backup process 
        //public void setModbusRefList (List<int> list) {
        //    if (!Constants.Local_API) {
        //         string domainName = _iConnectionString.GetSubDomain();
        //            modbusRefDict[domainName] = list;
        //    } else {
        //        Startup.AvailableModbusRefList = list;
        //    }
        //}

        //Hrushikesh added to return particular client's modbus List
        //public List<int> getModbusRefList () {
        //if (!Constants.Local_API) {
        //    string domainName = _iConnectionString.GetSubDomain();
        //        return modbusRefDict[domainName];
        //    } else {
        //        return Startup.AvailableModbusRefList;
        //    }

        //}

        //IOT
        //hrushikesh added to read all multitenant modbusList from Backup
        private static List<int> getModbusRefList (String RequestOriginString) {
            try {
                String requestUrl = "Masters/getModbusRefList";
                String url = Startup.CommonUrl + requestUrl;
                String result;
                StreamWriter myWriter = null;
                WebRequest request = WebRequest.Create (url);
                request.Headers.Add ("apiurl", RequestOriginString);
                request.Method = "GET";
                WebResponse objResponse = request.GetResponseAsync ().Result;
                using (StreamReader sr = new StreamReader (objResponse.GetResponseStream ())) {
                    result = sr.ReadToEnd ();
                    sr.Dispose ();
                }
                return JsonConvert.DeserializeObject<List<int>> (result);
            } catch (Exception exc) {
                throw exc;
            }
        }

    }
}