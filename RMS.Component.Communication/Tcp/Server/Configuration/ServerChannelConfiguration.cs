using Newtonsoft.Json;
using RMS.Component.Configuration;
using System.Net;

namespace RMS.Component.Communication.Tcp.Server
{
    public class ServerChannelConfiguration : BaseConfiguration
    {
        public string ChannelId { get; set; }
        public string ChannelKey { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public int RetryAfterSeconds { get; set; }
        public bool EnableTls { get; set; }
        public bool Enabled { get; set; }
        public string Thumbprint { get; set; }
        public int PingIntervalInSeconds { get; set; }
        public int KickIntervalInSeconds { get; set; }
        public int KickWaitInSeconds { get; set; }
        public int SyncIntervalInSeconds { get; set; }
        [JsonIgnore]
        public IPAddress IpAddress => IPAddress.Parse(Ip);
        [JsonIgnore]
        public IPEndPoint EndPoint => new IPEndPoint(IpAddress, Port);
    }
}
