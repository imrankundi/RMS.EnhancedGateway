using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RMS.AWS;
using RMS.Component.Communication.Tcp.Client;
using RMS.Component.Communication.Tcp.Server;
using RMS.Core.Common;
using RMS.Server.DataTypes;
using RMS.Server.DataTypes.Requests;
using RMS.Server.Tcp.Messages;
using RMS.Server.WebApi.Common;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace RMS.Server.WebApi
{
    public class EnhancedGateway : ITerminalCommandHandler
    {
        private ServerChannel server;
        private ServerInfo info;
        private ServerChannelConfiguration configurations;
        private Timer timer;
        public EnhancedGateway()
        {
            timer = new Timer();
            timer.Interval = ServerChannelConfigurationManager.Instance.Configurations.SyncIntervalInSeconds * 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Timer Tick...");
            timer.Enabled = false;
            try
            {
                SynchronizeTerminals();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                timer.Enabled = true;
            }

        }

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
            TerminalCommandRequest command = jsonObject.ToObject<TerminalCommandRequest>();

            // parse and store command to a dictionary
            Console.WriteLine(command.ToFormattedJson());
            Task.Run(() => server.Send(command.TerminalId, command.Data));
        }

        private void SynchronizeTerminals()
        {
            //ClientChannelManager.BroadCast(TerminalHelper.TimeSync());
            server.SynchronizeTerminals();
            //terminalGateway.BroadCast(TerminalHelper.TimeSync());
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

        //private void SendResponse(TcpMessage message, ClientContext context)
        //{
        //    CommunicationContext communicationContext = new CommunicationContext
        //    {
        //        IP = context.IP
        //    };
        //    var response = RequestHandler.HandleRequest(message.Data, communicationContext);
        //    TcpMessage msg = new TcpMessage
        //    {
        //        MessageType = MessageType.Response,
        //        Data = response,
        //        ChannelId = message.ChannelId,
        //        ChannelKey = message.ChannelKey
        //    };
        //    context.Send(msg.ToJson());
        //}

        public void TerminalCommandReceived(TerminalCommandReceivedEventArgs e)
        {
            Console.WriteLine("{0}: {1}", e.ChannelKey, e.Message);
            var command = TerminalCommandHandler.Instance.Find(e.ChannelKey);
            if (command != null)
            {
                command.ResponseData = e.Message;
                command.ResponseReceivedOn = DateTimeHelper.CurrentUniversalTime;
                command.Status = CommandStatus.ResponseReceived;
            }
            //var result = ParsingManager.FirstLevelParser(e.Message);
            //UpdateClientInfo(e.Context, result);
            //var packet = ParsingManager.SecondLevelParser(result);
            //var json = JsonConvert.SerializeObject(packet, Formatting.Indented);
            //Console.WriteLine(json);
            //PushToServer(json);
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
        //private void UpdateClientInfo(ClientContext context, ReceivedPacket packet)
        //{
        //    if (context == null)
        //        return;

        //    if (packet == null)
        //        return;

        //    if (server == null)
        //        return;


        //    var info = server.ClientChannelManager.fi(context);
        //    if(info.ChannelKey.Equals(TerminalHelper.DefaultTerminalId))
        //    {
        //        //info.ChannelKey = packet.TerminalId;
        //        server.RegisterChannelKey(context, packet.TerminalId);
        //    }
        //}


        private void PopulateChannelList()
        {
            //var keys = server.ClientChannelManager?.ChannelKeys;
            //if (keys == null)
            //    return;
            ////StringBuilder sb = new StringBuilder();
            //Console.WriteLine("------------------------------------");
            //foreach (var key in keys)
            //{
            //    Console.WriteLine(key);
            //}
            //Console.WriteLine("------------------------------------");

        }


    }
}
