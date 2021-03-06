﻿using Dapper;
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

        public SmtpConfig ReadSmtpConfiguration()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            SmtpConfig config = null;
            using (var connection = CreateConnection())
            {
                try
                {
                    connection.Open();
                    var query = "SELECT * FROM SmtpSettings;";
                    config = connection.Query<SmtpConfig>(query).FirstOrDefault();
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
                    BindJwtSettings(connection, config);
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
        public IEnumerable<UserEntity> GetUsers()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            IEnumerable<UserEntity> entity = null;
            using (var connection = CreateConnection())
            {
                try
                {
                    connection.Open();
                    var query = "SELECT * FROM Users;";
                    entity = connection.Query<UserEntity>(query);
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

            return entity;
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
        public IEnumerable<EmailSubscriberEntity> GetEmailSubscribersForNoChannelConnected()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            IEnumerable<EmailSubscriberEntity> entity = null;
            using (var connection = CreateConnection())
            {
                try
                {
                    connection.Open();
                    var query = "SELECT s.*, et.Template FROM EmailSubscriptions es INNER JOIN EmailSubscribers s ON es.SubscriberId = s.Id INNER JOIN EmailTemplates et ON es.TemplateId = et.Id WHERE et.TemplateKey = 'NO_CHANNEL_CONNECTED';";
                    entity = connection.Query<EmailSubscriberEntity>(query);
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

            return entity;
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

        private void BindJwtSettings(SQLiteConnection connection, WebApiConfig config)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            try
            {
                var query = "SELECT js.* FROM WebApiServer ws INNER JOIN JwtSettings js ON ws.JwtSettingsId = js.Id;";
                var timeOffsetConfigs = connection.Query<JwtSettingsEntity>(query).FirstOrDefault();
                config.JwtSettings = timeOffsetConfigs;
            }
            catch (Exception ex)
            {
                Log?.Error(className, methodName, ex.ToString());
            }

        }

    }
}
