using Microsoft.EntityFrameworkCore;
using ProjectManager.Data;
using ProjectManager.Server.Dtos;
using ProjectManager.Server.Models;

namespace ProjectManager.Server.Services
{
    public class ProjectService
    {
        private readonly ApplicationDbContext _context;

        public ProjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SimpleProjectDto>> GetUserProjectsAsync(int userId)
        {
            return await _context.Projects
                .Where(p => p.UserId == userId)
                .Select(p => new SimpleProjectDto
                {
                    ProjectId = p.ProjectId,
                    Title = p.Title,
                    Description = p.Description,
                    CreationDate = p.CreationDate,
                    TaskCount = p.Tasks.Count,
                    CompletedTaskCount = p.Tasks.Count(t => t.IsCompleted)
                })
                .OrderByDescending(p => p.CreationDate)
                .ToListAsync();
        }

        public async Task<DetailedProjectDto?> GetProjectByIdAsync(int projectId, int userId)
        {
            var project = await _context.Projects
                .Include(p => p.Tasks) // present also project's tasks
                .FirstOrDefaultAsync(p => p.ProjectId == projectId && p.UserId == userId);

            if (project == null) return null;

            return new DetailedProjectDto
            {
                ProjectId = project.ProjectId,
                Title = project.Title,
                Description = project.Description,
                CreationDate = project.CreationDate,
                Tasks = project.Tasks.Select(t => new TaskDto
                {
                    TaskId = t.TaskId,
                    Title = t.Title,
                    DueDate = t.DueDate,
                    IsCompleted = t.IsCompleted,
                    ProjectId = t.ProjectId
                }).OrderBy(t => t.IsCompleted).ThenBy(t => t.DueDate).ToList()
            };
        }

        public async Task<SimpleProjectDto?> CreateProjectAsync(CreateProjectDto createProjectDto, int userId)
        {
            var project = new Project
            {
                Title = createProjectDto.Title,
                Description = createProjectDto.Description,
                UserId = userId
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return new SimpleProjectDto
            {
                ProjectId = project.ProjectId,
                Title = project.Title,
                Description = project.Description,
                CreationDate = project.CreationDate,
                TaskCount = 0,
                CompletedTaskCount = 0
            };
        }

        public async Task<bool> DeleteProjectAsync(int projectId, int userId)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.ProjectId == projectId && p.UserId == userId);

            if (project == null) return false;

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
