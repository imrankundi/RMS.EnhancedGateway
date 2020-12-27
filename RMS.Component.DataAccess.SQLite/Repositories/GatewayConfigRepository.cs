using Dapper;
using RMS.Component.DataAccess.SQLite.Entities;
using RMS.Component.Logging;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;

namespace RMS.Component.DataAccess.SQLite.Repositories
{
    public class GatewayConfigRepository
    {
        public static string FileName => "GatewayConfig";
        public static string FileExtension => ".sqlite";
        public static string DatabaseFile => string.Format(@"{0}\{1}{2}", DatabaseDirectory, FileName, FileExtension);
        public static string DatabaseDirectory => string.Format(@"{0}\Database", AppDomain.CurrentDomain.BaseDirectory);
        public static string className = nameof(GatewayConfigRepository);
        public ILog Log { get; set; }

        public GatewayConfigRepository() : this(null)
        {
        }
        public GatewayConfigRepository(ILog log)
        {
            Log = log;
        }
        public SQLiteConnection CreateConnection()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            SQLiteConnection connection = null;
            try
            {
                connection = new SQLiteConnection("Data Source=" + DatabaseFile);
            }
            catch (Exception ex)
            {
                Log?.Error(className, methodName, ex.ToString());
            }
            return connection;
        }

        public TcpServerChannelConfig ReadTcpServerConfiguration()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            TcpServerChannelConfig config = null;
            using (var connection = CreateConnection())
            {
                try
                {
                    connection.Open();
                    var query = "SELECT * FROM TcpServerChannels;";
                    config = connection.Query<TcpServerChannelConfig>(query).FirstOrDefault();
                    if (config != null)
                    {
                        BindServerListeners(connection, config);
                    }
                }
                catch (Exception ex)
                {
                    Log?.Error(className, methodName, ex.ToString());
                }
                finally
                {
                    connection.Close();
                }

            }

            return config;
        }

        private void BindServerListeners(SQLiteConnection connection, TcpServerChannelConfig config)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            try
            {
                var query = "SELECT l.* FROM TcpServerChannelListenerApis scl INNER JOIN ListenerApis l ON scl.ListenerApiId = l.Id;";
                var listeners = connection.Query<ListenerApiConfig>(query);
                config.Listeners = listeners;
            }
            catch (Exception ex)
            {
                Log?.Error(className, methodName, ex.ToString());
            }

        }

        public WebApiConfig ReadWebApiConfiguration()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            WebApiConfig config = null;
            using (var connection = CreateConnection())
            {
                try
                {
                    connection.Open();
                    var query = "SELECT * FROM WebApiServer;";
                    config = connection.Query<WebApiConfig>(query).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    Log?.Error(className, methodName, ex.ToString());
                }
                finally
                {
                    connection.Close();
                }

            }

            return config;
        }

        public IEnumerable<SiteConfig> ReadSitesConfig()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            IEnumerable<SiteConfig> config = null;
            using (var connection = CreateConnection())
            {
                try
                {
                    connection.Open();
                    var query = "SELECT * FROM Sites;";
                    config = connection.Query<SiteConfig>(query);
                    if (config != null)
                    {
                        foreach (var c in config)
                        {
                            BindTimeOffset(connection, c);
                        }

                    }
                }
                catch (Exception ex)
                {
                    Log?.Error(className, methodName, ex.ToString());
                }
                finally
                {
                    connection.Close();
                }

            }

            return config;
        }

        private void BindTimeOffset(SQLiteConnection connection, SiteConfig config)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            try
            {
                var query = "SELECT t.* FROM Sites s INNER JOIN TimeOffsets t ON s.TimeOffsetId = t.Id;";
                var timeOffsetConfigs = connection.Query<TimeOffsetConfig>(query).FirstOrDefault();
                config.TimeOffset = timeOffsetConfigs;
            }
            catch (Exception ex)
            {
                Log?.Error(className, methodName, ex.ToString());
            }

        }

    }
}
