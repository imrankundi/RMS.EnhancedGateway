using System.Data;
using System.Data.SqlClient;

namespace RMS.Component.DataAccess
{
    public class SqlConnectionFactory : DatabaseConnectionFactory
    {
        public SqlConnectionFactory(string connectionString)
            : base(connectionString)
        {
        }
        public override IDbConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
