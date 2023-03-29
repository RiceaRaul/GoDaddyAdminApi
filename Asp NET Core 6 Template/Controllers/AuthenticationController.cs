using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Authentification;

namespace Asp_NET_Core_6_Template.Controllers
{
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthentificationService _authentificationService;
        public AuthenticationController(IAuthentificationService authentificationService)
        {
            _authentificationService = authentificationService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Authentification/Auth")]
        public ActionResult<AuthentificationResponse> Auth([FromBody] AuthentificationRequest request)
        {
            var response = _authentificationService.Authentificate(request);

            return Ok(response);
        }
    }
}
