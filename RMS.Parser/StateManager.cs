using RMS.Core.Logging;
using RMS.Core.QueryBuilder;
using System;
using System.Collections.Generic;

namespace RMS.Parser
{
    public class StateManager
    {
        public static StateManager Instance => instance;

        private static StateManager instance = new StateManager();

        public Dictionary<string, ParsedSitePacket> SitePackets { get; set; }

        private StateManager()
        {
            SitePackets = new Dictionary<string, ParsedSitePacket>();
        }

        public void SaveState(ParsedPacket packet)
        {
            ParsedSitePacket sitePacket = null;

            if (packet == null)
                return;

            if (!SitePackets.ContainsKey(packet.TerminalId))
            {
                sitePacket = new ParsedSitePacket(packet);
                SitePackets.Add(sitePacket.TerminalId, sitePacket);
            }
            else
            {
                sitePacket = SitePackets[packet.TerminalId];
                if (sitePacket != null)
                {
                    sitePacket.AddDevicePacket(packet);
                }
            }
        }

        public ParsedSitePacket FindSitePacket(string terminalId)
        {
            if (!SitePackets.ContainsKey(terminalId))
                return null;

            return SitePackets[terminalId];
        }

        public ParsedPagePacket FindPagePacket(ParsedPacket packet)
        {
            ParsedSitePacket sitePacket = FindSitePacket(packet.TerminalId);
            if (sitePacket == null)
                return null;

            ParsedDevicePacket devicePacket = sitePacket.FindDevicePacket(packet.ProtocolHeader);
            if (devicePacket == null)
                return null;

            ParsedPagePacket pagePacket = devicePacket.FindPagePacket(packet.PageNumber);


            return pagePacket;
        }

        public ParsedPagePacket FindPagePacketCopy(ParsedPacket packet)
        {
            ParsedPagePacket pagePacket = FindPagePacket(packet);
            if (pagePacket == null)
                return null;

            ParsedPagePacket pagePacketCopy = new ParsedPagePacket(pagePacket.Packet);


            return pagePacketCopy;
        }


        public void ExecuteQuery(ParsedPacket packet)
        {
            ParsedPacketQueryBuilder query = new ParsedPacketQueryBuilder(packet);
            try
            {
                //QueryExecutor.ExecuteWithLock(query.RealTimeQuery());
                //QueryExecutor.Execute(query.SiteQuery());
            }
            catch (Exception ex)
            {
                LoggingManager.Log(ex);
            }
        }
    }



    public class ParsedSitePacket
    {
        public string TerminalId { get; set; }
        public Dictionary<string, ParsedDevicePacket> DevicePackets { get; set; }

        public ParsedSitePacket(ParsedPacket packet)
        {
            this.TerminalId = packet.TerminalId;
            DevicePackets = new Dictionary<string, ParsedDevicePacket>();
            AddDevicePacket(packet);
        }

        public void AddDevicePacket(ParsedPacket packet)
        {
            ParsedDevicePacket devicePacket = null;
            if (!DevicePackets.ContainsKey(packet.ProtocolHeader))
            {
                devicePacket = new ParsedDevicePacket(packet);
                DevicePackets.Add(devicePacket.ProtocolHeader, devicePacket);
            }
            else
            {
                devicePacket = DevicePackets[packet.ProtocolHeader];
                if (devicePacket != null)
                    devicePacket.AddPagePacket(packet);
            }
        }

        public ParsedDevicePacket FindDevicePacket(string protocolHeader)
        {
            if (!DevicePackets.ContainsKey(protocolHeader))
                return null;

            return DevicePackets[protocolHeader];
        }

    }

    public class ParsedDevicePacket
    {
        public DateTime ReceivedOn { get; set; }
        public string ProtocolHeader { get; set; }
        public string Id { get; set; }
        public Dictionary<int, ParsedPagePacket> Pages { get; set; }

        public ParsedDevicePacket(ParsedPacket packet)
        {
            Pages = new Dictionary<int, ParsedPagePacket>();
            ReceivedOn = packet.ReceivedOn;
            Id = packet.Id;
            ProtocolHeader = packet.ProtocolHeader;
            AddPagePacket(packet);
        }

        public void AddPagePacket(ParsedPacket packet)
        {
            ParsedPagePacket pagePacket = null;
            if (!Pages.ContainsKey(packet.PageNumber))
            {
                pagePacket = new ParsedPagePacket(packet);
                Pages.Add(pagePacket.PageNumber, pagePacket);
            }
            else
            {
                pagePacket = Pages[packet.PageNumber];
                if (pagePacket != null)
                    pagePacket.ModifyFields(packet);
            }
        }

        public ParsedPagePacket FindPagePacket(int pageNumber)
        {
            if (!Pages.ContainsKey(pageNumber))
                return null;

            return Pages[pageNumber];
        }
    }

    public class ParsedPagePacket
    {
        public ParsedPacket Packet { get; set; }
        public int PageNumber { get; internal set; }
        public Dictionary<string, Field> Fields { get; set; }

        public ParsedPagePacket(ParsedPacket packet)
        {
            PageNumber = packet.PageNumber;
            Fields = new Dictionary<string, Field>();
            ModifyFields(packet);
        }

        public void ModifyFields(ParsedPacket packet)
        {
            Packet = packet;
            foreach (Field field in packet.Fields)
            {
                ModifyField(field);
            }
        }

        private void ModifyField(Field field)
        {
            if (field == null)
                return;

            if (!Fields.ContainsKey(field.Name))
                Fields.Add(field.Name, field);
            else
                Fields[field.Name] = field;
        }

    }





}
