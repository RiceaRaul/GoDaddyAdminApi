using Models.GoDaddyApi.Domains;
using Models.GoDaddyApi.Shopper;

namespace ExternalServices.Interfaces
{
    public interface IGoDaddyClient
    {
        Task<ShopperResponse> GetShopperById(string shopperId);
        Task<GetDomainsResponse> GetDomains();
    }
}
