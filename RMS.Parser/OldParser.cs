using RMS.Core.Common;
using RMS.Core.Enumerations;
using RMS.Core.Logging;
using RMS.Core.QueryBuilder;
using LoreSoft.MathExpressions;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RMS.Parser
{
    public class OldParser
    {
        private const char splitter = ',';
        private ReceivedPacket packet;
        public ReceivedPacket Packet => packet;


        public OldParser(ReceivedPacket packet)
        {
            this.packet = packet;
        }

        public virtual bool Parse()
        {
            Protocol protocol = ProtocolList.Instance.Find(Packet.ProtocolHeader);
            if (protocol == null)
                return false;

            if (string.IsNullOrWhiteSpace(Packet.Data))
                return false;

            string[] array = Packet.Data.Split(splitter);

            if (array == null)
                return false;

            if (array.Length == 0)
                return false;

            ParsedPacket parsedPacket = new ParsedPacket()
            {
                TerminalId = Packet.TerminalId,
                ProtocolHeader = Packet.ProtocolHeader,
                ReceivedOn = DateTimeHelper.CurrentUniversalTime
            };

            if (protocol.PageNumberIndex != -1)
            {
                if (array.Length > protocol.PageNumberIndex)
                {
                    parsedPacket.PageNumber = int.Parse(array[protocol.PageNumberIndex]);
                }
            }

            Page page = protocol.FindPage(parsedPacket.PageNumber);
            if (page == null)
                return false;

            List<string> values = new List<string>();

            foreach (Parameter mapping in page.Parameters)
            {
                try
                {
                    values.Clear();
                    foreach (int index in mapping.ParameterIndexes)
                    {
                        values.Add(array[index]);
                    }

                    string value = string.Empty;
                    if (!string.IsNullOrWhiteSpace(mapping.Expression))
                    {
                        value = string.Format(mapping.Expression, values.ToArray());
                    }

                    if (mapping.ParameterType == ParameterType.Id)
                    {
                        int.TryParse(value, out int id);
                        parsedPacket.Id = id.ToString("00");
                    }
                    else if (mapping.DataType == DataType.DateTime)
                    {
                        DateTime datetime = DateTimeHelper.Parse(value, DateTimeFormat.UK.DateTime);
                        parsedPacket.Fields.Add(new Field(mapping.Field, datetime));
                    }
                    else if (mapping.DataType == DataType.Float)
                    {
                        byte[] bytes = new byte[4];
                        for (int ii = 0; ii < values.Count; ii++)
                        {
                            bytes[ii] = Convert.ToByte(values[ii]);
                        }

                        float floatValue = BitConverter.ToSingle(bytes, 0);
                        double val = Math.Round(floatValue, mapping.Precision);
                        parsedPacket.Fields.Add(new Field(mapping.Field, val));
                    }
                    else if (mapping.DataType == DataType.MicrochipFloat)
                    {

                        /* Float 1 bit for sign 8 bits for exponent and 23 bits for mantissa
                         * Microchip Float
                         *     a         b       c        d
                         * eeeeeeee Sxxxxxxx xxxxxxxx xxxxxxxx where x = 0 or 1 and S is sign bit
                         * 
                         * Standard Float
                         *     a         b       c        d
                         * Seeeeeee exxxxxxx xxxxxxxx xxxxxxxx where x = 0 or 1 and S is sign bit
                         * 
                         * conversion performed to make microchip float to standard float
                         * f = a & 0x80
                         * e = a & 0x01
                         * a >>= 1
                         * a |= f
                         * e <<= 7
                         * b |= e
                         */


                        byte[] bytes = new byte[4];
                        for (int ii = 0; ii < values.Count; ii++)
                        {
                            bytes[ii] = Convert.ToByte(values[ii]);
                        }

                        byte f = Convert.ToByte(bytes[0] & Convert.ToByte(0x80));
                        byte e = Convert.ToByte(bytes[0] & Convert.ToByte(0x01));

                        bytes[0] >>= 1;
                        bytes[0] |= f;

                        e <<= 7;
                        bytes[1] |= e;

                        float floatValue = BitConverter.ToSingle(bytes, 0);
                        double val = Math.Round(floatValue, mapping.Precision);
                        parsedPacket.Fields.Add(new Field(mapping.Field, val));
                    }
                    else if (mapping.Evaluate)
                    {
                        MathEvaluator evaluator = new MathEvaluator();
                        double val = Math.Round(evaluator.Evaluate(value), mapping.Precision);

                        if (mapping.DataType == DataType.Binary)
                        {
                            int bin = Convert.ToInt32(val);
                            int length = mapping.ParameterIndexes.Count * 8;
                            string binary = ConversionHelper.ToBinary(bin, length);
                            parsedPacket.Fields.Add(new Field(mapping.Field, binary));
                        }
                        else if (mapping.DataType == DataType.Epoch)
                        {
                            DateTime datetime = DateTimeHelper.FromEpoch(val);
                            parsedPacket.Fields.Add(new Field(mapping.Field, datetime));
                        }
                        else
                        {
                            if (mapping.Signed)
                            {
                                int power = (mapping.ParameterIndexes.Count * 8) - 1;
                                double maxValue = Math.Pow(2, power) - 1;
                                //val = val > maxValue ? maxValue - val : val;
                                val = val > maxValue ? val - maxValue * 2 : val;

                            }

                            float factor = mapping.Factor == 0 ? 1 : mapping.Factor;

                            val = Math.Round(val * factor, mapping.Precision);

                            parsedPacket.Fields.Add(new Field(mapping.Field, val));

                        }
                    }
                    else
                    {
                        object val = DataTypeHelper.Convert(mapping.DataType, value);

                        parsedPacket.Fields.Add(new Field(mapping.Field, val));
                    }
                }
                catch (Exception ex)
                {
                    LoggingManager.Log(ex);
                    return false;
                }
            }

            ExecuteQuery(parsedPacket);
            return true;
        }

        public void ExecuteQuery(ParsedPacket packet)
        {
            Console.WriteLine(JsonConvert.SerializeObject(packet, Formatting.Indented));
            ParsedPacketQueryBuilder query = new ParsedPacketQueryBuilder(packet);
            try
            {
                //QueryExecutor.ExecuteWithLock(query.RealTimeQuery());
                //QueryExecutor.ExecuteWithLock(query.SiteQuery());
            }
            catch (Exception ex)
            {
                LoggingManager.Log(ex);
            }
        }
    }
}
