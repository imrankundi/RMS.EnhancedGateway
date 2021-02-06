using RMS.Component.Logging.Models;
using RMS.Server.DataTypes;

namespace RMS.Server.WebApi.Configuration
{
    public class WebApiServerConfiguration
    {
        public string Url { get; set; }
        public bool EnableTcpServer { get; set; }
        public bool EnableSimulation { get; set; }
        public int TerminalCommandRetries { get; set; }
        public int TerminalCommandRetryIntervalInSeconds { get; set; }
        public string LogPath { get; set; }
        public LogLevel LogLevel { get; set; }
        public string EmailPath { get; set; }
        public JwtSettings JwtSettings { get; set; }
    }
}
