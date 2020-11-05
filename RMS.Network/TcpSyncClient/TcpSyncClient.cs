using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace RMS.Network.Client
{
    public class TcpSyncClient
    {
        private TcpClient clientConnection;
        private NetworkStream clientStream;
        private StreamReader streamReader;
        private StreamWriter streamWriter;
        private Thread clientThread;

        private readonly string ip;
        private readonly int port;


        public bool IsConnected { get; private set; }

        public event DataReceivedEventHandler DataReceived;
        public event ClientDisconnectedEventHandler ClientDisconnected;
        public event ClientConnectedEventHandler ClientConnected;
        public event ClientConnectingEventHandler ClientConnecting;

        public TcpSyncClient(string ip, int port)
        {
            this.clientConnection = null;
            this.clientStream = null;
            this.streamReader = null;
            this.streamWriter = null;
            this.clientThread = null;
            this.ip = ip;
            this.port = port;
        }

        protected virtual void OnDataReceived(DataReceivedEventArgs e)
        {
            if (DataReceived != null)
                DataReceived(this, e);
        }

        protected virtual void OnClientDisconnected(ConnectionStatusEventArgs e)
        {
            if (ClientDisconnected != null)
                ClientDisconnected(this, e);
        }

        protected virtual void OnClientConnected(ConnectionStatusEventArgs e)
        {
            if (ClientConnected != null)
                ClientConnected(this, e);
        }


        protected virtual void OnClientConnecting(ConnectionStatusEventArgs e)
        {
            if (ClientConnecting != null)
                ClientConnecting(this, e);
        }

        public void Connect()
        {
            if (IsConnected)
                return;


            clientThread = new Thread(new ThreadStart(this.PerformConnection));
            clientThread.Start();

        }

        public void Disconnect()
        {
            if (clientConnection == null)
                return;

            try
            {
                clientConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void Send(string data)
        {
            if (!IsConnected) { throw new Exception("Client Is Not Connected"); }

            streamWriter.WriteLine(data);
            streamWriter.Flush();
        }

        private void PerformConnection()
        {
            try
            {
                clientConnection = new TcpClient();

                OnClientConnecting(new ConnectionStatusEventArgs
                {
                    ConnectionStatus = ConnectionStatus.Connecting,
                    Message = "Connecting"
                });

                clientConnection.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
                clientStream = clientConnection.GetStream();
                streamReader = new StreamReader(clientStream);
                streamWriter = new StreamWriter(clientStream);
                HandleConnection();
                streamReader.Close();
                streamWriter.Close();
                clientStream.Close();
                clientConnection.Close();
                OnClientDisconnected(new ConnectionStatusEventArgs
                {
                    ConnectionStatus = ConnectionStatus.Disconnected,
                    Message = "Disconnected"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                IsConnected = false;

                Thread.Sleep(10 * 1000);
                PerformConnection();
            }
        }

        private void HandleConnection()
        {
            IsConnected = true;
            OnClientConnected(new ConnectionStatusEventArgs
            {
                ConnectionStatus = ConnectionStatus.Connected,
                Message = "Connected"
            });

            while (true)
            {
                try
                {
                    string receivedData = streamReader.ReadLine();
                    if (receivedData == null)
                        break;
                    OnDataReceived(new DataReceivedEventArgs
                    {
                        ReceivedData = receivedData
                    });
                }
                catch (Exception)
                {
                    break;
                }
            }
            IsConnected = false;
        }
    }
}
