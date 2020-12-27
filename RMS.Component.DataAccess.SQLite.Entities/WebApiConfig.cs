using RMS.Component.Logging.Models;

namespace RMS.Component.DataAccess.SQLite.Entities
{
    public class WebApiConfig
    {
        public int Id { get; set; }
        public string LogPath { get; set; }
        public LogLevel LogLevel { get; set; }
        public string FileName { get; set; }
        public string Directory { get; set; }
        public string Url { get; set; }
        public bool EnableTcpServer { get; set; }
        public bool EnableSimulation { get; set; }
        public int TerminalCommandRetries { get; set; }
        public int TerminalCommandRetryIntervalInSeconds { get; set; }
    }
}
