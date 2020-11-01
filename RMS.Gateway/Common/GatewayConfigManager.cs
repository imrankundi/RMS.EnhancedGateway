using RMS.Core.Common;

namespace RMS.Gateway
{
    public class GatewayConfigManager : AppConfigManager
    {
        public static string GatewayIP
        {
            get
            {
                string key = nameof(GatewayIP);
                if (!KeyExist(key))
                    return string.Empty;

                return GetAppSettingValue(key);
            }
        }

        public static int GatewayPort
        {
            get
            {
                string key = nameof(GatewayPort);
                if (!KeyExist(key))
                    return 0;

                return ConversionHelper.ToInteger(GetAppSettingValue(key));
            }
        }

        public static int ListenerPort
        {
            get
            {
                string key = nameof(ListenerPort);
                if (!KeyExist(key))
                    return 0;

                return ConversionHelper.ToInteger(GetAppSettingValue(key));
            }
        }

        public static bool EnableErrorLog
        {
            get
            {
                string key = nameof(EnableErrorLog);
                if (!KeyExist(key))
                    return false;

                return ConversionHelper.ToBoolean(GetAppSettingValue(key));
            }
        }

        public static int PingIntervalInSeconds
        {
            get
            {
                string key = nameof(PingIntervalInSeconds);
                if (!KeyExist(key))
                    return 0;

                return ConversionHelper.ToInteger(GetAppSettingValue(key));
            }
        }

        public static int SyncIntervalInSeconds
        {
            get
            {
                string key = nameof(SyncIntervalInSeconds);
                if (!KeyExist(key))
                    return 0;

                return ConversionHelper.ToInteger(GetAppSettingValue(key));
            }
        }

        public static int RestartWaitInSeconds
        {
            get
            {
                string key = nameof(RestartWaitInSeconds);
                if (!KeyExist(key))
                    return 0;

                return ConversionHelper.ToInteger(GetAppSettingValue(key));
            }
        }

        public static int KickIntervalInSeconds
        {
            get
            {
                string key = nameof(KickIntervalInSeconds);
                if (!KeyExist(key))
                    return 0;

                return ConversionHelper.ToInteger(GetAppSettingValue(key));
            }
        }

        public static int KickWaitInSeconds
        {
            get
            {
                string key = nameof(KickWaitInSeconds);
                if (!KeyExist(key))
                    return 0;

                return ConversionHelper.ToInteger(GetAppSettingValue(key));
            }
        }
    }
}
