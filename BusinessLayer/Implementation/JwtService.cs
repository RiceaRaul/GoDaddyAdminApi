using BusinessLayer.Interfaces;
using Common.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.Authentification;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessLayer.Implementation
{
    public class JwtService : IJwtService
    {
        private readonly AppSettings _appSettings;

        public JwtService(IOptions<AppSettings> appSettings)
        {
            _appSettings= appSettings.Value;
        }

        public AuthentificationResponse CreateJwt(Claim[] claim)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT.Secret));
            var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var username = claim.Where(claim => claim.Type == ClaimTypes.Name).First().Value;
            var token = new JwtSecurityToken(
                _appSettings.JWT.Issuer,
                _appSettings.JWT.Audience,
                claim,
                notBefore:DateTime.Now,
                expires:DateTime.Now.AddHours(_appSettings.JWT.ExpireHours),
                signingCredentials:credentials
            );

            return new AuthentificationResponse(token, username);
        }
    }
}
