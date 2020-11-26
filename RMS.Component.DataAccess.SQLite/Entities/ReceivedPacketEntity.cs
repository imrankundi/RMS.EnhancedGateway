using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Component.DataAccess.SQLite.Entities
{
    public class ReceivedPacketEntity
    {
        public long Id { get; set; }
        public DateTime ReceivedOn { get; set; }
        public string TerminalId { get; set; }
        public string ProtocolHeader { get; set; }
        public string Data { get; set; }
        public Status Status { get; set; }
    }
}
