namespace RMS.Server.DataTypes.Requests
{
    public class TokenRequest : Request
    {
        public TokenRequest()
        {
            RequestType = GatewayRequestType.Token;
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        //public int LifeMinutes { get; set; }
    }
}
