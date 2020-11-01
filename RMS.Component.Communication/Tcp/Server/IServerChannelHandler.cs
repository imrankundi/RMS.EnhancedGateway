namespace RMS.Component.Communication.Tcp.Server
{
    public interface IServerChannelHandler
    {
        void ServerChannelDataReceived(ServerChannelDataReceivedEventArgs e);
        void ServerChannelConnected(ServerChannelConnectedEventArgs e);
        void ServerChannelRegistered(ServerChannelRegisteredEventArgs e);
        void ServerChannelDisconnected(ServerChannelDisconnectedEventArgs e);
        void ServerChannelError(ServerChannelErrorEventArgs e);
        void ServerListeningStateChanged(ServerChannelEventArgs e);
    }
}
