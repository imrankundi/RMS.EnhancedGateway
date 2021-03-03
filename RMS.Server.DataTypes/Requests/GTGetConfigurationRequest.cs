using Newtonsoft.Json;

namespace RMS.Server.DataTypes.Requests
{
    public class GTGetConfigurationRequest : Request
    {
        public GTGetConfigurationRequest()
        {
            RequestType = GatewayRequestType.TerminalCommand;
        }
        [JsonProperty("terminalId")]
        public string TerminalId { get; set; }
        [JsonProperty("commandType")]
        public GTCommandType CommandType { get; set; }
    }
}
