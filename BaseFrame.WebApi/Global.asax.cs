using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using BaseFrame.Common.log4net;

namespace BaseFrame.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            //初始化log4net
            Log4Helper.LogConfig(Server.MapPath(@"~/Log4Net.config"));

            // 使api返回为json 
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            //Autofac
            ApiAutofacRegisterConfig.BuildConfig();
        }
    }
}
