using BusinessLayer.Interfaces;
using DataAccessLayer;
using Models.Authentification;
using System.Security.Claims;

namespace BusinessLayer.Implementation
{
    public class AuthentificationService : IAuthentificationService
    {
        private readonly IJwtService _jwtService;
        public AuthentificationService(IJwtService jwtService)
        {
            _jwtService= jwtService;
        }

        public AuthentificationResponse? Authentificate(AuthentificationRequest request)
        {
            var claim = CreateClaim(request);
            if(claim == null ) {
                return null;
            }

            return _jwtService.CreateJwt(claim);
        }

        private static Claim[] CreateClaim(AuthentificationRequest request)
        {
            var listClaim = new List<Claim>
            {
                new Claim(ClaimTypes.Name,request.Username)
            };

            return listClaim.ToArray();
        }
    }
}
