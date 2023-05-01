using Models.ErrorHandling;

namespace Models.GoDaddyApi.Shopper
{
    public class ShopperResponse : HttpWrapperError
    {
        public string ShopperId { get; set; } = string.Empty;
        public string MarketId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public object ExternalId { get; set; } = string.Empty;
        public string NameFirst { get; set; } = string.Empty;
        public string NameLast { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
    }
}
