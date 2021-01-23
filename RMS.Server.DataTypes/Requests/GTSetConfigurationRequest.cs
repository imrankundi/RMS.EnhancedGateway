using RMS.Protocols.GT;

namespace RMS.Server.DataTypes.Requests
{
    public class GTSetConfigurationRequest : Request
    {
        public GTSetConfigurationRequest()
        {
            RequestType = GatewayRequestType.TerminalCommand;
        }

        public string TerminalId { get; set; }
        public GTCommandType CommandType { get; set; }
        public object Data { get; set; }
    }
}
