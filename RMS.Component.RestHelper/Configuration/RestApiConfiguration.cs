using RMS.Component.Configuration;
using System.Collections.Generic;

namespace RMS.Component.RestHelper.Configuration
{
    public class RestApiConfiguration : BaseConfiguration
    {
        public List<RestApi> RestApis { get; set; } = new List<RestApi>();
    }
}
