namespace RMS.Component.Communication.Tcp.Server
{
    public class TerminalCommandReceivedEventArgs
    {
        public string ChannelKey { get; set; }
        public string Message { get; set; }
    }
}
