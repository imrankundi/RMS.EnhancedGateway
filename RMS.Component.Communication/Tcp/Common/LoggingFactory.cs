using RMS.Component.Common;
using RMS.Component.Communication.Tcp.Client;
using RMS.Component.Communication.Tcp.Server;
using RMS.Component.Logging;
using RMS.Component.Logging.Models;
using System;

namespace RMS.Component.Communication.Tcp.Logging
{
    public class ServerChannelLogger
    {
        private static ServerChannelLogger instance = new ServerChannelLogger();
        private const string logFileName = "ServerChannel.log";

        private ServerChannelConfiguration configurations = ServerChannelConfigurationManager.Instance.Configurations;
        public ILog Log { get; set; }


        //private List<object> list = new List<object>();


        public static ServerChannelLogger Instance { get { return instance; } }

        private ServerChannelLogger()
        {
            try
            {

                Log = LoggingFactory.CreateLogger(configurations.LogPath, logFileName, configurations.LogLevel);
            }
            catch (Exception ex)
            {
                string defaultLogFolder = string.Format(@"{0}\Log\", AppDirectory.BaseDirectory);
                Log = LoggingFactory.CreateLogger(defaultLogFolder, logFileName, LogLevel.Verbose);

            }
        }
    }
    public class ClientChannelLogger
    {
        private static ClientChannelLogger instance = new ClientChannelLogger();
        private const string logFileName = "ClientChannel.log";
        private string defaultLogFolder = string.Format(@"{0}\Log\", AppDirectory.BaseDirectory);
        public ILog Log { get; set; }


        //private List<object> list = new List<object>();


        public static ClientChannelLogger Instance { get { return instance; } }

        private ClientChannelLogger()
        {
            string folder = string.Empty;
            try
            {
                var configurations = ClientChannelConfigurationManager.Instance.Configurations;
                if (configurations == null)
                {
                    folder = string.Format(@"{0}\Log\", AppDirectory.BaseDirectory);
                }
                else
                {
                    folder = configurations.LogPath;
                }
                Log = LoggingFactory.CreateLogger(folder, logFileName, configurations.LogLevel);
            }
            catch (Exception ex)
            {
                //Log = LoggingFactory.CreateLogger(defaultLogFolder, logFileName, LogLevel.Verbose);

            }
        }
    }
}
