using Newtonsoft.Json;

namespace RMS.Component.WebApi.Responses
{
    public class BaseResponse : IResponse
    {
        [JsonProperty("requestId")]
        public string RequestId { get; set; }
        [JsonProperty("responseType")]
        public ResponseType ResponseType { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        public string ToFormattedJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public void Success(string message = null)
        {
            Message = message != null ? message : null;
            ResponseType = ResponseType.Success;
        }

        public void Failure(string message = null)
        {
            Message = message != null ? message : null;
            ResponseType = ResponseType.Failed;
        }
    }
}
