using ExternalServices.Interfaces;

namespace ExternalServices.Implementations
{
    public class GoDaddyClient
    {
        private readonly IHttpClientWrapper _httpClient;
        public GoDaddyClient(IHttpClientWrapper httpClient)
        {
            _httpClient = httpClient;
        }
    }
}
