using Newtonsoft.Json;
using RMS.Component.WebApi.Responses;
using RMS.Server.DataTypes.Requests;
using System.Collections.Generic;
using System.Linq;

namespace RMS.Server.DataTypes.Responses
{
    public class GetSitesResponse : BaseResponse
    {
        [JsonProperty("requestType")]
        public GatewayRequestType RequestType { get; set; }
        [JsonProperty("sites")]
        public IEnumerable<string> Sites { get; set; }
        [JsonProperty("siteCount")]
        public int SiteCount => Sites.Count();

        public GetSitesResponse()
        {
            RequestType = GatewayRequestType.GetSites;
        }
    }
}
