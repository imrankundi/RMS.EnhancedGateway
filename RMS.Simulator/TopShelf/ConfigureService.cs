using System;
using Topshelf;

namespace RMS.Simulator
{
    internal static class ConfigureService
    {
        static readonly string className = nameof(SimulatorService);
        static readonly string serviceName = "RMS Simulator Server Service";
        static readonly string displayName = "RMS Simulator Server Service";
        static readonly string description = "RMS Simulator Server Service";
        internal static void Configure()
        {
            try
            {

                HostFactory.Run(configure =>
                {
                    configure.Service<SimulatorService>(service =>
                    {
                        service.ConstructUsing(s => new SimulatorService());
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
