﻿using System.Collections.Generic;
using System.Text;

namespace RMS.Core.QueryBuilder
{
    public class UpdateQuery : IQuery
    {
        private string table;
        private List<Field> fields;
        private List<Field> filters;

        public UpdateQuery(string table)
        {
            this.table = table;
            this.fields = new List<Field>();
        }

        public void RemoveField(Field field)
        {
            if (field != null && fields.Contains(field))
                fields.Remove(field);
        }

        public void AddFilter(Field field)
        {
            if (field != null)
                filters.Add(field);
        }

        public void RemoveFilter(Field field)
        {
            if (field != null && filters.Contains(field))
                filters.Remove(field);
        }

        public string ToQuery()
        {
            if (fields.Count == 0)
                return string.Empty;


            StringBuilder query = new StringBuilder();

            query.Append("UPDATE ")
                    .Append(this.table)
                    .Append(" SET ");

            for (int ii = 0; ii < fields.Count; ii++)
            {
                Field field = fields[ii];
                query.Append(field.Name)
                        .Append(" = ")
                        .AppendFormat("'{0}'", field.Value);

                if (ii < fields.Count - 1)
                {
                    query.Append(",");
                }
            }

            for (int ii = 0; ii < filters.Count; ii++)
            {
                Field filter = filters[ii];
                if (ii < filters.Count)
                {
                    if (ii == 0)
                    {
                        query.Append(" WHERE ");
                    }
                    else
                    {
                        query.Append(" AND ");
                    }
                }

                query.Append(filter.Name)
                        .Append(" = ")
                        .AppendFormat("'{0}'", filter.Value);

            }

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

        public void AddFilters(List<Field> filters)
        {
            this.filters = filters;
        }
    }
}
