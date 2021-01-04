using RMS.Component.DataAccess.SQLite.Repositories;
using RMS.Server.DataTypes;

namespace RMS.Server.ServiceMonitor
{
    public class ServiceMonitorConfigurationManager
    {
        string className = nameof(ServiceMonitorConfigurationManager);
        private ServiceMonitorConfigurationManager()
        {
            Configurations = LoadConfiguration();
            if (Configurations == null)
                IsConfigurationLoaded = false;
            else
                IsConfigurationLoaded = true;



        }
        static ServiceMonitorConfigurationManager() { }
        public static ServiceMonitorConfigurationManager Instance { get; } = new ServiceMonitorConfigurationManager();
        public bool IsConfigurationLoaded { get; } = false;
        public ServiceMonitorServiceConfiguration Configurations { get; private set; }
        private ServiceMonitorServiceConfiguration LoadConfiguration()
        {
            var repo = new ServiceMonitorConfigRepository();
            var config = repo.ReadServiceMonitorConfiguration();
            var configuration = Component.Mappers.ConfigurationMapper.Map(config);
            return configuration;
        }
    }
}
