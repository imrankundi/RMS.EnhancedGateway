using Newtonsoft.Json;

namespace RMS.Server.DataTypes.Requests
{
    public class DeleteSiteRequest : Request
    {
        public DeleteSiteRequest()
        {
            RequestType = GatewayRequestType.TerminalCommand;
        }
        [JsonProperty("terminalId")]
        public string TerminalId { get; set; }
        [JsonProperty("commandType")]
        public GTCommandType CommandType { get; set; }
        [JsonProperty("data")]
        public object Data { get; set; }
    }
}
