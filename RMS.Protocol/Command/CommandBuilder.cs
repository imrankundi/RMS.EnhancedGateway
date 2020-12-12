using System.Collections.Generic;
using System.Text;

namespace RMS.Protocols
{
    public class CommandBuilder
    {
        public string TerminalId { get; private set; }
        public string ProtocolHeader { get; private set; }
        private Dictionary<string, CommandSection> sections;
        public CommandBuilder(string protocolHeader, string terminalId)
        {
            ProtocolHeader = protocolHeader;
            TerminalId = terminalId;
            sections = new Dictionary<string, CommandSection>();
        }
        public CommandSection CreateCommandSection(string sectionKey)
        {
            CommandSection section = new CommandSection(sectionKey);
            sections.Add(sectionKey, section);
            return section;
        }
        public void AddParameterValue(string sectionKey, string value)
        {
            CommandSection section = sections[sectionKey];
            section.AddParameterValue(value);
        }

        public string Build()
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, CommandSection> kv in sections)
            {
                sb.Append(kv.Value.ToString());
            }
            return string.Format("{0}<{1}{2}>", TerminalId, ProtocolHeader, sb.ToString());
        }
    }
}
