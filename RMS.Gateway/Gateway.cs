using RMS.Core.Common;
using RMS.Core.Logging;
using RMS.Network.Server;
using RMS.Parser;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Timers;

namespace RMS.Gateway
{
    public class Gateway
    {
        public IContext Context { get; set; }

        //private const string ListenerRegisterRequest = "SHOW-YOUR-VALIDITY-CLIENT-WHO-YOU-ARE";
        //private const string ListenerRegisterResponse = "SALTEC-LISTENER-SERVICE-REQUESTING-CONNECT";

        GatewaySettings gatewaySettings;
        GatewayStatus gatewayStatus;
        Timer timer;
        // server for hardware clients e.g. GT / ET
        AsyncServer terminalGateway;
        Dictionary<Socket, ClientStatus> terminalClients;

        // server for software clients e.g. Listeners
        //AsyncServer listenerGateway;
        //Dictionary<Socket, ClientStatus> listenerClients;

        private void StartTerminalGateway()
        {
            try
            {
                terminalGateway = new AsyncServer(gatewaySettings.GatewayIP, gatewaySettings.GatewayPort);
                terminalClients = new Dictionary<Socket, ClientStatus>();

                terminalGateway.DataReceived += new DataReceivedEventHandler(TerminalDataReceived);
                terminalGateway.ClientDisconnected += new ClientDisconnectedEventHandler(TerminalClientDisconnected);
                terminalGateway.ClientConnected += new ClientConnectedEventHandler(TerminalClientConnected);
                terminalGateway.ServerError += new ServerErrorEventHandler(TerminalServerError);

                terminalGateway.Start();
            }
            catch (Exception ex)
            {
                LoggingManager.Log(ex);
            }

        }

        //private void StartListenerGateway()
        //{
        //    try
        //    {
        //        listenerGateway = new AsyncServer(gatewaySettings.GatewayIP, gatewaySettings.ListenerPort);
        //        listenerClients = new Dictionary<Socket, ClientStatus>();

        //        listenerGateway.DataReceived += new DataReceivedEventHandler(ListenerDataReceived);
        //        listenerGateway.ClientDisconnected += new ClientDisconnectedEventHandler(ListenerClientDisconnected);
        //        listenerGateway.ClientConnected += new ClientConnectedEventHandler(ListenerClientConnected);
        //        listenerGateway.ServerError += new ServerErrorEventHandler(ListenerServerError);

        //        listenerGateway.Start();
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingManager.Log(ex);
        //    }
        //}

        private void InitializeGateway()
        {
            try
            {
                gatewaySettings = GatewaySettings.Instance;
                gatewayStatus = new GatewayStatus();

                // timer for synchronize GT by sending SPDT
                timer = new Timer()
                {
                    Interval = gatewaySettings.SyncIntervalInSeconds
                };
                timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
                timer.Enabled = true;
            }
            catch (Exception ex)
            {
                LoggingManager.Log(ex);
            }
        }

        private void TransferText(string text)
        {
            try
            {
                if (Context != null)
                    Context.TransferText(text);

            }
            catch (Exception ex)
            {
                LoggingManager.Log(ex);
            }

        }

        public void Start()
        {
            try
            {
                InitializeGateway();
                StartTerminalGateway();
                //StartListenerGateway();
            }
            catch (Exception ex)
            {
                LoggingManager.Log(ex);
            }
        }

        public void Stop()
        {
            try
            {
                terminalGateway.Stop();
                terminalClients.Clear();

                //listenerGateway.Stop();
                //listenerClients.Clear();

                timer.Enabled = false;
            }
            catch (Exception ex)
            {
                LoggingManager.Log(ex);
            }
        }

