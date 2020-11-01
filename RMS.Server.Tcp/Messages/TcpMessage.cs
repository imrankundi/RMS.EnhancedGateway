using Newtonsoft.Json;

namespace RMS.Server.Tcp.Messages
{
    public class TcpMessage
    {
        public MessageType MessageType { get; set; }
        public string ChannelId { get; set; }
        public string ChannelKey { get; set; }
        public object Data { get; set; }
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public virtual string ToFormattedJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public override string ToString()
        {
            return ToJson();
        }
    }
}
