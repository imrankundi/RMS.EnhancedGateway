using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RMS.AWS;
using RMS.Component.Communication.Tcp.Client;
using RMS.Component.Communication.Tcp.Server;
using RMS.Component.RestHelper;
using RMS.Gateway;
using RMS.Parser;
using RMS.Server.BusinessLogic;
using RMS.Server.DataTypes;
using RMS.Server.DataTypes.Requests;
using RMS.Server.Tcp.Messages;
using RMS.Server.WebApi.Configuration;
using System;
using System.Threading.Tasks;

namespace RMS.Server.WebApi
{
    public class EnhancedGateway : IServerChannelHandler
    {
        private ServerChannel server;
        private ServerInfo info;
        private ServerChannelConfiguration configurations;

        public async Task Start()
        {
            await EstablishServerChannel();
        }

        public async Task Stop()
        {
            await server.StopAsync();
        }

        public void Notify(object request)
        {
            var jsonObject = JObject.FromObject(request);
            Request req = jsonObject.ToObject<Request>();
            Console.WriteLine(req.ToFormattedJson());
            //server.Send(request.ChannelKey, request.Data);
        }

        private async Task EstablishServerChannel()
        {

            try
            {
                configurations = ServerChannelConfigurationManager.Instance.Configurations;
                server = new ServerChannel(configurations)
                {
                    ServerChannelHandler = this
                };
                await server.StartAsync();


            }
            catch (Exception)
            {
                server.ServerChannelHandler = null;
                server = null;
            }
        }

        private void HandleResponse(TcpMessage message, ClientContext context)
        {
            //throw new NotImplementedException();
        }

        private void SendResponse(TcpMessage message, ClientContext context)
        {
            CommunicationContext communicationContext = new CommunicationContext
            {
                IP = context.IP
            };
            var response = RequestHandler.HandleRequest(message.Data, communicationContext);
            TcpMessage msg = new TcpMessage
            {
                MessageType = MessageType.Response,
                Data = response,
                ChannelId = message.ChannelId,
                ChannelKey = message.ChannelKey
            };
            context.Send(msg.ToJson());
        }

        public void ServerChannelDataReceived(ServerChannelDataReceivedEventArgs e)
        {
            var message = e.Message;
            Console.WriteLine(message);
            var result = ParsingManager.FirstLevelParser(message);
            UpdateClientInfo(e.Context, result);
            var packet = ParsingManager.SecondLevelParser(result);
            var json = JsonConvert.SerializeObject(packet, Formatting.Indented);
            Console.WriteLine(json);
            PushToServer(json);
        }

        private static void PushToServer(object request)
        {

            try
            {
                ServerInfo info = new ServerInfo
                {
                    AccessKey = "ACCESS_KEY",
                    AuthenticationType = "AWS4",
                    EndPointUri = "http://localhost:5600/api/simulator/notify",
                    HttpTimeoutSecs = 10,
                    Id = 1,
                    Region = "Pakistan",
                    Service = "execute-api",
                    XApiKey = "API_KEY",
                    SecretKey = "SECRET_KEY",
                    UploadInterval = 100,
                    Name = "Reon (AWSV4)",
                    MaxRecordsPerHit = 10,
                    MaxRecordsToFetch = 10,
                    ParallelTcpConn = 2
                };
                AWS4Client client = new AWS4Client(info);
                client.PostData(JsonConvert.SerializeObject(request, Formatting.Indented));
                //var configuration = WebApiServerConfigurationManager.Instance.Configurations;
                //if (!configuration.EnableSimulation)
                //    return;

                //var client = new RestClientFactory("PushServer");
                //var response = client.PostCallAsync<object, object>
                //    (client.apiConfiguration.Apis["notify"], request);
            }
            catch (Exception ex)
            {

            }

        }
        private void UpdateClientInfo(ClientContext context, ReceivedPacket packet)
        {
            if (context == null)
                return;

            if (packet == null)
                return;

            if (server == null)
                return;


            var info = server.FindChannelInfo(context);
            if(info.ChannelKey.Equals(TerminalHelper.DefaultTerminalId))
            {
                //info.ChannelKey = packet.TerminalId;
                server.RegisterChannelKey(context, packet.TerminalId);
            }
        }

        public void ServerChannelConnected(ServerChannelConnectedEventArgs e)
        {
            TcpMessage message = new TcpMessage
            {
                MessageType = MessageType.Connected,
                ChannelId = e.ChannelId,
                ChannelKey = null
            };
            string msg = string.Format("{0} -> {1}", e.ChannelId, "Connected");
            //e.Context.Send(message.ToJson());
            e.Context.Send(TerminalHelper.TimeSync());
            Console.WriteLine(msg);
            PopulateChannelList();
        }

        private void PopulateChannelList()
        {
            var keys = server.ChannelIds;
            //StringBuilder sb = new StringBuilder();
            Console.WriteLine("------------------------------------");
            foreach (var key in keys)
            {
                Console.WriteLine(key);
            }
            Console.WriteLine("------------------------------------");

        }

        public void ServerChannelDisconnected(ServerChannelDisconnectedEventArgs e)
        {
            string msg = string.Format("{0} -> {1}", e.ChannelId, "Disconnected");
            Console.WriteLine(msg);
            PopulateChannelList();
        }

        public void ServerChannelError(ServerChannelErrorEventArgs e)
        {
        }

        public void ServerListeningStateChanged(ServerChannelEventArgs e)
        {
            Console.WriteLine(server.IsStarted ? "Server Started" : "Server Stopped");
        }

        public void ServerChannelRegistered(ServerChannelRegisteredEventArgs e)
        {
            string msg = string.Format("{0} -> {1}", e.ChannelId, "Registered");
            Console.WriteLine(msg);
        }
    }
}
