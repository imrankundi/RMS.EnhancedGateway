using Microsoft.Owin.Hosting;
using RMS.Server.WebApi.Configuration;
using System;
using System.Threading.Tasks;

namespace RMS.Server.WebApi
{
    public class WebServer
    {
        private IDisposable _webapp;

        public static EnhancedGateway server = null;
        private WebApiServerConfiguration configurations = null;

        public void Start()
        {
            configurations = WebApiServerConfigurationManager.Instance.Configurations;
            if (true)
            {
                server = new EnhancedGateway();
                Task.Run(() => server.Start());
            }

            //Task.Run(() => TaskManager.StartJobScheduler());
            _webapp = WebApp.Start<Startup>(configurations.Url);
        }

        public void Stop()
        {
            if (server != null)
            {
                Task.Run(() => server.Stop());
            }

            server = null;
            _webapp?.Dispose();
        }
    }
}
