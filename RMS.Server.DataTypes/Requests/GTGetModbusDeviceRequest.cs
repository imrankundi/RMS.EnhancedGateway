using RMS.Protocols.GT;

namespace RMS.Server.DataTypes.Requests
{
    public class GTGetModbusDeviceRequest : Request
    {
        public GTGetModbusDeviceRequest()
        {
            RequestType = GatewayRequestType.TerminalCommand;
        }

        public string TerminalId { get; set; }
        public GTCommandType CommandType { get; set; }
        public int StartIndex { get; set; }
        public int NumberOfDevices { get; set; }
    }
}
