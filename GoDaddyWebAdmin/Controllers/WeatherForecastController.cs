using Encryptions.Interfaces;
using ExternalServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GoDaddyWebAdmin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ISalsa20Service _encryptService;

        public WeatherForecastController(ISalsa20Service encryptService)
        {
            _encryptService = encryptService;
        }

        [HttpGet]
        [Route("encrypt/{text}")]
        public string Encrypt(string text)
        {
            return _encryptService.Encrypt(text);
        }

        [HttpGet]
        [Route("decrypt/{text}")]
        public string Decrypt(string text)
        {
            return _encryptService.Decrypt(text);
        }

    }
}