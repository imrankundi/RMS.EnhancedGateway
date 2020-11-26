using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Component.DataAccess.SQLite.Entities
{
    public class PushApiEntity
    {
        public long Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int ServerId { get; set; }
        public string Data { get; set; }
        public Status Status { get; set; }
    }
}
