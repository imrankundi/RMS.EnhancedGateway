using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;
using RMS.AWS.Logging;
using RMS.Component.DataAccess.SQLite.Entities;
using RMS.Core.Common;
using System;
using System.Net;
using System.Threading.Tasks;

namespace RMS.AWS
{
    public class AwsSqsClient : IHttpClient
    {
        public bool LogPacketOnSuccess { get; set; }
        public bool LogPacketOnFailure { get; set; }
        private ServerInfo server;
        public AwsSqsClient(ServerInfo server)
        {
            this.server = server;
        }

        public async Task<bool> PostData(string messageBody, int httpTimeout = 30)
        {
            DateTime timeStamp = DateTime.UtcNow;
            bool result = false;
            if (messageBody != null)
            {
                try
                {
                    var accessKey = server.AccessKey;
                    var secretKey = server.SecretKey;
                    var auth = new BasicAWSCredentials(accessKey, secretKey);
                    var sqsConfig = new AmazonSQSConfig();
                    //sqsConfig.ServiceURL = "https://sqs.ap-southeast-1.amazonaws.com";
                    sqsConfig.RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(server.Region);

                    using (var client = new AmazonSQSClient(auth, sqsConfig))
                    {
                        var request = new SendMessageRequest
                        {
                            DelaySeconds = (int)TimeSpan.FromSeconds(1).TotalSeconds,
                            MessageBody = messageBody, // make it stringify
                            QueueUrl = server.EndPointUri

                        };

                        var response = await client.SendMessageAsync(request);
                        var content = JsonConvert.SerializeObject(response);

                        if (response.HttpStatusCode == HttpStatusCode.OK)
                        {
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }

                        if (!result)
                        {
                            if (LogPacketOnFailure)
                            {
                                PushApiEntity entity = new PushApiEntity
                                {
                                    Timestamp = DateTimeHelper.CurrentUniversalTime,
                                    Request = messageBody,
                                    ServerId = server.Id,
                                    Response = content,
                                    HttpStatusCode = response.HttpStatusCode
                                };
                                //PushApiRepository.Save(entity);
                                FailedPacketLogger.Instance.Log.Write(JsonConvert.SerializeObject(entity));
                            }

                        }
                        else
                        {
                            if (LogPacketOnSuccess)
                            {
                                PushApiEntity entity = new PushApiEntity
                                {
                                    Timestamp = DateTimeHelper.CurrentUniversalTime,
                                    Request = messageBody,
                                    ServerId = server.Id,
                                    Response = content,
                                    HttpStatusCode = response.HttpStatusCode
                                };
                                //PushApiRepository.Save(entity);
                                Logger.Instance.Log.Write(JsonConvert.SerializeObject(entity));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                    if (LogPacketOnFailure)
                    {
                        PushApiEntity entity = new PushApiEntity
                        {
                            Timestamp = DateTimeHelper.CurrentUniversalTime,
                            Request = messageBody,
                            ServerId = server.Id,
                            HttpStatusCode = HttpStatusCode.Unused,
                            Response = "[ERROR_AT_GATEWAY] => " + ex.Message
                        };

                        FailedPacketLogger.Instance.Log.Write(JsonConvert.SerializeObject(entity));
                    }
                }
            }

            return result;
        }

        bool IHttpClient.IsValid()
        {
            return !(string.IsNullOrWhiteSpace(server.AccessKey)
                || string.IsNullOrWhiteSpace(server.SecretKey)
                || string.IsNullOrWhiteSpace(server.Region)
                || string.IsNullOrWhiteSpace(server.Service)
                || string.IsNullOrWhiteSpace(server.XApiKey)
                || string.IsNullOrWhiteSpace(server.EndPointUri));
        }
    }
}