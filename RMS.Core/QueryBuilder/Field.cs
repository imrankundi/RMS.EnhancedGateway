using System;

namespace RMS.Core.QueryBuilder
{
    public class Field : ICloneable
    {
        public string Name => name;
        public object Value => value;
        public bool PrimaryKey => primaryKey;

        private string name;
        private object value;
        private bool primaryKey;

        public Field()
        {

        }
        public Field(string name, object value, bool primaryKey = false)
        {
            this.name = name;
            this.value = value;
            this.primaryKey = primaryKey;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
