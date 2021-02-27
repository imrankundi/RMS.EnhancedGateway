using System.Data;

namespace RMS.Component.DataAccess
{
    interface IDatabaseConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
