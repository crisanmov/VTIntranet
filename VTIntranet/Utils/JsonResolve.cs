using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static VTIntranet.Utils.JsonResolve;

namespace VTIntranet.Utils
{

    public class JsonResolve
    {

        [Serializable]
        public class JsonResultData
        {
            public string status { get; set; }
            public string message { get; set; }
            public string stackTrace { get; set; }
            public object data { get; set; }
            public bool success { get; set; }
        }
    }


    public class JsonSerialResponse
    {
        private const string STATUS_OK = "0";
        private const string STATUS_ERROR = "1";
        private const string REGISTER_NOT_FOUND = "3";

        public static JsonResultData ResultSuccess(object data, string message = null)
        {
            var apiResult = new JsonResultData();
            apiResult.status = STATUS_OK;
            apiResult.message = message;
            apiResult.data = data;
            apiResult.success = true;
            return apiResult;
        }

        public static JsonResultData ResultError(string message, string stackTrace = null)
        {
            var apiResult = new JsonResultData();
            apiResult.status = STATUS_ERROR;
            apiResult.message = message;
            apiResult.stackTrace = stackTrace;
            apiResult.success = false;
            return apiResult;
        }
        public static JsonResultData ResultRegisterNotFound(string message)
        {
            var apiResult = new JsonResultData();
            apiResult.status = REGISTER_NOT_FOUND;
            apiResult.message = message;
            apiResult.success = false;
            return apiResult;
        }


        public static string SuccessResultOK(object data, string message = null)
        {
            var apiResult = new JsonResultData();
            apiResult.status = STATUS_OK;
            apiResult.message = message;
            apiResult.data = data;
            apiResult.success = true;
            return apiResult.ToString();
        }

        public static string ErrorResultOK(string message)
        {
            var apiResult = new JsonResultData();
            apiResult.status = STATUS_ERROR;
            apiResult.message = message;
            apiResult.success = false;
            return apiResult.ToString();
        }

    }
}