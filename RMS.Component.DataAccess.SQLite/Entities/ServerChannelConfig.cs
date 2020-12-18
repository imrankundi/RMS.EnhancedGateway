using System.Collections.Generic;

namespace RMS.Component.DataAccess.SQLite.Entities
{
    public class TcpServerChannelConfig
    {
        public long Id { get; set; }
        public string ChannelKey { get; set; }
        public int Port { get; set; }
        public int RetryAfterSeconds { get; set; }
        public bool EnableTls { get; set; }
        public bool Enabled { get; set; }
        public string Thumbprint { get; set; }
        public int PingIntervalInSeconds { get; set; }
        public int KickIntervalInSeconds { get; set; }
        public int KickWaitInSeconds { get; set; }
        public int SyncIntervalInSeconds { get; set; }
        public virtual IEnumerable<ListenerApiConfig> Listeners { get; set; }
    }
}
