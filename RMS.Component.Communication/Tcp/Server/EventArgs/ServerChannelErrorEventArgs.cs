using RMS.Component.Communication.Tcp.Common;

namespace RMS.Component.Communication.Tcp.Server
{
    public class ServerChannelErrorEventArgs : ServerChannelEventArgs
    {
        public string Message { get; set; }
        public string Details { get; set; }
        public ErrorType ErrorType { get; set; }
    }
}
