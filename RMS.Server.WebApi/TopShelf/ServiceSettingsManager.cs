namespace RMS.Server.WebApi
{
    public class ServiceSettingsManager
    {
        private const string fileName = "ServerChannelConfig";
        private const string directory = @"Configuration";
        private const string channelKeyFilename = "ChannelKey.json";
        private ServiceSettingsManager()
        {
            Configurations = LoadConfiguration();
            if (Configurations == null)
                IsConfigurationLoaded = false;
            else
                IsConfigurationLoaded = true;
        }
        static ServiceSettingsManager() { }
        public static ServiceSettingsManager Instance { get; } = new ServiceSettingsManager();
        public bool IsConfigurationLoaded { get; } = false;
        public ServiceSettings Configurations { get; private set; }
        private ServiceSettings LoadConfiguration()
        {
            ServiceSettings settings = new ServiceSettings();
            settings.DisplayName = RMS.Component.Common.AppConfigReader.Read("DisplayName");
            settings.ServiceName = RMS.Component.Common.AppConfigReader.Read("ServiceName");
            settings.Description = RMS.Component.Common.AppConfigReader.Read("Description");
            settings.InstanceName = RMS.Component.Common.AppConfigReader.Read("InstanceName");

            return settings;
        }


    }

}
