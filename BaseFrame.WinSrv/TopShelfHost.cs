using Topshelf;

namespace BaseFrame.WinSrv
{
    public sealed class ServiceRunner : ServiceControl, ServiceSuspend
    {
        public ServiceRunner()
        {
            
        }

        public bool Start(HostControl hostControl)
        {
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            return true;
        }

        public bool Continue(HostControl hostControl)
        {
            return true;
        }

        public bool Pause(HostControl hostControl)
        {
            return true;
        }


    }
}
