namespace RMS.Component.Communication.Tcp.Server
{
    public interface ITerminalCommandHandler
    {
        void TerminalCommandReceived(TerminalCommandReceivedEventArgs e);
    }
}
