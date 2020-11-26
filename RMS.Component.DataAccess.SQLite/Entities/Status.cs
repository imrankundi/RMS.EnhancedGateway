using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Component.DataAccess.SQLite.Entities
{
    public enum Status
    {
        Unknown = 0,
        Sending = 1,
        Sent = 2,
        NotSent = 3
    }
}
