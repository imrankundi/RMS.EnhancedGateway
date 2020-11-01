using RMS.Core.QueryBuilder;
using System.Text;

namespace RMS.Parser
{
    public class ParsedPacketQueryBuilder
    {
        private const string RealTimeData = nameof(RealTimeData);
        private const string ReceivedOn = nameof(ReceivedOn);

        private ParsedPacket packet;
        public ParsedPacketQueryBuilder(ParsedPacket packet)
        {
            this.packet = packet;
        }

        public string RealTimeQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("IF EXISTS(");
            sb.AppendLine(RealTimeSelectQuery());
            sb.AppendLine(") ");
            sb.Append(RealTimeUpdateQuery());
            sb.AppendLine(" ELSE ");
            sb.AppendLine(RealTimeInsertQuery());

            return sb.ToString();
        }

        public string SiteQuery()
        {
            InsertQuery query = new InsertQuery(packet.TerminalId);
            query.AddField(new Field(nameof(ReceivedOn), packet.ReceivedOn));
            query.AddFieldRange(packet.FilterFields);
            query.AddFieldRange(packet.Fields);
            return query.ToQuery();
        }


        public string RealTimeSelectQuery()
        {
            SelectQuery query = new SelectQuery(RealTimeData);
            query.AddFields(packet.Fields);
            query.AddFilters(packet.FilterFields);
            return query.ToQuery();
        }

        public string RealTimeInsertQuery()
        {
            InsertQuery query = new InsertQuery(RealTimeData);
            query.AddField(new Field(nameof(ReceivedOn), packet.ReceivedOn));
            query.AddFieldRange(packet.FilterFields);
            query.AddFieldRange(packet.Fields);
            return query.ToQuery();
        }


        public string RealTimeUpdateQuery()
        {
            UpdateQuery query = new UpdateQuery(RealTimeData);
            query.AddField(new Field(nameof(ReceivedOn), packet.ReceivedOn));
            query.AddFieldRange(packet.Fields);
            query.AddFilters(packet.FilterFields);
            return query.ToQuery();
        }

    }
}
