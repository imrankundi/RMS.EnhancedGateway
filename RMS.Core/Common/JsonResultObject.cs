using RMS.Core.Enumerations;
using Newtonsoft.Json;

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