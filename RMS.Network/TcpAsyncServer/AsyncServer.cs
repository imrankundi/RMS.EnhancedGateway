using RMS.Core.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace RMS.Network.Server
{
    public class AsyncServer
    {
        public event DataReceivedEventHandler DataReceived;
        public event ClientDisconnectedEventHandler ClientDisconnected;
        public event ClientConnectedEventHandler ClientConnected;
        public event ServerErrorEventHandler ServerError;

        private List<Socket> clientList;
        private int clientCount = 0;
        private Socket connectionListener;
        private IPEndPoint endPoint;

        private AsyncCallback clientCallBack;

        public List<Socket> ClientList => clientList;

        public bool IsStarted { get; private set; }

        public AsyncServer(string ip, int port)
        {
            clientList = new List<Socket>();
            endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        }

        public void Start()
        {
            try
            {
                connectionListener = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);
                connectionListener.Bind(endPoint);
                connectionListener.Listen(10);
                connectionListener.BeginAccept(OnClientConnected, null);
                IsStarted = true;
            }
            catch (Exception ex)
            {
                OnServerError(new ServerErrorEventArgs(ex.ToString()));
                LoggingManager.Log(ex);
            }
        }

        private void DisconnectAllClients()
        {
            try
            {
                foreach (Socket clientSocket in clientList.ToArray())
                {
                    try
                    {
                        if (clientSocket != null && clientSocket.Connected)
                        {
                            clientSocket.BeginDisconnect(true, OnClientDisconnected, clientSocket);
                            Thread.Sleep(10);
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggingManager.Log(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Log(ex);
            }
            
        }

        private void StopListening()
        {
            try
            {
                if (connectionListener != null)
                {
                    if (connectionListener.Connected)
                    {
                        connectionListener.Shutdown(SocketShutdown.Both);
                        connectionListener.Close();
                    }

                    if (connectionListener.IsBound)
                    {
                        connectionListener.Dispose();
                    }

                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Log(ex);
            }
        }

        private void PerformCleanup()
        {
            try
            {
                clientList.Clear();
                clientCount = 0;
                //endPoint = null;
                //clientCallBack = null;
                //connectionListener = null;
                IsStarted = false;
            }
            catch(Exception ex)
            {
                LoggingManager.Log(ex);
            }
            
        }

        public void Stop()
        {
            try
            {
                DisconnectAllClients();
                StopListening();
                PerformCleanup();
            }
            catch (Exception ex)
            {
                LoggingManager.Log(ex);
            }
        }

        public void DisconnectClient(Socket clientSocket)
        {
            try
            {
                clientSocket.BeginDisconnect(false, OnClientDisconnected, clientSocket);
            }
            catch(Exception ex)
            {
                LoggingManager.Log(ex);
            }
            
        }
        private void OnClientDisconnected(IAsyncResult asyncResult)
        {
            try
            {
                Socket clientSocket = (Socket)asyncResult.AsyncState;

                if (clientSocket != null)
                {
                    clientSocket.EndDisconnect(asyncResult);
                    clientList.Remove(clientSocket);
                    --clientCount;
                    OnClientDisconnected(new ClientDisconnectedEventArgs(clientSocket));
                    Thread.Sleep(10);
                    clientSocket.Close();
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Log(ex);
            }
        }

        public void Send(Socket clientSocket, string data)
        {
            try
            {
                if (string.IsNullOrEmpty(data))
                    return;

                if (clientSocket != null && clientSocket.Connected)
                {
                    string text = data + Environment.NewLine;
                    byte[] dataBytes = Encoding.Default.GetBytes(text);
                    clientSocket.BeginSend(dataBytes, 0, dataBytes.Length, 0,
                        new AsyncCallback(SendCallBack), clientSocket);
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Log(ex);
            }

        }

        private void OnClientConnected(IAsyncResult asyncResult)
        {
            try
            {
                if (connectionListener != null && connectionListener.IsBound)
                {
                    Socket clientSocket = connectionListener.EndAccept(asyncResult);
                    if (clientSocket != null && clientSocket.IsBound)
                    {
                        clientList.Add(clientSocket);
                        WaitForData(clientSocket);
                        connectionListener.BeginAccept(OnClientConnected, null);
                        OnClientConnected(new ClientConnectedEventArgs(clientSocket));
                    }
                }

            }
            catch (Exception ex)
            {
                LoggingManager.Log(ex);
            }
        }

        private void SendCallBack(IAsyncResult asyncResult)
        {
            try
            {
                Socket socket = (Socket)asyncResult.AsyncState;
                if (socket != null)
                {
                    int noOfBytesSent = socket.EndSend(asyncResult);
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Log(ex);
            }
        }

        private void OnDataReceived(IAsyncResult asyncResult)
        {
            try
            {
                string data = string.Empty;
                ClientState clientStateObject = (ClientState)asyncResult.AsyncState;

                if (clientStateObject == null)
                    return;

                string clientIP = ((IPEndPoint)clientStateObject.ClientSocket.RemoteEndPoint).Address.ToString();
                int clientPort = ((IPEndPoint)clientStateObject.ClientSocket.RemoteEndPoint).Port;

                try
                {
                    if (clientStateObject.ClientSocket != null && clientStateObject.ClientSocket.Connected)
                    {
                        int receivedBytes = clientStateObject.ClientSocket.EndReceive(asyncResult);
                        string receivedString = Encoding.Default.GetString(clientStateObject.Buffer, 0, receivedBytes);

                        clientStateObject.ReceivedText.Append(receivedString);
                        if (clientStateObject.ReceivedText.Length > 0)
                        {
                            data = clientStateObject.ReceivedText.ToString().Replace("\r\n", "").Trim();
                            clientStateObject.ReceivedText = null;

                            OnDataReceived(new DataReceivedEventArgs(clientStateObject.ClientSocket, data));
                            WaitForData(clientStateObject.ClientSocket);
                        }
                        else
                        {
                            DisconnectClient(clientStateObject.ClientSocket);
                        }
                    }

                }
                catch (Exception ex)
                {
                    DisconnectClient(clientStateObject.ClientSocket);
                    LoggingManager.Log(ex);
                }

            }
            catch (Exception ex)
            {
                LoggingManager.Log(ex);
            }
        }

        private void WaitForData(Socket socket)
        {
            try
            {
                if (clientCallBack == null)
                {
                    clientCallBack = new AsyncCallback(OnDataReceived);
                }
                if (socket != null && socket.Connected)
                {
                    ClientState clientStateObject = new ClientState(socket);

                    socket.BeginReceive(clientStateObject.Buffer, 0, clientStateObject.Buffer.Length, SocketFlags.None, clientCallBack, clientStateObject);
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Log(ex);
            }
        }

        public void BroadCast(string data)
        {
            try
            {
                foreach (Socket clientSocket in clientList.ToArray())
                {
                    Send(clientSocket, data);
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Log(ex);
            }

        }

        #region Events

        protected virtual void OnDataReceived(DataReceivedEventArgs e)
        {
            DataReceived?.Invoke(this, e);
        }

        protected virtual void OnClientDisconnected(ClientDisconnectedEventArgs e)
        {
            ClientDisconnected?.Invoke(this, e);
        }

        protected virtual void OnClientConnected(ClientConnectedEventArgs e)
        {
            ClientConnected?.Invoke(this, e);
        }

        protected virtual void OnServerError(ServerErrorEventArgs e)
        {
            ServerError?.Invoke(this, e);
        }

        #endregion


    }
}
