using System.ComponentModel.DataAnnotations;
using ProjectManager.Server.Models;

namespace ProjectManager.Server.Dtos
{
    public class TaskDto
    {
        public int TaskId { get; set; }
        public int ProjectId { get; set; } // Foreign key to Project
        public string Title { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; } = false;
    }
}
