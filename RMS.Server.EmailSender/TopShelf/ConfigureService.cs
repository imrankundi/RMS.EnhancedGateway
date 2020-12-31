using System;
using Topshelf;

namespace RMS.Server.EmailSender
{
    internal static class ConfigureService
    {
        static readonly string className = nameof(ConfigureService);
        static readonly string serviceName = "SalTec RMS Email Sender Service";
        static readonly string displayName = "SalTec RMS Email Sender Service";
        static readonly string description = "SalTec RMS Email Sender Service";
        internal static void Configure()
        {
            try
            {

                HostFactory.Run(configure =>
                {
                    configure.Service<EmailSenderService>(service =>
                    {
                        service.ConstructUsing(s => new EmailSenderService());
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
