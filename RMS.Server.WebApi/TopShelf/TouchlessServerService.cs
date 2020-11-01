using System;
using System.Threading.Tasks;
namespace RMS.Server.WebApi
{
    public class TouchlessServerService
    {

        WebServer webServer = null;
        //AppCore.ApplicationCoreStatusMonitorAgent applicationCoreAgent = null;
        string className = nameof(TouchlessServerService);

        public async Task Start()
        {
            try
            {
                webServer = new WebServer();
                webServer.Start();
            }
            catch (Exception ex)
            {
            }

        }

        public async Task Stop()
        {
            try
            {
                if (webServer != null)
                    webServer.Stop();

                webServer = null;
            }
            catch (Exception ex)
            {
            }

        }
    }
}
