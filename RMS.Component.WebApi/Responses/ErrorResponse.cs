using Newtonsoft.Json;

namespace RMS.Component.WebApi.Responses
{
    public class ErrorResponse : BaseResponse
    {
        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }
        [JsonProperty("errorDetails")]
        public string ErrorDetails { get; set; }
    }
}
