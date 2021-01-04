using RMS.Component.Logging.Models;
using System.Collections.Generic;

namespace RMS.Server.DataTypes
{
    public class ServiceMonitorServiceConfiguration
    {
        public long Id { get; set; }
        public string LogPath { get; set; }
        public LogLevel LogLevel { get; set; }
        public int IntervalInSeconds { get; set; }
        public IEnumerable<MonitoringParameter> Parameters { get; set; }
    }
}
