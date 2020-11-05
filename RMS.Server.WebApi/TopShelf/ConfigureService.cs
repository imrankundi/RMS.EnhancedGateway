using System;
using Topshelf;

namespace RMS.Server.WebApi
{
    internal static class ConfigureService
    {
        static readonly string className = nameof(ConfigureService);
        static readonly string serviceName = "SalTec RMS Enhanced Gateway Service";
        static readonly string displayName = "SalTec RMS Enhanced Gateway Service";
        static readonly string description = "SalTec RMS Enhanced Gateway Service";
        internal static void Configure()
        {
            try
            {

                HostFactory.Run(configure =>
                {
                    configure.Service<WebServer>(service =>
                    {
                        service.ConstructUsing(s => new WebServer());
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
