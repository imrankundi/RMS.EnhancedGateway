using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace PKPSAssets.Component.DataAccess
{
    public class OracleConnectionFactory : DatabaseConnectionFactory
    {
        public OracleConnectionFactory(string connectionString)
        : base(connectionString)
        {
        }
        public override IDbConnection CreateConnection()
        {
            //this.ConnectionString = connectionString;
            //var connectionString = @"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.0.134)(PORT=1521)))(CONNECT_DATA=(SID=orcl)));User ID=identify;Password=identify;";
            return new OracleConnection(ConnectionString);
            //connection.ConnectionString = connectionString;
            //connection.Open();
            //return connection;
        }




    }
}
