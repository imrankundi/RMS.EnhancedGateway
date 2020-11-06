using DotNetty.Transport.Channels;
using Newtonsoft.Json;
using RMS.AWS;
using RMS.Core.Enumerations;
using RMS.Gateway;
using RMS.Parser;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RMS.Component.Communication.Tcp.Server
{
    public class ServerChannelHandler : SimpleChannelInboundHandler<string>
    {
        private string className = nameof(ServerChannelHandler);
        public ITerminalCommandHandler ChannelHandler { get; set; }

        private void PushToServer(object request)
        {
            string methodName = nameof(PushToServer);
            try
            {
                var listeners = ServerChannelConfigurationManager.Instance.Configurations.Listeners;

                foreach (var listener in listeners)
                {
                    try
                    {
                        AWS4Client client = new AWS4Client(listener);
                        Task.Run(() => client.PostData(JsonConvert.SerializeObject(request, Formatting.None)));
                    }
                    catch (Exception ex)
                    {
                        Logging.ServerChannelLogger.Instance.Log.Error(className, methodName, ex.ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                Logging.ServerChannelLogger.Instance.Log.Error(className, methodName, ex.ToString());
            }


        }

        protected override void ChannelRead0(IChannelHandlerContext context, string message)
        {
            string methodName = nameof(ChannelRead0);
            try
            {
                Logging.ServerChannelLogger.Instance.Log.Verbose(className, methodName, string.Format("{0}: [{1}]", context.Channel, message));
                string key = string.Empty;
                var result = ParsingManager.FirstLevelParser(message);

                if(result != null)
                {
                    if (!result.Data.Equals(TerminalHelper.PONG))
                    {
                        var protocol = ProtocolList.Instance.Find(result.ProtocolHeader);
                        if (protocol != null)
                        {
                            if (protocol.ProtocolType == ProtocolType.Monitoring)
                            {
                                var packet = ParsingManager.SecondLevelParser(result);
                                if (packet != null)
                                {
                                    var json = JsonConvert.SerializeObject(packet, Formatting.None);
                                    ChannelManager.Instance.UpdateChannelInfo(context, result.TerminalId);
                                    Console.WriteLine(json);
                                    if (!string.IsNullOrEmpty(json))
                                    {
                                        PushToServer(json);
                                    }
                                }
                            }
                            else if (protocol.ProtocolType == ProtocolType.Control)
                            {
                                ChannelHandler.TerminalCommandReceived(new TerminalCommandReceivedEventArgs
                                {
                                    ChannelKey = result.TerminalId,
                                    Message = message
                                });
                            }
                        }

                    }
                }
                
            }
            catch (Exception ex)
            {

                Logging.ServerChannelLogger.Instance.Log.Error(className, methodName, ex.ToString());
            }

        }

        public override void ChannelReadComplete(IChannelHandlerContext context)
        {
            string methodName = nameof(ChannelReadComplete);
            try
            {
                context.Flush();
            }
            catch (Exception ex)
            {
                Logging.ServerChannelLogger.Instance.Log.Error(className, methodName, ex.ToString());
            }

        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            string methodName = nameof(ExceptionCaught);
            Logging.ServerChannelLogger.Instance.Log.Error(className, methodName, exception.ToString());

        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            string methodName = nameof(ChannelActive);
            try
            {
                base.ChannelActive(context);
                context.WriteAndFlushAsync(TerminalHelper.TimeSync());
                var info = ChannelManager.Instance.AddChannel(context);
                if (info == null)
                    return;

            }
            catch (Exception ex)
            {
                Logging.ServerChannelLogger.Instance.Log.Error(className, methodName, ex.ToString());
            }
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            string methodName = nameof(ChannelInactive);
            try
            {
                base.ChannelInactive(context);
                Thread.Sleep(10);
                ChannelManager.Instance.RemoveChannel(context);
            }
            catch (Exception ex)
            {
                Logging.ServerChannelLogger.Instance.Log.Error(className, methodName, ex.ToString());
            }

        }



    }
}
