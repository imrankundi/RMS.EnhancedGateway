using System;
using Topshelf;

namespace RMS.Server.WebApi
{
    internal static class ConfigureService
    {
        static readonly string className = nameof(ConfigureService);
        internal static void Configure()
        {
            try
            {
                var config = ServiceSettingsManager.Instance.Configurations;
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
                    configure.SetServiceName(config.ServiceName);
                    configure.SetDisplayName(config.DisplayName);
                    configure.SetDescription(config.Description);
                    configure.SetInstanceName(config.InstanceName);
                });
            }
            catch (Exception)
            {
            }

        }
    }
}
