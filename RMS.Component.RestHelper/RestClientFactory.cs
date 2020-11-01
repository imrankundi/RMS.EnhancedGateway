using RestSharp;
using RestSharp.Authenticators;
using RMS.Component.RestHelper.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RMS.Component.RestHelper
{
    public class RestClientFactory
    {
        protected RestApiConfiguration configuration;
        public RestApi apiConfiguration;
        readonly IRestClient client;

        #region Constructors

        /// <summary>
        /// Initializes the RestClient based on the API Name in RestApiConfiguration.json
        /// </summary>
        /// <param name="baseUrl">API Name in the RestApiConfiguration.json</param>
        public RestClientFactory(string apiName)
        {
            try
            {
                this.configuration = RestApiConfigurationManager.Instance.Configurations;
                this.apiConfiguration = this.configuration.RestApis.FirstOrDefault(api => api.Name == apiName);

                client = new RestClient(this.apiConfiguration.BaseUrl);
            }
            catch (Exception ex)
            {
                //errHandler();
                #region Text Logging

                #endregion Text Logging

            }
        }

        /// <summary>
        /// Initializes the RestClient based on the API Name in RestApiConfiguration.json
        /// And Initializes the client with given AgentBaseUrl
        /// </summary>
        /// <param name="baseUrl">API Name in the RestApiConfiguration.json</param>
        public RestClientFactory(string ApiName, string AgentBaseUrl)
        {
            try
            {
                this.configuration = RestApiConfigurationManager.Instance.Configurations;
                this.apiConfiguration = this.configuration.RestApis.FirstOrDefault(api => api.Name == ApiName);

                client = new RestClient(AgentBaseUrl);
            }
            catch (Exception ex)
            {
                //errHandler();
                #region Text Logging

                #endregion Text Logging

            }
        }

        /// <summary>
        /// Initializes the RestClient Authenticator with provided username and password.
        /// </summary>
        /// <param name="baseUrl">API Name in the RestApiConfiguration.json</param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public RestClientFactory(string ApiName, string username, string password)
        {
            try
            {
                this.configuration = RestApiConfigurationManager.Instance.Configurations;
                this.apiConfiguration = this.configuration.RestApis.FirstOrDefault(api => api.Name == ApiName);

                client = new RestClient(this.apiConfiguration.BaseUrl);
                client.Authenticator = new HttpBasicAuthenticator(username, password);
            }
            catch (Exception ex)
            {
                //errHandler();
                #region Text Logging

                #endregion Text Logging

            }

        }

        #endregion

        #region Private Methods

        private TResponse Execute<TResponse>(RestRequest request, string token = null)
            where TResponse : new()
        {
            AddCommonHeaders(ref request);
            if (token != null)
                request.AddHeader("Authorization", "Bearer " + token);
            var response = client.Execute<TResponse>(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var ex = new Exception(message, response.ErrorException);
                #region Text Logging
                #endregion Text Logging
                //throw ex;
            }

            return response.Data;
        }

        private async Task<TResponse> ExecuteAsync<TResponse>(RestRequest request, string token = null)
            where TResponse : new()
        {
            AddCommonHeaders(ref request);
            if (token != null)
                request.AddHeader("Authorization", "Bearer " + token);
            var response = await client.ExecuteAsync<TResponse>(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var ex = new Exception(message, response.ErrorException);
                #region Text Logging
                #endregion Text Logging
                //throw ex;
            }

            return response.Data;
        }

        private void AddCommonHeaders(ref RestRequest request)
        {
            request.AddHeader("Content-Type", "application/json");
        }

        #endregion

        #region Public Methods Sync

        public TResponse GetCall<TResponse, TRequest>(string resource, TRequest RequestObject, string token = null)
            where TResponse : new()
            where TRequest : new()
        {
            var request = new RestRequest(resource, Method.GET);
            request.AddObject(RequestObject);

            return Execute<TResponse>(request, token);
        }

        public TResponse PostCall<TResponse, TRequest>(string resource, TRequest RequestObject, string token = null)
            where TResponse : new()
            where TRequest : new()
        {
            var request = new RestRequest(resource, Method.POST);
            request.AddJsonBody(RequestObject);

            return Execute<TResponse>(request, token);
        }

        #endregion

        #region Public Methods Async

        public async Task<TResponse> GetCallAsync<TResponse, TRequest>(string resource, TRequest RequestObject, string token = null)
            where TResponse : new()
            where TRequest : new()
        {
            var request = new RestRequest(resource, Method.GET);
            request.AddObject(RequestObject);

            return await ExecuteAsync<TResponse>(request, token);
        }

        public async Task<TResponse> PostCallAsync<TResponse, TRequest>(string resource, TRequest RequestObject, string token = null)
            where TResponse : new()
            where TRequest : new()
        {
            var request = new RestRequest(resource, Method.POST);
            request.AddJsonBody(RequestObject);

            return await ExecuteAsync<TResponse>(request, token);
        }

        #endregion

        #region Public Factory Methods
        // to do
        public static RestClient CreateRestClient(string url)
        {
            return new RestClient(url);
        }

        public static RestRequest CreateRequest(string source, Method method)
        {
            return new RestRequest(source, method);
        }

        public static RestRequest CreateJsonPostRequest(string resource, object obj)
        {
            var request = new RestRequest(resource, Method.POST, DataFormat.Json);
            request.AddJsonBody(obj);
            return request;
        }

        #endregion
    }
}
