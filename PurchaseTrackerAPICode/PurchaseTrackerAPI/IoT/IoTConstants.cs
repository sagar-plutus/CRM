using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using System.Linq;

namespace PurchaseTrackerAPI.IoT
{
    public class IoTConstants
    {

        public  int TimeOutMilliSeconds = 3000;
        public enum WeightIotColE
        {
            LoadingId = 0,
            LayerId = 1,
            ItemRefNo = 2,
            WeighTypeId = 3,
            GrossWeight = 4,
            UnLoadedWeight = 5,
            NetWeight = 6,
            TimeStamp = 7,
            LoadedBundle = 8
        }

        public enum GateIoTColE
        {
            LoadingId = 0,
            VehicleNo = 1,
            StatusId = 2,
            StatusDate = 3,
            TransportorId = 4
        }
    }

}
