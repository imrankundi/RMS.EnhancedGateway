﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace RMS.AWS
{
    public class AWS4Client
    {
        public bool SaveResponseOnSuccess { get; set; }
        public bool SaveResponseOnFailure { get; set; }
        private ServerInfo server;
        public AWS4Client(ServerInfo server)
        {
            this.server = server;
        }

        public bool PostData(string messageBody, int httpTimeout = 30)
        {
            DateTime timeStamp = DateTime.UtcNow;
            bool result = false;
            if (messageBody != null)
            {
                try
                {
                    string messageBodyHash = ComputeSha256Hash(messageBody);

                    Uri targetUri = new Uri(server.EndPointUri);
                    HttpMethod httpMethod = HttpMethod.Post;

                    HttpContent httpContent = new StringContent(messageBody);
                    httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                    HttpRequestMessage httpRequestMessage = new HttpRequestMessage(httpMethod, targetUri);
                    httpRequestMessage.Content = httpContent;
                    httpRequestMessage.Headers.Add("x-api-key", server.XApiKey);
                    httpRequestMessage.Headers.Add("X-Amz-Content-Sha256", messageBodyHash);
                    httpRequestMessage.Headers.Add("X-Amz-Date", timeStamp.ToString("yyyyMMddTHHmmssZ"));
                    httpRequestMessage.Headers.Host = targetUri.Host;

                    string signature = GetSignature(httpRequestMessage, server.SecretKey, server.Region, server.Service, timeStamp);
                    string authorizationHeader = GetAuthorizationHeader(server.AccessKey, timeStamp, server.Region, server.Service, signature);

                    using (HttpClient httpClient = new HttpClient())
                    {
                        httpClient.Timeout = TimeSpan.FromSeconds(httpTimeout);
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("AWS4-HMAC-SHA256", authorizationHeader);

                        HttpResponseMessage msg = httpClient.SendAsync(httpRequestMessage).Result;
                        string content = msg.Content.ReadAsStringAsync().Result;

                        //Log?.Verbose(JsonConvert.SerializeObject(entity));
                        if (msg.StatusCode == HttpStatusCode.OK)
                        {
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                        Console.WriteLine(content);
                    }
                }
                catch (Exception ex)
                {
                    //Logger.Instance.Log.Write(server.Id.ToString(), MessageBody);
                    //ErrorLogger.GetInstance().LogExceptionAsync(ex, "Posting data to server from AWS4Client");
                    result = false;

                }
            }

            return result;
        }

        static byte[] HmacSHA256(string data, byte[] key)
        {
            string algorithm = "HmacSHA256";
            KeyedHashAlgorithm kha = KeyedHashAlgorithm.Create(algorithm);
            kha.Key = key;

            return kha.ComputeHash(Encoding.UTF8.GetBytes(data));
        }

        static byte[] GetSignatureKey(string key, DateTime dateStamp, string regionName, string serviceName)
        {
            byte[] kSecret = Encoding.UTF8.GetBytes(("AWS4" + key).ToCharArray());
            byte[] kDate = HmacSHA256(dateStamp.ToString("yyyyMMdd"), kSecret);
            byte[] kRegion = HmacSHA256(regionName, kDate);
            byte[] kService = HmacSHA256(serviceName, kRegion);
            byte[] kSigning = HmacSHA256("aws4_request", kService);

            return kSigning;
        }

        static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                return string.Join("", bytes.Select(x => x.ToString("x2")));
            }
        }

        private string GetAuthorizationHeader(string accessKey, DateTime timeStamp, string region, string service, string signature)
        {
            StringBuilder authorizationHeader = new StringBuilder();

            authorizationHeader.Append("Credential=");
            authorizationHeader.Append(accessKey);
            authorizationHeader.Append("/");
            authorizationHeader.Append(timeStamp.ToString("yyyyMMdd"));
            authorizationHeader.Append("/");
            authorizationHeader.Append(region);
            authorizationHeader.Append("/");
            authorizationHeader.Append(service);
            authorizationHeader.Append("/aws4_request, ");
            authorizationHeader.Append("SignedHeaders=content-type;host;x-amz-content-sha256;x-amz-date;x-api-key, Signature=");
            authorizationHeader.Append(signature);

            return authorizationHeader.ToString();
        }

        private string GetCanonicalRequest(HttpRequestMessage msg)
        {
            StringBuilder canonicalRequest = new StringBuilder();

            canonicalRequest.Append(msg.Method.ToString());
            canonicalRequest.Append("\n");

            canonicalRequest.Append(msg.RequestUri.AbsolutePath);
            canonicalRequest.Append("\n");

            canonicalRequest.Append(""); //blank query string for post
            canonicalRequest.Append("\n");

            canonicalRequest.Append("content-type:");
            canonicalRequest.Append(msg.Content.Headers.ContentType.ToString());
            canonicalRequest.Append("\n");

            canonicalRequest.Append("host:");
            canonicalRequest.Append(msg.Headers.Host.ToString());
            canonicalRequest.Append("\n");

            canonicalRequest.Append("X-Amz-Content-Sha256:".ToLower());
            canonicalRequest.Append(msg.Headers.Where(x => x.Key == "X-Amz-Content-Sha256").ElementAt(0).Value.ElementAt(0));
            canonicalRequest.Append("\n");

            canonicalRequest.Append("X-Amz-Date:".ToLower());
            canonicalRequest.Append(msg.Headers.Where(x => x.Key == "X-Amz-Date").ElementAt(0).Value.ElementAt(0));
            canonicalRequest.Append("\n");

            canonicalRequest.Append("x-api-key:".ToLower());
            canonicalRequest.Append(msg.Headers.Where(x => x.Key == "x-api-key").ElementAt(0).Value.ElementAt(0));
            canonicalRequest.Append("\n");

            canonicalRequest.Append("\n");

            canonicalRequest.Append("content-type;host;x-amz-content-sha256;x-amz-date;x-api-key");
            canonicalRequest.Append("\n");

            canonicalRequest.Append(msg.Headers.Where(x => x.Key == "X-Amz-Content-Sha256").ElementAt(0).Value.ElementAt(0));

            return canonicalRequest.ToString();
        }

        private string GetStringToSign(DateTime timeStamp, string region, string service, string canonicalRequest)
        {
            StringBuilder stringToSign = new StringBuilder();

            stringToSign.Append("AWS4-HMAC-SHA256");
            stringToSign.Append("\n");
            stringToSign.Append(timeStamp.ToString("yyyyMMddTHHmmssZ"));
            stringToSign.Append("\n");
            stringToSign.Append(timeStamp.ToString("yyyyMMdd") + "/" + region + "/" + service + "/aws4_request");
            stringToSign.Append("\n");
            stringToSign.Append(ComputeSha256Hash(canonicalRequest));

            return stringToSign.ToString();
        }

        private string GetSignature(HttpRequestMessage msg, string SecretKey, string region, string service, DateTime timeStamp)
        {
            string CanonicalRequest = GetCanonicalRequest(msg);
            string StringToSign = GetStringToSign(timeStamp, region, service, CanonicalRequest);

            byte[] signKey = GetSignatureKey(SecretKey, timeStamp, region, service);
            byte[] signature = HmacSHA256(StringToSign, signKey);

            return string.Join("", signature.Select(x => x.ToString("x2")));
        }

    }
}