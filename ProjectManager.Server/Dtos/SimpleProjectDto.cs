namespace ProjectManager.Server.Dtos
{
    public class SimpleProjectDto // a simple project presentation without tasks
    {
        public int ProjectId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; }
        public int TaskCount { get; set; }
        public int CompletedTaskCount { get; set; }
    }
}
