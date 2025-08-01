using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Server.Dtos
{
    public class RegisterDto
    {
        [Required]
        [StringLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
    }
}
