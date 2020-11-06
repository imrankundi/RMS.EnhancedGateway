using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace RMS.Parser
{
    public class ProtocolList
    {
        private const string Drivers = nameof(Drivers);
        private static ProtocolList instance = new ProtocolList();
        private static Dictionary<string, Protocol> protocols;

        public static ProtocolList Instance => instance;
        public Dictionary<string, Protocol> Protocols => protocols;

        private ProtocolList()
        {
            protocols = new Dictionary<string, Protocol>();
            ReadProtocolFromFile();
        }

        private void ReadProtocolFromFile()
        {
            string folder = FolderPath();
            if (Directory.Exists(folder))
            {
                foreach (string file in Directory.EnumerateFiles(folder))
                {
                    try
                    {
                        string allText = File.ReadAllText(file);
                        Protocol protocol = JsonConvert.DeserializeObject<Protocol>(allText);
                        protocols.Add(protocol.ProcotoclHeader, protocol);
                    }
                    catch (Exception ex)
                    {

                    }

                }
            }
        }

        private string FolderPath()
        {
            return string.Format(@"{0}\{1}", AppDomain.CurrentDomain.BaseDirectory, Drivers);
        }

        public Protocol Find(string protocolHeader)
        {
            if (protocols == null)
                return null;

            if (!protocols.ContainsKey(protocolHeader))
                return null;

            return protocols[protocolHeader];

        }
    }
}
