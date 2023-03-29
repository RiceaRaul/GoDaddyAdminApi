using System.IdentityModel.Tokens.Jwt;

namespace Models.Authentification
{
    public class AuthentificationResponse
    {
        public string token { get; set; } = default!;
        public string token_type { get; set; } = default!;
        public double expires_in { get; set; } = default!;
        public string username { get; set; } = default!;

        public AuthentificationResponse(JwtSecurityToken securityToken, string username)
        {
            this.token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            this.token_type = "Bearer";
            this.expires_in = Math.Truncate((securityToken.ValidTo - DateTime.Now).TotalSeconds);
            this.username = username;
        }
    }
}
