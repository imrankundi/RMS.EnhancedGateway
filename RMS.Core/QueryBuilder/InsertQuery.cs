using System.Collections.Generic;
using System.Text;

namespace RMS.Core.QueryBuilder
{
    public class InsertQuery : IQuery
    {
        private string table;
        private List<Field> fields;

        public InsertQuery(string table)
        {
            this.table = table;
            this.fields = new List<Field>();
        }



        public void RemoveField(Field field)
        {
            if (field != null && fields.Contains(field))
                fields.Remove(field);
        }

        public string ToQuery()
        {
            if (fields.Count == 0)
                return string.Empty;


            StringBuilder query = new StringBuilder();
            string[] columns = new string[fields.Count];
            string[] values = new string[fields.Count];


            for (int ii = 0; ii < fields.Count; ii++)
            {
                Field field = fields[ii];
                columns[ii] = field.Name;
                values[ii] = string.Format("'{0}'", field.Value);
            }

            query.Append("INSERT INTO ")
                    .Append(this.table)
                    .Append("(")
                    .Append(string.Join(",", columns))
                    .Append(") ")
                    .Append("VALUES(")
                    .Append(string.Join(",", values))
                    .Append(");");

            return query.ToString();
        }

        public void AddField(Field field)
        {
            if (field != null)
                fields.Add(field);
        }

        public void AddFields(List<Field> fields)
        {
            if (fields == null && fields.Count == 0)
                return;

            this.fields = fields;
        }

        public void AddFieldRange(List<Field> fields)
        {
            if (fields == null && fields.Count == 0)
                return;

            this.fields.AddRange(fields);
        }
    }
}
