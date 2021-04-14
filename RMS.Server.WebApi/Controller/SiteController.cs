using RMS.Component.WebApi.Requests;
using RMS.Component.WebApi.Responses;
using RMS.Core.Common;
using RMS.Protocols.GT;
using RMS.Server.DataTypes;
using RMS.Server.DataTypes.Requests;
using RMS.Server.DataTypes.Responses;
using RMS.Server.WebApi.Common;
using RMS.Server.WebApi.Configuration;
using RMS.Server.WebApi.Jwt.Filters;
using System;
using System.Threading;
using System.Web.Http;

namespace RMS.Server.WebApi.Controller
{

    public class SiteController : ApiController
    {
        // POST api/demo 
        public BaseResponse Post(BaseRequest request)
        {
            return null;
        }

        [HttpPost]
        [JwtAuthentication]
        public TerminalCommandResponse Command(TerminalCommandRequest request)
        {
            var config = WebApiServerConfigurationManager.Instance.Configurations;
            try
            {
                if (request.RequestType != GatewayRequestType.TerminalCommand)
                {
                    return new TerminalCommandResponse
                    {
                        RequestType = request.RequestType,
                        RequestId = request.RequestId,
                        Message = "Invalid Request Type",
                        ResponseStatus = ResponseStatus.Failed,
                        TerminalId = request.TerminalId
                    };
                }
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
                                Message = "Response Received from Device",
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
        [JwtAuthentication]
        public TerminalCommandResponse GetConfiguration(GTGetConfigurationRequest request)
        {
            var config = WebApiServerConfigurationManager.Instance.Configurations;
            TerminalCommandRequest commandRequest = new TerminalCommandRequest();
            commandRequest.TerminalId = request.TerminalId;
            try
            {
                if (request.RequestType != GatewayRequestType.TerminalCommand)
                {
                    return new TerminalCommandResponse
                    {
                        RequestType = request.RequestType,
                        RequestId = request.RequestId,
                        Message = "Invalid Request Type",
                        ResponseStatus = ResponseStatus.Failed,
                        TerminalId = request.TerminalId
                    };
                }

                if (request.CommandType == GTCommandType.GeneralSettings ||
                    request.CommandType == GTCommandType.ExtendedConfigurationSettings ||
                    request.CommandType == GTCommandType.PollingAndGprsSettings ||
                    request.CommandType == GTCommandType.SimAndServerSettings ||
                    request.CommandType == GTCommandType.WatchdogSettings)
                {
                    commandRequest.Data = GTCommandFactory.CreateGetCommand(request.TerminalId, request.CommandType);
                    var response = TerminalRequestHandler.SendGTCommandRequest(commandRequest, request.CommandType);
                    response.RequestId = request.RequestId;
                    return response;
                }
                else
                {
                    return new TerminalCommandResponse
                    {
                        RequestType = request.RequestType,
                        RequestId = request.RequestId,
                        Message = "Invalid Request Type",
                        ResponseStatus = ResponseStatus.Failed,
                        TerminalId = request.TerminalId
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
        }

        [HttpPost]
        [JwtAuthentication]
        public TerminalCommandResponse SetConfiguration(GTSetConfigurationRequest request)
        {
            var config = WebApiServerConfigurationManager.Instance.Configurations;
            TerminalCommandRequest commandRequest = new TerminalCommandRequest();
            commandRequest.TerminalId = request.TerminalId;
            try
            {



                if (request.RequestType != GatewayRequestType.TerminalCommand)
                {
                    return new TerminalCommandResponse
                    {
                        RequestType = request.RequestType,
                        RequestId = request.RequestId,
                        Message = "Invalid Request Type",
                        ResponseStatus = ResponseStatus.Failed,
                        TerminalId = request.TerminalId
                    };
                }

                if (request.CommandType == GTCommandType.GeneralSettings ||
                    request.CommandType == GTCommandType.ExtendedConfigurationSettings ||
                    request.CommandType == GTCommandType.PollingAndGprsSettings ||
                    request.CommandType == GTCommandType.Reset ||
                    request.CommandType == GTCommandType.ResetRom ||
                    request.CommandType == GTCommandType.SimAndServerSettings ||
                    request.CommandType == GTCommandType.WatchdogSettings)
                {
                    commandRequest.Data = GTCommandFactory.CreateSetCommand(request.TerminalId, request.Data, request.CommandType);
                    var response = TerminalRequestHandler.SendGTCommandRequest(commandRequest, request.CommandType);
                    response.RequestId = request.RequestId;
                    return response;
                }
                else
                {
                    return new TerminalCommandResponse
                    {
                        RequestType = request.RequestType,
                        RequestId = request.RequestId,
                        Message = "Invalid Request Type",
                        ResponseStatus = ResponseStatus.Failed,
                        TerminalId = request.TerminalId
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
        }

        [HttpPost]
        [JwtAuthentication]
        public TerminalCommandResponse AddMultipleModbusDevices(GTSetConfigurationRequest request)
        {
            var config = WebApiServerConfigurationManager.Instance.Configurations;
            TerminalCommandRequest commandRequest = new TerminalCommandRequest();
            commandRequest.TerminalId = request.TerminalId;
            try
            {
                if (!(request.RequestType == GatewayRequestType.TerminalCommand &&
                    request.CommandType == GTCommandType.AddMultipleModbusDevices))
                {
                    return new TerminalCommandResponse
                    {
                        RequestType = request.RequestType,
                        RequestId = request.RequestId,
                        Message = "Invalid Request Type",
                        ResponseStatus = ResponseStatus.Failed,
                        TerminalId = request.TerminalId
                    };
                }

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

        [HttpPost]
        [JwtAuthentication]
        public TerminalCommandResponse AddModbusDevice(GTSetConfigurationRequest request)
        {
            var config = WebApiServerConfigurationManager.Instance.Configurations;
            TerminalCommandRequest commandRequest = new TerminalCommandRequest();
            commandRequest.TerminalId = request.TerminalId;
            try
            {
                if (!(request.RequestType == GatewayRequestType.TerminalCommand &&
                    request.CommandType == GTCommandType.AddModbusDevice))
                {
                    return new TerminalCommandResponse
                    {
                        RequestType = request.RequestType,
                        RequestId = request.RequestId,
                        Message = "Invalid Request Type",
                        ResponseStatus = ResponseStatus.Failed,
                        TerminalId = request.TerminalId
                    };
                }
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

        [HttpPost]
        [JwtAuthentication]
        public TerminalCommandResponse GetModbusDevice(GTGetModbusDeviceRequest request)
        {
            var config = WebApiServerConfigurationManager.Instance.Configurations;
            TerminalCommandRequest commandRequest = new TerminalCommandRequest();
            commandRequest.TerminalId = request.TerminalId;
            try
            {
                if (request.CommandType == GTCommandType.GetModbusDevice)
                {
                    commandRequest.Data = GTCommandFactory.CreateGetModbusDeviceCommand(request.TerminalId, request.StartIndex, 1);
                    var response = TerminalRequestHandler.SendGTCommandRequest(commandRequest, request.CommandType);
                    return response;
                }
                else if (request.CommandType == GTCommandType.GetMultipleModbusDevices)
                {
                    commandRequest.Data = GTCommandFactory.CreateGetModbusDeviceCommand(request.TerminalId, request.StartIndex, request.NumberOfDevices);
                    var response = TerminalRequestHandler.SendGTCommandRequest(commandRequest, request.CommandType);
                    return response;
                }
                else
                {
                    return new TerminalCommandResponse
                    {
                        RequestId = commandRequest.RequestId,
                        Data = null,
                        RequestType = commandRequest.RequestType,
                        ResponseStatus = ResponseStatus.Failed,
                        Message = "Invalid Command Request"
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
        }
        [HttpPost]
        [JwtAuthentication]
        public TerminalCommandResponse ClearAllModbusDevices(GTClearModbusDevicesConfigurationRequest request)
        {
            var config = WebApiServerConfigurationManager.Instance.Configurations;
            TerminalCommandRequest commandRequest = new TerminalCommandRequest();
            commandRequest.TerminalId = request.TerminalId;
            try
            {
                if (!(request.RequestType == GatewayRequestType.TerminalCommand &&
                    request.CommandType == GTCommandType.ClearAllModbusDevices))
                {
                    return new TerminalCommandResponse
                    {
                        RequestType = request.RequestType,
                        RequestId = request.RequestId,
                        Message = "Invalid Request Type",
                        ResponseStatus = ResponseStatus.Failed,
                        TerminalId = request.TerminalId
                    };
                }

                commandRequest.Data = GTCommandFactory.CreateClearModbusDevicesCommand(request.TerminalId);
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
