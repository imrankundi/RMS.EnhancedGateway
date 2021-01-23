using Newtonsoft.Json;
using RMS.Server.DataTypes.Requests;
using System;
using System.Text;

namespace RMS.Protocols.GT
{
    public class GTExtendedConfigurationSettings : ICGRC
    {
        public string TerminalId { get; private set; }
        public string Code => "03";
        public GTCommandType CommandType { get; set; }
        public string CommandTypeDescription => CommandType.ToString();
        public string N1 { get; set; }
        public string N2 { get; set; }
        public string N3 { get; set; }
        public string N4 { get; set; }
        public string N5 { get; set; }
        public string N6 { get; set; }
        public string N7 { get; set; }
        public string N8 { get; set; }
        public bool L1 { get; set; }
        public bool L2 { get; set; }
        public bool L3 { get; set; }
        public bool L4 { get; set; }
        public bool L5 { get; set; }
        public bool L6 { get; set; }
        public bool L7 { get; set; }
        public bool L8 { get; set; }
        
        public GTExtendedConfigurationSettings(string terminalId)
        {
            TerminalId = terminalId;
            CommandType = GTCommandType.ExtendedConfigurationSettings;
        }
        public override string ToString()
        {
            N1 = string.IsNullOrEmpty(N1) ? "" : N1;
            N2 = string.IsNullOrEmpty(N2) ? "" : N2;
            N3 = string.IsNullOrEmpty(N3) ? "" : N3;
            N4 = string.IsNullOrEmpty(N4) ? "" : N4;
            N5 = string.IsNullOrEmpty(N5) ? "" : N5;
            N8 = string.IsNullOrEmpty(N8) ? "" : N8;

            StringBuilder sb = new StringBuilder();
            sb.Append(GetBooleanAsString(L1));
            sb.Append(GetBooleanAsString(L2));
            sb.Append(GetBooleanAsString(L3));
            sb.Append(GetBooleanAsString(L4));
            sb.Append(GetBooleanAsString(L5));
            sb.Append(GetBooleanAsString(L6));
            sb.Append(GetBooleanAsString(L7));
            sb.Append(GetBooleanAsString(L8));

            return string.Format("{0}<CGRC(ID({1},N,N)N({2},{3},{4},{5},{6},{7},{8},{9})L({10})>",
                TerminalId, Code, N1, N2, N3, N4, N5, N6, N7, N8, sb.ToString());
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
                if (strArray.Length > 12)
                {
                    N1 = strArray[5];
                    N2 = strArray[6];
                    N3 = strArray[7];
                    N4 = strArray[8];
                    N5 = strArray[9];
                    N6 = strArray[10];
                    N7 = strArray[11];
                    N8 = strArray[12];
                }

                if(strArray.Length > 14)
                {
                    var charArray = strArray[14].ToCharArray();

                    L1 = GetCharAsBoolean(charArray[0]);
                    L2 = GetCharAsBoolean(charArray[1]);
                    L3 = GetCharAsBoolean(charArray[2]);
                    L4 = GetCharAsBoolean(charArray[3]);
                    L5 = GetCharAsBoolean(charArray[4]);
                    L6 = GetCharAsBoolean(charArray[5]);
                    L7 = GetCharAsBoolean(charArray[6]);
                    L8 = GetCharAsBoolean(charArray[7]);

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
