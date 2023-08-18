using Newtonsoft.Json;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SalesTrackerAPI.BL
{
    public class TblSupportDetailsBL
    {
        #region Selection
        public static List<TblSupportDetailsTO> SelectAllTblSupportDetails()
        {
            return TblSupportDetailsDAO.SelectAllTblSupportDetails();
        }

        public static List<TblSupportDetailsTO> SelectAllTblSupportDetailsList()
        {
            List<TblSupportDetailsTO> tblSupportDetailsList = TblSupportDetailsDAO.SelectAllTblSupportDetails();
            if (tblSupportDetailsList != null)
                return tblSupportDetailsList;
            else
                return null;
        }

        public static TblSupportDetailsTO SelectTblSupportDetailsTO(Int32 idsupport)
        {
            TblSupportDetailsTO tblSupportDetailsTO = TblSupportDetailsDAO.SelectTblSupportDetails(idsupport);
            if (tblSupportDetailsTO != null)
                return tblSupportDetailsTO;
            else
                return null;
        }

        public static IoTDetialsTo SelectInformationFormIoT(int modbusRefId,int portNumber)
        {
            IoTDetialsTo ioTDetialsTo = new IoTDetialsTo();
            decimal WeightTransactionId = Math.Floor(Convert.ToDecimal(modbusRefId * 256));
            int GateTransactionId = GetTransactionIdUsingCal(modbusRefId, 1);
            Dictionary<string, String> WeighingkeyValuePairs = SelectlayerWiseData(WeightTransactionId, 2);
            if (WeighingkeyValuePairs != null)
            {
                GateIoTResultForIot gateIoTResult = new GateIoTResultForIot();
                foreach (var item in WeighingkeyValuePairs)
                {
                    var frames = new ArraySegment<string>(item.Value.Split(","), 0, 8);
                    string mergeAllLayerData = string.Join(",", frames.ToList().ToArray());
                    gateIoTResult = GetDecryptedLoadingId(item.Value, "GetLoadingDecriptedLayerData");
                    ioTDetialsTo.WeighingDeatils.Add(item.Key, gateIoTResult.Data);
                }
            }
            Dictionary<string, String> GatekeyValuePairs = SelectlayerWiseData(GateTransactionId, 1);
            if (GatekeyValuePairs != null)
            {
                var frames = new ArraySegment<string>(GatekeyValuePairs[portNumber.ToString()].Split(","), 0, 8);
                string IOTframe = string.Join(",", frames.ToList().ToArray());
                GateIoTResultForIot gateIoTResult = GetDecryptedLoadingId(IOTframe, "GetLoadingDecriptionData");
                ioTDetialsTo.GateDeatils = gateIoTResult.Data;
            }
            return ioTDetialsTo;

        }


        public static GateIoTResultForIot GetDecryptedLoadingId(string dataFrame, string methodName)
        {
            GateIoTResultForIot gateIOTResult = new GateIoTResultForIot();
            try
            {
                if (String.IsNullOrEmpty(dataFrame))
                {
                    gateIOTResult.DefaultErrorBehavior(0, "transaction ID not found");
                    return gateIOTResult;
                }
                String url = Startup.GateIotApiURL + methodName + "?data=" + dataFrame;
                //String url = "https://brmnodejs.azurewebsites.net/api/" + methodName + "?data=" + dataFrame;

                String result;
                System.Net.WebRequest request = WebRequest.Create(url);
                request.Method = "GET";
                // request.Timeout = 10000;
                var response = (HttpWebResponse)request.GetResponseAsync().Result;
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    var resultdata = JsonConvert.DeserializeObject<GateIoTResultForIot>(result);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        if (resultdata != null && resultdata.Code == 1)
                        {
                            gateIOTResult.DefaultSuccessBehavior(1, "data Found", resultdata.Data);
                        }
                    }
                    else
                    {
                        gateIOTResult.DefaultErrorBehavior(0, resultdata.Msg);
                    }
                    request.Abort();
                    sr.Dispose();
                }
                return gateIOTResult;
            }
            catch (Exception ex)
            {
                gateIOTResult.DefaultErrorBehavior(0, "Error in GetDecryptedLoadingId");
                return gateIOTResult;
            }
        }

        public static Dictionary<string, String> SelectlayerWiseData(decimal transactionId, int machineType)
        {
            List<TblLoadingSlipTO> loadingSlipList = new List<TblLoadingSlipTO>();
            String sqlConnStr = Startup.IoTBackUpConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader dr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT machinedata,machinePortNumber FROM tblMachineBackup WHERE transactionId=" + transactionId + " AND machineType=" + machineType;
                // cmdSelect.CommandText = "select idloadingslip,idinvoice from temploadingslip t inner join tempinvoice i on t.idloadingslip=i.loadingslipid where  loadingid="+loadingId;
                cmdSelect.Connection = conn;
                //cmdSelect.Transaction = sqlTransaction;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                dr = (cmdSelect.ExecuteReader());
                Dictionary<string, String> layerWiseData = new Dictionary<string, string>();
                while (dr.Read())
                {
                    layerWiseData.Add(Convert.ToString(dr["machinePortNumber"].ToString()), Convert.ToString(dr["machinedata"].ToString()));
                }
                return layerWiseData;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<int> GetAllTransIds(int machineType)
        {
            String sqlConnStr = Startup.IoTBackUpConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT transactionId FROM [tblMachineBackup] WHERE machineType=" + machineType;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<int> List = new List<int>();
                if (sqlReader != null)
                {
                    while (sqlReader.Read())
                    {
                        if (sqlReader["transactionId"] != DBNull.Value)
                            List.Add(Convert.ToInt32(sqlReader["transactionId"].ToString()));
                    }
                }
                return List;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                // sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static int GetTransactionIdUsingCal(int transId, int machineType)
        {
            int finalItemVal = 0;
            try
            {
                List<int> list = GetAllTransIds(machineType);
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        var hexData = item.ToString("X");
                        if (hexData.Length == 3)
                            hexData = "0" + hexData;
                        hexData = hexData.Substring(0, 2);
                        int transIdDub = int.Parse(hexData, System.Globalization.NumberStyles.HexNumber);
                        if (transIdDub == transId)
                        {
                            finalItemVal = item;
                            break;
                        }
                    }
                }
                return finalItemVal;
            }
            catch (Exception)
            {
                return finalItemVal;
            }

        }


        #endregion

        #region Insertion
        public static int InsertTblSupportDetails(TblSupportDetailsTO tblSupportDetailsTO)
        {
            return TblSupportDetailsDAO.InsertTblSupportDetails(tblSupportDetailsTO);
        }

        public static int InsertTblSupportDetails(TblSupportDetailsTO tblSupportDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblSupportDetailsDAO.InsertTblSupportDetails(tblSupportDetailsTO, conn, tran);
        }

        public static ResultMessage PostDeleteWeighingMeasureForSupport(TblWeighingMeasuresTO tblWeighingMeasuresTO, Int32 fromUser)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            TblWeighingMeasuresTO previousWeighingMeasuresTO = new TblWeighingMeasuresTO();
            int result = 0;
            try
            {
                previousWeighingMeasuresTO = TblWeighingMeasuresBL.SelectTblWeighingMeasuresTO(tblWeighingMeasuresTO.IdWeightMeasure);
                //tblUserTO = TblUserBL.SelectTblUserTO(fromUser);
                conn.Open();
                tran = conn.BeginTransaction();
                result = TblWeighingMeasuresBL.DeleteTblWeighingMeasures(tblWeighingMeasuresTO.IdWeightMeasure);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error in UpdateInvoiceforSupport");
                    return resultMessage;
                }
                else
                {
                    String description = String.Empty;
                    description = "In TblWeighingMeasures Id " + tblWeighingMeasuresTO.IdWeightMeasure + " Related Entry Deleted";

                    String Comment = String.Empty;
                    Comment = "Previous Record Information of WeighingMeasuresId=#" + tblWeighingMeasuresTO.IdWeightMeasure + "|| LoadingId is=" + previousWeighingMeasuresTO.LoadingId +
                              " || WeighingMachineId Is" + previousWeighingMeasuresTO.WeighingMachineId + " || Vehicle No Is " + previousWeighingMeasuresTO.VehicleNo +
                              " || WeightMeasurTypeId Is " + previousWeighingMeasuresTO.WeightMeasurTypeId + "|| WeightMT is " + previousWeighingMeasuresTO.WeightMT +
                              " || Created By" + previousWeighingMeasuresTO.CreatedBy;


                    TblSupportDetailsTO tblSupportDetailsTO = new TblSupportDetailsTO();
                    tblSupportDetailsTO.ModuleId = 1;//By Defalult Module id is Set to Commercial;
                    tblSupportDetailsTO.Formid = Convert.ToInt32(Constants.SupportPageTypE.LOADING_SLIP);
                    tblSupportDetailsTO.FromUser = fromUser;
                    tblSupportDetailsTO.CreatedBy = tblWeighingMeasuresTO.UpdatedBy;
                    tblSupportDetailsTO.CreatedOn = Constants.ServerDateTime;
                    tblSupportDetailsTO.Description = description;
                    tblSupportDetailsTO.RequireTime = 30;//HardCoded 30 Minutes;
                    tblSupportDetailsTO.Comments = Comment;

                    result = TblSupportDetailsBL.InsertTblSupportDetails(tblSupportDetailsTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Error in InsertTblSupportDetails");
                        return resultMessage;
                    }
                    tran.Commit();
                    resultMessage.Tag = description;
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "Error in UpdateInvoiceForSupport");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }

        }

        #endregion

        #region Updation
        public static int UpdateTblSupportDetails(TblSupportDetailsTO tblSupportDetailsTO)
        {
            return TblSupportDetailsDAO.UpdateTblSupportDetails(tblSupportDetailsTO);
        }

        public static ResultMessage UpdateInvoiceForSupport(TblInvoiceTO tblInvoiceTO, Int32 fromUser, String Comments)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            TblInvoiceTO previousInvoiceTO = new TblInvoiceTO();
            //TblUserTO tblUserTO = new TblUserTO();
            int result = 0;
            try
            {
                previousInvoiceTO = TblInvoiceBL.SelectTblInvoiceTO(tblInvoiceTO.IdInvoice);
                //tblUserTO = TblUserBL.SelectTblUserTO(fromUser);
                conn.Open();
                tran = conn.BeginTransaction();
                result = TblInvoiceBL.UpdateTblInvoice(tblInvoiceTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error in UpdateInvoiceforSupport");
                    return resultMessage;
                }
                else
                {
                    String description = String.Empty;
                    description = "Invoice Id " + tblInvoiceTO.IdInvoice + " ,";
                    if (previousInvoiceTO.StatusId != tblInvoiceTO.StatusId)
                    {
                        description += " Status change from " + previousInvoiceTO.InvoiceStatusE.ToString() + " to " + tblInvoiceTO.InvoiceStatusE.ToString() + " ,";
                    }
                    if (previousInvoiceTO.InvoiceModeId != tblInvoiceTO.InvoiceModeId)
                    {
                        description += " Mode change from " + previousInvoiceTO.InvoiceModeE.ToString() + " to " + tblInvoiceTO.InvoiceModeE.ToString();
                    }


                    TblSupportDetailsTO tblSupportDetailsTO = new TblSupportDetailsTO();
                    tblSupportDetailsTO.ModuleId = 1;//By Defalult Module id is Set to Commercial;
                    tblSupportDetailsTO.Formid = Convert.ToInt32(Constants.SupportPageTypE.BILLING);
                    tblSupportDetailsTO.FromUser = fromUser;
                    tblSupportDetailsTO.CreatedBy = tblInvoiceTO.UpdatedBy;
                    tblSupportDetailsTO.CreatedOn = Constants.ServerDateTime;
                    tblSupportDetailsTO.Description = description;
                    tblSupportDetailsTO.RequireTime = 30;//HardCoded 30 Minutes;
                    tblSupportDetailsTO.Comments = Comments;

                    result = TblSupportDetailsBL.InsertTblSupportDetails(tblSupportDetailsTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Error in InsertTblSupportDetails");
                        return resultMessage;
                    }
                    tran.Commit();
                    resultMessage.Tag = description;
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "Error in UpdateInvoiceForSupport");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public static int UpdateTblSupportDetails(TblSupportDetailsTO tblSupportDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblSupportDetailsDAO.UpdateTblSupportDetails(tblSupportDetailsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblSupportDetails(Int32 idsupport)
        {
            return TblSupportDetailsDAO.DeleteTblSupportDetails(idsupport);
        }

        public static int DeleteTblSupportDetails(Int32 idsupport, SqlConnection conn, SqlTransaction tran)
        {
            return TblSupportDetailsDAO.DeleteTblSupportDetails(idsupport, conn, tran);
        }

        #endregion
    }
}
