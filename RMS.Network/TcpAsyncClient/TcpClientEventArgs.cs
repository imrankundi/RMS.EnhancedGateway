using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
