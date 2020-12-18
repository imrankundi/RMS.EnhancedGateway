using System.Collections.Generic;

namespace RMS.Protocols.Configuration
{
    public class ConfigSection
    {
        public string SectionKey { get; set; }
        public string Name { get; set; }
        public List<ConfigParameter> Parameters { get; set; }

        public ConfigSection()
        {
            Parameters = new List<ConfigParameter>();
        }
    }
}
