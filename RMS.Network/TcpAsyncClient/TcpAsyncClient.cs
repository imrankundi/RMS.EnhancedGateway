using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace RMS.Network.Client
{
    public class TcpAsyncClient
    {
        private static ManualResetEvent disconnectDone = new ManualResetEvent(false);
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);

        public event DataReceivedEventHandler DataReceived;
        public event ClientConnectedEventHandler ClientConnected;
        public event ClientDisconnectedEventHandler ClientDisconnected;
        public event ClientConnectingEventHandler ClientConnecting;

        public void OnDataReceived(DataReceivedEventArgs e)
        {
            DataReceived?.Invoke(this, e);
        }

        public void OnClientConnecting(ConnectionStatusEventArgs e)
        {
            ClientConnecting?.Invoke(this, e);
        }

        public void OnClientConnected(ConnectionStatusEventArgs e)
        {
            ClientConnected?.Invoke(this, e);
        }

        public void OnClientDisconnected(ConnectionStatusEventArgs e)
        {
            ClientDisconnected?.Invoke(this, e);
        }

        private IPEndPoint remoteEndPoint;
        private Socket client;
        public bool Stopped { get; set; }
        public bool IsConnected { get; set; }
        private Thread clientThread;

        public TcpAsyncClient(string ip, int port)
        {
            remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        }


        public void Connect()
        {
            if (IsConnected)
                return;


            clientThread = new Thread(new ThreadStart(this.PerformConnection));
            clientThread.Start();

        }

        private void PerformConnection()
        {
            try
            {
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.BeginConnect(remoteEndPoint, new AsyncCallback(ConnectCallback), client);
                connectDone.WaitOne();
                sendDone.WaitOne();
                Receive(client);
                receiveDone.WaitOne();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                IsConnected = false;
            }
        }

        private void ConnectCallback(IAsyncResult asyncResult)
        {
            try
            {
                if (asyncResult != null)
                {
                    Socket client = (Socket)asyncResult.AsyncState;

                    if (client != null && client.Connected)
                    {
                        client.EndConnect(asyncResult);
                        connectDone.Set();
                        ClientConnected(this, new ConnectionStatusEventArgs
                        {
                            ConnectionStatus = ConnectionStatus.Connected,
                            Message = "Connected"
                        });
                        IsConnected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                IsConnected = false;
            }
        }

        private void DisconnectCallback(IAsyncResult asyncResult)
        {
            try
            {
                if (asyncResult != null)
                {
                    Socket client = (Socket)asyncResult.AsyncState;

                    if (client != null && client.Connected)
                    {
                        client.EndDisconnect(asyncResult);
                        disconnectDone.Set();
                        ClientDisconnected(this, new ConnectionStatusEventArgs
                        {
                            ConnectionStatus = ConnectionStatus.Disconnected,
                            Message = "Disconnected"
                        });
                        IsConnected = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                IsConnected = false;
            }
        }
        private void Receive(Socket client)
        {
            try
            {
                if (client != null && client.Connected)
                {
                    ClientState state = new ClientState(client);
                    if (state != null)
                    {
                        client.BeginReceive(state.Buffer, 0, ClientState.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                IsConnected = false;
                Disconnect();
            }
        }

        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            try
            {
                if (asyncResult != null)
                {
                    ClientState state = (ClientState)asyncResult.AsyncState;
                    if (state != null)
                    {
                        Socket clientSocket = state.ClientSocket;

                        if (clientSocket != null && clientSocket.Connected)
                        {
                            int bytesRead = clientSocket.EndReceive(asyncResult);

                            if (bytesRead > 0)
                            {
                                state.ReceivedText.Append(Encoding.ASCII.GetString(state.Buffer, 0, bytesRead));
                                string response = state.ReceivedText.ToString();
                                state.ReceivedText.Length = 0;
                                //OnDataReceived(new TcpClientEventArgs(response));
                                OnDataReceived(new DataReceivedEventArgs
                                {
                                    ReceivedData = response
                                });
                                clientSocket.BeginReceive(state.Buffer, 0, ClientState.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                            }
                            else
                            {
                                //if (state.ReceivedText.Length > 1) { } // why this line is used ??
                                receiveDone.Set();
                            }
                        }
                    }
                }
            }
            catch (SocketException sx)
            {
                if (sx.ErrorCode == 10054)
                {
                    Disconnect();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Disconnect();
            }
        }

        public void Send(String data)
        {
            Send(client, data);
        }

        private void Send(Socket client, String data)
        {
            if (client != null && client.Connected && !string.IsNullOrEmpty(data))
            {
                byte[] byteData = Encoding.Default.GetBytes(data);
                client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), client);
            }
            //sendDone.WaitOne();
        }

        private void SendCallback(IAsyncResult asyncResult)
        {
            try
            {
                if (asyncResult != null)
                {
                    Socket clientSocket = (Socket)asyncResult.AsyncState;
                    if (clientSocket != null && clientSocket.Connected)
                    {
                        int bytesSent = clientSocket.EndSend(asyncResult);
                        sendDone.Set();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void Disconnect()
        {
            // Release the socket.
            try
            {

                if (client != null && client.Connected)
                {
                    client.Shutdown(SocketShutdown.Both);
                    client.BeginDisconnect(true, new AsyncCallback(DisconnectCallback), client);

                    // Wait for the disconnect to complete.
                    disconnectDone.WaitOne();

                    OnClientDisconnected(new ConnectionStatusEventArgs
                    {
                        ConnectionStatus = ConnectionStatus.Disconnected,
                        Message = "Disconnected"
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                IsConnected = false;
            }
        }

        private void CheckClientConnection()
        {
            // This is how you can determine whether a socket is still connected.
            bool blockingState = client.Blocking;
            try
            {
                client.Blocking = false;
                client.Send(new byte[1], 0, 0);
                Console.WriteLine("Connected!");
            }
            catch (SocketException e)
            {
                // 10035 == WSAEWOULDBLOCK
                if (e.NativeErrorCode.Equals(10035))
                    Console.WriteLine("Still Connected, but the Send would block");
                else
                {
                    Console.WriteLine("Disconnected: error code {0}!", e.NativeErrorCode);
                }
            }
            finally
            {
                client.Blocking = blockingState;
            }

        }
    }
}
