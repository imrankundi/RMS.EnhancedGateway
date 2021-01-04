using System;
using System.ServiceProcess;

namespace RMS.Server.ServiceMonitor
{
    public class ServiceControllerContainer
    {
        public ServiceController ServiceController { get; set; }
        public ServiceInstallationStatus InstallationStatus { get; set; }
        public string ServiceName
        {
            get
            {
                if (InstallationStatus == ServiceInstallationStatus.NotInstalled)
                    return string.Empty;

                try
                {
                    return ServiceController.ServiceName;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }
        public ServiceStatus ServiceStatus
        {
            get
            {
                try
                {
                    if (InstallationStatus == ServiceInstallationStatus.NotInstalled)
                        return ServiceStatus.Unknown;

                    ServiceController.Refresh();
                    switch (ServiceController.Status)
                    {
                        case ServiceControllerStatus.Running:
                            return ServiceStatus.Running;
                        default:
                            return ServiceStatus.Stopped;
                    }
                }
                catch (Exception)
                {
                    return ServiceStatus.Stopped;
                }
            }
        }
    }
}
