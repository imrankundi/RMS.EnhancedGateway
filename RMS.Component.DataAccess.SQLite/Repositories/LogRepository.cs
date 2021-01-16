using Dapper;
using RMS.Component.DataAccess.SQLite.Entities;
using RMS.Component.Logging;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Reflection;

namespace RMS.Component.DataAccess.SQLite.Repositories
{
    public class LogRepository
    {
        public string DatabaseFile { get; private set; }
        public static string DatabaseDirectory => string.Format(@"{0}\Database", AppDomain.CurrentDomain.BaseDirectory);
        public static string className = nameof(LogRepository);
        public ILog Log { get; set; }

        public LogRepository(string file) : this(null, file)
        {
        }
        public LogRepository(ILog log, string file)
        {
            Log = log;
            DatabaseFile = file;
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

        public IEnumerable<PushApiEntity> ReadPushApiLog(string query)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            IEnumerable<PushApiEntity> result = null;
            using (var connection = CreateConnection())
            {
                try
                {
                    connection.Open();
                    result = connection.Query<PushApiEntity>(query);

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

            return result;
        }
        public IEnumerable<ReceivedPacketEntity> ReadReceivedPacketLog(string query)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            IEnumerable<ReceivedPacketEntity> result = null;
            using (var connection = CreateConnection())
            {
                try
                {
                    connection.Open();
                    result = connection.Query<ReceivedPacketEntity>(query);

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

            return result;
        }

    }
}
