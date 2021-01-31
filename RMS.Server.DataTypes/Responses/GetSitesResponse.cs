using RMS.Component.WebApi.Responses;
using RMS.Server.DataTypes.Requests;
using System.Collections.Generic;
using System.Linq;

namespace RMS.Server.DataTypes.Responses
{
    public class GetSitesResponse : BaseResponse
    {
        public GatewayRequestType RequestType { get; set; }
        public IEnumerable<string> Sites { get; set; }
        public int SiteCount => Sites.Count();

        public GetSitesResponse()
        {
            RequestType = GatewayRequestType.GetSites;
        }
    }
}
