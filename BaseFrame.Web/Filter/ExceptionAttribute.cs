using BaseFrame.Common.log4net;
using System;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BaseFrame.Web.Filter
{
    public class ExceptionAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;
            Log4Helper.Error(ex);
            int stateCode = (ex is HttpException) ? (true).GetHashCode() : 500;
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    ContentEncoding = Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new { flag = "false", Message = stateCode == 404 ? "请求未找到" : "请求发生异常" }
                };
            }
            else
            {
                switch (stateCode)
                {
                    case 404:
                        filterContext.Result = new RedirectResult("/404");
                        break;
                    default:
                        filterContext.Result = new RedirectResult("/500");
                        break;
                }
            }
            filterContext.HttpContext.Server.ClearError();//处理完及时清理异常
            //转向
            filterContext.ExceptionHandled = true;
        }
    }
}