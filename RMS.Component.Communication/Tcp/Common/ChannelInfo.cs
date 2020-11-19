using System;
using System.Collections.Generic;
using System.Text;

namespace RMS.Component.Communication.Tcp.Event
{
    public class ChannelInfo
    {
        public ChannelInfo()
        {
            MessageBuffer = new StringBuilder();
        }
        public string ChannelId { get; set; }
        public string ChannelKey { get; set; }
        public DateTime ConnectedOn { get; set; }
        public DateTime LastDataReceived { get; set; }
        public DateTime LastDataSent { get; set; }
        public DateTime RegisteredOn { get; set; }
        public DateTime LastSynchronized { get; set; }
        public StringBuilder MessageBuffer { get; set; }
        public bool PartialPacket { get; set; }
        //public ChannelStatus ChannelStatus { get; set; }

    }
}
