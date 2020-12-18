namespace RMS.Protocols.GT
{
    public class CGRC01
    {
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

        public override string ToString()
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
            return string.Format("CGRC(ID(01,N,N)N({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},)",
                SIM1Number, SIM1APN, Sim1UserID, Sim1Password, SIM2Number, Sim2Password, GPRSTerminalID, ServerNumber,
                ServerIP, ServerPort);
        }
    }
}
