using DotNetty.Transport.Channels;
using RMS.Component.Communication.Tcp.Client;
using RMS.Component.Communication.Tcp.Event;
using System;
using System.Threading;

namespace RMS.Component.Communication.Tcp.Server
{
    public class ServerChannelHandler : SimpleChannelInboundHandler<string>
    {

        public IServerChannelHandler ChannelHandler { get; set; }

        public ServerChannelHandler()
        {
        }
        protected override void ChannelRead0(IChannelHandlerContext context, string message)
        {
            try
            {
                string key = string.Empty;

                var channelInfo = ClientChannelManager.FindChannelInfo(context);
                channelInfo.LastDataReceived = DateTime.UtcNow;

                if (ChannelHandler != null)
                {
                    ChannelHandler.ServerChannelDataReceived(new ServerChannelDataReceivedEventArgs
                    {
                        ChannelId = channelInfo.ChannelId,
                        Context = new Client.ClientContext(context),
                        Message = message
                    });
                }
            }
            catch (Exception ex)
            {

                Logging.ClientChannelLogger.Instance.Log.Error("TcpServerChannelHandler", "ChannelRead0",
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
                Logging.ClientChannelLogger.Instance.Log.Error("TcpServerChannelHandler", "ChannelReadComplete",
                    string.Format("Message: {0}, Details: {1}", ex.Message, ex.ToString()));
            }

        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            base.ExceptionCaught(context, exception);
            var channelInfo = ClientChannelManager.FindChannelInfo(context);
            try
            {
                ChannelHandler.ServerChannelError(new ServerChannelErrorEventArgs
                {
                    ChannelId = channelInfo.ChannelId,
                    Context = new Client.ClientContext(context),
                    Message = exception.Message
                });
            }
            catch (Exception ex)
            {
                Logging.ClientChannelLogger.Instance.Log.Error("TcpServerChannelHandler", "ExceptionCaught",
                    string.Format("Message: {0}, Details: {1}", ex.Message, ex.ToString()));
            }

        }

        public override void ChannelActive(IChannelHandlerContext context)
        {
            try
            {
                base.ChannelActive(context);
                var info = ClientChannelManager.Add(context);
                if (info == null)
                    return;

                ClientChannelManager.RegisterChannelKey(context, info.ChannelKey);

                if (ChannelHandler != null)
                {
                    ChannelHandler.ServerChannelConnected(new ServerChannelConnectedEventArgs
                    {
                        //ChannelKey = info.ChannelKey,
                        ChannelId = info.ChannelId,
                        Context = new Client.ClientContext(context)
                    });
                }

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

                var info = ClientChannelManager.FindChannelInfo(context);
                Thread.Sleep(10);
                ClientChannelManager.Remove(context);
                ClientChannelManager.UnrigisterChannelKey(info.ChannelKey);
                if (ChannelHandler != null)
                {
                    ChannelHandler.ServerChannelDisconnected(new ServerChannelDisconnectedEventArgs
                    {
                        ChannelId = info.ChannelId,
                        //ChannelKey = info.ChannelKey,
                        Context = new Client.ClientContext(context)
                    });
                }
            }
            catch (Exception ex)
            {
                Logging.ClientChannelLogger.Instance.Log.Error("TcpServerChannelHandler", "ChannelInactive",
                    string.Format("Message: {0}, Details: {1}", ex.Message, ex.ToString()));
            }

        }

        

    }
}
