using Dapper;
using RMS.Component.DataAccess.SQLite.Entities;
using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;

namespace RMS.Component.DataAccess.SQLite
{
    public class ReceivedPacketRepository
    {
        public static string DatabaseFile => string.Format(@"{0}\{1}.sqlite", DatabaseDirectory, DateTime.Now.ToString("HH"));
        public static string DatabaseDirectory => string.Format(@"{0}\Logs\ReceivedPackets\{1}", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("yyyyMMdd"));

        public static SQLiteConnection CreateConnection()
        {
            return new SQLiteConnection("Data Source=" + DatabaseFile);
        }

        private static bool CreateDatabase()
        {
            bool result = false;
            try
            {
                using (var connection = CreateConnection())
                {
                    connection.Open();
                    connection.Execute(CreateReceivedPacketTable());
                }
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        private static readonly object locker = new object();
        public static bool Save(ReceivedPacketEntity entity)
        {
            bool result;
            try
            {
                lock (locker)
                {
                    if (!File.Exists(DatabaseFile))
                    {
                        if (!Directory.Exists(DatabaseDirectory))
                        {
                            Directory.CreateDirectory(DatabaseDirectory);
                        }
                        CreateDatabase();
                    }
                    using (var connection = CreateConnection())
                    {
                        connection.Open();
                        var query = @"INSERT INTO ReceivedPackets(ReceivedOn, TerminalId, ProtocolHeader, Data, Status) VALUES(@ReceivedOn, @TerminalId, @ProtocolHeader, @Data, @Status); SELECT last_insert_rowid();";
                        entity.Id = connection.Query<long>(query, entity).First();
                    }
                }

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        private static string CreateReceivedPacketTable()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("CREATE TABLE ReceivedPackets");
            sb.AppendLine("(");
            sb.AppendLine("Id INTEGER PRIMARY KEY AUTOINCREMENT,");
            sb.AppendLine("ReceivedOn TEXT,");
            sb.AppendLine("TerminalId TEXT,");
            sb.AppendLine("ProtocolHeader TEXT,");
            sb.AppendLine("Data TEXT,");
            sb.AppendLine("Status INTEGER");
            sb.AppendLine(");");
            return sb.ToString();
        }
    }
}
