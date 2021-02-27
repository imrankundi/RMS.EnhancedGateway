using RMS.Component.Configuration;
using System.Collections.Generic;

namespace RMS.Component.DataAccess
{
    public class DatabaseConfiguration : BaseConfiguration
    {
        public string ConnectionStringName { get; set; }
        public IList<ConnectionStrings> ConnectionStrings { get; set; }

    }
    public class ConnectionStrings
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public bool Encrypted { get; set; }
        public DatabaseProvider DbType { get; set; }

    }
}
