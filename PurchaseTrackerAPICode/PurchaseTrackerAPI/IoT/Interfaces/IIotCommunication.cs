using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.IoT;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.IoT.Interfaces
{
    public interface IIotCommunication
    {
        TblPurchaseScheduleSummaryTO GetItemDataFromIotAndMerge(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO);

        //void GetItemDataFromIotForGivenLoadingSlip(TblLoadingSlipTO tblLoadingSlipTO);

        DateTime IoTDateTimeStringToDate(string statusDate);

        List<TblPurchaseScheduleSummaryTO> GetLoadingData(List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryList);

        //void GetWeighingMeasuresFromIoT(string loadingId, bool isUnloading, List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList, SqlConnection conn, SqlTransaction tran);

        int PostGateAPIDataToModbusTcpApi(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, Object[] writeData);

        int UpdateLoadingStatusOnGateAPIToModbusTcpApi(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, Object[] writeData);

        NodeJsResult DeleteSingleLoadingFromWeightIoTByModBusRefId(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, List<TblPurchaseWeighingStageSummaryTO> WeighingStageList);

        GateIoTResult GetLoadingSlipsByStatusFromIoTByStatusId(String statusIds, TblGateTO tblGateTO);

        List<int[]> GenerateFrameData(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, TblPurchaseWeighingStageSummaryTO weighingMeasureTo);

        List<object[]> GenerateGateIoTFrameData(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, string vehicleNo, Int32 statusId);

        List<object[]> GenerateGateIoTStatusFrameData(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, Int32 statusId);

        List<int[]> FormatIoTFrameToWrite(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, TblPurchaseWeighingStageSummaryTO weighingMeasureTo);

        Int32 GetDateToTimestap();

        void FormatStdGateIoTFrameToWrite(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, string vehicleNo, Int32 statusId, Int32 transportorId, List<object[]> frameList);

        void FormatStatusUpdateGateIoTFrameToWrite(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO, Int32 statusId, List<object[]> frameList);

        string GetIotEncodedStatusIdsForGivenStatus(string statusIds);

        string GetIotDecodedStatusIdsForGivenStatus(string statusIds);

        GateIoTResult GetDecryptedLoadingId(string dataFrame, string methodName, string URL);

        List<TblPurchaseScheduleSummaryTO> setGateDetailsFormIoT(string statusId, List<DimStatusTO> statusList, List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList);

        List<TblPurchaseWeighingStageSummaryTO> GetWeightDataFromIotAndMerge(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO,List<TblPurchaseWeighingStageSummaryTO> tblPurchaseWeighingStageSummaryList);
        List<TblPurchaseScheduleSummaryTO> GetItemDataFromIotAndMergeMulti(List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryList);
        List<TblPurchaseScheduleSummaryTO> phaseWiseFilterList(List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryList, int statusId, int phaseId);
        string GetVehicleNumbers(string vehicleNo, Boolean isForSelect);//chetan[10-feb-2020] added for allow Old vehicle on Iot
    }
}
