using RMS.Component.WebApi.Responses;
using RMS.Server.DataTypes.Requests;

namespace RMS.Server.DataTypes.Responses
{
    public class TerminalCommandResponse : BaseResponse
    {
        public ServerRequestType RequestType { get; set; }
        public string TerminalId { get; set; }
        public string Data { get; set; }
    }
}
