using System.ComponentModel.DataAnnotations;

namespace Server.Dtos.Auth
{
    public class RegisterRequestDto
    {
        [Required]
        [StringLength(50)]
        public string Nickname { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
    }
}

