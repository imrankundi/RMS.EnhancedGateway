using RMS.Server.DataTypes.Requests;

namespace RMS.Protocols.GT
{
    public class GTSimAndServerSettings : ICGRC
    {
        public string TerminalId { get; set; }
        public string Code => "01";
        public GTCommandType CommandType { get; set; }
        public string CommandTypeDescription => CommandType.ToString();
        public string SIM1Number { get; set; }
        public string SIM1APN { get; set; }
        public string Sim1UserID { get; set; }
        public string Sim1Password { get; set; }
        public string SIM2Number { get; set; }
        public string SIM2APN { get; set; }
        public string Sim2UserID { get; set; }
        public string Sim2Password { get; set; }
        public string GPRSTerminalID { get; set; }
        public string ServerNumber { get; set; }
        public string ServerIP { get; set; }
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
                    SIM1Number= strArray[18];
                    SIM1APN= strArray[19];
                    Sim1UserID= strArray[20];
                    Sim1Password= strArray[21];
                    SIM2Number= strArray[22];
                    SIM2APN= strArray[23];
                    Sim2UserID= strArray[24];
                    Sim2Password= strArray[25];
                    GPRSTerminalID= strArray[26];
                    ServerNumber = strArray[27];
                    ServerIP = strArray[28];

                    int.TryParse(strArray[29], out int port);
                    ServerPort = port;
                }
            }
        }
        public string CreateCommand()
        {
            SIM1Number = string.IsNullOrEmpty(SIM1Number) ? "" : SIM1Number;
            SIM1APN = string.IsNullOrEmpty(SIM1APN) ? "" : SIM1APN;
            Sim1UserID = string.IsNullOrEmpty(Sim1UserID) ? "" : Sim1UserID;
            Sim1Password = string.IsNullOrEmpty(Sim1Password) ? "" : Sim1Password;
            SIM2Number = string.IsNullOrEmpty(SIM2Number) ? "" : SIM2Number;
            Sim2Password = string.IsNullOrEmpty(Sim2Password) ? "" : Sim2Password;
            GPRSTerminalID = string.IsNullOrEmpty(GPRSTerminalID) ? "" : GPRSTerminalID;
            ServerNumber = string.IsNullOrEmpty(ServerNumber) ? "" : ServerNumber;
            ServerIP = string.IsNullOrEmpty(ServerIP) ? "" : ServerIP;
            return string.Format("CGRC(ID({0},N,N)N({1},{2},{3},{4},{5},{6},{7},{8},{9},{10})",
                TerminalId, Code, SIM1Number, SIM1APN, Sim1UserID, Sim1Password, SIM2Number, 
                Sim2Password, GPRSTerminalID, ServerNumber, ServerIP, ServerPort);
        }
        public override string ToString()
        {
            return string.Format("{0}<{1}>", TerminalId, CreateCommand());
        }
    }
}
