using Newtonsoft.Json;
using RMS.Component.Common;
using RMS.Component.Configuration;
using RMS.Component.Wmi;
using System;
using System.IO;
using System.Linq;

namespace RMS.Component.Communication.Tcp.Client
{
    public class ClientChannelConfigurationManager
    {
        //private const string ConfigFolder = nameof(ConfigFolder);
        private const string filename = "ClientChannelConfig";
        private const string directory = @"Tcp\Configuration";
        private const string channelKeyFilename = "ChannelKey.json";
        private ClientChannelConfigurationManager()
        {
            Configurations = LoadConfiguration();
            if (Configurations == null)
                IsConfigurationLoaded = false;
            else
                IsConfigurationLoaded = true;



        }
        static ClientChannelConfigurationManager() { }
        public static ClientChannelConfigurationManager Instance { get; } = new ClientChannelConfigurationManager();
        public bool IsConfigurationLoaded { get; } = false;
        public ClientChannelConfiguration Configurations { get; private set; }
        private ClientChannelConfiguration LoadConfiguration()
        {
            //try
            //{
            //    var folder = ConfigurationManager.AppSettings.Get(ConfigFolder);
            //    if (string.IsNullOrEmpty(folder))
            //    {
            //        folder = string.Format(@"{0}\{1}", AppDirectory.BaseDirectory, DefaultConfigDirectory);
            //    }

            //    var file = string.Format(@"{0}\{1}", folder, FileName);
            //    string jsonString = File.ReadAllText(file);
            //    ClientChannelConfiguration configurations = JsonConvert.DeserializeObject<ClientChannelConfiguration>(jsonString);
            //    configurations.ChannelKey = GetChannelKey();
            //    return configurations;
            //}
            //catch
            //{
            //    return null;
            //}
            var configurtionLoader = new ConfigurtionLoader<ClientChannelConfiguration>(filename, directory);
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
                File.WriteAllText(filePath, JsonConvert.SerializeObject(pair, Formatting.Indented));
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

    }
}
