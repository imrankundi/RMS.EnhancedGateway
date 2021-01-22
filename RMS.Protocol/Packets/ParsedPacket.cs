using RMS.Core.Enumerations;
using RMS.Core.QueryBuilder;
using System;
using System.Collections.Generic;

namespace RMS.Parser
{
    public class ParsedPacket
    {
        public string ProtocolHeader { get; set; }
        public ProtocolType ProtocolType { get; set; }
        public string TerminalId { get; set; }
        public string Id { get; set; }
        public int PageNumber { get; set; }
        public DateTime ReceivedOn { get; set; }
        public List<Field> Fields { get; private set; }

        public ParsedPacket()
        {
            Fields = new List<Field>();
        }

        public List<Field> FilterFields
        {
            get
            {
                return new List<Field>()
                {
                    new Field(nameof(ProtocolHeader), ProtocolHeader),
                    new Field(nameof(Id), Id),
                    new Field(nameof(PageNumber), PageNumber),
                    new Field(nameof(TerminalId), TerminalId)
                };
            }
        }

    }
}
