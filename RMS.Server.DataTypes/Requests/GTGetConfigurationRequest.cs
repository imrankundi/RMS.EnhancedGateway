using RMS.Protocols.GT;

namespace RMS.Server.DataTypes.Requests
{
    public class GTGetConfigurationRequest : Request
    {
        public GTGetConfigurationRequest()
        {
            RequestType = GatewayRequestType.TerminalCommand;
        }

        public string TerminalId { get; set; }
        public GTCommandType CommandType { get; set; }
    }
}
