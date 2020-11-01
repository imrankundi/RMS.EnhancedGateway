using RMS.Component.Communication.Tcp.Common;

namespace RMS.Component.Communication.Tcp.Client
{
    public class ClientChannelErrorEventArgs : ClientChannelEventArgs
    {
        public string Message { get; set; }
        public string Details { get; set; }
        public ErrorType ErrorType { get; set; }
    }
}
