using RMS.Component.Common;
using RMS.Component.Logging;
using RMS.Component.Logging.Models;
using RMS.Server.WebApi.Configuration;
using System;

namespace RMS.Server.WebApi.Logging
{
    public class Logger
    {
        private readonly WebApiServerConfiguration configuration;
        private static Logger instance = new Logger();
        private const string logFileName = "WebApi.log";
        public ILog Log { get; set; }


        //private List<object> list = new List<object>();


        public static Logger Instance { get { return instance; } }

        private Logger()
        {
            try
            {
                configuration = WebApiServerConfigurationManager.Instance.Configurations;
                Log = LoggingFactory.CreateLogger(configuration.LogPath, logFileName, configuration.LogLevel);
            }
            catch (Exception ex)
            {
                var folder = string.Format(@"{0}\Logs\", AppDirectory.BaseDirectory);
                Log = LoggingFactory.CreateLogger(folder, logFileName, LogLevel.Verbose);

            }
        }
    }

}
