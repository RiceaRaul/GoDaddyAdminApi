using Common.Settings;
using Encryptions.Interfaces;
using ExternalServices.Interfaces;
using Microsoft.Extensions.Options;
using Models.GoDaddyApi.Shopper;

namespace ExternalServices.Implementations
{
    public class GoDaddyClient : IGoDaddyClient
    {
        private readonly IHttpClientWrapper _httpClient;
        private readonly ISalsa20Service _encryptionService;
        private readonly AppSettings _appSettings;

        private const string GET_SHOPPER = "https://api.godaddy.com/v1/shoppers/{0}?includes=customerId";

        public GoDaddyClient(IHttpClientWrapper httpClient,IOptions<AppSettings> appSettings, ISalsa20Service encryptionService)
        {
            _httpClient = httpClient;
            _encryptionService = encryptionService;
            _appSettings = appSettings.Value;

            setAuthorization();
        }

        private void setAuthorization()
        {
            _httpClient.addHeader(nameof(_appSettings.GoDaddySettings.Authorization), _encryptionService.Decrypt(_appSettings.GoDaddySettings.Authorization));
        }

        public async Task<ShopperResponse> GetShopperById(string shopperId)
        {
            var url = string.Format(GET_SHOPPER, shopperId);
            var request = await _httpClient.PerformAction<ShopperResponse>(url, HttpMethod.Get);

            return request;
        }
    }
}
