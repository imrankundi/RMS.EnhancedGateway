using RMS.Component.Configuration;

namespace RMS.Simulator.Configuration
{
    public class SimulatorConfigManager
    {
        private const string fileName = nameof(SimulatorConfig);
        private const string directory = "Configuration";
        private SimulatorConfigManager()
        {
            Configurations = LoadConfiguration();
            if (Configurations == null)
                IsConfigurationLoaded = false;
            else
                IsConfigurationLoaded = true;
        }
        static SimulatorConfigManager() { }
        public static SimulatorConfigManager Instance { get; } = new SimulatorConfigManager();
        public bool IsConfigurationLoaded { get; } = false;
        public SimulatorConfig Configurations { get; private set; }
        private SimulatorConfig LoadConfiguration()
        {
            var configurtionLoader = new ConfigurtionLoader<SimulatorConfig>(fileName, directory);
            var configuration = configurtionLoader.LoadConfiguration();
            return configuration;
        }
    }
}
