using Microsoft.Owin.Hosting;
using RMS.Component.Logging;
using RMS.Server.BusinessLogic;
using RMS.Server.WebApi.Configuration;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace RMS.Server.WebApi
{
    public class WebServer
    {
        private IDisposable _webapp;

        public static EnhancedGateway server = null;
        private WebApiServerConfiguration configurations = null;
        public static ILog WebApiLogger;
        protected static string ClassName => nameof(WebServer);
        protected static string MethodName { get; private set; }
        public WebServer()
        {
            #region Logging
            MethodBase method = MethodBase.GetCurrentMethod();
            MethodName = method.Name;
            #endregion
            configurations = WebApiServerConfigurationManager.Instance.Configurations;
            WebApiLogger = LoggingFactory.CreateLogger(configurations.LogPath, "web-api.log", configurations.LogLevel);           
        }
        public void Start()
        {
            #region Logging
            MethodBase method = MethodBase.GetCurrentMethod();
            MethodName = method.Name;
            #endregion

            WebApiLogger?.Verbose(ClassName, MethodName, "Start");
            WebApiLogger?.Information(ClassName, MethodName, "Starting Service");
            WebApiLogger?.Information(ClassName, MethodName, "Loading configuration");
            var serverChannelLog = LoggingFactory.CreateLogger(configurations.LogPath, "server-channel.log", configurations.LogLevel);
            if (true)
            {
                WebApiLogger?.Information(ClassName, MethodName, "Instantiating TCP Server Channel");
                server = new EnhancedGateway(serverChannelLog);
                Task.Run(() => server.Start());
                WebApiLogger?.Information(ClassName, MethodName, "Starting Job Scheduler");
                Task.Run(() => TaskManager.StartJobScheduler());
            }

            //Task.Run(() => TaskManager.StartJobScheduler());
            _webapp = WebApp.Start<Startup>(configurations.Url);
            WebApiLogger?.Verbose(ClassName, MethodName, "End");
        }

        public void Stop()
        {
            #region Logging
            MethodBase method = MethodBase.GetCurrentMethod();
            MethodName = method.Name;
            #endregion

            WebApiLogger?.Verbose(ClassName, MethodName, "Start");
            WebApiLogger?.Information(ClassName, MethodName, "Stopping Service");
            if (server != null)
            {
                Task.Run(() => server.Stop());
            }

            server = null;
            _webapp?.Dispose();
            WebApiLogger?.Verbose(ClassName, MethodName, "End");
        }
    }
}
