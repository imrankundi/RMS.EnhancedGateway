﻿using Newtonsoft.Json;
using RMS.Server.DataTypes.Requests;
using System;
using System.Text;

namespace RMS.Protocols.GT
{
    public class GTResetRom : ICGRC
    {
        public string TerminalId { get; set; }
        public string Code => "NA";
        public GTCommandType CommandType { get; set; }
        public string CommandTypeDescription => CommandType.ToString();
        public int TimerInterval { get; set; }
        public GTResetRom(string terminalId)
        {
            TerminalId = terminalId;
            CommandType = GTCommandType.ResetRom;
        }
        public string CreateCommand()
        {
            return "RESETGDT";
        }
        public override string ToString()
        {
            return CreateCommand();
        }
        public void Parse(string[] strArray)
        {
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
