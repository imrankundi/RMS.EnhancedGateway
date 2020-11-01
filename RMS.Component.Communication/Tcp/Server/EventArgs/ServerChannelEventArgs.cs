using RMS.Component.Communication.Tcp.Client;

namespace RMS.Component.Communication.Tcp.Server
{
    public class ServerChannelEventArgs
    {
        public ClientContext Context { get; set; }
        //public string ChannelKey { get; set; }
        public string ChannelId { get; set; }

    }
}
