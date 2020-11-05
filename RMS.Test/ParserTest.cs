using Microsoft.VisualStudio.TestTools.UnitTesting;
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
             */
            var message = CreateModbusPacket("SP000316", "MENT", new byte[] { 95, 156, 17, 74, 25, 3, 3, 0, 0, 20, 20, 242, 0, 235, 0, 234, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 11, 114, 227, 233 });
            //var message = "SP002411<SGRC(ID(00,30/10/2020,14:23:19)X(28.0,24.2,-65,0)H(14,45,32264261,10478812,32264298,32236195,32216922,32217082)L(99,900.35,43106,21828503,43109,71240,90465,90265)A(00101111))>";
            var packet = ParsingManager.FirstLevelParser(message);
            var protocl = ProtocolList.Instance.Find(packet.ProtocolHeader);
            if (protocl.ProtocolType == Core.Enumerations.ProtocolType.Monitoring)
            {
                var pkt = ParsingManager.SecondLevelParser(packet);
            }
            else
            {

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
