using Newtonsoft.Json;
using RMS.Component.WebApi.Responses;
using RMS.Server.DataTypes.Requests;

namespace RMS.Server.DataTypes.Responses
{
    public class TerminalCommandResponse : BaseResponse
    {
        [JsonProperty("requestType")]
        public GatewayRequestType RequestType { get; set; }
        [JsonProperty("terminalId")]
        public string TerminalId { get; set; }
        [JsonProperty("data")]
        public object Data { get; set; }
    }
}
