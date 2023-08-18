using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using System.Linq;
using SalesTrackerAPI.BL;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;

namespace SalesTrackerAPI.IoT
{
    public class GateCommunication
    {
        private static readonly object GatebalanceLock = new object();
        public static int PostGateAPIDataToModbusTcpApiForLoadingSlip(TblLoadingTO tblLoadingTO, Object[] writeData)
        {
            lock (GatebalanceLock)
            {
                //var tRequest = WebRequest.Create(Startup.GateIotApiURL + "WriteOnGateIoTCommand") as HttpWebRequest;
                var tRequest = WebRequest.Create(tblLoadingTO.IoTUrl + "WriteOnGateIoTCommand") as HttpWebRequest;
                try
                {
                    tRequest.Method = "post";
                    tRequest.ContentType = "application/json";
                    var data = new
                    {
                        data = writeData,
                        portNumber = tblLoadingTO.PortNumber,
                        machineIP = tblLoadingTO.MachineIP
                    };
                    tRequest.Timeout = 3000;
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                    Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                    using (Stream dataStream = tRequest.GetRequestStreamAsync().Result)
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);
                    }
                    var response = (HttpWebResponse)tRequest.GetResponseAsync().Result;
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    var resultdata = JsonConvert.DeserializeObject<NodeJsResult>(responseString);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        if (resultdata != null && resultdata.Code == 1)
                        {
                            return 1;
                        }
                    }
                    return 0;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
            //return PostGateApiCalls(writeData, tRequest);
        }

