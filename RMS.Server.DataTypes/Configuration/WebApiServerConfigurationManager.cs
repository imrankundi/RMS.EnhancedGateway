using RMS.Component.Configuration;

namespace RMS.Server.WebApi.Configuration
{
    public class WebApiServerConfigurationManager
    {
        private const string fileName = "WebApiServerConfiguration";
        private const string directory = "Configuration";
        private WebApiServerConfigurationManager()
        {
            Configurations = LoadConfiguration();
            if (Configurations == null)
                IsConfigurationLoaded = false;
            else
                IsConfigurationLoaded = true;
        }
        static WebApiServerConfigurationManager() { }
        public static WebApiServerConfigurationManager Instance { get; } = new WebApiServerConfigurationManager();
        public bool IsConfigurationLoaded { get; } = false;
        public WebApiServerConfiguration Configurations { get; private set; }
        private WebApiServerConfiguration LoadConfiguration()
        {
            var configurtionLoader = new ConfigurtionLoader<WebApiServerConfiguration>(fileName, directory);
            var configuration = configurtionLoader.LoadConfiguration();
            return configuration;
        }
    }
}
