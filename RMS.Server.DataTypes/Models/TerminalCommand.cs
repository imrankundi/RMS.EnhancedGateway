using System;

namespace RMS.Server.DataTypes
{
    public class TerminalCommand
    {
        public string RequestData { get; set; }
        public string ResponseData { get; set; }
        public DateTime RequestReceivedOn { get; set; }
        public DateTime ResponseReceivedOn { get; set; }
        public string TerminalId { get; set; }
        public CommandStatus Status { get; set; }

    }
}
