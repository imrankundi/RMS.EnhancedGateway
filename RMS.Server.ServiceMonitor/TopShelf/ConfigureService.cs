using System;
using Topshelf;

namespace RMS.Server.ServiceMonitor
{
    internal static class ConfigureService
    {
        private static readonly string className = nameof(ConfigureService);
        static readonly string serviceName = "SalTec RMS Service Monitor Service";
        static readonly string displayName = "SalTec RMS Service Monitor Service";
        static readonly string description = "SalTec RMS Service Monitor Service";
        internal static void Configure()
        {
            try
            {

                HostFactory.Run(configure =>
                {
                    configure.Service<ServiceMonitorService>(service =>
                    {
                        service.ConstructUsing(s => new ServiceMonitorService());
                        service.WhenStarted(s => s.Start());
                        service.WhenStopped(s => s.Stop());
                    });

                    //Setup Account that window service use to run.  
                    configure.RunAsLocalSystem();
                    configure.SetServiceName(serviceName);
                    configure.SetDisplayName(displayName);
                    configure.SetDescription(description);
                });
            }
            catch (Exception)
            {
            }

        }
    }
}
