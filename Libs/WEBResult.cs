using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
namespace Chair80CP.Libs
{
    public class WEBResult<T> 
    {
        public bool isSuccess { get; set; }
        public ResponseCode code { get; set; }
        public string message { get; set; }
        public T data { get; set; }
        public WEBResult()
        {

        }


        public static WEBResult<T> Error(ResponseCode code, string message)
        {
            return new WEBResult<T>() { isSuccess = false, code = code, message = message };
        }

        public static WEBResult<T> Error(ResponseCode code, string message,T data)
        {
            return new WEBResult<T>() { isSuccess = false, code = code, message = message, data=data };
        }
        public static WEBResult<T> Success(T Data, string message="")
        {
            return new WEBResult<T>() { isSuccess = true, code = ResponseCode.Success, message =message,data=Data };
        }




    }

    /// <summary>
    /// 
    /// </summary>
    public enum ResponseCode
    {
        /// <summary>
        /// 
        /// </summary>
        Success=200,
        /// <summary>
        /// 
        /// </summary>
        UserForbidden=1403,
        /// <summary>
        /// 
        /// </summary>
        UserUnauthorized = 1401,
        /// <summary>
        /// 
        /// </summary>
        UserNotFound = 1404,
        /// <summary>
        /// 
        /// </summary>
        UserRequestTimeout = 1408,
        /// <summary>
        /// 
        /// </summary>
        UserNotAcceptable =1406,
        /// <summary>
        /// 
        /// </summary>
        UserUnVerified=1407,
        /// <summary>
        /// 
        /// </summary>
        UserValidationField=1408,
        /// <summary>
        /// 
        /// </summary>
        UserDoublicate=1409,


        /// <summary>
        /// 
        /// </summary>
        DevBadGeteway=2503,
        /// <summary>
        /// 
        /// </summary>
        DevNotFound = 2404,

        /// <summary>
        /// 
        /// </summary>
        BackendServerRequest = 3400,
        /// <summary>
        /// 
        /// </summary>
        BackendInternalServer =3500,
        /// <summary>
        /// 
        /// </summary>
        BackendDatabase=3600,

    }

  
}