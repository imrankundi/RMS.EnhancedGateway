using Newtonsoft.Json;

namespace RMS.Server.DataTypes.Requests
{
    public class TokenRequest : Request
    {
        public TokenRequest()
        {
            RequestType = GatewayRequestType.Token;
        }
        [JsonProperty("username")]
        public string UserName { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        //public int LifeMinutes { get; set; }
    }
}
