using Common.Settings;
using Encryptions.Interfaces;
using ExternalServices.Interfaces;
using Microsoft.Extensions.Options;
using Models.GoDaddyApi.Domains;
using Models.GoDaddyApi.Shopper;

namespace ExternalServices.Implementations
{
    public class GoDaddyClient : IGoDaddyClient
    {
        private readonly IHttpClientWrapper _httpClient;
        private readonly ISalsa20Service _encryptionService;
        private readonly AppSettings _appSettings;

        private const string GET_SHOPPER = "/shoppers/{0}?includes=customerId";
        private const string DOMAINS = "domains";

        public GoDaddyClient(IHttpClientWrapper httpClient,IOptions<AppSettings> appSettings, ISalsa20Service encryptionService)
        {
            _httpClient = httpClient;
            _encryptionService = encryptionService;
            _appSettings = appSettings.Value;
            _httpClient.setBaseUrl(_appSettings.GoDaddySettings.BaseUrl);

            SetAuthorization();
        }

        private void SetAuthorization()
        {
            _httpClient.addHeader(nameof(_appSettings.GoDaddySettings.Authorization), _encryptionService.Decrypt(_appSettings.GoDaddySettings.Authorization));
        }

        public async Task<ShopperResponse> GetShopperById(string shopperId)
        {
            var url = string.Format(GET_SHOPPER, shopperId);
            var request = await _httpClient.PerformAction<ShopperResponse>(url, HttpMethod.Get);

            return request;
        }

        public async Task<GetDomainsResponse> GetDomains()
        {
            var request = await _httpClient.PerformAction<List<Domain>>(DOMAINS, HttpMethod.Get);

            return new GetDomainsResponse {
                Domains = request
            };
        }
    }
}
