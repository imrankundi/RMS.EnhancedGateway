using System;

namespace RMS.Network.Client
{
    public class ConnectionStatusEventArgs : EventArgs
    {
        public ConnectionStatus ConnectionStatus { get; set; }
        public string Message { get; set; }
    }
}
