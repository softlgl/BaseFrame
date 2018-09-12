using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BaseFrame.WebApi.Filter
{
    public class WebApiAuthFilter : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.ActionDescriptor.GetCustomAttributes<CustomAllowAnonyAttribute>().Any()
                ||actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<CustomAllowAnonyAttribute>().Any())
                return;
        }
    }
}