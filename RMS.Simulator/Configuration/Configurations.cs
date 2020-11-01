using ES.Logging;

namespace ES.Server.WebApi.Configuration
{
    public class Configurations
    {
        public AgentConfiguration Agent { get; set; }
        public string LogPath { get; set; }
        public LogLevel LogLevel { get; set; }
    }
}
