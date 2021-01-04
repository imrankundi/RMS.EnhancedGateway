using RMS.Component.Logging;
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

                var list = GetServiceNames();

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

        private IEnumerable<string> GetServiceNames()
        {
            var parameters = ServiceMonitorConfigurationManager.Instance.Configurations.Parameters;

            foreach (var parameter in parameters)
                yield return parameter.ServiceName;
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
                    Console.WriteLine("Service: {0}, Status: {1}, [{2}]", value.ServiceName, value.ServiceStatus, value.InstallationStatus);
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

        public void Stop()
        {


        }
    }
}
