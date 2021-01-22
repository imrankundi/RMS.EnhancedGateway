using RMS.Core.Common;
using RMS.Core.Logging;
using System;
using System.Text;
using System.Threading;

namespace RMS.Parser
{
    public class ParsingManager
    {

        public static void Parse(string packet)
        {
            SecondLevelParser(packet);
        }

        public static ReceivedPacket FirstLevelParser(string packet)
        {
            try
            {
                int index = 0;
                string terminalId = string.Empty;
                int terminalIdLength = 8;
                string pkt = string.Empty;
                packet = packet.TrimEnd('\n').TrimEnd('\r');
                packet = packet.TrimEnd('\r').TrimEnd('\n');
                index = packet.IndexOf('<');
                if (index != -1)
                {

                    if(index > terminalIdLength)
                    {
                        int start = (index - terminalIdLength);
                        terminalId = packet.Substring(start, terminalIdLength);
                    }
                    else
                    {
                        terminalId = packet.Substring(0, index);
                    }



                    pkt = packet.Substring(index, packet.Length - index).TrimStart('<').TrimEnd('>');

                    string protocolHeader = string.Empty;
                    string data = string.Empty;
                    index = pkt.IndexOf('(');
                    if (index != -1)
                    {
                        protocolHeader = pkt.Substring(0, index);
                        if (protocolHeader.StartsWith("M") || protocolHeader.StartsWith("3P"))
                        {
                            StringBuilder sb = new StringBuilder();
                            byte[] dataBytes = Encoding.Default.GetBytes(pkt.Substring(index, pkt.Length - index).TrimStart('(').TrimEnd(')'));

                            foreach (byte b in dataBytes)
                            {
                                int value = b;
                                sb.Append(value.ToString());
                                sb.Append(",");
                            }

                            data = sb.ToString().TrimEnd(',');

                            return new ReceivedPacket()
                            {
                                ReceivedOn = DateTimeHelper.CurrentUniversalTime,
                                Data = data,
                                ProtocolHeader = protocolHeader,
                                TerminalId = terminalId
                            };
                        }
                        else
                        {
                            data = pkt.Substring(index, pkt.Length - index).Replace('(', ',').Replace(')', ',').TrimStart(',').TrimEnd(',');

                            return new ReceivedPacket()
                            {
                                ReceivedOn = DateTimeHelper.CurrentUniversalTime,
                                Data = data,
                                ProtocolHeader = protocolHeader,
                                TerminalId = terminalId
                            };
                        }
                    }
                    else if (pkt.Equals("PONG"))
                    {
                        return new ReceivedPacket()
                        {
                            ReceivedOn = DateTimeHelper.CurrentUniversalTime,
                            Data = pkt,
                            ProtocolHeader = pkt,
                            TerminalId = terminalId
                        };
                    }
                }


            }
            catch (Exception ex)
            {
                LoggingManager.Log(ex);
            }

            return null;
        }

        public static ReonParsedPacket SecondLevelParser(ReceivedPacket receivedPacket)
        {
            try
            {
                if (receivedPacket != null)
                {
                    PacketParser parser = new PacketParser(receivedPacket);

                    //#if DEBUG
                    return ParsePacket(parser);
                    //#else
                    //                    if (parser != null)
                    //                        (new Thread(() => ParsePacket(parser))).Start();
                    //#endif
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Log(ex);
            }

            return null;

        }

        private static ReonParsedPacket SecondLevelParser(string packet)
        {
            try
            {
                string[] pkt = packet.Split('|');
                if (pkt.Length > 0 && pkt.Length == 3)
                {
                    PacketParser parser = new PacketParser(new ReceivedPacket()
                    {
                        Data = pkt[2],
                        ProtocolHeader = pkt[1],
                        TerminalId = pkt[0]
                    });

                    //#if DEBUG
                    return ParsePacket(parser);
                    //#else
                    //                    if (parser != null)
                    //                        (new Thread(() => ParsePacket(parser))).Start();
                    //#endif


                }
            }
            catch (Exception ex)
            {
                LoggingManager.Log(ex);
            }

            return null;

        }

        private static void ParsePacket(string packet)
        {
            try
            {
                //DeviceParser parser = null;
                PacketParser parser = null;
                int index = 0;
                string terminalId = string.Empty;
                string pkt = string.Empty;

                index = packet.IndexOf('<');
                if (index != -1)
                {
                    terminalId = packet.Substring(0, index);
                    pkt = packet.Substring(index, packet.Length - index).TrimStart('<').TrimEnd('>');

                    string protocolHeader = string.Empty;
                    string data = string.Empty;
                    index = pkt.IndexOf('(');
                    if (index != -1)
                    {
                        protocolHeader = pkt.Substring(0, index);
                        if (protocolHeader.StartsWith("M") || protocolHeader.StartsWith("3P"))
                        {
                            StringBuilder sb = new StringBuilder();
                            byte[] dataBytes = Encoding.Default.GetBytes(pkt.Substring(index, pkt.Length - index).TrimStart('(').TrimEnd(')'));

                            foreach (byte b in dataBytes)
                            {
                                int value = b;
                                sb.Append(value.ToString());
                                sb.Append(",");
                            }

                            data = sb.ToString().TrimEnd(',');

                            parser = new PacketParser(new ReceivedPacket()
                            {
                                Data = data,
                                ProtocolHeader = protocolHeader,
                                TerminalId = terminalId
                            });
                        }
                        else
                        {
                            data = pkt.Substring(index, pkt.Length - index).Replace('(', ',').Replace(')', ',').TrimStart(',').TrimEnd(',');

                            parser = new PacketParser(new ReceivedPacket()
                            {
                                Data = data,
                                ProtocolHeader = protocolHeader,
                                TerminalId = terminalId
                            });



                        }

                        if (parser != null)
                            new Thread(() => ParsePacket(parser)).Start();
                    }
                }

            }
            catch (Exception ex)
            {
                LoggingManager.Log(ex);
            }

        }

        public static ReonParsedPacket ParsePacket(PacketParser parser)
        {
            return parser.Parse();
        }

    }
}
