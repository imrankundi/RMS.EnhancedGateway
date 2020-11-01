namespace RMS.Component.Communication.Tcp.Client
{
    public interface IClientChannelHandler
    {
        void ClientChannelDataReceived(ClientChannelDataReceivedEventArgs e);
        void ClientChannelConnected(ClientChannelConnectedEventArgs e);
        void ClientChannelDisconnected(ClientChannelDisconnectedEventArgs e);
        void ClientChannelError(ClientChannelErrorEventArgs e);
    }
}
