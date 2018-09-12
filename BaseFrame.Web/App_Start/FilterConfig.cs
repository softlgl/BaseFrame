using System.Web;
using System.Web.Mvc;
using BaseFrame.Web.Filter;

namespace BaseFrame.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //全局异常处理
            filters.Add(new ExceptionAttribute());
        }
    }
}
