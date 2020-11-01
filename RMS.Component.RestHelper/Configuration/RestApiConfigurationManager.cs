using RMS.Component.Configuration;

namespace RMS.Component.RestHelper.Configuration
{
    public class RestApiConfigurationManager
    {
        private const string fileName = nameof(RestApiConfiguration);
        private const string directory = "Configuration";
        private RestApiConfigurationManager()
        {
            Configurations = LoadConfiguration();
            if (Configurations == null)
                IsConfigurationLoaded = false;
            else
                IsConfigurationLoaded = true;
        }
        static RestApiConfigurationManager() { }
        public static RestApiConfigurationManager Instance { get; } = new RestApiConfigurationManager();
        public bool IsConfigurationLoaded { get; } = false;
        public RestApiConfiguration Configurations { get; private set; }
        private RestApiConfiguration LoadConfiguration()
        {
            var configurtionLoader = new ConfigurtionLoader<RestApiConfiguration>(fileName, directory);
            var configuration = configurtionLoader.LoadConfiguration();
            return configuration;
        }
    }
}
