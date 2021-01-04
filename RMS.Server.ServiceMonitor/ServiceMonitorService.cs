using Newtonsoft.Json;
using RMS.Component.Common.Helpers;
using RMS.Component.DataAccess.SQLite.Entities;
using RMS.Component.DataAccess.SQLite.Repositories;
using RMS.Component.Logging;
using RMS.Server.DataTypes.Email;
using RMS.Server.DataTypes.WindowsService;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Timers;

namespace RMS.Server.ServiceMonitor
{
    public class ServiceMonitorService
    {
        string className = nameof(ServiceMonitorService);
        ILog log;
        Timer timer;
        WindowsServiceInfoManager serviceManager;
        public void Start()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            try
            {
                log = LoggingFactory.CreateLogger(ServiceMonitorConfigurationManager.Instance.Configurations.LogPath,
                    "ServiceMonitor", ServiceMonitorConfigurationManager.Instance.Configurations.LogLevel);

                var list = GetServiceInfo();

                serviceManager = new WindowsServiceInfoManager(log, list);
                timer = new Timer();
                timer.Interval = ServiceMonitorConfigurationManager.Instance.Configurations.IntervalInSeconds * 1000;
                timer.Elapsed += Timer_Elapsed;

                timer.Start();
            }
            catch (Exception ex)
            {
                log?.Error(className, methodName, ex.ToString());
            }
        }

        private IEnumerable<ServiceInfo> GetServiceInfo()
        {
            var parameters = ServiceMonitorConfigurationManager.Instance.Configurations.Parameters;

            foreach (var parameter in parameters)
                yield return new ServiceInfo
                {
                    Id = parameter.Id,
                    ServiceName = parameter.ServiceName,
                    ServiceStatus = (ServiceStatus)parameter.ServiceState
                };
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            try
            {
                //log?.Verbose(className, methodName, "Sending Email");
                Console.WriteLine("Timer Tick...");
                foreach (var kv in serviceManager.Servies)
                {
                    var key = kv.Key;
                    ServiceControllerContainer value = kv.Value;
                    if (value.InstallationStatus == ServiceInstallationStatus.Installed)
                    {
                        if (value.ServiceInfo.ServiceStatus != value.ServiceStatus)
                        {
                            Console.WriteLine("Service: {0}, Status: {1}, [{2}]", value.ServiceName, value.ServiceStatus, value.InstallationStatus);
                            value.ServiceInfo.ServiceStatus = value.ServiceStatus;
                            var repo = new ServiceMonitorConfigRepository();
                            repo.Update(new MontioringParameterConfig
                            {
                                Id = value.ServiceInfo.Id,
                                ServiceState = (int)value.ServiceInfo.ServiceStatus,
                                LastStateChange = DateTime.UtcNow
                            });

                            GenerateEmail(value);
                        }

                    }

                }

                timer.Enabled = false;

            }
            catch (Exception ex)
            {
                log?.Error(className, methodName, ex.ToString());
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                timer.Enabled = true;
            }
        }

        private void GenerateEmail(ServiceControllerContainer value)
        {
            EmailTemplate template = new EmailTemplate
            {
                ToEmailAddresses = new List<string>
                {
                    "imrankundi@hotmail.com" 
                },
                BccEmailAddresses = new List<string>
                {
                    "imrankundi@hotmail.com",
                    "kundi.imranullah@gmail.com"
                },
                EmailMessage = string.Format("Service Name: {0}\nServiceState: {1}", value.ServiceName, value.ServiceStatus),
                EmailSubject = "Service Status",
                IsHtml = false
            };

            var json = JsonConvert.SerializeObject(template);

            FileHelper.WriteAllText(@"C:\RMS\EmailService\Email\" + DateTime.Now.ToString("yyMMddHHmmss") + ".json", json);
        }

        public void Stop()
        {


        }
    }
}
