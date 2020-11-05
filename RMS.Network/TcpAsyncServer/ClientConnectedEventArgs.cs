using System;
using System.Net;
using System.Net.Sockets;

namespace RMS.Network.Server
{
    public class ClientConnectedEventArgs : EventArgs
    {

        public DateTime ClientConnectedOn { get; private set; }

        public Socket ClientSocket { get; private set; }
        public string ClientIP
        {
            get
            {
                string ip = string.Empty;
                if (ClientSocket != null)
                {
                    ip = ((IPEndPoint)ClientSocket.RemoteEndPoint).Address.ToString();
                    int clientPort = ((IPEndPoint)ClientSocket.RemoteEndPoint).Port;
                }

                return ip;
            }
        }

        public int ClientPort
        {
            get
            {
                int port = 0;
                if (ClientSocket != null)
                {

                    port = ((IPEndPoint)ClientSocket.RemoteEndPoint).Port;
                }

                return port;
            }
        }

        public ClientConnectedEventArgs(Socket clientSocket)
        {
            this.ClientSocket = clientSocket;
            this.ClientConnectedOn = DateTime.UtcNow;
        }
    }
}
