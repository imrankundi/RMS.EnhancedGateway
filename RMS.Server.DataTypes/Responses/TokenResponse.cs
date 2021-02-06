using RMS.Component.WebApi.Responses;
using RMS.Server.DataTypes.Requests;
using System.Collections.Generic;
using System.Linq;

namespace RMS.Server.DataTypes.Responses
{
    public class TokenResponse : BaseResponse
    {
        public GatewayRequestType RequestType { get; set; }
        public string Token { get; set; }

        public TokenResponse()
        {
            RequestType = GatewayRequestType.Token;
        }
    }
}
