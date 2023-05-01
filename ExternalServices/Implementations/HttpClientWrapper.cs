using ExternalServices.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace ExternalServices.Implementations
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient _httpClient;
        public HttpClientWrapper()
        {
            _httpClient = new HttpClient();
        }


        private HttpRequestMessage GenerateRequest<REModel>(string url, REModel payload, HttpMethod method)
        {
            HttpRequestMessage sendRequest = new HttpRequestMessage(method, url);
            
            var payloadType = typeof(REModel);
            var content = new StringContent(
                payloadType == typeof(string)
                ? payload.ToString() 
                : JsonConvert.SerializeObject(payload)
                    ,Encoding.UTF8
                , payloadType == typeof(string) 
                ? "application/xml" 
                : "application/json");

            sendRequest.Content = content;

            return sendRequest;
        }

        private HttpRequestMessage GenerateRequest(string url, HttpMethod method)
        {
            HttpRequestMessage sendRequest = new HttpRequestMessage(method, url);

            return sendRequest;
        }

/*        private RModel HandlerError<RModel>(RModel payload,HttpStatusCode statusCode,string message)
        {

        }*/

        public void addHeader(string key,string value)
        {
            _httpClient.DefaultRequestHeaders.Add(key, value);
        }

        public async Task<RModel> PerformAction<REModel, RModel>(string url, REModel payload, HttpMethod method) where RModel : new()
        {
            var reqeuest = GenerateRequest<REModel>(url, payload, method);
            try
            {
                using (var request = await _httpClient.SendAsync(reqeuest))
                {
                    request.EnsureSuccessStatusCode();
                    var stringResponse = await request.Content.ReadAsStringAsync();

                    var responseType = typeof(RModel);
                    if (responseType.IsPrimitive)
                    {
                        return (RModel)Convert.ChangeType(stringResponse, responseType);
                    }
                    else
                    {
                        return JsonConvert.DeserializeObject<RModel>(stringResponse);
                    }
                }
            }
            catch(Exception ex) 
            {
                throw ex;
            }
        }

        public async Task<RModel> PerformAction<RModel>(string url, HttpMethod method) where RModel : new()
        {
            var reqeuest = GenerateRequest(url, method);
            try
            {
                using (var request = await _httpClient.SendAsync(reqeuest))
                {
                    request.EnsureSuccessStatusCode();
                    var stringResponse = await request.Content.ReadAsStringAsync();

                    var responseType = typeof(RModel);
                    if(responseType.IsPrimitive)
                    {
                        return (RModel)Convert.ChangeType(stringResponse,responseType);
                    }
                    else
                    {
                        return JsonConvert.DeserializeObject<RModel>(stringResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
