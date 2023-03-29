using Models.Authentification;
using System.Security.Claims;

namespace BusinessLayer.Interfaces
{
    public interface IJwtService
    {
        AuthentificationResponse CreateJwt(Claim[] claim);
    }
}
