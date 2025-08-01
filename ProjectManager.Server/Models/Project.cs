using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Server.Models
{
    public class Project
    {
        public int ProjectId { get; set; } 
        public int UserId { get; set; } // Foreign key to User
        public User User { get; set; } = null!; // Navigation property to User

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;
        [StringLength(500)]
        public string? Description { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();

    }
}
