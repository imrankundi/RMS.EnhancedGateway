using Microsoft.VisualStudio.TestTools.UnitTesting;
using RMS.Component.DataAccess.SQLite.Repositories;

namespace RMS.Test
{
    [TestClass]
    public class SQLiteTest
    {

        [TestMethod]
        public void LoadServerChannelConfig()
        {
            var repo = new GatewayConfigRepository();
            var config = repo.ReadConfiguration();
        }
    }
}
