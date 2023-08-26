using ExternalServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.GoDaddyApi.Domains;
using Models.GoDaddyApi.Shopper;

namespace GoDaddyWebAdmin.Controllers
{
    [ApiController]
    public class GoDaddyController : ControllerBase
    {
        private readonly IGoDaddyClient _goDaddyClient;
        public GoDaddyController(IGoDaddyClient goDaddyClient)
        {
            _goDaddyClient = goDaddyClient;
        }

        [HttpGet]
        [Route("GoDaddy/GetShopper/{id}")]
        public async Task<ActionResult<ShopperResponse>> GetShopper(string id)
        {
            var response = await _goDaddyClient.GetShopperById(id);

            return Ok(response);
        }

        [HttpGet]
        [Route("GoDaddy/GetDomains")]
        public async Task<ActionResult<GetDomainsResponse>> GetDomains()
        {
            var response = await _goDaddyClient.GetDomains();

            return Ok(response);
        }
    }
}
