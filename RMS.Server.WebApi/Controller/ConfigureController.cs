using Newtonsoft.Json;
using RMS.Component.WebApi.Requests;
using RMS.Component.WebApi.Responses;
using RMS.Core.Common;
using RMS.Protocols;
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

    public class ConfigureController : ApiController
    {
        // POST api/demo 
        public BaseResponse Post(BaseRequest request)
        {
            return null;
        }

        [HttpGet]
        public BaseResponse ReloadProtocols()
        {
            try
            {
                ProtocolList.Instance.Reload();
                Console.WriteLine(JsonConvert.SerializeObject(ProtocolList.Instance.Protocols.Keys));
                return new BaseResponse
                {
                    Message = "Protocols are succesfully reloaded",
                    ResponseStatus = ResponseStatus.Success
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Message = "Unable to reloaded Protocols [" + ex.Message + "]",
                    ResponseStatus = ResponseStatus.Failed
                };
            }
        }
        [HttpGet]
        public ConfigurationResponse GetParameters()
        {
            try
            {
                return new ConfigurationResponse
                {
                    Data = ConfigManager.GetConfigProtocol("CGRC"),
                    Message = "",
                    ResponseStatus = ResponseStatus.Success
                };
            }
            catch (Exception ex)
            {
                return new ConfigurationResponse
                {
                    Message = "Unable to reloaded Sites [" + ex.Message + "]",
                    ResponseStatus = ResponseStatus.Failed
                };
            }
        }

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

                //var configurations = WebApiServerConfigurationManager.Instance.Configurations;
                //IOwinContext owinContext = Request.GetOwinContext();
                //string clientIp = owinContext.Environment["server.RemoteIpAddress"] as string;

                //return RequestHandler.HandleRequest(jsonObject, new CommunicationContext { IP = clientIp });

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

    }
}
