using Autofac;
using BaseFrame.Common.Assembly;

namespace BaseFrame.Common.Autofac
{
    public  class AutofacContainer
    {
        private static ContainerBuilder Container = new ContainerBuilder();
        static IContainer build = null;
        static AutofacContainer()
        {
            System.Reflection.Assembly[] asmDal = AssemblyHelper.GetAllAssembly("*.DAL.dll").ToArray();
            Container.RegisterAssemblyTypes(asmDal)
                .Where(w => w.Name.EndsWith("DAL"))
                .PropertiesAutowired()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .SingleInstance();
            
            System.Reflection.Assembly[] asmService = AssemblyHelper.GetAllAssembly("*.Service.dll").ToArray();
            Container.RegisterAssemblyTypes(asmService)
                .Where(w => w.Name.EndsWith("Service"))
                .PropertiesAutowired()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .SingleInstance();

            System.Reflection.Assembly[] asmBLL = AssemblyHelper.GetAllAssembly("*.BLL.dll").ToArray();
            Container.RegisterAssemblyTypes(asmBLL)
                .Where(w => w.Name.EndsWith("BLL"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .SingleInstance();

            build = Container.Build();

        }
        public static T Resolve<T>()
        {
            var res= build.IsRegistered<T>() ? build.Resolve<T>() : default(T);
            return res;
        }
        
    }
}
