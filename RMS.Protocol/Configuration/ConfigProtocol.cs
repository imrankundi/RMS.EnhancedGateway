using System.Collections.Generic;

namespace RMS.Protocols.Configuration
{
    public class ConfigProtocol
    {
        public string Device { get; set; }
        public string ProtocolHeader { get; set; }
        public List<ConfigSection> ConfigSections { get; set; }

        public ConfigProtocol()
        {
            ConfigSections = new List<ConfigSection>();
        }
    }
}
