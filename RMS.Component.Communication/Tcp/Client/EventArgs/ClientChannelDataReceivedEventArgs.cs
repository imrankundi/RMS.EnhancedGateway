namespace RMS.Component.Communication.Tcp.Client
{
    public class ClientChannelDataReceivedEventArgs : ClientChannelEventArgs
    {
        public string Message { get; set; }
    }
}
