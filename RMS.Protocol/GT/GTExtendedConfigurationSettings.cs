using Newtonsoft.Json;
using RMS.Server.DataTypes.Requests;
using System;
using System.Text;

namespace RMS.Protocols.GT
{
    public class GTExtendedConfigurationSettings : ICGRC
    {
        [JsonProperty("terminalId")]
        public string TerminalId { get; set; }
        [JsonProperty("code")]
        public string Code => "03";
        [JsonProperty("commandType")]
        public GTCommandType CommandType { get; set; }
        [JsonProperty("commandTypeDescription")]
        public string CommandTypeDescription => CommandType.ToString();
        [JsonProperty("storageSnapshotInterval")]
        public int StorageSnapshotInterval { get; set; }
        [JsonProperty("storageRmsSendInterval")]
        public int StorageRmsSendInterval { get; set; }
        [JsonIgnore]
        public string AuxLatchStatus
        {
            get
            {
                return string.Format("{0}{1}{2}{3}{4}{5}", (int)Aux1LatchState, (int)Aux2LatchState,
                    (int)Aux3LatchState, (int)Aux4LatchState, (int)Aux5LatchState, (int)Aux6LatchState);
            }
        }
        [JsonProperty("aux1LatchState")]
        public GTAuxLatchState Aux1LatchState { get; set; }
        [JsonProperty("aux2LatchState")]
        public GTAuxLatchState Aux2LatchState { get; set; }
        [JsonProperty("aux3LatchState")]
        public GTAuxLatchState Aux3LatchState { get; set; }
        [JsonProperty("aux4LatchState")]
        public GTAuxLatchState Aux4LatchState { get; set; }
        [JsonProperty("aux5LatchState")]
        public GTAuxLatchState Aux5LatchState { get; set; }
        [JsonProperty("aux6LatchState")]
        public GTAuxLatchState Aux6LatchState { get; set; }
        [JsonProperty("auxLatchTime")]
        public int AuxLatchTime { get; set; }
        [JsonIgnore]
        public string Reserved1 { get; set; }
        [JsonIgnore]
        public string Reserved2 { get; set; }
        [JsonIgnore]
        public string Reserved3 { get; set; }
        [JsonIgnore]
        public string Reserved4 { get; set; }
        [JsonProperty("clearStorage")]
        public bool ClearStorage { get; set; }
        [JsonProperty("storageFoundStatus")]
        public bool StorageFoundStatus { get; private set; }
        [JsonProperty("storageBusyStatus")]
        public bool StorageBusyStatus { get; private set; }
        [JsonIgnore]
        public bool ReservedFlag1 { get; set; }
        [JsonIgnore]
        public bool ReservedFlag2 { get; set; }
        [JsonIgnore]
        public bool ReservedFlag3 { get; set; }
        [JsonIgnore]
        public bool ReservedFlag4 { get; set; }
        [JsonIgnore]
        public bool ReservedFlag5 { get; set; }

        public GTExtendedConfigurationSettings(string terminalId)
        {
            TerminalId = terminalId;
            CommandType = GTCommandType.ExtendedConfigurationSettings;
        }
        public string CreateCommand()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetBooleanAsString(ClearStorage));
            sb.Append(GetBooleanAsString(StorageFoundStatus));
            sb.Append(GetBooleanAsString(StorageBusyStatus));
            sb.Append(GetBooleanAsString(ReservedFlag1));
            sb.Append(GetBooleanAsString(ReservedFlag2));
            sb.Append(GetBooleanAsString(ReservedFlag3));
            sb.Append(GetBooleanAsString(ReservedFlag4));
            sb.Append(GetBooleanAsString(ReservedFlag5));

            return string.Format("CGRC(ID({0},N,N)N({1},{2},{3},{4},{5},{6},{7},{8})L({9})",
                Code, StorageSnapshotInterval, StorageRmsSendInterval, AuxLatchStatus,
                AuxLatchTime, Reserved1, Reserved2, Reserved3, Reserved4, sb.ToString());
        }
        public override string ToString()
        {
            return string.Format("{0}<{1}>",
                TerminalId, CreateCommand());
        }
        private string GetBooleanAsString(bool value)
        {
            return (value ? "1" : "0");
        }
        private bool GetCharAsBoolean(char value)
        {
            return (value == '1' ? true : false);
        }
        private void GetLatchStatus(string value)
        {
            var charArray = value.ToCharArray();
            if (charArray != null && charArray.Length > 5)
            {
                Aux1LatchState = GetCharToLatchState(charArray[5]);
                Aux2LatchState = GetCharToLatchState(charArray[4]);
                Aux3LatchState = GetCharToLatchState(charArray[3]);
                Aux4LatchState = GetCharToLatchState(charArray[2]);
                Aux5LatchState = GetCharToLatchState(charArray[1]);
                Aux6LatchState = GetCharToLatchState(charArray[0]);
            }
        }
        private GTAuxLatchState GetCharToLatchState(char c)
        {
            switch (c)
            {
                case '0':
                    return GTAuxLatchState.LatchOnLow;
                case '1':
                    return GTAuxLatchState.LatchOnHigh;
                default:
                    return GTAuxLatchState.DisableLatching;
            }
        }
        public void Parse(string[] strArray)
        {
            if (strArray != null)
            {
                if (strArray.Length > 12)
                {
                    int.TryParse(strArray[5], out int storageSnapshotInterval);
                    StorageSnapshotInterval = storageSnapshotInterval;

                    int.TryParse(strArray[6], out int storageRmsSendInterval);
                    StorageRmsSendInterval = storageRmsSendInterval;

                    int.TryParse(strArray[8], out int auxLatchTime);
                    AuxLatchTime = auxLatchTime;

                    Reserved1 = strArray[9];
                    Reserved2 = strArray[10];
                    Reserved3 = strArray[11];
                    Reserved4 = strArray[12];
                }

                if (strArray.Length > 14)
                {
                    var charArray = strArray[14].ToCharArray();

                    ClearStorage = GetCharAsBoolean(charArray[0]);
                    StorageFoundStatus = GetCharAsBoolean(charArray[1]);
                    StorageBusyStatus = GetCharAsBoolean(charArray[2]);
                    ReservedFlag1 = GetCharAsBoolean(charArray[3]);
                    ReservedFlag2 = GetCharAsBoolean(charArray[4]);
                    ReservedFlag3 = GetCharAsBoolean(charArray[5]);
                    ReservedFlag4 = GetCharAsBoolean(charArray[6]);
                    ReservedFlag5 = GetCharAsBoolean(charArray[7]);

                }
            }
        }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        public string ToFormattedJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
