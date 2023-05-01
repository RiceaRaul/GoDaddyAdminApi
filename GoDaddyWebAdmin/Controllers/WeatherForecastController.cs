using GoDaddyWebAdmin.Attributes;
using Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using ExternalServices.Interfaces;
using Models.ErrorHandling;
using Encryptions.Implementations;
using Encryptions.Interfaces;

namespace GoDaddyWebAdmin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IHttpClientWrapper _httpClient;
        private readonly ISalsa20Service _encryptService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpClientWrapper httpClient, ISalsa20Service encryptService)
        {
            _logger = logger;
            _httpClient = httpClient;
            _encryptService = encryptService;
        }

        [HttpGet]
        [Route("encrypt/{text}")]
        public string Encrypt(string text)
        {
            //  _httpClient.addHeader("Authorization", "sso-key gHVdbUvwRNw2_9zdbiwfMqbNAoTZozvnr71:2GPUAz7kfjgaj1szJwJmRP");
            // _httpClient.PerformAction<ShopperResponse>("https://api.godaddy.com/v1/shoppers/569165511?includes=customerId", HttpMethod.Get);
            return _encryptService.Encrypt(text);
        }

        [HttpGet]
        [Route("decrypt/{text}")]
        public string Decrypt(string text)
        {
            //  _httpClient.addHeader("Authorization", "sso-key gHVdbUvwRNw2_9zdbiwfMqbNAoTZozvnr71:2GPUAz7kfjgaj1szJwJmRP");
            // _httpClient.PerformAction<ShopperResponse>("https://api.godaddy.com/v1/shoppers/569165511?includes=customerId", HttpMethod.Get);
            return _encryptService.Decrypt(text);
        }

    }


    public class ShopperResponse : HttpWrapperError
    {
        public string shopperId { get; set; }
        public string marketId { get; set; }
        public string email { get; set; }
        public object externalId { get; set; }
        public string nameFirst { get; set; }
        public string nameLast { get; set; }
        public string customerId { get; set; }
    }

}