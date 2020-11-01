using System.Configuration;
using System.Linq;

namespace RMS.Component.Common
{
    public class AppConfigReader
    {
        public static string Read(string key)
        {
            if (KeyExist(key))
            {
                return ConfigurationManager.AppSettings[key];
            }

            return string.Empty;
        }

        public static bool KeyExist(string key)
        {
            return ConfigurationManager.AppSettings.AllKeys.Contains(key);
        }
    }
}
