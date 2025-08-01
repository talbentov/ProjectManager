using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Server.Dtos
{
    public class CreateTaskDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        public DateTime? DueDate { get; set; }
    }
}
