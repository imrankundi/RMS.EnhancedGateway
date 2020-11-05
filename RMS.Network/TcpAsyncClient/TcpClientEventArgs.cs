using System;

namespace RMS.Network.Client
{
    public class TcpClientEventArgs : EventArgs
    {
        public string ReceivedText { get; set; }
        public TcpClientEventArgs(string receivedText)
        {
            ReceivedText = receivedText;
        }
    }

}
