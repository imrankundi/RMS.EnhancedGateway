using Newtonsoft.Json;
using RMS.Server.DataTypes.Requests;
using System;
using System.Text;

namespace RMS.Protocols.GT
{
    public class GTGeneralSettings : ICGRC
    {
        public string TerminalId { get; set; }
        public string Code => "00";
        public GTCommandType CommandType { get; set; }
        public string CommandTypeDescription => CommandType.ToString();
        public string User1 { get; set; }
        public string User2 { get; set; }
        public string User3 { get; set; }
        public string User4 { get; set; }
        public string User5 { get; set; }
        public string User6 { get; set; }
        public string User7 { get; set; }
        public string User8 { get; set; }
        public string User9 { get; set; }
        public string User10 { get; set; }
        public string FirmwareVersion { get; private set; }
        public string CompilationDate { get; private set; }
        public string CompilationTime { get; private set; }
        public bool GPRS { get; set; }
        public bool SmsFallBack { get; set; }
        public bool AlertUser1OnSMS { get; set; }
        public bool GPRSReconnection { get; set; }
        public bool RS232Port { get; set; }
        public bool RS485Port { get; set; }
        public bool Polling { get; set; }
        public bool BypassMode { get; set; }
        public bool CounterString { get; set; }
        public bool FuelString { get; set; }
        public bool ResetCounter2 { get; set; }
        public bool ResetCounter3 { get; set; }
        public bool ResetCounter4 { get; set; }
        public bool ResetCounter5 { get; set; }
        public bool ResetCounter6 { get; set; }
        public bool ResetCounter7 { get; set; }
        public bool ResetCounter8 { get; set; }
        public bool ForcedReset { get; set; }
        public bool ModemReset { get; set; }
        public bool GTReset { get; set; }
        public bool Modbus { get; set; }
        public bool Reserved1 { get; set; }
        public bool Storage { get; set; }
        public bool Reserved3 { get; set; }
        public bool Reserved4 { get; set; }
        public bool Reserved5 { get; set; }
        public bool Reserved6 { get; set; }
        public bool Reserved7 { get; set; }
        public bool Reserved8 { get; set; }
        public bool Reserved9 { get; set; }
        public bool CounterFormat { get; set; }
        public bool Reserved11 { get; set; }
        public bool Reserved12 { get; set; }
        public bool Reserved13 { get; set; }

