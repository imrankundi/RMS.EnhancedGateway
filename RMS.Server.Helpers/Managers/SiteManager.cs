using Newtonsoft.Json;
using RMS.Component.DataAccess.SQLite.Repositories;
using RMS.Server.DataTypes;
using System;
using System.Collections.Generic;
using System.IO;

namespace RMS.Parser
{

    public class SiteManager
    {
        public Sites Sites { get; set; }
        public static SiteManager Instance { get; } = new SiteManager();
        private SiteManager()
        {
            //ReadFromFile();
            ReadFromDatabase();
        }
        private string FolderPath()
        {
            return string.Format(@"{0}\{1}", AppDomain.CurrentDomain.BaseDirectory, "Sites");
        }
        private void ReadFromFile()
        {
            string folder = FolderPath();
            if (Directory.Exists(folder))
            {
                string filePath = string.Format(@"{0}\{1}", folder, "Sites.json");

                if (File.Exists(filePath))
                {
                    string text = File.ReadAllText(filePath);
                    Sites = JsonConvert.DeserializeObject<Sites>(text);
                }
            }
        }

        private void ReadFromDatabase()
        {
            try
            {
                var sites = LoadConfiguration();
                Sites = new Sites();
                Sites.SiteList = new Dictionary<string, SiteInfo>();
                if (sites != null)
                {
                    foreach (var site in sites)
                    {
                        Sites.SiteList.Add(site.TerminalId, site);
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        private IEnumerable<SiteInfo> LoadConfiguration()
        {
            var repo = new GatewayConfigRepository();
            var sites = repo.ReadSitesConfig();
            var configuration = Component.Mappers.ConfigurationMapper.Map(sites);
            return configuration;
        }

        public void Reload()
        {
            //ReadFromFile();
            ReadFromDatabase();
        }
    }
}
