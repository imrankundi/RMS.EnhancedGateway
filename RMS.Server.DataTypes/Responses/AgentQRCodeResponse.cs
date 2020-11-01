using Newtonsoft.Json;
using RMS.Component.WebApi.Responses;

namespace RMS.Server.DataTypes.Responses
{
    public class AgentQRCodeResponse : BaseResponse
    {
        [JsonProperty("encryptedQRCode")]
        public string EncryptedQRCode { get; set; }
    }
}
