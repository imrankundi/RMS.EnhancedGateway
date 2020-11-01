using System;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace RMS.Network
{
    public class ClientState
    {
        public const int BufferSize = 1024;
        public Socket ClientSocket { get; private set; }
        public byte[] Buffer { get; set; }
        public StringBuilder ReceivedText { get; set; }

        public ClientState(Socket socket)
        {
            Buffer = new byte[BufferSize];
            ReceivedText = new StringBuilder();
            ClientSocket = socket;
        }
    }
}
