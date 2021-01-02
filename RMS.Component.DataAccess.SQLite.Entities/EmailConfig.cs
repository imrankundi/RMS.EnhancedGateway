using RMS.Component.Logging.Models;

namespace RMS.Component.DataAccess.SQLite.Entities
{
    public class EmailConfig
    {
        public long Id { get; set; }
        public string LogPath { get; set; }
        public LogLevel LogLevel { get; set; }
        public int FolderMonitoringIntervalInSeconds { get; set; }
        public string EmailFolderPath { get; set; }
        public virtual SmtpConfig SmtpSettings { get; set; }
    }
}
