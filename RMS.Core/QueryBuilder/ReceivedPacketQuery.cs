using RMS.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace RMS.Core.QueryBuilder
{
    public class ReceivedPacketQuery
    {
        private const string RealTimeData = nameof(RealTimeData);
        public string ProtocolHeader { get; set; }
        public ProtocolType ProtocolType { get; set; }
        public string TerminalId { get; set; }
        public string Id { get; set; }
        public int PageNumber { get; set; }
        public DateTime ReceivedOn { get; set; }
        public List<Field> Fields { get; private set; }

        public Dictionary<string, Field> FieldDictionary { get; private set; }

        public ReceivedPacketQuery()
        {
            Fields = new List<Field>();
        }

        public string RealTimeQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("IF EXISTS(");
            sb.AppendLine(RealTimeSelectQuery());
            sb.AppendLine(") ");
            sb.Append(RealTimeUpdateQuery());
            sb.AppendLine("ELSE ");
            sb.AppendLine(RealTimeInsertQuery());

            return sb.ToString();
        }

        public string SiteQuery()
        {
            InsertQuery query = new InsertQuery(TerminalId);
            query.AddField(new Field(nameof(ReceivedOn), ReceivedOn));
            query.AddFieldRange(MendatoryFields());
            query.AddFieldRange(Fields);
            return query.ToQuery();
        }


        public string RealTimeSelectQuery()
        {
            SelectQuery query = new SelectQuery(RealTimeData);
            query.AddFields(Fields);
            query.AddFilters(MendatoryFields());
            return query.ToQuery();
        }

        public string RealTimeInsertQuery()
        {
            InsertQuery query = new InsertQuery(RealTimeData);
            query.AddField(new Field(nameof(ReceivedOn), ReceivedOn));
            query.AddFieldRange(MendatoryFields());
            query.AddFieldRange(Fields);
            return query.ToQuery();
        }

        private List<Field> MendatoryFields()
        {
            List<Field> fields = new List<Field>()
            {
                new Field(nameof(ProtocolHeader), ProtocolHeader),
                new Field(nameof(Id), Id),
                new Field(nameof(PageNumber), PageNumber),
                new Field(nameof(TerminalId), TerminalId)
            };

            return fields;
        }

        public string RealTimeUpdateQuery()
        {
            UpdateQuery query = new UpdateQuery(RealTimeData);
            query.AddField(new Field(nameof(ReceivedOn), ReceivedOn));
            query.AddFieldRange(Fields);
            query.AddFilters(MendatoryFields());
            return query.ToQuery();
        }

        public void AddField(Field field)
        {
            if (field == null)
                return;

            Fields.Add(field);
            FieldDictionary.Add(field.Name, field);
        }
    }
}
