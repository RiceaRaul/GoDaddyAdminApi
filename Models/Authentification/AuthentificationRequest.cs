using System.ComponentModel.DataAnnotations;

namespace Models.Authentification
{
    public class AuthentificationRequest
    {
        [Required]
        public string Username { get; set; } = default!;
        [Required]
        public string Password { get; set; } = default!;
    }
}
