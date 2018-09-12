using BaseFrame.Common.log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BaseFrame.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            //隐藏mvc版本标头出现
            MvcHandler.DisableMvcResponseHeader = true;

            //初始化log4net
            Log4Helper.LogConfig(Server.MapPath(@"~/Log4Net.config"));

            //Autpfac IOC 控制反转
            AutofacRegisterConfig.BuildConfig();
        }
    }
}
