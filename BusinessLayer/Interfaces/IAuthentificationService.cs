using Models.Authentification;

namespace BusinessLayer.Interfaces
{
    public interface IAuthentificationService
    {
        AuthentificationResponse? Authentificate(AuthentificationRequest request);
    }
}
