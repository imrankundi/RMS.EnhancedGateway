using Newtonsoft.Json;

namespace RMS.Server.DataTypes.Requests
{
    public class TerminalCommandRequest : Request
    {
        public TerminalCommandRequest()
        {
            RequestType = GatewayRequestType.TerminalCommand;
        }
        [JsonProperty("terminalId")]
        public string TerminalId { get; set; }
        [JsonProperty("data")]
        public string Data { get; set; }
    }
}
