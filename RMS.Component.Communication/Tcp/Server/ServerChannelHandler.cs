using DotNetty.Transport.Channels;
using Newtonsoft.Json;
using RMS.AWS;
using RMS.Component.DataAccess.SQLite;
using RMS.Component.DataAccess.SQLite.Entities;
using RMS.Component.Logging;
using RMS.Core.Enumerations;
using RMS.Gateway;
using RMS.Parser;
using RMS.Protocols;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RMS.Component.Communication.Tcp.Server
{
    public class ServerChannelHandler : SimpleChannelInboundHandler<string>
    {
        private string className = nameof(ServerChannelHandler);
        public ITerminalCommandHandler ChannelHandler { get; set; }
        public ILog Log { get; set; }
        public ServerChannelHandler()
        {
        }

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
                        Log?.Error(className, methodName, ex.ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                Log?.Error(className, methodName, ex.ToString());
            }


        }

        protected override void ChannelRead0(IChannelHandlerContext context, string message)
        {
            string methodName = nameof(ChannelRead0);

            Log?.Verbose(className, methodName, string.Format("{0}: [{1}]", context.Channel, message));

            try
            {
                var info = ChannelManager.Instance.FindChannelInfo(context);
                if (info != null)
                {

                    string filter = string.Empty;
                    int filterLenght = 3;
                    if (!string.IsNullOrEmpty(message))
                    {
                        if (message.Length > filterLenght)
                        {
                            filter = message.Substring(message.Length - filterLenght, filterLenght);
                            if (!filter.Contains(">"))
                            {
                                Log?.Warning(className, methodName, string.Format("Partial Packet Received {0}: [{1}]", context.Channel, message));
                                info.MessageBuffer.Append(message);
                                info.PartialPacket = true;
                                return;
                            }
                            else
                            {
                                info.MessageBuffer.Append(message);

                                message = info.MessageBuffer.ToString();
                                info.MessageBuffer.Clear();
                                if (info.PartialPacket)
                                {
                                    Log?.Warning(className, methodName, string.Format("Merging Partial Packets {0}: [{1}]", context.Channel, message));
                                }
                                info.PartialPacket = false;
                            }
                        }
                        else
                        {
                            info.MessageBuffer.Append(message);
                            info.PartialPacket = false;
                            message = info.MessageBuffer.ToString();
                            info.MessageBuffer.Clear();
                            Log?.Warning(className, methodName, string.Format("Merging Partial Packets {0}: [{1}]", context.Channel, message));

                        }
                    }
                }

                string key = string.Empty;
                var result = ParsingManager.FirstLevelParser(message);

                if (result != null)
                {
                    if (!result.Data.Equals(TerminalHelper.PONG))
                    {
                        ReceivedPacketEntity entity = new ReceivedPacketEntity
                        {
                            ReceivedOn = result.ReceivedOn,
                            Data = result.Data,
                            ProtocolHeader = result.ProtocolHeader,
                            TerminalId = result.TerminalId
                        };
                        ReceivedPacketRepository.Save(entity);
                        //RMS.Component.Communication.Logging.Logger.Instance.Log.Write(JsonConvert.SerializeObject(entity));

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
                                        PushToServer(packet);
                                    }
                                }
                            }
                            else if (protocol.ProtocolType == ProtocolType.Control)
                            {
                                if (protocol.DeviceType == DeviceType.Saltec)
                                {
                                    ChannelHandler.TerminalCommandReceived(new TerminalCommandReceivedEventArgs
                                    {
                                        ChannelKey = result.TerminalId,
                                        Message = message
                                    });
                                }
                                else if (protocol.DeviceType == DeviceType.Modbus)
                                {
                                    //string.Format("{0}<{1}({2})>\r", packet.TerminalId, packet.ProtocolHeader, packet.Data);
                                    ChannelHandler.TerminalCommandReceived(new TerminalCommandReceivedEventArgs
                                    {
                                        ChannelKey = result.TerminalId,
                                        Message = string.Format("{0}<{1}({2})>\r", result.TerminalId, result.ProtocolHeader, result.Data)
                                    });
                                }

                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {

                Log?.Error(className, methodName, ex.ToString());
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
                Log?.Error(className, methodName, ex.ToString());
            }

        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            string methodName = nameof(ExceptionCaught);
            Log?.Error(className, methodName, exception.ToString());

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
                Log?.Error(className, methodName, ex.ToString());
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
                Log?.Error(className, methodName, ex.ToString());
            }

        }



    }
}
