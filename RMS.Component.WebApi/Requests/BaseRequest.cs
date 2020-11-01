using Newtonsoft.Json;
using System;

namespace RMS.Component.WebApi.Requests
{
    public class BaseRequest : IRequest
    {
        public string RequestId => DateTime.UtcNow.ToString("yyyyMMddHHmmssffffff");
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        public string ToFormattedJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
