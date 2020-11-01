using RMS.Component.WebApi.Controller;
using RMS.Component.WebApi.Responses;

namespace RMS.Simulator
{
    public class SimulatorController : BaseController
    {
        // POST api/demo 
        public override IResponse Post(object request)
        {
            //var configurations = WebApiServerConfigurationManager.Instance.Configurations;
            //IOwinContext owinContext = Request.GetOwinContext();
            //string clientIp = owinContext.Environment["server.RemoteIpAddress"] as string;
            return Main.Server.RequestHandler.HandleRequest(request);

        }

        public IResponse Notify(object request)
        {
            return Post(request);
        }

    }
}
