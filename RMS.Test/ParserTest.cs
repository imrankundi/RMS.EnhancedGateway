using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RMS.Parser;
using RMS.Protocols;
using RMS.Protocols.GT;
using System;

namespace RMS.Test
{
    [TestClass]
    public class ParserTest
    {

        [TestMethod]
        public void ParseModbusPacket()
        {
            // SP003169<MENT(95,151,17,251,25,3,7,24,48,24,0,0,0,20,0,3,0,0,0,0,0,0,0,0,0,20,0,3,0,0,0,0,0,0,178,93)>
            // SP003199<MENT(95,156,6,32,25,3,3,0,0,20,19,10,0,0,1,98,254,153,0,0,0,0,0,0,0,0,11,74,11,4,49,170)>
            /*
            Raw : SP002911<MENT(95,156,17,74,25,3,3,0,0,20,20,242,0,235,0,234,0,0,0,0,0,0,0,0,0,0,0,0,11,114,227,233)> Page3
	        Data: 53.62,23.5,23.4,0,0,0,0,0,0,29.3
            SP900019[MECM(95,192,125,175,1,4,5,0,0,66,0,251,0,16,133,36,0,0,33,135,0,15,252,125,0,0,170,46,0,0,23,152,0,16,143,19,0,11,67,100,0,5,99,71,0,9,129,106,0,7,37,65,0,16,166,157,0,0,0,14,0,16,166,156,0,0,0,15,0,16,166,151,0,0,0,20,0,0]
            SP003230[MENT(95,189,32,28,25,3,12,27,36,80,56,39,87,80,21,24,4,238,10,240,11,184,0,38,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,76,168]
            SP002968[MEMT(95,189,32,28,25,3,3,2,178,90,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,228,61]
             */
            var message = CreateModbusPacket("SP900019", "MECM",
                new byte[] { 95, 192, 125, 175, 1, 4, 5, 0, 0, 66, 0, 251, 0, 16, 133, 36, 0, 0, 33, 135, 0, 15, 252, 125, 0, 0, 170, 46, 0, 0, 23, 152, 0, 16, 143, 19, 0, 11, 67, 100, 0, 5, 99, 71, 0, 9, 129, 106, 0, 7, 37, 65, 0, 16, 166, 157, 0, 0, 0, 14, 0, 16, 166, 156, 0, 0, 0, 15, 0, 16, 166, 151, 0, 0, 0, 20, 0, 0 });
            //var message = "SP002411<SGRC(ID(00,30/10/2020,14:23:19)X(28.0,24.2,-65,0)H(14,45,32264261,10478812,32264298,32236195,32216922,32217082)L(99,900.35,43106,21828503,43109,71240,90465,90265)A(00101111))>";

            var packet = ParsingManager.FirstLevelParser(message);
            var protocl = ProtocolList.Instance.Find(packet.ProtocolHeader);
            if (protocl.ProtocolType == Core.Enumerations.ProtocolType.Monitoring)
            {
                var pkt = Parser.ParsingManager.SecondLevelParser(packet);
                var str = JsonConvert.SerializeObject(pkt, Formatting.Indented);
            }
            else
            {
                var pkt = string.Format("{0}<{1}({2})>\r", packet.TerminalId, packet.ProtocolHeader, packet.Data);
            }

        }
        [TestMethod]
        public void LoadSites()
        {
            var sites = SiteManager.Instance.Sites;
        }

        private string CreateModbusPacket(string siteId, string deviceName, byte[] data)
        {
            string d = System.Text.Encoding.Default.GetString(data);
            return string.Format("{0}<{1}({2})>\r\n", siteId, deviceName, d);
        }
        [TestMethod]
        public void CreateCommand()
        {
            Protocols.CommandBuilder builder = new Protocols.CommandBuilder("CGRC", "SP100100");
            var section = builder.CreateCommandSection("ID");
            section.AddParameterValue("00");
            section.AddParameterValue("N");
            section.AddParameterValue("N");
            section = builder.CreateCommandSection("N");
            section.AddParameterValue("1");
            section.AddParameterValue("2");
            section.AddParameterValue("3");
            section.AddParameterValue("4");

            var str = builder.Build();
        }

