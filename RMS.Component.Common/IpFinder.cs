using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace RMS.Component.Common
{
    [Obsolete("Still this is unfinished", false)]
    public class IpFinder
    {
        public static IPAddress FindHostIP()
        {
            String hostName = string.Empty;
            hostName = Dns.GetHostName();
            IPHostEntry myIP = Dns.GetHostEntry(hostName);
            IPAddress[] address = myIP.AddressList;

            if (address == null)
                return null;

            if (address.Length <= 0)
                return null;

            return address[0];
        }

        public static IEnumerable<IPAddress> GetAddresses()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var ips = host.AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetwork);
            return ips;
        }

        public static string FindHostName()
        {
            string hostName = string.Empty;
            hostName = Dns.GetHostName();
            return hostName;
        }
        public static IPAddress GetIPAddress(string hostName)
        {
            Ping ping = new Ping();
            var replay = ping.Send(hostName);

            if (replay.Status == IPStatus.Success)
            {
                return replay.Address;
            }
            return null;
        }

        public static string GetIP4Address()
        {
            string IP4Address = String.Empty;

            foreach (IPAddress IPA in Dns.GetHostAddresses(FindHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            if (IP4Address != String.Empty)
            {
                return IP4Address;
            }

            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            return IP4Address;
        }

    }
}
