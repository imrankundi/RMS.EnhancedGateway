using Newtonsoft.Json;
using RMS.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceivedPacketToPushPacketTransformer
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadSitesCsv();
            //ReadReceivedPackets(@"ReceivedPacket\received-packets-2021030516.log");
            var files = Directory.GetFiles(@"ReceivedPacket", "*.log");
            foreach(var file in files)
            {
                ReadReceivedPackets(file);
            }
            Console.WriteLine("Completed....");
            Console.ReadKey();
        }

        static Dictionary<string, SiteInfo> dict = new Dictionary<string, SiteInfo>();

        public static void ReadSitesCsv()
        {
            dict.Clear();
            var lines = File.ReadAllLines(@"Sites\Sites.csv");
            foreach (var line in lines)
            {
                var str = line.Split(',');

                var siteId = str[1];
                var siteName = str[2];

                dict.Add(siteId, new SiteInfo
                {
                    Id = siteId,
                    Name = siteName
                });
                Console.WriteLine("Id: {0}, Name: {1}", siteId, siteName);
                
            }
        }

        public static void ReadReceivedPackets(string path)
        {
            Console.WriteLine("Processing: [{0}]", path);
            var fileInfo = new FileInfo(path);
            var text = File.ReadAllText(path);
            text = "[" + text.TrimEnd(',') + "]";
            IEnumerable<ReceivedPacket> json = JsonConvert.DeserializeObject<IEnumerable<ReceivedPacket>>(text);
            var list = FilteredList(json);
            Console.WriteLine("Count: {0}", list.Count());
            var txt = JsonConvert.SerializeObject(list);
            File.WriteAllText(string.Format(@"PushPacket\{0}", fileInfo.Name), txt);
            
            //File.WriteAllText(@"PushPacket\failed-parsed-received-packet.log", JsonConvert.SerializeObject(faildedParsedPackets));
        }
        static List<ReceivedPacket> faildedParsedPackets = new List<ReceivedPacket>();
        public static List<ReonParsedPacket> FilteredList(IEnumerable<ReceivedPacket> json)
        {
            List<ReonParsedPacket> list = new List<ReonParsedPacket>();
            foreach (var packet in json)
            {
                if (dict.ContainsKey(packet.TerminalId))
                {
                    try
                    {
                        var parsedPacket = SecondLevelParsing(packet);
                        
                        if (parsedPacket != null)
                        {
                            var timestamp = ((DateTime)parsedPacket.Data[0]["Timestamp"]).AddHours(5);
                            parsedPacket.Data[0]["Timestamp"] = timestamp;
                            parsedPacket.Mapping = dict[packet.TerminalId].Name;
                            list.Add(parsedPacket);
                        }
                        else
                        {
                            faildedParsedPackets.Add(packet);
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                    
                }
            }

            return list;
        }


        public static ReonParsedPacket SecondLevelParsing(ReceivedPacket packet)
        {

            return ParsingManager.SecondLevelParser(packet);
        }
    }
}
