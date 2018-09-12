using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using BaseFrame.Common.Assembly;

namespace BaseFrame.Web
{
    public class AutofacRegisterConfig
    {
        public static void BuildConfig()
        {
            ContainerBuilder builder = new ContainerBuilder();
            //注册Module方法2 在Web.config中配制方式
            //builder.RegisterModule(new ConfigurationSettingsReader("autofacMvc"));
            //加载 *.Controllers 层的控制器,否则无法在其他层控制器构造注入，只能在web层注入

            //var dataAccess = Assembly.GetExecutingAssembly();

            Assembly[] asmDal = AssemblyHelper.GetAllAssembly("*.DAL.dll").ToArray();
            builder.RegisterAssemblyTypes(asmDal)
                .Where(w => w.Name.EndsWith("DAL"))
                .PropertiesAutowired()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .SingleInstance();

            Assembly[] asmService = AssemblyHelper.GetAllAssembly("*.Service.dll").ToArray();
            builder.RegisterAssemblyTypes(asmService)
                .Where(w => w.Name.EndsWith("Service"))
                .PropertiesAutowired()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .SingleInstance();

            //builder.RegisterControllers(Assembly.GetExecutingAssembly());
            //builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            //builder.RegisterModelBinderProvider();

            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterControllers(assembly).Where(w => w.Name.EndsWith("Controller")).PropertiesAutowired();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}