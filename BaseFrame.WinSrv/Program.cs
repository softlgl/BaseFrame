using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace BaseFrame.WinSrv
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<ServiceRunner>();

                x.SetDescription("服务描述");
                x.SetDisplayName("展示名称");
                x.SetServiceName("服务名称");

                x.EnablePauseAndContinue();
            });
        }
    }
}
