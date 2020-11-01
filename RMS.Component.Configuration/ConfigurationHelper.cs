using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKPSAssets.Component.Configuration
{
    public class ConfigurationHelper
    {
        private const string ConfigFile = nameof(ConfigFile);
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
        public Configurations Configurations { get; private set; }
        private Configurations LoadConfiguration()
        {
            try
            {
                var filePath = System.Configuration.ConfigurationManager.AppSettings.Get(ConfigFile);
                string jsonString = File.ReadAllText(filePath);
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
