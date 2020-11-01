using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace RMS.Gateway
{
    public class GatewaySettings
    {
        private static GatewaySettings settings = new GatewaySettings();

        public static GatewaySettings Instance => settings;

        public string GatewayIP { get; private set; }
        public int GatewayPort { get; set; }
        //public int ListenerPort { get; set; }
        public bool EnableErrorLog { get; set; }
        public int PingIntervalInSeconds { get; set; }
        public int KickIntervalInSeconds { get; set; }
        public int KickWaitInSeconds { get; set; }
        public int SyncIntervalInSeconds { get; set; }
        public int RestartWaitInSeconds { get; set; }

        private GatewaySettings()
        {
            GatewayIP = GatewayConfigManager.GatewayIP;
            GatewayPort = GatewayConfigManager.GatewayPort;
            //ListenerPort = GatewayConfigManager.ListenerPort;
            EnableErrorLog = GatewayConfigManager.EnableErrorLog;
            PingIntervalInSeconds = GatewayConfigManager.PingIntervalInSeconds;
            KickIntervalInSeconds = GatewayConfigManager.KickIntervalInSeconds;
            KickWaitInSeconds = GatewayConfigManager.KickWaitInSeconds;
            SyncIntervalInSeconds = GatewayConfigManager.SyncIntervalInSeconds;
            RestartWaitInSeconds = GatewayConfigManager.RestartWaitInSeconds;
        }
    }
}
