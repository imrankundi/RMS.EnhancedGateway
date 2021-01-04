using RMS.Component.Logging;
using RMS.Server.DataTypes.WindowsService;
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
        private IEnumerable<ServiceInfo> serviceInfo;
        private ILog log;

        public WindowsServiceInfoManager(ILog log, IEnumerable<ServiceInfo> serviceInfo)
        {
            this.serviceInfo = serviceInfo;
            this.log = log;
            PopulateDictionary();
            PopulateList();
        }
        private void PopulateDictionary()
        {
            foreach (var info in serviceInfo)
            {
                services.Add(info.ServiceName, new ServiceControllerContainer
                {
                    InstallationStatus = ServiceInstallationStatus.NotInstalled,
                    ServiceInfo = info
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
                    foreach (var info in serviceInfo)
                    {
                        if (!string.IsNullOrEmpty(info.ServiceName))
                        {
                            try
                            {
                                if (s.ServiceName == info.ServiceName)
                                {
                                    if (services.ContainsKey(info.ServiceName))
                                    {
                                        var container = services[info.ServiceName];
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