        public GTGeneralSettings(string terminalId)
        {
            TerminalId = terminalId;
            CommandType = GTCommandType.GeneralSettings;
        }
        public string CreateCommand()
        {
            User1 = string.IsNullOrEmpty(User1) ? "" : User1;
            User2 = string.IsNullOrEmpty(User2) ? "" : User2;
            User3 = string.IsNullOrEmpty(User3) ? "" : User3;
            User4 = string.IsNullOrEmpty(User4) ? "" : User4;
            User5 = string.IsNullOrEmpty(User5) ? "" : User5;
            User8 = string.IsNullOrEmpty(User8) ? "" : User8;
            User9 = string.IsNullOrEmpty(User9) ? "" : User9;
            User10 = string.IsNullOrEmpty(User10) ? "" : User10;

            StringBuilder sb = new StringBuilder();
            
            sb.Append(GetBooleanAsString(GPRS));
            sb.Append(GetBooleanAsString(SmsFallBack));
            sb.Append(GetBooleanAsString(Reserved1));
            sb.Append(GetBooleanAsString(AlertUser1OnSMS));
            sb.Append(GetBooleanAsString(GPRSReconnection));
            sb.Append(GetBooleanAsString(RS232Port));
            sb.Append(GetBooleanAsString(RS485Port));
            sb.Append(GetBooleanAsString(Polling));
            sb.Append(GetBooleanAsString(BypassMode));

            sb.Append(GetBooleanAsString(CounterString));
            sb.Append(GetBooleanAsString(FuelString));
            sb.Append(GetBooleanAsString(ResetCounter2));
            sb.Append(GetBooleanAsString(ResetCounter3));
            sb.Append(GetBooleanAsString(ResetCounter4));
            sb.Append(GetBooleanAsString(ResetCounter5));
            sb.Append(GetBooleanAsString(ResetCounter6));
            sb.Append(GetBooleanAsString(ResetCounter7));
            sb.Append(GetBooleanAsString(ResetCounter8));
            sb.Append(GetBooleanAsString(Modbus));
            
            sb.Append(GetBooleanAsString(Storage));
            sb.Append(GetBooleanAsString(Reserved3));
            sb.Append(GetBooleanAsString(Reserved4));
            sb.Append(GetBooleanAsString(Reserved5));
            sb.Append(GetBooleanAsString(Reserved6));
            sb.Append(GetBooleanAsString(Reserved7));
            sb.Append(GetBooleanAsString(Reserved8));
            sb.Append(GetBooleanAsString(Reserved9));
            sb.Append(GetBooleanAsString(CounterFormat));
            sb.Append(GetBooleanAsString(Reserved11));
            sb.Append(GetBooleanAsString(Reserved12));
            sb.Append(GetBooleanAsString(Reserved13));

            return string.Format("CGRC(ID({0},N,N)N({1},{2},{3},{4},{5},{6},{7},{8},{9},{10},)L({11})",
                Code, User1, User2, User3, User4, User5, User6, User7, User8, User9, User10, sb.ToString());
        }
        public override string ToString()
        {
            return string.Format("{0}<{1}>", TerminalId, CreateCommand());
        }
        private string GetBooleanAsString(bool value)
        {
            return (value ? "1" : "0");
        }
        private bool GetCharAsBoolean(char value)
        {
            return (value == '1' ? true : false);
        }
        public void Parse(string[] strArray)
        {
            if (strArray != null)
            {
                if (strArray.Length > 17)
                {
                    User1 = strArray[5];
                    User2 = strArray[6];
                    User3 = strArray[7];
                    User4 = strArray[8];
                    User5 = strArray[9];
                    User6 = strArray[10];
                    User7 = strArray[11];
                    User8 = strArray[12];
                    User9 = strArray[13];
                    User10 = strArray[14];
                    FirmwareVersion = strArray[15];
                    CompilationDate = strArray[16];
                    CompilationTime = strArray[17];
                }

                if(strArray.Length > 47)
                {
                    var charArray = strArray[47].ToCharArray();

                    
                    if(charArray.Length > 33)
                    {
                        GPRS = GetCharAsBoolean(charArray[0]);
                        SmsFallBack = GetCharAsBoolean(charArray[1]);
                        Reserved1 = GetCharAsBoolean(charArray[2]);
                        AlertUser1OnSMS = GetCharAsBoolean(charArray[3]);
                        GPRSReconnection = GetCharAsBoolean(charArray[4]);
                        RS232Port = GetCharAsBoolean(charArray[5]);
                        RS485Port = GetCharAsBoolean(charArray[6]);
                        Polling = GetCharAsBoolean(charArray[7]);
                        BypassMode = GetCharAsBoolean(charArray[8]);
                        CounterString = GetCharAsBoolean(charArray[9]);
                        FuelString = GetCharAsBoolean(charArray[10]);
                        ResetCounter2 = GetCharAsBoolean(charArray[11]);
                        ResetCounter3 = GetCharAsBoolean(charArray[12]);
                        ResetCounter4 = GetCharAsBoolean(charArray[13]);
                        ResetCounter5 = GetCharAsBoolean(charArray[14]);
                        ResetCounter6 = GetCharAsBoolean(charArray[15]);
                        ResetCounter7 = GetCharAsBoolean(charArray[16]);
                        ResetCounter8 = GetCharAsBoolean(charArray[17]);
                        ForcedReset = GetCharAsBoolean(charArray[18]);
                        GTReset = GetCharAsBoolean(charArray[19]);
                        ModemReset = GetCharAsBoolean(charArray[20]);

                        Modbus = GetCharAsBoolean(charArray[21]);
                        
                        Storage = GetCharAsBoolean(charArray[22]);
                        Reserved3 = GetCharAsBoolean(charArray[23]);
                        Reserved4 = GetCharAsBoolean(charArray[24]);
                        Reserved5 = GetCharAsBoolean(charArray[25]);
                        Reserved6 = GetCharAsBoolean(charArray[26]);
                        Reserved7 = GetCharAsBoolean(charArray[27]);
                        Reserved8 = GetCharAsBoolean(charArray[28]);
                        Reserved9 = GetCharAsBoolean(charArray[29]);
                        CounterFormat = GetCharAsBoolean(charArray[30]);
                        Reserved11 = GetCharAsBoolean(charArray[31]);
                        Reserved12 = GetCharAsBoolean(charArray[32]);
                        Reserved13 = GetCharAsBoolean(charArray[33]);
                    }
                    

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
