using Newtonsoft.Json;
using RMS.Component.Logging.Models;

namespace RMS.Component.Configuration
{
    public class BaseConfiguration
    {
        public string LogPath { get; set; }
        public LogLevel LogLevel { get; set; }
        [JsonIgnore]
        public string FileName { get; set; }
        [JsonIgnore]
        public string Directory { get; set; }

        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public virtual string ToFormattedJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
