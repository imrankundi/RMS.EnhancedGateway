using RMS.Component.DataAccess.SQLite.Repositories;

namespace RMS.Component.Communication.Tcp.Server
{
    public class ServerChannelConfigurationManager
    {
        private const string fileName = "ServerChannelConfig";
        private const string directory = @"Configuration";
        private const string channelKeyFilename = "ChannelKey.json";
        private ServerChannelConfigurationManager()
        {
            Configurations = LoadConfiguration();
            if (Configurations == null)
                IsConfigurationLoaded = false;
            else
                IsConfigurationLoaded = true;
        }
        static ServerChannelConfigurationManager() { }
        public static ServerChannelConfigurationManager Instance { get; } = new ServerChannelConfigurationManager();
        public bool IsConfigurationLoaded { get; } = false;
        public ServerChannelConfiguration Configurations { get; private set; }
        private ServerChannelConfiguration LoadConfiguration()
        {
            var repo = new GatewayConfigRepository();
            var config = repo.ReadTcpServerConfiguration();
            var res = Mappers.ConfigurationMapper.Map(config);
            return res;
        }


    }

}
