using RMS.Component.Logging.Models;
using System.Collections.Generic;

namespace RMS.Component.DataAccess.SQLite.Entities
{
    public class ServiceMonitorConfig
    {
        public long Id { get; set; }
        public string LogPath { get; set; }
        public LogLevel LogLevel { get; set; }
        public int IntervalInSeconds { get; set; }
        public virtual IEnumerable<MontioringParameterConfig> Parameters { get; set; }
    }
}
