using Newtonsoft.Json;
using RMS.Component.Common;
using RMS.Component.Common.Helpers;
using RMS.Component.Configuration;
using RMS.Component.Wmi;
using System;
using System.IO;
using System.Linq;

namespace RMS.Component.Communication.Tcp.Server
{
    public class ServerChannelConfigurationManager
    {
        private const string fileName = "ServerChannelConfig";
        private const string directory = @"Tcp\Configuration";
        private const string channelKeyFilename = "ChannelKey.json";
        private ServerChannelConfigurationManager()
        {
            Configurations = LoadConfiguration();
            if (Configurations == null)
                IsConfigurationLoaded = false;
            else
                IsConfigurationLoaded = true;
        }
        static ServerChannelConfigurationManager() { }
        public static ServerChannelConfigurationManager Instance { get; } = new ServerChannelConfigurationManager();
        public bool IsConfigurationLoaded { get; } = false;
        public ServerChannelConfiguration Configurations { get; private set; }
        private ServerChannelConfiguration LoadConfiguration()
        {
            var configurtionLoader = new ConfigurtionLoader<ServerChannelConfiguration>(fileName, directory);
            var configuration = configurtionLoader.LoadConfiguration();
            configuration.ChannelKey = GetChannelKey();
            return configuration;
        }

        private string GenerateChannelKey()
        {
            string channelKey = string.Empty;
            var processor = WmiObjectCreator.QueryObject<Processor>().FirstOrDefault();
            var bios = WmiObjectCreator.QueryObject<Bios>().FirstOrDefault();
            var baseBoard = WmiObjectCreator.QueryObject<BaseBoard>().FirstOrDefault();

            channelKey = string.Format("{0}:{1}:{2}", bios.SerialNumber, baseBoard.SerialNumber, processor.ProcessorId);
            return channelKey;
        }

        private string GetChannelKey()
        {
            string filePath = string.Format(@"{0}\{1}\{2}", AppDirectory.BaseDirectory, directory, channelKeyFilename);
            string channelKey;
            if (!File.Exists(filePath))
            {
                channelKey = GenerateChannelKey();
                WriteChannelKeyFile(channelKey, filePath);
                return channelKey;
            }

            channelKey = LoadChannelKeyFromFile(filePath);
            return channelKey;
        }

        private string LoadChannelKeyFromFile(string filePath)
        {
            try
            {
                string text = File.ReadAllText(filePath);
                if (string.IsNullOrEmpty(text))
                    return string.Empty;

                var pair = JsonConvert.DeserializeObject<ChannelKeyValuePair>(text);

                if (pair != null)
                    return pair.ChannelKey;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }

            return string.Empty;
        }
        private bool WriteChannelKeyFile(string channelKey, string filePath)
        {

            try
            {
                ChannelKeyValuePair pair = new ChannelKeyValuePair();
                pair.ChannelKey = channelKey;
                FileHelper.WriteAllText(filePath, JsonConvert.SerializeObject(pair, Formatting.Indented));
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }

}
