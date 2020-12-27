using DotNetty.Transport.Channels;
using RMS.Component.Communication.Tcp.Client;
using RMS.Component.Logging;
using System;

namespace RMS.Component.Communication.Tcp
{
    public class ChannelHandler : SimpleChannelInboundHandler<string>
    {
        private const string className = nameof(ChannelHandler);
        private volatile IChannel channel;
        public string ChannelKey { get; set; }
        public IChannel GetChannel() => this.channel;
        public IClientChannelHandler ClientChannelHandler { get; set; }
        public ILog Log { get; set; }
        public override void ChannelActive(IChannelHandlerContext context)
        {
            try
            {
                channel = context.Channel;
                if (ClientChannelHandler != null)
                {
                    ClientChannelHandler.ClientChannelConnected(new ClientChannelConnectedEventArgs
                    {
                        //ChannelId = ClientChannelConfigurationManager.Instance.Configurations.ChannelId,
                        //ChannelKey = ChannelKey,
                        Context = new ClientContext(context)
                    });
                }

                //var msg = new RegisterMessage
                //{
                //    ChannelKey = ChannelKey
                //};
                //var registerMessage = JsonConvert.SerializeObject(msg);

                //context.WriteAndFlushAsync(registerMessage);
                Log?.Verbose(className, "ChannelActive",
                    "Connected with Server");
            }
            catch (Exception ex)
            {
                LogException(context, ex, "ChannelActive");
            }

        }

        private void LogException(IChannelHandlerContext context, Exception ex, string methodName)
        {
            if (ClientChannelHandler != null)
            {
                ClientChannelHandler.ClientChannelError(new ClientChannelErrorEventArgs
                {
                    //ChannelId = ClientChannelConfigurationManager.Instance.Configurations.ChannelId,
                    //ChannelKey = ChannelKey,
                    Context = new ClientContext(context),
                    Message = ex.Message,
                    Details = ex.ToString()
                });
            }
            Log?.Error(className, methodName,
                string.Format("Message: {0}, Details: {1}", ex.Message, ex.ToString()));
        }
        public override void ChannelInactive(IChannelHandlerContext context)
        {
            try
            {
                channel = context.Channel;
                if (ClientChannelHandler != null)
                {
                    ClientChannelHandler.ClientChannelDisconnected(new ClientChannelDisconnectedEventArgs
                    {
                        //ChannelId = ClientChannelConfigurationManager.Instance.Configurations.ChannelId,
                        //ChannelKey = ChannelKey,
                        Context = new ClientContext(context)
                    });
                }
            }
            catch (Exception ex)
            {
                LogException(context, ex, "ChannelInactive");
            }

        }
        protected override void ChannelRead0(IChannelHandlerContext context, string message)
        {
            try
            {
                channel = context.Channel;
                if (ClientChannelHandler != null)
                {
                    ClientChannelHandler.ClientChannelDataReceived(new ClientChannelDataReceivedEventArgs
                    {
                        //ChannelKey = ChannelKey,
                        //ChannelId = ClientChannelConfigurationManager.Instance.Configurations.ChannelId,
                        Context = new ClientContext(context),
                        Message = message
                    });
                }
            }
            catch (Exception ex)
            {
                LogException(context, ex, "ChannelRead0");
            }

        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            base.ExceptionCaught(context, exception);
            try
            {
                channel = context.Channel;
                if (ClientChannelHandler != null)
                {
                    ClientChannelHandler.ClientChannelError(new ClientChannelErrorEventArgs
                    {
                        //ChannelKey = ChannelKey,
                        //ChannelId = ClientChannelConfigurationManager.Instance.Configurations.ChannelId,
                        Context = new ClientContext(context),
                        Message = exception.Message,
                        Details = exception.ToString()
                    });
                }
                //context.CloseAsync();
            }
            catch (Exception ex)
            {
                LogException(context, ex, "ExceptionCaught");
            }
        }

        public override void ChannelWritabilityChanged(IChannelHandlerContext context)
        {
            channel = context.Channel;
            base.ChannelWritabilityChanged(context);
        }
        public override void ChannelRegistered(IChannelHandlerContext context)
        {
            channel = context.Channel;
            base.ChannelRegistered(context);
        }
        public override void ChannelUnregistered(IChannelHandlerContext context)
        {
            channel = context.Channel;
            base.ChannelUnregistered(context);
        }

    }
}
