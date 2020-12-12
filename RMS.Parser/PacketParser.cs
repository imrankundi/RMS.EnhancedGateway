using LoreSoft.MathExpressions;
using Newtonsoft.Json;
using RMS.Core.Common;
using RMS.Core.Enumerations;
using RMS.Protocols;
using System;
using System.Collections.Generic;

namespace RMS.Parser
{
    public class PacketParser
    {
        private const char splitter = ',';

        public ReceivedPacket Packet { get; }


        public PacketParser(ReceivedPacket packet)
        {
            Packet = packet;
        }

        public virtual ReonParsedPacket Parse()
        {
            Dictionary<string, object> dataDictionary = new Dictionary<string, object>();
            Protocol protocol = ProtocolList.Instance.Find(Packet.ProtocolHeader);
            if (protocol == null)
                return null;

            if (string.IsNullOrWhiteSpace(Packet.Data))
                return null;

            string[] array = Packet.Data.Split(splitter);

            if (array == null)
                return null;

            if (array.Length == 0)
                return null;

            ReonParsedPacket parsedPacket = new ReonParsedPacket()
            {
                TerminalId = Packet.TerminalId,
                ProtocolHeader = Packet.ProtocolHeader,
                ReceivedOn = DateTimeHelper.CurrentUniversalTime
            };

            SiteInfo siteInfo = GetSiteInfo(parsedPacket.TerminalId);
            if (siteInfo != null)
            {
                parsedPacket.Mapping = siteInfo.Name;
            }
            dataDictionary.Add("DeviceType", Packet.ProtocolHeader);
            if (protocol.PageNumberIndex != -1)
            {
                if (array.Length > protocol.PageNumberIndex)
                {
                    parsedPacket.PageNumber = int.Parse(array[protocol.PageNumberIndex]);
                }
            }

            Page page = protocol.FindPage(parsedPacket.PageNumber);
            if (page == null)
                return null;


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
                        dataDictionary.Add("DeviceId", parsedPacket.Id);
                    }

                    else if (mapping.ParameterType == ParameterType.Data)
                    {
                        if (mapping.Evaluate)
                        {
                            MathEvaluator evaluator = new MathEvaluator();
                            double doubleValue = Math.Round(evaluator.Evaluate(value), mapping.Precision);

                            if (mapping.DataType == DataType.Binary)
                            {
                                int integerValue = Convert.ToInt32(doubleValue);
                                int length = mapping.ParameterIndexes.Count * 8;
                                string binary = ConversionHelper.ToBinary(integerValue, length);

                                /*------------------------------------------*/
                                BitwiseParameter(dataDictionary, mapping, binary);
                                /*------------------------------------------*/
                                //parsedPacket.Data.Add(mapping.Name, binary);
                            }
                            else if (mapping.DataType == DataType.Epoch)
                            {
                                DateTime datetime = DateTimeHelper.FromEpoch(doubleValue);
                                if (siteInfo != null)
                                {
                                    if (siteInfo.TimeOffset != null)
                                    {
                                        datetime = datetime.AddHours(siteInfo.TimeOffset.Hours);
                                        datetime = datetime.AddMinutes(siteInfo.TimeOffset.Minutes);
                                    }
                                }
                                dataDictionary.Add(mapping.Name, datetime);
                            }
                            else
                            {
                                if (mapping.Signed)
                                {
                                    int power = (mapping.ParameterIndexes.Count * 8) - 1;
                                    double maxValue = Math.Pow(2, power) - 1;
                                    //doubleValue = doubleValue > maxValue ? maxValue * 2 - doubleValue : doubleValue;
                                    doubleValue = doubleValue > maxValue ? doubleValue - (maxValue + 1) * 2 : doubleValue;

                                }

                                float factor = mapping.Factor == 0 ? 1 : mapping.Factor;
                                doubleValue = Math.Round(doubleValue * factor, mapping.Precision);
                                dataDictionary.Add(mapping.Name, doubleValue);
                            }
                        }
                        else
                        {
                            if (mapping.DataType == DataType.DateTime)
                            {
                                DateTime datetime = DateTimeHelper.Parse(value, DateTimeFormat.UK.DateTime);
                                if (siteInfo != null)
                                {
                                    if (siteInfo.TimeOffset != null)
                                    {
                                        datetime = datetime.AddHours(siteInfo.TimeOffset.Hours);
                                        datetime = datetime.AddMinutes(siteInfo.TimeOffset.Minutes);
                                    }
                                }
                                dataDictionary.Add(mapping.Name, datetime);
                            }
                            else if (mapping.DataType == DataType.Float || mapping.DataType == DataType.MicrochipFloat)
                            {
                                byte[] byteArray = ConversionHelper.ToByteArray(values.ToArray());
                                float floatValue;
                                if (mapping.DataType == DataType.Float)
                                {
                                    floatValue = ConversionHelper.ToFloat(byteArray);
                                }
                                else
                                {
                                    floatValue = ConversionHelper.ToMicrochipFloat(byteArray);
                                }
                                double val = Math.Round(floatValue, mapping.Precision);
                                dataDictionary.Add(mapping.Name, val);
                            }
                            else if (mapping.DataType == DataType.Binary)
                            {
                                /*------------------------------------------*/
                                BitwiseParameter(dataDictionary, mapping, value);
                                /*------------------------------------------*/
                            }
                            else
                            {
                                object val = DataTypeHelper.Convert(mapping.DataType, value);
                                dataDictionary.Add(mapping.Name, val);
                            }
                        }



                    }
                }
                catch (Exception ex)
                {
                    //return null;
                }
            }
            parsedPacket.Data.Add(dataDictionary);
            //ExecuteQuery(parsedPacket);
            return parsedPacket;
        }

        public void ExecuteQuery(ReonParsedPacket packet)
        {
            Console.WriteLine(JsonConvert.SerializeObject(packet, Formatting.Indented));
            //ParsedPacketQueryBuilder query = new ParsedPacketQueryBuilder(packet);
            //try
            //{
            //    //QueryExecutor.ExecuteWithLock(query.RealTimeQuery());
            //    //QueryExecutor.ExecuteWithLock(query.SiteQuery());
            //}
            //catch (Exception ex)
            //{
            //    LoggingManager.Log(ex);
            //}
        }

        private SiteInfo GetSiteInfo(string key)
        {
            if (SiteManager.Instance.Sites == null)
                return null;

            if (SiteManager.Instance.Sites.SiteList == null)
                return null;

            if (SiteManager.Instance.Sites.SiteList.ContainsKey(key))
            {
                return SiteManager.Instance.Sites.SiteList[key];
            }

            return null;
        }

        private Dictionary<string, object> BitwiseParameter(Dictionary<string, object> dictionary,
            Parameter mapping, string binary)
        {
            if (!string.IsNullOrEmpty(binary))
            {
                var bitwiseArray = ConversionHelper.ToIntArray(binary.ToCharArray());
                //for (int ii = 0; ii < bitwiseArray.Length; ii++)
                //{
                if (mapping.BitwiseLabels != null)
                {
                    if (mapping.BitwiseLabels.Count > 0)
                    {
                        foreach (BitwiseLabel label in mapping.BitwiseLabels)
                        {
                            dictionary.Add(label.Label, bitwiseArray[label.Index]);
                        }
                    }
                }
                //}
            }

            return dictionary;

        }
    }
}
