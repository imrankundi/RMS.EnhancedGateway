using Newtonsoft.Json;
using RMS.Component.Common;
using System.IO;

namespace RMS.Component.Configuration
{
    public class ConfigurtionLoader<T> where T : BaseConfiguration
    {
        private const string defaultDirectory = @"Configuration";
        private string fileName = string.Empty;
        private string directory = string.Empty;
        public ConfigurtionLoader(string fileName) : this(fileName, string.Empty) { }
        public ConfigurtionLoader(string fileName, string directory)
        {
            this.fileName = fileName;
            this.directory = directory;
        }
        public T LoadConfiguration()
        {
            try
            {
                if (string.IsNullOrEmpty(directory))
                {
                    directory = string.Format(@"{0}\{1}", AppDirectory.BaseDirectory, defaultDirectory);
                }

                var file = string.Format(@"{0}\{1}.json", directory, fileName);
                string jsonString = File.ReadAllText(file);
                T configurations = JsonConvert.DeserializeObject<T>(jsonString);
                configurations.Directory = directory;
                configurations.FileName = fileName;
                return configurations;
            }
            catch
            {
                return null;
            }
        }

    }
}
