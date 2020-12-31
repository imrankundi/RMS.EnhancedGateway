using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Handlers.Tls;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using RMS.Component.Common;
using RMS.Component.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Timers;

namespace RMS.Component.Communication.Tcp.Server
{
    public class ServerChannel
    {

        private readonly ServerChannelConfiguration configuration;
        public ITerminalCommandHandler ServerChannelHandler { get; set; }
        //public ClientChannelManager ClientChannelManager { get; set; }

        public ICollection<string> ChannelKeys => ChannelManager.Instance.ChannelKeys;
        public ILog Log { get; set; }

        private Timer timer;
        private const int timerIntervalInSeconds = 1;
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
                Log?.Information(className, "DisconnectAllClientChannels", "Disconnecting clients");


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
                Log?.Error(className, "DisconnectAllClientChannels",
                    string.Format("Message: {0}, Details: {1}", ex.Message, ex.ToString()));

            }
        }

        public void SynchronizeTerminals()
        {
            ChannelManager.Instance.SynchronizeTerminals();
        }

        public ServerChannel(ServerChannelConfiguration configuration)
        {
            Log?.Verbose(className, className, "Constructor fired");
            timer = new Timer();
            timer.Interval = timerIntervalInSeconds * 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            //ClientChannelManager = new ClientChannelManager();

            if (configuration == null)
            {

                Log?.Error(className, className, "No configuration found");
                Log?.Error(className, className, "Unable to to create proper Router Channel");
                return;
            }

            this.configuration = configuration;
        }

        bool lastServerStartState = false;
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {


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
            string methodName = MethodBase.GetCurrentMethod().Name;
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

                                Log?.Error(className, methodName, "Certificate not found");
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
                Log?.Error(className, methodName, ex.ToString());
            }
        }

        public async Task StopAsync()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
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
                Log?.Error(className, methodName, ex.ToString());
            }
        }

        public async Task Broadcast(string msg)
        {
            var channels = ChannelManager.Channels;
            var allChannels = channels.Keys.ToArray();
            foreach (var channel in allChannels)
            {
                await channel.WriteAndFlushAsync(msg);
            }
        }

        public async Task Send(string key, string message)
        {
            var channel = ChannelManager.Instance.FindChannelByKey(key);

            if (channel == null)
                return;

            await channel.WriteAndFlushAsync(message);

        }
    }
}
