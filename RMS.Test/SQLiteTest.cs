using Microsoft.VisualStudio.TestTools.UnitTesting;
using RMS.Component.DataAccess.SQLite.Repositories;
using RMS.Server.DataTypes.WindowsService;
using System.Linq;

namespace RMS.Test
{
    [TestClass]
    public class SQLiteTest
    {

        [TestMethod]
        public void LoadServerChannelConfig()
        {
            var repo = new GatewayConfigRepository();
            var config = repo.ReadTcpServerConfiguration();

            var webApi = repo.ReadWebApiConfiguration();
            var sites = repo.ReadSitesConfig();
        }

        [TestMethod]
        public void Map()
        {
            var repo = new GatewayConfigRepository();
            var config = repo.ReadTcpServerConfiguration();
            var res = Component.Mappers.ConfigurationMapper.Map(config);
        }

        [TestMethod]
        public void MapSite()
        {
            var repo = new GatewayConfigRepository();
            var sites = repo.ReadSitesConfig();
            var res = RMS.Component.Mappers.ConfigurationMapper.Map(sites.FirstOrDefault());
        }

        [TestMethod]
        public void MapSites()
        {
            var repo = new GatewayConfigRepository();
            var sites = repo.ReadSitesConfig();
            var res = RMS.Component.Mappers.ConfigurationMapper.Map(sites);
        }
        [TestMethod]
        public void MapWebApiConfig()
        {
            var repo = new GatewayConfigRepository();
            var webApi = repo.ReadWebApiConfiguration();
            var res = RMS.Component.Mappers.ConfigurationMapper.Map(webApi);
        }
        [TestMethod]
        public void UpdateServiceStatus()
        {
            var repo = new ServiceMonitorConfigRepository();
            repo.Update(new Component.DataAccess.SQLite.Entities.MontioringParameterConfig
            {
                Id = 1,
                ServiceState = (int)ServiceStatus.Running
            });
        }

        [TestMethod]
        public void GetNoChannelConnectedEmail()
        {
            var repo = new EmailConfigRepository();
            var res = repo.GetEmailTemplate("NO_CHANNEL_CONNECTED");
        }

        [TestMethod]
        public void GetServiceStatusEmail()
        {
            var repo = new EmailConfigRepository();
            var res = repo.GetEmailTemplate("SERVICE_STATUS");
        }
    }
}
