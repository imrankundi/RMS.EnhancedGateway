using System;

namespace RMS.Component.Communication.Tcp.Event
{
    public class ChannelInfo
    {
        public string ChannelId { get; set; }
        public string ChannelKey { get; set; }
        public DateTime ConnectedOn { get; set; }
        public DateTime LastDataReceived { get; set; }
        public DateTime LastDataSent { get; set; }
        public DateTime RegisteredOn { get; set; }
        //public ChannelStatus ChannelStatus { get; set; }

    }
}
