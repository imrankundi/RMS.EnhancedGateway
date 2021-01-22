using RMS.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Protocols.GT
{
    public class GTCommandFactory
    {
        public static ICollection<ICGRC> GetConfiguration(ReceivedPacket packet)
        {
            List<ICGRC> list = new List<ICGRC>();
            CGRC00 cgrc00 = new CGRC00(packet.TerminalId);
            CGRC01 cgrc01 = new CGRC01(packet.TerminalId);
            CGRC02 cgrc02 = new CGRC02(packet.TerminalId);
            list.Add(cgrc00);
            list.Add(cgrc01);
            list.Add(cgrc02);
            //string raw = "SP333444<CGRC(ID(00,17/01/2021,07:51:07)N(+923333451191,+923468220229,+923333404763,,,,,,,,SR117R00BL01R00,13-Jan-16,16:27:21,+923,internet,,,+923,internet,,,333444,+923333451191,67.23.248.114,30004,SPMX00DT,SPMX01DT,,,,,,,1,15,2,4,30,9600,15,2)L(1000101101000000000001000000000000))\r>";
            string[] strArray = SplitPacket(packet.Data);

            cgrc00.Parse(strArray);
            cgrc01.Parse(strArray);
            cgrc02.Parse(strArray);

            return list;
        }

        private static string[] SplitPacket(string rawPacket)
        {
            if (!string.IsNullOrEmpty(rawPacket))
            {
                string[] strArray = rawPacket.Split(',');
                return strArray;
            }

            return new string[] { };
        }
    }
}
