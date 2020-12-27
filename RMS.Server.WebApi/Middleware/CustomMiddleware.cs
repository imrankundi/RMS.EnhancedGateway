using Microsoft.Owin;
using RMS.Server.WebApi.Common;
using RMS.Server.WebApi.Configuration;
using System;
using System.Threading.Tasks;

namespace RMS.Server.WebApi.Middlewares
{
    public class CustomMiddleware : OwinMiddleware
    {
        private readonly WebApiServerConfiguration configuration;
        public CustomMiddleware(OwinMiddleware next) : base(next)
        {
            configuration = WebApiServerConfigurationManager.Instance.Configurations;
            //Logger.Instance.Log.Verbose("CustomMiddleware", "CustomMiddleware", "Instantiated");
        }

        public async override Task Invoke(IOwinContext context)
        {
            //Logger.Instance.Log.Verbose("CustomMiddleware", "Task", "Entered");
            context.Response.Headers[Constant.Headers.MachineName] = Environment.MachineName;
            context.Response.Headers[Constant.Headers.CompanyName] = Constant.Compnay.Name;

            // add ip restriction here to allow which ip can request agent
            string clientIp = context.Environment["server.RemoteIpAddress"] as string;
            //Logger.Instance.Log.Information("CustomMiddleware", "Task", string.Format("Client IP: {0}", clientIp));


            await Next.Invoke(context);
            //Logger.Instance.Log.Verbose("CustomMiddleware", "Task", "Exit");
        }
    }
}
