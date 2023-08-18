using PurchaseTrackerAPI.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.IoT.Interfaces
{
    public interface IGateCommunication
    {
        GateIoTResult DeleteAllLoadingFromGateIoT();

        GateIoTResult DeleteSingleLoadingFromGateIoT(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO);

        GateIoTResult GetAllLoadingSlipsByStatusFromIoT(Int32 statusId, TblGateTO tblGateTO, Int32 startLoadingId = 1);

        GateIoTResult GetLoadingSlipsByStatusFromIoT(Int32 statusId, TblGateTO tblGateTO, Int32 startLoadingId = 1);

        GateIoTResult GetLoadingStatusHistoryDataFromGateIoT(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO);

        int PostGateApiCalls(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, object[] writeData, HttpWebRequest tRequest);

        int PostGateAPIDataToModbusTcpApiForLoadingSlip(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, Object[] writeData);

        string ReadWeightFromWeightIoT(TblWeighingMachineTO machineTO);
    }
}
