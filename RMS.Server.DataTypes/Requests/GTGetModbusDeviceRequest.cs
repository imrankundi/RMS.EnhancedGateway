using Newtonsoft.Json;
using RMS.Protocols.GT;

namespace RMS.Server.DataTypes.Requests
{
    public class GTGetModbusDeviceRequest : Request
    {
        public GTGetModbusDeviceRequest()
        {
            RequestType = GatewayRequestType.TerminalCommand;
        }
        [JsonProperty("terminalId")]
        public string TerminalId { get; set; }
        [JsonProperty("commandType")]
        public GTCommandType CommandType { get; set; }
        [JsonProperty("startIndex")]
        public int StartIndex { get; set; }
        [JsonProperty("numberOfDevices")]
        public int NumberOfDevices { get; set; }
    }
}
