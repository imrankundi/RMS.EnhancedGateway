using DotNetty.Transport.Channels;
using System.Net;

namespace RMS.Component.Communication.Tcp.Client
{
    public class ClientContext
    {
        public IChannelHandlerContext Context { get; }

        public ClientContext(IChannelHandlerContext context)
        {
            Context = context;
        }
        public void Send(string message)
        {
            Context.WriteAndFlushAsync(message);
        }

        public EndPoint EndPoint => Context.Channel.RemoteAddress;
        public IPAddress IPAddress => ((IPEndPoint)EndPoint).Address;
        public string IP
        {
            get
            {
                IPAddress ip = IPAddress.MapToIPv4();
                return ip.ToString();
            }
        }
        public int Port => ((IPEndPoint)EndPoint).Port;
    }
}
