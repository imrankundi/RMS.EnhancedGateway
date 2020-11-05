using LoreSoft.MathExpressions;
using RMS.Core.Common;
using RMS.Core.Enumerations;
using RMS.Core.Logging;
using RMS.Core.QueryBuilder;
using System;
using System.Collections.Generic;

namespace RMS.Parser
{
    public class DeviceParser
    {
        private const char splitter = ',';

        public ReceivedPacket Packet { get; }


        public DeviceParser(ReceivedPacket packet)
        {
            Packet = packet;
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

            ReceivedPacketQuery query = new ReceivedPacketQuery()
            {
                TerminalId = Packet.TerminalId,
                ProtocolHeader = Packet.ProtocolHeader,
                ReceivedOn = DateTimeHelper.CurrentUniversalTime
            };
            if (protocol.PageNumberIndex != -1)
            {
                if (array.Length > protocol.PageNumberIndex)
                {
                    query.PageNumber = int.Parse(array[protocol.PageNumberIndex]);
                }
            }

            Page page = protocol.FindPage(query.PageNumber);
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

                    string value = string.Format(mapping.Expression, values.ToArray());



                    if (mapping.ParameterType == ParameterType.Id)
                    {
                        query.Id = value;
                    }
                    else if (mapping.DataType == DataType.DateTime)
                    {
                        DateTime datetime = DateTimeHelper.Parse(value, DateTimeFormat.UK.DateTime);
                        query.Fields.Add(new Field(mapping.Field, datetime));
                    }
                    else if (mapping.Evaluate)
                    {
                        MathEvaluator evaluator = new MathEvaluator();
                        double val = Math.Round(evaluator.Evaluate(value), mapping.Precision);

                        query.Fields.Add(new Field(mapping.Field, val));
                        query.AddField(new Field(mapping.Field, val));
                    }
                    else
                    {
                        if (mapping.EnableDataType &&
                            (mapping.DataType == DataType.Integer ||
                            mapping.DataType == DataType.Double))
                        {
                            if (!ConversionHelper.IsNumeric(value))
                            {
                                // do stuff here
                            }
                        }
                        //query.Fields.Add(new Field(mapping.Field, value));
                        query.AddField(new Field(mapping.Field, value));
                    }
                }
                catch (Exception ex)
                {
                    LoggingManager.Log(ex);
                    return false;
                }
            }

            try
            {
                //QueryExecutor.ExecuteWithLock(query.RealTimeQuery());
                //QueryExecutor.Execute(query.SiteQuery());
            }
            catch (Exception ex)
            {
                LoggingManager.Log(ex);
                return false;
            }

            try
            {
                //QueryExecutor.Execute(query.SiteQuery());
            }
            catch (Exception ex)
            {
                LoggingManager.Log(ex);
                return false;
            }

            return true;
        }
    }
}
