using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.IoT
{
    public class NodeJsResult
    {
        string msg;
        int code;
        List<int[]> data;


        public string Msg { get => msg; set => msg = value; }
        public int Code { get => code; set => code = value; }
        public List<int[]> Data { get => data; set => data = value; }

        public void DefaultErrorBehavior(int CodeNumber, string MsgString)
        {
            Code = CodeNumber;
            Msg = MsgString;
        }
        public void DefaultSuccessBehavior(int CodeNumber, string MsgString, List<int[]> Resultdata)
        {
            Code = CodeNumber;
            Msg = MsgString;
            data = Resultdata;
        }
    }

    public class GateIoTResult
    {
        string msg;
        int code;
        List<object[]> data = new List<object[]>();

        public string Msg { get => msg; set => msg = value; }
        public int Code { get => code; set => code = value; }
        public List<object[]> Data { get => data; set => data = value; }

        public void DefaultErrorBehavior(int CodeNumber, string MsgString)
        {
            Code = CodeNumber;
            Msg = MsgString;
        }
        public void DefaultSuccessBehavior(int CodeNumber, string MsgString, List<object[]> Resultdata)
        {
            Code = CodeNumber;
            Msg = MsgString;
            data = Resultdata;
        }
    }
}
