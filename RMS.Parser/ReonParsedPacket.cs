using Newtonsoft.Json;
using RMS.Core.Enumerations;
using RMS.Core.QueryBuilder;
using System;
using System.Collections.Generic;

namespace RMS.Parser
{
    public class ReonParsedPacket
    {
        [JsonIgnore]
        public string ProtocolHeader { get; set; }
        [JsonIgnore]
        public ProtocolType ProtocolType { get; set; }
        [JsonProperty("siteId")]
        public string TerminalId { get; set; }
        [JsonProperty("mapping")]
        public string Mapping { get; set; }
        [JsonProperty("ingester")]
        public string Ingester => "SALTEC";
        [JsonIgnore]
        public string Id { get; set; }
        [JsonIgnore]
        public int PageNumber { get; internal set; }
        [JsonIgnore]
        public DateTime ReceivedOn { get; internal set; }
        [JsonProperty("data")]
        public Dictionary<string, object> Data { get; private set; }

        public ReonParsedPacket()
        {
            Data = new Dictionary<string, object>();
        }
    }
}
