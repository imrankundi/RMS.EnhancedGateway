using System;
using System.Configuration;
using System.Data;

namespace RMS.Component.DataAccess
{
    public abstract class DatabaseConnectionFactory : IDatabaseConnectionFactory
    {
        public string ConnectionString { get; private set; }

        public DatabaseConnectionFactory(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public abstract IDbConnection CreateConnection();

        [Obsolete("This method is no longer in use")]
        public static string GetConnectionString(string connectionStringName)
        {
            return ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        }
    }
}
