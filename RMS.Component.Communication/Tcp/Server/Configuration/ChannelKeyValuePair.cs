using Newtonsoft.Json;

namespace RMS.Component.Communication.Tcp.Server
{
    public class ChannelKeyValuePair
    {
        [JsonProperty("channelKey")]
        public string ChannelKey { get; set; }
    }
}