        private void SendText(string terminalId, string text)
        {
            try
            {
                Socket clientSocket = FindClientSocketByTerminalId(terminalId);
                if (clientSocket != null)
                {
                    terminalGateway.Send(clientSocket, text);
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Log(ex);
            }



        }

        private Socket FindClientSocketByTerminalId(string terminalId)
        {
            Socket socket = null;
            var keys = terminalGateway.ClientList.ToArray();
            foreach (var key in keys)
            {
                try
                {
                    if (terminalClients.ContainsKey(key))
                    {
                        ClientStatus status = terminalClients[key];
                        if (status != null)
                        {
                            if (status.TerminalId.Equals(terminalId))
                            {
                                socket = key;
                                break;
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    LoggingManager.Log(ex);
                }

            }


            return socket;
        }


        private void SyncTerminal()
        {
            terminalGateway.BroadCast(TerminalHelper.TimeSync());
        }

        private void KickTerminals()
        {
            if (terminalGateway == null)
                return;

            if (terminalGateway.ClientList == null)
                return;

            var keys = terminalGateway.ClientList.ToArray();
            foreach (var key in keys)
            {
                try
                {
                    var status = terminalClients[key];
                    if (status != null)
                    {
                        if (status.LastDataReceivedElapsedSeconds > gatewaySettings.KickIntervalInSeconds)
                        {
                            terminalGateway.DisconnectClient(key);
                            terminalClients.Remove(key);
                        }
                    }

                }
                catch (Exception ex)
                {
                    LoggingManager.Log(ex);
                }

            }
        }

        private void SyncTerminalTask()
        {
            if (gatewayStatus.LastSyncElapsedSeconds > gatewaySettings.PingIntervalInSeconds)
            {
                gatewayStatus.LastSync = DateTimeHelper.CurrentUniversalTime;
                new System.Threading.Thread(new System.Threading.ThreadStart(SyncTerminal)).Start();
            }
        }

        private void RestartGatewayTask()
        {
            if (gatewayStatus.UpTimeSeconds > 24 * 1000)
            {
                if (!gatewayStatus.Restarting)
                {
                    gatewayStatus.Restarting = true;
                    Restart();
                    gatewayStatus.Restarting = false;
                }
            }
        }

        private void KickTerminalTask()
        {
            if (gatewayStatus == null)
                return;

            if (gatewayStatus.LastKickCheckElapsedSeconds > gatewaySettings.KickIntervalInSeconds)
            {
                gatewayStatus.LastKickCheck = DateTimeHelper.CurrentUniversalTime;
                gatewayStatus.Kicking = true;
                KickTerminals();
                gatewayStatus.Kicking = false;
            }
        }
        void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // send sync packet to terminal
            SyncTerminalTask();

            // kick terminal
            KickTerminalTask();

            // kick listener
            //KickInvalidListeners();

            // restart gateway
            //RestartGatewayTask();

        }


        public void Restart()
        {
            Stop();
            System.Threading.Thread.Sleep(gatewaySettings.RestartWaitInSeconds);
            Start();
        }

        private void UpdateGTStatus(Socket clientSocket, string receivedText)
        {
            if (clientSocket == null)
                return;

            if (string.IsNullOrEmpty(receivedText))
                return;

            string[] temp = receivedText.Split('<');
            if (temp.Length <= 0)
                return;

            if (string.IsNullOrEmpty(temp[0]))
                return;

            if (temp[0].Trim().Length != 8)
                return;

            string terminalId = temp[0];

            if (terminalClients == null)
                return;

            if (!terminalClients.ContainsKey(clientSocket))
                return;

            ClientStatus status = terminalClients[clientSocket];
            if (status == null)
                return;

            status.LastDataReceived = DateTimeHelper.CurrentUniversalTime;

            if (status.TerminalId == TerminalHelper.DefaultTerminalId)
                status.TerminalId = terminalId;
        }

        #region Events

        void TerminalClientConnected(object Sender, ClientConnectedEventArgs e)
        {
            try
            {
                ClientStatus status = new ClientStatus()
                {
                    ConnectedOn = e.ClientConnectedOn,
                    LastDataReceived = e.ClientConnectedOn,
                    TerminalId = TerminalHelper.DefaultTerminalId
                };
                gatewayStatus.TerminalClientCount = terminalGateway.ClientList.Count;

                terminalGateway.Send(e.ClientSocket, TerminalHelper.TimeSync());
                terminalClients.Add(e.ClientSocket, status);
            }
            catch (Exception ex)
            {
                LoggingManager.Log(ex);
            }

        }

        void TerminalClientDisconnected(object Sender, ClientDisconnectedEventArgs e)
        {
            try
            {
                if (terminalClients.ContainsKey(e.ClientSocket))
                    terminalClients.Remove(e.ClientSocket);
            }
            catch (Exception ex)
            {
                LoggingManager.Log(ex);
            }

        }

        void TerminalDataReceived(object Sender, DataReceivedEventArgs e)
        {
            try
            {
                //byte[] dataBytes1 = Encoding.Default.GetBytes(e.ReceivedText);
                var result = ParsingManager.FirstLevelParser(e.ReceivedText);


                UpdateGTStatus(e.ClientSocket, result.ToString());
                SendToListener(result);
                TransferText(result.ToString());
            }
            catch (Exception ex)
            {
                LoggingManager.Log(ex);
            }
        }

        private void SendToListener(ReceivedPacket receivedPacket)
        {
            try
            {
                if (Context != null)
                    Context.TransferText(receivedPacket.ToString());

                ParsingManager.SecondLevelParser(receivedPacket);

            }
            catch (Exception ex)
            {
                LoggingManager.Log(ex);
            }
        }

        //private void SendToListener(string text)
        //{
        //    if (listenerClients == null)
        //        return;

        //    var keys = listenerGateway.ClientList.ToArray();
        //    foreach (var key in keys)
        //    {
        //        if (listenerClients.ContainsKey(key))
        //        {
        //            try
        //            {
        //                var status = listenerClients[key];
        //                if (status != null)
        //                {
        //                    if (status.IsValid)
        //                    {
        //                        listenerGateway.Send(key, text);
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                LoggingManager.Log(ex);
        //            }
        //        }

        //    }
        //}

        //void ListenerClientConnected(object Sender, ClientConnectedEventArgs e)
        //{

        //    try
        //    {
        //        ClientStatus listenerStatus = new ClientStatus()
        //        {
        //            ConnectedOn = e.ClientConnectedOn
        //        };
        //        gatewayStatus.ListenerConnected = true;

        //        listenerGateway.Send(e.ClientSocket, ListenerRegisterRequest);
        //        listenerClients.Add(e.ClientSocket, listenerStatus);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingManager.Log(ex);
        //    }

        //}

        //void ListenerClientDisconnected(object Sender, ClientDisconnectedEventArgs e)
        //{

        //    try
        //    {
        //        if(listenerClients.ContainsKey(e.ClientSocket))
        //            listenerClients.Remove(e.ClientSocket);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingManager.Log(ex);
        //    }

        //}

        //void ListenerDataReceived(object Sender, DataReceivedEventArgs e)
        //{
        //    try
        //    {
        //        if (e.ReceivedText.Equals(ListenerRegisterResponse))
        //        {
        //            if(listenerClients.ContainsKey(e.ClientSocket))
        //            {
        //                ClientStatus listenerStatus = listenerClients[e.ClientSocket];
        //                listenerStatus.IsValid = true;
        //            }

        //        }
        //        else
        //        {
        //            string[] temp = e.ReceivedText.Split('<');

        //            if (temp == null)
        //                return;

        //            if (temp.Length > 0)
        //            {
        //                string terminalId = temp[0].Trim();
        //                if (terminalId.Length == 8)
        //                {
        //                    SendText(terminalId, e.ReceivedText);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingManager.Log(ex);
        //    }

        //}

        //void ListenerServerError(object sender, ServerErrorEventArgs e)
        //{
        //}

        void TerminalServerError(object sender, ServerErrorEventArgs e)
        {
        }

        #endregion




    }

}
