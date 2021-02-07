using Newtonsoft.Json;
using RMS.Component.WebApi.Requests;

namespace RMS.Server.DataTypes.Requests
{
    public class Request : BaseRequest
    {
        [JsonProperty("requestType")]
        public GatewayRequestType RequestType { get; set; }
    }
}
