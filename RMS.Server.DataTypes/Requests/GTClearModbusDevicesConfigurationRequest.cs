using Newtonsoft.Json;
using RMS.Protocols.GT;

namespace RMS.Server.DataTypes.Requests
{
    public class GTClearModbusDevicesConfigurationRequest : Request
    {
        public GTClearModbusDevicesConfigurationRequest()
        {
            RequestType = GatewayRequestType.TerminalCommand;
            CommandType = GTCommandType.ClearAllModbusDevices;
        }
        [JsonProperty("terminalId")]
        public string TerminalId { get; set; }
        [JsonProperty("commandType")]
        public GTCommandType CommandType { get; set; }
    }
}
