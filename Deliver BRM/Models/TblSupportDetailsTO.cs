using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class TblSupportDetailsTO
    {
        #region Declarations
        Int32 idsupport;
        Int32 moduleId;
        Int32 formid;
        Int32 fromUser;
        Int32 createdBy;
        DateTime createdOn;
        String description;
        Int32 requireTime;
        String comments;
        #endregion

        #region Constructor
        public TblSupportDetailsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 Idsupport
        {
            get { return idsupport; }
            set { idsupport = value; }
        }
        public Int32 ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }
        public Int32 Formid
        {
            get { return formid; }
            set { formid = value; }
        }
        public Int32 FromUser
        {
            get { return fromUser; }
            set { fromUser = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String Description
        {
            get { return description; }
            set { description = value; }
        }

        public int RequireTime { get => requireTime; set => requireTime = value; }
        public string Comments { get => comments; set => comments = value; }
        #endregion

    }

    public class IoTDetialsTo
    {
        List<object[]> gateDeatils = new List<object[]>();
        Dictionary<string, List<object[]>> weighingDeatils = new Dictionary<string, List<object[]>>();
        string apiUrl;
        DropDownTO dropDownTO;
        public List<object[]> GateDeatils { get => gateDeatils; set => gateDeatils = value; }
        public Dictionary<string, List<object[]>> WeighingDeatils { get => weighingDeatils; set => weighingDeatils = value; }
        public string ApiUrl { get => apiUrl; set => apiUrl = value; }
        public DropDownTO DropDownTO { get => dropDownTO; set => dropDownTO = value; }
    }

    public class GateIoTResultForIot
    {
        string msg;
        int code;
        // object[] data;
        List<object[]> data = new List<object[]>();

        public string Msg { get => msg; set => msg = value; }
        public int Code { get => code; set => code = value; }
        public List<object[]> Data { get => data; set => data = value; }

        // public object[] Data { get => data; set => data = value; }

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
