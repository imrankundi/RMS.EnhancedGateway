namespace RMS.Server.DataTypes.Requests
{
    public enum GTCommandType
    {
        Unknown = 0,
        GeneralSettings = 1,
        SimAndServerSettings = 2,
        PollingAndGprsSettings = 3,
        ExtendedConfigurationSettings = 4,
        Reset = 5,
        ResetRom = 6,
        WatchdogSettings = 7,
        AddModbusDevice = 8,
        AddMultipleModbusDevices = 9,
        GetModbusDevice = 10,
        GetMultipleModbusDevices = 11
    }
}
