using System;
using System.Configuration;

namespace RMS.Core.Common
{
    public class AppConfigManager
    {
        //public const string TimeOffsetKey = "TimeOffset";
        //public const string ApplicationIdKey = "ApplicationId";
        //public const string DatabaseProviderKey = "DatabaseProvider";
        //public const string SuperAdminUserKey = "SuperAdminUser";
        //public const string SuperAdminPasswordKey = "SuperAdminPassword";

        public static double TimeOffset
        {
            get
            {
                string key = nameof(TimeOffset);
                if (!KeyExist(key))
                    return 0;

                return ConversionHelper.ToDouble(GetAppSettingValue(key));
            }
        }

        public static Guid ApplicationId
        {
            get
            {
                string key = nameof(ApplicationId);
                if (!KeyExist(key))
                    return Guid.Empty;

                return ConversionHelper.ToGuid(GetAppSettingValue(key));
            }
        }

        public static string DatabaseProvider
        {
            get
            {
                string key = nameof(DatabaseProvider);
                if (!KeyExist(key))
                    return string.Empty;

                return GetAppSettingValue(key);
            }
        }

        public static string SuperAdminUser
        {
            get
            {
                string key = nameof(SuperAdminUser);
                if (!KeyExist(key))
                    return string.Empty;

                return GetAppSettingValue(key);
            }
        }

        public static string SuperAdminPassword
        {
            get
            {
                string key = nameof(SuperAdminPassword);
                if (!KeyExist(key))
                    return string.Empty;

                return GetAppSettingValue(key);
            }
        }

        public static string Version
        {
            get
            {
                string key = nameof(Version);
                if (!KeyExist(key))
                    return string.Empty;

                return GetAppSettingValue(key);
            }
        }


        public static bool KeyExist(string key)
        {
            string[] keys = ConfigurationManager.AppSettings.AllKeys;
            foreach (string k in keys)
            {
                if (k.Equals(key))
                    return true;
            }
            return false;
        }

        public static string GetAppSettingValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }

}
