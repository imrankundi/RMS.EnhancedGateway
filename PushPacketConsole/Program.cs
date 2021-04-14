using Newtonsoft.Json;
using RMS.AWS;
using RMS.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushPacketConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var files = Directory.GetFiles(@"PushPacket", "*.log");
            foreach (var file in files)
            {
                ReadPackets(file);
            }
            Console.WriteLine("Completed....");
            Console.ReadKey();
        }

        private static void ReadPackets(string path)
        {
            try
            {
                IEnumerable<ReonParsedPacket> packets = JsonConvert.DeserializeObject<IEnumerable<ReonParsedPacket>>(File.ReadAllText(path));
                Console.WriteLine("Reading File: [{0}]", path);
                File.Move(path, string.Format("{0}.working", path));
                PushToServer(packets);
            }
            catch (Exception)
            {

            }
        }
        private static void PushToServer(IEnumerable<ReonParsedPacket> packets)
        {
            try
            {
                ServerInfo listener = JsonConvert.DeserializeObject<ServerInfo>(File.ReadAllText(@"Config\aws.json"));
                int total = packets.Count();
                Console.WriteLine("Total: {0}", total);
                var client = new AwsSqsClient(listener);

                int count = 0;
                foreach (var packet in packets)
                {
                    count++;
                    try
                    {
                        var request = JsonConvert.SerializeObject(packet);
                        
                        var res = client.PostData(request);
                        Console.WriteLine(string.Format("{0} - Count: {1} - Remaining: {2}", res.Result, count, (total - count)));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                
            }

        }
    }
}
