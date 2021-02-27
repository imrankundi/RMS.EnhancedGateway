using RMS.Component.Configuration;
using System.Collections.Generic;

namespace RMS.Component.DataAccess
{
    public class DatabaseConfigurationManager
    {
        private const string fileName = "DbConfig";
        private const string directory = "Configuration";
        private Dictionary<string, string> connectionStringDictionary;
        private DatabaseConfigurationManager()
        {
            Configurations = LoadConfiguration();
            if (Configurations == null)
                IsConfigurationLoaded = false;
            else
                IsConfigurationLoaded = true;

            PopulateConnectionStringDictionary();
        }
        static DatabaseConfigurationManager() { }
        public static DatabaseConfigurationManager Instance { get; } = new DatabaseConfigurationManager();
        public bool IsConfigurationLoaded { get; } = false;
        public DatabaseConfiguration Configurations { get; private set; }
        private DatabaseConfiguration LoadConfiguration()
        {
            var configurtionLoader = new ConfigurtionLoader<DatabaseConfiguration>(fileName, directory);
            var configuration = configurtionLoader.LoadConfiguration();
            return configuration;
        }

        private void PopulateConnectionStringDictionary()
        {
            connectionStringDictionary = new Dictionary<string, string>();
            if (Configurations == null)
                return;

            var connectionStrings = Configurations.ConnectionStrings;
            foreach (var connectionString in connectionStrings)
            {
                if (connectionString.Encrypted)
                {
                    connectionStringDictionary.Add(connectionString.Name, DatabaseEncryptor.Decrypt(connectionString.ConnectionString));
                }
                else
                {
                    connectionStringDictionary.Add(connectionString.Name, connectionString.ConnectionString);
                }
            }
        }

        public string GetConnectionString(string name)
        {
            if (!connectionStringDictionary.ContainsKey(name))
                return string.Empty;

            return connectionStringDictionary[name];
        }

    }
}
