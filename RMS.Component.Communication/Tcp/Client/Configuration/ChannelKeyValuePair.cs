using Newtonsoft.Json;

namespace RMS.Component.Communication.Tcp.Client
{
    public class ChannelKeyValuePair
    {
        [JsonProperty("channelKey")]
        public string ChannelKey { get; set; }
    }
}
