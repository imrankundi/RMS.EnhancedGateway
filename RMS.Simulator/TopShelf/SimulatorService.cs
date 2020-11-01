using System;
using System.Threading.Tasks;
namespace RMS.Simulator
{
    public class SimulatorService
    {

        WebServer webServer = null;
        //AppCore.ApplicationCoreStatusMonitorAgent applicationCoreAgent = null;
        string className = nameof(SimulatorService);

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
