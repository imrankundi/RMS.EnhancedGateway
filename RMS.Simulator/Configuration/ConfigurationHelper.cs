
using Newtonsoft.Json;
using System.Configuration;
using System.IO;

namespace ES.Server.WebApi.Configuration
{

    public class ConfigurationHelper
    {
        private const string ConfigFolder = nameof(ConfigFolder);
        private const string FileName = "Configuration.json";
        private const string DefaultConfigDirectory = @"Configuration";
        public Configurations Configurations { get; private set; }

        private ConfigurationHelper()
        {
            Configurations = LoadConfiguration();
            if (Configurations == null)
                IsConfigurationLoaded = false;
            else
                IsConfigurationLoaded = true;
        }
        static ConfigurationHelper() { }
        public static ConfigurationHelper Instance { get; } = new ConfigurationHelper();
        public bool IsConfigurationLoaded { get; } = false;
        private Configurations LoadConfiguration()
        {
            try
            {
                var folder = ConfigurationManager.AppSettings.Get(ConfigFolder);
                if (string.IsNullOrEmpty(folder))
                {
                    folder = string.Format(@"{0}\{1}", ES.Common.AppDirectory.BaseDirectory, DefaultConfigDirectory);
                }

                var file = string.Format(@"{0}\{1}", folder, FileName);
                string jsonString = File.ReadAllText(file);
                Configurations configurations = JsonConvert.DeserializeObject<Configurations>(jsonString);
                return configurations;
            }
            catch
            {
                return null;
            }
        }

    }

}
