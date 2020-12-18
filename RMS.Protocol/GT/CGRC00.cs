using System.Text;

namespace RMS.Protocols.GT
{
    public class CGRC00
    {
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
        public bool GPRS { get; set; }
        public bool AlertUser1onSMS { get; set; }
        public bool GPRSReconnection { get; set; }
        public bool RS232Port { get; set; }
        public bool RS485Port { get; set; }
        public bool Polling { get; set; }
        public bool CounterString { get; set; }
        public bool FuelString { get; set; }
        public bool ResetCounter2 { get; set; }
        public bool ResetCounter3 { get; set; }
        public bool ResetCounter4 { get; set; }
        public bool ResetCounter5 { get; set; }
        public bool ResetCounter6 { get; set; }
        public bool ResetCounter7 { get; set; }
        public bool ResetCounter8 { get; set; }
        public bool Modbus { get; set; }
        public bool Reserved1 { get; set; }
        public bool Reserved2 { get; set; }
        public bool Reserved3 { get; set; }
        public bool Reserved4 { get; set; }
        public bool Reserved5 { get; set; }
        public bool Reserved6 { get; set; }
        public bool Reserved7 { get; set; }
        public bool Reserved8 { get; set; }
        public bool Reserved9 { get; set; }
        public bool Reserved10 { get; set; }
        public bool Reserved11 { get; set; }
        public bool Reserved12 { get; set; }


        public override string ToString()
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

            return string.Format("CGRC(ID(00,N,N)N({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},)L(1000101101000000000001100000000000)",
                User1, User2, User3, User4, User5, User6, User7, User8, User9, User10);
        }
    }
}
