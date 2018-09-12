using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http.Controllers;

namespace YSWL.WMS.WebApi.Common
{
    public class HttpHelper
    {
        /// <summary>
        /// 获取post参数
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetPostData(HttpActionContext actionContext, string key)
        {
            HttpContextBase context = (HttpContextBase)actionContext.Request.Properties["MS_HttpContext"];//获取传统context     
            HttpRequestBase request = context.Request;//定义传统request对象
            return request.Form[key];
        }

        /// <summary>
        /// 获取url参数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetQueryStrings(HttpRequestMessage request)
        {
            return request.GetQueryNameValuePairs()
                          .ToDictionary(kv => kv.Key, kv => kv.Value,
                               StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 获取cookie
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetCookieValue(HttpRequestMessage request, string key)
        {
            CookieHeaderValue cookie = request.Headers.GetCookies(key).FirstOrDefault();
            return cookie?[key].Value;
        }

        /// <summary>
        /// 获取头文件信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetHeaderValue(HttpRequestMessage request, string key)
        {
            var header = request.Headers;
            if (header.Contains(key))
            {
                IEnumerable<string> enterprise = header.GetValues(key);
                string[] enumerable = enterprise as string[] ?? enterprise.ToArray();
                if (enumerable.Any())
                {
                   return enumerable.FirstOrDefault();
                }
            }
            return null;
        }
    }
}