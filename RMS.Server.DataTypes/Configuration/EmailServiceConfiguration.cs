using RMS.Component.Common;
using RMS.Component.Logging.Models;

namespace RMS.Server.DataTypes.Email
{
    public class EmailServiceConfiguration
    {
        public long Id { get; set; }
        public string LogPath { get; set; }
        public LogLevel LogLevel { get; set; }
        public int FolderMonitoringIntervalInSeconds { get; set; }
        public string EmailFolderPath { get; set; }
        public virtual SmtpSettings SmtpSettings { get; set; }
    }
}