        [TestMethod]
        public void SecondLevelParsing()
        {
            //SP900018[MECM(95,211,185,189,1,4,9,0,0,64,0,3,14,169,244,125,230,174,0,1,116,236,190,125,218,223,0,1,255,175,160,43,2,28,0,41,2,43,138,2,39,38,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]
            Parser.ReceivedPacket packet = new ReceivedPacket
            {
                Data = "95,211,185,189,1,4,9,0,0,64,0,3,14,169,244,125,230,174,0,1,116,236,190,125,218,223,0,1,255,175,160,43,2,28,0,41,2,43,138,2,39,38,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0",
                ProtocolHeader = "MECM",
                ReceivedOn = DateTime.UtcNow,
                TerminalId = "SP111111"
            };

            var pkt = ParsingManager.SecondLevelParser(packet);
            var str = JsonConvert.SerializeObject(pkt, Formatting.Indented);
        }
        [TestMethod]
        public void GT02()
        {
            GTPollingAndGprsSettings c = new GTPollingAndGprsSettings("SP333444");
            c.Device6 = "SPMX00DT";
            c.SMSTransmissionInterval = 10;

            var cmd = c.ToString();
        }
        [TestMethod]
        public void FirsLevelParsing()
        {
            //CGRC00 cgrc00 = new CGRC00();
            //CGRC01 cgrc01 = new CGRC01();
            //CGRC02 cgrc02 = new CGRC02();

            string raw = "SP333444<CGRC(ID(00,17/01/2021,07:51:07)N(+923333451191,+923468220229,+923333404763,,,,,,,,SR117R00BL01R00,13-Jan-16,16:27:21,+923,internet,,,+923,internet,,,333444,+923333451191,67.23.248.114,30004,SPMX00DT,SPMX01DT,,,,,,,1,15,2,4,30,9600,15,2)L(1000101101000000000001000000000000))\r>";
            var packet = ParsingManager.FirstLevelParser(raw);

            var list = GTCommandFactory.GetConfiguration(packet);
            //cgrc00.Parse(packet.Data);
            //cgrc01.Parse(packet.Data);
            //cgrc02.Parse(packet.Data);

        }

        [TestMethod]
        public void ParseModbusGetCommand()
        {
            var cmd1 = GTCommandFactory.CreateGetModbusDeviceCommand("SP111111", 1, 1);
            var cmd2 = GTCommandFactory.CreateGetModbusDeviceCommand("SP111111", 1, 5);
            var cmd3 = GTCommandFactory.CreateGetModbusDeviceCommand("SP111111", 10, 5);
        }

        [TestMethod]
        public void ParseModbusGetParsing()
        {
            string raw = "SP003254<CMOD(GET[MNAR,38,4,4095,51,0])>";
            var packet = ParsingManager.FirstLevelParser(raw);
            var str = packet.Data.Replace("GET[", "").TrimEnd(']');
            var strArray = str.Split(',');
            GTGetModbusDevice device = new GTGetModbusDevice(packet.TerminalId);
            device.Parse(strArray);

            device.DeviceName = "MDPC";
            var s = device.ToString();
        }
        [TestMethod]
        public void ParseModbusAddCommand()
        {
            GTAddModbusDeviceCollection col = new GTAddModbusDeviceCollection("SP111111");
            
            
            GTAddModbusDevice device = new GTAddModbusDevice("SP111111");

            device.DeviceName = "MDPC";
            device.DeviceId = 2;
            device.NumberOfElements = 10;
            device.StartingAddress = 100;

            col.Devices.Add(device);
            col.Devices.Add(device);

            GTAddModbusDevice device2 = new GTAddModbusDevice("SP111111");

            device.DeviceName = "MGSE";
            device.DeviceId = 5;
            device.NumberOfElements = 10;
            device.StartingAddress = 200;

            col.Devices.Add(device);
            col.Devices.Add(device);

            var s = device.ToString();

            var c = col.ToString();
        }
    }
}
