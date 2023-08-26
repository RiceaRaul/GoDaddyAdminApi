using ExternalServices.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace ExternalServices.Implementations
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HttpClientWrapper> _logger;
        public HttpClientWrapper(ILogger<HttpClientWrapper>  logger)
        {
            _httpClient = new HttpClient();
            _logger = logger;
        }


        private static HttpRequestMessage GenerateRequest<REModel>(string url, REModel payload, HttpMethod method)
        {
            HttpRequestMessage sendRequest = new(method, url);
            
            var payloadType = typeof(REModel);

            var content = payloadType == typeof(string) ? payload!.ToString() : JsonConvert.SerializeObject(payload);
            var requestType = payloadType == typeof(string) ? "application/xml" : "application/json";
            var stringContent = new StringContent(content!,Encoding.UTF8, requestType);
            sendRequest.Content = stringContent;

            return sendRequest;
        }

        private static HttpRequestMessage GenerateRequest(string url, HttpMethod method)
        {
            HttpRequestMessage sendRequest = new(method, url);

            return sendRequest;
        }

        public void addHeader(string key,string value)
        {
            _httpClient.DefaultRequestHeaders.Add(key, value);
        }

        public void setBaseUrl(string url)
        {
            _httpClient.BaseAddress = new Uri(url);
        }

        public async Task<RModel> PerformAction<REModel, RModel>(string url, REModel payload, HttpMethod method) where RModel : new()
        {
            var reqeuest = GenerateRequest<REModel>(url, payload, method);
            try
            {
                using var request = await _httpClient.SendAsync(reqeuest);
                request.EnsureSuccessStatusCode();
                var stringResponse = await request.Content.ReadAsStringAsync();

                var responseType = typeof(RModel);
                if (responseType.IsPrimitive)
                {
                    return (RModel)Convert.ChangeType(stringResponse, responseType);
                }
                else
                {
                    var response = JsonConvert.DeserializeObject<RModel>(stringResponse);
                    if (response is null)
                    {
                        return new RModel();
                    }
                    return response;
                }
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public async Task<RModel> PerformAction<RModel>(string url, HttpMethod method) where RModel : new()
        {
            var reqeuest = GenerateRequest(url, method);
            try
            {
                using var request = await _httpClient.SendAsync(reqeuest);
                request.EnsureSuccessStatusCode();
                var stringResponse = await request.Content.ReadAsStringAsync();

                var responseType = typeof(RModel);
                if (responseType.IsPrimitive)
                {
                    if (responseType.Equals(typeof(bool)))
                    {
                        return stringResponse switch
                        {
                            "" => (RModel)Convert.ChangeType(true, responseType),
                            "true" => (RModel)Convert.ChangeType(true, responseType),
                            "false" => (RModel)Convert.ChangeType(false, responseType),
                            _ => throw new NotImplementedException(),
                        };
                    }
                    return (RModel)Convert.ChangeType(stringResponse, responseType);
                }
                else
                {
                    var response = JsonConvert.DeserializeObject<RModel>(stringResponse);
                    if (response is null)
                    {
                        return new RModel();
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }
    }
}