        public static int PostGateApiCalls(TblLoadingTO tblLoadingTO, object[] writeData, HttpWebRequest tRequest)
        {
            lock (GatebalanceLock)
            {
                try
                {
                    tRequest.Method = "post";
                    tRequest.ContentType = "application/json";
                    //var data = new
                    //{
                    //    data = writeData
                    //};
                    var data = new
                    {
                        data = writeData,
                        portNumber = tblLoadingTO.PortNumber,
                        machineIP = tblLoadingTO.MachineIP
                    };
                    tRequest.Timeout = 5000;
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                    Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                    using (Stream dataStream = tRequest.GetRequestStreamAsync().Result)
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);
                    }
                    var response = (HttpWebResponse)tRequest.GetResponseAsync().Result;
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    var resultdata = JsonConvert.DeserializeObject<NodeJsResult>(responseString);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        if (resultdata != null && resultdata.Code == 1)
                        {
                            return 1;
                        }
                    }
                    return 0;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }

        }

        public static GateIoTResult GetLoadingStatusHistoryDataFromGateIoT(TblLoadingTO tblLoadingTO)
        {

            //var sendportInfo = new SendPortToIoT();
            //sendportInfo.ModBusRefId = tblLoadingTO.ModbusRefId.ToString();
            //sendportInfo.MachineIP = tblLoadingTO.MachineIP.ToString();
            //sendportInfo.PortNumber = tblLoadingTO.PortNumber.ToString();

            var querystring = "?ModbusRefId=" + tblLoadingTO.ModbusRefId;
            querystring += "&MachineIP=" + tblLoadingTO.MachineIP;
            querystring += "&PortNumber=" + tblLoadingTO.PortNumber;
            lock (GatebalanceLock)
            {
                GateIoTResult gateIoTResult = new GateIoTResult();
                try
                {
                    if (tblLoadingTO.ModbusRefId != 0)
                    {
                        //var request = WebRequest.Create(Startup.GateIotApiURL + "GetLoadingStatusHistoryData?loadingId=" + tblLoadingTO.ModbusRefId) as HttpWebRequest;
                        var request = WebRequest.Create(tblLoadingTO.IoTUrl + "GetLoadingStatusHistoryData" + querystring) as HttpWebRequest;
                        String result;
                        //WebRequest request = WebRequest.Create(url);
                        request.Method = "GET";
                        request.Timeout = 4000;
                        var response = (HttpWebResponse)request.GetResponseAsync().Result;
                        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        {
                            result = sr.ReadToEnd();
                            var resultdata = JsonConvert.DeserializeObject<GateIoTResult>(result);
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                if (resultdata != null && resultdata.Code == 1)
                                {
                                    gateIoTResult.DefaultSuccessBehavior(1, "data Found", resultdata.Data);
                                }
                            }
                            else
                            {
                                gateIoTResult.DefaultErrorBehavior(0, resultdata.Msg);
                            }

                            request.Abort();
                            sr.Dispose();
                        }
                        return gateIoTResult;
                    }
                    else
                    {
                        gateIoTResult.DefaultErrorBehavior(0, "Loading id not found");
                        return gateIoTResult;
                    }
                }
                catch (Exception ex)
                {
                    gateIoTResult.DefaultErrorBehavior(0, "Error in GetLoadingStatusHistoryDataFromGateIoT");
                    return gateIoTResult;
                }
            }
        }

        public static GateIoTResult GetLoadingSlipsByStatusFromIoT(Int32 statusId, TblGateTO tblGateTO, Int32 startLoadingId = 1)
        {
            lock (GatebalanceLock)
            {
                GateIoTResult gateIoTResult = new GateIoTResult();
                try
                {
                    if (tblGateTO == null || String.IsNullOrEmpty(tblGateTO.IoTUrl))
                    {
                        return null;
                    }
                    var queryString = "&PortNumber=" + tblGateTO.PortNumber;
                    queryString += "&MachineIP=" + tblGateTO.MachineIP;

                    //    var request = WebRequest.Create(Startup.GateIotApiURL + "GetLoadingStatusData?loadingId=" + startLoadingId + "&statusId=" + statusId + "") as HttpWebRequest;
                    var request = WebRequest.Create(tblGateTO.IoTUrl + "GetLoadingStatusData?loadingId=" + startLoadingId + "&statusId=" + statusId + queryString) as HttpWebRequest;
                    String result;
                    //WebRequest request = WebRequest.Create(url);
                    request.Method = "GET";
                    request.Timeout = 4000;
                    var response = (HttpWebResponse)request.GetResponseAsync().Result;
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        result = sr.ReadToEnd();
                        var resultdata = JsonConvert.DeserializeObject<GateIoTResult>(result);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            if (resultdata != null && resultdata.Code == 1)
                            {
                                gateIoTResult.DefaultSuccessBehavior(1, "data Found", resultdata.Data);
                            }
                        }
                        else
                        {
                            gateIoTResult.DefaultErrorBehavior(0, resultdata.Msg);
                        }

                        request.Abort();
                        sr.Dispose();
                    }
                    return gateIoTResult;
                }
                catch (Exception ex)
                {
                    gateIoTResult.DefaultErrorBehavior(0, "Error in GetLoadingStatusHistoryDataFromGateIoT");
                    return gateIoTResult;
                }
            }
        }

        public static GateIoTResult GetAllLoadingSlipsByStatusFromIoT(Int32 statusId, TblGateTO tblGateTO, Int32 startLoadingId = 1)
        {
            lock (GatebalanceLock)
            {
                GateIoTResult gateIoTResult = new GateIoTResult();
                try
                {
                    if (tblGateTO == null || String.IsNullOrEmpty(tblGateTO.IoTUrl))
                    {
                        return null;
                    }
                    var queryString = "&portNumber=" + tblGateTO.PortNumber;
                    queryString += "&machineIP=" + tblGateTO.MachineIP;
                    var request = WebRequest.Create(tblGateTO.IoTUrl + "GetLoadingStatusDataAll?loadingId=" + startLoadingId + "&statusId=" + statusId + queryString) as HttpWebRequest;
                    //var request = WebRequest.Create("http://localhost:3000/api/GetLoadingStatusDataAll?loadingId=" + startLoadingId + "&statusId=" + statusId + queryString) as HttpWebRequest;
                    String result;
                    //WebRequest request = WebRequest.Create(url);
                    request.Method = "GET";
                    //request.Timeout = 4000;
                    var response = (HttpWebResponse)request.GetResponseAsync().Result;
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        result = sr.ReadToEnd();
                        var resultdata = JsonConvert.DeserializeObject<GateIoTResult>(result);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            if (resultdata != null && resultdata.Code == 1)
                            {
                                gateIoTResult.DefaultSuccessBehavior(1, "data Found", resultdata.Data);
                            }
                        }
                        else
                        {
                            gateIoTResult.DefaultErrorBehavior(0, resultdata.Msg);
                        }

                        request.Abort();
                        sr.Dispose();
                    }
                    return gateIoTResult;
                }
                catch (Exception ex)
                {
                    gateIoTResult.DefaultErrorBehavior(0, "Error in GetLoadingStatusHistoryDataFromGateIoT");
                    return gateIoTResult;
                }
            }
        }

        public static GateIoTResult DeleteSingleLoadingFromGateIoT(TblLoadingTO tblLoadingTO)
        {
            lock (GatebalanceLock)
            {
                GateIoTResult gateIoTResult = new GateIoTResult();
                try
                {
                    if (tblLoadingTO.ModbusRefId != 0)
                    {
                        var queryString = "?PortNumber=" + tblLoadingTO.PortNumber;
                        queryString += "&MachineIP=" + tblLoadingTO.MachineIP;
                        queryString += "&ModBusRefId=" + tblLoadingTO.ModbusRefId;
                        //var sendportInfo = new SendPortToIoT();
                        //sendportInfo.ModBusRefId = tblLoadingTO.ModbusRefId.ToString();
                        //sendportInfo.PortNumber = tblLoadingTO.PortNumber.ToString();
                        //sendportInfo.MachineIP = tblLoadingTO.MachineIP.ToString();
                        var request = WebRequest.Create(tblLoadingTO.IoTUrl + "DeleteLoadingStatus" + queryString) as HttpWebRequest;
                        String result;
                        //WebRequest request = WebRequest.Create(url);
                        request.Method = "GET";
                        request.Timeout = 5000;
                        var response = (HttpWebResponse)request.GetResponseAsync().Result;
                        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        {
                            result = sr.ReadToEnd();
                            var resultdata = JsonConvert.DeserializeObject<GateIoTResult>(result);
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                if (resultdata != null && resultdata.Code == 1)
                                {
                                    gateIoTResult.DefaultSuccessBehavior(1, "Loading Status Details Deleted Successfully.", resultdata.Data);
                                }
                            }
                            else
                            {
                                gateIoTResult.DefaultErrorBehavior(0, resultdata.Msg);
                            }
                            sr.Dispose();
                        }
                        return gateIoTResult;
                    }
                    else
                    {
                        gateIoTResult.DefaultErrorBehavior(0, "Loading id not found");
                        return gateIoTResult;
                    }
                }
                catch (Exception ex)
                {
                    gateIoTResult.DefaultErrorBehavior(0, "Error in DeleteSingleLoadingFromGateIoT");
                    return gateIoTResult;
                }
            }
        }

        public static GateIoTResult DeleteAllLoadingFromGateIoT()
        {
            lock (GatebalanceLock)
            {
                GateIoTResult gateIoTResult = new GateIoTResult();
                try
                {

                    var request = WebRequest.Create("CompleteStatusClear") as HttpWebRequest;
                    String result;
                    //WebRequest request = WebRequest.Create(url);
                    request.Method = "GET";
                    request.Timeout = 3000;
                    var response = (HttpWebResponse)request.GetResponseAsync().Result;
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        result = sr.ReadToEnd();
                        var resultdata = JsonConvert.DeserializeObject<GateIoTResult>(result);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            if (resultdata != null && resultdata.Code == 1)
                            {
                                gateIoTResult.DefaultSuccessBehavior(1, "Loading Status Details Deleted Successfully.", resultdata.Data);
                            }
                        }
                        else
                        {
                            gateIoTResult.DefaultErrorBehavior(0, resultdata.Msg);
                        }
                        sr.Dispose();
                    }
                    return gateIoTResult;

                }
                catch (Exception ex)
                {
                    gateIoTResult.DefaultErrorBehavior(0, "Error in DeleteAllLoadingFromGateIoT");
                    return gateIoTResult;
                }
            }
        }
    }
}
