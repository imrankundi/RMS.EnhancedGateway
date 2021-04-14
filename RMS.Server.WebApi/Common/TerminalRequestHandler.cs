using RMS.Component.WebApi.Responses;
using RMS.Core.Common;
using RMS.Parser;
using RMS.Protocols.GT;
using RMS.Server.DataTypes;
using RMS.Server.DataTypes.Requests;
using RMS.Server.DataTypes.Responses;
using RMS.Server.WebApi.Configuration;
using System;
using System.Linq;
using System.Threading;

namespace RMS.Server.WebApi.Common
{
    public class TerminalRequestHandler
    {
        public static TerminalCommandResponse SendGTCommandRequest(TerminalCommandRequest commandRequest,
            GTCommandType commandType)
        {
            var config = WebApiServerConfigurationManager.Instance.Configurations;
            TerminalCommandResponse response = new TerminalCommandResponse();
            try
            {
                var connected = WebServer.server.ChannelKeys.Contains(commandRequest.TerminalId);
                if (!connected)
                {
                    return new TerminalCommandResponse
                    {
                        RequestId = commandRequest.RequestId,
                        Data = null,//commandRequest.Data,
                        RequestType = commandRequest.RequestType,
                        ResponseStatus = ResponseStatus.Failed,
                        Message = "Site not connected."
                    };
                }
                var cmd = TerminalCommandHandler.Instance.Find(commandRequest.TerminalId);
                if (cmd != null)
                {
                    return new TerminalCommandResponse
                    {
                        RequestId = commandRequest.RequestId,
                        Data = commandRequest.Data,
                        RequestType = commandRequest.RequestType,
                        ResponseStatus = ResponseStatus.Failed,
                        Message = "Another configuration already in process. Please try again later"
                    };
                }
                TerminalCommandHandler.Instance.Add(new TerminalCommand
                {
                    RequestData = commandRequest.Data,
                    TerminalId = commandRequest.TerminalId,
                    RequestReceivedOn = DateTimeHelper.CurrentUniversalTime,
                    Status = CommandStatus.RequestReceived
                });
                WebServer.server.Notify(commandRequest);

                int retries = config.TerminalCommandRetries;
                while (retries > 0)
                {
                    retries--;
                    try
                    {
                        var command = TerminalCommandHandler.Instance.Find(commandRequest.TerminalId);
                        if (command.Status == CommandStatus.ResponseReceived)
                        {
                            TerminalCommandHandler.Instance.Remove(commandRequest.TerminalId);
                            var data = command.ResponseData;
                            ICGRC gtConfig = null;
                            if (!string.IsNullOrEmpty(data))
                            {
                                var packet = ParsingManager.FirstLevelParser(data);
                                gtConfig = GTCommandFactory.GetConfiguration(packet, commandType);
                            }
                            return new TerminalCommandResponse
                            {
                                RequestId = commandRequest.RequestId,
                                Data = gtConfig,
                                RequestType = commandRequest.RequestType,
                                ResponseStatus = ResponseStatus.Success,
                                Message = "Response Received",
                                TerminalId = command.TerminalId
                            };
                        }

                    }
                    catch (Exception ex)
                    {
                        return new TerminalCommandResponse
                        {
                            RequestId = commandRequest.RequestId,
                            Data = null,
                            RequestType = commandRequest.RequestType,
                            ResponseStatus = ResponseStatus.Failed,
                            Message = ex.Message
                        };
                    }
                    Thread.Sleep(config.TerminalCommandRetryIntervalInSeconds * 1000);
                }

                TerminalCommandHandler.Instance.Remove(commandRequest.TerminalId);
                return new TerminalCommandResponse
                {
                    RequestId = commandRequest.RequestId,
                    Data = null,
                    RequestType = commandRequest.RequestType,
                    ResponseStatus = ResponseStatus.Failed,
                    Message = "Request Timed Out",
                    TerminalId = commandRequest.TerminalId
                };

            }
            catch (Exception ex)
            {
                return new TerminalCommandResponse
                {
                    RequestId = commandRequest.RequestId,
                    Data = null,
                    RequestType = commandRequest.RequestType,
                    ResponseStatus = ResponseStatus.Failed,
                    Message = ex.Message
                };
            }
        }
    }
}
