using DotNetty.Codecs;
using DotNetty.Handlers.Tls;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using RMS.Component.Common;
using RMS.Component.Communication.Tcp.Common;
using RMS.Component.Communication.Tcp.Logging;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace RMS.Component.Communication.Tcp.Client
{
    public class ClientChannel
    {
        string className = nameof(ClientChannel);
        MultithreadEventLoopGroup group;
        Bootstrap bootstrap;
        StringEncoder encoder;
        StringDecoder decoder;
        IChannelHandler channelHandler;
        string targetHost = string.Empty;
        ChannelHandler handler;

        ClientChannelConfiguration configuration;
        IChannel clientChannel;
        public bool IsConnected
        {
            get
            {
                if (clientChannel == null)
                    return false;

                return clientChannel.Open;
            }
        }

        public IClientChannelHandler ClientChannelHandler { get; set; }

        public ClientChannel(ClientChannelConfiguration configuration)
        {
            if (configuration == null)
            {
                RaiseError(null, ErrorMessages.Message(ErrorType.NullConfiguration), ErrorType.NullConfiguration);
                return;
            }

            this.configuration = configuration;
        }

        private void RaiseError(IChannelHandlerContext context, string message, ErrorType errorType)
        {
            if (ClientChannelHandler != null)
            {
                ClientChannelHandler.ClientChannelError(new ClientChannelErrorEventArgs
                {
                    Context = new ClientContext(context),
                    Message = message,
                    Details = message,
                    ErrorType = errorType
                });
            }
        }
        private void RaiseError(IChannelHandlerContext context, Exception ex, ErrorType errorType)
        {
            if (ClientChannelHandler != null)
            {
                ClientChannelHandler.ClientChannelError(new ClientChannelErrorEventArgs
                {
                    Context = new ClientContext(context),
                    Message = ex.Message,
                    Details = ex.ToString(),
                    ErrorType = errorType
                });
            }
        }
        private X509Certificate2 GetCertificate()
        {
            string thumbprint = configuration.Thumbprint;
            if (string.IsNullOrWhiteSpace(thumbprint))
            {
                ClientChannelLogger.Instance.Log.Information(className, "GetCertificate", "No thumbprint found in configuration");
                return null;

            }
            ClientChannelLogger.Instance.Log.Information(className, "GetCertificate", string.Format("Fetching Installed System Certificate, Thumbprint = [{0}]", thumbprint));
            var certificate = CertificateHelper.GetSystemCertificateByThumbprint(configuration.Thumbprint);

            if (certificate != null)
                ClientChannelLogger.Instance.Log.Information(className, "GetCertificate", "Certificate found");

            return certificate;
        }

        private IChannelHandler CreateChannelHandler()
        {
            IChannelHandler channelHandler = null;
            try
            {
                channelHandler = new ActionChannelInitializer<ISocketChannel>(channel =>
                {
                    IChannelPipeline pipeline = channel.Pipeline;

                    if (configuration.EnableTls)
                    {
                        X509Certificate2 cert = GetCertificate();
                        if (cert != null)
                        {
                            targetHost = cert.GetNameInfo(X509NameType.DnsName, false);
                            if (string.IsNullOrEmpty(targetHost))
                            {
                                RaiseError(null, ErrorMessages.Message(ErrorType.TargetHostDnsNameNotFound),
                                ErrorType.TargetHostDnsNameNotFound);
                                throw new NullReferenceException(ErrorMessages.Message(ErrorType.TargetHostDnsNameNotFound));
                            }

                            TlsHandlerFactory tlsFactory = new TlsHandlerFactory();
                            TlsHandler tlsHandler = tlsFactory.CreateStandardTlsHandler(targetHost);
                            pipeline.AddLast("tls", tlsHandler);
                            //pipeline.AddLast("tls", new TlsHandler(stream => new SslStream(stream, true, (sender, certificate, chain, errors) => true), new ClientTlsSettings(targetHost)));
                            //}

                        }
                        else
                        {
                            RaiseError(null, ErrorMessages.Message(ErrorType.NoCertificateFoundWhenTlsEnabled),
                                ErrorType.NoCertificateFoundWhenTlsEnabled);
                            throw new NullReferenceException(ErrorMessages.Message(ErrorType.NoCertificateFoundWhenTlsEnabled));

                            // add intialization error here
                        }
                    }

                    pipeline.AddLast("encoder", encoder);
                    pipeline.AddLast("decoder", decoder);

                    handler = new ChannelHandler();
                    handler.ChannelKey = configuration.ChannelKey;
                    handler.ClientChannelHandler = ClientChannelHandler;
                    pipeline.AddLast("echo", handler);
                });
            }
            catch (Exception ex)
            {
                channelHandler = null;
                ClientChannelLogger.Instance.Log.Error(className, "CreateChannelHandler", ex.ToString());
            }

            return channelHandler;
        }
        public bool Initialize()
        {
            ClientChannelLogger.Instance.Log.Verbose(className, "Initialize", "Initializing Cxp Server Channel");

            channelHandler = CreateChannelHandler();
            if (channelHandler == null)
            {
                ClientChannelLogger.Instance.Log.Information(className, "Initialize", "Failed to Initialize Cxp Server Channel");
                return false;

            }


            bootstrap = new Bootstrap();
            group = new MultithreadEventLoopGroup();
            encoder = new StringEncoder();
            decoder = new StringDecoder();

            bootstrap
                    .Group(group)
                    .Channel<TcpSocketChannel>()
                    .Option(ChannelOption.TcpNodelay, true)
                    .Handler(channelHandler);

            ClientChannelLogger.Instance.Log.Information(className, "Initialize", "Cxp Server Channel Initialized");
            return true;
        }
        public async Task ConnectAsync()
        {
            try
            {
                ClientChannelLogger.Instance.Log.Information(className, "ConnectAsync", string.Format("Connecting to Cxp Server. IP = {0}", configuration.EndPoint));
                clientChannel = await bootstrap.ConnectAsync(configuration.EndPoint);
            }
            catch (Exception ex)
            {
                RaiseError(null, ex, ErrorType.ClientInitialization);
                Cleanup();
            }

        }

        private void Cleanup()
        {
            if (group != null)
                group.ShutdownGracefullyAsync();

            group = null;
            encoder = null;
            decoder = null;
            channelHandler = null;
            bootstrap = null;
        }

        public async Task SendAsync(string msg)
        {
            await clientChannel.WriteAndFlushAsync(msg);
        }

        public async Task DisconnectAsync()
        {
            if (clientChannel != null)
            {
                try
                {
                    await clientChannel.DisconnectAsync();
                    await clientChannel.CloseAsync();
                }
                catch (Exception)
                {

                }
            }

        }
    }
}
