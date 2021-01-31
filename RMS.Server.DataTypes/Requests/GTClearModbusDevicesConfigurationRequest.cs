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

        public string TerminalId { get; set; }
        public GTCommandType CommandType { get; set; }
    }
}
