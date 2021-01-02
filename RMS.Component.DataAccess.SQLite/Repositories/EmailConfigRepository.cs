using Dapper;
using RMS.Component.DataAccess.SQLite.Entities;
using RMS.Component.Logging;
using System;
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
        public static string className = nameof(EmailConfigRepository);
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
        public EmailConfig ReadEmailServiceConfiguration()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            EmailConfig entity = null;
            using (var connection = CreateConnection())
            {
                try
                {
                    connection.Open();
                    var query = "SELECT * FROM EmailConfig ORDER BY Id LIMIT 1;";
                    entity = connection.Query<EmailConfig>(query).FirstOrDefault();
                    entity.SmtpSettings = BindSmtpConfiguration(entity);
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
        private SmtpConfig BindSmtpConfiguration(EmailConfig entity)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            SmtpConfig config = null;
            using (var connection = CreateConnection())
            {
                try
                {
                    connection.Open();
                    var query = @"SELECT s.* FROM EmailConfig e INNER JOIN SmtpSettings s ON e.SmtpSettingsId = s.Id WHERE e.Id = @EmailServiceConfigId;";
                    var param = new { @EmailServiceConfigId = entity.Id };
                    config = connection.Query<SmtpConfig>(query, param).FirstOrDefault();
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
