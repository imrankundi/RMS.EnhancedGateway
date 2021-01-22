using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Protocols.GT
{
    public interface ICGRC
    {
        string Code { get; }
        string TerminalId { get; }
        void Parse(string[] strArray);
    }
}
