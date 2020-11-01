using System.Collections.Generic;

namespace RMS.Component.RestHelper.Configuration
{
    public class RestApi
    {
        public string Name { get; set; }
        public string BaseUrl { get; set; }
        public Dictionary<string, string> Apis { get; set; } = new Dictionary<string, string>();
    }
}
