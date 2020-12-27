﻿namespace RMS.Server.WebApi.Configuration
{
    public class WebApiServerConfiguration
    {
        public string Url { get; set; }
        public bool EnableTcpServer { get; set; }
        public bool EnableSimulation { get; set; }
        public int TerminalCommandRetries { get; set; }
        public int TerminalCommandRetryIntervalInSeconds { get; set; }
    }
}
