using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{

    public class ResponseWrapper<T> : BaseResponseWrapper
    {
        public T Body { get; set; }
        public ResponseWrapper()
        {

        }
        public ResponseWrapper(T body, String msg)
        {
            StatusCode = HttpStatusCode.OK;
            Body = body;
            Message = msg;
        }
        public ResponseWrapper(T body, String msg, HttpStatusCode code,dynamic data)
        {
            StatusCode = code;
            Body = body;
            Message = msg;
            DyanamicData = data;
        }
        public List<DynamicReportTO> DyanamicData { get; set; }
    }
    public class BaseResponseWrapper
    {
        public HttpStatusCode StatusCode { get; set; }
        public String Message { get; set; }
        public BaseResponseWrapper()
        {
        }
        public BaseResponseWrapper(string msg)
        {
            StatusCode = HttpStatusCode.NotFound;
            Message = msg;
        }
        public BaseResponseWrapper(HttpStatusCode code, string msg)
        {
            StatusCode = code;
            Message = msg;
        }
    }
}
