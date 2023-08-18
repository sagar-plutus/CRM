using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using System.Linq;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.IoT.Interfaces;
using PurchaseTrackerAPI.IoT;

namespace PurchaseTrackerAPI.IoT
{
    public class GateCommunication : IGateCommunication
    {

        private static readonly object GatebalanceLock = new object();
        public int PostGateAPIDataToModbusTcpApiForLoadingSlip(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, Object[] writeData)
        {
            lock (GatebalanceLock)
            {
                //var tRequest = WebRequest.Create(Startup.GateIotApiURL + "WriteOnGateIoTCommand") as HttpWebRequest;
                var tRequest = WebRequest.Create(tblPurchaseScheduleSummaryTO.IoTUrl + "WriteOnGateIoTCommand") as HttpWebRequest;
                try
                {
                    tRequest.Method = "post";
                    tRequest.ContentType = "application/json";
                    var data = new
                    {
                        data = writeData,
                        portNumber = tblPurchaseScheduleSummaryTO.PortNumber,
                        machineIP = tblPurchaseScheduleSummaryTO.MachineIP
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

        public int PostGateApiCalls(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, object[] writeData, HttpWebRequest tRequest)
        {
            lock (GatebalanceLock)
            {
                try
                {
                    tRequest.Method = "post";
                    tRequest.ContentType = "application/json";
                    var data = new
                    {
                        data = writeData,
                        portNumber = tblPurchaseScheduleSummaryTO.PortNumber,
                        machineIP = tblPurchaseScheduleSummaryTO.MachineIP
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

        public GateIoTResult GetLoadingStatusHistoryDataFromGateIoT(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO)
        {

            //var sendportInfo = new SendPortToIoT();
            //sendportInfo.ModBusRefId = tblLoadingTO.ModbusRefId.ToString();
            //sendportInfo.MachineIP = tblLoadingTO.MachineIP.ToString();
            //sendportInfo.PortNumber = tblLoadingTO.PortNumber.ToString();

            var querystring = "?ModbusRefId=" + tblPurchaseScheduleSummaryTO.ModbusRefId;
            querystring += "&MachineIP=" + tblPurchaseScheduleSummaryTO.MachineIP;
            querystring += "&PortNumber=" + tblPurchaseScheduleSummaryTO.PortNumber;
            lock (GatebalanceLock)
            {
                GateIoTResult gateIoTResult = new GateIoTResult();
                try
                {
                    if (tblPurchaseScheduleSummaryTO.ModbusRefId != 0)
                    {
                        //var request = WebRequest.Create(Startup.GateIotApiURL + "GetLoadingStatusHistoryData?loadingId=" + tblLoadingTO.ModbusRefId) as HttpWebRequest;
                        var request = WebRequest.Create(tblPurchaseScheduleSummaryTO.IoTUrl + "GetLoadingStatusHistoryData" + querystring) as HttpWebRequest;
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

        public GateIoTResult GetLoadingSlipsByStatusFromIoT(Int32 statusId, TblGateTO tblGateTO, Int32 startLoadingId = 1)
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

        public GateIoTResult GetAllLoadingSlipsByStatusFromIoT(Int32 statusId, TblGateTO tblGateTO, Int32 startLoadingId = 1)
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

        public GateIoTResult DeleteSingleLoadingFromGateIoT(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO)
        {
            lock (GatebalanceLock)
            {
                GateIoTResult gateIoTResult = new GateIoTResult();
                try
                {
                    if (tblPurchaseScheduleSummaryTO.ModbusRefId != 0)
                    {
                        var queryString = "?PortNumber=" + tblPurchaseScheduleSummaryTO.PortNumber;
                        queryString += "&MachineIP=" + tblPurchaseScheduleSummaryTO.MachineIP;
                        queryString += "&ModBusRefId=" + tblPurchaseScheduleSummaryTO.ModbusRefId;
                        //var sendportInfo = new SendPortToIoT();
                        //sendportInfo.ModBusRefId = tblLoadingTO.ModbusRefId.ToString();
                        //sendportInfo.PortNumber = tblLoadingTO.PortNumber.ToString();
                        //sendportInfo.MachineIP = tblLoadingTO.MachineIP.ToString();
                        var request = WebRequest.Create(tblPurchaseScheduleSummaryTO.IoTUrl + "DeleteLoadingStatus" + queryString) as HttpWebRequest;
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

        public string ReadWeightFromWeightIoT(TblWeighingMachineTO machineTO)
        {
            lock (GatebalanceLock)
            {
                GateIoTResult gateIoTResult = new GateIoTResult();
                try
                {
                        var queryString = "?portNumber=" + machineTO.PortNumber;
                        queryString += "&machineIP=" + machineTO.MachineIP;
                        //var sendportInfo = new SendPortToIoT();
                        //sendportInfo.ModBusRefId = tblLoadingTO.ModbusRefId.ToString();
                        //sendportInfo.PortNumber = tblLoadingTO.PortNumber.ToString();
                        //sendportInfo.MachineIP = tblLoadingTO.MachineIP.ToString();
                        var request = WebRequest.Create(machineTO.IoTUrl + "ReadWeight" + queryString) as HttpWebRequest;
                        string result;
                        //WebRequest request = WebRequest.Create(url);
                        request.Method = "GET";
                        request.Timeout = 5000;
                        var response = (HttpWebResponse)request.GetResponseAsync().Result;
                        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        {
                            result = sr.ReadToEnd();
                            sr.Dispose(); 
                        }
                    return result;
                }
                catch (Exception ex)
                {
                    return "0";
                }
            }
        }

        public GateIoTResult DeleteAllLoadingFromGateIoT()
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
