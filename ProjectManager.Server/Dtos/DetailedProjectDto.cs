namespace ProjectManager.Server.Dtos
{
    public class DetailedProjectDto // a detailed project presentation (project with tasks)
    {
        public int ProjectId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; }
        public List<TaskDto> Tasks { get; set; } = new();
    }
}
