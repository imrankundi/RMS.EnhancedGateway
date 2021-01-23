using Newtonsoft.Json;
using RMS.Component.WebApi.Requests;
using RMS.Component.WebApi.Responses;
using RMS.Core.Common;
using RMS.Protocols;
using RMS.Protocols.GT;
using RMS.Server.DataTypes;
using RMS.Server.DataTypes.Requests;
using RMS.Server.DataTypes.Responses;
using RMS.Server.WebApi.Common;
using RMS.Server.WebApi.Configuration;
using System;
using System.Threading;
using System.Web.Http;

namespace RMS.Server.WebApi.Controller
{

    public class GTConfigurationController : ApiController
    {
        // POST api/demo 
        public BaseResponse Post(BaseRequest request)
        {
            return null;
        }
        
        [HttpPost]
        public TerminalCommandResponse Command(TerminalCommandRequest request)
        {
            var config = WebApiServerConfigurationManager.Instance.Configurations;
            try
            {
                var cmd = TerminalCommandHandler.Instance.Find(request.TerminalId);
                if (cmd != null)
                {
                    return new TerminalCommandResponse
                    {
                        RequestId = request.RequestId,
                        Data = null,
                        RequestType = request.RequestType,
                        ResponseStatus = ResponseStatus.Failed,
                        Message = "Another configuration already in process. Please try again later"
                    };
                }
                TerminalCommandHandler.Instance.Add(new TerminalCommand
                {
                    RequestData = request.Data,
                    TerminalId = request.TerminalId,
                    RequestReceivedOn = DateTimeHelper.CurrentUniversalTime,
                    Status = CommandStatus.RequestReceived
                });
                WebServer.server.Notify(request);

                int retries = config.TerminalCommandRetries;
                while (retries > 0)
                {
                    retries--;
                    try
                    {
                        var command = TerminalCommandHandler.Instance.Find(request.TerminalId);
                        if (command.Status == CommandStatus.ResponseReceived)
                        {
                            TerminalCommandHandler.Instance.Remove(request.TerminalId);
                            return new TerminalCommandResponse
                            {
                                RequestId = request.RequestId,
                                Data = command.ResponseData,
                                RequestType = request.RequestType,
                                ResponseStatus = ResponseStatus.Success,
                                Message = "Configuration Successful",
                                TerminalId = command.TerminalId
                            };
                        }

                    }
                    catch (Exception ex)
                    {
                        return new TerminalCommandResponse
                        {
                            RequestId = request.RequestId,
                            Data = null,
                            RequestType = request.RequestType,
                            ResponseStatus = ResponseStatus.Failed,
                            Message = ex.Message
                        };
                    }
                    Thread.Sleep(config.TerminalCommandRetryIntervalInSeconds * 1000);
                }

                TerminalCommandHandler.Instance.Remove(request.TerminalId);
                return new TerminalCommandResponse
                {
                    RequestId = request.RequestId,
                    Data = null,
                    RequestType = request.RequestType,
                    ResponseStatus = ResponseStatus.Failed,
                    Message = "Request Timed Out",
                    TerminalId = request.TerminalId
                };
            }
            catch (Exception ex)
            {
                return new TerminalCommandResponse
                {
                    RequestId = request.RequestId,
                    Data = null,
                    RequestType = request.RequestType,
                    ResponseStatus = ResponseStatus.Failed,
                    Message = ex.Message
                };
            }
        }
        [HttpPost]
        public TerminalCommandResponse GetConfiguration(GTGetConfigurationRequest request)
        {
            var config = WebApiServerConfigurationManager.Instance.Configurations;
            TerminalCommandRequest commandRequest = new TerminalCommandRequest();
            commandRequest.TerminalId = request.TerminalId;
            try
            {
                commandRequest.Data = GTCommandFactory.CreateGetCommand(request.TerminalId, request.CommandType);
                return TerminalRequestHandler.SendGTCommandRequest(commandRequest, request.CommandType);
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
        [HttpPost]
        public TerminalCommandResponse SetConfiguration(GTSetConfigurationRequest request)
        {
            var config = WebApiServerConfigurationManager.Instance.Configurations;
            TerminalCommandRequest commandRequest = new TerminalCommandRequest();
            commandRequest.TerminalId = request.TerminalId;
            try
            {
                commandRequest.Data = GTCommandFactory.CreateSetCommand(request.TerminalId, request.Data, request.CommandType);
                return TerminalRequestHandler.SendGTCommandRequest(commandRequest, request.CommandType);
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
