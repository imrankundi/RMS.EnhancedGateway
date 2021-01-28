using Newtonsoft.Json;
using RMS.Server.DataTypes.Requests;
using System;
using System.Text;

namespace RMS.Protocols.GT
{
    public enum GTAuxLatchState
    {
        LatchOnLow = 0,
        LatchOnHigh = 1,
        DisableLatching = 2
    }
}
