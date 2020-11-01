namespace RMS.Component.Communication.Tcp.Server
{
    public class ServerChannelDataReceivedEventArgs : ServerChannelEventArgs
    {
        public string Message { get; set; }
    }
}
