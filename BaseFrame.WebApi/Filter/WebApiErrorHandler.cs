using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using BaseFrame.Common.log4net;
using BaseFrame.WebApi.Models;

namespace BaseFrame.WebApi.Filter
{
    /// <summary>
    /// WebApi错误处理
    /// </summary>
    public class WebApiErrorHandlerAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(HttpActionExecutedContext context)
        {
            var ex = context.Exception;
            var responseMsg = new ResponseResult { Status = ResultStatus.Error, Result = new FailResult { Code = ResponseCode.InternalServerError,Msg = ex.Message} };
            Log4Helper.Error(ex);
            // 返回http返回信息
            context.Response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, responseMsg);
        }
    }
}