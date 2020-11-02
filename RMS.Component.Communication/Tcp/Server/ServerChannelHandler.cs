using DotNetty.Transport.Channels;
using Newtonsoft.Json;
using RMS.AWS;
using RMS.Core.Common;
using RMS.Gateway;
using RMS.Parser;
using System;
using System.Threading;

namespace RMS.Component.Communication.Tcp.Server
{
    public class ServerChannelHandler : SimpleChannelInboundHandler<string>
    {
        //public ClientChannelManager ClientChannelManager { get; set; }
        public IServerChannelHandler ChannelHandler { get; set; }

        public void ServerChannelDataReceived(IChannelHandlerContext context, string message)
        {
            Console.WriteLine(message);
            var result = ParsingManager.FirstLevelParser(message);
            UpdateClientInfo(context, result);
            var packet = ParsingManager.SecondLevelParser(result);
            var json = JsonConvert.SerializeObject(packet, Formatting.Indented);
            Console.WriteLine(json);
            PushToServer(json);
        }
        private void UpdateClientInfo(IChannelHandlerContext context, ReceivedPacket packet)
        {
            //if (context == null)
            //    return;


            //var info = ClientChannelManager.FindChannelInfo(context);
            //if (info.ChannelKey.Equals(TerminalHelper.DefaultTerminalId))
            //{
            //    //info.ChannelKey = packet.TerminalId;
            //    ClientChannelManager.RegisterChannelKey(context, packet.TerminalId);
            //}
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

        protected override void ChannelRead0(IChannelHandlerContext context, string message)
        {
            try
            {
                string key = string.Empty;
                var result = ParsingManager.FirstLevelParser(message);
                var packet = ParsingManager.SecondLevelParser(result);
                var json = JsonConvert.SerializeObject(packet, Formatting.Indented);
                ChannelManager.Instance.UpdateChannelInfo(context, result.TerminalId);
                //var channelInfo = ClientChannelManager.FindChannelInfo(context);
                //if(channelInfo != null)
                //{

                //    channelInfo.LastDataReceived = DateTimeHelper.CurrentUniversalTime;
                    

                //    if (ChannelHandler != null)
                //    {
                //        ChannelHandler.ServerChannelDataReceived(new ServerChannelDataReceivedEventArgs
                //        {
                //            ChannelId = channelInfo.ChannelId,
                //            Context = new Client.ClientContext(context),
                //            Message = message
                //        });
                //    }
                //}
                
            }
            catch (Exception ex)
            {

                Logging.ServerChannelLogger.Instance.Log.Error("TcpServerChannelHandler", "ChannelRead0",
                    string.Format("Message: {0}, Details: {1}", ex.Message, ex.ToString()));
            }

        }

        public override void ChannelReadComplete(IChannelHandlerContext context)
        {
            try
            {
                context.Flush();
            }
            catch (Exception ex)
            {
                Logging.ServerChannelLogger.Instance.Log.Error("TcpServerChannelHandler", "ChannelReadComplete",
                    string.Format("Message: {0}, Details: {1}", ex.Message, ex.ToString()));
            }

        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            //base.ExceptionCaught(context, exception);
            //var channelInfo = ClientChannelManager.FindChannelInfo(context);
            //try
            //{
            //    ChannelHandler.ServerChannelError(new ServerChannelErrorEventArgs
            //    {
            //        ChannelId = channelInfo.ChannelId,
            //        Context = new Client.ClientContext(context),
            //        Message = exception.Message
            //    });
            //}
            //catch (Exception ex)
            //{
            //    Logging.ClientChannelLogger.Instance.Log.Error("TcpServerChannelHandler", "ExceptionCaught",
            //        string.Format("Message: {0}, Details: {1}", ex.Message, ex.ToString()));
            //}

        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            try
            {
                base.ChannelActive(context);
                var info = ChannelManager.Instance.AddChannel(context);
                if (info == null)
                    return;

                //ClientChannelManager.RegisterChannelKey(context, info.ChannelKey);

                //if (ChannelHandler != null)
                //{
                //    ChannelHandler.ServerChannelConnected(new ServerChannelConnectedEventArgs
                //    {
                //        //ChannelKey = info.ChannelKey,
                //        ChannelId = info.ChannelId,
                //        Context = new Client.ClientContext(context)
                //    });
                //}

            }
            catch (Exception ex)
            {
                Logging.ClientChannelLogger.Instance.Log.Error("TcpServerChannelHandler", "ChannelActive",
                    string.Format("Message: {0}, Details: {1}", ex.Message, ex.ToString()));
            }
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            try
            {
                base.ChannelInactive(context);

                //var info = ChannelManager.FindChannelInfo(context);
                Thread.Sleep(10);
                ChannelManager.Instance.RemoveChannel(context);
                //ChannelManager.Instance.RemoveChannel(context);
                //ClientChannelManager.UnrigisterChannelKey(info.ChannelKey);
                //if (ChannelHandler != null)
                //{
                //    ChannelHandler.ServerChannelDisconnected(new ServerChannelDisconnectedEventArgs
                //    {
                //        ChannelId = info.ChannelId,
                //        //ChannelKey = info.ChannelKey,
                //        Context = new Client.ClientContext(context)
                //    });
                //}
            }
            catch (Exception ex)
            {
                Logging.ClientChannelLogger.Instance.Log.Error("TcpServerChannelHandler", "ChannelInactive",
                    string.Format("Message: {0}, Details: {1}", ex.Message, ex.ToString()));
            }

        }

        

    }
}
