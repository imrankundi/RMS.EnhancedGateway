using System;
using System.Data;

namespace PKPSAssets.Component.DataAccess
{
    public class DatabaseFactory
    {
        public static IDbConnection CreateConnection(string connectionString, DatabaseProvider databaseProvider = DatabaseProvider.MSSQL)
        {
            switch (databaseProvider)
            {
                case DatabaseProvider.MSSQL:
                    return new SqlConnectionFactory(connectionString).CreateConnection();
                case DatabaseProvider.PGSQL:
                    throw new NotImplementedException();
                case DatabaseProvider.MySql:
                    throw new NotImplementedException();
                case DatabaseProvider.Oracle:
                    return new OracleConnectionFactory(connectionString).CreateConnection();
                default:
                    return new SqlConnectionFactory(connectionString).CreateConnection();
            }
        }

    }
}
