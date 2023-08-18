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
using PurchaseTrackerAPI.IoT.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.IoT;

namespace PurchaseTrackerAPI.IoT
{
    public class WeighingCommunication : IWeighingCommunication
    {
        private readonly object WeightbalanceLock = new object();

        public int PostDataFrommodbusTcpApi(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, int[] writeData, TblWeighingMachineTO machineTO)
        {
            lock (WeightbalanceLock)
            {
                try
                {
                    if (machineTO == null || String.IsNullOrEmpty(machineTO.IoTUrl))
                    {
                        return 0;
                    }

                    var tRequest = WebRequest.Create(machineTO.IoTUrl + "WriteCommand") as HttpWebRequest;
                    tRequest.Method = "post";
                    tRequest.ContentType = "application/json";
                    var data = new
                    {
                        data = writeData,
                        portNumber = machineTO.PortNumber,
                        machineIP = machineTO.MachineIP
                    };
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

        public NodeJsResult GetLoadingLayerData(int loadingId, int layerId, TblWeighingMachineTO machineTO)
        {
            lock (WeightbalanceLock)
            {
                NodeJsResult nodeJsResult = new NodeJsResult();
                try
                {
                    if (machineTO == null || String.IsNullOrEmpty(machineTO.IoTUrl))
                    {
                        nodeJsResult.DefaultErrorBehavior(0, "IOT url not found");
                        return nodeJsResult;
                    }

                    if (loadingId != 0)
                    {
                        String queryStr = "&portNumber=" + machineTO.PortNumber + "&machineIP=" + machineTO.MachineIP;

                        String url = machineTO.IoTUrl + "GetLoadingLayerData?loadingId=" + loadingId + "&layerId=" + layerId + queryStr;
                        String result;
                        WebRequest request = WebRequest.Create(url);
                        request.Method = "GET";
                        request.Timeout = 4000;
                        var response = (HttpWebResponse)request.GetResponseAsync().Result;
                        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        {
                            result = sr.ReadToEnd();
                            var resultdata = JsonConvert.DeserializeObject<NodeJsResult>(result);
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                if (resultdata != null && resultdata.Code == 1)
                                {
                                    nodeJsResult.DefaultSuccessBehavior(1, "data Found", resultdata.Data);
                                }
                            }
                            else
                            {
                                nodeJsResult.DefaultErrorBehavior(0, resultdata.Msg);
                            }
                            request.Abort();
                            sr.Dispose();
                        }
                        return nodeJsResult;
                    }
                    else
                    {
                        nodeJsResult.DefaultErrorBehavior(0, "Loading id not found");
                        return nodeJsResult;
                    }
                }
                catch (Exception ex)
                {
                    nodeJsResult.DefaultErrorBehavior(0, "Error in GetLoadingLayerData");
                    return nodeJsResult;
                }
            }
        }

        public NodeJsResult DeleteSingleLoadingFromWeightIoT(int loadingId, int layerId, TblWeighingMachineTO machineTO)
        {
            lock (WeightbalanceLock)
            {
                NodeJsResult nodeJsResult = new NodeJsResult();
                try
                {
                    if (machineTO == null || String.IsNullOrEmpty(machineTO.IoTUrl))
                    {
                        nodeJsResult.DefaultErrorBehavior(0, "IOT url not found");
                        return nodeJsResult;
                    }
                    if (loadingId != 0)
                    {
                        var queryString = "&portNumber=" + machineTO.PortNumber;
                        queryString += "&machineIP=" + machineTO.MachineIP;
                        String url = machineTO.IoTUrl + "DeleteCompleteLoading?loadingId=" + loadingId + "&layerId=" + layerId + queryString;
                        String result;
                        WebRequest request = WebRequest.Create(url);
                        request.Method = "GET";
                        request.Timeout = 10000;
                        var response = (HttpWebResponse)request.GetResponseAsync().Result;
                        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        {
                            result = sr.ReadToEnd();
                            var resultdata = JsonConvert.DeserializeObject<NodeJsResult>(result);
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                if (resultdata != null && resultdata.Code == 1)
                                {
                                    nodeJsResult.DefaultSuccessBehavior(1, "Given Loading Deleted Successfully.", resultdata.Data);
                                }
                            }
                            else
                            {
                                nodeJsResult.DefaultErrorBehavior(0, resultdata.Msg);
                            }
                            sr.Dispose();
                        }
                        return nodeJsResult;
                    }
                    else
                    {
                        nodeJsResult.DefaultErrorBehavior(0, "Loading id not found");
                        return nodeJsResult;
                    }
                }
                catch (Exception ex)
                {
                    nodeJsResult.DefaultErrorBehavior(0, "Error in DeleteSingleLoadingFromWeightIoT");
                    return nodeJsResult;
                }
            }
        }

        public NodeJsResult DeleteAllLoadingFromWeightIoT(string ioTUrl)
        {
            lock (WeightbalanceLock)
            {
                NodeJsResult nodeJsResult = new NodeJsResult();
                try
                {

                    String url = ioTUrl + "CompleteClear";
                    String result;
                    WebRequest request = WebRequest.Create(url);
                    request.Method = "GET";
                    request.Timeout = 3000;
                    var response = (HttpWebResponse)request.GetResponseAsync().Result;
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        result = sr.ReadToEnd();
                        var resultdata = JsonConvert.DeserializeObject<NodeJsResult>(result);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            if (resultdata != null && resultdata.Code == 1)
                            {
                                nodeJsResult.DefaultSuccessBehavior(1, "All Data Deleted.", resultdata.Data);
                            }
                        }
                        else
                        {
                            nodeJsResult.DefaultErrorBehavior(0, resultdata.Msg);
                        }

                        request.Abort();
                        sr.Dispose();
                    }
                    return nodeJsResult;

                }
                catch (Exception ex)
                {
                    nodeJsResult.DefaultErrorBehavior(0, "Error in DeleteAllLoadingFromWeightIoT");
                    return nodeJsResult;
                }
            }
        }

    }
}
