using Microsoft.Owin;
using RMS.Component.WebApi.Controller;
using RMS.Component.WebApi.Responses;
using RMS.Server.BusinessLogic;
using RMS.Server.DataTypes;
using RMS.Server.WebApi.Configuration;
using System.Net.Http;
using System.Web;
using RMS.Parser.Configuration;

namespace RMS.Server.WebApi.Controller
{

    public class ConfigurationController : BaseController
    {
        // POST api/demo 
        public override IResponse Post(object request)
        {
            DeviceConfigurationManager.Instance.Add(new ConfigurationPacket { Packet = "SP000190", TerminalId = "SP000190" });
            WebServer.server.Notify(request);


            var configurations = WebApiServerConfigurationManager.Instance.Configurations;
            IOwinContext owinContext = Request.GetOwinContext();
            string clientIp = owinContext.Environment["server.RemoteIpAddress"] as string;
            return RequestHandler.HandleRequest(request, new CommunicationContext { IP = clientIp });
        }

        public IResponse Configure(object request)
        {
            return Post(request);
        }

    }
}
