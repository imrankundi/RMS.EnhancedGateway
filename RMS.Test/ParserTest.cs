﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RMS.Parser;

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
                var pkt = ParsingManager.SecondLevelParser(packet);
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
    }
}
