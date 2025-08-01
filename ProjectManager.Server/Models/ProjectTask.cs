using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Server.Models
{
    public class ProjectTask
    {
        public int TaskId { get; set; }
        public int ProjectId { get; set; } // Foreign key to Project
        public Project Project { get; set; } = null!; // Navigation property to Project
        [Required]
        public string Title { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; } = false;

    }
}
