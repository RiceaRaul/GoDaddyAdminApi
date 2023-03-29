using BusinessLayer.Interfaces;
using DataAccessLayer;
using Models.Authentification;
using System.Security.Claims;

namespace BusinessLayer.Implementation
{
    public class AuthentificationService : IAuthentificationService
    {
        private readonly IJwtService _jwtService;
        private readonly IUnitOfWork _unitOfWork;
        public AuthentificationService(IJwtService jwtService,IUnitOfWork unitOfWork)
        {
            _jwtService= jwtService;
            _unitOfWork = unitOfWork;
        }

        public AuthentificationResponse? Authentificate(AuthentificationRequest request)
        {
            var claim = CreateClaim(request);
            if(claim == null ) {
                return null;
            }

            return _jwtService.CreateJwt(claim);
        }

        private Claim[] CreateClaim(AuthentificationRequest request)
        {
            var listClaim = new List<Claim>
            {
                new Claim(ClaimTypes.Name,request.Username)
            };

            return listClaim.ToArray();
        }
    }
}
