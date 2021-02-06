using Newtonsoft.Json;
using RMS.Component.WebApi.Requests;
using RMS.Component.WebApi.Responses;
using RMS.Core.Common;
using RMS.Parser;
using RMS.Protocols;
using RMS.Server.DataTypes;
using RMS.Server.DataTypes.Requests;
using RMS.Server.DataTypes.Responses;
using RMS.Server.WebApi.Common;
using RMS.Server.WebApi.Configuration;
using System;
using System.Threading;
using System.Web.Http;
using System.Linq;
using RMS.Server.WebApi.Jwt.Filters;

namespace RMS.Server.WebApi.Controller
{

    public class GatewayController : ApiController
    {
        // POST api/demo 
        public BaseResponse Post(BaseRequest request)
        {
            return null;
        }
        [HttpGet]
        [AllowAnonymous]
        public BaseResponse GetSiteCount()
        {
            try
            {
                var count = WebServer.server.ChannelKeys.Count();
                Console.WriteLine(count);
                return new BaseResponse
                {
                    RequestId = DateTime.UtcNow.ToString("yyyyMMddHHmmssffffff"),
                    Message = string.Format("Connected Sites Count: {0}", count),
                    ResponseStatus = ResponseStatus.Success
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    RequestId = DateTime.UtcNow.ToString("yyyyMMddHHmmssffffff"),
                    Message = ex.Message,
                    ResponseStatus = ResponseStatus.Failed
                };
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public GetSitesResponse GetSites()
        {
            try
            {
                return new GetSitesResponse
                {
                    RequestId = DateTime.UtcNow.ToString("yyyyMMddHHmmssffffff"),
                    Sites = WebServer.server.ChannelKeys,
                    ResponseStatus = ResponseStatus.Success
                };
            }
            catch (Exception ex)
            {
                return new GetSitesResponse
                {
                    RequestId = DateTime.UtcNow.ToString("yyyyMMddHHmmssffffff"),
                    Message = ex.Message,
                    ResponseStatus = ResponseStatus.Failed
                };
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public BaseResponse ReloadProtocols()
        {
            try
            {
                ProtocolList.Instance.Reload();
                Console.WriteLine(JsonConvert.SerializeObject(ProtocolList.Instance.Protocols.Keys));
                return new BaseResponse
                {
                    RequestId = DateTime.UtcNow.ToString("yyyyMMddHHmmssffffff"),
                    Message = "Protocols are succesfully reloaded",
                    ResponseStatus = ResponseStatus.Success
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    RequestId = DateTime.UtcNow.ToString("yyyyMMddHHmmssffffff"),
                    Message = "Unable to reloaded Protocols [" + ex.Message + "]",
                    ResponseStatus = ResponseStatus.Failed
                };
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public BaseResponse ReloadSites()
        {
            try
            {
                SiteManager.Instance.Reload();
                Console.WriteLine(JsonConvert.SerializeObject(SiteManager.Instance.Sites, Formatting.Indented));
                return new BaseResponse
                {
                    RequestId = DateTime.UtcNow.ToString("yyyyMMddHHmmssffffff"),
                    Message = "Sites are succesfully reloaded",
                    ResponseStatus = ResponseStatus.Success
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    RequestId = DateTime.UtcNow.ToString("yyyyMMddHHmmssffffff"),
                    Message = "Unable to reloaded Sites [" + ex.Message + "]",
                    ResponseStatus = ResponseStatus.Failed
                };
            }
        }
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
