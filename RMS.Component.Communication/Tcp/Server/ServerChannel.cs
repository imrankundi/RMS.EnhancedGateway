using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Handlers.Tls;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using RMS.Component.Common;
using RMS.Component.Communication.Tcp.Client;
using RMS.Component.Communication.Tcp.Common;
using RMS.Component.Communication.Tcp.Event;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Timers;

namespace RMS.Component.Communication.Tcp.Server
{
    public class ServerChannel
    {

        private readonly ServerChannelConfiguration configuration;
        public IServerChannelHandler ServerChannelHandler { get; set; }
        //public ClientChannelManager ClientChannelManager { get; set; }
        
        private Timer timer;
        private const int timerIntervalInSectons = 1;
        public bool IsStarted
        {
            get
            {
                if (boundChannel == null)
                    return false;

                return boundChannel.Open;

            }
        }
        string className = nameof(ServerChannel);

        private void DisconnectAllClientChannels()
        {
            try
            {
                Logging.ServerChannelLogger.Instance.Log.Information(className, "DisconnectAllClientChannels", "Disconnecting clients");


                //var channels = ClientChannelManager?.Channels.Keys.ToArray();
                //foreach (var channel in channels)
                //{
                //    try
                //    {
                //        channel.CloseAsync();
                //    }
                //    catch (Exception ex)
                //    {
                //        Logging.ServerChannelLogger.Instance.Log.Error(className, "DisconnectAllClientChannels",
                //            string.Format("Message: {0}, Details: {1}", ex.Message, ex.ToString()));
                //    }
                //}
            }
            catch (Exception ex)
            {
                Logging.ServerChannelLogger.Instance.Log.Error(className, "DisconnectAllClientChannels",
                    string.Format("Message: {0}, Details: {1}", ex.Message, ex.ToString()));

            }
        }

        public void SynchronizeTerminals()
        {
            ChannelManager.Instance.SynchronizeTerminals();
        }

        public ServerChannel(ServerChannelConfiguration configuration)
        {
            Logging.ServerChannelLogger.Instance.Log.Verbose(className, className, "Constructor fired");
            timer = new Timer();
            timer.Interval = timerIntervalInSectons * 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            //ClientChannelManager = new ClientChannelManager();

            if (configuration == null)
            {

                RaiseError(null, ErrorMessages.Message(ErrorType.NullConfiguration), ErrorType.NullConfiguration);

                Logging.ServerChannelLogger.Instance.Log.Error(className, className, "No configuration found");
                Logging.ServerChannelLogger.Instance.Log.Error(className, className, "Unable to to create proper Router Channel");
                return;
            }

            this.configuration = configuration;
        }

        bool lastServerStartState = false;
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (lastServerStartState != IsStarted)
                {
                    if (ServerChannelHandler == null)
                        return;

                    lastServerStartState = IsStarted;
                    ServerChannelHandler.ServerListeningStateChanged(new ServerChannelEventArgs());

                }
            }
            catch (Exception ex)
            {

            }

        }

        private void RaiseError(ClientContext context, string message, ErrorType errorType)
        {
            if (ServerChannelHandler != null)
            {
                ServerChannelHandler.ServerChannelError(new ServerChannelErrorEventArgs
                {
                    Context = context,
                    Message = message,
                    Details = message,
                    ErrorType = errorType
                });
            }
        }
        private void RaiseError(ClientContext context, Exception ex, ErrorType errorType)
        {
            if (ServerChannelHandler != null)
            {
                ServerChannelHandler.ServerChannelError(new ServerChannelErrorEventArgs
                {
                    Context = context,
                    Message = ex.Message,
                    Details = ex.ToString(),
                    ErrorType = errorType
                });
            }
        }
        private X509Certificate2 GetCertificate()
        {
            return CertificateHelper.GetSystemCertificateByThumbprint(configuration.Thumbprint);
        }

        ServerBootstrap bootstrap;
        IChannel boundChannel;
        IEventLoopGroup bossGroup;
        IEventLoopGroup workerGroup;
        ServerChannelHandler handler;
        public async Task StartAsync()
        {
            bossGroup = new MultithreadEventLoopGroup(1);
            workerGroup = new MultithreadEventLoopGroup();

            var encoder = new StringEncoder();
            var decoder = new StringDecoder();

            try
            {
                bootstrap = new ServerBootstrap();
                bootstrap.Group(bossGroup, workerGroup);
                bootstrap.Channel<TcpServerSocketChannel>();
                bootstrap
                    .Option(ChannelOption.SoBacklog, 100)
                    .Handler(new LoggingHandler("SRV-LSTN"))
                    .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;
                        if (configuration.EnableTls)
                        {
                            X509Certificate2 tlsCertificate = GetCertificate();
                            if (tlsCertificate != null)
                            {
                                pipeline.AddLast("tls", TlsHandler.Server(tlsCertificate));
                            }
                            else
                            {
                                RaiseError(null, ErrorMessages.Message(ErrorType.NoCertificateFoundWhenTlsEnabled),
                                    ErrorType.NoCertificateFoundWhenTlsEnabled);
                            }
                        }


                        pipeline.AddLast("encoder", encoder);
                        pipeline.AddLast("decoder", decoder);

                        handler = new ServerChannelHandler();
                        //handler.ClientChannelManager = ClientChannelManager;
                        handler.ChannelHandler = ServerChannelHandler;
                        pipeline.AddLast("handler", handler);
                    }));

                boundChannel = await bootstrap.BindAsync(configuration.Port);



            }
            catch (Exception ex)
            {
                RaiseError(null, ex, ErrorType.ServerInitialization);
            }
        }

        public async Task StopAsync()
        {
            try
            {
                DisconnectAllClientChannels();
                if (boundChannel != null)
                {
                    await boundChannel.CloseAsync();
                    await boundChannel.DisconnectAsync();
                    await boundChannel.DeregisterAsync();


                    if (!boundChannel.Open)
                    {
                        if (!boundChannel.Registered)
                        {
                            boundChannel = null;
                        }
                    }
                }


                if (bossGroup != null)
                    await Task.WhenAll(bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));
                if (workerGroup != null)
                    await Task.WhenAll(workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));
            }
            catch (Exception ex)
            {
                RaiseError(null, ex.Message, ErrorType.ErrorWhileClosingServerGracefully);
            }
        }

        public async Task Broadcast(string msg)
        {
            var channels = ClientChannelManager.Channels;
            var allChannels = channels.Keys.ToArray();
            foreach (var channel in allChannels)
            {
                await channel.WriteAndFlushAsync(msg);
            }
        }

        public async Task Send(string id, string msg)
        {
            //var channel = ClientChannelManager.FindChannelByKey(id);

            //if (channel == null)
            //    return;
            //await channel.WriteAndFlushAsync(msg);

        }
    }
}
