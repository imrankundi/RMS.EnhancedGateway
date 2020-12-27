using RMS.Component.Logging.Models;

namespace RMS.Component.Communication.Tcp.Client
{
    public class ClientChannelConfiguration
    {
        public string ChannelId { get; set; }
        public string ChannelKey { get; set; }
        public string LogPath { get; set; }
        public LogLevel LogLevel { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public int RetryAfterSeconds { get; set; }
        public bool EnableTls { get; set; }
        public string Thumbprint { get; set; }
        public bool ValidateCertificate { get; set; }
        //[JsonIgnore]
        //public IPAddress IpAddress => IPAddress.Parse(Ip);
        //[JsonIgnore]
        //public IPEndPoint EndPoint => new IPEndPoint(IpAddress, Port);
    }
}
