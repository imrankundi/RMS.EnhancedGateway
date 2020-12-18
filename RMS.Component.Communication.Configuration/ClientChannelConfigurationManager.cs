using RMS.Component.Configuration;

namespace RMS.Component.Communication.Tcp.Client
{
    public class ClientChannelConfigurationManager
    {
        //private const string ConfigFolder = nameof(ConfigFolder);
        private const string filename = "ClientChannelConfig";
        private const string directory = @"Configuration";
        private const string channelKeyFilename = "ChannelKey.json";
        private ClientChannelConfigurationManager()
        {
            Configurations = LoadConfiguration();
            if (Configurations == null)
                IsConfigurationLoaded = false;
            else
                IsConfigurationLoaded = true;



        }
        static ClientChannelConfigurationManager() { }
        public static ClientChannelConfigurationManager Instance { get; } = new ClientChannelConfigurationManager();
        public bool IsConfigurationLoaded { get; } = false;
        public ClientChannelConfiguration Configurations { get; private set; }
        private ClientChannelConfiguration LoadConfiguration()
        {

            var configurtionLoader = new ConfigurtionLoader<ClientChannelConfiguration>(filename, directory);
            var configuration = configurtionLoader.LoadConfiguration();
            return configuration;
        }
    }
}
