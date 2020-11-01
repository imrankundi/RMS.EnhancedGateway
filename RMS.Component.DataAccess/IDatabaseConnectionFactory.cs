using System.Data;

namespace PKPSAssets.Component.DataAccess
{
    interface IDatabaseConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
