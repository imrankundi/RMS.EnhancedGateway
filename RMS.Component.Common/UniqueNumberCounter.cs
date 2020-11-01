using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;

namespace RMS.Component.Common
{
    public sealed class UniqueNumberCounter
    {
        class UncModel
        {
            public int Value { get; set; }
            public ulong UniqueNumber { get; set; }
            public string DateString { get; set; }
        }
        private const string dateFormat = "yyMMdd";
        private const string datetimeFormat = "yyMMddHHmmss";
        private const int minValue = 100000;
        private const int maxValue = 999999;

        private static int value = 100000;
        private static string dateString = DateTime.Now.ToString(dateFormat);
        private static object syncLock = new object();

        public string Name { get; private set; }
        public string FileName { get; private set; }
        public string FileDirectory { get; private set; }
        public UniqueNumberCounter() : this("Default")
        {

        }
        public UniqueNumberCounter(string name)
        {
            this.Name = name;
            this.FileDirectory = string.Format(@"{0}\{1}", AppDirectory.BaseDirectory, "UNG");
            this.FileName = string.Format(@"{0}\{1}.json", FileDirectory, Name);
            LoadFromFile();
        }
        public ulong NextNumber()
        {
            lock (syncLock)
            {
                value++;
                if (value > maxValue || value < minValue)
                    value = minValue;

                DateTime currentDateTime = DateTime.Now;
                var currentDateString = currentDateTime.ToString(dateFormat);
                if (!dateString.Equals(currentDateString))
                {
                    value = minValue;
                }

                var uniqueNumberString = currentDateTime.ToString(datetimeFormat) + value.ToString();
                ulong uniqueNumber = Convert.ToUInt64(uniqueNumberString);
                WriteToFile(value, uniqueNumber, currentDateString);
                return uniqueNumber;
            }
        }

        private static ReaderWriterLockSlim readWriteLock = new ReaderWriterLockSlim();
        private void WriteToFile(int value, ulong uniqueNumber, string dateString)
        {
            UncModel model = new UncModel();
            model.Value = value;
            model.UniqueNumber = uniqueNumber;
            model.DateString = dateString;
            string json = JsonConvert.SerializeObject(model);
            if (!Directory.Exists(FileDirectory))
            {
                Directory.CreateDirectory(FileDirectory);
            }
            try
            {
                readWriteLock.EnterWriteLock();
                File.WriteAllText(FileName, json);
            }
            finally
            {
                readWriteLock.ExitWriteLock();
            }

        }
        private void LoadFromFile()
        {
            if (File.Exists(FileName))
            {
                string json = File.ReadAllText(FileName);
                if (!string.IsNullOrEmpty(json))
                {
                    var obj = JsonConvert.DeserializeObject<UncModel>(json);
                    value = obj.Value;
                    dateString = string.IsNullOrEmpty(obj.DateString) ? DateTime.Now.ToString(dateFormat) : obj.DateString;
                }
            }
        }
    }
}
