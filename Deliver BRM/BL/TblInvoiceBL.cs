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
using static SalesTrackerAPI.StaticStuff.Constants;
using SalesTrackerAPI.IoT;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using Newtonsoft.Json.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;

namespace SalesTrackerAPI.BL
{
    public class TblInvoiceBL
    {
        private readonly ILogger loggerObj;

        public TblInvoiceBL(ILogger<TblInvoiceBL> logger)
        {
            loggerObj = logger;
            Constants.LoggerObj = logger;
        }

        #region Selection

        public static List<TblInvoiceAddressTO> SelectTblInvoiceAddressByDealerId(Int32 dealerOrgId, String txnAddrTypeId)
        {
            Int32 cnt = 0;
            String txnAddrTypeIdtemp = String.Empty;

            if (!String.IsNullOrEmpty(txnAddrTypeId))
            {
                txnAddrTypeIdtemp = txnAddrTypeId;
            }

            TblConfigParamsTO tblConfigParamsTO = TblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_EXISTING_ADDRESS_COUNT_FOR_BOOKING);
            if (tblConfigParamsTO != null)
            {
                cnt = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
            }

            List<TblInvoiceAddressTO> tblInvoiceDelAddrTOList = new List<TblInvoiceAddressTO>();
            try
            {
                List<TblInvoiceAddressTO> tblInvoiceDelAddrTOListtemp = new List<TblInvoiceAddressTO>();
                tblInvoiceDelAddrTOList = TblInvoiceAddressDAO.SelectTblInvoiceAddressByDealerId(dealerOrgId, txnAddrTypeIdtemp,cnt);
                if(tblInvoiceDelAddrTOList!=null && tblInvoiceDelAddrTOList.Count>0)
                {
                    tblInvoiceDelAddrTOList = tblInvoiceDelAddrTOList.GroupBy(g => new { g.BillingName, g.GstinNo,g.PanNo,g.AadharNo,g.ContactNo,g.Address,g.Taluka,g.District,g.State,g.VillageName,g.PinCode }).Select(s=>s.FirstOrDefault()).ToList();
                    if (cnt > tblInvoiceDelAddrTOList.Count)
                    {
                        tblInvoiceDelAddrTOListtemp = tblInvoiceDelAddrTOList;
                    }
                    else
                    {
                        for (int i = 0; i < cnt; i++)
                        {
                            TblInvoiceAddressTO tblInvoiceDelAddrTO = tblInvoiceDelAddrTOList[i];
                            tblInvoiceDelAddrTOListtemp.Add(tblInvoiceDelAddrTO);
                        }
                    }
                }
                return tblInvoiceDelAddrTOListtemp;
            }

            catch (Exception ex)
            {
                return null;
            }

        }

        public static List<TblInvoiceTO> SelectAllTblInvoiceList()
        {
            return TblInvoiceDAO.SelectAllTblInvoice();
        }
        /// <summary>
        /// Ramdas.w @24102017 this method is  Get Generated Invoice List
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="isConfirm"></param>
        /// <param name="cnfId"></param>
        /// <param name="dealerID"></param>
        /// <param name="userRoleTO"></param>
        /// <returns></returns>
        public static List<TblInvoiceTO> SelectAllTblInvoiceList(DateTime frmDt, DateTime toDt, int isConfirm, Int32 cnfId, Int32 dealerID, List<TblUserRoleTO> tblUserRoleTOList, string selectedOrg)
        {

            TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();
            if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0)
            {
                tblUserRoleTO = BL.TblUserRoleBL.SelectUserRoleTOAccToPriority(tblUserRoleTOList);
            }
            Int32 defualtOrgId = 0;

            //Hrushikesh changed as multiple internal Org changes

            //TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_INTERNALTXFER_INVOICE_ORG_ID);
            //if (tblConfigParamsTO != null)
            //{
            //    internalOrgId = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
            //}


            TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID);
            if (tblConfigParamsTO != null)
            {
                defualtOrgId = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
            }
            if(string.IsNullOrEmpty(selectedOrg))
            {
                selectedOrg = defualtOrgId.ToString();
            }

            List<TblInvoiceTO> list = TblInvoiceDAO.SelectAllTblInvoice(frmDt, toDt, isConfirm, cnfId, dealerID, tblUserRoleTO, selectedOrg, defualtOrgId);

            //SetGateIotDataToInvoiceTOV2(list);
            if (isConfirm == 1)
            {
                var nonAuthList = list.Where(n => n.InvoiceStatusE != InvoiceStatusE.AUTHORIZED).ToList();
                SetGateIotDataToInvoiceTOV2(nonAuthList);
            }
            else
            {
                //var nonAuthList = list.Where(n => n.LoadingStatusId != (int)TranStatusE.LOADING_DELIVERED).ToList();
                var nonAuthList = list.Where(n => n.LoadingStatusId != (int)TranStatusE.LOADING_DELIVERED).ToList();
                SetGateIotDataToInvoiceTOV2(nonAuthList);
            }
            list = list.Where(n => !String.IsNullOrEmpty(n.VehicleNo)).ToList();
            return list;
        }

        public static void SetGateIotDataToInvoiceTO(List<TblInvoiceTO> list)
        {
            if (Constants.getweightSourceConfigTO() == (int)Constants.WeighingDataSourceE.IoT)
            {
                var nonAuthList = list;//.Where(x => x.InvoiceStatusE != InvoiceStatusE.AUTHORIZED).ToList();
                if (nonAuthList != null)
                {
                    string commSepSlipIds = string.Empty;
                    for (int i = 0; i < nonAuthList.Count; i++)
                    {
                        commSepSlipIds += nonAuthList[i].LoadingSlipId + ",";
                    }
                    commSepSlipIds = commSepSlipIds.TrimEnd(',');
                    Dictionary<Int32, TblLoadingTO> loadingSlipModBusRefDCT = DAL.TblLoadingSlipDAO.SelectModbusRefIdByLoadingSlipIdDCT(commSepSlipIds);
                    if (loadingSlipModBusRefDCT != null)
                    {
                        foreach (var item in loadingSlipModBusRefDCT.Keys)
                        {
                            //Added By Vipul
                            TblLoadingTO tblLoadingTO = loadingSlipModBusRefDCT[item];

                            //End
                            //GateIoTResult gateIoTResult = IotCommunication.GetLoadingStatusHistoryDataFromGateIoT(loadingSlipModBusRefDCT[item]);
                            GateIoTResult gateIoTResult = IoT.GateCommunication.GetLoadingStatusHistoryDataFromGateIoT(tblLoadingTO);

                            if (gateIoTResult != null && gateIoTResult.Data != null && gateIoTResult.Data.Count > 0)
                            {
                                string vehicleNo = (string)gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.VehicleNo];
                                int transporterOrgId = Convert.ToInt32(gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.TransportorId]);
                                String transporterName = TblOrganizationBL.GetFirmNameByOrgId(transporterOrgId);

                                var invoiceList = nonAuthList.Where(inv => inv.LoadingSlipId == item).ToList();
                                if (invoiceList != null)
                                {
                                    invoiceList.ForEach(f => { f.VehicleNo = vehicleNo; f.TransportOrgId = transporterOrgId; f.TransporterName = transporterName; });
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void SetGateIotDataToInvoiceTOV2(List<TblInvoiceTO> list)
        {
            if (Constants.getweightSourceConfigTO() == (int)Constants.WeighingDataSourceE.IoT)
            {
                var nonAuthList = list;
                if (nonAuthList != null && nonAuthList.Count > 0)
                {
                    string commSepSlipIds = string.Empty;
                    for (int i = 0; i < nonAuthList.Count; i++)
                    {
                        commSepSlipIds += nonAuthList[i].LoadingSlipId + ",";
                    }
                    commSepSlipIds = commSepSlipIds.TrimEnd(',');
                    //dict of loadingslipId and ModRefId
                    //Dictionary<int, int> loadingSlipModBusRefDCT = DAL.TblLoadingSlipDAO.SelectModbusRefIdWrtLoadingSlipIdDCT(commSepSlipIds);

                    Dictionary<Int32, TblLoadingTO> loadingSlipModBusRefDCT = DAL.TblLoadingSlipDAO.SelectModbusRefIdByLoadingSlipIdDCT(commSepSlipIds);

                    if (loadingSlipModBusRefDCT != null)
                    {
                        List<TblLoadingTO> tblLoadingTOList = new List<TblLoadingTO>();
                        foreach (var item in loadingSlipModBusRefDCT)
                        {
                            tblLoadingTOList.Add(item.Value);
                        }

                        List<TblLoadingTO> distGate = tblLoadingTOList.GroupBy(g => g.GateId).Select(s => s.FirstOrDefault()).ToList();

                        GateIoTResult gateIoTResult = new GateIoTResult();

                        for (int g = 0; g < distGate.Count; g++)
                        {
                            TblLoadingTO tblLoadingTOTemp = distGate[g];
                            TblGateTO tblGateTO = new TblGateTO(tblLoadingTOTemp);
                            GateIoTResult temp = IoT.IotCommunication.GetLoadingSlipsByStatusFromIoTByStatusId("102", tblGateTO);

                            if (temp != null && temp.Data != null && temp.Data.Count > 0)
                            {
                                gateIoTResult.Data.AddRange(temp.Data);
                            }
                        }


                        //GateIoTResult gateIoTResult = IoT.IotCommunication.GetLoadingSlipsByStatusFromIoTByStatusId("102");

                        if (gateIoTResult != null && gateIoTResult.Data != null && gateIoTResult.Data.Count > 0)
                        {
                            foreach (var item in loadingSlipModBusRefDCT.Keys)
                            {
                                //int modRefId = loadingSlipModBusRefDCT[item];

                                TblLoadingTO tblLoadingTO = loadingSlipModBusRefDCT[item];
                                int modRefId = tblLoadingTO.ModbusRefId;

                                var data = gateIoTResult.Data.Where(w => Convert.ToInt32(w[0]) == modRefId).FirstOrDefault();
                                if (data == null)
                                    continue;
                                string vehicleNo = (string)data[(int)IoTConstants.GateIoTColE.VehicleNo];
                                //int transporterOrgId = Convert.ToInt32(data[(int)IoTConstants.GateIoTColE.TransportorId]);
                                //String transporterName = TblOrganizationBL.GetFirmNameByOrgId(transporterOrgId);

                                var invoiceList = nonAuthList.Where(inv => inv.LoadingSlipId == item).ToList();
                                if (invoiceList != null)
                                {
                                    //invoiceList.ForEach(f => { f.VehicleNo = vehicleNo; f.TransportOrgId = transporterOrgId; f.TransporterName = transporterName; });
                                    invoiceList.ForEach(f => { f.VehicleNo = vehicleNo; });
                                }
                            }
                        }
                    }
                }
            }
        }

        public static ResultMessage SetWeightIotDateToInvoiceTO(TblInvoiceTO tblInvoice, int IsExtractionAllowed)
        {
            ResultMessage resultMessage = new ResultMessage();
            double conversionFactor = 1000;
            int weightSourceConfigId = Constants.getweightSourceConfigTO();
            if (weightSourceConfigId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
            {
                if (tblInvoice.InvoiceModeE != InvoiceModeE.MANUAL_INVOICE)
                {
                    if (tblInvoice.LoadingSlipId != 0)
                    {
                        TblLoadingSlipTO tblLoadingSlipTO = new TblLoadingSlipTO();
                        if (IsExtractionAllowed == 0)
                        {
                            tblLoadingSlipTO = TblLoadingSlipBL.SelectAllLoadingSlipWithDetails(tblInvoice.LoadingSlipId);
                        }
                        else
                        {
                            tblLoadingSlipTO = TblLoadingSlipBL.SelectAllLoadingSlipWithDetailsForExtract(tblInvoice.LoadingSlipId);
                        }
                        for (int i = 0; i < tblInvoice.InvoiceItemDetailsTOList.Count; i++)
                        {
                            TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = tblInvoice.InvoiceItemDetailsTOList[i];

                            if (tblInvoiceItemDetailsTO.LoadingSlipExtId > 0)
                            {
                                TblLoadingSlipExtTO tblLoadingSlipExtTO = tblLoadingSlipTO.LoadingSlipExtTOList.Where(w => w.IdLoadingSlipExt == tblInvoiceItemDetailsTO.LoadingSlipExtId).FirstOrDefault();
                                if (tblLoadingSlipExtTO != null)
                                {
                                    tblInvoiceItemDetailsTO.Bundles = tblLoadingSlipExtTO.LoadedBundles;
                                    if (tblLoadingSlipExtTO.LoadedWeight > 0)
                                        tblInvoiceItemDetailsTO.InvoiceQty = tblLoadingSlipExtTO.LoadedWeight / conversionFactor;
                                }
                            }
                        }

                        //Saket [2019-05-27] Added round off and added LoadingSlipExtId > 0 conidtion.
                        tblInvoice.NetWeight = tblInvoice.InvoiceItemDetailsTOList.Where(w => w.LoadingSlipExtId > 0).Sum(s => s.InvoiceQty);
                        tblInvoice.NetWeight = tblInvoice.NetWeight * conversionFactor;
                        tblInvoice.NetWeight = Math.Round(tblInvoice.NetWeight, 2);

                        tblInvoice.GrossWeight = tblInvoice.TareWeight + tblInvoice.NetWeight;
                    }
                }
            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;

        }
        public static TblInvoiceTO SelectTblInvoiceTO(Int32 idInvoice)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblInvoiceDAO.SelectTblInvoice(idInvoice, conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// Ramdas.W:@22092017:API This method is used to Get List of Invoice By Status
        /// </summary>
        /// <param name="StatusId"></param>
        /// <param name="distributorOrgId"></param>
        /// <returns></returns>
        public static List<TblInvoiceTO> SelectTblInvoiceByStatus(int statusId, int distributorOrgId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                List<TblInvoiceTO> tblInvoiceTOList = TblInvoiceDAO.SelectTblInvoiceByStatus(statusId, distributorOrgId, conn, tran);

                SetGateIotDataToInvoiceTOV2(tblInvoiceTOList);

                return tblInvoiceTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public static TblInvoiceTO SelectTblInvoiceTOWithDetails(Int32 idInvoice, SqlConnection conn, SqlTransaction tran)
        {
            TblInvoiceTO invoiceTO = TblInvoiceDAO.SelectTblInvoice(idInvoice, conn, tran);
            if (invoiceTO != null)
            {
                invoiceTO.InvoiceItemDetailsTOList = BL.TblInvoiceItemDetailsBL.SelectAllTblInvoiceItemDetailsList(invoiceTO.IdInvoice, conn, tran);
                for (int i = 0; i < invoiceTO.InvoiceItemDetailsTOList.Count; i++)
                {
                    invoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList = BL.TblInvoiceItemTaxDtlsBL.SelectAllTblInvoiceItemTaxDtlsList(invoiceTO.InvoiceItemDetailsTOList[i].IdInvoiceItem, conn, tran);
                }
                invoiceTO.InvoiceAddressTOList = BL.TblInvoiceAddressBL.SelectAllTblInvoiceAddressList(invoiceTO.IdInvoice, conn, tran);
                //invoiceTO.InvoiceTaxesTOList = BL.TblInvoiceTaxesBL.SelectAllTblInvoiceTaxesList(invoiceTO.IdInvoice, conn, tran);
            }

            SetGateAndWeightIotData(invoiceTO, 0);

            return invoiceTO;
        }
        public static TblInvoiceTO GetInvoiceDetails(Int32 invoiceId, int IsExtractionAllowed)
        {
            try
            {
                Constants.writeLog("GetInvoiceDetails : For Id - " + invoiceId + " Start ");

                string historyDetails = string.Empty;
                TblInvoiceTO invoiceTO = BL.TblInvoiceBL.SelectTblInvoiceTO(invoiceId);
                if (invoiceTO != null)
                {
                    invoiceTO.InvoiceAddressTOList = BL.TblInvoiceAddressBL.SelectAllTblInvoiceAddressList(invoiceId);
                    List<TblInvoiceItemDetailsTO> itemList = BL.TblInvoiceItemDetailsBL.SelectAllTblInvoiceItemDetailsList(invoiceId);
                    if (itemList != null)
                    {
                        for (int i = 0; i < itemList.Count; i++)
                        {
                            itemList[i].InvoiceItemTaxDtlsTOList = BL.TblInvoiceItemTaxDtlsBL.SelectAllTblInvoiceItemTaxDtlsList(itemList[i].IdInvoiceItem);

                        }
                        invoiceTO.InvoiceItemDetailsTOList = itemList;
                        /*GJ@20170929 : To get the History Details for Approval and Acceptance*/
                        //if (invoiceTO.StatusId == (int)Constants.InvoiceStatusE.PENDING_FOR_AUTHORIZATION || invoiceTO.StatusId == (int)Constants.InvoiceStatusE.PENDING_FOR_ACCEPTANCE)
                        //{

                        Constants.writeLog("GetInvoiceDetails : For Id - " + invoiceId + " END ");
                        Constants.writeLog("GetInvoiceDetails : For Id - " + invoiceId + "  Start For IOT ");

                        //Saket [2018-12-14] Added
                        if (invoiceTO.InvoiceModeE != InvoiceModeE.MANUAL_INVOICE)
                        {
                            //Sanjay [30-May-2019] Conditions added for auth invoice. If type is firm and auth then data wil be written to DB else on IoT
                            if (invoiceTO.IsConfirmed == 0)
                                BL.TblInvoiceBL.SetGateAndWeightIotData(invoiceTO, IsExtractionAllowed);
                            else if (invoiceTO.InvoiceStatusE != InvoiceStatusE.AUTHORIZED && invoiceTO.InvoiceStatusE != InvoiceStatusE.CANCELLED)
                                BL.TblInvoiceBL.SetGateAndWeightIotData(invoiceTO, IsExtractionAllowed);

                        }

                        Constants.writeLog("GetInvoiceDetails : For Id - " + invoiceId + "  END For IOT ");
                        Constants.writeLog("GetInvoiceDetails : For Id - " + invoiceId + "  Start Processing and Mapping ");


                        //BL.TblInvoiceBL.SetWeightIotDateToInvoiceTO(invoiceTO);

                        //Saket [2017-11-21]
                        String strProdGstCode = String.Join(",", invoiceTO.InvoiceItemDetailsTOList.Select(s => s.ProdGstCodeId.ToString()).ToArray());

                        List<TblProdGstCodeDtlsTO> tblProdGstCodeDtlsTOList = BL.TblProdGstCodeDtlsBL.SelectTblProdGstCodeDtlsTOList(strProdGstCode);

                        //List <TblGstCodeDtlsTO> tblGstCodeDtlsTOList = BL.TblGstCodeDtlsBL.SelectTblGstCodeDtlsTOList(strProdGstCode);

                        for (int p = 0; p < itemList.Count; p++)
                        {

                            TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO = tblProdGstCodeDtlsTOList.Where(w => w.IdProdGstCode == itemList[p].ProdGstCodeId).FirstOrDefault();
                            if (tblProdGstCodeDtlsTO != null)
                            {
                                if (tblProdGstCodeDtlsTO.GstCodeId > 0)
                                {
                                    itemList[p].GstCodeDtlsTO = BL.TblGstCodeDtlsBL.SelectTblGstCodeDtlsTO(tblProdGstCodeDtlsTO.GstCodeId);
                                    itemList[p].GstCodeDtlsTO.TaxRatesTOList = BL.TblTaxRatesBL.SelectAllTblTaxRatesList(tblProdGstCodeDtlsTO.GstCodeId);
                                }
                            }
                        }

                        List<TblInvoiceHistoryTO> InvoiceHistoryTOList = new List<TblInvoiceHistoryTO>();
                        InvoiceHistoryTOList = BL.TblInvoiceHistoryBL.SelectAllTblInvoiceHistoryById(invoiceTO.IdInvoice, true);
                        if (InvoiceHistoryTOList != null && InvoiceHistoryTOList.Count > 0)
                        {
                            for (int i = 0; i < InvoiceHistoryTOList.Count; i++)
                            {
                                string editedBy = string.Empty;
                                TblInvoiceHistoryTO element = InvoiceHistoryTOList[i];
                                TblUserTO tblUserTo = BL.TblUserBL.SelectTblUserTO(element.CreatedBy);
                                if (tblUserTo != null)
                                {
                                    editedBy = tblUserTo.UserDisplayName;
                                }
                                if (element.OldBillingAddr != null && element.NewBillingAddr != null)
                                {
                                    if (string.IsNullOrEmpty(historyDetails))
                                    {
                                        historyDetails = "Billing Address " + "!" + editedBy + "!" + element.OldBillingAddr + "!" + element.NewBillingAddr;
                                    }
                                    else
                                    {
                                        historyDetails += "::" + "Billing Address " + "!" + editedBy + "!" + element.OldBillingAddr + "!" + element.NewBillingAddr;
                                    }
                                }
                                if (element.OldConsinAddr != null && element.NewConsinAddr != null)
                                {
                                    if (string.IsNullOrEmpty(historyDetails))
                                    {
                                        historyDetails = "Consignee Address " + "!" + editedBy + "!" + element.OldConsinAddr + "!" + element.NewConsinAddr;
                                    }
                                    else
                                    {
                                        historyDetails += "::" + "Consignee Address " + "!" + editedBy + "!" + element.OldConsinAddr + "!" + element.NewConsinAddr;
                                    }
                                }
                            }

                            for (int i = 0; i < itemList.Count; i++)
                            {
                                //InvoiceHistoryTOList = BL.TblInvoiceHistoryBL.SelectAllTblInvoiceHistoryById(itemList[i].IdInvoiceItem);
                                List<TblInvoiceHistoryTO> filteredInvoiceList = InvoiceHistoryTOList.Where(p => p.InvoiceItemId == itemList[i].IdInvoiceItem).ToList();

                                if (filteredInvoiceList != null && filteredInvoiceList.Count > 0)
                                {
                                    for (int j = 0; j < filteredInvoiceList.Count; j++)
                                    {
                                        itemList[i].ChangeIn = "Rate";
                                        TblInvoiceHistoryTO element = filteredInvoiceList[j];
                                        string editedBy = string.Empty;
                                        TblUserTO tblUserTo = BL.TblUserBL.SelectTblUserTO(element.CreatedBy);
                                        if (tblUserTo != null)
                                        {
                                            editedBy = tblUserTo.UserDisplayName;
                                        }
                                        //historyDetails = string.IsNullOrEmpty(historyDetails)? 
                                        if (element.OldUnitRate != 0 && element.NewUnitRate != 0)
                                        {
                                            itemList[i].ChangeIn = itemList[i].ChangeIn == "" || string.IsNullOrEmpty(itemList[i].ChangeIn) ? "Rate" : itemList[i].ChangeIn + "|" + "Rate";
                                            if (string.IsNullOrEmpty(historyDetails))
                                            {
                                                historyDetails = itemList[i].ProdItemDesc + " (Rate)" + "!" + editedBy + "!" + element.OldUnitRate + "!" + element.NewUnitRate;
                                            }
                                            else
                                            {
                                                historyDetails += "::" + itemList[i].ProdItemDesc + " (Rate)" + "!" + editedBy + "!" + element.OldUnitRate + "!" + element.NewUnitRate;
                                            }
                                        }
                                        if (element.OldCdStructureId != 0 && element.NewCdStructureId != 0)
                                        {
                                            List<DropDownTO> cdStructureList = BL.DimensionBL.SelectCDStructureForDropDown();
                                            var vOldRes = cdStructureList.Where(p => p.Value == element.OldCdStructureId).ToList();
                                            var vNewRes = cdStructureList.Where(p => p.Value == element.NewCdStructureId).ToList();
                                            itemList[i].ChangeIn = itemList[i].ChangeIn == "" || string.IsNullOrEmpty(itemList[i].ChangeIn) ? "CD" : itemList[i].ChangeIn + "|" + "CD";

                                            if (string.IsNullOrEmpty(historyDetails))
                                            {
                                                historyDetails = itemList[i].ProdItemDesc + " (CD)" + "!" + editedBy + "!" + (vOldRes.Count > 0 ? vOldRes[0].Text : "0") + "!" + (vNewRes.Count > 0 ? vNewRes[0].Text : "0");
                                            }
                                            else
                                            {
                                                historyDetails += "::" + itemList[i].ProdItemDesc + " (CD)" + "!" + editedBy + "!" + (vOldRes.Count > 0 ? vOldRes[0].Text : "0") + "!" + (vNewRes.Count > 0 ? vNewRes[0].Text : "0");
                                            }
                                        }
                                    }
                                }

                            }
                        }
                        // }

                        Constants.writeLog("GetInvoiceDetails : For Id - " + invoiceId + "  END Processing and Mapping ");

                    }

                    invoiceTO.HistoryDetails = historyDetails;
                }

                return invoiceTO;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }

        }
        public static List<TblInvoiceTO> SelectInvoiceTOFromLoadingSlipId(Int32 loadingSlipId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblInvoiceDAO.SelectInvoiceTOFromLoadingSlipId(loadingSlipId, conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public static List<TblInvoiceTO> SelectInvoiceTOFromLoadingSlipId(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceDAO.SelectInvoiceTOFromLoadingSlipId(loadingSlipId, conn, tran);
        }

        public static List<TblInvoiceTO> SelectInvoiceListFromLoadingSlipId(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceDAO.SelectInvoiceListFromLoadingSlipId(loadingSlipId, conn, tran);
        }

        /// <summary>
        /// Vijaymala[15-09-2017] Added To Get Invoice List To Generate Report
        /// </summary>
        /// <returns></returns>
        public static List<TblInvoiceRptTO> SelectAllRptInvoiceList(DateTime frmDt, DateTime toDt, int isConfirm,string strOrgTempId)
        {
            return TblInvoiceDAO.SelectAllRptInvoiceList(frmDt, toDt, isConfirm, strOrgTempId);
        }


        
        /// <summary>
        /// Vijaymala[06-10-2017] Added To Get Invoice List To Generate Invoice Excel
        /// </summary>
        /// <returns></returns>
        public static List<TblInvoiceRptTO> SelectInvoiceExportList(DateTime frmDt, DateTime toDt, int isConfirm, string strOrgTempId)
        {
            return TblInvoiceDAO.SelectInvoiceExportList(frmDt, toDt, isConfirm, strOrgTempId);
        }

        /// <summary>
        /// Vijaymala[07-10-2017] Added To Get Invoice List To Generate Invoice Excel
        /// </summary>
        /// <returns></returns>
        public static List<TblInvoiceRptTO> SelectHsnExportList(DateTime frmDt, DateTime toDt, int isConfirm,string strOrgTempId)
        {
            return TblInvoiceDAO.SelectHsnExportList(frmDt, toDt, isConfirm, strOrgTempId);
        }

        /// <summary>
        /// Vijaymala[11-01-2018] Added To Get Sales Invoice List To Generate Report
        /// </summary>
        /// <returns></returns>
        public static List<TblInvoiceRptTO> SelectSalesInvoiceListForReport(DateTime frmDt, DateTime toDt, int isConfirm, string selectedOrg, int isManual)
        {
            Int32 defualtOrgId = 0;
            //TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID);
            //if (tblConfigParamsTO != null)
            //{
            //    defualtOrgId = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
            //}
            return TblInvoiceDAO.SelectSalesInvoiceListForReport(frmDt, toDt, isConfirm, selectedOrg, defualtOrgId, isManual);
        }

        //Deepali Added [07-06-2021] for task no 1145
        public static ResultMessage PrintSaleReport(DateTime frmDt, DateTime toDt, int isConfirm, string selectedOrg,int isFromPurchase=0)
        {
            Int32 defualtOrgId = 0;
            ResultMessage resultMessage = new ResultMessage();
            List <TblInvoiceRptTO> TblReportsTOList = TblInvoiceDAO.SelectSalesPurchaseListForReport(frmDt, toDt, isConfirm, selectedOrg, defualtOrgId, isFromPurchase);

            if (TblReportsTOList!= null && TblReportsTOList.Count >0)
            {
                DataSet printDataSet = new DataSet();
                DataTable headerDT = new DataTable();
                if (TblReportsTOList != null && TblReportsTOList.Count > 0)
                {
                    headerDT = CommonDAO.ToDataTable(TblReportsTOList);
                }
                headerDT.TableName = "headerDT";

                printDataSet.Tables.Add(headerDT);
                String ReportTemplateName = "SalesReportRpt";
                if (isFromPurchase == 1)
                {
                   ReportTemplateName = "PurchaseReportRpt";
                }

                String templateFilePath = CommonDAO.SelectReportFullName(ReportTemplateName);
                String fileName = "Doc-" + DateTime.Now.Ticks;
                String saveLocation = AppDomain.CurrentDomain.BaseDirectory + fileName + ".xls";
                Boolean IsProduction = true;

                TblConfigParamsTO tblConfigParamsTO = TblConfigParamsDAO.SelectTblConfigParamsValByName("IS_PRODUCTION_ENVIRONMENT_ACTIVE");
                if (tblConfigParamsTO != null)
                {
                    if (Convert.ToInt32(tblConfigParamsTO.ConfigParamVal) == 0)
                    {
                        IsProduction = false;
                    }
                }
                resultMessage = CommonDAO.GenrateMktgInvoiceReport(printDataSet, templateFilePath, saveLocation, Constants.ReportE.EXCEL_DONT_OPEN, IsProduction);
                if (resultMessage.MessageType == ResultMessageE.Information)
                {
                    String filePath = String.Empty;
                    if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(String))
                    {
                        filePath = resultMessage.Tag.ToString();
                    }
                    //driveName + path;
                    int returnPath = 0;
                    if (returnPath != 1)
                    {
                        String fileName1 = Path.GetFileName(saveLocation);
                        Byte[] bytes = File.ReadAllBytes(filePath);
                        if (bytes != null && bytes.Length > 0)
                        {
                            resultMessage.Tag = bytes;

                            string resFname = Path.GetFileNameWithoutExtension(saveLocation);
                            string directoryName;
                            directoryName = Path.GetDirectoryName(saveLocation);
                            string[] fileEntries = Directory.GetFiles(directoryName, "*Doc*");
                            string[] filesList = Directory.GetFiles(directoryName, "*Doc*");

                            foreach (string file in filesList)
                            {
                                //if (file.ToUpper().Contains(resFname.ToUpper()))
                                {
                                    File.Delete(file);
                                }
                            }
                        }

                        if (resultMessage.MessageType == ResultMessageE.Information)
                        {
                           return resultMessage;
                        }
                    }

                }
                else
                {
                    resultMessage.Text = "Something wents wrong please try again";
                    resultMessage.DisplayMessage = "Something wents wrong please try again";
                    resultMessage.Result = 0;
                }
            }
            else
            {
                resultMessage.DefaultBehaviour();
                return resultMessage;

            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }

        // Vaibhav [14-Nov-2017] added to select invoice details by loading id
        public static List<TblInvoiceTO> SelectTempInvoiceTOList(Int32 loadingSlipId)
        {
            return TblInvoiceDAO.SelectAllTempInvoice(loadingSlipId);
        }

        // Vaibhav [14-Nov-2017] added to select invoice details by loading id
        public static List<TblInvoiceTO> SelectTempInvoiceTOList(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceDAO.SelectAllTempInvoice(loadingSlipId, conn, tran);
        }

        public static List<TblInvoiceTO> SelectInvoiceTOListFromLoadingSlipId(Int32 loadingSlipId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblInvoiceDAO.SelectInvoiceListFromLoadingSlipId(loadingSlipId, conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Vijaymala added [09-05-2018]:To get notified invoices list
        /// </summary>
        /// <returns></returns>
        public static List<TblInvoiceTO> SelectAllTNotifiedblInvoiceList(DateTime frmDt, DateTime toDt, int isConfirm,string strOrgTempId)
        {
            return TblInvoiceDAO.SelectAllTNotifiedblInvoiceList(frmDt, toDt, isConfirm, strOrgTempId);
        }

        #endregion

        #region Insertion
        /// <summary>
        /// RW:14092017:API This Methos is used to Add new Invoice 
        /// </summary>
        /// <param name="tblInvoiceTO"></param>
        /// <returns></returns>
        public static ResultMessage InsertTblInvoice(TblInvoiceTO tblInvoiceTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            resultMessage.Text = "Not Entered In The Loop";
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                /*GJ@20170927 : For get RCM and pass to Invoice*/
                TblConfigParamsTO rcmConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_REVERSE_CHARGE_MECHANISM, conn, tran);
                if (rcmConfigParamsTO == null)
                {
                    tran.Commit();
                    resultMessage.DefaultBehaviour("RCM value Not Found in Configuration.");
                    return resultMessage;
                }
                resultMessage = SaveNewInvoice(tblInvoiceTO, conn, tran);
                if (resultMessage.MessageType == ResultMessageE.Information && resultMessage.Result == 1)
                {
                    tran.Commit();
                    resultMessage.DefaultSuccessBehaviour();
                }
                else
                {
                    tran.Rollback();

                }
                return resultMessage;
            }
            catch (Exception ex)
            {
                if (tran.Connection.State == ConnectionState.Open)
                    tran.Rollback();

                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblInvoice");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }


        }


        public static ResultMessage exchangeInvoice(Int32 invoiceId, Int32 invGenModeId, int fromOrgId, int toOrgId, int isCalculateWithBaseRate)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            DateTime serverDate = Constants.ServerDateTime;

            string entityRangeString = "REGULAR_TAX_INVOICE_";
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                TblInvoiceTO invoiceTO = TblInvoiceDAO.SelectTblInvoice(invoiceId, conn, tran);
                if (invoiceTO == null)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("invoiceTO Found NULL"); return resultMessage;
                }

                if (invoiceTO.InvFromOrgFreeze == 1)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "Invoie Already Converted";
                    return resultMessage;
                }

                //Vijaymala[23-03-2016]added to check invoice details of igst,cgst,sgst taxes
                #region To check invoice details is valid or not
                string errorMsg = string.Empty;
                //Boolean isValidInvoice = CheckInvoiceDetailsAccToState(invoiceTO, ref errorMsg);
                //if (!isValidInvoice)
                //{
                //    resultMessage.DefaultBehaviour(errorMsg);
                //    resultMessage.DisplayMessage = errorMsg;
                //    return resultMessage;
                //}
                #endregion

                TblInvoiceChangeOrgHistoryTO invHistTO = new TblInvoiceChangeOrgHistoryTO();
                invHistTO.InvoiceId = invoiceTO.IdInvoice;
                invHistTO.CreatedOn = Constants.ServerDateTime;
                invHistTO.CreatedBy = 1;
                if (invGenModeId == (int)Constants.InvoiceGenerateModeE.DUPLICATE)
                {
                    invHistTO.ActionDesc = "Duplicate";
                }
                if (invGenModeId == (int)Constants.InvoiceGenerateModeE.CHANGEFROM)
                {
                    invHistTO.ActionDesc = "Change Organization";
                }
                resultMessage = PrepareAndSaveInternalTaxInvoices(invoiceTO, invGenModeId, fromOrgId, toOrgId, isCalculateWithBaseRate, invHistTO, conn, tran);
                int res = TblInvoiceChangeOrgHistoryDAO.InsertTblInvoiceChangeOrgHistory(invHistTO, conn, tran);

                if (res != 1)
                {
                    tran.Rollback();
                    resultMessage.Text = "failed to save History in exchangeInvoice";
                    return resultMessage;
                }
                if (resultMessage.MessageType == ResultMessageE.Information)
                {
                    tran.Commit();
                    resultMessage.Text = "Invoice Converted Successfully";
                    resultMessage.DisplayMessage = "Invoice Converted Successfully";
                    return resultMessage;
                }
                else
                {
                    tran.Rollback();
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "GenerateInvoiceNumber");
                return resultMessage;
            }

            finally
            {
                conn.Close();
            }
        }
        public static ResultMessage PostUpdateInvoiceStatus(TblInvoiceTO tblInvoiceTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                tblInvoiceTO.UpdatedOn = Constants.ServerDateTime;
                result = TblInvoiceDAO.PostUpdateInvoiceStatus(tblInvoiceTO);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While Update tempInvoice");
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateInvoiceStatus");
                return resultMessage;
            }
            finally
            {
                
            }
        }
        public static ResultMessage SaveNewInvoice(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            int result = 0;
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            resultMessage.Text = "Not Entered In The Loop";
            try
            {
                #region 1. Save the New Invoice

                DimFinYearTO curFinYearTO = DimensionBL.GetCurrentFinancialYear(tblInvoiceTO.CreatedOn, conn, tran);
                if (curFinYearTO == null)
                {
                    resultMessage.DefaultBehaviour("Current Fin Year Object Not Found");
                    return resultMessage;
                }

                tblInvoiceTO.FinYearId = curFinYearTO.IdFinYear;
                if (tblInvoiceTO.InvoiceModeE == Constants.InvoiceModeE.MANUAL_INVOICE)
                {
                    tblInvoiceTO.IsConfirmed = 1;
                }

                if (tblInvoiceTO.InvFromOrgId == 0)
                {
                    TblConfigParamsTO paramTO = TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID, conn, tran);
                    if (paramTO != null)
                        tblInvoiceTO.InvFromOrgId = Convert.ToInt32(paramTO.ConfigParamVal);
                }

                result = InsertTblInvoice(tblInvoiceTO, conn, tran);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error in Insert InvoiceTbl");
                    return resultMessage;
                }
                #endregion

                #region 2. Save the Address Details 
                if (tblInvoiceTO.InvoiceAddressTOList == null || tblInvoiceTO.InvoiceAddressTOList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("Error : Invoce Item Address Det List Found Empty Or Null");
                    return resultMessage;
                }
                for (int i = 0; i < tblInvoiceTO.InvoiceAddressTOList.Count; i++)
                {
                    tblInvoiceTO.InvoiceAddressTOList[i].InvoiceId = tblInvoiceTO.IdInvoice;
                    result = TblInvoiceAddressBL.InsertTblInvoiceAddress(tblInvoiceTO.InvoiceAddressTOList[i], conn, tran);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error in Insert InvoiceAddressDetailTbl");
                        return resultMessage;
                    }

                }
                #endregion


                #region 3. Save the Invoice Item Details
                if (tblInvoiceTO.InvoiceItemDetailsTOList == null || tblInvoiceTO.InvoiceItemDetailsTOList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("Error : Invoce Item Det List Found Empty Or Null");
                    return resultMessage;
                }
                for (int i = 0; i < tblInvoiceTO.InvoiceItemDetailsTOList.Count; i++)
                {
                    TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = new TblInvoiceItemDetailsTO();
                    tblInvoiceItemDetailsTO = tblInvoiceTO.InvoiceItemDetailsTOList[i];
                    tblInvoiceItemDetailsTO.InvoiceId = tblInvoiceTO.IdInvoice;

                    result = TblInvoiceItemDetailsBL.InsertTblInvoiceItemDetails(tblInvoiceItemDetailsTO, conn, tran);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error in Insert InvoiceItemDetailTbl");
                        return resultMessage;
                    }
                    #region 1. Save the Invoice Tax Item Details
                    if (tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList == null
                        || tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList.Count == 0)
                    {
                        if (tblInvoiceTO.InvoiceTypeE == Constants.InvoiceTypeE.REGULAR_TAX_INVOICE
                            || tblInvoiceTO.InvoiceTypeE == Constants.InvoiceTypeE.SEZ_WITH_DUTY)
                        {
                            resultMessage.DefaultBehaviour("Error : Invoice Item Det Tax List Found Empty Or Null");
                            return resultMessage;
                        }
                    }

                    if (tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList != null)
                    {
                        for (int j = 0; j < tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList.Count; j++)
                        {
                            tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList[j].InvoiceItemId = tblInvoiceTO.InvoiceItemDetailsTOList[i].IdInvoiceItem;
                            result = TblInvoiceItemTaxDtlsBL.InsertTblInvoiceItemTaxDtls(tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList[j], conn, tran);
                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error in Insert InvoiceItemTaxDetailTbl");
                                return resultMessage;
                            }
                        }
                    }
                    #endregion
                }
                #endregion


                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblInvoice");
                return resultMessage;
            }
            finally
            {

            }

        }

        public static int InsertTblInvoice(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {

            return TblInvoiceDAO.InsertTblInvoice(tblInvoiceTO, conn, tran);
        }

        /// <summary>
        /// Saket [2018-11-05] Added
        /// </summary>
        /// <param name="loadingSlipIds"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static List<TblInvoiceTO> SelectInvoiceListFromLoadingSlipIds(String loadingSlipIds, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceDAO.SelectInvoiceListFromLoadingSlipIds(loadingSlipIds, conn, tran);
        }
        public static Double CalculateTDS(TblInvoiceTO tblInvoiceTO)
        {
            Double TdsAmt = 0;
            List<TblOtherTaxesTO> TblOtherTaxesTOList = DAL.TblOtherTaxesDAO.SelectAllTblOtherTaxes();
            if (tblInvoiceTO != null)
            {
                if (tblInvoiceTO.InvoiceItemDetailsTOList != null && tblInvoiceTO.InvoiceItemDetailsTOList.Count > 0)
                {
                    tblInvoiceTO.InvoiceItemDetailsTOList.ForEach(element =>
                    {
                        if(element.OtherTaxId == 0)
                        {
                            TdsAmt += element.TaxableAmt;
                        }
                        else if(element.OtherTaxId > 0 && TblOtherTaxesTOList != null && TblOtherTaxesTOList.Count > 0)
                        {
                            var matchTO = TblOtherTaxesTOList.Where(w => w.IdOtherTax == element.OtherTaxId).FirstOrDefault();
                            if (matchTO != null)
                            {
                                if(matchTO.IsBefore == 1)
                                {
                                    TdsAmt += element.TaxableAmt;
                                }
                            }
                        }
                    });
                }
            }
            return TdsAmt;
        }
        public static ResultMessage PrepareAndSaveNewTaxInvoice(TblLoadingTO loadingTO, List<TblLoadingSlipExtTO> lastItemList, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMsg = new ResultMessage();
            string entityRangeName = string.Empty;

            /*GJ@20170915 :  Default Weighing is done in  KG UOM , hence we need to convert it MT while we assign this*/
            double conversionFactor = 0.001;
            List<DropDownTO> districtList = new List<DropDownTO>();
            List<DropDownTO> talukaList = new List<DropDownTO>();
            try
            {
                DateTime serverDateTime = Constants.ServerDateTime;
                Int32 billingStateId = 0;
                List<TblLoadingSlipTO> loadingSlipTOList = BL.TblLoadingSlipBL.SelectAllLoadingSlipListWithDetails(loadingTO.IdLoading, conn, tran);
                TblOrganizationTO tblOrganizationTO = new TblOrganizationTO();

                loadingTO.LoadingSlipList = loadingSlipTOList;

                int confiqId = Constants.getweightSourceConfigTO();

                if (confiqId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
                {
                    IoT.IotCommunication.GetItemDataFromIotAndMerge(loadingTO, true);

                    List<TblLoadingSlipExtTO> allItem = new List<TblLoadingSlipExtTO>();

                    for (int j = 0; j < loadingSlipTOList.Count; j++)
                    {
                        allItem.AddRange(loadingSlipTOList[j].LoadingSlipExtTOList);
                    }

                    for (int j = 0; j < lastItemList.Count; j++)
                    {
                        TblLoadingSlipExtTO item = lastItemList[j];
                        TblLoadingSlipExtTO var = allItem.Where(w => w.IdLoadingSlipExt == item.IdLoadingSlipExt).FirstOrDefault();

                        var.LoadedBundles = item.LoadedBundles;
                        var.LoadedWeight = item.LoadedWeight;
                        var.CalcTareWeight = item.CalcTareWeight;
                    }

                    var emptyItem = lastItemList.Where(w => w.LoadedWeight <= 0).ToList();
                    if (emptyItem != null && emptyItem.Count > 0)
                    {
                        resultMsg.DefaultBehaviour("Weight Not Found Against " + emptyItem.Count + " Item ");
                        return resultMsg;
                    }
                }

                #region Check if invoice is already generated

                //Saket [2018-02-15] Added

                if (loadingSlipTOList != null && loadingSlipTOList.Count > 0)
                {
                    String loadingSlipIds = String.Join(',', loadingSlipTOList.Select(s => s.IdLoadingSlip.ToString()).ToArray());
                    if (!String.IsNullOrEmpty(loadingSlipIds))
                    {
                        List<TblInvoiceTO> tblInvoiceTOListTemp = SelectInvoiceListFromLoadingSlipIds(loadingSlipIds, conn, tran);
                        if (tblInvoiceTOListTemp != null && tblInvoiceTOListTemp.Count > 0)
                        {
                            for (int t = 0; t < tblInvoiceTOListTemp.Count; t++)
                            {
                                loadingSlipTOList = loadingSlipTOList.Where(w => w.IdLoadingSlip != tblInvoiceTOListTemp[t].LoadingSlipId).ToList();
                            }
                        }

                    }
                }

                #endregion


                TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID, conn, tran);
                if (tblConfigParamsTO == null)
                {
                    resultMsg.DefaultBehaviour("Internal Self Organization Not Found in Configuration.");
                    return resultMsg;
                }
                Int32 internalOrgId = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
                TblAddressTO ofcAddrTO = BL.TblAddressBL.SelectOrgAddressWrtAddrType(internalOrgId, Constants.AddressTypeE.OFFICE_ADDRESS, conn, tran);
                if (ofcAddrTO == null)
                {
                    resultMsg.DefaultBehaviour("Address Not Found For Self Organization.");
                    return resultMsg;
                }
                /*GJ@20170927 : For get RCM and pass to Invoice*/
                TblConfigParamsTO rcmConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_REVERSE_CHARGE_MECHANISM, conn, tran);
                if (rcmConfigParamsTO == null)
                {
                    resultMsg.DefaultBehaviour("RCM value Not Found in Configuration.");
                    return resultMsg;
                }

                if (loadingSlipTOList == null && loadingSlipTOList.Count == 0)
                {
                    resultMsg.DefaultBehaviour("Loading Slip list Found Null.");
                    return resultMsg;
                }

                #region Prepare List Of Invoices To Save

                List<TblInvoiceTO> tblInvoiceTOList = new List<TblInvoiceTO>();
                foreach (var loadingSlipTo in loadingSlipTOList)
                {
                    TblInvoiceTO tblInvoiceTO = new TblInvoiceTO();
                    tblInvoiceTO.InvoiceAddressTOList = new List<TblInvoiceAddressTO>();
                    tblInvoiceTO.InvoiceItemDetailsTOList = new List<TblInvoiceItemDetailsTO>();
                    double grandTotal = 0;
                    double discountTotal = 0;
                    double igstTotal = 0;
                    double cgstTotal = 0;
                    double sgstTotal = 0;
                    double basicTotal = 0;
                    double taxableTotal = 0;
                    Boolean isPanNoPresent = false;
                    List<TblInvoiceItemDetailsTO> tblInvoiceItemDetailsTOList = new List<TblInvoiceItemDetailsTO>();

                    #region 1 Preparing main InvoiceTO
                    tblInvoiceTO.RcmFlag = Convert.ToInt32(rcmConfigParamsTO.ConfigParamVal);
                    tblInvoiceTO.CurrencyId = loadingTO.CurrencyId;
                    tblInvoiceTO.CurrencyRate = loadingTO.CurrencyRate;
                    if (loadingTO.CurrencyId == 0 || loadingTO.CurrencyRate == 0)
                    {
                        tblInvoiceTO.CurrencyId = Constants.DefaultCurrencyID;
                        tblInvoiceTO.CurrencyRate = Constants.DefaultCurrencyRate;
                    }

                    //It will be based on confirm not confirm. Hence commented and added at the end
                    //tblInvoiceTO.FreightAmt = loadingTO.FreightAmt;
                    tblInvoiceTO.VehicleNo = loadingTO.VehicleNo;
                    tblInvoiceTO.Narration = loadingTO.CnfOrgName;
                    //tblInvoiceTO.InvFromOrgId = internalOrgId; //No need to aasign from loading Only use for BMM

                    tblInvoiceTO.InvFromOrgId = loadingTO.FromOrgId;
                    tblInvoiceTO.CreatedOn = Constants.ServerDateTime;
                    tblInvoiceTO.CreatedBy = loadingTO.CreatedBy;
                    tblInvoiceTO.DistributorOrgId = loadingTO.CnfOrgId;
                    tblInvoiceTO.DealerOrgId = loadingSlipTo.DealerOrgId;
                    tblInvoiceTO.InvoiceDate = Constants.ServerDateTime;
                    tblInvoiceTO.InvoiceModeE = Constants.InvoiceModeE.AUTO_INVOICE;
                    tblInvoiceTO.InvoiceStatusE = Constants.InvoiceStatusE.NEW;
                    tblInvoiceTO.LoadingSlipId = loadingSlipTo.IdLoadingSlip;
                    tblInvoiceTO.StatusDate = tblInvoiceTO.InvoiceDate;
                    tblInvoiceTO.TransportOrgId = loadingTO.TransporterOrgId;
                    tblInvoiceTO.InvoiceTypeE = Constants.InvoiceTypeE.REGULAR_TAX_INVOICE;
                    tblInvoiceTO.IsConfirmed = loadingSlipTo.IsConfirmed;
                    if (loadingTO.CallFlag == 1)
                        tblInvoiceTO.InvoiceModeE = Constants.InvoiceModeE.AUTO_INVOICE_EDIT;

                    #endregion

                    #region 2 Added Invoice Address Details
                    foreach (var deliveryAddrTo in loadingSlipTo.DeliveryAddressTOList)
                    {
                        TblInvoiceAddressTO tblInvoiceAddressTo = new TblInvoiceAddressTO();
                        tblInvoiceAddressTo.AadharNo = deliveryAddrTo.AadharNo;
                        tblInvoiceAddressTo.GstinNo = deliveryAddrTo.GstNo;
                        tblInvoiceAddressTo.PanNo = deliveryAddrTo.PanNo;
                        tblInvoiceAddressTo.StateId = deliveryAddrTo.StateId;
                        tblInvoiceAddressTo.State = deliveryAddrTo.State;
                        tblInvoiceAddressTo.Taluka = deliveryAddrTo.TalukaName;
                        tblInvoiceAddressTo.District = deliveryAddrTo.DistrictName;
                        tblInvoiceAddressTo.BillingName = deliveryAddrTo.BillingName;
                        tblInvoiceAddressTo.ContactNo = deliveryAddrTo.ContactNo;
                        tblInvoiceAddressTo.PinCode = deliveryAddrTo.Pincode;
                        tblInvoiceAddressTo.TxnAddrTypeId = deliveryAddrTo.TxnAddrTypeId;
                        tblInvoiceAddressTo.Address = deliveryAddrTo.Address;
                        tblInvoiceAddressTo.AddrSourceTypeId = deliveryAddrTo.AddrSourceTypeId;
                        if (deliveryAddrTo.AddrSourceTypeId == (int)Constants.AddressSourceTypeE.FROM_CNF)
                        {
                            tblInvoiceAddressTo.BillingOrgId = loadingTO.CnfOrgId;
                        }
                        else
                        {
                            tblInvoiceAddressTo.BillingOrgId = loadingSlipTo.DealerOrgId;
                        }

                        if (deliveryAddrTo.TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS)
                        {
                            billingStateId = deliveryAddrTo.StateId;
                            if (string.IsNullOrEmpty(deliveryAddrTo.VillageName))
                            {
                                if (string.IsNullOrEmpty(deliveryAddrTo.TalukaName))
                                {
                                    tblInvoiceTO.DeliveryLocation = deliveryAddrTo.DistrictName;
                                }
                                tblInvoiceTO.DeliveryLocation = deliveryAddrTo.TalukaName;
                            }
                            else
                                tblInvoiceTO.DeliveryLocation = deliveryAddrTo.VillageName;

                            isPanNoPresent = IsPanOrGstPresent(deliveryAddrTo.PanNo, deliveryAddrTo.GstNo);

                        }
                        tblInvoiceTO.InvoiceAddressTOList.Add(tblInvoiceAddressTo);
                    }


                    if (billingStateId == 0)
                    {
                        resultMsg.DefaultBehaviour("Billing State Not Found");
                        return resultMsg;
                    }
                    #endregion

                    #region 3 Added Invoice Item details

                    Double totalInvQty = 0;
                    Double totalNCExpAmt = 0;
                    Double totalNCOtherAmt = 0;
                    #region GJ@20170922 : Find the Minium Weight from LoadingSlipExtTo to Know Tare wt for that Loading Slip
                    if (loadingSlipTo.LoadingSlipExtTOList != null && loadingSlipTo.LoadingSlipExtTOList.Count > 0)
                    {
                        var minCalcTareWt = loadingSlipTo.LoadingSlipExtTOList.Aggregate((curMin, x) => (curMin == null || (x.CalcTareWeight) < curMin.CalcTareWeight ? x : curMin)).CalcTareWeight;
                        tblInvoiceTO.TareWeight = minCalcTareWt;
                        tblInvoiceTO.GrossWeight = minCalcTareWt;
                    }
                    #endregion
                    #region Regular Invoice Items i.e from Loading
                    foreach (var loadingSlipExtTo in loadingSlipTo.LoadingSlipExtTOList)
                    {
                        TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = new TblInvoiceItemDetailsTO();
                        List<TblInvoiceItemTaxDtlsTO> tblInvoiceItemTaxDtlsTOList = new List<TblInvoiceItemTaxDtlsTO>();
                        TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO = new TblProdGstCodeDtlsTO();
                        TblProductItemTO tblProductItemTO = new TblProductItemTO();
                        Double itemGrandTotal = 0;
                        if (loadingTO.LoadingType == (int)Constants.LoadingTypeE.OTHER)
                        {
                            tblInvoiceItemDetailsTO.ProdItemDesc = loadingSlipExtTo.ProdItemDesc;
                            tblInvoiceItemDetailsTO.CdStructure = loadingSlipExtTo.CdStructure;
                            tblInvoiceItemDetailsTO.Rate = loadingSlipExtTo.RatePerMT;

                            //Sanjay [30-May-2019] Added during bug foundin IoT Implementation. Previously this code was applied for other.
                            // Code modified at Jalna
                            tblInvoiceItemDetailsTO.LoadingSlipExtId = loadingSlipExtTo.IdLoadingSlipExt;

                        }
                        else
                        {
                            tblInvoiceItemDetailsTO.LoadingSlipExtId = loadingSlipExtTo.IdLoadingSlipExt;
                            tblInvoiceItemDetailsTO.CdStructure = loadingSlipTo.CdStructure;
                            tblInvoiceItemDetailsTO.Rate = loadingSlipExtTo.CdApplicableAmt;
                            //tblInvoiceItemDetailsTO.ProdItemDesc = loadingSlipExtTo.ProdCatDesc + " " + loadingSlipExtTo.ProdSpecDesc + "-" + loadingSlipExtTo.MaterialDesc; //Commented Sudhir[02-APR-2018]
                            tblInvoiceItemDetailsTO.ProdItemDesc = loadingSlipExtTo.DisplayName;

                            TblLoadingSlipDtlTO tblLoadingSlipDtlTO = new TblLoadingSlipDtlTO();

                            if (loadingSlipTo.IsConfirmed == 0)
                            {
                                if (loadingSlipTo.IdLoadingSlip != 0)
                                {

                                    //loadingSlipTO.IsConfirmed = tblInvoiceTO.IsConfirmed;
                                    tblLoadingSlipDtlTO = BL.TblLoadingSlipDtlBL.SelectLoadingSlipDtlTO(tblInvoiceTO.LoadingSlipId, conn, tran);
                                    //if (tblLoadingSlipDtlTO == null)
                                    //{
                                    //    tran.Rollback();
                                    //    resultMessage.MessageType = ResultMessageE.Error;
                                    //    resultMessage.Text = "Error :tblLoadingSlipDtlTO Found NUll Or Empty";
                                    //    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                    //    return resultMessage;
                                    //}
                                }


                                //Call to get the TblBooking for Parity Id
                                TblBookingsTO tblBookingsTO = new Models.TblBookingsTO();
                                tblBookingsTO = BL.TblBookingsBL.SelectTblBookingsTO(tblLoadingSlipDtlTO.BookingId, conn, tran);
                                if (tblBookingsTO == null)
                                {
                                    tran.Rollback();
                                    resultMsg.MessageType = ResultMessageE.Error;
                                    resultMsg.Text = "Error :tblBookingsTO Found NUll Or Empty";
                                    resultMsg.DisplayMessage = Constants.DefaultErrorMsg;
                                    return resultMsg;
                                }

                                //Vijaymala commented[18-06-2018] to get data from paritydetails
                                // TblParitySummaryTO parityTO = BL.TblParitySummaryBL.SelectParitySummaryTOFromParityDtlId(loadingSlipExtTo.ParityDtlId, conn, tran);

                                TblParityDetailsTO parityDtlTO = null;

                                TblAddressTO addrTO = BL.TblAddressBL.SelectOrgAddressWrtAddrType(tblBookingsTO.DealerOrgId, Constants.AddressTypeE.OFFICE_ADDRESS, conn, tran);

                                parityDtlTO = BL.TblParityDetailsBL.SelectParityDetailToListOnBooking(loadingSlipExtTo.MaterialId, loadingSlipExtTo.ProdCatId, loadingSlipExtTo.ProdSpecId, loadingSlipExtTo.ProdItemId, addrTO.StateId, tblBookingsTO.BookingDatetime);


                                if (parityDtlTO != null)
                                {
                                    totalNCExpAmt += parityDtlTO.ExpenseAmt * Math.Round(loadingSlipExtTo.LoadedWeight * conversionFactor, 2);
                                    totalNCOtherAmt += parityDtlTO.OtherAmt * Math.Round(loadingSlipExtTo.LoadedWeight * conversionFactor, 2);
                                }
                                else
                                {
                                    tran.Rollback();
                                    resultMsg.DefaultBehaviour();
                                    resultMsg.Text = "Error : ParityDtlTO Not Found";
                                    string mateDesc = loadingSlipExtTo.DisplayName;
                                    //loadingSlipExtTo.MaterialDesc + " " + loadingSlipExtTo.ProdCatDesc + "-" + loadingSlipExtTo.ProdSpecDesc;
                                    resultMsg.DisplayMessage = "Warning : Parity Details Not Found For " + mateDesc + " Please contact BackOffice";
                                    return resultMsg;
                                }
                            }
                        }
                        if (loadingSlipExtTo.ProdItemId > 0)
                        {
                            tblProductItemTO = BL.TblProductItemBL.SelectTblProductItemTO(loadingSlipExtTo.ProdItemId, conn, tran);
                            if (tblProductItemTO == null)
                            {
                                resultMsg.DefaultBehaviour("Product conversion Factor Found Null againest the Product Item :  " + loadingSlipExtTo.ProdItemId + ".");
                                return resultMsg;
                            }
                            conversionFactor = tblProductItemTO.ConversionFactor;
                        }
                        //Reshma[3-Aug-2023] Added For Deliver New changes for nozal invoice as PermissionTO kushal sir email
                        Boolean isAllowLoadingSlipInvoiceQty = false;
                        TblConfigParamsTO tblConfigParamsTOtemp = TblConfigParamsBL.SelectTblConfigParamsTO("CP_DELIVER_SUB_GROUP_TO_ADD_INVOICE_QTY_AS_PER_LODING_QTY", conn, tran);
                        if (tblConfigParamsTOtemp != null)
                        {
                            string SubgrupIdStr = tblConfigParamsTOtemp.ConfigParamVal;
                            int[] array = SubgrupIdStr.Split(',').Select(int.Parse).ToArray();
                            for (int a = 0; a < array.Count(); a++)
                            {
                                if (array[a] == tblProductItemTO.ProdClassId)
                                    isAllowLoadingSlipInvoiceQty = true;
                            }
                        }


                        tblInvoiceTO.NetWeight += loadingSlipExtTo.LoadedWeight;
                        tblInvoiceTO.GrossWeight += loadingSlipExtTo.LoadedWeight;
                        if(isAllowLoadingSlipInvoiceQty)
                            tblInvoiceItemDetailsTO.InvoiceQty = Math.Round(loadingSlipExtTo.LoadingQty ,2);
                        else
                             tblInvoiceItemDetailsTO.InvoiceQty = Math.Round(loadingSlipExtTo.LoadedWeight * conversionFactor, 2);
                        totalInvQty += tblInvoiceItemDetailsTO.InvoiceQty;
                        tblInvoiceItemDetailsTO.CdStructureId = loadingSlipTo.CdStructureId;
                        tblInvoiceItemDetailsTO.Bundles = loadingSlipExtTo.LoadedBundles;
                        if (isAllowLoadingSlipInvoiceQty)
                            tblInvoiceItemDetailsTO.BasicTotal = Math.Round(tblInvoiceItemDetailsTO.InvoiceQty * tblInvoiceItemDetailsTO.Rate);
                        else
                            tblInvoiceItemDetailsTO.BasicTotal = Math.Round(loadingSlipExtTo.LoadedWeight * conversionFactor * tblInvoiceItemDetailsTO.Rate);
                        basicTotal += tblInvoiceItemDetailsTO.BasicTotal;
                        if (tblInvoiceItemDetailsTO.CdStructure > 0)
                        {
                            tblInvoiceItemDetailsTO.CdAmt = Math.Round(tblInvoiceItemDetailsTO.BasicTotal * tblInvoiceItemDetailsTO.CdStructure) / 100;
                        }
                        else
                        {
                            tblInvoiceItemDetailsTO.CdAmt = 0;
                        }
                        discountTotal += tblInvoiceItemDetailsTO.CdAmt;
                        Double taxbleAmt = 0;
                        //Double totalFreExpOtherAmt = loadingSlipExtTo.LoadedWeight * conversionFactor * loadingSlipExtTo.FreExpOtherAmt;

                        if (loadingSlipTo.IsConfirmed == 1)
                            taxbleAmt = tblInvoiceItemDetailsTO.BasicTotal - tblInvoiceItemDetailsTO.CdAmt;// + totalFreExpOtherAmt;
                        else
                            taxbleAmt = tblInvoiceItemDetailsTO.BasicTotal - tblInvoiceItemDetailsTO.CdAmt;

                        tblInvoiceItemDetailsTO.TaxableAmt = taxbleAmt;
                        itemGrandTotal += taxbleAmt;
                        taxableTotal += tblInvoiceItemDetailsTO.TaxableAmt;
                        tblProdGstCodeDtlsTO = BL.TblProdGstCodeDtlsBL.SelectTblProdGstCodeDtlsTO(loadingSlipExtTo.ProdCatId, loadingSlipExtTo.ProdSpecId, loadingSlipExtTo.MaterialId, loadingSlipExtTo.ProdItemId, 0, conn, tran);
                        if (tblProdGstCodeDtlsTO == null)
                        {
                            resultMsg.DefaultBehaviour("ProdGSTCodeDetails found null against loadingSlipExtId is : " + loadingSlipExtTo.IdLoadingSlipExt + ".");
                            resultMsg.DisplayMessage = "GSTIN Not Defined for Item :" + tblInvoiceItemDetailsTO.ProdItemDesc;
                            return resultMsg;
                        }
                        tblInvoiceItemDetailsTO.ProdGstCodeId = tblProdGstCodeDtlsTO.IdProdGstCode;
                        TblGstCodeDtlsTO gstCodeDtlsTO = BL.TblGstCodeDtlsBL.SelectTblGstCodeDtlsTO(tblProdGstCodeDtlsTO.GstCodeId, conn, tran);
                        if (gstCodeDtlsTO != null)
                        {
                            gstCodeDtlsTO.TaxRatesTOList = BL.TblTaxRatesBL.SelectAllTblTaxRatesList(tblProdGstCodeDtlsTO.GstCodeId, conn, tran);
                        }
                        if (gstCodeDtlsTO == null)
                        {
                            resultMsg.DefaultBehaviour("GST code details found null againest loadingSlipExtId is : " + loadingSlipExtTo.IdLoadingSlipExt + ".");
                            resultMsg.DisplayMessage = "GSTIN Not Defined for Item :" + tblInvoiceItemDetailsTO.ProdItemDesc;
                            return resultMsg;
                        }

                        tblInvoiceItemDetailsTO.GstinCodeNo = gstCodeDtlsTO.CodeNumber;

                        #region 4 Added Invoice Item Tax details

                        foreach (var taxRateTo in gstCodeDtlsTO.TaxRatesTOList)
                        {
                            TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO = new TblInvoiceItemTaxDtlsTO();
                            tblInvoiceItemTaxDtlsTO.TaxRateId = taxRateTo.IdTaxRate;
                            tblInvoiceItemTaxDtlsTO.TaxPct = taxRateTo.TaxPct;
                            tblInvoiceItemTaxDtlsTO.TaxRatePct = (gstCodeDtlsTO.TaxPct * taxRateTo.TaxPct) / 100;
                            tblInvoiceItemTaxDtlsTO.TaxableAmt = tblInvoiceItemDetailsTO.TaxableAmt;
                            tblInvoiceItemTaxDtlsTO.TaxAmt = (tblInvoiceItemTaxDtlsTO.TaxableAmt * tblInvoiceItemTaxDtlsTO.TaxRatePct) / 100;
                            tblInvoiceItemTaxDtlsTO.TaxTypeId = taxRateTo.TaxTypeId;
                            if (billingStateId == ofcAddrTO.StateId)
                            {
                                if (taxRateTo.TaxTypeId == (int)Constants.TaxTypeE.CGST)
                                {
                                    cgstTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                    itemGrandTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                    tblInvoiceItemTaxDtlsTOList.Add(tblInvoiceItemTaxDtlsTO);
                                }
                                else if (taxRateTo.TaxTypeId == (int)Constants.TaxTypeE.SGST)
                                {
                                    sgstTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                    itemGrandTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                    tblInvoiceItemTaxDtlsTOList.Add(tblInvoiceItemTaxDtlsTO);
                                }
                                else continue;
                            }
                            else
                            {
                                if (taxRateTo.TaxTypeId == (int)Constants.TaxTypeE.IGST)
                                {
                                    igstTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                    itemGrandTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                    tblInvoiceItemTaxDtlsTOList.Add(tblInvoiceItemTaxDtlsTO);
                                }
                                else continue;
                            }
                        }
                        #endregion


                        grandTotal += itemGrandTotal;
                        tblInvoiceItemDetailsTO.GrandTotal = itemGrandTotal;
                        tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList = tblInvoiceItemTaxDtlsTOList;
                        tblInvoiceItemDetailsTOList.Add(tblInvoiceItemDetailsTO);
                    }

                    #endregion

                    #region Freight , Expenses or Other Charges if Applicable while loading

                    if (loadingSlipTo.IsConfirmed == 1)
                    {
                        if (loadingTO.IsFreightIncluded == 1 && loadingTO.FreightAmt > 0)
                        {

                            TblConfigParamsTO otherFreighConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_FRIEGHT_OTHER_TAX_ID, conn, tran);
                            if (otherFreighConfigParamsTO == null)
                            {
                                resultMsg.DefaultBehaviour("Other Tax id is not configured for freight");
                                return resultMsg;
                            }
                            Int32 freightOtherTaxId = Convert.ToInt32(otherFreighConfigParamsTO.ConfigParamVal);
                            Double freightGrandTotal = 0;
                            TblInvoiceItemDetailsTO freightItemTO = new TblInvoiceItemDetailsTO();
                            freightItemTO.OtherTaxId = freightOtherTaxId;
                            freightItemTO.InvoiceQty = totalInvQty;
                            freightItemTO.Rate = loadingTO.FreightAmt;
                            freightItemTO.TaxableAmt = freightItemTO.BasicTotal = loadingTO.FreightAmt * totalInvQty;
                            freightGrandTotal += freightItemTO.TaxableAmt;
                            var maxTaxableItemTO = tblInvoiceItemDetailsTOList.OrderByDescending(m => m.TaxableAmt).FirstOrDefault();

                            freightItemTO.ProdGstCodeId = maxTaxableItemTO.ProdGstCodeId;
                            freightItemTO.ProdItemDesc = "Freight Charges";
                            freightItemTO.GstinCodeNo = maxTaxableItemTO.GstinCodeNo;

                            basicTotal += freightItemTO.BasicTotal;
                            taxableTotal += freightItemTO.TaxableAmt;

                            if (maxTaxableItemTO.InvoiceItemTaxDtlsTOList != null && maxTaxableItemTO.InvoiceItemTaxDtlsTOList.Count > 0)
                            {
                                for (int ti = 0; ti < maxTaxableItemTO.InvoiceItemTaxDtlsTOList.Count; ti++)
                                {
                                    TblInvoiceItemTaxDtlsTO taxDtlTO = maxTaxableItemTO.InvoiceItemTaxDtlsTOList[ti].DeepCopy();
                                    taxDtlTO.TaxableAmt = freightItemTO.TaxableAmt;
                                    taxDtlTO.TaxAmt = (taxDtlTO.TaxableAmt * taxDtlTO.TaxRatePct) / 100;
                                    freightGrandTotal += taxDtlTO.TaxAmt;

                                    if (taxDtlTO.TaxTypeId == (int)Constants.TaxTypeE.CGST)
                                    {
                                        cgstTotal += taxDtlTO.TaxAmt;
                                    }
                                    if (taxDtlTO.TaxTypeId == (int)Constants.TaxTypeE.SGST)
                                    {
                                        sgstTotal += taxDtlTO.TaxAmt;
                                    }
                                    if (taxDtlTO.TaxTypeId == (int)Constants.TaxTypeE.IGST)
                                    {
                                        igstTotal += taxDtlTO.TaxAmt;
                                    }

                                    if (freightItemTO.InvoiceItemTaxDtlsTOList == null)
                                        freightItemTO.InvoiceItemTaxDtlsTOList = new List<TblInvoiceItemTaxDtlsTO>();
                                    freightItemTO.InvoiceItemTaxDtlsTOList.Add(taxDtlTO);
                                }
                            }

                            freightItemTO.GrandTotal = freightGrandTotal;

                            tblInvoiceItemDetailsTOList.Add(freightItemTO);
                            grandTotal += freightGrandTotal;
                        }
                        Int32 BillingOrgId = tblInvoiceTO.DealerOrgId;
                        if(tblInvoiceTO.InvoiceAddressTOList != null && tblInvoiceTO.InvoiceAddressTOList.Count > 0)
                        {
                            var matchTO = tblInvoiceTO.InvoiceAddressTOList.Where(w => w.TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS).FirstOrDefault();
                            if(matchTO != null && matchTO.BillingOrgId > 0)
                            {
                                BillingOrgId = matchTO.BillingOrgId;
                            }
                        }

                        tblOrganizationTO = TblOrganizationDAO.SelectTblOrganization(BillingOrgId, conn, tran);
                        if (tblOrganizationTO == null)
                        {
                            resultMsg.DefaultBehaviour("Failed to get dealer org details");
                            return resultMsg;
                        }
                        tblInvoiceTO.IsTcsApplicable = tblOrganizationTO.IsTcsApplicable;
                        if (tblOrganizationTO.IsTcsApplicable == 1)
                        {
                            resultMsg = AddTcsTOInTaxItemDtls(conn, tran, ref grandTotal, ref taxableTotal, ref basicTotal, isPanNoPresent, tblInvoiceItemDetailsTOList);
                            if (resultMsg == null || resultMsg.MessageType != ResultMessageE.Information)
                            {
                                return resultMsg;
                            }
                        }
                    }
                    else
                    {
                        tblInvoiceTO.ExpenseAmt = totalNCExpAmt;
                        tblInvoiceTO.OtherAmt = totalNCOtherAmt;
                        if (loadingTO.IsFreightIncluded == 1)
                            tblInvoiceTO.FreightAmt = totalInvQty * loadingTO.FreightAmt;

                        // grandTotal += totalNCExpAmt + totalNCOtherAmt + tblInvoiceTO.FreightAmt;
                        //As discuss with Saket above statement commented and below statement added
                        grandTotal += tblInvoiceTO.FreightAmt;
                    }
                    #endregion

                    #endregion

                    #region 5 Save main Invoice
                    tblInvoiceTO.TaxableAmt = taxableTotal;
                    tblInvoiceTO.DiscountAmt = discountTotal;
                    tblInvoiceTO.IgstAmt = igstTotal;
                    tblInvoiceTO.CgstAmt = cgstTotal;
                    tblInvoiceTO.SgstAmt = sgstTotal;
                    double finalGrandTotal = Math.Round(grandTotal);
                    tblInvoiceTO.GrandTotal = finalGrandTotal;
                    tblInvoiceTO.RoundOffAmt = Math.Round(finalGrandTotal - grandTotal, 2);
                    tblInvoiceTO.BasicAmt = basicTotal;
                    tblInvoiceTO.InvoiceItemDetailsTOList = tblInvoiceItemDetailsTOList;
                    #endregion

                    tblInvoiceTOList.Add(tblInvoiceTO);
                }

                #endregion


                #region

                if (confiqId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
                {
                    for (int i = 0; i < tblInvoiceTOList.Count; i++)
                    {
                        RemoveIotFieldsFromDB(tblInvoiceTOList[i]);
                    }
                }

                #endregion



                #region Call To Save Invoice

                if (tblInvoiceTOList != null)
                {
                    //Added by kiran for checking of grand total zero
                    if (loadingTO.LoadingType != (int)Constants.LoadingTypeE.OTHER)
                    {
                        List<TblInvoiceTO> tblTempInvoiceTOList = tblInvoiceTOList.Where(s => s.GrandTotal == 0).ToList();
                        if (tblTempInvoiceTOList.Count > 0)
                        {
                            resultMsg.DefaultBehaviour("Grand Total zero in invoice");
                            return resultMsg;
                        }
                    }
                    //Apply TDS
                    Double tdsTaxPct = 0;
                    //if (tblOrganizationTO != null)
                    //{
                    //    if (tblOrganizationTO.IsTcsApplicable == 0)
                    //    {
                    //        TblConfigParamsTO tdsConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DELIVER_INVOICE_TDS_TAX_PCT, conn, tran);
                    //        if (tdsConfigParamsTO != null)
                    //        {
                    //            if (!String.IsNullOrEmpty(tdsConfigParamsTO.ConfigParamVal))
                    //            {
                    //                tdsTaxPct = Convert.ToDouble(tdsConfigParamsTO.ConfigParamVal);
                    //            }
                    //        }
                    //    }
                    //}
                    TblConfigParamsTO tdsConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DELIVER_INVOICE_TDS_TAX_PCT, conn, tran);
                    if (tdsConfigParamsTO != null)
                    {
                        if (!String.IsNullOrEmpty(tdsConfigParamsTO.ConfigParamVal))
                        {
                            tdsTaxPct = Convert.ToDouble(tdsConfigParamsTO.ConfigParamVal);
                        }
                    }
                    for (int i = 0; i < tblInvoiceTOList.Count; i++)
                    {
                        tblInvoiceTOList[i].TdsAmt = 0;
                        if (tblInvoiceTOList[i].IsConfirmed == 1 && tblInvoiceTOList[i].IsTcsApplicable == 0)
                        {
                            tblInvoiceTOList[i].TdsAmt = (CalculateTDS(tblInvoiceTOList[i]) * tdsTaxPct) / 100;
                            tblInvoiceTOList[i].TdsAmt = Math.Ceiling(tblInvoiceTOList[i].TdsAmt);
                        }
                        resultMsg = SaveNewInvoice(tblInvoiceTOList[i], conn, tran);
                        if (resultMsg.MessageType != ResultMessageE.Information)
                        {
                            return resultMsg;
                        }
                    }
                }

                #endregion

                resultMsg.DefaultSuccessBehaviour();
                return resultMsg;
            }
            catch (Exception ex)
            {
                resultMsg.DefaultExceptionBehaviour(ex, "PrepareAndSaveNewTaxInvoice");
                return resultMsg;
            }
        }

        //Harshala[30/09/3030] added to calculate TCS 
        public static Boolean IsPanOrGstPresent(String panNo, String gstNo)
        {
            if ((!String.IsNullOrEmpty(panNo) && panNo != "") || (!String.IsNullOrEmpty(gstNo) && gstNo != ""))
            {

                return true;
            }
            else
                return false;
        }

        //Harshala[30/09/3030] added to calculate TCS 
        private static ResultMessage AddTcsTOInTaxItemDtls(SqlConnection conn, SqlTransaction tran, ref double grandTotal, ref double taxableTotal, ref double basicTotal, bool isPanNoPresent, List<TblInvoiceItemDetailsTO> tblInvoiceItemDetailsTOList)
        {
            ResultMessage resultMessage = new ResultMessage();
 
            TblConfigParamsTO configParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_IS_INCLUDE_TCS_TO_AUTO_INVOICE, conn, tran);

            if (configParamsTO != null)
            {
                if (configParamsTO.ConfigParamVal == "1")
                {

                    TblConfigParamsTO tcsConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_TCS_OTHER_TAX_ID, conn, tran);

                    if (tcsConfigParamsTO == null)
                    {
                        resultMessage.DefaultBehaviour("Other Tax id is not configured for freight");
                        return resultMessage;
                    }

                    TblConfigParamsTO tcsPercentConfigParamsTO = new TblConfigParamsTO();

                    if (isPanNoPresent)
                        tcsPercentConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.DEFAULT_TCS_PERCENT_IF_PAN_PRESENT, conn, tran);
                    else
                        tcsPercentConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.DEFAULT_TCS_PERCENT_IF_PAN_NOT_PRESENT, conn, tran);

                    if (tcsPercentConfigParamsTO != null)
                    {
                        if (tcsPercentConfigParamsTO.ConfigParamVal != "0" && tcsPercentConfigParamsTO.ConfigParamVal != null)
                        {
                            Int32 tcsOtherTaxId = Convert.ToInt32(tcsConfigParamsTO.ConfigParamVal);
                            Double tcsGrandTotal = 0;
                            TblInvoiceItemDetailsTO tcsItemTO = new TblInvoiceItemDetailsTO();
                            tcsItemTO.OtherTaxId = tcsOtherTaxId;
                            TblOtherTaxesTO otherTaxesTO = DAL.TblOtherTaxesDAO.SelectTblOtherTaxes(tcsOtherTaxId);
                            tcsItemTO.ProdItemDesc = otherTaxesTO.TaxName;

                            var maxTaxableItemTO = tblInvoiceItemDetailsTOList.OrderByDescending(m => m.TaxableAmt).FirstOrDefault();


                            tcsItemTO.ProdGstCodeId = maxTaxableItemTO.ProdGstCodeId;
                            tcsItemTO.GstinCodeNo = maxTaxableItemTO.GstinCodeNo;
                            Double tcsTaxPercent = Convert.ToDouble(tcsPercentConfigParamsTO.ConfigParamVal);
                            tcsItemTO.TaxPct = tcsTaxPercent;
                            tcsItemTO.TaxableAmt = (grandTotal * tcsTaxPercent) / 100;
                            tcsItemTO.TaxableAmt = tcsItemTO.BasicTotal = Math.Ceiling(tcsItemTO.TaxableAmt);
                            tcsGrandTotal += tcsItemTO.TaxableAmt;
                            tcsItemTO.GrandTotal = tcsGrandTotal;
                            taxableTotal += tcsItemTO.TaxableAmt;
                            basicTotal += tcsItemTO.BasicTotal;

                            if (maxTaxableItemTO.InvoiceItemTaxDtlsTOList != null && maxTaxableItemTO.InvoiceItemTaxDtlsTOList.Count > 0)
                            {
                                for (int ti = 0; ti < maxTaxableItemTO.InvoiceItemTaxDtlsTOList.Count; ti++)
                                {
                                    TblInvoiceItemTaxDtlsTO taxDtlTO = maxTaxableItemTO.InvoiceItemTaxDtlsTOList[ti].DeepCopy();
                                    taxDtlTO.TaxableAmt = tcsItemTO.TaxableAmt;
                                    taxDtlTO.TaxAmt = 0;
                                    taxDtlTO.TaxRatePct = 0.00;

                                    if (tcsItemTO.InvoiceItemTaxDtlsTOList == null)
                                        tcsItemTO.InvoiceItemTaxDtlsTOList = new List<TblInvoiceItemTaxDtlsTO>();
                                    tcsItemTO.InvoiceItemTaxDtlsTOList.Add(taxDtlTO);
                                }
                            }
                            tblInvoiceItemDetailsTOList.Add(tcsItemTO);

                            grandTotal += tcsGrandTotal;
                        }
                    }
                }
            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }

        private static void RemoveIotFieldsFromDB(TblInvoiceTO tblInvoiceTO)
        {
            int configId = Constants.getweightSourceConfigTO();
            if (configId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
            {
                if (tblInvoiceTO.InvoiceStatusE != InvoiceStatusE.AUTHORIZED && tblInvoiceTO.InvoiceModeE != InvoiceModeE.MANUAL_INVOICE)
                {
                    if (tblInvoiceTO.LoadingSlipId > 0)
                    {
                        tblInvoiceTO.VehicleNo = String.Empty;
                        tblInvoiceTO.TransportOrgId = 0;

                        tblInvoiceTO.GrossWeight = 0;
                        tblInvoiceTO.NetWeight = 0;
                        //tblInvoiceTO.TareWeight = 0;
                        if (tblInvoiceTO.InvoiceItemDetailsTOList != null && tblInvoiceTO.InvoiceItemDetailsTOList.Count > 0)
                        {
                            for (int j = 0; j < tblInvoiceTO.InvoiceItemDetailsTOList.Count; j++)
                            {
                                TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = tblInvoiceTO.InvoiceItemDetailsTOList[j];
                                if (tblInvoiceItemDetailsTO.LoadingSlipExtId > 0)
                                {
                                    tblInvoiceItemDetailsTO.InvoiceQty = 0;
                                    tblInvoiceItemDetailsTO.Bundles = 0;
                                }
                            }
                        }
                    }
                }
            }
        }

        #region Old Invoice COde
        //public static ResultMessage PrepareAndSaveInternalTaxInvoices(TblInvoiceTO invoiceTO, InvoiceGenerateModeE invoiceGenerateModeE, SqlConnection conn, SqlTransaction tran)
        //{
        //    ResultMessage resultMsg = new ResultMessage();
        //    string entityRangeName = string.Empty;
        //    Int32 result = 0;
        //    List<DropDownTO> districtList = new List<DropDownTO>();
        //    List<DropDownTO> talukaList = new List<DropDownTO>();
        //    try
        //    {
        //        DateTime serverDateTime = Constants.ServerDateTime;
        //        Int32 invoiceId = invoiceTO.IdInvoice;
        //        List<TblInvoiceItemDetailsTO> invoiceItemTOList = null;
        //        List<TblInvoiceItemTaxDtlsTO> invoiceItemTaxTOList = BL.TblInvoiceItemTaxDtlsBL.SelectInvoiceItemTaxDtlsListByInvoiceId(invoiceId, conn, tran);
        //        List<TblInvoiceAddressTO> invoiceAddressTOList = BL.TblInvoiceAddressBL.SelectAllTblInvoiceAddressList(invoiceId, conn, tran);

        //        if (Constants.getweightSourceConfigTO() == (Int32)Constants.WeighingDataSourceE.IoT)
        //        {
        //            if (invoiceTO.IsConfirmed == 1)
        //            {
        //                invoiceTO = SelectTblInvoiceTOWithDetails(invoiceTO.IdInvoice, conn, tran);

        //                #region Validate IOT data

        //                if (invoiceTO == null || invoiceTO.VehicleNo == null || invoiceTO.TransportOrgId == 0)
        //                {
        //                    tran.Rollback();
        //                    resultMsg.DefaultBehaviour("invoiceTO Found NULL OR VehicleNo Found NULL OR TransportOrgId Found NULL when write Data to Invoice");
        //                    return resultMsg;
        //                }

        //                var invoiceItemList = invoiceTO.InvoiceItemDetailsTOList.Where(w => w.LoadingSlipExtId > 0).ToList();
        //                if (invoiceItemList != null && invoiceItemList.Count > 0)
        //                {
        //                    for (int s = 0; s < invoiceItemList.Count; s++)
        //                    {
        //                        if (invoiceItemList[s].InvoiceQty <= 0)
        //                        {
        //                            tran.Rollback();
        //                            resultMsg.DefaultBehaviour("Invoice Item Qty found zero when write Data to Invoice");
        //                            return resultMsg;
        //                        }
        //                    }
        //                }

        //                #endregion

        //                invoiceItemTOList = invoiceTO.InvoiceItemDetailsTOList;
        //            }
        //        }
        //        else
        //        {
        //            invoiceItemTOList = BL.TblInvoiceItemDetailsBL.SelectAllTblInvoiceItemDetailsList(invoiceId, conn, tran);
        //        }
        //        #region 1 BRM TO BM Invoice

        //        //resultMsg = PrepareNewInvoiceObjectList(invoiceTO, invoiceItemTOList, invoiceAddressTOList, InvoiceGenerateModeE.BRMTOBM, conn, tran);
        //        resultMsg = PrepareNewInvoiceObjectList(invoiceTO, invoiceItemTOList, invoiceAddressTOList, invoiceGenerateModeE, conn, tran);
        //        if (resultMsg.MessageType == ResultMessageE.Information)
        //        {
        //            if (resultMsg.Tag != null && resultMsg.Tag.GetType() == typeof(List<TblInvoiceTO>))
        //            {
        //                List<TblInvoiceTO> tblInvoiceTOList = (List<TblInvoiceTO>)resultMsg.Tag;
        //                if (tblInvoiceTOList != null)
        //                {
        //                    //Update Existing Invoice
        //                    TblInvoiceTO invToUpdateTO = tblInvoiceTOList[0]; //Taken 0th Object as it will always go for single invoice at a time.List is return as existing code is used.

        //                    //Delete existing invoice item taxes details
        //                    result = BL.TblInvoiceItemTaxDtlsBL.DeleteInvoiceItemTaxDtlsByInvId(invToUpdateTO.IdInvoice, conn, tran);
        //                    if (result <= -1)
        //                    {
        //                        resultMsg.DefaultBehaviour("Error While DeleteInvoiceItemTaxDtlsByInvId");
        //                        return resultMsg;
        //                    }

        //                    //Update Invoice Object
        //                    invToUpdateTO.UpdatedBy = invoiceTO.CreatedBy;
        //                    invToUpdateTO.UpdatedOn = Constants.ServerDateTime;
        //                    result = UpdateTblInvoice(invToUpdateTO, conn, tran);
        //                    if (result != 1)
        //                    {
        //                        resultMsg.DefaultBehaviour("Error While UpdateTblInvoice");
        //                        return resultMsg;
        //                    }

        //                    // Update Invoice Item Details
        //                    for (int invI = 0; invI < invToUpdateTO.InvoiceItemDetailsTOList.Count; invI++)
        //                    {
        //                        TblInvoiceItemDetailsTO itemTO = invToUpdateTO.InvoiceItemDetailsTOList[invI];
        //                        result = BL.TblInvoiceItemDetailsBL.UpdateTblInvoiceItemDetails(itemTO, conn, tran);
        //                        if (result != 1)
        //                        {
        //                            resultMsg.DefaultBehaviour("Error While UpdateTblInvoiceItemDetails");
        //                            return resultMsg;
        //                        }

        //                        if (itemTO.InvoiceItemTaxDtlsTOList != null && itemTO.InvoiceItemTaxDtlsTOList.Count > 0)
        //                        {
        //                            for (int t = 0; t < itemTO.InvoiceItemTaxDtlsTOList.Count; t++)
        //                            {
        //                                itemTO.InvoiceItemTaxDtlsTOList[t].InvoiceItemId = itemTO.IdInvoiceItem;
        //                                result = BL.TblInvoiceItemTaxDtlsBL.InsertTblInvoiceItemTaxDtls(itemTO.InvoiceItemTaxDtlsTOList[t], conn, tran);
        //                                if (result != 1)
        //                                {
        //                                    resultMsg.DefaultBehaviour("Error While InsertTblInvoiceItemTaxDtls");
        //                                    return resultMsg;
        //                                }
        //                            }
        //                        }
        //                    }

        //                    result = BL.TblInvoiceAddressBL.DeleteTblInvoiceAddressByinvoiceId(invToUpdateTO.IdInvoice, conn, tran);
        //                    if (result == -1)
        //                    {
        //                        resultMsg.DefaultBehaviour("Error While DeleteTblInvoiceItemTaxDtls");
        //                        return resultMsg;
        //                    }
        //                    //Update Existing Address Details
        //                    for (int ac = 0; ac < invToUpdateTO.InvoiceAddressTOList.Count; ac++)
        //                    {
        //                        invToUpdateTO.InvoiceAddressTOList[ac].InvoiceId = invToUpdateTO.IdInvoice;
        //                        result = BL.TblInvoiceAddressBL.InsertTblInvoiceAddress(invToUpdateTO.InvoiceAddressTOList[ac], conn, tran);
        //                        if (result != 1)
        //                        {
        //                            resultMsg.DefaultBehaviour("Error While InsertTblInvoiceItemTaxDtls");
        //                            return resultMsg;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            return resultMsg;
        //        }

        //        #endregion

        //        #region 2 BM TO Actual Customer Invoice

        //        if (invoiceGenerateModeE == InvoiceGenerateModeE.BRMTOBM)
        //        {
        //            resultMsg = PrepareNewInvoiceObjectList(invoiceTO, invoiceItemTOList, invoiceAddressTOList, InvoiceGenerateModeE.BMTOCUSTOMER, conn, tran);
        //            if (resultMsg.MessageType == ResultMessageE.Information)
        //            {
        //                if (resultMsg.Tag != null && resultMsg.Tag.GetType() == typeof(List<TblInvoiceTO>))
        //                {
        //                    List<TblInvoiceTO> tblInvoiceTOList = (List<TblInvoiceTO>)resultMsg.Tag;
        //                    if (tblInvoiceTOList != null)
        //                    {
        //                        for (int i = 0; i < tblInvoiceTOList.Count; i++)
        //                        {
        //                            resultMsg = SaveNewInvoice(tblInvoiceTOList[i], conn, tran);
        //                            if (resultMsg.MessageType != ResultMessageE.Information)
        //                            {
        //                                return resultMsg;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                return resultMsg;
        //            }
        //        }
        //        #endregion

        //        resultMsg.DefaultSuccessBehaviour();
        //        return resultMsg;
        //    }
        //    catch (Exception ex)
        //    {
        //        resultMsg.DefaultExceptionBehaviour(ex, "PrepareAndSaveNewTaxInvoice");
        //        return resultMsg;
        //    }
        //}

        //public static ResultMessage PrepareNewInvoiceObjectList(TblInvoiceTO invoiceTO, List<TblInvoiceItemDetailsTO> invoiceItemTOList, List<TblInvoiceAddressTO> invoiceAddressTOList, InvoiceGenerateModeE invoiceGenerateModeE, SqlConnection conn, SqlTransaction tran)
        //{
        //    ResultMessage resultMsg = new ResultMessage();
        //    try
        //    {
        //        #region Prepare List Of Invoices To Save

        //        List<TblInvoiceTO> tblInvoiceTOList = new List<TblInvoiceTO>();

        //        TblInvoiceTO tblInvoiceTO = invoiceTO.DeepCopy();
        //        tblInvoiceTO.InvoiceAddressTOList = new List<TblInvoiceAddressTO>();
        //        tblInvoiceTO.InvoiceItemDetailsTOList = new List<TblInvoiceItemDetailsTO>();
        //        double grandTotal = 0;
        //        double discountTotal = 0;
        //        double igstTotal = 0;
        //        double cgstTotal = 0;
        //        double sgstTotal = 0;
        //        double basicTotal = 0;
        //        double taxableTotal = 0;

        //        TblConfigParamsTO tblConfigParamsTO = null;
        //        DateTime serverDateTime = Constants.ServerDateTime;
        //        Int32 billingStateId = 0;
        //        if (invoiceGenerateModeE == InvoiceGenerateModeE.BRMTOBM)
        //        {
        //            tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID, conn, tran);
        //        }
        //        else
        //            tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_INTERNALTXFER_INVOICE_ORG_ID, conn, tran);

        //        if (tblConfigParamsTO == null)
        //        {
        //            resultMsg.DefaultBehaviour("Internal Self Organization Not Found in Configuration.");
        //            return resultMsg;
        //        }
        //        Int32 internalOrgId = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
        //        TblOrganizationTO orgTO = BL.TblOrganizationBL.SelectTblOrganizationTO(internalOrgId, conn, tran);
        //        TblAddressTO ofcAddrTO = BL.TblAddressBL.SelectOrgAddressWrtAddrType(internalOrgId, Constants.AddressTypeE.OFFICE_ADDRESS, conn, tran);
        //        if (ofcAddrTO == null)
        //        {
        //            resultMsg.DefaultBehaviour("Address Not Found For Self Organization.");
        //            return resultMsg;
        //        }

        //        List<TblInvoiceItemDetailsTO> tblInvoiceItemDetailsTOList = new List<TblInvoiceItemDetailsTO>();

        //        #region 1 Preparing main InvoiceTO

        //        //Saket [2018-01-15] Added For BMM only invoices.
        //        invoiceTO.InvFromOrgId = internalOrgId;

        //        tblInvoiceTO.InvFromOrgId = internalOrgId;
        //        tblInvoiceTO.CreatedOn = Constants.ServerDateTime;
        //        tblInvoiceTO.CreatedBy = invoiceTO.CreatedBy;
        //        tblInvoiceTO.InvoiceDate = tblInvoiceTO.CreatedOn;
        //        tblInvoiceTO.StatusDate = tblInvoiceTO.InvoiceDate;

        //        #endregion

        //        #region 2 Added Invoice Address Details
        //        if (invoiceGenerateModeE == InvoiceGenerateModeE.BRMTOBM)
        //        {
        //            tblInvoiceTO.Narration = "To Bhagylaxmi Metal";

        //            TblConfigParamsTO configParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_INTERNALTXFER_INVOICE_ORG_ID, conn, tran);

        //            if (configParamsTO == null)
        //            {
        //                resultMsg.DefaultBehaviour("Internal Self Organization Not Found in Configuration.");
        //                return resultMsg;
        //            }
        //            Int32 internalBMOrgId = Convert.ToInt32(configParamsTO.ConfigParamVal);
        //            TblOrganizationTO bmOrgTO = BL.TblOrganizationBL.SelectTblOrganizationTO(internalBMOrgId, conn, tran);
        //            TblAddressTO bmOfcAddrTO = BL.TblAddressBL.SelectOrgAddressWrtAddrType(internalBMOrgId, Constants.AddressTypeE.OFFICE_ADDRESS, conn, tran);
        //            if (bmOfcAddrTO == null)
        //            {
        //                resultMsg.DefaultBehaviour("Address Not Found For BM Organization.");
        //                return resultMsg;
        //            }

        //            tblInvoiceTO.DealerOrgId = internalBMOrgId;  //BMM AS dealer.

        //            List<TblOrgLicenseDtlTO> licenseList = BL.TblOrgLicenseDtlBL.SelectAllTblOrgLicenseDtlList(internalBMOrgId, conn, tran);
        //            String aadharNo = string.Empty;
        //            String gstNo = string.Empty;
        //            String panNo = string.Empty;

        //            if (licenseList != null)
        //            {
        //                var panNoObj = licenseList.Where(a => a.LicenseId == (int)Constants.CommercialLicenseE.PAN_NO).FirstOrDefault();
        //                if (panNoObj != null)
        //                    panNo = panNoObj.LicenseValue;
        //                var gstinObj = licenseList.Where(a => a.LicenseId == (int)Constants.CommercialLicenseE.IGST_NO).FirstOrDefault();
        //                if (gstinObj != null)
        //                    gstNo = gstinObj.LicenseValue;
        //            }

        //            TblInvoiceAddressTO tblInvoiceAddressTo = new TblInvoiceAddressTO();
        //            tblInvoiceAddressTo.GstinNo = gstNo;
        //            tblInvoiceAddressTo.PanNo = panNo;
        //            tblInvoiceAddressTo.StateId = bmOfcAddrTO.StateId;
        //            tblInvoiceAddressTo.State = bmOfcAddrTO.StateName;
        //            tblInvoiceAddressTo.Taluka = bmOfcAddrTO.TalukaName;
        //            tblInvoiceAddressTo.District = bmOfcAddrTO.DistrictName;
        //            tblInvoiceAddressTo.BillingName = bmOrgTO.FirmName;
        //            tblInvoiceAddressTo.ContactNo = bmOrgTO.RegisteredMobileNos;
        //            tblInvoiceAddressTo.PinCode = bmOfcAddrTO.Pincode.ToString();
        //            tblInvoiceAddressTo.TxnAddrTypeId = (int)TxnDeliveryAddressTypeE.BILLING_ADDRESS;
        //            tblInvoiceAddressTo.Address = bmOfcAddrTO.PlotNo + bmOfcAddrTO.StreetName;
        //            tblInvoiceAddressTo.AddrSourceTypeId = (int)AddressSourceTypeE.FROM_CNF;
        //            tblInvoiceAddressTo.BillingOrgId = bmOrgTO.IdOrganization;

        //            billingStateId = bmOfcAddrTO.StateId;
        //            if (string.IsNullOrEmpty(bmOfcAddrTO.VillageName))
        //            {
        //                if (string.IsNullOrEmpty(bmOfcAddrTO.TalukaName))
        //                {
        //                    tblInvoiceTO.DeliveryLocation = bmOfcAddrTO.DistrictName;
        //                }
        //                tblInvoiceTO.DeliveryLocation = bmOfcAddrTO.TalukaName;
        //            }
        //            else
        //                tblInvoiceTO.DeliveryLocation = bmOfcAddrTO.VillageName;


        //            tblInvoiceTO.InvoiceAddressTOList.Add(tblInvoiceAddressTo);

        //            TblInvoiceAddressTO consigneeInvoiceAddressTo = tblInvoiceAddressTo.DeepCopy();
        //            consigneeInvoiceAddressTo.TxnAddrTypeId = (int)TxnDeliveryAddressTypeE.CONSIGNEE_ADDRESS;
        //            tblInvoiceTO.InvoiceAddressTOList.Add(consigneeInvoiceAddressTo);

        //        }
        //        else if (invoiceGenerateModeE == InvoiceGenerateModeE.BMTOCUSTOMER)
        //        {
        //            foreach (var deliveryAddrTo in invoiceAddressTOList)
        //            {
        //                TblInvoiceAddressTO tblInvoiceAddressTo = new TblInvoiceAddressTO();
        //                tblInvoiceAddressTo.AadharNo = deliveryAddrTo.AadharNo;
        //                tblInvoiceAddressTo.GstinNo = deliveryAddrTo.GstinNo;
        //                tblInvoiceAddressTo.PanNo = deliveryAddrTo.PanNo;
        //                tblInvoiceAddressTo.StateId = deliveryAddrTo.StateId;
        //                tblInvoiceAddressTo.State = deliveryAddrTo.State;
        //                tblInvoiceAddressTo.Taluka = deliveryAddrTo.Taluka;
        //                tblInvoiceAddressTo.District = deliveryAddrTo.District;
        //                tblInvoiceAddressTo.BillingName = deliveryAddrTo.BillingName;
        //                tblInvoiceAddressTo.ContactNo = deliveryAddrTo.ContactNo;
        //                tblInvoiceAddressTo.PinCode = deliveryAddrTo.PinCode;
        //                tblInvoiceAddressTo.TxnAddrTypeId = deliveryAddrTo.TxnAddrTypeId;
        //                tblInvoiceAddressTo.Address = deliveryAddrTo.Address;
        //                tblInvoiceAddressTo.AddrSourceTypeId = deliveryAddrTo.AddrSourceTypeId;
        //                tblInvoiceAddressTo.BillingOrgId = deliveryAddrTo.BillingOrgId;

        //                if (deliveryAddrTo.TxnAddrTypeId == (Int32)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS)
        //                    billingStateId = deliveryAddrTo.StateId;

        //                tblInvoiceTO.InvoiceAddressTOList.Add(tblInvoiceAddressTo);
        //            }
        //        }

        //        if (billingStateId == 0)
        //        {
        //            resultMsg.DefaultBehaviour("Billing State Not Found");
        //            return resultMsg;
        //        }
        //        #endregion

        //        #region 3 Added Invoice Item details


        //        #region Regular Invoice Items i.e from Loading
        //        foreach (var existingInvItemTO in invoiceItemTOList)
        //        {
        //            TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = existingInvItemTO.DeepCopy();
        //            List<TblInvoiceItemTaxDtlsTO> tblInvoiceItemTaxDtlsTOList = new List<TblInvoiceItemTaxDtlsTO>();
        //            TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO = new TblProdGstCodeDtlsTO();
        //            TblProductItemTO tblProductItemTO = new TblProductItemTO();
        //            Double itemGrandTotal = 0;

        //            tblProdGstCodeDtlsTO = BL.TblProdGstCodeDtlsBL.SelectTblProdGstCodeDtlsTO(tblInvoiceItemDetailsTO.ProdGstCodeId, conn, tran);
        //            if (tblProdGstCodeDtlsTO == null)
        //            {
        //                resultMsg.DefaultBehaviour("ProdGSTCodeDetails found null against IdInvoiceItem is : " + tblInvoiceItemDetailsTO.IdInvoiceItem + ".");
        //                resultMsg.DisplayMessage = "GSTIN Not Defined for Item :" + tblInvoiceItemDetailsTO.ProdItemDesc;
        //                return resultMsg;
        //            }
        //            TblGstCodeDtlsTO gstCodeDtlsTO = BL.TblGstCodeDtlsBL.SelectTblGstCodeDtlsTO(tblProdGstCodeDtlsTO.GstCodeId, conn, tran);
        //            if (gstCodeDtlsTO != null)
        //            {
        //                gstCodeDtlsTO.TaxRatesTOList = BL.TblTaxRatesBL.SelectAllTblTaxRatesList(tblProdGstCodeDtlsTO.GstCodeId, conn, tran);
        //            }
        //            if (gstCodeDtlsTO == null)
        //            {
        //                resultMsg.DefaultBehaviour("GST code details found null : " + tblInvoiceItemDetailsTO.ProdItemDesc + ".");
        //                resultMsg.DisplayMessage = "GSTIN Not Defined for Item :" + tblInvoiceItemDetailsTO.ProdItemDesc;
        //                return resultMsg;
        //            }

        //            tblInvoiceItemDetailsTO.GstinCodeNo = gstCodeDtlsTO.CodeNumber;

        //            #region 4 Added Invoice Item Tax details

        //            foreach (var taxRateTo in gstCodeDtlsTO.TaxRatesTOList)
        //            {
        //                TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO = new TblInvoiceItemTaxDtlsTO();
        //                tblInvoiceItemTaxDtlsTO.TaxRateId = taxRateTo.IdTaxRate;
        //                tblInvoiceItemTaxDtlsTO.TaxPct = taxRateTo.TaxPct;
        //                tblInvoiceItemTaxDtlsTO.TaxRatePct = (gstCodeDtlsTO.TaxPct * taxRateTo.TaxPct) / 100;
        //                tblInvoiceItemTaxDtlsTO.TaxableAmt = tblInvoiceItemDetailsTO.TaxableAmt;
        //                tblInvoiceItemTaxDtlsTO.TaxAmt = (tblInvoiceItemTaxDtlsTO.TaxableAmt * tblInvoiceItemTaxDtlsTO.TaxRatePct) / 100;
        //                tblInvoiceItemTaxDtlsTO.TaxTypeId = taxRateTo.TaxTypeId;
        //                if (billingStateId == ofcAddrTO.StateId)
        //                {
        //                    if (taxRateTo.TaxTypeId == (int)Constants.TaxTypeE.CGST)
        //                    {
        //                        cgstTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
        //                        itemGrandTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
        //                        tblInvoiceItemTaxDtlsTOList.Add(tblInvoiceItemTaxDtlsTO);
        //                    }
        //                    else if (taxRateTo.TaxTypeId == (int)Constants.TaxTypeE.SGST)
        //                    {
        //                        sgstTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
        //                        itemGrandTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
        //                        tblInvoiceItemTaxDtlsTOList.Add(tblInvoiceItemTaxDtlsTO);
        //                    }
        //                    else continue;
        //                }
        //                else
        //                {
        //                    if (taxRateTo.TaxTypeId == (int)Constants.TaxTypeE.IGST)
        //                    {
        //                        igstTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
        //                        itemGrandTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
        //                        tblInvoiceItemTaxDtlsTOList.Add(tblInvoiceItemTaxDtlsTO);
        //                    }
        //                    else continue;
        //                }
        //            }
        //            #endregion

        //            basicTotal += existingInvItemTO.BasicTotal;
        //            taxableTotal += existingInvItemTO.TaxableAmt;
        //            discountTotal += existingInvItemTO.CdAmt;

        //            itemGrandTotal += existingInvItemTO.TaxableAmt;

        //            grandTotal += itemGrandTotal;
        //            tblInvoiceItemDetailsTO.GrandTotal = itemGrandTotal;
        //            tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList = tblInvoiceItemTaxDtlsTOList;
        //            tblInvoiceItemDetailsTOList.Add(tblInvoiceItemDetailsTO);
        //        }

        //        #endregion


        //        #endregion

        //        #region 5 Save main Invoice
        //        tblInvoiceTO.TaxableAmt = taxableTotal;
        //        tblInvoiceTO.DiscountAmt = discountTotal;
        //        tblInvoiceTO.IgstAmt = igstTotal;
        //        tblInvoiceTO.CgstAmt = cgstTotal;
        //        tblInvoiceTO.SgstAmt = sgstTotal;
        //        double finalGrandTotal = Math.Round(grandTotal);
        //        tblInvoiceTO.GrandTotal = finalGrandTotal;
        //        tblInvoiceTO.RoundOffAmt = Math.Round(finalGrandTotal - grandTotal, 2);
        //        tblInvoiceTO.BasicAmt = basicTotal;
        //        tblInvoiceTO.InvoiceItemDetailsTOList = tblInvoiceItemDetailsTOList;
        //        #endregion

        //        tblInvoiceTOList.Add(tblInvoiceTO);

        //        resultMsg.DefaultSuccessBehaviour();
        //        resultMsg.Tag = tblInvoiceTOList;
        //        return resultMsg;

        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        resultMsg.DefaultExceptionBehaviour(ex, "PrepareNewInvoiceObjectList");
        //        return resultMsg;
        //    }
        //    finally
        //    {

        //    }
        //}
        #endregion

        public static ResultMessage PrepareAndSaveInternalTaxInvoices(TblInvoiceTO invoiceTO, int invoiceGenerateModeE, int fromOrgId, int toOrgId, int isCalculateWithBaseRate, TblInvoiceChangeOrgHistoryTO changeHisTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMsg = new ResultMessage();
            string entityRangeName = string.Empty;
            Int32 result = 0;
            List<DropDownTO> districtList = new List<DropDownTO>();
            List<DropDownTO> talukaList = new List<DropDownTO>();
            try
            {
                DateTime serverDateTime = Constants.ServerDateTime;
                Int32 invoiceId = invoiceTO.IdInvoice;
                List<TblInvoiceItemDetailsTO> invoiceItemTOList = TblInvoiceItemDetailsBL.SelectAllTblInvoiceItemDetailsList(invoiceId, conn, tran);
                //List<TblInvoiceItemTaxDtlsTO> invoiceItemTaxTOList = TblInvoiceItemTaxDtlsBL.SelectInvoiceItemTaxDtlsListByInvoiceId(invoiceId, conn, tran);
                List<TblInvoiceAddressTO> invoiceAddressTOList = TblInvoiceAddressBL.SelectAllTblInvoiceAddressList(invoiceId, conn, tran);

                #region 1 BRM TO BM Invoice

                //pass changed from org ()    
                List<TblInvoiceItemDetailsTO> invoiceItemChangeList = new List<TblInvoiceItemDetailsTO>();
                invoiceItemTOList.ForEach(d =>
                {
                    invoiceItemChangeList.Add(d.DeepCopy());
                }
                );
                resultMsg = PrepareNewInvoiceObjectList(invoiceTO, invoiceItemTOList, invoiceAddressTOList, invoiceGenerateModeE, fromOrgId, toOrgId, 0, conn, tran, 0);
                if (resultMsg.MessageType == ResultMessageE.Information)
                {
                    if (resultMsg.Tag != null && resultMsg.Tag.GetType() == typeof(List<TblInvoiceTO>))
                    {
                        List<TblInvoiceTO> tblInvoiceTOList = (List<TblInvoiceTO>)resultMsg.Tag;
                        if (tblInvoiceTOList != null)
                        {
                            //Update Existing Invoice
                            TblInvoiceTO invToUpdateTO = tblInvoiceTOList[0]; //Taken 0th Object as it will always go for single invoice at a time.List is return as existing code is used.

                            //Delete existing invoice item taxes details
                            result = TblInvoiceItemTaxDtlsBL.DeleteInvoiceItemTaxDtlsByInvId(invToUpdateTO.IdInvoice, conn, tran);
                            if (result <= -1)
                            {
                                resultMsg.DefaultBehaviour("Error While DeleteInvoiceItemTaxDtlsByInvId");
                                return resultMsg;
                            }

                            //Update Invoice Object
                            invToUpdateTO.UpdatedBy = invoiceTO.CreatedBy;
                            invToUpdateTO.UpdatedOn = Constants.ServerDateTime;
                            invToUpdateTO.InvFromOrgFreeze = 1;
                            result = UpdateTblInvoice(invToUpdateTO, conn, tran);
                            if (result != 1)
                            {
                                resultMsg.DefaultBehaviour("Error While UpdateTblInvoice");
                                return resultMsg;
                            }

                            // Added for insert new created item list and tex details for change invoice mode and calculate with base rate @Kiran

                            //if(isCalculateWithBaseRate == 1 && invoiceGenerateModeE == (int)InvoiceGenerateModeE.DUPLICATE)
                            //{
                            //    for (int invI = 0; invI < invToUpdateTO.InvoiceItemDetailsTOList.Count; invI++)
                            //    {
                            //        TblInvoiceItemDetailsTO itemTO = invToUpdateTO.InvoiceItemDetailsTOList[invI];

                            //        result = _iTblInvoiceItemDetailsBL.InsertTblInvoiceItemDetails(itemTO, conn, tran);
                            //        if (result != 1)
                            //        {
                            //            resultMsg.DefaultBehaviour("Error While InsertTblInvoiceItemDetails");
                            //            return resultMsg;
                            //        }
                            //        if (itemTO.InvoiceItemTaxDtlsTOList != null && itemTO.InvoiceItemTaxDtlsTOList.Count > 0)
                            //        {
                            //            for (int t = 0; t < itemTO.InvoiceItemTaxDtlsTOList.Count; t++)
                            //            {
                            //                itemTO.InvoiceItemTaxDtlsTOList[t].InvoiceItemId = itemTO.IdInvoiceItem;
                            //                result = _iTblInvoiceItemTaxDtlsBL.InsertTblInvoiceItemTaxDtls(itemTO.InvoiceItemTaxDtlsTOList[t], conn, tran);
                            //                if (result != 1)
                            //                {
                            //                    resultMsg.DefaultBehaviour("Error While InsertTblInvoiceItemTaxDtls");
                            //                    return resultMsg;
                            //                }
                            //            }
                            //        }
                            //    }
                            //}else
                            //{
                            // Update Invoice Item Details
                            for (int invI = 0; invI < invToUpdateTO.InvoiceItemDetailsTOList.Count; invI++)
                            {
                                TblInvoiceItemDetailsTO itemTO = invToUpdateTO.InvoiceItemDetailsTOList[invI];
                                result = TblInvoiceItemDetailsBL.UpdateTblInvoiceItemDetails(itemTO, conn, tran);
                                if (result != 1)
                                {
                                    resultMsg.DefaultBehaviour("Error While UpdateTblInvoiceItemDetails");
                                    return resultMsg;
                                }

                                if (itemTO.InvoiceItemTaxDtlsTOList != null && itemTO.InvoiceItemTaxDtlsTOList.Count > 0)
                                {
                                    for (int t = 0; t < itemTO.InvoiceItemTaxDtlsTOList.Count; t++)
                                    {
                                        itemTO.InvoiceItemTaxDtlsTOList[t].InvoiceItemId = itemTO.IdInvoiceItem;
                                        result = TblInvoiceItemTaxDtlsBL.InsertTblInvoiceItemTaxDtls(itemTO.InvoiceItemTaxDtlsTOList[t], conn, tran);
                                        if (result != 1)
                                        {
                                            resultMsg.DefaultBehaviour("Error While InsertTblInvoiceItemTaxDtls");
                                            return resultMsg;
                                        }
                                    }
                                }
                            }
                            // }

                            result = TblInvoiceAddressBL.DeleteTblInvoiceAddressByinvoiceId(invToUpdateTO.IdInvoice, conn, tran);
                            if (result == -1)
                            {
                                resultMsg.DefaultBehaviour("Error While DeleteTblInvoiceItemTaxDtls");
                                return resultMsg;
                            }
                            //Update Existing Address Details
                            for (int ac = 0; ac < invToUpdateTO.InvoiceAddressTOList.Count; ac++)
                            {
                                invToUpdateTO.InvoiceAddressTOList[ac].InvoiceId = invToUpdateTO.IdInvoice;
                                result = TblInvoiceAddressBL.InsertTblInvoiceAddress(invToUpdateTO.InvoiceAddressTOList[ac], conn, tran);
                                if (result != 1)
                                {
                                    resultMsg.DefaultBehaviour("Error While InsertTblInvoiceItemTaxDtls");
                                    return resultMsg;
                                }
                            }
                        }
                    }
                }
                else
                {
                    return resultMsg;
                }

                #endregion

                //duplicate   
                #region 2 BM TO Actual Customer Invoice
                if (invoiceGenerateModeE == (int)InvoiceGenerateModeE.DUPLICATE)
                {
                    //pass second org (org->cust)
                    //resultMsg = PrepareNewInvoiceObjectList(invoiceTO, invoiceItemTOList, invoiceAddressTOList, invoiceGenerateModeE,fromOrgId,toOrgId, 0,conn, tran);
                    //Change method Parameter for new colne @Kiran
                    resultMsg = PrepareNewInvoiceObjectList(invoiceTO, invoiceItemChangeList, invoiceAddressTOList, invoiceGenerateModeE, fromOrgId, toOrgId, isCalculateWithBaseRate, conn, tran);
                    if (resultMsg.MessageType == ResultMessageE.Information)
                    {
                        if (resultMsg.Tag != null && resultMsg.Tag.GetType() == typeof(List<TblInvoiceTO>))
                        {
                            List<TblInvoiceTO> tblInvoiceTOList = (List<TblInvoiceTO>)resultMsg.Tag;
                            if (tblInvoiceTOList != null)
                            {
                                for (int i = 0; i < tblInvoiceTOList.Count; i++)
                                {
                                    tblInvoiceTOList[i].InvFromOrgFreeze = 1;
                                    Boolean isPanPresent = false;
                                    if (tblInvoiceTOList[i].IsTcsApplicable == 1 && tblInvoiceTOList[i].IsConfirmed == 1)
                                    {
                                        tblInvoiceTOList[i].InvoiceAddressTOList.ForEach(element =>
                                        {
                                            if (element.TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS)
                                            {
                                                isPanPresent = IsPanOrGstPresent(element.PanNo, element.GstinNo);
                                            }
                                        });
                                        Double grandTotal = tblInvoiceTOList[i].GrandTotal;
                                        Double taxableAmt = tblInvoiceTOList[i].TaxableAmt;
                                        Double basicTotalAmt = tblInvoiceTOList[i].BasicAmt;
                                        ResultMessage message = AddTcsTOInTaxItemDtls(conn, tran, ref grandTotal, ref taxableAmt, ref basicTotalAmt, isPanPresent, tblInvoiceTOList[i].InvoiceItemDetailsTOList);
                                        tblInvoiceTOList[i].GrandTotal = grandTotal;
                                        tblInvoiceTOList[i].TaxableAmt = taxableAmt;
                                        tblInvoiceTOList[i].BasicAmt = basicTotalAmt;
                                    }
                                    resultMsg = SaveNewInvoice(tblInvoiceTOList[i], conn, tran);
                                    if (resultMsg.MessageType != ResultMessageE.Information)
                                    {
                                        return resultMsg;
                                    }
                                    else
                                    {
                                        changeHisTO.DupInvoiceId = tblInvoiceTOList[i].IdInvoice;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        return resultMsg;
                    }
                }
                #endregion

                resultMsg.DefaultSuccessBehaviour();
                return resultMsg;
            }
            catch (Exception ex)
            {
                resultMsg.DefaultExceptionBehaviour(ex, "PrepareAndSaveNewTaxInvoice");
                return resultMsg;
            }
        }

        public static ResultMessage  PrepareNewInvoiceObjectList(TblInvoiceTO invoiceTO, List<TblInvoiceItemDetailsTO> invoiceItemTOList, List<TblInvoiceAddressTO> invoiceAddressTOList, int invoiceGenerateModeE, int fromOrgId, int toOrgId, int isCalculateWithBaseRate, SqlConnection conn, SqlTransaction tran, int swap = 1)
        {
            ResultMessage resultMsg = new ResultMessage();
            try
            {
                #region Prepare List Of Invoices To Save

                List<TblInvoiceTO> tblInvoiceTOList = new List<TblInvoiceTO>();

                TblInvoiceTO tblInvoiceTO = invoiceTO.DeepCopy();
                tblInvoiceTO.InvoiceAddressTOList = new List<TblInvoiceAddressTO>();
                tblInvoiceTO.InvoiceItemDetailsTOList = new List<TblInvoiceItemDetailsTO>();
                double grandTotal = 0;
                double discountTotal = 0;
                double igstTotal = 0;
                double cgstTotal = 0;
                double sgstTotal = 0;
                double basicTotal = 0;
                double taxableTotal = 0;

                TblConfigParamsTO tblConfigParamsTO = null;
                DateTime serverDateTime = Constants.ServerDateTime;
                Int32 billingStateId = 0;

                //Hrushikesh Need to change here
                // if (invoiceGenerateModeE == (int)Constants.InvoiceGenerateModeE.Duplicate)
                // {
                //     //org1 id (org1->org2)
                //     tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID, conn, tran);
                // }
                // else
                //     //changed from org                
                //     tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_INTERNALTXFER_INVOICE_ORG_ID, conn, tran);

                // if (tblConfigParamsTO == null)
                // {
                //     resultMsg.DefaultBehaviour("Internal Self Organization Not Found in Configuration.");
                //     return resultMsg;
                // }
                // //Hrushikesh Need to change here 
                // Int32 internalOrgId = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
                Int32 internalOrgId = fromOrgId;
                
                if (invoiceGenerateModeE == (int)InvoiceGenerateModeE.DUPLICATE && swap == 0)
                    internalOrgId = toOrgId;
                TblOrganizationTO orgTO = TblOrganizationBL.SelectTblOrganizationTO(internalOrgId, conn, tran);
                TblAddressTO ofcAddrTO = TblAddressBL.SelectOrgAddressWrtAddrType(internalOrgId, Constants.AddressTypeE.OFFICE_ADDRESS, conn, tran);
                if (ofcAddrTO == null)
                {
                    resultMsg.DefaultBehaviour("Address Not Found For Self Organization.");
                    return resultMsg;
                }
                
                List <TblInvoiceItemDetailsTO> tblInvoiceItemDetailsTOList = new List<TblInvoiceItemDetailsTO>();

                #region 1 Preparing main InvoiceTO

                tblInvoiceTO.InvFromOrgId = internalOrgId;
                tblInvoiceTO.CreatedOn = Constants.ServerDateTime;
                tblInvoiceTO.CreatedBy = invoiceTO.CreatedBy;
                tblInvoiceTO.InvoiceDate = tblInvoiceTO.CreatedOn;
                tblInvoiceTO.StatusDate = tblInvoiceTO.InvoiceDate;

                #endregion

                #region 2 Added Invoice Address Details
                if (invoiceGenerateModeE == (int)Constants.InvoiceGenerateModeE.DUPLICATE && swap == 1)
                {
                    tblInvoiceTO.Narration = "To ";

                    TblOrganizationTO tblOrganizationTO = DAL.TblOrganizationDAO.SelectTblOrganization(toOrgId, conn, tran);
                    if (tblOrganizationTO != null)
                    {
                        tblInvoiceTO.Narration += tblOrganizationTO.FirmName;
                        tblInvoiceTO.IsTcsApplicable = tblOrganizationTO.IsTcsApplicable;
                    }
                    else
                        tblInvoiceTO.Narration += "Bhagylaxmi Metal";

                    TblConfigParamsTO configParamsTO =TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_INTERNALTXFER_INVOICE_ORG_ID, conn, tran);

                    if (configParamsTO == null)
                    {
                        resultMsg.DefaultBehaviour("Internal Self Organization Not Found in Configuration.");
                        return resultMsg;
                    }
                    //Hrushikesh Need to change here 
                    //org2 id (org2->cust)
                    // Int32 internalBMOrgId =   Convert.ToInt32(configParamsTO.ConfigParamVal);
                    Int32 internalBMOrgId = toOrgId;
                    TblOrganizationTO bmOrgTO = TblOrganizationBL.SelectTblOrganizationTO(internalBMOrgId, conn, tran);
                    TblAddressTO bmOfcAddrTO = TblAddressBL.SelectOrgAddressWrtAddrType(internalBMOrgId, Constants.AddressTypeE.OFFICE_ADDRESS, conn, tran);
                    if (bmOfcAddrTO == null)
                    {
                        resultMsg.DefaultBehaviour("Address Not Found For BM Organization.");
                        return resultMsg;
                    }

                    tblInvoiceTO.DealerOrgId = internalBMOrgId;  //BMM AS dealer.

                    List<TblOrgLicenseDtlTO> licenseList = TblOrgLicenseDtlDAO.SelectAllTblOrgLicenseDtl(internalBMOrgId, conn, tran);
                    String aadharNo = string.Empty;
                    String gstNo = string.Empty;
                    String panNo = string.Empty;

                    if (licenseList != null)
                    {
                        var panNoObj = licenseList.Where(a => a.LicenseId == (int)Constants.CommercialLicenseE.PAN_NO).FirstOrDefault();
                        if (panNoObj != null)
                            panNo = panNoObj.LicenseValue;
                        var gstinObj = licenseList.Where(a => a.LicenseId == (int)Constants.CommercialLicenseE.IGST_NO).FirstOrDefault();
                        if (gstinObj != null)
                            gstNo = gstinObj.LicenseValue;
                    }

                    TblInvoiceAddressTO tblInvoiceAddressTo = new TblInvoiceAddressTO();
                    tblInvoiceAddressTo.GstinNo = gstNo;
                    tblInvoiceAddressTo.PanNo = panNo;
                    tblInvoiceAddressTo.StateId = bmOfcAddrTO.StateId;
                    tblInvoiceAddressTo.State = bmOfcAddrTO.StateName;
                    tblInvoiceAddressTo.Taluka = bmOfcAddrTO.TalukaName;
                    tblInvoiceAddressTo.District = bmOfcAddrTO.DistrictName;
                    tblInvoiceAddressTo.BillingName = bmOrgTO.FirmName;
                    tblInvoiceAddressTo.ContactNo = bmOrgTO.RegisteredMobileNos;
                    tblInvoiceAddressTo.PinCode = bmOfcAddrTO.Pincode.ToString();
                    tblInvoiceAddressTo.TxnAddrTypeId = (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS;
                    tblInvoiceAddressTo.Address = bmOfcAddrTO.PlotNo + bmOfcAddrTO.StreetName + bmOfcAddrTO.AreaName;
                    tblInvoiceAddressTo.AddrSourceTypeId = (int)Constants.AddressSourceTypeE.FROM_CNF;
                    tblInvoiceAddressTo.BillingOrgId = bmOrgTO.IdOrganization;

                    billingStateId = bmOfcAddrTO.StateId;
                    if (string.IsNullOrEmpty(bmOfcAddrTO.VillageName))
                    {
                        if (string.IsNullOrEmpty(bmOfcAddrTO.TalukaName))
                        {
                            tblInvoiceTO.DeliveryLocation = bmOfcAddrTO.DistrictName;
                        }
                        tblInvoiceTO.DeliveryLocation = bmOfcAddrTO.TalukaName;
                    }
                    else
                        tblInvoiceTO.DeliveryLocation = bmOfcAddrTO.VillageName;


                    tblInvoiceTO.InvoiceAddressTOList.Add(tblInvoiceAddressTo);

                    TblInvoiceAddressTO consigneeInvoiceAddressTo = tblInvoiceAddressTo.DeepCopy();
                    consigneeInvoiceAddressTo.TxnAddrTypeId = (int)Constants.TxnDeliveryAddressTypeE.CONSIGNEE_ADDRESS;
                    tblInvoiceTO.InvoiceAddressTOList.Add(consigneeInvoiceAddressTo);

                }
                else if (invoiceGenerateModeE == (int)Constants.InvoiceGenerateModeE.CHANGEFROM || swap == 0)
                {
                    foreach (var deliveryAddrTo in invoiceAddressTOList)
                    {
                        TblInvoiceAddressTO tblInvoiceAddressTo = new TblInvoiceAddressTO();
                        tblInvoiceAddressTo.AadharNo = deliveryAddrTo.AadharNo;
                        tblInvoiceAddressTo.GstinNo = deliveryAddrTo.GstinNo;
                        tblInvoiceAddressTo.PanNo = deliveryAddrTo.PanNo;
                        tblInvoiceAddressTo.StateId = deliveryAddrTo.StateId;
                        tblInvoiceAddressTo.State = deliveryAddrTo.State;
                        tblInvoiceAddressTo.Taluka = deliveryAddrTo.Taluka;
                        tblInvoiceAddressTo.District = deliveryAddrTo.District;
                        tblInvoiceAddressTo.BillingName = deliveryAddrTo.BillingName;
                        tblInvoiceAddressTo.ContactNo = deliveryAddrTo.ContactNo;
                        tblInvoiceAddressTo.PinCode = deliveryAddrTo.PinCode;
                        tblInvoiceAddressTo.TxnAddrTypeId = deliveryAddrTo.TxnAddrTypeId;
                        tblInvoiceAddressTo.Address = deliveryAddrTo.Address;
                        tblInvoiceAddressTo.AddrSourceTypeId = deliveryAddrTo.AddrSourceTypeId;
                        tblInvoiceAddressTo.BillingOrgId = deliveryAddrTo.BillingOrgId;

                        if (deliveryAddrTo.TxnAddrTypeId == (Int32)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS)
                            billingStateId = deliveryAddrTo.StateId;

                        tblInvoiceTO.InvoiceAddressTOList.Add(tblInvoiceAddressTo);
                    }
                }

                if (billingStateId == 0)
                {
                    resultMsg.DefaultBehaviour("Billing State Not Found");
                    return resultMsg;
                }
                #endregion

                #region 3 Added Invoice Item details


                #region Regular Invoice Items i.e from Loading
                //Added By Kiran new changes for invoice calculate using base rate
                //Commented For BRM
                //if (isCalculateWithBaseRate == 1 && invoiceGenerateModeE == (int)InvoiceGenerateModeE.DUPLICATE)
                //{
                //    List<TblInvoiceItemDetailsTO> tblInvoiceItemDetailsTOsList = new List<TblInvoiceItemDetailsTO>();
                //    TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = invoiceItemTOList[0];

                //    tblInvoiceItemDetailsTO.InvoiceQty = invoiceItemTOList.Sum(s => s.InvoiceQty);
                //    tblInvoiceItemDetailsTO.CdStructure = 0;
                //    tblInvoiceItemDetailsTO.CdAmt = 0;
                //    tblInvoiceItemDetailsTO.LoadingSlipExtId = 0;
                //    tblInvoiceItemDetailsTO.CdStructureId = 0;

                //    Double sum = 0;
                //    //var resultDelInvoice = _iTblInvoiceItemTaxDtlsBL.DeleteInvoiceItemTaxDtlsByInvId(tblInvoiceTO.IdInvoice, conn, tran);
                //    //if (resultDelInvoice <= -1)
                //    //{
                //    //    resultMsg.DefaultBehaviour("Error While DeleteInvoiceItemTaxDtlsByInvId");
                //    //    return resultMsg;
                //    //}
                //    for (int i = 0; i < invoiceItemTOList.Count; i++)
                //    {
                //        Double bundles = 0;
                //        bool result = double.TryParse(invoiceItemTOList[i].Bundles, out bundles);
                //        if (result)
                //        {
                //            sum += bundles;
                //        }
                //        //var resultItem = _iTblInvoiceItemDetailsBL.DeleteTblInvoiceItemDetails(invoiceItemTOList[i].IdInvoiceItem, conn, tran);
                //        //if (resultItem != 1)
                //        //{
                //        //    resultMsg.DefaultBehaviour("Error While DeleteTblInvoiceItemDetails");
                //        //    return resultMsg;
                //        //}

                //    }

                //    tblInvoiceItemDetailsTO.Bundles = Convert.ToString(sum);
                //    TblBookingsTO TblBookingsTO = _iTblBookingsBL.SelectBookingsDetailsFromInVoiceId(tblInvoiceTO.IdInvoice, conn, tran);
                //    if (TblBookingsTO == null)
                //    {
                //        resultMsg.DefaultBehaviour("Booking details not found");
                //        resultMsg.DisplayMessage = "Booking Details Not Found for invoice Id " + tblInvoiceTO.IdInvoice;
                //        return resultMsg;
                //    }
                //    Int32 isTaxInclusiveWithTaxes = 0;
                //    TblConfigParamsTO rateCalcConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_RATE_CALCULATIONS_TAX_INCLUSIVE, conn, tran);
                //    if (rateCalcConfigParamsTO != null)
                //    {
                //        isTaxInclusiveWithTaxes = Convert.ToInt32(rateCalcConfigParamsTO.ConfigParamVal);
                //    }
                //    Int32 isTaxInclusive = 0;
                //    DimBrandTO dimBrandTO = _iDimBrandDAO.SelectDimBrand(TblBookingsTO.BrandId);

                //    if (dimBrandTO != null)
                //    {
                //        isTaxInclusive = dimBrandTO.IsTaxInclusive;
                //    }
                //    if (isTaxInclusive == 1 && isTaxInclusiveWithTaxes == 0)
                //    {
                //        TblBookingsTO.BookingRate = TblBookingsTO.BookingRate / 1.18;
                //        TblBookingsTO.BookingRate = Math.Round(TblBookingsTO.BookingRate, 2);
                //    }
                //    tblInvoiceItemDetailsTO.Rate = TblBookingsTO.BookingRate;
                //    tblInvoiceItemDetailsTO.BasicTotal = tblInvoiceItemDetailsTO.Rate * tblInvoiceItemDetailsTO.InvoiceQty;
                //    tblInvoiceItemDetailsTO.TaxableAmt = tblInvoiceItemDetailsTO.BasicTotal - tblInvoiceItemDetailsTO.CdAmt;
                //    invoiceTO.TaxableAmt = tblInvoiceItemDetailsTO.TaxableAmt;
                //    //To get INTERNAL DEFAULT ITEM
                //    TblConfigParamsTO tblConfigParamForInternalItem = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.INTERNAL_DEFAULT_ITEM, conn, tran);

                //    if (tblConfigParamForInternalItem == null)
                //    {
                //        tran.Rollback();
                //        resultMsg.DefaultBehaviour("Internal INTERNAL DEFAULT ITEM Not Found in Configuration.");
                //        return resultMsg;
                //    }

                //    Int32 prodItemId = Convert.ToInt32(tblConfigParamForInternalItem.ConfigParamVal);
                //    if (prodItemId == 0)
                //    {
                //        tran.Rollback();
                //        resultMsg.DefaultBehaviour("Internal INTERNAL DEFAULT ITEM Not Found in Configuration.");
                //        return resultMsg;
                //    }

                //    TblProductItemTO tblProductItemTO = _iTblProductItemDAO.SelectTblProductItem(prodItemId);
                //    if (tblProductItemTO == null)
                //    {
                //        tran.Rollback();
                //        resultMsg.DefaultBehaviour("Internal INTERNAL DEFAULT ITEM Not Found in Configuration.");
                //        return resultMsg;
                //    }

                //    tblInvoiceItemDetailsTO.ProdItemDesc = tblProductItemTO.ItemDesc;

                //    //tblInvoiceItemDetailsTO.ProdGstCodeId = Convert.ToInt32(tblConfigParamForInternalItem.ConfigParamVal);

                //    TblProdGstCodeDtlsTO tblProdGstCodeDtlsTOTemp = _iTblProdGstCodeDtlsDAO.SelectTblProdGstCodeDtls(0, 0, 0, prodItemId, 0, conn, tran);

                //    if (tblProdGstCodeDtlsTOTemp == null)
                //    {
                //        tran.Rollback();
                //        resultMsg.DefaultBehaviour("Please define GST code for item Id - " + prodItemId);
                //        return resultMsg;
                //    }

                //    tblInvoiceItemDetailsTO.ProdGstCodeId = tblProdGstCodeDtlsTOTemp.IdProdGstCode;

                //    tblInvoiceItemDetailsTOsList.Add(tblInvoiceItemDetailsTO);

                //    invoiceItemTOList = tblInvoiceItemDetailsTOsList;
                //}
                Int32 tcsId = 5;
                TblConfigParamsTO tcsConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_TCS_OTHER_TAX_ID, conn, tran);
                if (tcsConfigParamsTO != null)
                {
                    tcsId = Convert.ToInt32(tcsConfigParamsTO.ConfigParamVal);
                }
                foreach (var existingInvItemTO in invoiceItemTOList)
                {
                    Boolean ItsTcs = false;
                    if (invoiceGenerateModeE == (int)InvoiceGenerateModeE.DUPLICATE && existingInvItemTO.OtherTaxId == tcsId && swap == 1)
                    {
                        ItsTcs = true;
                    }
                    if(ItsTcs == false)
                    {
                        TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = existingInvItemTO.DeepCopy();
                        List<TblInvoiceItemTaxDtlsTO> tblInvoiceItemTaxDtlsTOList = new List<TblInvoiceItemTaxDtlsTO>();
                        TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO = new TblProdGstCodeDtlsTO();
                        TblProductItemTO tblProductItemTO = new TblProductItemTO();
                        Double itemGrandTotal = 0;

                        tblProdGstCodeDtlsTO = TblProdGstCodeDtlsDAO.SelectTblProdGstCodeDtls(tblInvoiceItemDetailsTO.ProdGstCodeId, conn, tran);
                        if (tblProdGstCodeDtlsTO == null)
                        {
                            resultMsg.DefaultBehaviour("ProdGSTCodeDetails found null against IdInvoiceItem is : " + tblInvoiceItemDetailsTO.IdInvoiceItem + ".");
                            resultMsg.DisplayMessage = "GSTIN Not Defined for Item :" + tblInvoiceItemDetailsTO.ProdItemDesc;
                            return resultMsg;
                        }
                        TblGstCodeDtlsTO gstCodeDtlsTO = TblGstCodeDtlsDAO.SelectTblGstCodeDtls(tblProdGstCodeDtlsTO.GstCodeId, conn, tran);
                        if (gstCodeDtlsTO != null)
                        {
                            gstCodeDtlsTO.TaxRatesTOList = TblTaxRatesDAO.SelectAllTblTaxRates(tblProdGstCodeDtlsTO.GstCodeId, conn, tran);
                        }
                        if (gstCodeDtlsTO == null)
                        {
                            resultMsg.DefaultBehaviour("GST code details found null : " + tblInvoiceItemDetailsTO.ProdItemDesc + ".");
                            resultMsg.DisplayMessage = "GSTIN Not Defined for Item :" + tblInvoiceItemDetailsTO.ProdItemDesc;
                            return resultMsg;
                        }

                        tblInvoiceItemDetailsTO.GstinCodeNo = gstCodeDtlsTO.CodeNumber;

                        #region 4 Added Invoice Item Tax details

                        foreach (var taxRateTo in gstCodeDtlsTO.TaxRatesTOList)
                        {
                            TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO = new TblInvoiceItemTaxDtlsTO();
                            tblInvoiceItemTaxDtlsTO.TaxRateId = taxRateTo.IdTaxRate;
                            tblInvoiceItemTaxDtlsTO.TaxPct = taxRateTo.TaxPct;
                            //Harshala
                            if (existingInvItemTO.OtherTaxId > 0)
                            {
                                TblOtherTaxesTO tblOtherTaxesTO = DAL.TblOtherTaxesDAO.SelectTblOtherTaxes(existingInvItemTO.OtherTaxId);
                                if (tblOtherTaxesTO != null && tblOtherTaxesTO.IsAfter == 1)
                                {
                                    taxRateTo.TaxPct = 0;
                                    tblInvoiceItemTaxDtlsTO.TaxPct = 0;
                                }
                            }
                            //
                            tblInvoiceItemTaxDtlsTO.TaxRatePct = (gstCodeDtlsTO.TaxPct * taxRateTo.TaxPct) / 100;
                            tblInvoiceItemTaxDtlsTO.TaxableAmt = tblInvoiceItemDetailsTO.TaxableAmt;
                            tblInvoiceItemTaxDtlsTO.TaxAmt = (tblInvoiceItemTaxDtlsTO.TaxableAmt * tblInvoiceItemTaxDtlsTO.TaxRatePct) / 100;
                            tblInvoiceItemTaxDtlsTO.TaxTypeId = taxRateTo.TaxTypeId;
                            if (billingStateId == ofcAddrTO.StateId)
                            {
                                if (taxRateTo.TaxTypeId == (int)Constants.TaxTypeE.CGST)
                                {
                                    cgstTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                    itemGrandTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                    tblInvoiceItemTaxDtlsTOList.Add(tblInvoiceItemTaxDtlsTO);
                                }
                                else if (taxRateTo.TaxTypeId == (int)Constants.TaxTypeE.SGST)
                                {
                                    sgstTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                    itemGrandTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                    tblInvoiceItemTaxDtlsTOList.Add(tblInvoiceItemTaxDtlsTO);
                                }
                                else continue;
                            }
                            else
                            {
                                if (taxRateTo.TaxTypeId == (int)Constants.TaxTypeE.IGST)
                                {
                                    igstTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                    itemGrandTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                    tblInvoiceItemTaxDtlsTOList.Add(tblInvoiceItemTaxDtlsTO);
                                }
                                else continue;
                            }
                        }
                        #endregion

                        basicTotal += existingInvItemTO.BasicTotal;
                        taxableTotal += existingInvItemTO.TaxableAmt;
                        discountTotal += existingInvItemTO.CdAmt;

                        itemGrandTotal += existingInvItemTO.TaxableAmt;

                        grandTotal += itemGrandTotal;
                        tblInvoiceItemDetailsTO.GrandTotal = itemGrandTotal;
                        tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList = tblInvoiceItemTaxDtlsTOList;
                        tblInvoiceItemDetailsTOList.Add(tblInvoiceItemDetailsTO);
                    }
                }
                #endregion


                #endregion

                #region 5 Save main Invoice
                tblInvoiceTO.TaxableAmt = taxableTotal;
                tblInvoiceTO.DiscountAmt = discountTotal;
                tblInvoiceTO.IgstAmt = igstTotal;
                tblInvoiceTO.CgstAmt = cgstTotal;
                tblInvoiceTO.SgstAmt = sgstTotal;
                double finalGrandTotal = Math.Round(grandTotal);
                tblInvoiceTO.GrandTotal = finalGrandTotal;
                tblInvoiceTO.RoundOffAmt = Math.Round(finalGrandTotal - grandTotal, 2);
                tblInvoiceTO.BasicAmt = basicTotal;
                tblInvoiceTO.InvoiceItemDetailsTOList = tblInvoiceItemDetailsTOList;
                if (invoiceGenerateModeE == (int)InvoiceGenerateModeE.DUPLICATE && swap == 1)
                {
                    tblInvoiceTO.TdsAmt = 0;
                    if (tblInvoiceTO.IsTcsApplicable == 0)
                    {
                        Double tdsTaxPct = 0;
                        TblConfigParamsTO tdsConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DELIVER_INVOICE_TDS_TAX_PCT, conn, tran);
                        if (tdsConfigParamsTO != null)
                        {
                            if (!String.IsNullOrEmpty(tdsConfigParamsTO.ConfigParamVal))
                            {
                                tdsTaxPct = Convert.ToDouble(tdsConfigParamsTO.ConfigParamVal);
                            }
                        }
                        if (tblInvoiceTO.IsConfirmed == 1)
                        {
                            tblInvoiceTO.TdsAmt = (CalculateTDS(tblInvoiceTO) * tdsTaxPct) / 100;
                            tblInvoiceTO.TdsAmt = Math.Ceiling(tblInvoiceTO.TdsAmt);
                        }
                    }
                    
                }
                #endregion

                tblInvoiceTOList.Add(tblInvoiceTO);

                resultMsg.DefaultSuccessBehaviour();
                resultMsg.Tag = tblInvoiceTOList;
                return resultMsg;

                #endregion
            }
            catch (Exception ex)
            {
                resultMsg.DefaultExceptionBehaviour(ex, "PrepareNewInvoiceObjectList");
                return resultMsg;
            }
            finally
            {

            }
        }


        /// <summary>
        /// Vijaymala[22-05-2018] : Added To save invoice document details.
        /// </summary>
        /// <returns></returns>
        /// 
        public static ResultMessage SaveInvoiceDocumentDetails(TblInvoiceTO invoiceTO, List<TblDocumentDetailsTO> tblDocumentDetailsTOList, Int32 loginUserId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMSg = new ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                resultMSg = InsertInvoiceDocumentDetails(invoiceTO, tblDocumentDetailsTOList, loginUserId, conn, tran);
                if (resultMSg.MessageType == ResultMessageE.Information)
                {
                    tran.Commit();
                    resultMSg.DefaultSuccessBehaviour();
                }
                else
                {
                    tran.Rollback();
                }
                return resultMSg;
            }
            catch (Exception ex)
            {
                resultMSg.DefaultExceptionBehaviour(ex, "");
                return resultMSg;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Vijaymala[22-05-2018] : Added To save invoice document details.
        /// </summary>
        /// <returns></returns>
        /// 
        public static ResultMessage InsertInvoiceDocumentDetails(TblInvoiceTO tblInvoiceTO, List<TblDocumentDetailsTO> tblDocumentDetailsTOList, Int32 loginUserId, SqlConnection conn, SqlTransaction tran)
        {
            int result = 0;
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                #region 1. Save the Document Details

                resultMessage = BL.TblDocumentDetailsBL.UploadDocumentWithConnTran(tblDocumentDetailsTOList, conn, tran);
                if (resultMessage.MessageType != ResultMessageE.Information)
                {
                    resultMessage.DefaultBehaviour("Error To Upload Documenrt");
                    return resultMessage;
                }

                #endregion

                #region 2. Save the Invoice Document Linking 
                if (tblInvoiceTO == null)
                {
                    resultMessage.DefaultBehaviour("Error : InvoiceTO Found Empty Or Null");
                    return resultMessage;
                }
                TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO = new TempInvoiceDocumentDetailsTO();
                if (resultMessage != null && resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(List<TblDocumentDetailsTO>))
                {
                    List<TblDocumentDetailsTO> tblDocumentDetailsTOListTemp = (List<TblDocumentDetailsTO>)resultMessage.Tag;
                    if (tblDocumentDetailsTOListTemp == null && tblDocumentDetailsTOListTemp.Count == 0)
                    {
                        resultMessage.DefaultBehaviour("Error : Document List Found Empty Or Null");
                        return resultMessage;
                    }

                    DateTime serverDateTime = Constants.ServerDateTime;
                    tempInvoiceDocumentDetailsTO.DocumentId = tblDocumentDetailsTOListTemp[0].IdDocument;
                    tempInvoiceDocumentDetailsTO.InvoiceId = tblInvoiceTO.IdInvoice;
                    tempInvoiceDocumentDetailsTO.CreatedBy = loginUserId;
                    tempInvoiceDocumentDetailsTO.CreatedOn = Constants.ServerDateTime;
                    tempInvoiceDocumentDetailsTO.IsActive = 1;
                    result = BL.TempInvoiceDocumentDetailsBL.InsertTempInvoiceDocumentDetails(tempInvoiceDocumentDetailsTO, conn, tran);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error in Insert InvoiceTbl");
                        return resultMessage;
                    }
                }
                else
                {
                    resultMessage.DefaultBehaviour("Error To Upload Documenrt");
                    return resultMessage;
                }



                #endregion



                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblInvoice");
                return resultMessage;
            }
            finally
            {

            }

        }
        #region Post Invoice to SAP
        public static ResultMessage PostSalesInvoiceToSAP(TblInvoiceTO tblInvoiceTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                if (Startup.CompanyObject == null)
                {
                    resultMessage.DefaultBehaviour("SAP Company Object Found NULL");
                    return resultMessage;
                }
                string TxnId = "";
                #region 1. Create Sale Order Against Invoice
                SAPbobsCOM.Documents saleOrderDocument;
                saleOrderDocument = (SAPbobsCOM.Documents)Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                saleOrderDocument.CardCode = tblInvoiceTO.DealerOrgId.ToString();
                saleOrderDocument.CardCode = "845";
                saleOrderDocument.CardName = tblInvoiceTO.DealerName;
                saleOrderDocument.DocDate = Constants.ServerDateTime;
                saleOrderDocument.DocDueDate = saleOrderDocument.DocDate;
                saleOrderDocument.EndDeliveryDate = saleOrderDocument.DocDate;
                Dictionary<string, double> itemQtyDCT = new Dictionary<string, double>();
                int itemCount = 0;
                for (int i = 0; i < tblInvoiceTO.InvoiceItemDetailsTOList.Count; i++)
                {
                    //Freight Or Other taxes to ignore from this
                    if (tblInvoiceTO.InvoiceItemDetailsTOList[i].OtherTaxId > 0)
                        continue;

                    saleOrderDocument.Lines.SetCurrentLine(itemCount);

                    #region Get Item code Details from RM to FG Configuration and Prod Gst Code Number
                    if (tblInvoiceTO.InvoiceItemDetailsTOList[i].ProdGstCodeId == 0)
                    {
                        resultMessage.DefaultBehaviour("ProdGstCodeId - GSTINCode/HSN Code not found for this item :" + tblInvoiceTO.InvoiceItemDetailsTOList[i].ProdItemDesc);
                        return resultMessage;
                    }

                    TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO = TblProdGstCodeDtlsBL.SelectTblProdGstCodeDtlsTO(tblInvoiceTO.InvoiceItemDetailsTOList[i].ProdGstCodeId);
                    if (tblProdGstCodeDtlsTO == null)
                    {
                        resultMessage.DefaultBehaviour("tblProdGstCodeDtlsTO Found NULL. Hence Mapped Sap Item Can not be found");
                        return resultMessage;
                    }

                    Int64 productItemId = DAL.DimensionDAO.GetProductItemIdFromGivenRMDetails(tblProdGstCodeDtlsTO.ProdCatId, tblProdGstCodeDtlsTO.ProdSpecId, tblProdGstCodeDtlsTO.MaterialId, 0, tblProdGstCodeDtlsTO.ProdItemId);
                    if (productItemId <= 0)
                    {
                        resultMessage.DefaultBehaviour("Invoice Item and FG Item Linkgae Not Found in configuration: Check tblProductItemRmToFGConfig");
                        return resultMessage;
                    }

                    saleOrderDocument.Lines.ItemCode = productItemId.ToString();
                    itemQtyDCT.Add(saleOrderDocument.Lines.ItemCode, tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceQty);

                    #endregion

                    #region Quantity, Price and discount details

                    saleOrderDocument.Lines.Quantity = tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceQty;
                    saleOrderDocument.Lines.UnitPrice = tblInvoiceTO.InvoiceItemDetailsTOList[i].Rate;
                    saleOrderDocument.Lines.DiscountPercent = tblInvoiceTO.InvoiceItemDetailsTOList[i].CdStructure;

                    #endregion

                    #region Get Tax Details

                    string sapTaxCode = string.Empty;
                    if (tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList != null
                        && tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList.Count > 0)
                    {
                        TblTaxRatesTO tblTaxRatesTO = TblTaxRatesDAO.SelectTblTaxRates(tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList[0].TaxRateId);
                        if (tblTaxRatesTO != null)
                            sapTaxCode = tblTaxRatesTO.SapTaxCode;
                    }
                    saleOrderDocument.Lines.TaxCode = sapTaxCode;

                    #endregion

                    saleOrderDocument.Lines.Add();
                    itemCount++;
                }
                List<TblOtherTaxesTO> tblOtherTaxesTOList = DAL.DimensionDAO.GetAllActiveOtherTaxesList();
                if (tblOtherTaxesTOList == null || tblOtherTaxesTOList.Count == 0)
                {
                    string errorMsg = "Other Tax Details(PF,Freight) Not found in Masters";
                    resultMessage.DefaultBehaviour(errorMsg);
                    return resultMessage;
                }
                int ExpenseCount = 0;
                for (int i = 0; i < tblInvoiceTO.InvoiceItemDetailsTOList.Count; i++)
                {
                    if (tblInvoiceTO.InvoiceItemDetailsTOList[i].OtherTaxId == 0)
                        continue;

                    saleOrderDocument.Expenses.SetCurrentLine(ExpenseCount);
                    var matchingTO = tblOtherTaxesTOList.Where(x => x.IdOtherTax == tblInvoiceTO.InvoiceItemDetailsTOList[i].OtherTaxId).FirstOrDefault();
                    if (matchingTO == null)
                    {
                        string errorMsg = "Other Tax Details(PF,Freight) Not found in Masters";
                        resultMessage.DefaultBehaviour(errorMsg);
                        return resultMessage;
                    }
                    saleOrderDocument.Expenses.ExpenseCode = Convert.ToInt32(matchingTO.SapExpenseCode);
                    saleOrderDocument.Expenses.LineTotal = tblInvoiceTO.InvoiceItemDetailsTOList[i].TaxableAmt;
                    //oGRN.Expenses.TaxSum = otherExpenseList[oe].TaxAmt;
                    ResultMessage taxCodeMsg = GetSAPEquivalentTaxCode(tblInvoiceTO, tblInvoiceTO.InvoiceItemDetailsTOList[i]);
                    if (taxCodeMsg != null && taxCodeMsg.MessageType != ResultMessageE.Information)
                    {
                        return taxCodeMsg;
                    }
                    else
                        saleOrderDocument.Expenses.TaxCode = (string)taxCodeMsg.data;

                    saleOrderDocument.Expenses.LineGross = tblInvoiceTO.InvoiceItemDetailsTOList[i].GrandTotal;
                    saleOrderDocument.Expenses.Add();
                    ExpenseCount++;
                }
                int result = saleOrderDocument.Add();
                if (result != 0)
                {
                    string errorMsg = Startup.CompanyObject.GetLastErrorDescription();
                    resultMessage.DefaultBehaviour(errorMsg);
                    resultMessage.DisplayMessage = errorMsg;
                    return resultMessage;
                }
                else
                {
                    TxnId = Startup.CompanyObject.GetNewObjectKey();
                    tblInvoiceTO.SapMappedSalesOrderNo = TxnId;
                    resultMessage.DefaultSuccessBehaviour();
                }
                #endregion

                #region 2. Do Stock Adjustment Against Invoice Items

                #endregion

                #region 3. Create Sale Invoice Against Above Order Ref
                if (String.IsNullOrEmpty(TxnId))
                {
                    resultMessage.DefaultBehaviour("Failed to get sales order details");
                    return resultMessage;
                }

                SAPbobsCOM.Documents saleOrder;
                saleOrder = (SAPbobsCOM.Documents)Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                saleOrder.GetByKey(Convert.ToInt32(TxnId));

                SAPbobsCOM.Documents oInvoice;
                oInvoice = (SAPbobsCOM.Documents)Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                oInvoice.GSTTransactionType = SAPbobsCOM.GSTTransactionTypeEnum.gsttrantyp_GSTTaxInvoice;
                oInvoice.CardCode = saleOrder.CardCode;
                oInvoice.DocDate = saleOrder.DocDate;
                oInvoice.DocDueDate = saleOrder.DocDueDate;
                oInvoice.TaxDate = saleOrder.TaxDate;
                oInvoice.EndDeliveryDate = Constants.ServerDateTime;
                //oInvoice.TaxInvoiceNo = tblInvoiceTO.InvoiceNo;
                //oInvoice.TaxInvoiceDate = Constants.ServerDateTime;
                for (int i = 0; i < saleOrder.Lines.Count; i++)
                {
                    if (String.IsNullOrEmpty(saleOrder.Lines.ItemCode))
                        continue;
                    saleOrder.Lines.SetCurrentLine(i);
                    oInvoice.Lines.SetCurrentLine(i);
                    oInvoice.Lines.BaseEntry = Convert.ToInt32(TxnId);
                    oInvoice.Lines.BaseType = (Int32)SAPbobsCOM.BoObjectTypes.oOrders;
                    oInvoice.Lines.ItemCode = saleOrder.Lines.ItemCode;
                    oInvoice.Lines.DiscountPercent = saleOrder.Lines.DiscountPercent;
                    oInvoice.Lines.WarehouseCode = saleOrder.Lines.WarehouseCode;
                    oInvoice.Lines.TaxCode = saleOrder.Lines.TaxCode;
                    oInvoice.Lines.Quantity = saleOrder.Lines.Quantity;
                    oInvoice.Lines.UnitPrice = saleOrder.Lines.UnitPrice;
                    oInvoice.Lines.BaseLine = i;
                    oInvoice.Lines.Add();
                }
                for (int j = 0; j < saleOrder.Expenses.Count; j++)
                {
                    if (String.IsNullOrEmpty(saleOrder.Expenses.TaxCode))
                        continue;
                    saleOrder.Expenses.SetCurrentLine(j);
                    oInvoice.Expenses.SetCurrentLine(j);
                    oInvoice.Expenses.ExpenseCode = saleOrder.Expenses.ExpenseCode;
                    oInvoice.Expenses.LineTotal = saleOrder.Expenses.LineTotal;
                    oInvoice.Expenses.TaxCode = saleOrder.Expenses.TaxCode;
                    oInvoice.Expenses.LineGross = saleOrder.Expenses.LineGross;
                    oInvoice.Expenses.Add();
                }
                result = oInvoice.Add();
                if (result != 0)
                {
                    string errorMsg = Startup.CompanyObject.GetLastErrorDescription();
                    resultMessage.DefaultBehaviour(errorMsg);
                    resultMessage.DisplayMessage = errorMsg;
                    return resultMessage;
                }
                else
                {
                    string InvoiceTxnId = Startup.CompanyObject.GetNewObjectKey();
                    tblInvoiceTO.SapMappedSalesInvoiceNo = InvoiceTxnId;
                    resultMessage.DefaultSuccessBehaviour();
                    resultMessage.data = tblInvoiceTO;
                    return resultMessage;
                }
                #endregion
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostSalesInvoiceToSAP");
                return resultMessage;
            }
        }
        #endregion
        private static ResultMessage GetSAPEquivalentTaxCode(TblInvoiceTO tblInvoiceTO, TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                if (tblInvoiceItemDetailsTO.ProdGstCodeId == 0)
                {
                    resultMessage.DefaultBehaviour("Invalid SAPTaxCode");
                    return resultMessage;
                }
                List<TblProdGstCodeDtlsTO> SapTaxCodeList = DAL.DimensionDAO.getSAPTaxCodeByIdProdGstCode(Convert.ToInt32(tblInvoiceItemDetailsTO.ProdGstCodeId));
                if (SapTaxCodeList == null || SapTaxCodeList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("Invalid SAPTaxCode");
                    return resultMessage;
                }
                string sapTaxCode = string.Empty;
                var sameStateTaxCode = SapTaxCodeList.Where(m => m.TaxTypeId == 2).ToList();
                var diffStateTaxCode = SapTaxCodeList.Where(n => n.TaxTypeId == 1).ToList();
                if (sameStateTaxCode == null || sameStateTaxCode.Count == 0 || diffStateTaxCode == null || diffStateTaxCode.Count == 0)
                {
                    resultMessage.DefaultBehaviour("Failed To Get SAPTaxCode List");
                    return resultMessage;
                }
                if (tblInvoiceTO.InvoiceAddressTOList == null || tblInvoiceTO.InvoiceAddressTOList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("Invalid Address List");
                    return resultMessage;
                }

                var billingFromAddress = tblInvoiceTO.InvoiceAddressTOList.Where(m => m.TxnAddrTypeId == (int)Constants.TxnAddressTypeE.BILLING_FROM_ADDRESS).ToList();
                var billingAddress = tblInvoiceTO.InvoiceAddressTOList.Where(n => n.TxnAddrTypeId == (int)Constants.TxnAddressTypeE.BILLING).ToList();

                if (billingFromAddress[0].StateId == billingAddress[0].StateId)
                {
                    sapTaxCode = sameStateTaxCode[0].SapTaxCode;
                }
                else
                {
                    sapTaxCode = diffStateTaxCode[0].SapTaxCode;
                }

                resultMessage.data = sapTaxCode;
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
        }
        #endregion

        #region Updation
        public static int UpdateTblInvoice(TblInvoiceTO tblInvoiceTO)
        {
            return TblInvoiceDAO.UpdateTblInvoice(tblInvoiceTO);
        }

        public static int UpdateTblInvoice(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceDAO.UpdateTblInvoice(tblInvoiceTO, conn, tran);
        }

        public static int UpdateTblInvoiceFinalTareWt(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceDAO.UpdateTblInvoiceFinalTareWt(tblInvoiceTO, conn, tran);
        }

        public static ResultMessage SaveUpdatedInvoice(TblInvoiceTO invoiceTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMSg = new ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                resultMSg = UpdateInvoice(invoiceTO, conn, tran);
                if (resultMSg.MessageType == ResultMessageE.Information)
                {
                    tran.Commit();
                    resultMSg.DefaultSuccessBehaviour();
                }
                else
                {
                    tran.Rollback();
                }
                return resultMSg;
            }
            catch (Exception ex)
            {
                resultMSg.DefaultExceptionBehaviour(ex, "");
                return resultMSg;
            }
            finally
            {
                conn.Close();
            }
        }

        public static ResultMessage UpdateInvoice(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            int result = 0;
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            resultMessage.Text = "Not Entered In The Loop";
            Boolean isAddrChanged = false;
            Boolean isCdChanged = false;
            Boolean isRateChange = false;
            String changeIn = string.Empty;
            try
            {
                #region 1. Update the Invoice
                List<TblInvoiceHistoryTO> invHistoryList = new List<TblInvoiceHistoryTO>();
                TblInvoiceTO existingInvoiceTO = SelectTblInvoiceTOWithDetails(tblInvoiceTO.IdInvoice, conn, tran);
                if (existingInvoiceTO == null)
                {
                    resultMessage.DefaultBehaviour("existingInvoiceTO Object Not Found");
                    return resultMessage;
                }
                //TblInvoiceTO invoiceTO = BL.TblInvoiceBL.SelectTblInvoiceTO(invoiceId);
                if (tblInvoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.REJECTED_BY_DIRECTOR || tblInvoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.REJECTED_BY_DISTRIBUTOR)
                {
                    TblInvoiceTO existStatusinvoiceTO = BL.TblInvoiceBL.SelectTblInvoiceTO(tblInvoiceTO.IdInvoice);
                    if (existStatusinvoiceTO.StatusId == tblInvoiceTO.StatusId)
                    {
                        tblInvoiceTO.StatusId = (int)Constants.InvoiceStatusE.NEW;
                    }
                }

                /*GJ@20170926: Update the Rate if Status is Rejected By Director Status*/
                if (tblInvoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.REJECTED_BY_DIRECTOR)
                {
                    existingInvoiceTO = updateInvoiceToCalc(existingInvoiceTO, conn, tran);
                    tblInvoiceTO.BasicAmt = existingInvoiceTO.BasicAmt;
                    tblInvoiceTO.TaxableAmt = existingInvoiceTO.TaxableAmt;
                    tblInvoiceTO.DiscountAmt = existingInvoiceTO.DiscountAmt;
                    tblInvoiceTO.IgstAmt = existingInvoiceTO.IgstAmt;
                    tblInvoiceTO.CgstAmt = existingInvoiceTO.CgstAmt;
                    tblInvoiceTO.SgstAmt = existingInvoiceTO.SgstAmt;
                    tblInvoiceTO.GrandTotal = existingInvoiceTO.GrandTotal;

                    tblInvoiceTO.InvoiceItemDetailsTOList = existingInvoiceTO.InvoiceItemDetailsTOList;
                    Double tdsTaxPct = 0;
                    if (existingInvoiceTO != null)
                    {
                        if (existingInvoiceTO.IsTcsApplicable == 0)
                        {
                            TblConfigParamsTO tdsConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DELIVER_INVOICE_TDS_TAX_PCT, conn, tran);
                            if (tdsConfigParamsTO != null)
                            {
                                if (!String.IsNullOrEmpty(tdsConfigParamsTO.ConfigParamVal))
                                {
                                    tdsTaxPct = Convert.ToDouble(tdsConfigParamsTO.ConfigParamVal);
                                }
                            }
                        }
                    }
                    tblInvoiceTO.TdsAmt = 0;
                    if (existingInvoiceTO.IsConfirmed == 1)
                    {
                        tblInvoiceTO.TdsAmt = (CalculateTDS(tblInvoiceTO) * tdsTaxPct) / 100;
                        tblInvoiceTO.TdsAmt = Math.Ceiling(tblInvoiceTO.TdsAmt);
                    }
                }

                RemoveIotFieldsFromDB(tblInvoiceTO);
               
                result = UpdateTblInvoice(tblInvoiceTO, conn, tran);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error in UpdateTblInvoice InvoiceTbl");
                    return resultMessage;
                }
                #endregion
                #region 2. Save the Address Details
                if (tblInvoiceTO.StatusId == Convert.ToInt32(Constants.InvoiceStatusE.AUTHORIZED_BY_DIRECTOR) && tblInvoiceTO.InvoiceItemDetailsTOList == null && tblInvoiceTO.InvoiceAddressTOList == null)
                {
                    tblInvoiceTO.InvoiceAddressTOList = TblInvoiceAddressDAO.SelectAllTblInvoiceAddress(tblInvoiceTO.IdInvoice, conn, tran);
                    List<TblInvoiceItemDetailsTO> itemList = BL.TblInvoiceItemDetailsBL.SelectAllTblInvoiceItemDetailsList(tblInvoiceTO.IdInvoice);
                    if (itemList != null)
                    {
                        for (int i = 0; i < itemList.Count; i++)
                        {
                            itemList[i].InvoiceItemTaxDtlsTOList = BL.TblInvoiceItemTaxDtlsBL.SelectAllTblInvoiceItemTaxDtlsList(itemList[i].IdInvoiceItem);
                        }
                        tblInvoiceTO.InvoiceItemDetailsTOList = itemList;
                    }
                }

                List<string> addrChangedPropertiesList = new List<string>();
                for (int i = 0; i < tblInvoiceTO.InvoiceAddressTOList.Count; i++)
                {
                    TblInvoiceAddressTO newAddrTO = tblInvoiceTO.InvoiceAddressTOList[i];
                    TblInvoiceAddressTO addrTO = existingInvoiceTO.InvoiceAddressTOList.Where(a => a.IdInvoiceAddr == tblInvoiceTO.InvoiceAddressTOList[i].IdInvoiceAddr).FirstOrDefault();
                    addrChangedPropertiesList = Constants.GetChangedProperties(addrTO, tblInvoiceTO.InvoiceAddressTOList[i]);
                    result = TblInvoiceAddressBL.UpdateTblInvoiceAddress(tblInvoiceTO.InvoiceAddressTOList[i], conn, tran);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error in Insert UpdateTblInvoiceAddress");
                        return resultMessage;
                    }

                    if (addrChangedPropertiesList != null && addrChangedPropertiesList.Count > 0)
                    {
                        TblInvoiceHistoryTO addrHistoryTO = new TblInvoiceHistoryTO();
                        addrHistoryTO.InvoiceId = tblInvoiceTO.IdInvoice;
                        if (addrTO.TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS)
                        {
                            addrHistoryTO.CreatedBy = tblInvoiceTO.UpdatedBy;
                            addrHistoryTO.CreatedOn = tblInvoiceTO.UpdatedOn;
                            addrHistoryTO.OldBillingAddr = addrTO.BillingName + "|" + addrTO.BillingOrgId + "|" + addrTO.GstinNo + "|" + addrTO.PanNo + "|" + addrTO.AadharNo + "|" + addrTO.ContactNo + "|" + addrTO.Address + "|" + addrTO.Taluka + "|" + addrTO.TalukaId + "|" + addrTO.District + "|" + addrTO.DistrictId + "|" + addrTO.State + "|" + addrTO.StateId + "|" + addrTO.PinCode + "|" + addrTO.CountryId;
                            addrHistoryTO.NewBillingAddr = newAddrTO.BillingName + "|" + newAddrTO.BillingOrgId + "|" + newAddrTO.GstinNo + "|" + newAddrTO.PanNo + "|" + newAddrTO.AadharNo + "|" + newAddrTO.ContactNo + "|" + newAddrTO.Address + "|" + newAddrTO.Taluka + "|" + newAddrTO.TalukaId + "|" + newAddrTO.District + "|" + newAddrTO.DistrictId + "|" + newAddrTO.State + "|" + newAddrTO.StateId + "|" + newAddrTO.PinCode + "|" + newAddrTO.CountryId;
                            addrHistoryTO.StatusDate = tblInvoiceTO.UpdatedOn;
                            addrHistoryTO.StatusId = tblInvoiceTO.StatusId;
                        }
                        else
                        {
                            addrHistoryTO.CreatedBy = tblInvoiceTO.UpdatedBy;
                            addrHistoryTO.CreatedOn = tblInvoiceTO.UpdatedOn;
                            addrHistoryTO.OldConsinAddr = addrTO.BillingName + "|" + addrTO.BillingOrgId + "|" + addrTO.GstinNo + "|" + addrTO.PanNo + "|" + addrTO.AadharNo + "|" + addrTO.ContactNo + "|" + addrTO.Address + "|" + addrTO.Taluka + "|" + addrTO.TalukaId + "|" + addrTO.District + "|" + addrTO.DistrictId + "|" + addrTO.State + "|" + addrTO.StateId + "|" + addrTO.PinCode + "|" + addrTO.CountryId;
                            addrHistoryTO.NewConsinAddr = newAddrTO.BillingName + "|" + newAddrTO.BillingOrgId + "|" + newAddrTO.GstinNo + "|" + newAddrTO.PanNo + "|" + newAddrTO.AadharNo + "|" + newAddrTO.ContactNo + "|" + newAddrTO.Address + "|" + newAddrTO.Taluka + "|" + newAddrTO.TalukaId + "|" + newAddrTO.District + "|" + newAddrTO.DistrictId + "|" + newAddrTO.State + "|" + newAddrTO.StateId + "|" + newAddrTO.PinCode + "|" + newAddrTO.CountryId;
                            addrHistoryTO.StatusDate = tblInvoiceTO.UpdatedOn;
                            addrHistoryTO.StatusId = tblInvoiceTO.StatusId;
                        }

                        isAddrChanged = true;
                        changeIn += "ADDRESS|";
                        invHistoryList.Add(addrHistoryTO);
                    }
                }


                #endregion


                #region 3. Save the Invoice Item Details

                #region Delete Previous Tax Details


                //Saket [2017-11-22] Added For Edit Option in for state.
                result = TblInvoiceItemTaxDtlsBL.DeleteInvoiceItemTaxDtlsByInvId(tblInvoiceTO.IdInvoice, conn, tran);
                if (result == -1)
                {
                    resultMessage.DefaultBehaviour("Error in DeleteTblInvoiceItemTaxDtls");
                    return resultMessage;
                }

                #endregion

                if (tblInvoiceTO.InvoiceItemDetailsTOList == null || tblInvoiceTO.InvoiceItemDetailsTOList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("Error : Invoice Item Det List Found Empty Or Null");
                    return resultMessage;
                }

                //Ramdas [2017-12-14]  Delete Record if exist.
                for (int i = 0; i < tblInvoiceTO.InvoiceItemDetailsTOList.Count; i++)
                {
                    TblInvoiceItemDetailsTO tblInvoiceItemDetailsTOResult = tblInvoiceTO.InvoiceItemDetailsTOList[i];
                    if (tblInvoiceItemDetailsTOResult != null && tblInvoiceItemDetailsTOResult.OtherTaxId != 0)
                    {

                        List<TblInvoiceItemDetailsTO> temp = tblInvoiceTO.InvoiceItemDetailsTOList.Where(w => w.OtherTaxId == tblInvoiceItemDetailsTOResult.OtherTaxId).ToList();
                        if (temp != null && temp.Count > 1)
                        {
                            resultMessage.DefaultBehaviour("Other Tax type is double - " + tblInvoiceItemDetailsTOResult.ProdItemDesc);
                            return resultMessage;
                        }
                    }
                }
                for (int i = 0; i < tblInvoiceTO.InvoiceItemDetailsTOList.Count; i++)
                {

                    var exInvItemTO = existingInvoiceTO.InvoiceItemDetailsTOList.Where(ei => ei.IdInvoiceItem == tblInvoiceTO.InvoiceItemDetailsTOList[i].IdInvoiceItem).FirstOrDefault();
                    if (exInvItemTO == null)
                    {
                        TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = new TblInvoiceItemDetailsTO();
                        tblInvoiceItemDetailsTO = tblInvoiceTO.InvoiceItemDetailsTOList[i];
                        tblInvoiceItemDetailsTO.InvoiceId = tblInvoiceTO.IdInvoice;


                        result = TblInvoiceItemDetailsBL.InsertTblInvoiceItemDetails(tblInvoiceItemDetailsTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error in Insert InvoiceItemDetailTbl");
                            return resultMessage;
                        }
                        #region 1. Save the Invoice Tax Item Details
                        if (tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList == null
                            || tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList.Count == 0)
                        {
                            if (tblInvoiceTO.InvoiceTypeE == Constants.InvoiceTypeE.REGULAR_TAX_INVOICE
                                || tblInvoiceTO.InvoiceTypeE == Constants.InvoiceTypeE.SEZ_WITH_DUTY)
                            {
                                resultMessage.DefaultBehaviour("Error : Invoice Item Det Tax List Found Empty Or Null");
                                return resultMessage;
                            }
                        }

                        for (int j = 0; j < tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList.Count; j++)
                        {
                            tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList[j].InvoiceItemId = tblInvoiceTO.InvoiceItemDetailsTOList[i].IdInvoiceItem;
                            result = TblInvoiceItemTaxDtlsBL.InsertTblInvoiceItemTaxDtls(tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList[j], conn, tran);
                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error in Insert InvoiceItemTaxDetailTbl");
                                return resultMessage;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = tblInvoiceTO.InvoiceItemDetailsTOList[i];
                        tblInvoiceItemDetailsTO.InvoiceId = tblInvoiceTO.IdInvoice;

                        result = TblInvoiceItemDetailsBL.UpdateTblInvoiceItemDetails(tblInvoiceItemDetailsTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error in update UpdateTblInvoiceItemDetails");
                            return resultMessage;
                        }
                        #region 1. Save the Invoice Tax Item Details                       

                        for (int j = 0; j < tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList.Count; j++)
                        {
                            tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList[j].InvoiceItemId = tblInvoiceTO.InvoiceItemDetailsTOList[i].IdInvoiceItem;

                            //Saket [2017-11-22] Added For Edit Option in for state.
                            //result = TblInvoiceItemTaxDtlsBL.UpdateTblInvoiceItemTaxDtls(tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList[j], conn, tran);
                            result = TblInvoiceItemTaxDtlsBL.InsertTblInvoiceItemTaxDtls(tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList[j], conn, tran);
                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error in Insert UpdateTblInvoiceItemTaxDtls");
                                return resultMessage;
                            }
                        }

                        if ((exInvItemTO.Rate != tblInvoiceItemDetailsTO.Rate)
                            || (exInvItemTO.CdStructure != tblInvoiceItemDetailsTO.CdStructure))
                        {
                            TblInvoiceHistoryTO historyTO = new TblInvoiceHistoryTO();
                            historyTO.InvoiceId = tblInvoiceTO.IdInvoice;
                            historyTO.InvoiceItemId = tblInvoiceItemDetailsTO.IdInvoiceItem;
                            if (exInvItemTO.Rate != tblInvoiceItemDetailsTO.Rate)
                            {
                                historyTO.CreatedBy = tblInvoiceTO.UpdatedBy;
                                historyTO.CreatedOn = tblInvoiceTO.UpdatedOn;
                                historyTO.OldUnitRate = exInvItemTO.Rate;
                                historyTO.NewUnitRate = tblInvoiceItemDetailsTO.Rate;
                                historyTO.StatusDate = tblInvoiceTO.UpdatedOn;
                                historyTO.StatusId = tblInvoiceTO.StatusId;
                                isRateChange = true;
                                changeIn += "RATE|";

                            }

                            if (exInvItemTO.CdStructure != tblInvoiceItemDetailsTO.CdStructure)
                            {
                                historyTO.CreatedBy = tblInvoiceTO.UpdatedBy;
                                historyTO.CreatedOn = tblInvoiceTO.UpdatedOn;
                                historyTO.OldCdStructureId = exInvItemTO.CdStructureId;
                                historyTO.NewCdStructureId = tblInvoiceItemDetailsTO.CdStructureId;
                                historyTO.StatusDate = tblInvoiceTO.UpdatedOn;
                                historyTO.StatusId = tblInvoiceTO.StatusId;
                                isCdChanged = true;
                                changeIn += "CD|";
                            }

                            invHistoryList.Add(historyTO);
                        }

                    }

                    #endregion
                }



                for (int i = 0; i < existingInvoiceTO.InvoiceItemDetailsTOList.Count; i++)
                {
                    var exInvItemTO = tblInvoiceTO.InvoiceItemDetailsTOList.Where(ei => ei.IdInvoiceItem == existingInvoiceTO.InvoiceItemDetailsTOList[i].IdInvoiceItem).FirstOrDefault();
                    if (exInvItemTO == null)
                    {
                        result = TblInvoiceItemDetailsBL.DeleteTblInvoiceItemDetails(existingInvoiceTO.InvoiceItemDetailsTOList[i].IdInvoiceItem, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error in Delete DeleteTblInvoiceItemDetails");
                            return resultMessage;
                        }
                    }

                }
                #endregion

                if (invHistoryList != null && invHistoryList.Count > 0)
                {
                    for (int i = 0; i < invHistoryList.Count; i++)
                    {
                        result = BL.TblInvoiceHistoryBL.InsertTblInvoiceHistory(invHistoryList[i], conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error while InsertTblInvoiceHistory");
                            return resultMessage;
                        }
                    }
                }

                #region Notifications & SMSs

                if (tblInvoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.NEW
                    || tblInvoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.PENDING_FOR_AUTHORIZATION
                    || tblInvoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.AUTHORIZED_BY_DIRECTOR
                    || tblInvoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.REJECTED_BY_DIRECTOR
                    || tblInvoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.ACCEPTED_BY_DISTRIBUTOR
                    || tblInvoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.REJECTED_BY_DISTRIBUTOR)

                {

                    TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                    List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();
                    TblUserTO userTO = TblUserBL.SelectTblUserTO(tblInvoiceTO.CreatedBy, conn, tran);
                    if (userTO != null)
                    {
                        TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                        tblAlertUsersTO.UserId = userTO.IdUser;
                        tblAlertUsersTO.DeviceId = userTO.RegisteredDeviceId;
                        tblAlertUsersTOList.Add(tblAlertUsersTO);
                    }

                    if (tblInvoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.NEW)
                    {
                        tblInvoiceTO.ChangeIn = changeIn;

                        //[24-01-2018] Vijaymala:Added cd change condition to update invoice to invoice approval

                        if (isRateChange || isCdChanged)
                        {
                            tblInvoiceTO.InvoiceStatusE = Constants.InvoiceStatusE.PENDING_FOR_AUTHORIZATION;
                            tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.INVOICE_APPROVAL_REQUIRED;
                            tblAlertInstanceTO.AlertAction = "INVOICE_APPROVAL_REQUIRED";
                            tblAlertInstanceTO.AlertComment = "Approval Required For Invoice #" + tblInvoiceTO.IdInvoice;
                            resultMessage = InvoiceStatusUpdate(tblInvoiceTO, tblInvoiceTO.StatusId, conn, tran);
                            if (resultMessage.MessageType != ResultMessageE.Information)
                            {
                                return resultMessage;
                            }
                        }
                        //else if (!isRateChange && (isCdChanged || isAddrChanged))
                        else
                        {
                            List<TblUserTO> cnfUserList = BL.TblUserBL.SelectAllTblUserList(tblInvoiceTO.DistributorOrgId, conn, tran);
                            if (cnfUserList != null && cnfUserList.Count > 0)
                            {
                                for (int a = 0; a < cnfUserList.Count; a++)
                                {
                                    TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                                    tblAlertUsersTO.UserId = cnfUserList[a].IdUser;
                                    tblAlertUsersTO.DeviceId = cnfUserList[a].RegisteredDeviceId;
                                    tblAlertUsersTOList.Add(tblAlertUsersTO);
                                }
                            }

                            tblInvoiceTO.InvoiceStatusE = Constants.InvoiceStatusE.PENDING_FOR_ACCEPTANCE;
                            tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.INVOICE_ACCEPTANCE_REQUIRED;
                            tblAlertInstanceTO.AlertAction = "INVOICE_ACCEPTANCE_REQUIRED";
                            tblAlertInstanceTO.AlertComment = "Invoice #" + tblInvoiceTO.IdInvoice + " is awaiting for acceptance";

                            resultMessage = InvoiceStatusUpdate(tblInvoiceTO, tblInvoiceTO.StatusId, conn, tran);
                            if (resultMessage.MessageType != ResultMessageE.Information)
                            {
                                return resultMessage;
                            }

                        }
                        //else
                        //{
                        //    goto exitNotification;
                        //}
                    }
                    else if (tblInvoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.AUTHORIZED_BY_DIRECTOR)
                    {
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.INVOICE_APPROVED_BY_DIRECTOR;
                        tblAlertInstanceTO.AlertAction = "INVOICE_APPROVED_BY_DIRECTOR";
                        tblAlertInstanceTO.AlertComment = "Invoice #" + tblInvoiceTO.IdInvoice + " Is Approved.";

                        resultMessage = CheckAndUpdateForInvoiceAcceptanceStatus(existingInvoiceTO, tblInvoiceTO, tblAlertInstanceTO, tblAlertUsersTOList, conn, tran);
                        if (resultMessage.MessageType != ResultMessageE.Information)
                            return resultMessage;
                    }
                    else if (tblInvoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.REJECTED_BY_DIRECTOR)
                    {
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.INVOICE_REJECTED_BY_DIRECTOR;
                        tblAlertInstanceTO.AlertAction = "INVOICE_REJECTED_BY_DIRECTOR";
                        tblAlertInstanceTO.AlertComment = "Invoice #" + tblInvoiceTO.IdInvoice + " Is Rejected by Director";

                        resultMessage = InvoiceStatusUpdate(tblInvoiceTO, tblInvoiceTO.StatusId, conn, tran);
                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            return resultMessage;
                        }
                        //resultMessage = CheckAndUpdateForInvoiceAcceptanceStatus(existingInvoiceTO, tblInvoiceTO, tblAlertInstanceTO, tblAlertUsersTOList, conn, tran);
                        //if (resultMessage.MessageType != ResultMessageE.Information)
                        //{
                        //    return resultMessage;
                        //}

                    }
                    else if (tblInvoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.REJECTED_BY_DISTRIBUTOR)
                    {
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.INVOICE_REJECTED_BY_DISTRIBUTOR;
                        tblAlertInstanceTO.AlertAction = "INVOICE_REJECTED_BY_DISTRIBUTOR";
                        tblAlertInstanceTO.AlertComment = "Invoice #" + tblInvoiceTO.IdInvoice + " Is Rejected by Distributer";

                        resultMessage = InvoiceStatusUpdate(tblInvoiceTO, tblInvoiceTO.StatusId, conn, tran);
                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            return resultMessage;
                        }
                        //resultMessage = CheckAndUpdateForInvoiceAcceptanceStatus(existingInvoiceTO, tblInvoiceTO, tblAlertInstanceTO, tblAlertUsersTOList, conn, tran);
                        //if (resultMessage.MessageType != ResultMessageE.Information)
                        //{
                        //    return resultMessage;
                        //}

                    }
                    else if (tblInvoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.ACCEPTED_BY_DISTRIBUTOR)
                    {
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.INVOICE_ACCEPTED_BY_DISTRIBUTOR;
                        tblAlertInstanceTO.AlertAction = "INVOICE_ACCEPTED_BY_DISTRIBUTOR";
                        tblAlertInstanceTO.AlertComment = "Invoice #" + tblInvoiceTO.IdInvoice + " Is accecpted By Distributor.";

                        resultMessage = InvoiceStatusUpdate(tblInvoiceTO, tblInvoiceTO.StatusId, conn, tran);
                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            return resultMessage;
                        }
                    }
                    //else if (tblInvoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.REJECTED_BY_DISTRIBUTOR)
                    //{
                    //    tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.INVOICE_REJECTED_BY_DISTRIBUTOR;
                    //    tblAlertInstanceTO.AlertAction = "INVOICE_REJECTED_BY_DISTRIBUTOR";
                    //    tblAlertInstanceTO.AlertComment = "Invoice #" + tblInvoiceTO.IdInvoice + " Is Rejected by Distributor.";
                    //}


                    tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;

                    tblAlertInstanceTO.EffectiveFromDate = tblInvoiceTO.UpdatedOn;
                    tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                    tblAlertInstanceTO.IsActive = 1;
                    tblAlertInstanceTO.SourceEntityId = tblInvoiceTO.IdInvoice;
                    tblAlertInstanceTO.RaisedBy = tblInvoiceTO.UpdatedBy;
                    tblAlertInstanceTO.RaisedOn = tblInvoiceTO.UpdatedOn;
                    tblAlertInstanceTO.IsAutoReset = 1;

                    ResultMessage rMessage = BL.TblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                    if (rMessage.MessageType != ResultMessageE.Information)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour();
                        resultMessage.Text = "Error While Generating Notification";
                        return resultMessage;
                    }
                }

                exitNotification:

                #endregion

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateInvoice");
                return resultMessage;
            }
            finally
            {

            }
        }

        /// <summary>
        /// GJ@20170926 : Update the Invoice To calculation
        /// </summary>
        /// <param name="tblInvoiceTO"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static TblInvoiceTO updateInvoiceToCalc(TblInvoiceTO tblInvoiceTo, SqlConnection conn, SqlTransaction tran, Boolean isCheckHist = true)
        {
            tblInvoiceTo.BasicAmt = 0;
            tblInvoiceTo.DiscountAmt = 0;
            tblInvoiceTo.TaxableAmt = 0;
            tblInvoiceTo.IgstAmt = 0;
            tblInvoiceTo.SgstAmt = 0;
            tblInvoiceTo.CgstAmt = 0;
            tblInvoiceTo.GrandTotal = 0;
            for (int i = 0; i < tblInvoiceTo.InvoiceItemDetailsTOList.Count; i++)
            {
                TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = tblInvoiceTo.InvoiceItemDetailsTOList[i];
                TblInvoiceHistoryTO historyInvoiceTo = new TblInvoiceHistoryTO();
                if (isCheckHist)
                {
                    //[24/01/2018]Vijaymala Added :To get previous cd structure and rate
                    historyInvoiceTo = TblInvoiceHistoryBL.SelectTblInvoiceHistoryTORateByInvoiceItemId(tblInvoiceItemDetailsTO.IdInvoiceItem, conn, tran);
                    if (historyInvoiceTo != null)
                    {
                        tblInvoiceItemDetailsTO.Rate = historyInvoiceTo.OldUnitRate;
                        //tblInvoiceItemDetailsTO.CdStructureId = historyInvoiceTo.OldCdStructureId;
                        //resultMessage.DefaultBehaviour("Invoice History Rate Object Not Found");
                        //return resultMessage;
                    }
                    historyInvoiceTo = TblInvoiceHistoryBL.SelectTblInvoiceHistoryTOCdByInvoiceItemId(tblInvoiceItemDetailsTO.IdInvoiceItem, conn, tran);
                    if (historyInvoiceTo != null)
                    {
                        // tblInvoiceItemDetailsTO.Rate = historyInvoiceTo.OldUnitRate;
                        tblInvoiceItemDetailsTO.CdStructureId = historyInvoiceTo.OldCdStructureId;
                        if (tblInvoiceItemDetailsTO.CdStructureId > 0)
                        {
                            List<DropDownTO> dropDownTOList = BL.DimensionBL.SelectCDStructureForDropDown();
                            if (dropDownTOList != null && dropDownTOList.Count > 0)
                            {
                                DropDownTO dropDownTO = dropDownTOList.Where(w => w.Value == tblInvoiceItemDetailsTO.CdStructureId).FirstOrDefault();
                                if (dropDownTO != null)
                                {
                                    tblInvoiceItemDetailsTO.CdStructure = Convert.ToDouble(dropDownTO.Text);
                                }
                            }
                        }
                        //resultMessage.DefaultBehaviour("Invoice History Rate Object Not Found");
                        //return resultMessage;
                    }

                }
                else
                {
                    historyInvoiceTo = null;
                }
                //if (historyInvoiceTo != null)
                //{
                //    tblInvoiceItemDetailsTO.Rate = historyInvoiceTo.OldUnitRate;
                //    tblInvoiceItemDetailsTO.CdStructureId = historyInvoiceTo.OldCdStructureId;
                //    //resultMessage.DefaultBehaviour("Invoice History Rate Object Not Found");
                //    //return resultMessage;
                //}
                tblInvoiceItemDetailsTO.BasicTotal = tblInvoiceItemDetailsTO.Rate * tblInvoiceItemDetailsTO.InvoiceQty;
                tblInvoiceTo.BasicAmt += tblInvoiceItemDetailsTO.BasicTotal;
                if (tblInvoiceItemDetailsTO.CdStructure > 0)
                {
                    tblInvoiceItemDetailsTO.CdAmt = Math.Round(tblInvoiceItemDetailsTO.BasicTotal * tblInvoiceItemDetailsTO.CdStructure) / 100;
                }
                else
                {
                    tblInvoiceItemDetailsTO.CdAmt = 0;
                }
                tblInvoiceTo.DiscountAmt += tblInvoiceItemDetailsTO.CdAmt;
                tblInvoiceItemDetailsTO.TaxableAmt = tblInvoiceItemDetailsTO.BasicTotal - tblInvoiceItemDetailsTO.CdAmt;
                tblInvoiceTo.TaxableAmt += tblInvoiceItemDetailsTO.TaxableAmt;

                tblInvoiceItemDetailsTO.GrandTotal = tblInvoiceItemDetailsTO.TaxableAmt;

                foreach (var itemTaxDet in tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList)
                {
                    itemTaxDet.TaxableAmt = tblInvoiceItemDetailsTO.TaxableAmt;
                    itemTaxDet.TaxAmt = (itemTaxDet.TaxableAmt * itemTaxDet.TaxRatePct) / 100;
                    tblInvoiceItemDetailsTO.GrandTotal += itemTaxDet.TaxAmt;
                }
                tblInvoiceTo.GrandTotal += tblInvoiceItemDetailsTO.GrandTotal;
                if (tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList.Count == 1)
                {
                    tblInvoiceTo.IgstAmt += tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList[0].TaxAmt;
                }
                else
                {
                    tblInvoiceTo.CgstAmt += tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList[0].TaxAmt;
                    tblInvoiceTo.SgstAmt += tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList.Count > 1 ?
                        tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList[1].TaxAmt : 0;
                }
            }

            //Harshala
            Boolean isAddTcsDtls = false;
            TblConfigParamsTO tcsConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_TCS_OTHER_TAX_ID, conn, tran);
            if (tcsConfigParamsTO != null)
            {
                Int32 tcsId = Convert.ToInt32(tcsConfigParamsTO.ConfigParamVal);
                var tcsItemList = tblInvoiceTo.InvoiceItemDetailsTOList.Where(e => e.OtherTaxId == tcsId).ToList();
                if (tcsItemList != null && tcsItemList.Count > 0)
                    isAddTcsDtls = false;
                else
                    isAddTcsDtls = true;
            }
            if (isAddTcsDtls)
            {
                Double grandTotal = tblInvoiceTo.GrandTotal;
                Double taxableAmt = tblInvoiceTo.TaxableAmt;
                Double basicTotalAmt = tblInvoiceTo.BasicAmt;
                Boolean isPanPresent = false;
                ResultMessage message = new ResultMessage();
                if (tblInvoiceTo.IsConfirmed == 1)
                {
                    Int32 BillingOrgId = tblInvoiceTo.DealerOrgId;
                    if (tblInvoiceTo.InvoiceAddressTOList != null && tblInvoiceTo.InvoiceAddressTOList.Count > 0)
                    {
                        tblInvoiceTo.InvoiceAddressTOList.ForEach(element =>
                        {
                            if (element.TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS)
                            {
                                isPanPresent = IsPanOrGstPresent(element.PanNo, element.GstinNo);
                                if (element.BillingOrgId > 0)
                                {
                                    BillingOrgId = element.BillingOrgId;
                                }
                            }
                        });
                    }

                    TblOrganizationTO tblOrganizationTO = TblOrganizationDAO.SelectTblOrganization(BillingOrgId, conn, tran);
                    if (tblOrganizationTO != null && tblOrganizationTO.IsTcsApplicable == 1)
                    {
                        tblInvoiceTo.IsTcsApplicable = tblOrganizationTO.IsTcsApplicable;
                        message = AddTcsTOInTaxItemDtls(conn, tran, ref grandTotal, ref taxableAmt, ref basicTotalAmt, isPanPresent, tblInvoiceTo.InvoiceItemDetailsTOList);
                    }
                    tblInvoiceTo.GrandTotal = grandTotal;
                    tblInvoiceTo.TaxableAmt = taxableAmt;
                    tblInvoiceTo.BasicAmt = basicTotalAmt;
                }
            }

            return tblInvoiceTo;

        }

        private static ResultMessage InvoiceStatusUpdate(TblInvoiceTO tblInvoiceTO, Int32 statusId, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                //Update Status
                tblInvoiceTO.StatusId = statusId;
                int result = UpdateTblInvoice(tblInvoiceTO, conn, tran);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error in UpdateTblInvoice InvoiceTbl");
                    return resultMessage;
                }

                //Generate inv history record
                TblInvoiceHistoryTO invHistoryTO = new TblInvoiceHistoryTO();
                invHistoryTO.InvoiceId = tblInvoiceTO.IdInvoice;
                invHistoryTO.CreatedOn = tblInvoiceTO.UpdatedOn;
                invHistoryTO.CreatedBy = tblInvoiceTO.UpdatedBy;
                invHistoryTO.StatusDate = tblInvoiceTO.UpdatedOn;
                invHistoryTO.StatusId = statusId;
                if (statusId == (int)Constants.InvoiceStatusE.PENDING_FOR_AUTHORIZATION)
                    invHistoryTO.StatusRemark = "Invoice #" + tblInvoiceTO.IdInvoice + " is pending for authorization";
                else if (statusId == (int)Constants.InvoiceStatusE.PENDING_FOR_ACCEPTANCE)
                    invHistoryTO.StatusRemark = "Invoice #" + tblInvoiceTO.IdInvoice + " is pending for Acceptance";
                if (statusId == (int)Constants.InvoiceStatusE.REJECTED_BY_DIRECTOR)
                    invHistoryTO.StatusRemark = "Invoice #" + tblInvoiceTO.IdInvoice + " is Rejected By Director";
                if (statusId == (int)Constants.InvoiceStatusE.REJECTED_BY_DISTRIBUTOR)
                    invHistoryTO.StatusRemark = "Invoice #" + tblInvoiceTO.IdInvoice + " is Rejected By Distributer";
                result = BL.TblInvoiceHistoryBL.InsertTblInvoiceHistory(invHistoryTO, conn, tran);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While InsertTblInvoiceHistory"); return resultMessage;
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InvoiceStatusUpdate");
                return resultMessage;
            }
        }

        private static ResultMessage CheckAndUpdateForInvoiceAcceptanceStatus(TblInvoiceTO existingInvoiceTO, TblInvoiceTO tblInvoiceTO, TblAlertInstanceTO tblAlertInstanceTO, List<TblAlertUsersTO> tblAlertUsersTOList, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                //Boolean isSentForAcceptance = false;
                //if (!string.IsNullOrEmpty(existingInvoiceTO.ChangeIn))
                //{
                //    string[] changedAttrs = existingInvoiceTO.ChangeIn.Split('|');
                //    if (changedAttrs.Length > 0)
                //    {
                //        for (int k = 0; k < changedAttrs.Length; k++)
                //        {
                //            if (changedAttrs[k] == "ADDRESS" || changedAttrs[k] == "CD")
                //            {
                //                isSentForAcceptance = true;
                //                break;
                //            }
                //        }
                //    }
                //}

                //if (isSentForAcceptance)
                //{
                List<TblUserTO> cnfUserList = BL.TblUserBL.SelectAllTblUserList(tblInvoiceTO.DistributorOrgId, conn, tran);
                if (cnfUserList != null && cnfUserList.Count > 0)
                {
                    for (int a = 0; a < cnfUserList.Count; a++)
                    {
                        TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                        tblAlertUsersTO.UserId = cnfUserList[a].IdUser;
                        tblAlertUsersTO.DeviceId = cnfUserList[a].RegisteredDeviceId;
                        tblAlertUsersTOList.Add(tblAlertUsersTO);
                    }
                }

                tblInvoiceTO.InvoiceStatusE = Constants.InvoiceStatusE.PENDING_FOR_ACCEPTANCE;
                tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.INVOICE_ACCEPTANCE_REQUIRED;
                tblAlertInstanceTO.AlertAction = "INVOICE_ACCEPTANCE_REQUIRED";
                tblAlertInstanceTO.AlertComment = "Invoice #" + tblInvoiceTO.IdInvoice + " is awaiting for acceptance";

                resultMessage = InvoiceStatusUpdate(tblInvoiceTO, tblInvoiceTO.StatusId, conn, tran);
                if (resultMessage.MessageType != ResultMessageE.Information)
                {
                    return resultMessage;
                }
                //}

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InvoiceAccetanceStatus");
                return resultMessage;
            }
        }


        public static ResultMessage GenerateInvoiceNumber(Int32 invoiceId, Int32 loginUserId, Int32 isconfirm, Int32 invGenModeId)
        {

            Boolean removeDataFromIot = false;

            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            DateTime serverDate = Constants.ServerDateTime;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                Int32 configId = Constants.getweightSourceConfigTO();
                //TblInvoiceTO invoiceTO = TblInvoiceDAO.SelectTblInvoice(invoiceId, conn, tran);
                TblInvoiceTO invoiceTO = SelectTblInvoiceTOWithDetails(invoiceId, conn, tran);
                if (invoiceTO == null)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("invoiceTO Found NULL OR VehicleNo Found NULL OR TransportOrgId Found NULL"); return resultMessage;
                }

                if (string.IsNullOrEmpty(invoiceTO.InvoiceNo) || invoiceTO.InvoiceNo == "0")
                {
                    if (invGenModeId != (int)Constants.InvoiceGenerateModeE.REGULAR)
                    {
                        TblInvoiceChangeOrgHistoryTO changeHisTO = new TblInvoiceChangeOrgHistoryTO();
                        resultMessage = PrepareAndSaveInternalTaxInvoices(invoiceTO,invGenModeId,0,0,0, changeHisTO, conn, tran);
                        if (resultMessage.MessageType == ResultMessageE.Information)
                        {
                            if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(List<TblInvoiceTO>))
                            {
                                List<TblInvoiceTO> tblInvoiceTOList = (List<TblInvoiceTO>)resultMessage.Tag;
                                if (tblInvoiceTOList != null)
                                {
                                    //Update Existing Invoice
                                    invoiceTO = tblInvoiceTOList[0];
                                }
                            }
                            //Hrushikesh changed Here
                            if (invGenModeId != (int)Constants.InvoiceGenerateModeE.CHANGEFROM)
                            {
                                tran.Commit();
                                resultMessage.Text = "Invoice Converted Successfully";
                                resultMessage.DisplayMessage = "Invoice Converted Successfully";
                                return resultMessage;
                            }
                        }
                        else
                        {
                            tran.Rollback();
                            return resultMessage;
                        }
                    }

                    // Ramdas.W @ 28102017 : chenge InvoiceStatus for new Invoice number genarate  AUTHORIZED And status not conform  
                    if (invoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.AUTHORIZED && isconfirm == 0)
                    {
                        invoiceTO.InvoiceStatusE = Constants.InvoiceStatusE.NEW;
                    }

                    if (invoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.PENDING_FOR_AUTHORIZATION
                        || invoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.CANCELLED
                        || invoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.AUTHORIZED
                        )
                    {


                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Can Not Continue.INvoice Status Is -" + invoiceTO.StatusName);
                        resultMessage.DisplayMessage = "Not Allowed.Invoice is :" + invoiceTO.StatusName;
                        return resultMessage;

                    }
                    invoiceTO.StatusDate = serverDate;
                    invoiceTO.UpdatedBy = loginUserId;
                    invoiceTO.UpdatedOn = serverDate;
                    invoiceTO.InvoiceStatusE = Constants.InvoiceStatusE.AUTHORIZED;
                    invoiceTO.InvoiceDate = serverDate;
                    if (invoiceTO.IsConfirmed == 1)
                    {
                        //Need TO change Here
                        TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID, conn, tran);
                        if (tblConfigParamsTO == null)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Defualt Organization Not Found in Configuration.");
                            return resultMessage;
                        }
                        Int32 defaultOrgId = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
                        TblEntityRangeTO entityRangeTO = null;
                        if (invoiceTO.InvFromOrgId == defaultOrgId)
                            entityRangeTO = BL.TblEntityRangeBL.SelectEntityRangeTOFromInvoiceType(invoiceTO.InvoiceTypeId, invoiceTO.FinYearId, conn, tran);
                        else
                        {
                            string EntityName =  Constants.ENTITY_RANGE_REGULAR_TAX_INTERNALORG + invoiceTO.InvFromOrgId;
                            entityRangeTO = BL.TblEntityRangeBL.SelectEntityRangeTOFromInvoiceType(EntityName, invoiceTO.FinYearId, conn, tran);
                        }

                        if (entityRangeTO == null)

                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("entityRangeTO Found NULL. Entity Range Not Defined"); return resultMessage;
                        }
                        //end
                        Int32 entityPrevVal = entityRangeTO.EntityPrevValue;
                        entityPrevVal++;
                        invoiceTO.InvoiceNo = entityRangeTO.Prefix + entityPrevVal.ToString();

                        result = UpdateTblInvoice(invoiceTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While Updating Invoice Number After Entity Range"); return resultMessage;
                        }

                        entityRangeTO.EntityPrevValue = entityPrevVal;
                        result = BL.TblEntityRangeBL.UpdateTblEntityRange(entityRangeTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While UpdateTblEntityRange"); return resultMessage;
                        }
                    }
                    else
                    {
                        result = UpdateTblInvoice(invoiceTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While Updating Invoice Number After Entity Range"); return resultMessage;
                        }
                    }

                    //Generate inv history record
                    TblInvoiceHistoryTO invHistoryTO = new TblInvoiceHistoryTO();
                    invHistoryTO.InvoiceId = invoiceTO.IdInvoice;
                    invHistoryTO.CreatedOn = serverDate;
                    invHistoryTO.CreatedBy = loginUserId;
                    invHistoryTO.StatusDate = serverDate;
                    invHistoryTO.StatusId = (int)Constants.InvoiceStatusE.AUTHORIZED;
                    invHistoryTO.StatusRemark = "Invoice Authorized With Inv No :" + invoiceTO.InvoiceNo;
                    result = BL.TblInvoiceHistoryBL.InsertTblInvoiceHistory(invHistoryTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Error While InsertTblInvoiceHistory"); return resultMessage;
                    }
                }
                else
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Invoice No is already Generated");
                    resultMessage.DisplayMessage = "Invoice No #" + invoiceTO.InvoiceNo + " is already Generated";
                    return resultMessage;
                }
                //Ramdas.w@19-12-2017: Update Individual Loading & Loading Slip statuses

                if (invoiceTO.InvoiceModeId != Convert.ToInt32(Constants.InvoiceModeE.MANUAL_INVOICE))
                {
                    TblLoadingTO tblLoadingTO = new TblLoadingTO();
                    TblLoadingSlipTO tblLoadingSlipTO = new TblLoadingSlipTO();
                    //if (invoiceTO.InvoiceNo != null)
                    if (invoiceTO.InvoiceStatusE == InvoiceStatusE.AUTHORIZED)
                    {
                        tblLoadingSlipTO.StatusId = Convert.ToInt16(Constants.TranStatusE.INVOICE_GENERATED_AND_READY_FOR_DISPACH);
                        tblLoadingSlipTO.StatusReason = "Invoice Generated and ready for dispach";
                        tblLoadingSlipTO.StatusDate = Constants.ServerDateTime;
                        tblLoadingSlipTO.IdLoadingSlip = invoiceTO.LoadingSlipId;
                        result = TblLoadingSlipDAO.UpdateTblLoadingsSlipById(tblLoadingSlipTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While UpdateTblLoadingSlip In Method UpdateStatus";
                            return resultMessage;
                        }

                        TblLoadingSlipTO tblLoadingSlipTOselect = TblLoadingSlipDAO.SelectTblLoadingSlip(tblLoadingSlipTO.IdLoadingSlip, conn, tran);
                        if (tblLoadingSlipTO == null)
                        {
                            tran.Rollback();
                            resultMessage.Text = "";
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Result = 0;
                            return resultMessage;
                        }
                        Int32 count = 0;
                        List<TblLoadingSlipTO> list = TblLoadingSlipDAO.SelectAllTblLoadingSlip(tblLoadingSlipTOselect.LoadingId, conn, tran);
                        if (list == null)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("LoadingSlip Found NULL"); return resultMessage;
                        }
                        else
                        {
                            for (int i = 0; i < list.Count; i++)
                            {
                                List<TblInvoiceTO> invoiceTOselectList = TblInvoiceBL.SelectInvoiceTOFromLoadingSlipId(list[i].IdLoadingSlip, conn, tran);

                                if (invoiceTOselectList != null)
                                {
                                    List<TblInvoiceTO> TblInvoiceTOTemp = invoiceTOselectList.Where(w => w.InvoiceStatusE == InvoiceStatusE.AUTHORIZED).ToList();

                                    //if (TblInvoiceTOTemp == null || TblInvoiceTOTemp.Count == 0)
                                    if (invoiceTOselectList != null && invoiceTOselectList.Count == TblInvoiceTOTemp.Count)
                                    {
                                        count++;
                                    }
                                    else
                                    {
                                        break;
                                    }

                                    //if (invoiceTO == null)
                                    //{
                                    //    tran.Rollback();
                                    //    resultMessage.DefaultBehaviour("LoadingSlip Found NULL"); return resultMessage;
                                    //}
                                }
                            }

                            //tblLoadingTO = BL.TblLoadingBL.SelectLoadingTOWithDetails(tblLoadingSlipTOselect.LoadingId, conn, tran, true);
                            //TblConfigParamsTO configParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_WEIGHING_SCALE, conn, tran);

                            if (list.Count == count)
                            {

                                tblLoadingTO = BL.TblLoadingBL.SelectLoadingTOWithDetails(tblLoadingSlipTOselect.LoadingId, conn, tran, true);

                                // tblLoadingTO = TblLoadingBL.SelectTblLoadingTO(tblLoadingSlipTOselect.LoadingId, conn, tran);
                                if (tblLoadingTO == null || tblLoadingTO.VehicleNo == null || tblLoadingTO.TransporterOrgId == 0)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour("tblLoadingTO Found NULL"); return resultMessage;
                                }

                                String wtScaleConfigStr = Constants.CP_DEFAULT_WEIGHING_SCALE;
                                if (tblLoadingTO.IsInternalCnf == 1)
                                    wtScaleConfigStr = Constants.CP_DEFAULT_WEIGHING_SCALE_INTERNAL_TRANSFER;
                                TblConfigParamsTO configParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(wtScaleConfigStr, conn, tran);

                                tblLoadingTO.StatusId = Convert.ToInt16(Constants.TranStatusE.INVOICE_GENERATED_AND_READY_FOR_DISPACH);
                                tblLoadingTO.StatusReason = "Invoice Generated and ready for dispach";
                                tblLoadingTO.StatusDate = Constants.ServerDateTime;
                                tblLoadingTO.IdLoading = tblLoadingSlipTOselect.LoadingId;
                                invoiceTO.VehicleNo = tblLoadingTO.VehicleNo;

                                if (configId == (Int32)Constants.WeighingDataSourceE.IoT || configId == (Int32)Constants.WeighingDataSourceE.BOTH)
                                {
                                    if (configParamsTO != null)
                                    {
                                        if (Convert.ToInt32(configParamsTO.ConfigParamVal) == 1)
                                        {
                                            // List<TblWeighingMeasuresTO> weighingMeasuresToList = new List<TblWeighingMeasuresTO>();
                                            List<TblWeighingMachineTO> weighingMeasuresToList = BL.TblWeighingMachineBL.SelectAllTblWeighingMachineOfWeighingList(tblLoadingTO.IdLoading);
                                            if (weighingMeasuresToList.Count > 0)
                                            {
                                                var vRes = weighingMeasuresToList.Where(p => p.WeightMeasurTypeId == (int)Constants.TransMeasureTypeE.GROSS_WEIGHT).ToList();
                                                if (vRes != null && vRes.Count == 1)
                                                {
                                                    resultMessage = TblLoadingBL.UpdateLoadingStatusToGateIoT(tblLoadingTO, conn, tran);
                                                    if (resultMessage.MessageType != ResultMessageE.Information)
                                                    {
                                                        return resultMessage;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    result = TblLoadingSlipDAO.UpdateTblLoadingById(tblLoadingTO, conn, tran);
                                    if (result <= 0)
                                    {
                                        tran.Rollback();
                                        resultMessage.MessageType = ResultMessageE.Error;
                                        resultMessage.Text = "Error While UpdateTblLoading In Method UpdateStatusForLoading";
                                        return resultMessage;
                                    }
                                }

                                if (configParamsTO != null)
                                {
                                    if (Convert.ToInt32(configParamsTO.ConfigParamVal) > 1 && configId == (Int32)Constants.WeighingDataSourceE.IoT)
                                    {
                                        //tblLoadingTO.StatusId = Convert.ToInt16(Constants.TranStatusE.LOADING_IN_PROGRESS);
                                        tblLoadingTO.StatusId = Convert.ToInt16(Constants.TranStatusE.LOADING_CONFIRM);
                                        tblLoadingTO.StatusReason = "Loading Scheduled & Confirmed";
                                    }
                                }
                                resultMessage = TblLoadingBL.RightDataFromIotToDB(tblLoadingTO.IdLoading, tblLoadingTO, conn, tran);
                                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                                {
                                    tran.Rollback();
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Text = "Error While Writng Data from DB";
                                    return resultMessage;
                                }

                                removeDataFromIot = true;

                            }
                        }
                    }

                    resultMessage = SpiltBookingAgainstInvoice(invoiceTO, tblLoadingTO, conn, tran);
                    if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                    {
                        return resultMessage;
                    }

                    #region Write Data to Invoice

                    if (Constants.getweightSourceConfigTO() == (Int32)Constants.WeighingDataSourceE.IoT)
                    {
                        if (invoiceTO.IsConfirmed == 1)
                        {
                           // invoiceTO = SelectTblInvoiceTOWithDetails(invoiceTO.IdInvoice, conn, tran);
                            if (invoiceTO == null || invoiceTO.VehicleNo == null || invoiceTO.TransportOrgId == 0)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("invoiceTO Found NULL OR VehicleNo Found NULL OR TransportOrgId Found NULL when write Data to Invoice"); return resultMessage;
                            }

                            var invoiceItemList = invoiceTO.InvoiceItemDetailsTOList.Where(w => w.LoadingSlipExtId > 0).ToList();
                            if (invoiceItemList != null && invoiceItemList.Count > 0)
                            {
                                for (int s = 0; s < invoiceItemList.Count; s++)
                                {
                                    if (invoiceItemList[s].InvoiceQty <= 0)
                                    {
                                        tran.Rollback();
                                        resultMessage.DefaultBehaviour("Invoice Item Qty found zero when write Data to Invoice");
                                        return resultMessage;
                                    }
                                }
                            }


                            Int32 statusId = invoiceTO.StatusId;

                            invoiceTO.StatusId = (Int32)InvoiceStatusE.NEW;

                            // TblInvoiceBL.SetGateAndWeightIotData(invoiceTO);

                            invoiceTO.StatusId = statusId;

                            result = TblInvoiceBL.UpdateTblInvoice(invoiceTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Text = "Error While updating tblInvoiceTO";
                                return resultMessage;
                            }

                            for (int p = 0; p < invoiceTO.InvoiceItemDetailsTOList.Count; p++)
                            {
                                TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = invoiceTO.InvoiceItemDetailsTOList[p];

                                result = TblInvoiceItemDetailsBL.UpdateTblInvoiceItemDetails(tblInvoiceItemDetailsTO, conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Text = "Error While updating tblInvoiceItemTO";
                                    return resultMessage;
                                }

                            }

                        }
                    }
                    #endregion
                }

                #region 4. Save the invoice data to SAP
                if (invoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.AUTHORIZED)
                {
                    TblConfigParamsTO tblConfigParams = TblConfigParamsBL.SelectTblConfigParamsValByName(Constants.CP_POST_SALES_INVOICE_TO_SAP_DIRECTLY_AFTER_INVOICE_GENERATION);
                    if (tblConfigParams != null)
                    {
                        if (Convert.ToInt32(tblConfigParams.ConfigParamVal) == 1)
                        {
                            TblConfigParamsTO tblConfigParamsTOSAPService = TblConfigParamsBL.SelectTblConfigParamsValByName(Constants.SAPB1_SERVICES_ENABLE);
                            if (tblConfigParamsTOSAPService != null)
                            {
                                if (Convert.ToInt32(tblConfigParamsTOSAPService.ConfigParamVal) == 1)
                                {
                                    resultMessage = JsonConvert.DeserializeObject<ResultMessage>(CommonDAO.PostSalesInvoiceToSAP(invoiceTO));
                                    if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                                    {
                                        tran.Rollback();
                                        return resultMessage;
                                    }
                                    TblInvoiceTO tblInvoiceTO = JsonConvert.DeserializeObject<TblInvoiceTO>(resultMessage.data.ToString());
                                    result = TblInvoiceDAO.UpdateMappedSAPInvoiceNo(tblInvoiceTO, conn, tran);
                                    if (result != 1)
                                    {
                                        tran.Rollback();
                                        resultMessage.MessageType = ResultMessageE.Error;
                                        resultMessage.Text = "Error While updating tblInvoiceTO";
                                        return resultMessage;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                if (invoiceTO.IsConfirmed == 1)
                    resultMessage.DisplayMessage = "Success..Invoice authorized and #" + invoiceTO.InvoiceNo + " is generated";
                else
                    resultMessage.DisplayMessage = "Success..tentative loading report is generated";

                resultMessage.Tag = invoiceTO;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "GenerateInvoiceNumber");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }


        public static void SetGateAndWeightIotData(List<TblInvoiceTO> tblInvoiceTOList, int IsExtractionAllowed)
        {
            int weightSourceConfigId = Constants.getweightSourceConfigTO();
            if (weightSourceConfigId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
            {
                if (IsExtractionAllowed == 0)
                {
                    SetGateIotDataToInvoiceTO(tblInvoiceTOList);
                }
                for (int i = 0; i < tblInvoiceTOList.Count; i++)
                {
                    SetWeightIotDateToInvoiceTO(tblInvoiceTOList[i], IsExtractionAllowed);
                }
            }
        }

        public static void SetGateAndWeightIotData(TblInvoiceTO tblInvoiceTO, int IsExtractionAllowed)
        {
            if (tblInvoiceTO != null)
            {
                List<TblInvoiceTO> tblInvoiceTOList = new List<TblInvoiceTO>();
                tblInvoiceTOList.Add(tblInvoiceTO);
                SetGateAndWeightIotData(tblInvoiceTOList, IsExtractionAllowed);
            }
        }

        public static ResultMessage UpdateInvoiceNonCommercialDetails(TblInvoiceTO tblInvoiceTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMSg = new ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                TblInvoiceTO exiInvoiceTO = TblInvoiceDAO.SelectTblInvoice(tblInvoiceTO.IdInvoice, conn, tran);
                if (exiInvoiceTO == null)
                {
                    tran.Rollback();
                    resultMSg.DefaultBehaviour("exiInvoiceTO Found NULL");
                    return resultMSg;
                }

                int result = TblInvoiceDAO.UpdateInvoiceNonCommercDtls(tblInvoiceTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMSg.DefaultBehaviour("Error While UpdateInvoiceNonCommercDtls");
                    return resultMSg;
                }

                //Generate inv history record
                TblInvoiceHistoryTO invHistoryTO = new TblInvoiceHistoryTO();
                invHistoryTO.InvoiceId = tblInvoiceTO.IdInvoice;
                invHistoryTO.CreatedOn = tblInvoiceTO.UpdatedOn;
                invHistoryTO.CreatedBy = tblInvoiceTO.UpdatedBy;
                invHistoryTO.StatusDate = tblInvoiceTO.UpdatedOn;
                invHistoryTO.StatusId = exiInvoiceTO.StatusId;
                invHistoryTO.OldEwayBillNo = exiInvoiceTO.ElectronicRefNo;
                invHistoryTO.NewEwayBillNo = tblInvoiceTO.ElectronicRefNo;
                invHistoryTO.StatusRemark = "Non-Commercial Details Update";

                result = BL.TblInvoiceHistoryBL.InsertTblInvoiceHistory(invHistoryTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMSg.DefaultBehaviour("Error While InsertTblInvoiceHistory"); return resultMSg;
                }

                tran.Commit();
                resultMSg.DefaultSuccessBehaviour();
                return resultMSg;
            }
            catch (Exception ex)
            {
                resultMSg.DefaultExceptionBehaviour(ex, "");
                return resultMSg;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// GJ@20171001 : update the Invoice ConfirmNonConfirm Status with Calculation
        /// </summary>
        /// <param name="idInvoice"></param>
        /// <returns></returns>
        /// 
        public static ResultMessage UpdateInvoiceConfrimNonConfirmDetails(TblInvoiceTO tblInvoiceTO, Int32 loginUserId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            Double totalInvQty = 0;
            Double totalNCExpAmt = 0;
            Double totalNCOtherAmt = 0;
            double conversionFactor = 0.001;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                TblInvoiceTO exiInvoiceTO = BL.TblInvoiceBL.SelectTblInvoiceTOWithDetails(tblInvoiceTO.IdInvoice, conn, tran);
                if (exiInvoiceTO == null)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("exiInvoiceTO Found NULL");
                    return resultMessage;
                }
                exiInvoiceTO.IsConfirmed = tblInvoiceTO.IsConfirmed;
                exiInvoiceTO.UpdatedBy = tblInvoiceTO.UpdatedBy;
                exiInvoiceTO.UpdatedOn = tblInvoiceTO.UpdatedOn;

                //Call to get the Loading Slip detail againest Loading Slip
                TblLoadingSlipDtlTO tblLoadingSlipDtlTO = new TblLoadingSlipDtlTO();
                TblLoadingSlipTO loadingSlipTO = new TblLoadingSlipTO();
                if (tblInvoiceTO.LoadingSlipId != 0)
                {
                    loadingSlipTO = BL.TblLoadingSlipBL.SelectAllLoadingSlipWithDetails(tblInvoiceTO.LoadingSlipId, conn, tran);
                    if (loadingSlipTO == null)
                    {
                        resultMessage.DefaultBehaviour("loadingSlipTO Found NULL");
                        return resultMessage;
                    }
                    //loadingSlipTO.IsConfirmed = tblInvoiceTO.IsConfirmed;
                    tblLoadingSlipDtlTO = BL.TblLoadingSlipDtlBL.SelectLoadingSlipDtlTO(tblInvoiceTO.LoadingSlipId, conn, tran);
                    //if (tblLoadingSlipDtlTO == null)
                    //{
                    //    tran.Rollback();
                    //    resultMessage.MessageType = ResultMessageE.Error;
                    //    resultMessage.Text = "Error :tblLoadingSlipDtlTO Found NUll Or Empty";
                    //    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    //    return resultMessage;
                    //}
                }
                if (tblInvoiceTO.LoadingSlipId == 0 || tblLoadingSlipDtlTO == null)
                {
                    int result = 0;
                    result = DAL.TblInvoiceDAO.UpdateTblInvoice(tblInvoiceTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Error While UpdateInvoiceConfrimNonConfirmDetails");
                        return resultMessage;
                    }
                    if (tblInvoiceTO.LoadingSlipId != 0)
                    {
                        // Vaibhav [28-Feb-2018] To solve invoice conversion issue i.e. C-NC.
                        //loadingSlipTO.IsConfirmed = tblInvoiceTO.IsConfirmed;

                        resultMessage = BL.TblLoadingSlipBL.ChangeLoadingSlipConfirmationStatus(loadingSlipTO, tblInvoiceTO.UpdatedBy, conn, tran);
                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While ChangeLoadingSlipConfirmationStatus");
                            return resultMessage;
                        }
                    }


                    tran.Commit();

                    resultMessage.DefaultSuccessBehaviour();
                    if (tblInvoiceTO.IsConfirmed == 1 && tblInvoiceTO.StatusId == Convert.ToInt32(Constants.InvoiceStatusE.AUTHORIZED))
                    {
                        Int32 isconfirm = 0;
                        GenerateInvoiceNumber(tblInvoiceTO.IdInvoice, loginUserId, isconfirm, (int)Constants.InvoiceGenerateModeE.REGULAR);

                    }
                    return resultMessage;
                }

                //Call to get the TblBooking for Parity Id
                TblBookingsTO tblBookingsTO = new Models.TblBookingsTO();
                tblBookingsTO = BL.TblBookingsBL.SelectTblBookingsTO(tblLoadingSlipDtlTO.BookingId, conn, tran);
                if (tblBookingsTO == null)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error :tblBookingsTO Found NUll Or Empty";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return resultMessage;
                }
                if (exiInvoiceTO.InvoiceItemDetailsTOList != null && exiInvoiceTO.InvoiceItemDetailsTOList.Count > 0)
                {
                    #region Sudhir [26-MARCH-2018] Commented 
                    // List<TblParityDetailsTO> parityDetailsTOList = null;
                    // if (tblBookingsTO.ParityId > 0)
                    //     parityDetailsTOList = BL.TblParityDetailsBL.SelectAllTblParityDetailsList(tblBookingsTO.ParityId, 0, conn, tran);
                    #endregion
                    //Harshala
                    if (tblInvoiceTO.IsConfirmed == 0)
                    {
                        Int32 tcsTaxId = 0;
                        TblConfigParamsTO configParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_TCS_OTHER_TAX_ID, conn, tran);
                        if (configParamsTO != null)
                        {
                            tcsTaxId = Convert.ToInt32(configParamsTO.ConfigParamVal);
                        }
                        if (tcsTaxId > 0)
                            exiInvoiceTO.InvoiceItemDetailsTOList = exiInvoiceTO.InvoiceItemDetailsTOList.Where(w => w.OtherTaxId != tcsTaxId).ToList();
                    }
                    for (int e = 0; e < exiInvoiceTO.InvoiceItemDetailsTOList.Count; e++)
                    {

                        TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = exiInvoiceTO.InvoiceItemDetailsTOList[e];
                        if (tblInvoiceItemDetailsTO.OtherTaxId == 0)
                        {

                            TblLoadingSlipExtTO tblLoadingSlipExtTO = BL.TblLoadingSlipExtBL.SelectTblLoadingSlipExtTO(tblInvoiceItemDetailsTO.LoadingSlipExtId, conn, tran);

                            if (tblLoadingSlipExtTO != null && tblLoadingSlipExtTO.LoadingQty > 0)
                            {
                                #region Calculate Actual Price From Booking and Parity Settings

                                //Double orcAmtPerTon = 0;
                                //if (tblBookingsTO.OrcMeasure == "Rs/MT")
                                //{
                                //    orcAmtPerTon = tblBookingsTO.OrcAmt;
                                //}
                                //else
                                //{
                                //    if (tblBookingsTO.OrcAmt > 0)
                                //        orcAmtPerTon = tblBookingsTO.OrcAmt / tblBookingsTO.BookingQty;
                                //}
                                Double orcAmtPerTon = 0;
                                if (loadingSlipTO.OrcMeasure == "Rs/MT")
                                {
                                    //orcAmtPerTon = tblBookingsTO.OrcAmt; Sudhir[30-APR-2018] ORC From Loading Slip Instead of Booking.
                                    orcAmtPerTon = loadingSlipTO.OrcAmt;
                                }
                                else
                                {
                                    //if (tblBookingsTO.OrcAmt > 0)
                                    //    orcAmtPerTon = tblBookingsTO.OrcAmt / tblBookingsTO.BookingQty;
                                    if (loadingSlipTO.OrcAmt > 0)
                                        orcAmtPerTon = loadingSlipTO.OrcAmt / tblLoadingSlipDtlTO.LoadingQty;
                                }

                                Double bookingPrice = tblBookingsTO.BookingRate;
                                Double parityAmt = 0;
                                Double priceSetOff = 0;
                                Double paritySettingAmt = 0;
                                Double bvcAmt = 0;
                                //TblParitySummaryTO parityTO = null; Sudhir[26 - MARCH - 2018] Commented As per new Parity Setting
                                TblParityDetailsTO parityDtlTO = null;

                                TblAddressTO addrTO = BL.TblAddressBL.SelectOrgAddressWrtAddrType(tblBookingsTO.DealerOrgId, Constants.AddressTypeE.OFFICE_ADDRESS, conn, tran);

                                parityDtlTO = BL.TblParityDetailsBL.SelectParityDetailToListOnBooking(tblLoadingSlipExtTO.MaterialId, tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, tblLoadingSlipExtTO.ProdItemId, addrTO.StateId, tblBookingsTO.BookingDatetime);


                                //  if (parityDetailsTOList != null) //Vijaymala Commented[18-06-2018]-to get data from paritydetails
                                //{
                                //Vijaymala Commented[18-06-2018]-to get data from paritydetails

                                //var parityDtlTO = parityDetailsTOList.Where(m => m.MaterialId == tblLoadingSlipExtTO.MaterialId
                                //                               && m.ProdCatId == tblLoadingSlipExtTO.ProdCatId
                                //                              && m.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId).FirstOrDefault();
                                if (parityDtlTO != null)
                                {
                                    parityAmt = parityDtlTO.ParityAmt;
                                    if (tblInvoiceTO.IsConfirmed != 1)
                                        priceSetOff = parityDtlTO.NonConfParityAmt;
                                    else
                                        priceSetOff = 0;

                                }
                                else
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour();
                                    resultMessage.Text = "Error : ParityDtlTO Not Found";
                                    string mateDesc = tblLoadingSlipExtTO.DisplayName;
                                    //tblLoadingSlipExtTO.MaterialDesc + " " + tblLoadingSlipExtTO.ProdCatDesc + "-" + tblLoadingSlipExtTO.ProdSpecDesc;
                                    resultMessage.DisplayMessage = "Warning : Parity Details Not Found For " + mateDesc + " Please contact BackOffice";
                                    return resultMessage;
                                }
                                //parityTO = BL.TblParitySummaryBL.SelectTblParitySummaryTO(parityDtlTO.ParityId, conn, tran);
                                //if (parityTO == null)
                                //{
                                //    tran.Rollback();
                                //    resultMessage.DefaultBehaviour();
                                //    resultMessage.Text = "Error : ParityTO Not Found";
                                //    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                //    return resultMessage;
                                //}
                                paritySettingAmt = parityDtlTO.BaseValCorAmt + parityDtlTO.ExpenseAmt + parityDtlTO.OtherAmt;
                                bvcAmt = parityDtlTO.BaseValCorAmt;

                                //}
                                //else
                                //{
                                //    tran.Rollback();
                                //    resultMessage.DefaultBehaviour();
                                //    resultMessage.Text = "Error : ParityTO Not Found";
                                //    resultMessage.DisplayMessage = "Warning : Parity Details Not Found, Please contact BackOffice";
                                //    return resultMessage;
                                //}
                                Double cdApplicableAmt = (bookingPrice + orcAmtPerTon + parityAmt + priceSetOff + bvcAmt);
                                if (tblInvoiceTO.IsConfirmed == 1)
                                    cdApplicableAmt += parityDtlTO.ExpenseAmt + parityDtlTO.OtherAmt;
                                tblInvoiceItemDetailsTO.Rate = cdApplicableAmt;

                                #endregion
                            }
                            else
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("Error : tblLoadingSlipExtTO Found Null Or Empty");
                                return resultMessage;
                            }
                        }
                    }
                }
                else
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "Error : InvoiceItemDetailsTOList(Invoice Item Details) Found Null Or Empty";
                    resultMessage.DisplayMessage = "Error 01 : No Items found to change the Status.";
                    return resultMessage;
                }

                exiInvoiceTO = updateInvoiceToCalc(exiInvoiceTO, conn, tran, false);
                if (tblInvoiceTO.IsConfirmed == 0)
                {
                    for (int i = 0; i < loadingSlipTO.LoadingSlipExtTOList.Count; i++)
                    {
                        TblLoadingSlipExtTO tblLoadingSlipExt = loadingSlipTO.LoadingSlipExtTOList[i];

                        //Vijaymala[18-06-2018] added to get data from parity details
                        TblAddressTO addrTO = BL.TblAddressBL.SelectOrgAddressWrtAddrType(tblBookingsTO.DealerOrgId, Constants.AddressTypeE.OFFICE_ADDRESS, conn, tran);

                        TblParityDetailsTO parityDtlTO = BL.TblParityDetailsBL.SelectParityDetailToListOnBooking(tblLoadingSlipExt.MaterialId, tblLoadingSlipExt.ProdCatId, tblLoadingSlipExt.ProdSpecId, tblLoadingSlipExt.ProdItemId, addrTO.StateId, tblBookingsTO.BookingDatetime);


                        //TblParitySummaryTO parityTO = BL.TblParitySummaryBL.SelectParitySummaryTOFromParityDtlId(tblLoadingSlipExt.ParityDtlId, conn, tran);
                        if (parityDtlTO != null)
                        {
                            totalNCExpAmt += parityDtlTO.ExpenseAmt * Math.Round(tblLoadingSlipExt.LoadedWeight * conversionFactor, 2);
                            totalNCOtherAmt += parityDtlTO.OtherAmt * Math.Round(tblLoadingSlipExt.LoadedWeight * conversionFactor, 2);
                        }
                        else
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour();
                            resultMessage.Text = "Error : ParityDtlTO Not Found";
                            string mateDesc = tblLoadingSlipExt.DisplayName;
                            //tblLoadingSlipExt.MaterialDesc + " " + tblLoadingSlipExt.ProdCatDesc + "-" + tblLoadingSlipExt.ProdSpecDesc;
                            resultMessage.DisplayMessage = "Warning : Parity Details Not Found For " + mateDesc + " Please contact BackOffice";
                            return resultMessage;
                        }
                    }
                    exiInvoiceTO.ExpenseAmt = totalNCExpAmt;
                    exiInvoiceTO.OtherAmt = totalNCOtherAmt;
                    exiInvoiceTO.GrandTotal += totalNCExpAmt + totalNCOtherAmt;

                }
                Double tdsTaxPct = 0;
                if (exiInvoiceTO != null)
                {
                    if (exiInvoiceTO.IsTcsApplicable == 0)
                    {
                        TblConfigParamsTO tdsConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DELIVER_INVOICE_TDS_TAX_PCT, conn, tran);
                        if (tdsConfigParamsTO != null)
                        {
                            if (!String.IsNullOrEmpty(tdsConfigParamsTO.ConfigParamVal))
                            {
                                tdsTaxPct = Convert.ToDouble(tdsConfigParamsTO.ConfigParamVal);
                            }
                        }
                    }
                }
                exiInvoiceTO.TdsAmt = 0;
                if (exiInvoiceTO.IsConfirmed == 1)
                {
                    exiInvoiceTO.TdsAmt = (CalculateTDS(exiInvoiceTO) * tdsTaxPct) / 100;
                    exiInvoiceTO.TdsAmt = Math.Ceiling(exiInvoiceTO.TdsAmt);
                }
                resultMessage = BL.TblInvoiceBL.UpdateInvoice(exiInvoiceTO, conn, tran);
                if (resultMessage.MessageType != ResultMessageE.Information)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While UpdateInvoiceConfrimNonConfirmDetails");
                    return resultMessage;
                }
                //Update the Loading Slip To Details
                resultMessage = BL.TblLoadingSlipBL.ChangeLoadingSlipConfirmationStatus(loadingSlipTO, tblInvoiceTO.UpdatedBy, conn, tran);
                if (resultMessage.MessageType != ResultMessageE.Information)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While ChangeLoadingSlipConfirmationStatus");
                    return resultMessage;
                }
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();

                if (tblInvoiceTO.IsConfirmed == 1 && tblInvoiceTO.StatusId == Convert.ToInt32(Constants.InvoiceStatusE.AUTHORIZED))
                {
                    Int32 isconfirm = 0;
                    GenerateInvoiceNumber(tblInvoiceTO.IdInvoice, loginUserId, isconfirm, (int)Constants.InvoiceGenerateModeE.REGULAR);

                }
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateInvoiceConfrimNonConfirmDetails");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Vijaymala [30-03-2018] added:to update invoice deliveredOn date after loading slip out
        /// </summary>
        /// <param name="idInvoice"></param>
        /// <returns></returns>
        public static ResultMessage UpdateInvoiceAfterloadingSlipOut(Int32 loadingId, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            Int32 result = 0;
            #region [30-03-2018]Vijaymala Added :To update invoice delivered on date

            List<TblLoadingSlipTO> loadingSlipTOList = new List<TblLoadingSlipTO>();
            loadingSlipTOList = BL.TblLoadingSlipBL.SelectAllLoadingSlipListWithDetails(loadingId, conn, tran);
            if (loadingSlipTOList == null || loadingSlipTOList.Count == 0)
            {
                resultMessage.DefaultBehaviour("Loading Slip List Found Null againest Loading Id");
                return resultMessage;
            }
            DateTime deliveredOn = Constants.ServerDateTime;
            for (int i = 0; i < loadingSlipTOList.Count; i++)
            {
                List<TblInvoiceTO> invoiceToList = new List<TblInvoiceTO>();
                invoiceToList = BL.TblInvoiceBL.SelectInvoiceListFromLoadingSlipId(loadingSlipTOList[i].IdLoadingSlip, conn, tran);
                if (invoiceToList == null || invoiceToList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("Invoice List Found Null");
                    return resultMessage;
                }
                for (int j = 0; j < invoiceToList.Count; j++)
                {
                    TblInvoiceTO tblInvoiceTO = invoiceToList[j];
                    tblInvoiceTO.DeliveredOn = deliveredOn;
                    result = BL.TblInvoiceBL.UpdateTblInvoice(tblInvoiceTO, conn, tran);

                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour();
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.Text = "Error While UpdateTblInvoice While updating Loading Slip Status";
                        return resultMessage;
                    }

                }


            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;




            #endregion
        }

        /// <summary>
        /// Vijaymala[22-05-2018] : Added To deactivate invoice document details.
        /// </summary>
        /// <returns></returns>
        /// 
        public static ResultMessage DeactivateInvoiceDocumentDetails(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO, Int32 loginUserId)
        {
            ResultMessage resultMessage = new ResultMessage();
            Int32 result = 0;
            if (tempInvoiceDocumentDetailsTO == null)
            {
                resultMessage.DefaultBehaviour("tempInvoiceDocumentDetailsTO Found Null againest document Id");
                return resultMessage;
            }
            DateTime serverDateTime = Constants.ServerDateTime;
            tempInvoiceDocumentDetailsTO.IsActive = 0;
            tempInvoiceDocumentDetailsTO.UpdatedOn = serverDateTime;
            tempInvoiceDocumentDetailsTO.UpdatedBy = loginUserId;
            result = BL.TempInvoiceDocumentDetailsBL.UpdateTempInvoiceDocumentDetails(tempInvoiceDocumentDetailsTO);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour();
                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                resultMessage.Text = "Error While UpdateTempInvoiceDocumentDetails While updating invoice document details";
                return resultMessage;
            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }
        #endregion

        #region Deletion
        public static int DeleteTblInvoice(Int32 idInvoice)
        {
            return TblInvoiceDAO.DeleteTblInvoice(idInvoice);
        }

        public static int DeleteTblInvoice(Int32 idInvoice, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceDAO.DeleteTblInvoice(idInvoice, conn, tran);
        }

        public static ResultMessage DeleteTblInvoiceDetails(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            Int32 result = 0;

            if (tblInvoiceTO != null)
            {

                #region 2. To delete Invoices History
                result = BL.TblInvoiceHistoryBL.DeleteTblInvoiceHistoryByInvoiceId(tblInvoiceTO.IdInvoice, conn, tran);
                if (result == -1)
                {
                    resultMessage.DefaultBehaviour("Error While DeleteTblInvoiceHistoryByInvoiceId");
                    return resultMessage;
                }
                #endregion

                #region 3. To delete Invoices Document Details
                result = BL.TempInvoiceDocumentDetailsBL.DeleteTblInvoiceDocumentByInvoiceId(tblInvoiceTO.IdInvoice, conn, tran);
                if (result == -1)
                {
                    resultMessage.DefaultBehaviour("Error While DeleteTblInvoiceDocumentByInvoiceId");
                    return resultMessage;
                }
                #endregion

                #region 4.To delete Invoice Address
                result = BL.TblInvoiceAddressBL.DeleteTblInvoiceAddressByinvoiceId(tblInvoiceTO.IdInvoice, conn, tran);
                if (result == -1)
                {
                    resultMessage.DefaultBehaviour("Error While DeleteTblInvoiceAddressByinvoiceId");
                    return resultMessage;
                }
                #endregion



                #region 5. Delete Previous Tax Details
                result = TblInvoiceItemTaxDtlsBL.DeleteInvoiceItemTaxDtlsByInvId(tblInvoiceTO.IdInvoice, conn, tran);
                if (result == -1)
                {
                    resultMessage.DefaultBehaviour("Error in DeleteTblInvoiceItemTaxDtls");
                    return resultMessage;
                }

                #endregion

                #region 6.To delete Invoice Item

                result = BL.TblInvoiceItemDetailsBL.DeleteTblInvoiceItemDetailsByInvoiceId(tblInvoiceTO.IdInvoice, conn, tran);
                if (result == -1)
                {
                    resultMessage.DefaultBehaviour("Error While DeleteTblInvoiceItemDetails");
                    return resultMessage;
                }

                #endregion


                #region 7. To delete Invoices
                result = BL.TblInvoiceBL.DeleteTblInvoice(tblInvoiceTO.IdInvoice, conn, tran);
                if (result == -1)
                {
                    resultMessage.DefaultBehaviour("Error While DeleteTblInvoice");
                    return resultMessage;
                }

                #endregion

            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }

        #endregion

        #region ExtractEnquiryData

        public static ResultMessage ExtractEnquiryData()
        {
            SqlConnection bookingConn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction bookingTran = null;
            SqlConnection enquiryConn = new SqlConnection(Startup.NewConnectionString);
            SqlTransaction enquiryTran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            List<TblLoadingTO> tempLoadingTOList = new List<TblLoadingTO>();
            List<TblInvoiceRptTO> tblInvoiceRptTOList = new List<TblInvoiceRptTO>();
            //List<TblUnLoadingTO> tblUnLoadingTOList = new List<TblUnLoadingTO>();

            int result = 0;
            int loadingCount = 0;
            int totalLoading = 0;

            List<int> loadingIdList = new List<int>();
            String loadingIds = String.Empty;

            //List<int> unLoadingIdList = new List<int>();
            //String unLoadingIds = String.Empty;

            try
            {

                if (bookingConn.State == ConnectionState.Closed)
                {
                    bookingConn.Open();
                    bookingTran = bookingConn.BeginTransaction();
                }
                TblConfigParamsTO configParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsValByName(Constants.CP_MIGRATE_ENQUIRY_DATA);

                if (configParamsTO.ConfigParamVal == "1")
                {
                    if (enquiryConn.State == ConnectionState.Closed)
                    {
                        try
                        {
                            enquiryConn.Open();
                            enquiryTran = enquiryConn.BeginTransaction();
                        }
                        catch (Exception ex)
                        {
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.DefaultBehaviour(ex.Message);
                            return resultMessage;
                        }
                    }
                }


                // Select temp loading data.
                tempLoadingTOList = BL.TblLoadingBL.SelectAllTempLoading(bookingConn, bookingTran);
                if (tempLoadingTOList == null || tempLoadingTOList.Count <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.DefaultBehaviour("Record not found!! tempLoadingTOList is null. ");
                    return resultMessage;
                }

                // Select temp invoice data for creating excel file.
                tblInvoiceRptTOList = BL.FinalBookingData.SelectTempInvoiceData(bookingConn, bookingTran);
                
                if (tempLoadingTOList != null && tempLoadingTOList.Count > 0)
                {
                    foreach (var tempLoadingTO in tempLoadingTOList)
                    {
                        loadingIdList.Add(tempLoadingTO.IdLoading);

                        #region Handle Connection
                        loadingCount = loadingCount + 1;
                        totalLoading = totalLoading + 1;

                        if (bookingConn.State == ConnectionState.Closed)
                        {
                            bookingConn.Open();
                            bookingTran = bookingConn.BeginTransaction();
                        }

                        if (configParamsTO.ConfigParamVal == "1")
                        {
                            if (enquiryConn.State == ConnectionState.Closed)
                            {
                                enquiryConn.Open();
                                enquiryTran = enquiryConn.BeginTransaction();
                            }
                        }
                        #endregion

                        #region Insert Booking Data
                        resultMessage = BL.FinalBookingData.InsertFinalBookingData(tempLoadingTO.IdLoading, bookingConn, bookingTran);
                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            bookingTran.Rollback();
                            enquiryTran.Rollback();
                            BL.FinalBookingData.UpdateIdentityFinalTables(bookingConn, bookingTran);
                            BL.FinalEnquiryData.UpdateIdentityFinalTables(enquiryConn, enquiryTran);
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error while InsertFinalBookingData";
                            return resultMessage;
                        }
                        #endregion

                        #region Insert Enquiry Data

                        //TblConfigParamsTO configParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsValByName(Constants.CP_MIGRATE_ENQUIRY_DATA);

                        if (configParamsTO.ConfigParamVal == "1")
                        {
                            resultMessage = BL.FinalEnquiryData.InsertFinalEnquiryData(tempLoadingTO.IdLoading, bookingConn, bookingTran, enquiryConn, enquiryTran);
                            if (resultMessage.MessageType != ResultMessageE.Information)
                            {
                                bookingTran.Rollback();
                                enquiryTran.Rollback();
                                BL.FinalBookingData.UpdateIdentityFinalTables(bookingConn, bookingTran);
                                BL.FinalEnquiryData.UpdateIdentityFinalTables(enquiryConn, enquiryTran);
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Text = "Error while InsertFinalEnquiryData";
                                return resultMessage;
                            }
                        }

                        #endregion

                        #region Delete transactional data
                        result = BL.FinalBookingData.DeleteTempLoadingData(tempLoadingTO.IdLoading, bookingConn, bookingTran);
                        if (result < 0)
                        {
                            bookingTran.Rollback();
                            enquiryTran.Rollback();
                            BL.FinalBookingData.UpdateIdentityFinalTables(bookingConn, bookingTran);
                            BL.FinalEnquiryData.UpdateIdentityFinalTables(enquiryConn, enquiryTran);
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error while DeleteTempLoadingData";
                            return resultMessage;
                        }
                        #endregion

                        #region Create Excel File. Delete Stock & Quota. Reset SQL Connection.
                        if (loadingCount == Constants.LoadingCountForDataExtraction || totalLoading == tempLoadingTOList.Count)
                        {
                            #region Create Excel File  

                            if (tblInvoiceRptTOList != null && tblInvoiceRptTOList.Count > 0)
                            {
                                List<TblInvoiceRptTO> enquiryInvoiceList = new List<TblInvoiceRptTO>();

                                if (loadingIdList != null && loadingIdList.Count > 0)
                                {
                                    foreach (var loadingId in loadingIdList)
                                    {
                                        enquiryInvoiceList.AddRange(tblInvoiceRptTOList.FindAll(ele => ele.LoadingId == loadingId));
                                    }
                                }

                                if (enquiryInvoiceList != null && enquiryInvoiceList.Count > 0)
                                {
                                    result = BL.FinalBookingData.CreateTempInvoiceExcel(enquiryInvoiceList, bookingConn, bookingTran);

                                    if (result != 1)
                                    {
                                        bookingTran.Rollback();
                                        enquiryTran.Rollback();
                                        BL.FinalBookingData.UpdateIdentityFinalTables(bookingConn, bookingTran);
                                        BL.FinalEnquiryData.UpdateIdentityFinalTables(enquiryConn, enquiryTran);
                                        resultMessage.MessageType = ResultMessageE.Error;
                                        resultMessage.Text = "Error while creating excel file.";
                                        return resultMessage;
                                    }
                                }
                            }
                            else
                            {
                                resultMessage.MessageType = ResultMessageE.Information;
                                resultMessage.Text = "Information : tblInvoiceRptTOList is null. Excel file is not created.";
                                //return resultMessage;
                            }
                            #endregion

                            #region Delete Stock And Quota
                            result = BL.FinalBookingData.DeleteYesterdaysStock(bookingConn, bookingTran);
                            if (result < 0)
                            {
                                bookingTran.Rollback();
                                enquiryTran.Rollback();
                                BL.FinalBookingData.UpdateIdentityFinalTables(bookingConn, bookingTran);
                                BL.FinalEnquiryData.UpdateIdentityFinalTables(enquiryConn, enquiryTran);
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Text = "Error while DeleteYesterdaysStock";
                                return resultMessage;
                            }

                            result = BL.FinalBookingData.DeleteYesterdaysLoadingQuotaDeclaration(bookingConn, bookingTran);
                            if (result < 0)
                            {
                                bookingTran.Rollback();
                                enquiryTran.Rollback();
                                BL.FinalBookingData.UpdateIdentityFinalTables(bookingConn, bookingTran);
                                BL.FinalEnquiryData.UpdateIdentityFinalTables(enquiryConn, enquiryTran);
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Text = "Error while DeleteYesterdaysQuotaDeclaration";
                                return resultMessage;
                            }

                            #endregion


                            bookingTran.Commit();
                            bookingConn.Close();
                            bookingTran.Dispose();

                            if (configParamsTO.ConfigParamVal == "1")
                            {
                                enquiryTran.Commit();
                                enquiryConn.Close();
                                enquiryTran.Dispose();
                            }

                            loadingCount = 0;
                            loadingIdList.Clear();
                        }
                        #endregion
                    }
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                bookingTran.Rollback();
                BL.FinalBookingData.UpdateIdentityFinalTables(bookingConn, bookingTran);

                if (enquiryTran.Connection.State == ConnectionState.Open)
                {
                    enquiryTran.Rollback();
                    BL.FinalEnquiryData.UpdateIdentityFinalTables(enquiryConn, enquiryTran);
                }
                resultMessage.DefaultExceptionBehaviour(ex, "ExtractEnquiryData");
                return resultMessage;
            }
            finally
            {
                bookingConn.Close();
                enquiryConn.Close();
            }
        }

        //Added by minal 02 March 2021
        public static ResultMessage CreateAndBackupExcelFile()
        {
            ResultMessage resultMessage = new ResultMessage();            
            DateTime toDate = Constants.ServerDateTime;
            int isConfirmed = (int)Constants.ConfirmedTypeE.NONCONFIRM;
            try
            {
                resultMessage = SelectAndExportToExcelTallyStockTransferReport(toDate);
                if (resultMessage.MessageType != ResultMessageE.Information)
                {
                    return resultMessage;
                }

                resultMessage = SelectAndExportToExcelCCStockTransferReport(toDate);
                if (resultMessage.MessageType != ResultMessageE.Information)
                {
                    return resultMessage;
                }

                resultMessage = SelectAndExportToExcelSalesChartEnquiryReport(toDate, isConfirmed);
                if (resultMessage.MessageType != ResultMessageE.Information)
                {
                    return resultMessage;
                }
                /*resultMessage = SelectAndExportToExcelWBReport(toDate);
                if (resultMessage.MessageType != ResultMessageE.Information)
                {
                    return resultMessage;
                }*/
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "CreateAndBackupExcelFile");
                return resultMessage;
            }
        }

        public static ResultMessage SelectAndExportToExcelTallyStockTransferReport(DateTime toDate)
        {
            ResultMessage resultMessage = new ResultMessage();
            DateTime fromDate = new DateTime();
            try
            {
                List<TblReportsBackupDtlsTO> reportBackUpList = TblReportsBackupDtlsBL.SelectReportBackupDateDtls(Constants.DeliverReportNameE.TALLY_STOCK_TRANSFER_REPORT.ToString());
                fromDate = toDate.AddHours(-24);
                
                if (reportBackUpList != null && reportBackUpList.Count > 0)
                {
                    TblReportsBackupDtlsTO tblReportsBackupDtlsTO = reportBackUpList[0];
                    fromDate = tblReportsBackupDtlsTO.BackupDate;
                }

                List<TblTallyStockTransferRptTO> tblTallyStockTransferRptTOList = TblInvoiceDAO.SelectTallyStockTransferDetails(fromDate,toDate);
                if (tblTallyStockTransferRptTOList == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.DefaultBehaviour("Record not found!! tblTallyStockTransferRptTOList is null. ");
                    return resultMessage;
                }
                TblReportsBackupDtlsTO reportBackUpTO = new TblReportsBackupDtlsTO();
                Int32 result = 0;
                if (tblTallyStockTransferRptTOList.Count > 0)
                {
                    //crete file and uplod res
                    result = CreateTempTallyStockTransferExcel(tblTallyStockTransferRptTOList);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour();
                        resultMessage.Text = "Error while creating excel file.";
                        return resultMessage;
                    }
                    reportBackUpTO.IsBackUp = 1;
                }
                else
                {
                    reportBackUpTO.IsBackUp = 0;
                }

                reportBackUpTO.ReportName = Constants.DeliverReportNameE.TALLY_STOCK_TRANSFER_REPORT.ToString();
                reportBackUpTO.BackupDate = toDate;
                
                result = TblReportsBackupDtlsBL.InsertTblReportsBackupDtls(reportBackUpTO);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "Error while InsertTblReportsBackupDtls(reportBackUpTO).";
                    return resultMessage;
                }


                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in SelectAndExportToExcelTallyTransportReport()");
                return resultMessage;
            }
        }

        public static int CreateTempTallyStockTransferExcel(List<TblTallyStockTransferRptTO> tblTallyStockTransferRptTOList)
        {
            ResultMessage resultMessage = new ResultMessage();
            ExcelPackage excelPackage = new ExcelPackage();
            int cellRow = 2;
            Int16 mathroundFact = 3;
            Double conversionFact = 1000;
            try
            {

                if (tblTallyStockTransferRptTOList == null || tblTallyStockTransferRptTOList.Count == 0)
                {
                    return 0;
                }

                #region Create Excel File
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add(Constants.ExcelSheetName);

                excelWorksheet.Cells[1, 1].Value = "Date";
                excelWorksheet.Cells[1, 2].Value = "VOUCHER TYPE";
                excelWorksheet.Cells[1, 3].Value = "STOCK ITEM";
                excelWorksheet.Cells[1, 4].Value = "GODOWN [C & F]";
                excelWorksheet.Cells[1, 5].Value = "QTY";
                excelWorksheet.Cells[1, 6].Value = "RATE";
                excelWorksheet.Cells[1, 7].Value = "AMOUNT";
                excelWorksheet.Cells[1, 8].Value = " STOCK ITEM";
                excelWorksheet.Cells[1, 9].Value = " GODOWN";
                excelWorksheet.Cells[1, 10].Value = " QTY";
                excelWorksheet.Cells[1, 11].Value = " RATE";
                excelWorksheet.Cells[1, 12].Value = " AMOUNT";
                excelWorksheet.Cells[1, 13].Value = " NARRATION";

                excelWorksheet.Cells[1, 1, 1, 13].Style.Font.Bold = true;

                for (int i = 0; i < tblTallyStockTransferRptTOList.Count; i++)
                {

                    excelWorksheet.Cells[cellRow, 1].Value = tblTallyStockTransferRptTOList[i].Date;
                    excelWorksheet.Cells[cellRow, 2].Value = tblTallyStockTransferRptTOList[i].VoucherType;
                    excelWorksheet.Cells[cellRow, 3].Value = tblTallyStockTransferRptTOList[i].StockItem;
                    excelWorksheet.Cells[cellRow, 4].Value = tblTallyStockTransferRptTOList[i].Godown;
                    excelWorksheet.Cells[cellRow, 5].Style.Numberformat.Format = "#,##0.000";
                    excelWorksheet.Cells[cellRow, 5].Value = tblTallyStockTransferRptTOList[i].Qty;
                    Double rate = Math.Round(((tblTallyStockTransferRptTOList[i].Rate)/conversionFact), mathroundFact);
                    excelWorksheet.Cells[cellRow, 6].Style.Numberformat.Format = "#,##0.000";
                    excelWorksheet.Cells[cellRow, 6].Value = rate;
                    Double amount = Math.Round(((tblTallyStockTransferRptTOList[i].Amount) / conversionFact), mathroundFact);
                    excelWorksheet.Cells[cellRow, 7].Style.Numberformat.Format = "#,##0.000";
                    excelWorksheet.Cells[cellRow, 7].Value = amount;
                    excelWorksheet.Cells[cellRow, 8].Value = tblTallyStockTransferRptTOList[i].StockItemD;
                    excelWorksheet.Cells[cellRow, 9].Value = tblTallyStockTransferRptTOList[i].GodownD;
                    excelWorksheet.Cells[cellRow, 10].Style.Numberformat.Format = "#,##0.000";
                    excelWorksheet.Cells[cellRow, 10].Value = tblTallyStockTransferRptTOList[i].QtyD;
                    Double rateD = Math.Round(((tblTallyStockTransferRptTOList[i].RateD) / conversionFact), mathroundFact);
                    excelWorksheet.Cells[cellRow, 11].Style.Numberformat.Format = "#,##0.000";
                    excelWorksheet.Cells[cellRow, 11].Value = rateD;
                    Double amountD = Math.Round(((tblTallyStockTransferRptTOList[i].AmountD) / conversionFact), mathroundFact);
                    excelWorksheet.Cells[cellRow, 12].Style.Numberformat.Format = "#,##0.000";
                    excelWorksheet.Cells[cellRow, 12].Value = amountD;
                    excelWorksheet.Cells[cellRow, 13].Value = tblTallyStockTransferRptTOList[i].Narration;

                    cellRow++;

                    if (i == (tblTallyStockTransferRptTOList.Count - 1))
                    {
                        excelWorksheet.Cells[cellRow, 3].Value = "Total";
                        Double totalQty = Math.Round((tblTallyStockTransferRptTOList.Sum(ele => ele.Qty)), 3);
                        excelWorksheet.Cells[cellRow, 5].Style.Numberformat.Format = "#,##0.000";
                        excelWorksheet.Cells[cellRow, 5].Value = totalQty;
                        Double totalAmt = Math.Round((tblTallyStockTransferRptTOList.Sum(ele => ele.Amount)), 3);
                        totalAmt = totalAmt / conversionFact;
                        excelWorksheet.Cells[cellRow, 7].Style.Numberformat.Format = "#,##0.000";
                        excelWorksheet.Cells[cellRow, 7].Value = totalAmt;
                        Double totalQtyD = Math.Round((tblTallyStockTransferRptTOList.Sum(ele => ele.QtyD)), 3);
                        excelWorksheet.Cells[cellRow, 10].Style.Numberformat.Format = "#,##0.000";
                        excelWorksheet.Cells[cellRow, 10].Value = totalQtyD;
                        Double totalAmtD = Math.Round((tblTallyStockTransferRptTOList.Sum(ele => ele.AmountD)), 3);
                        totalAmtD = totalAmtD / conversionFact;
                        excelWorksheet.Cells[cellRow, 12].Style.Numberformat.Format = "#,##0.000";
                        excelWorksheet.Cells[cellRow, 12].Value = totalAmtD;

                        excelWorksheet.Cells[cellRow, 1, cellRow, 13].Style.Font.Bold = true;
                    }


                    using (ExcelRange range = excelWorksheet.Cells[1, 1, cellRow, 13])
                    {
                        range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                        range.Style.Font.Name = "Times New Roman";
                        range.Style.Font.Size = 10;
                        range.AutoFitColumns();
                    }
                }

                excelWorksheet.Protection.IsProtected = true;
                excelPackage.Workbook.Protection.LockStructure = true;

                #endregion

                #region Upload File to Azure

                // Create azure storage  account connection.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Constants.AzureConnectionStr);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a target container.
                CloudBlobContainer container = blobClient.GetContainerReference(Constants.AzureSourceContainerName);

                String fileName = Constants.ExlFlnmTallyStockTransfer + Constants.ServerDateTime.ToString("ddMMyyyyHHmmss") + "-R" + ".xlsx";
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

                var fileStream = excelPackage.GetAsByteArray();

                Task t1 = blockBlob.UploadFromByteArrayAsync(fileStream, 0, fileStream.Length);

                excelPackage.Dispose();
                #endregion

                resultMessage.DefaultSuccessBehaviour();
                return 1;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "CreateTempTallyStockTransferExcel");
                return -1;
            }

        }

        public static ResultMessage SelectAndExportToExcelCCStockTransferReport(DateTime toDate)
        {
            ResultMessage resultMessage = new ResultMessage();
            DateTime fromDate = new DateTime();
            try
            {
                List<TblReportsBackupDtlsTO> reportBackUpList = TblReportsBackupDtlsBL.SelectReportBackupDateDtls(Constants.DeliverReportNameE.CC_STOCK_TRANSFER_REPORT.ToString());
                fromDate = toDate.AddHours(-24);
                
                if (reportBackUpList != null && reportBackUpList.Count > 0)
                {
                    TblReportsBackupDtlsTO tblReportsBackupDtlsTO = reportBackUpList[0];
                    fromDate = tblReportsBackupDtlsTO.BackupDate;
                }

                List<TblCCStockTransferRptTO> tblCCStockTransferRptTOList = TblInvoiceDAO.SelectCCStockTransferDetails(fromDate,toDate);
                if (tblCCStockTransferRptTOList == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.DefaultBehaviour("Record not found!! tblCCStockTransferRptTOList is null. ");
                    return resultMessage;
                }
                TblReportsBackupDtlsTO reportBackUpTO = new TblReportsBackupDtlsTO();
                Int32 result = 0;
                if (tblCCStockTransferRptTOList.Count > 0)
                {
                    //crete file and uplod res
                    result = CreateTempCCStockTransferExcel(tblCCStockTransferRptTOList);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour();
                        resultMessage.Text = "Error while creating excel file.";
                        return resultMessage;
                    }
                    reportBackUpTO.IsBackUp = 1;
                }
                else
                {
                    reportBackUpTO.IsBackUp = 0;
                }

                reportBackUpTO.ReportName = Constants.DeliverReportNameE.CC_STOCK_TRANSFER_REPORT.ToString();
                reportBackUpTO.BackupDate = toDate;
                
                result = TblReportsBackupDtlsBL.InsertTblReportsBackupDtls(reportBackUpTO);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "Error while InsertTblReportsBackupDtls(reportBackUpTO).";
                    return resultMessage;
                }


                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in SelectAndExportToExcelCCStockTransferReport()");
                return resultMessage;
            }
        }
        public static int CreateTempCCStockTransferExcel(List<TblCCStockTransferRptTO> tblCCStockTransferRptTOList)
        {
            ResultMessage resultMessage = new ResultMessage();
            ExcelPackage excelPackage = new ExcelPackage();
            int cellRow = 2;
            try
            {

                if (tblCCStockTransferRptTOList == null || tblCCStockTransferRptTOList.Count == 0)
                {
                    return 0;
                }

                #region Create Excel File
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add(Constants.ExcelSheetName);

                excelWorksheet.Cells[1, 1].Value = "Sr No";
                excelWorksheet.Cells[1, 2].Value = "Date";
                excelWorksheet.Cells[1, 3].Value = "Time";
                excelWorksheet.Cells[1, 4].Value = "Vehicle NO";
                excelWorksheet.Cells[1, 5].Value = "From C & F";
                excelWorksheet.Cells[1, 6].Value = "To From Delear";
                excelWorksheet.Cells[1, 7].Value = "Loading Net Weight";
                excelWorksheet.Cells[1, 8].Value = " Item Name";

                excelWorksheet.Cells[1, 1, 1, 8].Style.Font.Bold = true;

                for (int i = 0; i < tblCCStockTransferRptTOList.Count; i++)
                {

                    excelWorksheet.Cells[cellRow, 1].Value = tblCCStockTransferRptTOList[i].SrNo;
                    excelWorksheet.Cells[cellRow, 2].Value = tblCCStockTransferRptTOList[i].Date;
                    excelWorksheet.Cells[cellRow, 3].Value = tblCCStockTransferRptTOList[i].Time;
                    excelWorksheet.Cells[cellRow, 4].Value = tblCCStockTransferRptTOList[i].VehicleNo;
                    excelWorksheet.Cells[cellRow, 5].Value = tblCCStockTransferRptTOList[i].FromCAndF;
                    excelWorksheet.Cells[cellRow, 6].Value = tblCCStockTransferRptTOList[i].ToFromDealer;
                    excelWorksheet.Cells[cellRow, 7].Style.Numberformat.Format = "#,##0.000";
                    excelWorksheet.Cells[cellRow, 7].Value = tblCCStockTransferRptTOList[i].LoadingNetWeight;
                    excelWorksheet.Cells[cellRow, 8].Value = tblCCStockTransferRptTOList[i].ItemName;

                    cellRow++;

                    if (i == (tblCCStockTransferRptTOList.Count - 1))
                    {
                        excelWorksheet.Cells[cellRow, 4].Value = "Total";
                        Double totalQty = Math.Round((tblCCStockTransferRptTOList.Sum(ele => ele.LoadingNetWeight)), 3);
                        excelWorksheet.Cells[cellRow, 7].Style.Numberformat.Format = "#,##0.000";
                        excelWorksheet.Cells[cellRow, 7].Value = totalQty;
                        excelWorksheet.Cells[cellRow, 1, cellRow, 8].Style.Font.Bold = true;
                    }


                    using (ExcelRange range = excelWorksheet.Cells[1, 1, cellRow, 8])
                    {
                        range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                        range.Style.Font.Name = "Times New Roman";
                        range.Style.Font.Size = 10;
                        range.AutoFitColumns();
                    }
                }

                excelWorksheet.Protection.IsProtected = true;
                excelPackage.Workbook.Protection.LockStructure = true;

                #endregion

                #region Upload File to Azure

                // Create azure storage  account connection.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Constants.AzureConnectionStr);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a target container.
                CloudBlobContainer container = blobClient.GetContainerReference(Constants.AzureSourceContainerName);

                String fileName = Constants.ExlFlnmCCStockTransfer + Constants.ServerDateTime.ToString("ddMMyyyyHHmmss") + "-R" + ".xlsx";
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

                var fileStream = excelPackage.GetAsByteArray();

                Task t1 = blockBlob.UploadFromByteArrayAsync(fileStream, 0, fileStream.Length);

                excelPackage.Dispose();
                #endregion

                resultMessage.DefaultSuccessBehaviour();
                return 1;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "CreateTempCCStockTransferExcel");
                return -1;
            }

        }

        public static ResultMessage SelectAndExportToExcelSalesChartEnquiryReport(DateTime toDate,int isConfirmed)
        {
            ResultMessage resultMessage = new ResultMessage();
            DateTime fromDate = new DateTime();
            String strOrgTempId = String.Empty;
            try
            {
                List<TblReportsBackupDtlsTO> reportBackUpList = TblReportsBackupDtlsBL.SelectReportBackupDateDtls(Constants.DeliverReportNameE.TALLY_SALES_CHART_ENQUIRY_REPORT.ToString());
                fromDate = toDate.AddHours(-120);
                
                if (reportBackUpList != null && reportBackUpList.Count > 0)
                {
                    TblReportsBackupDtlsTO tblReportsBackupDtlsTO = reportBackUpList[0];
                    fromDate = tblReportsBackupDtlsTO.BackupDate;
                }

                TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID);
                if (tblConfigParamsTO != null)
                {
                    strOrgTempId = Convert.ToString(tblConfigParamsTO.ConfigParamVal) + ",0";
                }

                List<TblInvoiceRptTO> tblInvoiceRptDrpbxTOList = TblInvoiceDAO.SelectAllRptInvoiceDetailsForDropbox(fromDate,toDate,isConfirmed, strOrgTempId);
                if (tblInvoiceRptDrpbxTOList == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.DefaultBehaviour("Record not found!! tblInvoiceRptDrpbxTOList is null. ");
                    return resultMessage;
                }
                TblReportsBackupDtlsTO reportBackUpTO = new TblReportsBackupDtlsTO();
                Int32 result = 0;
                if (tblInvoiceRptDrpbxTOList.Count > 0)
                {

                    #region All Data
                    String fileName = Constants.ExlFlnmSCE + Constants.ServerDateTime.ToString("ddMMyyyyHHmmss");
                    //crete file and uplod res
                    result = CreateTempSalesChartEnquiryExcel(tblInvoiceRptDrpbxTOList, fileName);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour();
                        resultMessage.Text = "Error while creating excel file.";
                        return resultMessage;
                    }
                    reportBackUpTO.IsBackUp = 1;

                    #endregion

                    #region Bars

                    var tblInvoiceRptDrpbxTOListBars = tblInvoiceRptDrpbxTOList.Where(w => w.InternalCnf == 0).ToList();
                    if (tblInvoiceRptDrpbxTOListBars.Count > 0)
                    {
                        fileName = Constants.ExlFlnmSCEBars + Constants.ServerDateTime.ToString("ddMMyyyyHHmmss");
                        //crete file and uplod res
                        result = CreateTempSalesChartEnquiryExcel(tblInvoiceRptDrpbxTOListBars, fileName);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour();
                            resultMessage.Text = "Error while creating excel file.";
                            return resultMessage;
                        }
                    }
                    #endregion

                    #region Internal Data

                    var tblInvoiceRptDrpbxTOListInternal = tblInvoiceRptDrpbxTOList.Where(w => w.InternalCnf == 1).ToList();
                    if (tblInvoiceRptDrpbxTOListInternal.Count > 0)
                    {
                        fileName = Constants.ExlFlnmSCEInternal + Constants.ServerDateTime.ToString("ddMMyyyyHHmmss");
                        //crete file and uplod res
                        result = CreateTempSalesChartEnquiryExcel(tblInvoiceRptDrpbxTOListInternal, fileName);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour();
                            resultMessage.Text = "Error while creating excel file.";
                            return resultMessage;
                        }
                    }
                    #endregion

                }
                else
                {
                    reportBackUpTO.IsBackUp = 0;
                }

                reportBackUpTO.ReportName = Constants.DeliverReportNameE.TALLY_SALES_CHART_ENQUIRY_REPORT.ToString();
                reportBackUpTO.BackupDate = toDate;
                
                result = TblReportsBackupDtlsBL.InsertTblReportsBackupDtls(reportBackUpTO);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "Error while InsertTblReportsBackupDtls(reportBackUpTO).";
                    return resultMessage;
                }


                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in SelectAndExportToExcelSalesChartEnquiryReport()");
                return resultMessage;
            }
        }

        public static int CreateTempSalesChartEnquiryExcel(List<TblInvoiceRptTO> tblInvoiceRptDrpbxTOList , String inputFileName)
        {
            ResultMessage resultMessage = new ResultMessage();
            ExcelPackage excelPackage = new ExcelPackage();
            int cellRow = 2;
            Int16 mathroundFact = 2;
            Double conversionFact = 1000;
            try
            {

                if (tblInvoiceRptDrpbxTOList == null || tblInvoiceRptDrpbxTOList.Count == 0)
                {
                    return 0;
                }
                List<int> prodItemIdList = new List<int>();
                List<TblInvoiceRptTO> tblInvoiceRptTOList = new List<TblInvoiceRptTO>();
                List<TblInvoiceRptTO> tblInvoiceRptFinalTOList = new List<TblInvoiceRptTO>();
                prodItemIdList = tblInvoiceRptDrpbxTOList.Select(a => a.InvoiceItemId).Distinct().ToList();
               
                for (int i = 0; i < prodItemIdList.Count; i++)
                {
                    var rList = tblInvoiceRptDrpbxTOList.Where(a => a.InvoiceItemId == prodItemIdList[i]);
                    TblInvoiceRptTO tblInvoiceRptTO = new TblInvoiceRptTO();
                    foreach (var element in rList)
                    {
                        tblInvoiceRptTO.IdInvoice = element.IdInvoice;
                        tblInvoiceRptTO.StatusDate = element.StatusDate;
                        tblInvoiceRptTO.CreatedOn = element.CreatedOn;
                        tblInvoiceRptTO.RptStatusDate = element.statusDateStr;
                        tblInvoiceRptTO.RptInvoiceNoDate = element.InvoiceNoWrtDate;
                        tblInvoiceRptTO.VehicleNo = element.VehicleNo;
                        tblInvoiceRptTO.PartyName = element.PartyName;
                        tblInvoiceRptTO.CnfName = element.CnfName;
                        tblInvoiceRptTO.BookingRate = element.BookingRate;
                        tblInvoiceRptTO.ProdItemDesc = element.ProdItemDesc;
                        tblInvoiceRptTO.InvoiceItemId = element.InvoiceItemId;
                        tblInvoiceRptTO.Bundles = element.Bundles;
                        tblInvoiceRptTO.Rate = element.Rate;
                        tblInvoiceRptTO.CdStructure = element.CdStructure;
                        tblInvoiceRptTO.InvoiceQty = element.InvoiceQty;
                        tblInvoiceRptTO.TaxableAmt = element.TaxableAmt;
                        tblInvoiceRptTO.GrandTotal = element.GrandTotal;
                        tblInvoiceRptTO.CdAmt = element.CdAmt;
                        //tblInvoiceRptTO.FreightAmt = element.GrandTotal;
                        tblInvoiceRptTO.Godown = element.Godown;

                        tblInvoiceRptTO.ItemName = element.ItemName;
                        tblInvoiceRptTO.MaterialName = element.MaterialName;

                        if (element.TaxTypeId == (int)Constants.TaxTypeE.CGST)
                        {
                            tblInvoiceRptTO.CgstTaxAmt = element.TaxAmt;
                            tblInvoiceRptTO.CgstPct = element.TaxRatePct;
                        }
                        else if (element.TaxTypeId == (int)Constants.TaxTypeE.SGST)
                        {
                            tblInvoiceRptTO.SgstTaxAmt = element.TaxAmt;
                            tblInvoiceRptTO.SgstPct = element.TaxRatePct;
                        }
                        else if (element.TaxTypeId == (int)Constants.TaxTypeE.IGST)
                        {
                            tblInvoiceRptTO.IgstTaxAmt = element.TaxAmt;
                            tblInvoiceRptTO.IgstPct = element.TaxRatePct;
                        }
                    }
                    tblInvoiceRptTOList.Add(tblInvoiceRptTO);
                }

                /*
                var summuryGroupList = tblInvoiceRptTOList.ToLookup(p => p.IdInvoice).ToList();
                if (summuryGroupList != null)
                {
                    for (int i = 0; i < summuryGroupList.Count; i++)
                    {
                        TblInvoiceRptTO tblInvoiceRptTotalTO = new TblInvoiceRptTO();
                        tblInvoiceRptTotalTO.PartyName = "Total";
                        tblInvoiceRptTotalTO.IdInvoice = summuryGroupList[i].FirstOrDefault().IdInvoice;
                        tblInvoiceRptTotalTO.StatusDate = summuryGroupList[i].FirstOrDefault().StatusDate;
                        tblInvoiceRptTotalTO.CreatedOn = summuryGroupList[i].FirstOrDefault().CreatedOn;
                        tblInvoiceRptTotalTO.CgstPct = summuryGroupList[i].FirstOrDefault().CgstPct;
                        tblInvoiceRptTotalTO.SgstPct = summuryGroupList[i].FirstOrDefault().SgstPct;
                        tblInvoiceRptTotalTO.IgstPct = summuryGroupList[i].FirstOrDefault().IgstPct;
                        tblInvoiceRptTotalTO.InvoiceQty = summuryGroupList[i].Sum(w => w.InvoiceQty);
                        tblInvoiceRptTotalTO.TaxableAmt = summuryGroupList[i].Sum(w => w.TaxableAmt);
                        tblInvoiceRptTotalTO.CgstTaxAmt = summuryGroupList[i].Sum(w => w.CgstTaxAmt);
                        tblInvoiceRptTotalTO.SgstTaxAmt = summuryGroupList[i].Sum(w => w.SgstTaxAmt);
                        tblInvoiceRptTotalTO.IgstTaxAmt = summuryGroupList[i].Sum(w => w.IgstTaxAmt);
                        tblInvoiceRptTotalTO.GrandTotal = summuryGroupList[i].Sum(w => w.GrandTotal);
                        tblInvoiceRptTotalTO.CdAmt = summuryGroupList[i].Sum(w => w.CdAmt);
                        //tblInvoiceRptTotalTO.FreightAmt = Math.Round(summuryGroupList[i].Sum(w => w.GrandTotal), 2);
                        var gruopList = summuryGroupList[i].ToList();
                        gruopList.Add(tblInvoiceRptTotalTO);
                        tblInvoiceRptFinalTOList.AddRange(gruopList);
                    }
                }*/


                if (tblInvoiceRptTOList.Count > 0 && tblInvoiceRptTOList != null)
                {
                    TblInvoiceRptTO tblInvoiceRptGrandTotalTO = new TblInvoiceRptTO();
                    tblInvoiceRptGrandTotalTO.PartyName = "Grand Total";
                    //tblInvoiceRptGrandTotalTO.IdInvoice = summuryGroupList[i].FirstOrDefault().IdInvoice;
                    //tblInvoiceRptGrandTotalTO.StatusDate = summuryGroupList[i].FirstOrDefault().StatusDate;
                    //tblInvoiceRptGrandTotalTO.CreatedOn = summuryGroupList[i].FirstOrDefault().CreatedOn;
                    tblInvoiceRptGrandTotalTO.InvoiceQty = tblInvoiceRptTOList.Sum(w => w.InvoiceQty);
                    tblInvoiceRptGrandTotalTO.TaxableAmt = tblInvoiceRptTOList.Sum(w => w.TaxableAmt);
                    tblInvoiceRptGrandTotalTO.CgstTaxAmt = tblInvoiceRptTOList.Sum(w => w.CgstTaxAmt);
                    tblInvoiceRptGrandTotalTO.SgstTaxAmt = tblInvoiceRptTOList.Sum(w => w.SgstTaxAmt);
                    tblInvoiceRptGrandTotalTO.IgstTaxAmt = tblInvoiceRptTOList.Sum(w => w.IgstTaxAmt);
                    tblInvoiceRptGrandTotalTO.GrandTotal = tblInvoiceRptTOList.Sum(w => w.GrandTotal);
                    tblInvoiceRptGrandTotalTO.CdAmt = tblInvoiceRptTOList.Sum(w => w.CdAmt);
                    //tblInvoiceRptGrandTotalTO.FreightAmt = Math.Round(tblInvoiceRptTOList.Sum(w => w.GrandTotal), 2);
                    tblInvoiceRptFinalTOList.AddRange(tblInvoiceRptTOList);

                    //Prajakta[2021-07-20] Added to not add grand total in excel report
                    if (false)
                        tblInvoiceRptFinalTOList.Add(tblInvoiceRptGrandTotalTO);
                }
                

                #region Create Excel File
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add(Constants.ExcelSheetName);

                excelWorksheet.Cells[1, 1].Value = "Vehicle No";
                excelWorksheet.Cells[1, 2].Value = "Invoice No.";
                excelWorksheet.Cells[1, 3].Value = "Transaction Date";
                excelWorksheet.Cells[1, 4].Value = "Party Name";
                excelWorksheet.Cells[1, 5].Value = "C & F Name";
                excelWorksheet.Cells[1, 6].Value = "Enquiry Rate";
                excelWorksheet.Cells[1, 7].Value = "POLAAD QST BARS";
                excelWorksheet.Cells[1, 8].Value = "Size Bundle";
                excelWorksheet.Cells[1, 9].Value = "Size Rate";
                excelWorksheet.Cells[1, 10].Value = "CD (%)";
                excelWorksheet.Cells[1, 11].Value = "CGST (%)";
                excelWorksheet.Cells[1, 12].Value = "SGST(%)";
                excelWorksheet.Cells[1, 13].Value = "IGST(%)";
                excelWorksheet.Cells[1, 14].Value = "SIZE WEIGHT";
                excelWorksheet.Cells[1, 15].Value = "BASIC SALE VALUE";
                excelWorksheet.Cells[1, 16].Value = "CGST VALUE";
                excelWorksheet.Cells[1, 17].Value = "SGST VALUE";
                excelWorksheet.Cells[1, 18].Value = "IGST VALUE";
                excelWorksheet.Cells[1, 19].Value = "GROSS VALUE";
                excelWorksheet.Cells[1, 20].Value = "CD VALUE";
                excelWorksheet.Cells[1, 21].Value = "PARTY RECEIVABLE";
                excelWorksheet.Cells[1, 22].Value = "GODOWN";
                excelWorksheet.Cells[1, 23].Value = "Size";

                excelWorksheet.Cells[1, 1, 1, 23].Style.Font.Bold = true;

                for (int i = 0; i < tblInvoiceRptFinalTOList.Count; i++)
                {
                    excelWorksheet.Cells[cellRow, 1].Value = tblInvoiceRptFinalTOList[i].VehicleNo;
                    excelWorksheet.Cells[cellRow, 2].Value = tblInvoiceRptFinalTOList[i].RptInvoiceNoDate;
                    excelWorksheet.Cells[cellRow, 3].Value = tblInvoiceRptFinalTOList[i].RptStatusDate;
                    excelWorksheet.Cells[cellRow, 4].Value = tblInvoiceRptFinalTOList[i].PartyName;
                    excelWorksheet.Cells[cellRow, 5].Value = tblInvoiceRptFinalTOList[i].CnfName;
                    Double bookingRate = Math.Round(((Convert.ToDouble(tblInvoiceRptFinalTOList[i].BookingRate) / conversionFact)),mathroundFact);
                    excelWorksheet.Cells[cellRow, 6].Style.Numberformat.Format = "#,##0.000";
                    excelWorksheet.Cells[cellRow, 6].Value = bookingRate;
                    excelWorksheet.Cells[cellRow, 7].Value = tblInvoiceRptFinalTOList[i].ProdItemDesc;
                    excelWorksheet.Cells[cellRow, 8].Style.Numberformat.Format = "#,##0.000";
                    excelWorksheet.Cells[cellRow, 8].Value = tblInvoiceRptFinalTOList[i].Bundles;
                    Double rate = Math.Round((Convert.ToDouble(tblInvoiceRptFinalTOList[i].Rate) / conversionFact),mathroundFact);
                    excelWorksheet.Cells[cellRow, 9].Style.Numberformat.Format = "#,##0.000";
                    excelWorksheet.Cells[cellRow, 9].Value = rate;
                    excelWorksheet.Cells[cellRow, 10].Style.Numberformat.Format = "#,##0.000";
                    excelWorksheet.Cells[cellRow, 10].Value = tblInvoiceRptFinalTOList[i].CdStructure;
                    excelWorksheet.Cells[cellRow, 11].Style.Numberformat.Format = "#,##0";
                    excelWorksheet.Cells[cellRow, 11].Value = tblInvoiceRptFinalTOList[i].CgstPct;
                    excelWorksheet.Cells[cellRow, 12].Style.Numberformat.Format = "#,##0";
                    excelWorksheet.Cells[cellRow, 12].Value = tblInvoiceRptFinalTOList[i].SgstPct;
                    excelWorksheet.Cells[cellRow, 13].Style.Numberformat.Format = "#,##0";
                    excelWorksheet.Cells[cellRow, 13].Value = tblInvoiceRptFinalTOList[i].IgstPct;
                    excelWorksheet.Cells[cellRow, 14].Style.Numberformat.Format = "#,##0.000";
                    excelWorksheet.Cells[cellRow, 14].Value = tblInvoiceRptFinalTOList[i].InvoiceQty;
                    Double taxableAmt = Math.Round((Convert.ToDouble(tblInvoiceRptFinalTOList[i].TaxableAmt) / conversionFact),mathroundFact);
                    excelWorksheet.Cells[cellRow, 15].Style.Numberformat.Format = "#,##0.000";
                    excelWorksheet.Cells[cellRow, 15].Value = taxableAmt;
                    Double cgstTaxAmt = Math.Round((Convert.ToDouble(tblInvoiceRptFinalTOList[i].CgstTaxAmt) / conversionFact),mathroundFact);
                    excelWorksheet.Cells[cellRow, 16].Style.Numberformat.Format = "#,##0.000";
                    excelWorksheet.Cells[cellRow, 16].Value = cgstTaxAmt;
                    Double sgstTaxAmt = Math.Round((Convert.ToDouble(tblInvoiceRptFinalTOList[i].SgstTaxAmt) / conversionFact),mathroundFact);
                    excelWorksheet.Cells[cellRow, 17].Style.Numberformat.Format = "#,##0.000";
                    excelWorksheet.Cells[cellRow, 17].Value = sgstTaxAmt;
                    Double igstTaxAmt = Math.Round((Convert.ToDouble(tblInvoiceRptFinalTOList[i].IgstTaxAmt) / conversionFact), mathroundFact);
                    excelWorksheet.Cells[cellRow, 18].Style.Numberformat.Format = "#,##0.000";
                    excelWorksheet.Cells[cellRow, 18].Value = igstTaxAmt;
                    Double granndTotal = Math.Round((Convert.ToDouble(tblInvoiceRptFinalTOList[i].GrandTotal) / conversionFact), mathroundFact);
                    excelWorksheet.Cells[cellRow, 19].Style.Numberformat.Format = "#,##0.000";
                    excelWorksheet.Cells[cellRow, 19].Value = granndTotal;
                    Double cdAmt = Math.Round((Convert.ToDouble(tblInvoiceRptFinalTOList[i].CdAmt) / conversionFact), mathroundFact);
                    excelWorksheet.Cells[cellRow, 20].Style.Numberformat.Format = "#,##0.000";
                    excelWorksheet.Cells[cellRow, 20].Value = cdAmt;
                    excelWorksheet.Cells[cellRow, 21].Style.Numberformat.Format = "#,##0.000";
                    excelWorksheet.Cells[cellRow, 21].Value = granndTotal;
                    excelWorksheet.Cells[cellRow, 22].Value = tblInvoiceRptFinalTOList[i].Godown;
                    if (!String.IsNullOrEmpty(tblInvoiceRptFinalTOList[i].MaterialName))
                        excelWorksheet.Cells[cellRow, 23].Value = tblInvoiceRptFinalTOList[i].MaterialName;
                    else
                        excelWorksheet.Cells[cellRow, 23].Value = tblInvoiceRptFinalTOList[i].ItemName;

                    cellRow++;                    

                    using (ExcelRange range = excelWorksheet.Cells[1, 1, cellRow, 23])
                    {
                        range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                        range.Style.Font.Name = "Times New Roman";
                        range.Style.Font.Size = 10;
                        range.AutoFitColumns();
                       
                    }
                }

                excelWorksheet.Protection.IsProtected = true;
                excelPackage.Workbook.Protection.LockStructure = true;

                #endregion

                #region Upload File to Azure

                // Create azure storage  account connection.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Constants.AzureConnectionStr);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a target container.
                CloudBlobContainer container = blobClient.GetContainerReference(Constants.AzureSourceContainerName);

                //String fileName = Constants.ExlFlnmSCE + Constants.ServerDateTime.ToString("ddMMyyyyHHmmss") + "-R" + ".xlsx";
                String fileName = inputFileName + "-R" + ".xlsx";

                CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

                var fileStream = excelPackage.GetAsByteArray();

                Task t1 = blockBlob.UploadFromByteArrayAsync(fileStream, 0, fileStream.Length);

                excelPackage.Dispose();
                #endregion

                resultMessage.DefaultSuccessBehaviour();
                return 1;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "CreateTempSalesChartEnquiryExcel");
                return -1;
            }

        }

        public static ResultMessage SelectAndExportToExcelWBReport(DateTime toDate)
        {
            ResultMessage resultMessage = new ResultMessage();
            DateTime fromDate = new DateTime();
            try
            {
                List<TblReportsBackupDtlsTO> reportBackUpList = TblReportsBackupDtlsBL.SelectReportBackupDateDtls(Constants.DeliverReportNameE.WB_REPORT.ToString());
                fromDate = toDate.AddHours(-24);
                
                if (reportBackUpList != null && reportBackUpList.Count > 0)
                {
                    TblReportsBackupDtlsTO tblReportsBackupDtlsTO = reportBackUpList[0];
                    fromDate = tblReportsBackupDtlsTO.BackupDate;
                }

                List<TblWBRptTO> tblWBRptTOList = new List<TblWBRptTO>();
                List<TblWBRptTO> tblWBRptTOListForSale = new List<TblWBRptTO>();
                List<TblWBRptTO> tblWBRptTOListForUnload = new List<TblWBRptTO>();

                tblWBRptTOListForSale = TblInvoiceDAO.SelectWBForSaleReportList(fromDate,toDate);
                if (tblWBRptTOListForSale == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.DefaultBehaviour("Record not found!! tblWBRptTOListForSale is null. ");
                    return resultMessage;
                }
                if (tblWBRptTOListForSale.Count > 0)
                {
                    tblWBRptTOList.AddRange(tblWBRptTOListForSale);
                }
                tblWBRptTOListForUnload = TblInvoiceDAO.SelectWBForUnloadReportList(fromDate,toDate);
                if (tblWBRptTOListForUnload == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.DefaultBehaviour("Record not found!! tblWBRptTOListForUnload is null. ");
                    return resultMessage;
                }
                if (tblWBRptTOListForUnload.Count > 0)
                {
                    tblWBRptTOList.AddRange(tblWBRptTOListForUnload);
                }
                if (tblWBRptTOList == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.DefaultBehaviour("Record not found!! tblWBRptTOList is null. ");
                    return resultMessage;
                }
                TblReportsBackupDtlsTO reportBackUpTO = new TblReportsBackupDtlsTO();
                Int32 result = 0;
                if (tblWBRptTOList.Count > 0)
                {
                    //crete file and uplod res
                    result = CreateTempWBReportExcel(tblWBRptTOList);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour();
                        resultMessage.Text = "Error while creating excel file.";
                        return resultMessage;
                    }
                    reportBackUpTO.IsBackUp = 1;
                }
                else
                {
                    reportBackUpTO.IsBackUp = 0;
                }

                reportBackUpTO.ReportName = Constants.DeliverReportNameE.WB_REPORT.ToString();
                reportBackUpTO.BackupDate = toDate;
                
                result = TblReportsBackupDtlsBL.InsertTblReportsBackupDtls(reportBackUpTO);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "Error while InsertTblReportsBackupDtls(reportBackUpTO).";
                    return resultMessage;
                }


                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in SelectAndExportToExcelWBReport()");
                return resultMessage;
            }
        }

        public static int CreateTempWBReportExcel(List<TblWBRptTO> tblWBRptTOList)
        {
            ResultMessage resultMessage = new ResultMessage();
            ExcelPackage excelPackage = new ExcelPackage();
            int cellRow = 2;
            try
            {

                if (tblWBRptTOList == null || tblWBRptTOList.Count == 0)
                {
                    return 0;
                }

                #region Create Excel File
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add(Constants.ExcelSheetName);

                excelWorksheet.Cells[1, 1].Value = "WB ID";
                excelWorksheet.Cells[1, 2].Value = "User ID";
                excelWorksheet.Cells[1, 3].Value = "Orignal RST No";
                excelWorksheet.Cells[1, 4].Value = "Additional RST No";
                excelWorksheet.Cells[1, 5].Value = "Date";
                excelWorksheet.Cells[1, 6].Value = "Time";
                excelWorksheet.Cells[1, 7].Value = "Material Type";
                excelWorksheet.Cells[1, 8].Value = "Material Sub Type";
                excelWorksheet.Cells[1, 9].Value = "Gross Weight";
                excelWorksheet.Cells[1, 10].Value = "1st Weight";
                excelWorksheet.Cells[1, 11].Value = "2nd Weight";
                excelWorksheet.Cells[1, 12].Value = "3rd Weight";
                excelWorksheet.Cells[1, 13].Value = "4th Weight";
                excelWorksheet.Cells[1, 14].Value = "5th Weight";
                excelWorksheet.Cells[1, 15].Value = "6th Weight";
                excelWorksheet.Cells[1, 16].Value = "7th Weight";
                excelWorksheet.Cells[1, 17].Value = "Tare Weight";
                excelWorksheet.Cells[1, 18].Value = "Net Weight";
                excelWorksheet.Cells[1, 19].Value = "Load / Unload";
                excelWorksheet.Cells[1, 20].Value = "From Location";
                excelWorksheet.Cells[1, 21].Value = "To Location";
                excelWorksheet.Cells[1, 22].Value = "Transaction Type Sales,Purchase,Internal Transfer";
                excelWorksheet.Cells[1, 23].Value = "Vehicle Number";
                excelWorksheet.Cells[1, 24].Value = "Vehicle Status";
                excelWorksheet.Cells[1, 25].Value = "Bill Type";
                excelWorksheet.Cells[1, 26].Value = "Vehicle ID";

                excelWorksheet.Cells[1, 1, 1, 26].Style.Font.Bold = true;

                for (int i = 0; i < tblWBRptTOList.Count; i++)
                {

                    excelWorksheet.Cells[cellRow, 1].Value = tblWBRptTOList[i].WBID;
                    excelWorksheet.Cells[cellRow, 2].Value = tblWBRptTOList[i].UserID;
                    excelWorksheet.Cells[cellRow, 3].Value = tblWBRptTOList[i].OrignalRSTNo;
                    excelWorksheet.Cells[cellRow, 4].Value = tblWBRptTOList[i].AdditionalRSTNo;
                    excelWorksheet.Cells[cellRow, 5].Value = tblWBRptTOList[i].Date;
                    excelWorksheet.Cells[cellRow, 6].Value = tblWBRptTOList[i].Time;
                    excelWorksheet.Cells[cellRow, 7].Value = tblWBRptTOList[i].MaterialType;
                    excelWorksheet.Cells[cellRow, 8].Value = tblWBRptTOList[i].MaterialSubType;
                    excelWorksheet.Cells[cellRow, 9].Value = tblWBRptTOList[i].GrossWeight;
                    excelWorksheet.Cells[cellRow, 10].Value = tblWBRptTOList[i].FirstWeight;
                    excelWorksheet.Cells[cellRow, 11].Value = tblWBRptTOList[i].SecondWeight;
                    excelWorksheet.Cells[cellRow, 12].Value = tblWBRptTOList[i].ThirdWeight;
                    excelWorksheet.Cells[cellRow, 13].Value = tblWBRptTOList[i].ForthWeight;
                    excelWorksheet.Cells[cellRow, 14].Value = tblWBRptTOList[i].FifthWeight;
                    excelWorksheet.Cells[cellRow, 15].Value = tblWBRptTOList[i].SixthWeight;
                    excelWorksheet.Cells[cellRow, 16].Value = tblWBRptTOList[i].SeventhWeight;
                    excelWorksheet.Cells[cellRow, 17].Value = tblWBRptTOList[i].TareWeight;
                    excelWorksheet.Cells[cellRow, 18].Value = tblWBRptTOList[i].NetWeight;
                    excelWorksheet.Cells[cellRow, 19].Value = tblWBRptTOList[i].LoadOrUnload;
                    excelWorksheet.Cells[cellRow, 20].Value = tblWBRptTOList[i].FromLocation;
                    excelWorksheet.Cells[cellRow, 21].Value = tblWBRptTOList[i].ToLocation;
                    excelWorksheet.Cells[cellRow, 22].Value = tblWBRptTOList[i].TransactionType;
                    excelWorksheet.Cells[cellRow, 23].Value = tblWBRptTOList[i].VehicleNumber;
                    excelWorksheet.Cells[cellRow, 24].Value = tblWBRptTOList[i].VehicleStatus;
                    excelWorksheet.Cells[cellRow, 25].Value = tblWBRptTOList[i].BillType;
                    excelWorksheet.Cells[cellRow, 26].Value = tblWBRptTOList[i].VehicleID;

                    cellRow++;

                    if (i == (tblWBRptTOList.Count - 1))
                    {
                        excelWorksheet.Cells[cellRow, 8].Value = "Total";
                        excelWorksheet.Cells[cellRow, 9].Value = Math.Round((tblWBRptTOList.Sum(ele => ele.GrossWeight)), 2);
                        excelWorksheet.Cells[cellRow, 18].Value = Math.Round((tblWBRptTOList.Sum(ele => ele.NetWeight)), 2);
                        excelWorksheet.Cells[cellRow, 1, cellRow, 26].Style.Font.Bold = true;
                    }


                    using (ExcelRange range = excelWorksheet.Cells[1, 1, cellRow, 26])
                    {
                        range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                        range.Style.Font.Name = "Times New Roman";
                        range.Style.Font.Size = 10;
                        range.AutoFitColumns();
                    }
                }

                excelWorksheet.Protection.IsProtected = true;
                excelPackage.Workbook.Protection.LockStructure = true;

                #endregion

                #region Upload File to Azure

                // Create azure storage  account connection.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Constants.AzureConnectionStr);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a target container.
                CloudBlobContainer container = blobClient.GetContainerReference(Constants.AzureSourceContainerName);

                String fileName = Constants.ExlFlnmWBReport + Constants.ServerDateTime.ToString("ddMMyyyyHHmmss") + "-R" + ".xlsx";
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

                var fileStream = excelPackage.GetAsByteArray();

                Task t1 = blockBlob.UploadFromByteArrayAsync(fileStream, 0, fileStream.Length);

                excelPackage.Dispose();
                #endregion

                resultMessage.DefaultSuccessBehaviour();
                return 1;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "CreateTempWBReportExcel");
                return -1;
            }

        }
        //Added by minal
        #endregion


        #region Reports
        /// <summary>
        /// Priyanka [26-03-2018] : Added to get the other item Invoice Tax report.
        /// </summary>
        /// <param name="frmDt"></param>
        /// <param name="toDt"></param>
        /// <param name="isConfirm"></param>
        /// <param name="otherTaxId"></param>
        /// <returns></returns>
        public static List<TblOtherTaxRpt> SelectOtherTaxDetailsReport(DateTime frmDt, DateTime toDt, int isConfirm, Int32 otherTaxId,string strOrgTempId)
        {
            return TblInvoiceDAO.SelectOtherTaxDetailsReport(frmDt, toDt, isConfirm, otherTaxId, strOrgTempId);
        }

        #endregion


        #region Split Booking
        public static ResultMessage SpiltBookingAgainstInvoice(TblInvoiceTO tblInvoiceTO, TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                Int32 result = 0;
                if (tblInvoiceTO == null)
                {
                    throw new Exception("InvoiceTO is NULL");
                }

                //if (tblInvoiceTO.IsConfirmed != 0)
                //{
                //    resultMessage.DefaultSuccessBehaviour();
                //    resultMessage.DisplayMessage = "Invoice No - " + tblInvoiceTO.IdInvoice + " Is Confirm";
                //    resultMessage.Text = resultMessage.DisplayMessage;
                //    return resultMessage;
                //}

                if (tblInvoiceTO.LoadingSlipId <= 0)
                {
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }

                //Check if every invoice against loading is confirm
                List<TblInvoiceTO> loadingInvoiceList = TblInvoiceBL.SelectInvoiceListFromLoadingSlipId(tblInvoiceTO.LoadingSlipId, conn, tran);
                if (loadingInvoiceList == null || loadingInvoiceList.Count == 0)
                {
                    throw new Exception("Invoice not found against loading slip Id - " + tblInvoiceTO.LoadingSlipId);
                }
                for (int i = 0; i < loadingInvoiceList.Count; i++)
                {
                    if (loadingInvoiceList[i].InvoiceStatusE != InvoiceStatusE.AUTHORIZED)
                    {
                        resultMessage.DefaultSuccessBehaviour();
                        return resultMessage;
                    }
                }

                //if (tblLoadingTO.LoadingSlipList == null || tblLoadingTO.LoadingSlipList.Count == 0)
                //{
                //    throw new Exception("LoadingSlipList is null for LoadingSlipId");
                //}
                //TblLoadingSlipTO tblLoadingSlipTO = tblLoadingTO.LoadingSlipList.Where(w => w.IdLoadingSlip == tblInvoiceTO.LoadingSlipId).FirstOrDefault();

                TblLoadingSlipTO tblLoadingSlipTO = TblLoadingSlipDAO.SelectTblLoadingSlip(tblInvoiceTO.LoadingSlipId, conn, tran);
                if (tblLoadingSlipTO == null)
                {
                    throw new Exception("LoadingSlipList is null for LoadingSlipId");
                }
                tblLoadingSlipTO.TblLoadingSlipDtlTO = BL.TblLoadingSlipDtlBL.SelectLoadingSlipDtlTO(tblLoadingSlipTO.IdLoadingSlip, conn, tran);
                tblLoadingSlipTO.LoadingSlipExtTOList = BL.TblLoadingSlipExtBL.SelectAllTblLoadingSlipExtList(tblLoadingSlipTO.IdLoadingSlip, conn, tran);
                //tblLoadingSlipTO.DeliveryAddressTOList = BL.TblLoadingSlipAddressBL.SelectAllTblLoadingSlipAddressList(tblLoadingSlipTO.IdLoadingSlip, conn, tran);



                //TblLoadingSlipBL.SelectAllLoadingSlipWithDetails(tblInvoiceTO.LoadingSlipId, conn, tran);
                if (tblLoadingSlipTO == null)
                {
                    throw new Exception("tblLoadingSlipTO is null for LoadingSlipId - " + tblInvoiceTO.LoadingSlipId);
                }


                if (tblLoadingTO.LoadingTypeE == LoadingTypeE.OTHER)
                {
                    if (tblLoadingSlipTO.TblLoadingSlipDtlTO == null)
                    {
                        resultMessage.DefaultSuccessBehaviour();
                        return resultMessage;
                    }
                }

                if (tblLoadingSlipTO.TblLoadingSlipDtlTO == null)
                {
                    throw new Exception("TblLoadingSlipDtlTO is null for LoadingSlipId - " + tblInvoiceTO.LoadingSlipId);
                }

                Int32 currentBookingId = tblLoadingSlipTO.TblLoadingSlipDtlTO.BookingId;

                if (currentBookingId == 0)
                {
                    throw new Exception("Booking Id is zero for LoadingSlipId - " + tblInvoiceTO.LoadingSlipId);
                }

                TblBookingsTO tblBookingsTO = TblBookingsBL.SelectBookingsTOWithDetails(currentBookingId, conn, tran);
                if (tblBookingsTO == null)
                {
                    throw new Exception("tblBookingsTO == null for bookingId" + currentBookingId);
                }

                if (tblInvoiceTO.IsConfirmed == tblBookingsTO.IsConfirmed)
                {
                    resultMessage.DefaultSuccessBehaviour();
                    resultMessage.DisplayMessage = "Booking & Invoice are - " + tblInvoiceTO.IsConfirmed;
                    return resultMessage;
                }

                //if (tblBookingsTO.IsConfirmed == 0)
                //{
                //    resultMessage.DefaultSuccessBehaviour();
                //    resultMessage.DisplayMessage = "Booking is already tentative";
                //    return resultMessage;
                //}

                Double splitQty = tblLoadingSlipTO.TblLoadingSlipDtlTO.LoadingQty;
                #region Update Current Booking

                if (splitQty == tblBookingsTO.BookingQty)
                {
                    if (tblBookingsTO.IsConfirmed == 0)
                        tblBookingsTO.IsConfirmed = 1;
                    else
                        tblBookingsTO.IsConfirmed = 0;


                    result = TblBookingsBL.UpdateTblBookings(tblBookingsTO, conn, tran);
                    if (result != 1)
                    {
                        throw new Exception("Error while updating tblBookingsTO for bookingId - " + tblBookingsTO.IdBooking);
                    }

                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;

                }
                else
                {
                    tblBookingsTO.BookingQty -= splitQty;
                }
                result = TblBookingsBL.UpdateTblBookings(tblBookingsTO, conn, tran);
                if (result != 1)
                {
                    throw new Exception("Error while updating tblBookingsTO for bookingId - " + tblBookingsTO.IdBooking);
                }


                //Prajakta [2020-05-03] if the booking ext list is empty then get it from database
                if (tblBookingsTO.OrderDetailsLst == null || tblBookingsTO.OrderDetailsLst.Count == 0)
                    tblBookingsTO.OrderDetailsLst = TblBookingExtDAO.SelectAllTblBookingExt(tblBookingsTO.IdBooking, conn, tran);

                if (tblBookingsTO.BookingScheduleTOLst == null || tblBookingsTO.BookingScheduleTOLst.Count == 0)
                {
                    tblBookingsTO.BookingScheduleTOLst = TblBookingScheduleDAO.SelectAllTblBookingScheduleList(tblBookingsTO.IdBooking, conn, tran);
                }

                if (tblBookingsTO.OrderDetailsLst != null && tblBookingsTO.OrderDetailsLst.Count > 0)
                {

                    Double previousBookingItemQty = tblBookingsTO.OrderDetailsLst.Sum(s => s.BookedQty);

                    Double diffQty = previousBookingItemQty - tblBookingsTO.BookingQty;
                    if (diffQty > 0)
                    {
                        //Delete Item which are in loading slip.
                        for (int i = 0; i < tblLoadingSlipTO.LoadingSlipExtTOList.Count; i++)
                        {
                            if (diffQty <= 0)
                            {
                                break;
                            }

                            TblLoadingSlipExtTO tblLoadingSlipExtTO = tblLoadingSlipTO.LoadingSlipExtTOList[i];

                            Double adjustedQty = tblLoadingSlipExtTO.LoadingQty;
                            Double adjustedBookQty = tblLoadingSlipExtTO.LoadingQty;

                            List<TblBookingExtTO> tblBookingExtTOList = tblBookingsTO.OrderDetailsLst.Where(w => w.ProdCatId == tblLoadingSlipExtTO.ProdCatId &&
                                                            w.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId && w.MaterialId == tblLoadingSlipExtTO.MaterialId &&
                                                            w.ProdItemId == tblLoadingSlipExtTO.ProdItemId).ToList();

                            if (tblBookingExtTOList != null && tblBookingExtTOList.Count > 0)
                            {
                                for (int j = 0; j < tblBookingExtTOList.Count; j++)
                                {

                                    if (adjustedQty <= 0 || diffQty <= 0)
                                    {
                                        break;
                                    }

                                    TblBookingExtTO tblBookingExtTO = tblBookingExtTOList[j];
                                    if (tblBookingExtTO.BookedQty >= adjustedQty)
                                    {
                                        diffQty -= adjustedQty;

                                        tblBookingExtTO.BookedQty -= adjustedQty;

                                        //Prajakta [2020-05-04] Here upadte the schedule qty
                                        UpdateBookingQty(ref adjustedBookQty, tblBookingsTO, tblBookingExtTO);

                                        adjustedQty = 0;
                                    }
                                    else
                                    {
                                        diffQty -= tblBookingExtTO.BookedQty;

                                        adjustedQty -= tblBookingExtTO.BookedQty;
                                        tblBookingExtTO.BookedQty = 0;
                                        //Prajakta [2020-05-04] Here upadte the schedule qty
                                        UpdateBookingQty(ref adjustedBookQty, tblBookingsTO, tblBookingExtTO);
                                    }

                                    result = TblBookingExtBL.UpdateTblBookingExt(tblBookingExtTO, conn, tran);
                                    if (result != 1)
                                    {
                                        tran.Rollback();
                                        resultMessage.Text = "Sorry..Record Could not be saved. Error While InsertTblBookingExt in Function SaveNewBooking";
                                        resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                                        resultMessage.Result = 0;
                                        resultMessage.MessageType = ResultMessageE.Error;
                                        return resultMessage;
                                    }

                                }

                                //
                                if (tblBookingsTO.BookingScheduleTOLst != null && tblBookingsTO.BookingScheduleTOLst.Count > 0)
                                {
                                    for (int n = 0; n < tblBookingsTO.BookingScheduleTOLst.Count; n++)
                                    {
                                        if (tblBookingsTO.BookingScheduleTOLst[n].IsUpdated)
                                        {

                                            tblBookingsTO.BookingScheduleTOLst[n].Qty = Math.Round(tblBookingsTO.BookingScheduleTOLst[n].Qty, 3);

                                            tblBookingsTO.BookingScheduleTOLst[n].IsUpdated = false;
                                            result = TblBookingScheduleDAO.UpdateTblBookingSchedulePendingQty(tblBookingsTO.BookingScheduleTOLst[n], conn, tran);
                                            if (result != 1)
                                            {
                                                tran.Rollback();
                                                resultMessage.Text = "Sorry..Record Could not be saved. Error While Update Booking qty in Schedule table in Function UpdateTblBookingSchedule";
                                                resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                                                resultMessage.Result = 0;
                                                resultMessage.MessageType = ResultMessageE.Error;
                                                return resultMessage;
                                            }
                                        }
                                    }
                                }

                            }

                        }
                    }
                }

                //AmolG[2020-Mar-03] Update Size Qty. This is used to show the color on UI based on Schedule Qty of booking. 
                if (tblBookingsTO.BookingScheduleTOLst != null && tblBookingsTO.BookingScheduleTOLst.Count > 0)
                {
                    double sizeQty = tblBookingsTO.BookingScheduleTOLst.Sum(s => s.Qty);
                    tblBookingsTO.SizesQty = sizeQty;
                    result = TblBookingsDAO.UpdateSizeQuantity(tblBookingsTO, conn, tran);
                    if (result != 1)
                    {
                        resultMessage.Text = "Error When Update Size Qty in Booking";
                        resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                        resultMessage.Result = 0;
                        resultMessage.MessageType = ResultMessageE.Error;
                        return resultMessage;
                    }
                }


                #endregion

                #region New Booking

                tblBookingsTO.BookingRefId = Convert.ToInt32(tblBookingsTO.BookingDisplayNo);
                tblBookingsTO.PendingQty = 0;
                tblBookingsTO.BookingQty = splitQty;
                tblBookingsTO.SizesQty = splitQty;
                tblBookingsTO.IdBooking = 0;
                List<TblBookingsTO> List = DAL.TblBookingsDAO.SelectTblBookingsRef(tblBookingsTO.BookingRefId, conn, tran);
                tblBookingsTO.BookingDisplayNo = List != null && List.Count > 0 ? tblBookingsTO.BookingDisplayNo + "/" + (Convert.ToInt32(List.Count) + Convert.ToInt32(1)) : tblBookingsTO.BookingDisplayNo + "/1";
                if (tblBookingsTO.IsConfirmed == 0)
                {
                    tblBookingsTO.IsConfirmed = 1;
                }
                else
                {
                    tblBookingsTO.IsConfirmed = 0;
                }

                result = TblBookingsBL.InsertTblBookings(tblBookingsTO, conn, tran);
                if (result != 1)
                {
                    throw new Exception("Error while inserting tblBookingsTO for refBookingId- " + tblBookingsTO.BookingRefId);
                }

                Int32 newBookingId = tblBookingsTO.IdBooking;

                //Prajakta[2020-05-03] Save new Schedule Entry for new booking
                TblBookingScheduleTO tblBookingScheduleTO = null;
                if (tblBookingsTO.BookingScheduleTOLst != null && tblBookingsTO.BookingScheduleTOLst.Count > 0)
                {
                    tblBookingScheduleTO = tblBookingsTO.BookingScheduleTOLst[0];
                    tblBookingScheduleTO.BookingId = newBookingId;
                    tblBookingScheduleTO.Qty = tblBookingsTO.BookingQty;
                }
                else
                {
                    tblBookingScheduleTO = new TblBookingScheduleTO();
                    tblBookingScheduleTO.BookingId = newBookingId;
                    tblBookingScheduleTO.ScheduleDate = tblBookingsTO.CreatedOn;
                    tblBookingScheduleTO.CreatedBy = tblBookingsTO.CreatedBy;
                    tblBookingScheduleTO.CreatedOn = tblBookingsTO.CreatedOn;
                    tblBookingScheduleTO.Qty = tblBookingsTO.BookingQty;
                }

                result = TblBookingScheduleDAO.InsertTblBookingSchedule(tblBookingScheduleTO, conn, tran);
                if (result != 1)
                {
                    throw new Exception("Error while inserting InsertTblBookingSchedule for BookingId - " + tblBookingScheduleTO.BookingId);
                }


                if (tblBookingsTO.DeliveryAddressLst != null && tblBookingsTO.DeliveryAddressLst.Count > 0)
                {
                    for (int i = 0; i < tblBookingsTO.DeliveryAddressLst.Count; i++)
                    {
                        if (string.IsNullOrEmpty(tblBookingsTO.DeliveryAddressLst[i].Country))
                            tblBookingsTO.DeliveryAddressLst[i].Country = Constants.DefaultCountry;

                        tblBookingsTO.DeliveryAddressLst[i].BookingId = newBookingId;
                        result = BL.TblBookingDelAddrBL.InsertTblBookingDelAddr(tblBookingsTO.DeliveryAddressLst[i], conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.Text = "Error While Inserting Booking Del Address in Function SaveNewBooking";
                            resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                            resultMessage.MessageType = ResultMessageE.Error;
                            return resultMessage;
                        }
                    }
                }

                if (tblLoadingSlipTO.LoadingSlipExtTOList != null && tblLoadingSlipTO.LoadingSlipExtTOList.Count > 0)
                {
                    for (int i = 0; i < tblLoadingSlipTO.LoadingSlipExtTOList.Count; i++)
                    {
                        TblBookingExtTO tblBookingExtTO = new TblBookingExtTO(tblLoadingSlipTO.LoadingSlipExtTOList[i]);
                        tblBookingExtTO.BookingId = newBookingId;
                        tblBookingExtTO.Rate = tblBookingsTO.BookingRate;
                        tblBookingExtTO.BalanceQty = tblBookingExtTO.BookedQty;
                        tblBookingExtTO.ScheduleId = tblBookingScheduleTO.IdSchedule;

                        result = BL.TblBookingExtBL.InsertTblBookingExt(tblBookingExtTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.Text = "Sorry..Record Could not be saved. Error While InsertTblBookingExt in Function SaveNewBooking";
                            resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                            resultMessage.Result = 0;
                            resultMessage.MessageType = ResultMessageE.Error;
                            return resultMessage;
                        }
                    }
                }
                if (tblLoadingSlipTO.LoadingSlipExtTOList != null && tblLoadingSlipTO.LoadingSlipExtTOList.Count > 0)
                {
                    for (int i = 0; i < tblLoadingSlipTO.LoadingSlipExtTOList.Count; i++)
                    {
                        tblLoadingSlipTO.LoadingSlipExtTOList[i].BookingId = newBookingId;

                        if (Constants.getweightSourceConfigTO() == (int)Constants.WeighingDataSourceE.IoT)
                        {
                            if (tblLoadingSlipTO.IsConfirmed == 0)
                            {
                                tblLoadingSlipTO.LoadingSlipExtTOList[i].LoadedBundles = 0;
                                tblLoadingSlipTO.LoadingSlipExtTOList[i].LoadedWeight = 0;
                                tblLoadingSlipTO.LoadingSlipExtTOList[i].CalcTareWeight = 0;
                            }
                        }

                        result = BL.TblLoadingSlipExtBL.UpdateTblLoadingSlipExt(tblLoadingSlipTO.LoadingSlipExtTOList[i], conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.Text = "Sorry..Record Could not be saved. Error While InsertTblLoadingSlipExt in Function SaveNewBooking";
                            resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                            resultMessage.Result = 0;
                            resultMessage.MessageType = ResultMessageE.Error;
                            return resultMessage;
                        }
                    }
                }
                #endregion

                tblLoadingSlipTO.TblLoadingSlipDtlTO.BookingId = newBookingId;

                result = TblLoadingSlipDtlBL.UpdateTblLoadingSlipDtl(tblLoadingSlipTO.TblLoadingSlipDtlTO, conn, tran);
                if (result != 1)
                {
                    throw new Exception("Error while updating tblLoadingSlipTO.TblLoadingSlipDtlTO for IdLoadSlipDtl - " + tblLoadingSlipTO.TblLoadingSlipDtlTO.IdLoadSlipDtl);
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
        }

        /// <summary>
        ///Prajakta [2020-05-04] Update Booking Qty
        /// </summary>
        /// <param name="adjustedBookQty"></param>
        /// <param name="tblBookingsTO"></param>
        /// <param name="tblBookingExtTO"></param>
        public static void UpdateBookingQty(ref Double adjustedBookQty, TblBookingsTO tblBookingsTO, TblBookingExtTO tblBookingExtTO)
        {
            //AmolG[2020-Feb-25] Here upadte the schedule qty
            if (tblBookingsTO.BookingScheduleTOLst != null && tblBookingsTO.BookingScheduleTOLst.Count > 0)
            {
                for (int n = 0; n < tblBookingsTO.BookingScheduleTOLst.Count; n++)
                {
                    if (tblBookingsTO.BookingScheduleTOLst[n].IdSchedule == tblBookingExtTO.ScheduleId
                        && tblBookingsTO.BookingScheduleTOLst[n].Qty >= adjustedBookQty)
                    {
                        tblBookingsTO.BookingScheduleTOLst[n].Qty -= adjustedBookQty;

                        tblBookingsTO.BookingScheduleTOLst[n].Qty = Math.Round(tblBookingsTO.BookingScheduleTOLst[n].Qty, 3);

                        tblBookingsTO.BookingScheduleTOLst[n].IsUpdated = true;
                        adjustedBookQty = 0;
                    }
                    else if (tblBookingsTO.BookingScheduleTOLst[n].IdSchedule == tblBookingExtTO.ScheduleId)
                    {
                        adjustedBookQty = tblBookingsTO.BookingScheduleTOLst[n].Qty - adjustedBookQty;
                        tblBookingsTO.BookingScheduleTOLst[n].Qty = 0;
                        tblBookingsTO.BookingScheduleTOLst[n].IsUpdated = true;
                    }
                }
            }
        }


        //public static ResultMessage SpiltBookingAgainstInvoice(TblInvoiceTO tblInvoiceTO)
        //{
        //    SqlConnection conn = new SqlConnection(Startup.ConnectionString);
        //    SqlTransaction tran = null;
        //    ResultMessage resultMessage = new ResultMessage();
        //    resultMessage.MessageType = ResultMessageE.None;
        //    resultMessage.Text = "Not Entered In The Loop";
        //    try
        //    {
        //        conn.Open();
        //        tran = conn.BeginTransaction();

        //        resultMessage = SpiltBookingAgainstInvoice(tblInvoiceTO, conn, tran);
        //        if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
        //        {
        //            return resultMessage;
        //        }

        //        tran.Commit();
        //        resultMessage.DefaultSuccessBehaviour();
        //        return resultMessage;
        //    }
        //    catch (Exception ex)
        //    {
        //        resultMessage.DefaultExceptionBehaviour(ex, "");
        //        return resultMessage;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //}


        #endregion
        
        #region eInvoice

        /// <summary>
        /// Dhananjay[18-11-2020] : Added To Generate eInvvoice.
        /// </summary>
        public static ResultMessage GenerateEInvoice(Int32 loginUserId, Int32 idInvoice, Int32 eInvoiceCreationType, bool forceToGetToken = false)
        {
            ResultMessage resultMsg = new ResultMessage();
            try
            {
                string sellerGstin = "27AALFP1139Q004";
                TblInvoiceTO tblInvoiceTO = null;
                SqlConnection conn = new SqlConnection(Startup.ConnectionString);
                SqlTransaction tran = null;
                try
                {
                    conn.Open();
                    tblInvoiceTO = new TblInvoiceTO();
                    tblInvoiceTO = SelectTblInvoiceTOWithDetails(idInvoice, conn, tran);
                }
                catch (Exception ex1)
                {
                    resultMsg.MessageType = ResultMessageE.Error;
                    resultMsg.Text = ex1.Message;
                    return resultMsg;
                }
                finally
                {
                    conn.Close();
                }
                if (tblInvoiceTO == null)
                {
                    throw new Exception("InvoiceTO is null");
                }

                List<TblOrgLicenseDtlTO> TblOrgLicenseDtlTOList = TblOrgLicenseDtlBL.SelectAllTblOrgLicenseDtlList(tblInvoiceTO.InvFromOrgId);
                if (TblOrgLicenseDtlTOList != null)
                {
                    for (int i = 0; i <= TblOrgLicenseDtlTOList.Count - 1; i++)
                    {
                        if (TblOrgLicenseDtlTOList[i].LicenseId == (Int32)CommercialLicenseE.IGST_NO)
                        {
                            sellerGstin = TblOrgLicenseDtlTOList[i].LicenseValue.ToUpper();
                            break;
                        }
                    }
                }

                string access_token_OauthToken = null;
                resultMsg = EInvoice_OauthToken(loginUserId, sellerGstin, forceToGetToken, tblInvoiceTO.InvFromOrgId);
                if (resultMsg.Result != 1)
                {
                    throw new Exception("Error in EInvoice_OauthToken");
                }

                access_token_OauthToken = resultMsg.Tag.ToString();
                if (access_token_OauthToken == null)
                {
                    throw new Exception("access_token_OauthToken is null");
                }

                string access_token_Authentication = null;
                resultMsg = EInvoice_Authentication(loginUserId, access_token_OauthToken, sellerGstin, forceToGetToken, tblInvoiceTO.InvFromOrgId);
                if (resultMsg.Result != 1)
                {
                    throw new Exception("Error in EInvoice_Authentication");
                }

                access_token_Authentication = resultMsg.Tag.ToString();
                if (access_token_Authentication != null)
                {
                    return EInvoice_Generate(tblInvoiceTO, loginUserId, access_token_Authentication, sellerGstin, eInvoiceCreationType);
                }
                return resultMsg;
            }
            catch (Exception ex)
            {
                resultMsg.MessageType = ResultMessageE.Error;
                resultMsg.Text = ex.Message;
                return resultMsg;
            }
            finally
            {

            }
        }

        /// <summary>
        /// Dhananjay[18-11-2020] : Added To Get Oauth Token for eInvvoice.
        /// </summary>
        public static ResultMessage EInvoice_OauthToken(Int32 loginUserId, string sellerGstin, bool forceToGetToken, Int32 OrgId)
        {
            string access_token_OauthToken = null;
            DateTime tokenExpiresAt = CommonDAO.SelectServerDateTime();
            ResultMessage resultMsg = new ResultMessage();

            TblEInvoiceApiTO tblEInvoiceApiTO = GetTblEInvoiceApiTO((int)EInvoiceAPIE.OAUTH_TOKEN);
            if (tblEInvoiceApiTO == null)
            {
                throw new Exception("EInvoiceApiTO is null");
            }

            if (forceToGetToken == false)
            {
                if (tblEInvoiceApiTO.SessionExpiresAt > CommonDAO.SelectServerDateTime())
                {
                    resultMsg.DefaultSuccessBehaviour();
                    resultMsg.Tag = tblEInvoiceApiTO.AccessToken;
                    return resultMsg;
                }
            }
            
            tblEInvoiceApiTO.HeaderParam = tblEInvoiceApiTO.HeaderParam.Replace("@gstin", sellerGstin);

            IRestResponse response = CallRestAPIs(tblEInvoiceApiTO.ApiBaseUri + tblEInvoiceApiTO.ApiFunctionName, tblEInvoiceApiTO.ApiMethod, tblEInvoiceApiTO.HeaderParam, tblEInvoiceApiTO.BodyParam);

            JObject json = JObject.Parse(response.Content);
            access_token_OauthToken = (string)json["access_token"];
            
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                resultMsg = InsertIntoTblEInvoiceSessionApiResponse(response, tblEInvoiceApiTO.IdApi, loginUserId, OrgId, conn, tran);
                if (resultMsg.Result != 1)
                {
                    tran.Rollback();
                    return resultMsg;
                }

                if (access_token_OauthToken == null)
                {
                    tran.Commit();
                    resultMsg.DefaultBehaviour(json.ToString());
                    resultMsg.DisplayMessage = json.ToString();
                    resultMsg.Text = resultMsg.DisplayMessage;
                    return resultMsg;
                }

                int expires_in = (int)json["expires_in"];
                tokenExpiresAt = CommonDAO.SelectServerDateTime().AddSeconds(expires_in - secsToBeDeductededFromTokenExpTime);

                resultMsg = UpdateTblEInvoiceApi(tblEInvoiceApiTO.IdApi, access_token_OauthToken, tokenExpiresAt, loginUserId, conn, tran);
                if (resultMsg.Result != 1)
                {
                    tran.Rollback();
                    return resultMsg;
                }

                tran.Commit();
                resultMsg.Tag = access_token_OauthToken;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMsg.DefaultExceptionBehaviour(ex, "EInvoice_OauthToken");
            }
            finally
            {
                conn.Close();
            }
            return resultMsg;
        }

        /// <summary>
        /// Dhananjay[18-11-2020] : Added To Get Authentication for eInvvoice.
        /// </summary>
        public static ResultMessage EInvoice_Authentication(Int32 loginUserId, string access_token_OauthToken, string sellerGstin, bool forceToGetToken, int OrgId)
        {
            ResultMessage resultMsg = new ResultMessage();
            if (access_token_OauthToken == "")
            {
                resultMsg = EInvoice_OauthToken(loginUserId, sellerGstin, forceToGetToken, OrgId);
                if (resultMsg.Result != 1)
                {
                    throw new Exception("Error in EInvoice_OauthToken");
                }
                else
                {
                    access_token_OauthToken = resultMsg.Tag.ToString();
                }
            }
            string access_Token_Authentication = null;
            DateTime tokenExpiresAt = CommonDAO.SelectServerDateTime();

            List<TblEInvoiceApiTO> tblEInvoiceApiTOList = TblEInvoiceApiDAO.SelectTblEInvoiceApi(Constants.EINVOICE_AUTHENTICATE, OrgId);
            if (tblEInvoiceApiTOList == null)
            {
                resultMsg.DefaultExceptionBehaviour(new Exception("EInvoiceApiTOList is null"), "EInvoice_Authentication");
                return resultMsg;
            }
            if (tblEInvoiceApiTOList.Count == 0)
            {
                resultMsg.DefaultExceptionBehaviour(new Exception("EInvoiceApiTOList not found"), "EInvoice_Authentication");
                return resultMsg;
            }
            if (forceToGetToken == false)
            {
                if (tblEInvoiceApiTOList[0].SessionExpiresAt > CommonDAO.SelectServerDateTime())
                {
                    resultMsg.DefaultSuccessBehaviour();
                    resultMsg.Tag = tblEInvoiceApiTOList[0].AccessToken;
                    return resultMsg;
                }
            }
            TblEInvoiceApiTO tblEInvoiceApiTO = tblEInvoiceApiTOList[0];
            tblEInvoiceApiTO.HeaderParam = tblEInvoiceApiTO.HeaderParam.Replace("@gstin", sellerGstin);
            tblEInvoiceApiTO.HeaderParam = tblEInvoiceApiTO.HeaderParam.Replace("@token", access_token_OauthToken);

            IRestResponse response = CallRestAPIs(tblEInvoiceApiTO.ApiBaseUri + tblEInvoiceApiTO.ApiFunctionName, tblEInvoiceApiTO.ApiMethod, tblEInvoiceApiTO.HeaderParam, tblEInvoiceApiTO.BodyParam);

            JObject json = JObject.Parse(response.Content);
            access_Token_Authentication = (string)json["access_token"];

            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                resultMsg = InsertIntoTblEInvoiceSessionApiResponse(response, tblEInvoiceApiTO.IdApi, loginUserId, OrgId, conn, tran);
                if (resultMsg.Result != 1)
                {
                    tran.Rollback();
                    return resultMsg;
                }

                if (access_Token_Authentication == null)
                {
                    tran.Commit();
                    resultMsg.DefaultBehaviour(json.ToString());
                    resultMsg.DisplayMessage = json.ToString();
                    resultMsg.Text = resultMsg.DisplayMessage;
                    return resultMsg;
                }

                int expires_in = (int)json["expires_in"];
                tokenExpiresAt = CommonDAO.SelectServerDateTime().AddSeconds(expires_in - secsToBeDeductededFromTokenExpTime);

                resultMsg = UpdateTblEInvoiceApi(tblEInvoiceApiTO.IdApi, access_Token_Authentication, tokenExpiresAt, loginUserId, conn, tran);
                if (resultMsg.Result != 1)
                {
                    tran.Rollback();
                    return resultMsg;
                }
                resultMsg.Tag = access_Token_Authentication;
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMsg.DefaultExceptionBehaviour(ex, "EInvoice_Authentication");
            }
            finally
            {
                conn.Close();
            }
            return resultMsg;
        }

        private static string padRight(string str, char pad = '-', int totalWidth = 3)
        {
            str = RemoveSpecialChars(str);
            return str.PadRight(totalWidth, pad);
        }

        private static string GetValidVehichleNumber(string vehicleNumber)
        {
            vehicleNumber = RemoveSpecialChars(vehicleNumber).Replace(" ", "").Replace("-", "");
            int n, nOut;
            string last4Digit = "";
            if (vehicleNumber.Length >= 4)
            {
                for (n = 4; n > 0; n--)
                {
                    last4Digit = vehicleNumber.Substring(vehicleNumber.Length - n, n);
                    if (int.TryParse(last4Digit, out nOut) == true)
                    {
                        if (n == 4) break;
                    }
                    else
                    {
                        last4Digit = last4Digit.PadLeft(4, char.Parse("0"));
                        vehicleNumber = vehicleNumber.Substring(0, vehicleNumber.Length - n) + last4Digit;
                        break;
                    }
                }
            }
            return vehicleNumber;
        }

        private static string RemoveSpecialChars(string str)
        {
            if (String.IsNullOrEmpty(str)) return "";

            str = str.Replace("\r\n", "");
            str = str.Replace("\t", "");
            str = str.Replace("\n", "");
            str = str.Trim();

            return str;
        }

        private static string GetAreaFromAddress(TblInvoiceAddressTO tblInvoiceAddressTO)
        {
            string area = "";
            if (tblInvoiceAddressTO.VillageName != null && tblInvoiceAddressTO.VillageName != "")
            {
                area += tblInvoiceAddressTO.VillageName + ", ";
            }
            else if (tblInvoiceAddressTO.Taluka != null && tblInvoiceAddressTO.Taluka != "")
            {
                area += tblInvoiceAddressTO.Taluka + ", ";
            }
            else if (tblInvoiceAddressTO.District != null && tblInvoiceAddressTO.District != "")
            {
                area += tblInvoiceAddressTO.District + ", ";
            }

            if (String.IsNullOrEmpty(area))
            {
                area = "---";
            }
            else
            {
                area = area.Trim().TrimEnd(',');
            }

            area = RemoveSpecialChars(area);
            if (area.Length < 3)
            {
                area = padRight(area);
            }
            return area;
        }

        /// <summary>
        /// Dhananjay[06-01-2021] : Added To Generate eInvvoice.
        /// </summary>
        public static TblEInvoiceApiTO GetTblEInvoiceApiTO(int idAPI)
        {
            List <TblEInvoiceApiTO> tblEInvoiceApiTOList = TblEInvoiceApiDAO.SelectAllTblEInvoiceApi(idAPI);
            if (tblEInvoiceApiTOList == null)
            {
                return null;
            }
            if (tblEInvoiceApiTOList.Count == 0)
            {
                return null;
            }
            TblEInvoiceApiTO tblEInvoiceApiTO = tblEInvoiceApiTOList[0];
            return tblEInvoiceApiTO;
        }
        /// <summary>
        /// Dhananjay[18-11-2020] : Added To Generate eInvvoice.
        /// </summary>
        public static ResultMessage EInvoice_Generate(TblInvoiceTO tblInvoiceTO, Int32 loginUserId, string access_Token_Authentication, string sellerGstin, Int32 eInvoiceCreationType)
        {
            ResultMessage resultMsg = new ResultMessage();
            try
            {
                if (access_Token_Authentication == "")
                {
                    resultMsg = EInvoice_Authentication(loginUserId, "", sellerGstin, false, tblInvoiceTO.InvFromOrgId);
                    if (resultMsg.Result != 1)
                    {
                        throw new Exception("Error in EInvoice_Authentication");
                    }
                    else
                    {
                        access_Token_Authentication = resultMsg.Tag.ToString();
                    }
                }

                TblEInvoiceApiTO tblEInvoiceApiTO = GetTblEInvoiceApiTO((int)EInvoiceAPIE.GENERATE_EINVOICE);
                if (tblEInvoiceApiTO == null)
                {
                    throw new Exception("EInvoiceApiTO is null");
                }
                /*TblEInvoiceApiTO tblEInvoiceApiTOEWayBill = GetTblEInvoiceApiTO((int)EInvoiceAPIE.GENERATE_EWAYBILL);
                if (tblEInvoiceApiTOEWayBill == null)
                {
                    throw new Exception("EInvoiceApiTO for eWayBill is null");
                }*/

                tblEInvoiceApiTO.HeaderParam = tblEInvoiceApiTO.HeaderParam.Replace("@gstin", sellerGstin);
                tblEInvoiceApiTO.HeaderParam = tblEInvoiceApiTO.HeaderParam.Replace("@token", access_Token_Authentication);

                tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@invoiceType", "INV");
                tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@invoiceNo", tblInvoiceTO.InvoiceNo); //"AUG070720-19"
                tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@invoiceDate", tblInvoiceTO.InvoiceDate.Day.ToString("00") + "/" + tblInvoiceTO.InvoiceDate.Month.ToString("00") + "/" + tblInvoiceTO.InvoiceDate.Year.ToString("0000")); //"12/08/2020"

                int invFromOrgId = tblInvoiceTO.InvFromOrgId;
                TblOrganizationTO organizationTO = TblOrganizationBL.SelectTblOrganizationTO(invFromOrgId);
                TblInvoiceAddressTO billingAddrTO = null;
                TblInvoiceAddressTO consigneeAddrTO = null;
                TblInvoiceAddressTO shippingAddrTO = null;
                TblAddressTO sellerAddressTO = null;
                if (tblInvoiceTO.InvoiceAddressTOList != null && tblInvoiceTO.InvoiceAddressTOList.Count > 0)
                {
                    for (int i = 0; i < tblInvoiceTO.InvoiceAddressTOList.Count; i++)
                    {
                        if (tblInvoiceTO.InvoiceAddressTOList[i].TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS)
                        {
                            billingAddrTO = tblInvoiceTO.InvoiceAddressTOList[i];
                        }
                        else if (tblInvoiceTO.InvoiceAddressTOList[i].TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.CONSIGNEE_ADDRESS)
                        {
                            consigneeAddrTO = tblInvoiceTO.InvoiceAddressTOList[i];
                        }
                    }
                    if (shippingAddrTO == null)
                    {
                        if (consigneeAddrTO == null)
                        {
                            shippingAddrTO = billingAddrTO; //if shipping and consignee address not available
                        }
                        else
                        {
                            shippingAddrTO = consigneeAddrTO; //if shipping address not available
                        }
                    }
                }
                string sellerName = "";
                string sellerEmailAddr = "test@einv.com";
                string sellerPhoneNo = "9100000000";
                TblOrganizationTO tblSellerOrgTO = TblOrganizationBL.SelectTblOrganizationTO(invFromOrgId);
                if (tblSellerOrgTO != null)
                {
                    sellerName = tblSellerOrgTO.FirmName;
                    if (tblSellerOrgTO.EmailAddr != null)
                    {
                        if (tblSellerOrgTO.EmailAddr.Length >= 3 && tblSellerOrgTO.EmailAddr.Length <= 100)
                        {
                            sellerEmailAddr = tblSellerOrgTO.EmailAddr;
                        }
                    }
                    if (tblSellerOrgTO.PhoneNo != null)
                    {
                        if (tblSellerOrgTO.PhoneNo.Length >= 6 && tblSellerOrgTO.PhoneNo.Length <= 12)
                        {
                            sellerPhoneNo = tblSellerOrgTO.PhoneNo;
                        }
                    }
                    List<TblAddressTO> tblAddressTOList = TblAddressBL.SelectOrgAddressList(invFromOrgId);
                    if (tblAddressTOList != null)
                    {
                        if (tblAddressTOList.Count > 0)
                        {
                            sellerAddressTO = tblAddressTOList[0];
                        }
                    }
                }
                tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@sellerGstIn", RemoveSpecialChars(sellerGstin));
                tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@sellerName", RemoveSpecialChars(sellerName));
                if (sellerAddressTO != null)
                {
                    string sellerAddr1 = RemoveSpecialChars(sellerAddressTO.PlotNo);
                    string sellerAddr2 = RemoveSpecialChars(sellerAddressTO.StreetName);
                    string sellerArea = "";
                    if (sellerAddressTO.AreaName != null && sellerAddressTO.AreaName != "")
                    {
                        sellerArea += sellerAddressTO.AreaName + ", ";
                    }
                    if (sellerAddressTO.VillageName != null && sellerAddressTO.VillageName != "")
                    {
                        sellerArea += sellerAddressTO.VillageName + ", ";
                    }
                    if (sellerAddressTO.TalukaName != null && sellerAddressTO.TalukaName != "")
                    {
                        sellerArea += sellerAddressTO.TalukaName + ", ";
                    }
                    if (sellerAddressTO.DistrictName != null && sellerAddressTO.DistrictName != "")
                    {
                        sellerArea += sellerAddressTO.DistrictName + ", ";
                    }

                    if (String.IsNullOrEmpty(sellerArea))
                    {
                        sellerArea = "---";
                    }
                    else
                    {
                        sellerArea = sellerArea.Trim().TrimEnd(',');
                    }
                    sellerArea = RemoveSpecialChars(sellerArea);
                    if (sellerArea.Length < 3)
                    {
                        sellerArea = padRight(sellerArea);
                    }
                    if (sellerAddr1 == null || sellerAddr1 == "")
                    {
                        sellerAddr1 = sellerArea;
                    }

                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@sellerAddr1", padRight(sellerAddr1));
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@sellerAddr2", padRight(sellerAddr2));
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@sellerLocation", padRight(sellerArea));
                    /*if (sellerAddressTO.Pincode == 0)
                    {
                        tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@sellerPincode", "110001");
                    }
                    else
                    {*/
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@sellerPincode", RemoveSpecialChars(sellerAddressTO.Pincode.ToString()));
                    //}
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@sellerStateCode", RemoveSpecialChars(sellerAddressTO.StateOrUTCode));

                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@sellerPhone", RemoveSpecialChars(sellerPhoneNo));
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@sellerEMail", RemoveSpecialChars(sellerEmailAddr));
                    
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@dispFromAddr1", padRight(sellerAddr1));
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@dispFromAddr2", padRight(sellerAddr2));
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@dispFromLocation", padRight(sellerArea));
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@dispFromPincode", RemoveSpecialChars(sellerAddressTO.Pincode.ToString()));
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@dispFromStateCode", RemoveSpecialChars(sellerAddressTO.StateOrUTCode));
                }
                string buyerName = tblInvoiceTO.DealerName;
                string buyerEmailAddr = "";
                TblOrganizationTO tblBuyerOrgTO = TblOrganizationBL.SelectTblOrganizationTO(tblInvoiceTO.DealerOrgId);
                if (tblBuyerOrgTO != null)
                {
                    buyerName = tblBuyerOrgTO.FirmName;
                    buyerEmailAddr = tblBuyerOrgTO.EmailAddr;
                }
                if (billingAddrTO != null)
                {
                    //Saket Added
                    buyerName = billingAddrTO.BillingName;

                    string billingAddr2 = GetAreaFromAddress(billingAddrTO);
                    
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@buyerGstIn", RemoveSpecialChars(billingAddrTO.GstinNo.ToUpper()));
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@buyerName", RemoveSpecialChars(buyerName));
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@buyerAddr1", padRight(billingAddrTO.Address));
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@buyerAddr2", padRight(billingAddr2));
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@buyerLocation", padRight(billingAddr2));
                    /*if (billingAddrTO.PinCode == "0")
                    {
                        tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@buyerPincode", "110001");
                    }
                    else
                    {*/
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@buyerPincode", RemoveSpecialChars(billingAddrTO.PinCode));
                    //}
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@buyerStateCode", RemoveSpecialChars(billingAddrTO.StateOrUTCode));
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@buyerSupplyStateCode", RemoveSpecialChars(billingAddrTO.StateOrUTCode));
                    string contactNo = "9100000000";
                    if (billingAddrTO.ContactNo != null)
                    {
                        if (billingAddrTO.ContactNo.Length >= 6 && billingAddrTO.ContactNo.Length <= 12)
                        {
                            contactNo = billingAddrTO.ContactNo;
                        }
                    }
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@buyerPhone", RemoveSpecialChars(contactNo));
                    if (buyerEmailAddr != null)
                    {
                        if (buyerEmailAddr.Length >= 3 && buyerEmailAddr.Length <= 100)
                        {
                            tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@buyerEMail", RemoveSpecialChars(buyerEmailAddr));
                        }
                    }
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@buyerEMail", "test@einv.com"); //if buyerEmailAddr is not available
                }

                if (shippingAddrTO != null)
                {
                    string shippingAddr2 = GetAreaFromAddress(shippingAddrTO);

                    if (string.IsNullOrEmpty(shippingAddrTO.BillingName))
                    {
                        shippingAddrTO.BillingName = buyerName;
                    }
                    if (String.IsNullOrEmpty(billingAddrTO.BillingName) || String.IsNullOrEmpty(billingAddrTO.Address) || String.IsNullOrEmpty(billingAddrTO.GstinNo) || String.IsNullOrEmpty(shippingAddrTO.BillingName) || String.IsNullOrEmpty(shippingAddrTO.Address) || String.IsNullOrEmpty(shippingAddrTO.GstinNo))
                    {
                        TblConfigParamsTO tblConfigParamsTO = TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_EINVOICE_SHIPPING_ADDRESS);
                        if (tblConfigParamsTO == null)
                        {
                            tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@shippingAddr", "");
                        }
                        else
                        {
                            tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@shippingAddr", tblConfigParamsTO.ConfigParamVal);
                        }
                    }
                    else
                    {
                        //[2021-02-26] Dhananjay added
                        if (shippingAddrTO.BillingName.Trim() == billingAddrTO.BillingName.Trim() && shippingAddrTO.Address.Trim() == billingAddrTO.Address.Trim() && shippingAddrTO.GstinNo.Trim() == billingAddrTO.GstinNo.Trim())
                        {
                            tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@shippingAddr", "");
                        }
                        else
                        {
                            TblConfigParamsTO tblConfigParamsTO = TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_EINVOICE_SHIPPING_ADDRESS);
                            if (tblConfigParamsTO == null)
                            {
                                tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@shippingAddr", "");
                            }
                            else
                            {
                                tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@shippingAddr", tblConfigParamsTO.ConfigParamVal);
                            }
                        }
                        //[2021-02-26] Dhananjay end
                    }
                    if (string.IsNullOrEmpty(shippingAddrTO.GstinNo))
                    {
                        //shippingAddrTO.GstinNo = billingAddrTO.GstinNo;
                    }
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@shippingName", RemoveSpecialChars(shippingAddrTO.BillingName));
                    if (string.IsNullOrEmpty(shippingAddrTO.GstinNo))
                    {
                        tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("\"gstin\": \"@shippingGstIn\",", "");
                    }
                    else
                    {
                        tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@shippingGstIn", RemoveSpecialChars(shippingAddrTO.GstinNo.ToUpper()));
                    }
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@shipToAddr1", padRight(shippingAddrTO.Address));
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@shipToAddr2", padRight(shippingAddr2));
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@shipToLocation", padRight(shippingAddr2));
                    /*if (shippingAddrTO.PinCode == "0")
                    {
                        tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@shipToPincode", "110001");
                    }
                    else
                    {*/
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@shipToPincode", RemoveSpecialChars(shippingAddrTO.PinCode));
                    //}
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@shipToStateCode", RemoveSpecialChars(shippingAddrTO.StateOrUTCode));
                }

                //tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@othChrg", tblInvoiceTO.FreightAmt.ToString());
                tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@cgstAmt", tblInvoiceTO.CgstAmt.ToString());
                tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@sgstAmt", tblInvoiceTO.SgstAmt.ToString());
                tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@igstAmt", tblInvoiceTO.IgstAmt.ToString());
                tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@grandTotal", tblInvoiceTO.GrandTotal.ToString());
                tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@roundOffAmt", tblInvoiceTO.RoundOffAmt.ToString());
                if (eInvoiceCreationType == (Int32)Constants.EGenerateEInvoiceCreationType.GENERATE_INVOICE_ONLY)
                {
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@ewbDtls", "");
                }
                else if (eInvoiceCreationType == (Int32)Constants.EGenerateEInvoiceCreationType.INVOICE_WITH_EWAY_BILL)
                {
                    //tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("\r\n\t\t@ewbDtls", ",\r\n        \"EwbDtls\":{\r\n        \"VehType\": \"R\",\r\n        \"VehNo\": \"@vehicleNo\",\r\n        \"TransMode\": \"1\",\r\n        \"Distance\": \"@distanceInKM\"\r\n        }");
                    //tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@vehicleNo", GetValidVehichleNumber(tblInvoiceTO.VehicleNo));
                    //tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@distanceInKM", RemoveSpecialChars(tblInvoiceTO.DistanceInKM.ToString()));

                    /*tblEInvoiceApiTOEWayBill.BodyParam = tblEInvoiceApiTOEWayBill.BodyParam.Replace("{\"action\": \"GENERATEEWB\",", "");
                    tblEInvoiceApiTOEWayBill.BodyParam = tblEInvoiceApiTOEWayBill.BodyParam.Replace("data", "EwbDtls");
                    tblEInvoiceApiTOEWayBill.BodyParam = tblEInvoiceApiTOEWayBill.BodyParam.Replace("\"Irn\": \"@IrnNo\",", "");
                    tblEInvoiceApiTOEWayBill.BodyParam = tblEInvoiceApiTOEWayBill.BodyParam.Replace("}\r\n}", "}");

                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@ewbDtls", ",\r\n        " + tblEInvoiceApiTOEWayBill.BodyParam);*/

                    TblConfigParamsTO tblConfigParamsTO = TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_EINVOICE_EWAY_BILL);
                    if (tblConfigParamsTO == null)
                    {
                        tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@ewbDtls", "");
                    }
                    else
                    {
                        tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@ewbDtls", tblConfigParamsTO.ConfigParamVal);
                    }

                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@VehNo", GetValidVehichleNumber(tblInvoiceTO.VehicleNo));
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@DistanceinKm", RemoveSpecialChars(tblInvoiceTO.DistanceInKM.ToString()));
                }
                else
                {
                    resultMsg.DefaultBehaviour("Wrong eInvoiceType");
                    return resultMsg;
                }
                int nStartIdx = tblEInvoiceApiTO.BodyParam.IndexOf("slNo");
                int nEndIdx = tblEInvoiceApiTO.BodyParam.IndexOf("@idInvoiceItem");
                string itemList = tblEInvoiceApiTO.BodyParam.Substring(nStartIdx - 20, ((nEndIdx - nStartIdx) + "@idInvoiceItem".Length + 36));
                string itemListReplacedWithValue = "";
                List<TblInvoiceItemDetailsTO> InvoiceItemDetailsTOList = tblInvoiceTO.InvoiceItemDetailsTOList;
                Int32 nSrNo = 0;
                double otherCharges = 0;
                double taxableAmt = 0;
                for (int i = 0; i <= InvoiceItemDetailsTOList.Count - 1; i++)
                {
                    TblInvoiceItemDetailsTO InvoiceItemDetailsTO = InvoiceItemDetailsTOList[i];
                    string itemListTobeReplaced = itemList;


                    String unit = "MTS";

                    TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO = TblProdGstCodeDtlsBL.SelectTblProdGstCodeDtlsTO(InvoiceItemDetailsTO.ProdGstCodeId);
                    if (tblProdGstCodeDtlsTO == null)
                    {
                        resultMsg.DefaultBehaviour("ProdGSTCodeDetails found null against IdInvoiceItem is : " + InvoiceItemDetailsTO.IdInvoiceItem + ".");
                        resultMsg.DisplayMessage = "GSTIN Not Defined for Item :" + InvoiceItemDetailsTO.ProdItemDesc;
                        return resultMsg;
                    }
                    TblProductItemTO tblProductItemTO = null;
                    if (tblProdGstCodeDtlsTO.ProdItemId > 0)
                    {
                        tblProductItemTO = TblProductItemBL.SelectTblProductItemTO(tblProdGstCodeDtlsTO.ProdItemId);

                        if (tblProductItemTO != null)
                        {
                            if (! string.IsNullOrEmpty(tblProductItemTO.MappedEInvoiceUOM))
                            {
                                unit = tblProductItemTO.MappedEInvoiceUOM;
                            }
                        }
                    }

                    if (InvoiceItemDetailsTO.OtherTaxId > 0)
                    {
                        TblOtherTaxesTO otherTaxesTO = DAL.TblOtherTaxesDAO.SelectTblOtherTaxes(InvoiceItemDetailsTO.OtherTaxId);
                        if (otherTaxesTO != null)
                        {
                            if (otherTaxesTO.IsBefore == 1)
                            {
                                //itemListTobeReplaced = itemListTobeReplaced.Replace("@srNo", "-");
                                itemListTobeReplaced = itemListTobeReplaced.Replace("@isServc", "Y");
                                itemListTobeReplaced = itemListTobeReplaced.Replace("@gstinCodeNo", "9965");
                            }
                            else if (otherTaxesTO.IsAfter == 1)
                            {
                                otherCharges += InvoiceItemDetailsTO.GrandTotal;
                                continue;
                            }
                        }
                    }
                    if (InvoiceItemDetailsTO.InvoiceQty == 0)
                    {
                        itemListTobeReplaced = itemListTobeReplaced.Replace("@basicTotal", InvoiceItemDetailsTO.TaxableAmt.ToString());
                    }

                    taxableAmt += InvoiceItemDetailsTO.TaxableAmt;
                    nSrNo++;
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@srNo", nSrNo.ToString());
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@prodItemDesc", padRight(InvoiceItemDetailsTO.ProdItemDesc, char.Parse(" ")));
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@isServc", "N");
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@gstinCodeNo", padRight(InvoiceItemDetailsTO.GstinCodeNo, char.Parse("-"), 4));
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@invoiceQty", string.Format("{0:0.000}", InvoiceItemDetailsTO.InvoiceQty));
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@unit", unit);
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@rate", InvoiceItemDetailsTO.Rate.ToString());
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@basicTotal", InvoiceItemDetailsTO.BasicTotal.ToString());
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@discount", InvoiceItemDetailsTO.CdAmt.ToString());
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@pretaxAmt", InvoiceItemDetailsTO.TaxableAmt.ToString());
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@assAmt", InvoiceItemDetailsTO.TaxableAmt.ToString());

                    Double TaxRatePct = 0;
                    if (InvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList != null && InvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList.Count > 0)
                    {
                        for (int nTax = 0; nTax <= InvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList.Count - 1; nTax++)
                        {
                            TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO = InvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList[nTax];
                            //itemListTobeReplaced = itemListTobeReplaced.Replace("@taxRatePct", tblInvoiceItemTaxDtlsTO.TaxRatePct.ToString());
                            TaxRatePct += tblInvoiceItemTaxDtlsTO.TaxRatePct;
                            if (tblInvoiceItemTaxDtlsTO.TaxTypeId == (int)Constants.TaxTypeE.IGST)
                            {
                                itemListTobeReplaced = itemListTobeReplaced.Replace("@igstItemAmt", tblInvoiceItemTaxDtlsTO.TaxAmt.ToString());
                            }
                            else if (tblInvoiceItemTaxDtlsTO.TaxTypeId == (int)Constants.TaxTypeE.CGST)
                            {
                                itemListTobeReplaced = itemListTobeReplaced.Replace("@cgstItemAmt", tblInvoiceItemTaxDtlsTO.TaxAmt.ToString());
                            }
                            else if (tblInvoiceItemTaxDtlsTO.TaxTypeId == (int)Constants.TaxTypeE.SGST)
                            {
                                itemListTobeReplaced = itemListTobeReplaced.Replace("@sgstItemAmt", tblInvoiceItemTaxDtlsTO.TaxAmt.ToString());
                            }
                        }
                    }
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@taxRatePct", TaxRatePct.ToString());
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@igstItemAmt", "0");
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@cgstItemAmt", "0");
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@sgstItemAmt", "0");
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@totItemVal", InvoiceItemDetailsTO.GrandTotal.ToString());
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@idInvoiceItem", InvoiceItemDetailsTO.IdInvoiceItem.ToString());

                    if (i == 0)
                    {
                        itemListReplacedWithValue += itemListTobeReplaced;
                    }
                    else
                    {
                        itemListReplacedWithValue += ",\r\n            " + itemListTobeReplaced;
                    }
                }

                tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace(itemList, itemListReplacedWithValue);
                tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@taxableAmt", Math.Round(taxableAmt,2).ToString());
                tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@othChrg", Math.Round(otherCharges,2).ToString());

                IRestResponse response = CallRestAPIs(tblEInvoiceApiTO.ApiBaseUri + tblEInvoiceApiTO.ApiFunctionName, tblEInvoiceApiTO.ApiMethod, tblEInvoiceApiTO.HeaderParam, tblEInvoiceApiTO.BodyParam);

                resultMsg = ProcessEInvoiceAPIResponse(tblInvoiceTO, (int)EInvoiceAPIE.GENERATE_EINVOICE, loginUserId, response, true);
            }
            catch (Exception ex)
            {
                resultMsg.MessageType = ResultMessageE.Error;
                resultMsg.Text = ex.Message;
                return resultMsg;
            }
            finally
            {

            }
            return resultMsg;
        }

        private static ResultMessage ProcessEInvoiceAPIResponse(TblInvoiceTO tblInvoiceTO, Int32 IdAPI, Int32 loginUserId, IRestResponse response, bool bNewInvoice)
        {
            ResultMessage resultMsg = new ResultMessage();
            try
            {
                string IrnNo = null;
                string EwayBillNo = null;
                JObject json = JObject.Parse(response.Content);
                if (json.ContainsKey("data"))
                {
                    JObject jsonData = JObject.Parse(json["data"].ToString());
                    IrnNo = (string)jsonData["Irn"];
                    if (jsonData.ContainsKey("EwbNo"))
                    {
                        EwayBillNo = (string)jsonData["EwbNo"];
                    }
                }
                if (json.ContainsKey("error"))
                {
                    JArray arrError = JArray.Parse(json["error"].ToString());
                    foreach (var err in arrError)
                    {
                        JObject jsonError = JObject.Parse(err.ToString());
                        string errorCodes = (string)jsonError["errorCodes"];
                        string errorMsg = (string)jsonError["errorMsg"];
                        //if (errorCodes == "1005" && errorMsg == "Invalid Token")
                        //{
                        //    GenerateEInvoice(loginUserId, tblInvoiceTO.IdInvoice, eInvoiceCreationType, true);
                        //    return null;
                        //}
                    }
                }
                SqlConnection conn = new SqlConnection(Startup.ConnectionString);
                SqlTransaction tran = null;
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction();

                    resultMsg = InsertIntoTblEInvoiceApiResponse(IdAPI, tblInvoiceTO.IdInvoice, response, loginUserId, conn, tran);
                    if (resultMsg.Result != 1)
                    {
                        tran.Rollback();
                        return resultMsg;
                    }

                    if (IrnNo == null)
                    {
                        tran.Commit();
                        resultMsg.DefaultBehaviour(json.ToString());
                        resultMsg.DisplayMessage = json.ToString();
                        resultMsg.Text = resultMsg.DisplayMessage;
                        return resultMsg;
                    }

                    tblInvoiceTO.IrnNo = IrnNo;
                    tblInvoiceTO.IsEInvGenerated = 1;
                    tblInvoiceTO.UpdatedBy = Convert.ToInt32(loginUserId);

                    resultMsg = UpdateTempInvoiceForEInvoice(tblInvoiceTO, conn, tran);
                    if (resultMsg.Result == 0)
                    {
                        tran.Rollback();
                        return resultMsg;
                    }

                    resultMsg.DisplayMessage = "eInvoice generated successfully;";
                    if (bNewInvoice == false)
                    {
                        resultMsg.DisplayMessage = "eInvoice details updated successfully;";
                    }

                    if (EwayBillNo != null)
                    {
                        tblInvoiceTO.ElectronicRefNo = EwayBillNo;
                        tblInvoiceTO.IsEWayBillGenerated = 1;
                        tblInvoiceTO.UpdatedBy = Convert.ToInt32(loginUserId);

                        resultMsg = UpdateTempInvoiceForEWayBill(tblInvoiceTO, conn, tran);
                        if (resultMsg.Result == 0)
                        {
                            tran.Rollback();
                            return resultMsg;
                        }
                        resultMsg.DisplayMessage = "eInvoice and eWayBill generated successfully;";
                        if (bNewInvoice == false)
                        {
                            resultMsg.DisplayMessage = "eInvoice and eWayBill details updated successfully;";
                        }
                    }

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    resultMsg.DefaultExceptionBehaviour(ex, "EInvoice_Generate");
                    return resultMsg;
                }
                finally
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                resultMsg.MessageType = ResultMessageE.Error;
                resultMsg.Text = ex.Message;
                return resultMsg;
            }
            finally
            {

            }
            return resultMsg;
        }

        private static IRestResponse CallRestAPIs(string ApiBaseUri, string ApiMethod, string HeaderParam, string BodyParam)
        {
            var client = new RestClient(ApiBaseUri);
            client.Timeout = -1;
            Method method = Method.POST;
            if (ApiMethod.ToUpper() == "POST")
            {
                method = Method.POST;
            }
            else if (ApiMethod.ToUpper() == "GET")
            {
                method = Method.GET;
            }
            else if (ApiMethod.ToUpper() == "PUT")
            {
                method = Method.PUT;
            }
            var request = new RestRequest(method);

            JObject oHeaderParam = JObject.Parse(HeaderParam);
            foreach (var token in oHeaderParam)
            {
                request.AddHeader(token.Key, token.Value.ToString());
            }

            if (oHeaderParam["Content-Type"].ToString() == "application/x-www-form-urlencoded")
            {
                JObject oBodyParam = JObject.Parse(BodyParam);
                foreach (var token in oBodyParam)
                {
                    request.AddParameter(token.Key, token.Value.ToString());
                }
            }
            else if (oHeaderParam["Content-Type"].ToString() == "application/json")
            {
                request.AddParameter("application/json", BodyParam, ParameterType.RequestBody);
            }

            return client.Execute(request);
        }

        private static ResultMessage InsertIntoTblEInvoiceSessionApiResponse(IRestResponse response, Int32 apiId, Int32 loginUserId, Int32 OrgId, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            TblEInvoiceSessionApiResponseTO tblEInvoiceSessionApiResponseTO = new TblEInvoiceSessionApiResponseTO();
            tblEInvoiceSessionApiResponseTO.ApiId = apiId;
            if (response.ResponseStatus == ResponseStatus.Completed)
            {
                JObject json = JObject.Parse(response.Content);
                string access_token = (string)json["access_token"];
                tblEInvoiceSessionApiResponseTO.AccessToken = access_token;
                tblEInvoiceSessionApiResponseTO.Response = response.Content;
            }
            else
            {
                tblEInvoiceSessionApiResponseTO.Response = response.ErrorMessage;
            }
            tblEInvoiceSessionApiResponseTO.ResponseStatus = response.ResponseStatus.ToString();

            DateTime serverDate = CommonDAO.SelectServerDateTime();
            tblEInvoiceSessionApiResponseTO.CreatedBy = Convert.ToInt32(loginUserId);
            tblEInvoiceSessionApiResponseTO.CreatedOn = serverDate;
            tblEInvoiceSessionApiResponseTO.OrgId = OrgId;
            result = TblEInvoiceSessionApiResponseDAO.InsertTblEInvoiceSessionApiResponse(tblEInvoiceSessionApiResponseTO, conn, tran);
            if (result != 1)
            {
                resultMessage.Text = "Sorry..Record Could not be saved.";
                resultMessage.DisplayMessage = "Error while insert into TblEInvoiceSessionApiResponse";
                resultMessage.Result = 0;
                resultMessage.MessageType = ResultMessageE.Error;
            }
            else
            {
                resultMessage.DefaultSuccessBehaviour();
            }
            return resultMessage;
        }

        private static ResultMessage UpdateTblEInvoiceApi(int IdApi, string access_token, DateTime tokenExpiresAt, int loginUserId, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            TblEInvoiceApiTO tblEInvoiceApiTO = new TblEInvoiceApiTO();
            tblEInvoiceApiTO.IdApi = IdApi;
            tblEInvoiceApiTO.IsSession = 1;
            tblEInvoiceApiTO.AccessToken = access_token;
            tblEInvoiceApiTO.SessionExpiresAt = tokenExpiresAt;
            DateTime serverDate1 = CommonDAO.SelectServerDateTime();
            tblEInvoiceApiTO.UpdatedBy = Convert.ToInt32(loginUserId);
            tblEInvoiceApiTO.UpdatedOn = serverDate1;
            result = TblEInvoiceApiDAO.UpdateTblEInvoiceApiSession(tblEInvoiceApiTO, conn, tran);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour("Error While Update TblEInvoiceApi");
            }
            else
            {
                resultMessage.DefaultSuccessBehaviour();
            }
            return resultMessage;
        }

        private static ResultMessage InsertIntoTblEInvoiceApiResponse(Int32 apiId, Int32 InvoiceId, IRestResponse response, Int32 loginUserId, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            TblEInvoiceApiResponseTO tblEInvoiceApiResponseTO = new TblEInvoiceApiResponseTO();
            tblEInvoiceApiResponseTO.ApiId = apiId;
            if (response.ResponseStatus == ResponseStatus.Completed)
            {
                tblEInvoiceApiResponseTO.InvoiceId = InvoiceId;
                tblEInvoiceApiResponseTO.Response = response.Content;
            }
            else
            {
                tblEInvoiceApiResponseTO.Response = response.ErrorMessage;
            }
            tblEInvoiceApiResponseTO.ResponseStatus = response.ResponseStatus.ToString();

            DateTime serverDate = CommonDAO.SelectServerDateTime();
            tblEInvoiceApiResponseTO.CreatedBy = Convert.ToInt32(loginUserId);
            tblEInvoiceApiResponseTO.CreatedOn = serverDate;
            result = TblEInvoiceApiResponseDAO.InsertTblEInvoiceApiResponse(tblEInvoiceApiResponseTO, conn, tran);
            if (result != 1)
            {
                resultMessage.Text = "Sorry..Record Could not be saved.";
                resultMessage.DisplayMessage = "Error while insert into TempEInvoiceApiResponse";
                resultMessage.Result = 0;
                resultMessage.MessageType = ResultMessageE.Error;
            }
            else
            {
                resultMessage.DefaultSuccessBehaviour();
            }
            return resultMessage;
        }

        private static ResultMessage UpdateTempInvoiceForEInvoice(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();

            int result = 0;
            DateTime serverDate1 = CommonDAO.SelectServerDateTime();
            tblInvoiceTO.UpdatedOn = serverDate1;
            result = TblInvoiceDAO.UpdateEInvoicNo(tblInvoiceTO, conn, tran);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour("Error While Update tempInvoice for EInvoice");
            }
            else
            {
                resultMessage.DefaultSuccessBehaviour();
            }
            return resultMessage;
        }

        private static ResultMessage UpdateTempInvoiceForEWayBill(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            DateTime serverDate1 = CommonDAO.SelectServerDateTime();
            tblInvoiceTO.UpdatedOn = serverDate1;
            result = TblInvoiceDAO.UpdateEWayBill(tblInvoiceTO, conn, tran);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour("Error While Update tempInvoice for EWaybill");
            }
            else
            {
                resultMessage.DefaultSuccessBehaviour();
            }
            return resultMessage;
        }
        private static ResultMessage UpdateTempInvoiceDistanceInKM(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            DateTime serverDate1 = CommonDAO.SelectServerDateTime();
            tblInvoiceTO.UpdatedOn = serverDate1;
            result = TblInvoiceDAO.UpdateTempInvoiceDistanceInKM(tblInvoiceTO, conn, tran);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour("Error While Update tempInvoice DistanceInKM");
            }
            else
            {
                resultMessage.DefaultSuccessBehaviour();
            }
            return resultMessage;
        }

        /// <summary>
        /// Dhananjay[18-11-2020] : Added To Generate eInvvoice.
        /// </summary>
        public static ResultMessage CancelEInvoice(Int32 loginUserId, Int32 idInvoice, bool forceToGetToken = false)
        {
            ResultMessage resultMessage = new ResultMessage();
            string sellerGstin = "27AALFP1139Q004";

            TblInvoiceTO tblInvoiceTO = new TblInvoiceTO();
            tblInvoiceTO = SelectTblInvoiceTO(idInvoice);
            if (tblInvoiceTO == null)
            {
                throw new Exception("InvoiceTO is null");
            }

            if (tblInvoiceTO.IsEInvGenerated != 1)
            {
                resultMessage.Text = "EInvoice is not generated for this invoice.";
                resultMessage.DisplayMessage = "EInvoice is not generated for this invoice.";
                resultMessage.Result = 0;
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;
            }

            List<TblOrgLicenseDtlTO> TblOrgLicenseDtlTOList = TblOrgLicenseDtlBL.SelectAllTblOrgLicenseDtlList(tblInvoiceTO.InvFromOrgId);
            if (TblOrgLicenseDtlTOList != null)
            {
                for (int i = 0; i <= TblOrgLicenseDtlTOList.Count - 1; i++)
                {
                    if (TblOrgLicenseDtlTOList[i].LicenseId == (Int32)CommercialLicenseE.IGST_NO)
                    {
                        sellerGstin = TblOrgLicenseDtlTOList[i].LicenseValue.ToUpper();
                        break;
                    }
                }
            }

            string access_token_OauthToken = null;
            resultMessage = EInvoice_OauthToken(loginUserId, sellerGstin, forceToGetToken, tblInvoiceTO.InvFromOrgId);
            if (resultMessage.Result != 1)
            {
                throw new Exception("Error in EInvoice_OauthToken");
            }

            access_token_OauthToken = resultMessage.Tag.ToString();
            if (access_token_OauthToken == null)
            {
                throw new Exception("access_token_OauthToken is null");
            }

            string access_token_Authentication = null;
            resultMessage = EInvoice_Authentication(loginUserId, access_token_OauthToken, sellerGstin, forceToGetToken, tblInvoiceTO.InvFromOrgId);
            if (resultMessage.Result != 1)
            {
                throw new Exception("Error in EInvoice_Authentication");
            }

            access_token_Authentication = resultMessage.Tag.ToString();
            if (access_token_Authentication == null)
            {
                throw new Exception("access_token_Authentication is null");
            }

            return EInvoice_Cancel(tblInvoiceTO, loginUserId, access_token_Authentication, sellerGstin);
        }

        public static ResultMessage EInvoice_Cancel(TblInvoiceTO tblInvoiceTO, Int32 loginUserId, string access_token_Authentication, string sellerGstin)
        {
            ResultMessage resultMsg = new ResultMessage();
            if (access_token_Authentication == "")
            {
                resultMsg = EInvoice_Authentication(loginUserId, "", sellerGstin, false, tblInvoiceTO.InvFromOrgId);
                if (resultMsg.Result != 1)
                {
                    throw new Exception("Error in EInvoice_Authentication");
                }
                else
                {
                    access_token_Authentication = resultMsg.Tag.ToString();
                }
            }

            TblEInvoiceApiTO tblEInvoiceApiTO = GetTblEInvoiceApiTO((int)EInvoiceAPIE.CANCEL_EINVOICE);
            if (tblEInvoiceApiTO == null)
            {
                throw new Exception("EInvoiceApiTO is null");
            }

            tblEInvoiceApiTO.HeaderParam = tblEInvoiceApiTO.HeaderParam.Replace("@gstin", sellerGstin);
            tblEInvoiceApiTO.HeaderParam = tblEInvoiceApiTO.HeaderParam.Replace("@token", access_token_Authentication);

            tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@IrnNo", tblInvoiceTO.IrnNo);

            IRestResponse response = CallRestAPIs(tblEInvoiceApiTO.ApiBaseUri + tblEInvoiceApiTO.ApiFunctionName, tblEInvoiceApiTO.ApiMethod, tblEInvoiceApiTO.HeaderParam, tblEInvoiceApiTO.BodyParam);

            string IrnNo = null;
            JObject json = JObject.Parse(response.Content);
            if (json.ContainsKey("data"))
            {
                JObject jsonData = JObject.Parse(json["data"].ToString());
                IrnNo = (string)jsonData["Irn"];
            }
            if (json.ContainsKey("error"))
            {
                JArray arrError = JArray.Parse(json["error"].ToString());
                foreach (var err in arrError)
                {
                    JObject jsonError = JObject.Parse(err.ToString());
                    string errorCodes = (string)jsonError["errorCodes"];
                    string errorMsg = (string)jsonError["errorMsg"];
                    if (errorCodes == "1005" && errorMsg == "Invalid Token")
                    {
                        CancelEInvoice(loginUserId, tblInvoiceTO.IdInvoice, true);
                        return null;
                    }
                    if (errorCodes == "2270")
                    {
                        response.Content = response.Content.Replace("The allowed cancellation time limit is crossed, you cannot cancel the IRN", "You cannot cancel eInvoice after 24 hrs of generation.");
                        json = JObject.Parse(response.Content);
                    }
                }
            }


            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                resultMsg = InsertIntoTblEInvoiceApiResponse(tblEInvoiceApiTO.IdApi, tblInvoiceTO.IdInvoice, response, loginUserId, conn, tran);
                if (resultMsg.Result != 1)
                {
                    tran.Rollback();
                    return resultMsg;
                }

                if (IrnNo == null)
                {
                    tran.Commit();
                    resultMsg.DefaultBehaviour(json.ToString());
                    resultMsg.DisplayMessage = json.ToString();
                    resultMsg.Text = resultMsg.DisplayMessage;
                    return resultMsg;
                }

                tblInvoiceTO.IrnNo = "";
                tblInvoiceTO.IsEInvGenerated = 0;
                tblInvoiceTO.UpdatedBy = Convert.ToInt32(loginUserId);
                resultMsg = UpdateTempInvoiceForEInvoice(tblInvoiceTO, conn, tran);
                if (resultMsg.Result != 1)
                {
                    tran.Rollback();
                    return resultMsg;
                }
                resultMsg.DisplayMessage = "eInvoice cancelled successfully;";
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMsg.DefaultExceptionBehaviour(ex, "EInvoice_Cancel");
            }
            finally
            {
                conn.Close();
            }

            return resultMsg;
        }

        /// <summary>
        /// Dhananjay[01-03-2021] : Added To Get and Update eInvvoice.
        /// </summary>
        public static ResultMessage GetAndUpdateEInvoice(Int32 loginUserId, Int32 idInvoice, bool forceToGetToken = false)
        {
            ResultMessage resultMessage = new ResultMessage();
            string sellerGstin = "27AACCK4472B1ZS";

            TblInvoiceTO tblInvoiceTO = new TblInvoiceTO();
            tblInvoiceTO = SelectTblInvoiceTO(idInvoice);
            if (tblInvoiceTO == null)
            {
                throw new Exception("InvoiceTO is null");
            }

            if (tblInvoiceTO.IsEInvGenerated != 1)
            {
                resultMessage.Text = "EInvoice is not generated for this invoice.";
                resultMessage.DisplayMessage = "EInvoice is not generated for this invoice.";
                resultMessage.Result = 0;
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;
            }

            List<TblOrgLicenseDtlTO> TblOrgLicenseDtlTOList = TblOrgLicenseDtlBL.SelectAllTblOrgLicenseDtlList(tblInvoiceTO.InvFromOrgId);
            if (TblOrgLicenseDtlTOList != null)
            {
                for (int i = 0; i <= TblOrgLicenseDtlTOList.Count - 1; i++)
                {
                    if (TblOrgLicenseDtlTOList[i].LicenseId == (Int32)CommercialLicenseE.IGST_NO)
                    {
                        sellerGstin = TblOrgLicenseDtlTOList[i].LicenseValue.ToUpper();
                        break;
                    }
                }
            }

            string access_token_OauthToken = null;
            resultMessage = EInvoice_OauthToken(loginUserId, sellerGstin, forceToGetToken, tblInvoiceTO.InvFromOrgId);
            if (resultMessage.Result != 1)
            {
                throw new Exception("Error in EInvoice_OauthToken");
            }

            access_token_OauthToken = resultMessage.Tag.ToString();
            if (access_token_OauthToken == null)
            {
                throw new Exception("access_token_OauthToken is null");
            }

            string access_token_Authentication = null;
            resultMessage = EInvoice_Authentication(loginUserId, access_token_OauthToken, sellerGstin, forceToGetToken, tblInvoiceTO.InvFromOrgId);
            if (resultMessage.Result != 1)
            {
                throw new Exception("Error in EInvoice_Authentication");
            }

            access_token_Authentication = resultMessage.Tag.ToString();
            if (access_token_Authentication == null)
            {
                throw new Exception("access_token_Authentication is null");
            }

            return EInvoice_GetAndUpdate(tblInvoiceTO, loginUserId, access_token_Authentication, sellerGstin);
        }

        /// <summary>
        /// Dhananjay[01-03-2021] : Added To Get and Update eInvvoice.
        /// </summary>
        public static ResultMessage EInvoice_GetAndUpdate(TblInvoiceTO tblInvoiceTO, Int32 loginUserId, string access_token_Authentication, string sellerGstin)
        {
            ResultMessage resultMsg = new ResultMessage();
            if (access_token_Authentication == "")
            {
                resultMsg = EInvoice_Authentication(loginUserId, "", sellerGstin, false, tblInvoiceTO.InvFromOrgId);
                if (resultMsg.Result != 1)
                {
                    throw new Exception("Error in EInvoice_Authentication");
                }
                else
                {
                    access_token_Authentication = resultMsg.Tag.ToString();
                }
            }

            TblEInvoiceApiTO tblEInvoiceApiTO = GetTblEInvoiceApiTO((int)EInvoiceAPIE.GET_EINVOICE);
            if (tblEInvoiceApiTO == null)
            {
                throw new Exception("EInvoiceApiTO is null");
            }

            tblEInvoiceApiTO.HeaderParam = tblEInvoiceApiTO.HeaderParam.Replace("@gstin", sellerGstin);
            tblEInvoiceApiTO.HeaderParam = tblEInvoiceApiTO.HeaderParam.Replace("@token", access_token_Authentication);

            string ApiFunctionName = tblEInvoiceApiTO.ApiFunctionName.Replace("IrnNo", tblInvoiceTO.IrnNo);
            IRestResponse response = CallRestAPIs(tblEInvoiceApiTO.ApiBaseUri + ApiFunctionName, tblEInvoiceApiTO.ApiMethod, tblEInvoiceApiTO.HeaderParam, tblEInvoiceApiTO.BodyParam);

            resultMsg = ProcessEInvoiceAPIResponse(tblInvoiceTO, (int)EInvoiceAPIE.GENERATE_EINVOICE, loginUserId, response, false);
            return resultMsg;
        }

        /// <summary>
        /// Dhananjay[18-11-2020] : Added To Generate ewaybill.
        /// </summary>
        public static ResultMessage GenerateEWayBill(Int32 loginUserId, Int32 idInvoice, decimal distanceInKM, bool forceToGetToken = false)
        {
            ResultMessage resultMessage = new ResultMessage();

            string sellerGstin = "27AALFP1139Q004";
            TblInvoiceTO tblInvoiceTO = null;
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tblInvoiceTO = new TblInvoiceTO();
                tblInvoiceTO = SelectTblInvoiceTOWithDetails(idInvoice, conn, tran);

                if (tblInvoiceTO == null)
                {
                    conn.Close();
                    throw new Exception("InvoiceTO is null");
                }

                tblInvoiceTO.DistanceInKM = distanceInKM;
                tblInvoiceTO.UpdatedBy = Convert.ToInt32(loginUserId);
                resultMessage = UpdateTempInvoiceDistanceInKM(tblInvoiceTO, conn, tran);
                conn.Close();
                if (resultMessage.Result != 1)
                {
                    return resultMessage;
                }
            }
            catch (Exception ex1)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = ex1.Message;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }

            if (tblInvoiceTO.IsEInvGenerated != 1)
            {
                resultMessage.Text = "EInvoice is not generated for this invoice.";
                resultMessage.DisplayMessage = "EInvoice is not generated for this invoice.";
                resultMessage.Result = 0;
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;
            }

            List<TblOrgLicenseDtlTO> TblOrgLicenseDtlTOList = TblOrgLicenseDtlBL.SelectAllTblOrgLicenseDtlList(tblInvoiceTO.InvFromOrgId);
            if (TblOrgLicenseDtlTOList != null)
            {
                for (int i = 0; i <= TblOrgLicenseDtlTOList.Count - 1; i++)
                {
                    if (TblOrgLicenseDtlTOList[i].LicenseId == (Int32)CommercialLicenseE.IGST_NO)
                    {
                        sellerGstin = TblOrgLicenseDtlTOList[i].LicenseValue.ToUpper();
                        break;
                    }
                }
            }

            string access_token_OauthToken = null;
            resultMessage = EInvoice_OauthToken(loginUserId, sellerGstin, forceToGetToken, tblInvoiceTO.InvFromOrgId);
            if (resultMessage.Result != 1)
            {
                throw new Exception("Error in EInvoice_OauthToken");
            }

            access_token_OauthToken = resultMessage.Tag.ToString();
            if (access_token_OauthToken == null)
            {
                throw new Exception("access_token_OauthToken is null");
            }

            string access_token_Authentication = null;
            resultMessage = EInvoice_Authentication(loginUserId, access_token_OauthToken, sellerGstin, forceToGetToken, tblInvoiceTO.InvFromOrgId);
            if (resultMessage.Result != 1)
            {
                throw new Exception("Error in EInvoice_Authentication");
            }

            access_token_Authentication = resultMessage.Tag.ToString();
            if (access_token_Authentication == null)
            {
                throw new Exception("access_token_Authentication is null");
            }

            return EInvoice_EWayBill(tblInvoiceTO, loginUserId, access_token_Authentication, sellerGstin);
        }

        public static ResultMessage EInvoice_EWayBill(TblInvoiceTO tblInvoiceTO, Int32 loginUserId, string access_token_Authentication, string sellerGstin)
        {
            ResultMessage resultMsg = new ResultMessage();
            if (access_token_Authentication == "")
            {
                resultMsg = EInvoice_Authentication(loginUserId, "", sellerGstin, false, tblInvoiceTO.InvFromOrgId);
                if (resultMsg.Result != 1)
                {
                    throw new Exception("Error in EInvoice_Authentication");
                }
                else
                {
                    access_token_Authentication = resultMsg.Tag.ToString();
                }
            }

            TblEInvoiceApiTO tblEInvoiceApiTO = GetTblEInvoiceApiTO((int)EInvoiceAPIE.GENERATE_EWAYBILL);
            if (tblEInvoiceApiTO == null)
            {
                throw new Exception("EInvoiceApiTO is null");
            }

            tblEInvoiceApiTO.HeaderParam = tblEInvoiceApiTO.HeaderParam.Replace("@gstin", sellerGstin);
            tblEInvoiceApiTO.HeaderParam = tblEInvoiceApiTO.HeaderParam.Replace("@token", access_token_Authentication);

            tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@IrnNo", tblInvoiceTO.IrnNo);
            tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@VehNo", GetValidVehichleNumber(tblInvoiceTO.VehicleNo));
            tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@DistanceinKm", tblInvoiceTO.DistanceInKM.ToString());

            IRestResponse response = CallRestAPIs(tblEInvoiceApiTO.ApiBaseUri + tblEInvoiceApiTO.ApiFunctionName, tblEInvoiceApiTO.ApiMethod, tblEInvoiceApiTO.HeaderParam, tblEInvoiceApiTO.BodyParam);

            string EwayBillNo = null;
            JObject json = JObject.Parse(response.Content);
            if (json.ContainsKey("data"))
            {
                JObject jsonData = JObject.Parse(json["data"].ToString());
                EwayBillNo = (string)jsonData["EwbNo"];
            }
            if (json.ContainsKey("error"))
            {
                JArray arrError = JArray.Parse(json["error"].ToString());
                foreach (var err in arrError)
                {
                    JObject jsonError = JObject.Parse(err.ToString());
                    string errorCodes = (string)jsonError["errorCodes"];
                    string errorMsg = (string)jsonError["errorMsg"];
                    if (errorCodes == "1005" && errorMsg == "Invalid Token")
                    {
                        GenerateEWayBill(loginUserId, tblInvoiceTO.IdInvoice, tblInvoiceTO.DistanceInKM, true);
                        return null;
                    }
                }
            }


            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                resultMsg = InsertIntoTblEInvoiceApiResponse(tblEInvoiceApiTO.IdApi, tblInvoiceTO.IdInvoice, response, loginUserId, conn, tran);
                if (resultMsg.Result != 1)
                {
                    tran.Rollback();
                    return resultMsg;
                }

                if (EwayBillNo == null)
                {
                    tran.Commit();
                    resultMsg.DefaultBehaviour(json.ToString());
                    resultMsg.DisplayMessage = json.ToString();
                    resultMsg.Text = resultMsg.DisplayMessage;
                    return resultMsg;
                }

                tblInvoiceTO.ElectronicRefNo = EwayBillNo;
                tblInvoiceTO.IsEWayBillGenerated = 1;
                tblInvoiceTO.UpdatedBy = Convert.ToInt32(loginUserId);

                resultMsg = UpdateTempInvoiceForEWayBill(tblInvoiceTO, conn, tran);
                if (resultMsg.Result != 1)
                {
                    tran.Rollback();
                    return resultMsg;
                }

                resultMsg.DisplayMessage = "eWayBill generated successfully;";
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMsg.DefaultExceptionBehaviour(ex, "EInvoice_EWayBill");
            }
            finally
            {
                conn.Close();
            }
            return resultMsg;
        }

        /// <summary>
        /// Dhananjay[18-11-2020] : Added To Cancel ewaybill.
        /// </summary>
        public static ResultMessage CancelEWayBill(Int32 loginUserId, Int32 idInvoice, bool forceToGetToken = false)
        {
            ResultMessage resultMessage = new ResultMessage();
            string sellerGstin = "27AALFP1139Q004";

            TblInvoiceTO tblInvoiceTO = new TblInvoiceTO();
            tblInvoiceTO = SelectTblInvoiceTO(idInvoice);
            if (tblInvoiceTO == null)
            {
                throw new Exception("InvoiceTO is null");
            }

            if (tblInvoiceTO.IsEInvGenerated != 1)
            {
                resultMessage.Text = "EInvoice is not generated for this invoice.";
                resultMessage.DisplayMessage = "EInvoice is not generated for this invoice.";
                resultMessage.Result = 0;
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;
            }

            List<TblOrgLicenseDtlTO> TblOrgLicenseDtlTOList = TblOrgLicenseDtlBL.SelectAllTblOrgLicenseDtlList(tblInvoiceTO.InvFromOrgId);
            if (TblOrgLicenseDtlTOList != null)
            {
                for (int i = 0; i <= TblOrgLicenseDtlTOList.Count - 1; i++)
                {
                    if (TblOrgLicenseDtlTOList[i].LicenseId == (Int32)CommercialLicenseE.IGST_NO)
                    {
                        sellerGstin = TblOrgLicenseDtlTOList[i].LicenseValue.ToUpper();
                        break;
                    }
                }
            }

            string access_token_OauthToken = null;
            resultMessage = EInvoice_OauthToken(loginUserId, sellerGstin, forceToGetToken, tblInvoiceTO.InvFromOrgId);
            if (resultMessage.Result != 1)
            {
                throw new Exception("Error in EInvoice_OauthToken");
            }

            access_token_OauthToken = resultMessage.Tag.ToString();
            if (access_token_OauthToken == null)
            {
                throw new Exception("access_token_OauthToken is null");
            }

            string access_token_Authentication = null;
            resultMessage = EInvoice_Authentication(loginUserId, access_token_OauthToken, sellerGstin, forceToGetToken, tblInvoiceTO.InvFromOrgId);
            if (resultMessage.Result != 1)
            {
                throw new Exception("Error in EInvoice_Authentication");
            }

            access_token_Authentication = resultMessage.Tag.ToString();
            if (access_token_Authentication == null)
            {
                throw new Exception("access_token_Authentication is null");
            }

            return EInvoice_CancelEWayBill(tblInvoiceTO, loginUserId, access_token_Authentication, sellerGstin);
        }

        public static ResultMessage EInvoice_CancelEWayBill(TblInvoiceTO tblInvoiceTO, Int32 loginUserId, string access_token_Authentication, string sellerGstin)
        {
            ResultMessage resultMsg = new ResultMessage();
            if (access_token_Authentication == "")
            {
                resultMsg = EInvoice_Authentication(loginUserId, "", sellerGstin, false, tblInvoiceTO.InvFromOrgId);
                if (resultMsg.Result != 1)
                {
                    throw new Exception("Error in EInvoice_Authentication");
                }
                else
                {
                    access_token_Authentication = resultMsg.Tag.ToString();
                }
            }

            TblEInvoiceApiTO tblEInvoiceApiTO = GetTblEInvoiceApiTO((int)EInvoiceAPIE.CANCEL_EWAYBILL);
            if (tblEInvoiceApiTO == null)
            {
                throw new Exception("EInvoiceApiTO is null");
            }

            tblEInvoiceApiTO.HeaderParam = tblEInvoiceApiTO.HeaderParam.Replace("@gstin", sellerGstin);
            tblEInvoiceApiTO.HeaderParam = tblEInvoiceApiTO.HeaderParam.Replace("@token", access_token_Authentication);

            tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@irnNo", tblInvoiceTO.IrnNo);
            tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@ewbNo", tblInvoiceTO.ElectronicRefNo);

            IRestResponse response = CallRestAPIs(tblEInvoiceApiTO.ApiBaseUri + tblEInvoiceApiTO.ApiFunctionName, tblEInvoiceApiTO.ApiMethod, tblEInvoiceApiTO.HeaderParam, tblEInvoiceApiTO.BodyParam);

            string EwayBillNo = null;
            bool bPeriodLapsed = false;
            JObject json = JObject.Parse(response.Content);
            if (json.ContainsKey("data"))
            {
                JObject jsonData = JObject.Parse(json["data"].ToString());
                EwayBillNo = (string)jsonData["ewayBillNo"];
            }
            if (json.ContainsKey("error"))
            {
                JArray arrError = JArray.Parse(json["error"].ToString());
                foreach (var err in arrError)
                {
                    JObject jsonError = JObject.Parse(err.ToString());
                    string errorCodes = (string)jsonError["errorCodes"];
                    string errorMsg = (string)jsonError["errorMsg"];
                    if (errorCodes == "1005" && errorMsg == "Invalid Token")
                    {
                        CancelEWayBill(loginUserId, tblInvoiceTO.IdInvoice, true);
                        return null;
                    }
                    else if (errorCodes == "315")
                    {
                        bPeriodLapsed = true;
                    }
                }
            }

            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                resultMsg = InsertIntoTblEInvoiceApiResponse(tblEInvoiceApiTO.IdApi, tblInvoiceTO.IdInvoice, response, loginUserId, conn, tran);

                if (resultMsg.Result != 1)
                {
                    tran.Rollback();
                    return resultMsg;
                }

                if (bPeriodLapsed == true)
                {
                    tran.Commit();
                    resultMsg.DefaultBehaviour(json.ToString());
                    resultMsg.DisplayMessage = "You cannot cancel eWayBill after 24 hrs of generation.";
                    resultMsg.Text = resultMsg.DisplayMessage;
                    return resultMsg;
                }
                if (EwayBillNo == null)
                {
                    tran.Commit();
                    resultMsg.DefaultBehaviour(json.ToString());
                    resultMsg.DisplayMessage = json.ToString();
                    resultMsg.Text = resultMsg.DisplayMessage;
                    return resultMsg;
                }

                tblInvoiceTO.ElectronicRefNo = "";
                tblInvoiceTO.IsEWayBillGenerated = 0;
                tblInvoiceTO.UpdatedBy = Convert.ToInt32(loginUserId);
                resultMsg = UpdateTempInvoiceForEWayBill(tblInvoiceTO, conn, tran);

                if (resultMsg.Result != 1)
                {
                    tran.Rollback();
                    return resultMsg;
                }

                resultMsg.DisplayMessage = "eWayBill cancelled successfully;";
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMsg.DefaultExceptionBehaviour(ex, "EInvoice_CancelEwayBill");
            }
            finally
            {
                conn.Close();
            }
            return resultMsg;
        }

        public static ResultMessage UpdateInvoiceAddress(List<TblInvoiceAddressTO> tblInvoiceAddressTOList)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            //String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                int result = 0;
                if (tblInvoiceAddressTOList != null && tblInvoiceAddressTOList.Count > 0)
                {
                    for (int i = 0; i < tblInvoiceAddressTOList.Count; i++)
                    {
                        result = TblInvoiceAddressBL.UpdateTblInvoiceAddress(tblInvoiceAddressTOList[i], conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error in Insert UpdateTblInvoiceAddress");
                            return resultMessage;
                        }
                    }
                }
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

    }
}
