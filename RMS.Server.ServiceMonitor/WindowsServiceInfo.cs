using RMS.Component.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceProcess;

namespace RMS.Server.ServiceMonitor
{


    public class WindowsServiceInfoManager
    {
        private string className = nameof(WindowsServiceInfoManager);
        private Dictionary<string, ServiceControllerContainer> services = new Dictionary<string, ServiceControllerContainer>();
        public Dictionary<string, ServiceControllerContainer> Servies => services;
        private IEnumerable<string> serviceNames;
        private ILog log;

        public WindowsServiceInfoManager(ILog log, IEnumerable<string> serviceNames)
        {
            this.serviceNames = serviceNames;
            this.log = log;
            PopulateDictionary();
            PopulateList();
        }
        private void PopulateDictionary()
        {
            foreach (var serviceName in serviceNames)
            {
                services.Add(serviceName, new ServiceControllerContainer
                {
                    InstallationStatus = ServiceInstallationStatus.NotInstalled
                });
            }
        }
        private void PopulateList()
        {
            var methodName = MethodBase.GetCurrentMethod().Name;
            try
            {
                IEnumerable<ServiceController> sc = ServiceController.GetServices();
                foreach (var s in sc)
                {
                    foreach (var serviceName in serviceNames)
                    {
                        if (!string.IsNullOrEmpty(serviceName))
                        {
                            try
                            {
                                if (s.ServiceName == serviceName)
                                {
                                    if (services.ContainsKey(serviceName))
                                    {
                                        var container = services[serviceName];
                                        container.ServiceController = s;
                                        container.InstallationStatus = ServiceInstallationStatus.Installed;
                                        continue;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                log?.Error(className, methodName, ex.ToString());
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                log?.Error(className, methodName, ex.ToString());
            }
        }
    }
}
