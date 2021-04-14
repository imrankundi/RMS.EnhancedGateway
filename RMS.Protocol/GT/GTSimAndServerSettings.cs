using Newtonsoft.Json;
using RMS.Server.DataTypes.Requests;
using System;

namespace RMS.Protocols.GT
{
    public class GTSimAndServerSettings : ICGRC
    {
        [JsonProperty("terminalId")]
        public string TerminalId { get; set; }
        [JsonProperty("code")]
        public string Code => "01";
        [JsonProperty("commandType")]
        public GTCommandType CommandType { get; set; }
        [JsonProperty("commandTypeDescription")]
        public string CommandTypeDescription => CommandType.ToString();
        [JsonProperty("sim1Number")]
        public string Sim1Number { get; set; }
        [JsonProperty("sim1APN")]
        public string Sim1APN { get; set; }
        [JsonProperty("sim1UserId")]
        public string Sim1UserId { get; set; }
        [JsonProperty("sim1Password")]
        public string Sim1Password { get; set; }
        [JsonProperty("sim2Number")]
        public string Sim2Number { get; set; }
        [JsonProperty("sim2APN")]
        public string Sim2APN { get; set; }
        [JsonProperty("sim2UserID")]
        public string Sim2UserId { get; set; }
        [JsonProperty("sim2Password")]
        public string Sim2Password { get; set; }
        [JsonProperty("gprsTerminalID")]
        public string GPRSTerminalID { get; set; }
        [JsonProperty("serverNumber")]
        public string ServerNumber { get; set; }
        [JsonProperty("serverIP")]
        public string ServerIP { get; set; }
        [JsonProperty("serverPort")]
        public int ServerPort { get; set; }

        public GTSimAndServerSettings(string terminalId)
        {
            TerminalId = terminalId;
            CommandType = GTCommandType.SimAndServerSettings;
        }
        public void Parse(string[] strArray)
        {
            if (strArray != null)
            {
                if (strArray.Length > 29)
                {
                    Sim1Number = strArray[18];
                    Sim1APN = strArray[19];
                    Sim1UserId = strArray[20];
                    Sim1Password = strArray[21];
                    Sim2Number = strArray[22];
                    Sim2APN = strArray[23];
                    Sim2UserId = strArray[24];
                    Sim2Password = strArray[25];
                    GPRSTerminalID = strArray[26];
                    ServerNumber = strArray[27];
                    ServerIP = strArray[28];

                    int.TryParse(strArray[29], out int port);
                    ServerPort = port;
                }
            }
        }

        private void Validate()
        {
            if (string.IsNullOrEmpty(Sim1Number) ||
            string.IsNullOrEmpty(Sim1APN) ||
            string.IsNullOrEmpty(Sim1UserId) ||
            string.IsNullOrEmpty(Sim1Password) ||
            string.IsNullOrEmpty(Sim2Number) ||
            string.IsNullOrEmpty(Sim2APN) ||
            string.IsNullOrEmpty(Sim2UserId) ||
            string.IsNullOrEmpty(Sim2Password) ||
            string.IsNullOrEmpty(GPRSTerminalID) ||
            string.IsNullOrEmpty(ServerNumber) ||
            string.IsNullOrEmpty(ServerIP))
            {
                throw new Exception("All parameters are required while setting GT Configuration");
            }
        }
        public string CreateCommand()
        {
            Sim1Number = string.IsNullOrEmpty(Sim1Number) ? "" : Sim1Number;
            Sim1APN = string.IsNullOrEmpty(Sim1APN) ? "" : Sim1APN;
            Sim1UserId = string.IsNullOrEmpty(Sim1UserId) ? "" : Sim1UserId;
            Sim1Password = string.IsNullOrEmpty(Sim1Password) ? "" : Sim1Password;
            Sim2Number = string.IsNullOrEmpty(Sim2Number) ? "" : Sim2Number;
            Sim2APN = string.IsNullOrEmpty(Sim2APN) ? "" : Sim2APN;
            Sim2UserId = string.IsNullOrEmpty(Sim2UserId) ? "" : Sim2UserId;
            Sim2Password = string.IsNullOrEmpty(Sim2Password) ? "" : Sim2Password;
            GPRSTerminalID = string.IsNullOrEmpty(GPRSTerminalID) ? "" : GPRSTerminalID;
            ServerNumber = string.IsNullOrEmpty(ServerNumber) ? "" : ServerNumber;
            ServerIP = string.IsNullOrEmpty(ServerIP) ? "" : ServerIP;
            return string.Format("CGRC(ID({0},N,N)N({1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},)",
                Code, Sim1Number, Sim1APN, Sim1UserId, Sim1Password, Sim2Number, Sim2APN,
                Sim2UserId, Sim2Password, GPRSTerminalID, ServerNumber, ServerIP, ServerPort);
        }
        public override string ToString()
        {
            //Validate();
            return string.Format("{0}<{1}>", TerminalId, CreateCommand());
        }
    }
}
