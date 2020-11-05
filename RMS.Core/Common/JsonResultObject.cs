using Newtonsoft.Json;
using RMS.Core.Enumerations;

namespace RMS.Core.Common
{
    public class JsonResultObject
    {
        [JsonProperty("status")]
        public JsonResultStatus Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("jsonObject")]
        public object JsonObject { get; set; }
    }
}