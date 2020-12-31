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
    public class EmailConfigRepository
    {
        public static string FileName => "EmailConfig";
        public static string FileExtension => ".sqlite";
        public static string DatabaseFile => string.Format(@"{0}\{1}{2}", DatabaseDirectory, FileName, FileExtension);
        public static string DatabaseDirectory => string.Format(@"{0}\Database", AppDomain.CurrentDomain.BaseDirectory);
        public static string className = nameof(GatewayConfigRepository);
        public ILog Log { get; set; }
        public EmailConfigRepository() : this(null)
        {
        }
        public EmailConfigRepository(ILog log)
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
    }
}
