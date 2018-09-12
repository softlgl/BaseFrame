using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using BaseFrame.Common.Assembly;

namespace BaseFrame.WebApi
{
    public class ApiAutofacRegisterConfig
    {
        public static void BuildConfig()
        {
            ContainerBuilder builder = new ContainerBuilder();
            HttpConfiguration config = GlobalConfiguration.Configuration;

            Assembly[] asmService = AssemblyHelper.GetAllAssembly("*.DAL.dll").ToArray();
            builder.RegisterAssemblyTypes(asmService)
                .Where(w => w.Name.EndsWith("DAL"))
                .PropertiesAutowired()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .SingleInstance();

            asmService = AssemblyHelper.GetAllAssembly("*.Service.dll").ToArray();
            builder.RegisterAssemblyTypes(asmService)
                .Where(w => w.Name.EndsWith("Service"))
                .PropertiesAutowired()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .SingleInstance();

            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterApiControllers(assembly)
            .Where(w => w.Name.EndsWith("Controller"))
            .PropertiesAutowired();

            var container = builder.Build();
            //注册api容器需要使用HttpConfiguration对象
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}