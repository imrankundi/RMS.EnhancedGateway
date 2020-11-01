using RMS.Component.WebApi.Requests;

namespace RMS.Server.DataTypes.Requests
{
    public class Request : BaseRequest
    {
        public ServerRequestType RequestType { get; set; }
    }
}
