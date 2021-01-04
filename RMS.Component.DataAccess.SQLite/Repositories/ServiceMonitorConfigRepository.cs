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
    public class ServiceMonitorConfigRepository
    {
        public static string FileName => "ServiceMonitorConfig";
        public static string FileExtension => ".sqlite";
        public static string DatabaseFile => string.Format(@"{0}\{1}{2}", DatabaseDirectory, FileName, FileExtension);
        public static string DatabaseDirectory => string.Format(@"{0}\Database", AppDomain.CurrentDomain.BaseDirectory);
        public static string className = nameof(EmailConfigRepository);
        public ILog Log { get; set; }
        public ServiceMonitorConfigRepository() : this(null)
        {
        }
        public ServiceMonitorConfigRepository(ILog log)
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
        public ServiceMonitorConfig ReadServiceMonitorConfiguration()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            ServiceMonitorConfig entity = null;
            using (var connection = CreateConnection())
            {
                try
                {
                    connection.Open();
                    var query = "SELECT * FROM ServiceMonitorConfig ORDER BY Id LIMIT 1;";
                    entity = connection.Query<ServiceMonitorConfig>(query).FirstOrDefault();
                    entity.Parameters = BindParameters(entity);
                    //entity.SmtpSettings = BindSmtpConfiguration(entity);
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
        private IEnumerable<MontioringParameterConfig> BindParameters(ServiceMonitorConfig entity)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            IEnumerable<MontioringParameterConfig> config = null;
            using (var connection = CreateConnection())
            {
                try
                {
                    connection.Open();
                    var query = @"SELECT m.* FROM ServiceMonitorConfig s INNER JOIN ServiceMonitorParametersConfig sm ON s.Id = sm.ServiceMonitorId INNER JOIN MonitoringParameterConfig m ON m.Id = sm.MonitoringParameterId WHERE s.Id = @ServiceMonitorConfigId;";
                    var param = new { @ServiceMonitorConfigId = entity.Id };
                    config = connection.Query<MontioringParameterConfig>(query, param);
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
    }
}
