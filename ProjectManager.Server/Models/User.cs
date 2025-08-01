using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Server.Models
{
    public class User
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        [StringLength(100)]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public ICollection<Project> Projects { get; set; } = new List<Project>();

    }
}
