using System;

namespace RMS.Parser
{
    public class ReceivedPacket
    {
        public DateTime ReceivedOn { get; set; }
        public string TerminalId { get; set; }
        public string ProtocolHeader { get; set; }
        public string Data { get; set; }

        public override string ToString()
        {
            return string.Format("{0}|{1}|{2}", TerminalId, ProtocolHeader, Data);
        }
    }
}
