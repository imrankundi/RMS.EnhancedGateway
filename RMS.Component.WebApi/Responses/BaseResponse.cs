using Newtonsoft.Json;

namespace RMS.Component.WebApi.Responses
{
    public class BaseResponse : IResponse
    {
        [JsonProperty("requestId")]
        public string RequestId { get; set; }
        [JsonProperty("responseStatus")]
        public ResponseStatus ResponseStatus { get; set; }
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
            ResponseStatus = ResponseStatus.Success;
        }

        public void Failure(string message = null)
        {
            Message = message != null ? message : null;
            ResponseStatus = ResponseStatus.Failed;
        }
    }
}
