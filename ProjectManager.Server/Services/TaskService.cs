using Microsoft.EntityFrameworkCore;
using ProjectManager.Data;
using ProjectManager.Server.Dtos;
using ProjectManager.Server.Models;

namespace ProjectManager.Server.Services
{
    public class TaskService
    {
        private readonly ApplicationDbContext _context;
        public TaskService(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<TaskDto?> CreateTaskAsync(int projectId, CreateTaskDto createTaskDto, int userId)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.ProjectId == projectId && p.UserId == userId);

            if (project == null) return null;

            var task = new ProjectTask
            {
                ProjectId = projectId,
                Title = createTaskDto.Title,
                DueDate = createTaskDto.DueDate
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return new TaskDto
            {
                TaskId = task.TaskId,
                ProjectId = task.ProjectId,
                Title = task.Title,
                DueDate = task.DueDate,
                IsCompleted = task.IsCompleted
            };
        }

        public async Task<TaskDto?> UpdateTaskAsync(int taskId, UpdateTaskDto updateTaskDto, int userId)
        {
            var task = await _context.Tasks
                .Include(t => t.Project) // when updating a task, update it also in project's tasks
                .FirstOrDefaultAsync(t => (t.TaskId == taskId) && (t.Project.UserId == userId));

            if (task == null) return null;

            task.Title = updateTaskDto.Title;
            task.DueDate = updateTaskDto.DueDate;
            task.IsCompleted = updateTaskDto.IsCompleted;

            await _context.SaveChangesAsync();

            return new TaskDto
            {
                TaskId = task.TaskId,
                ProjectId = task.ProjectId,
                Title = task.Title,
                DueDate = task.DueDate,
                IsCompleted = task.IsCompleted
            };
        }

        public async Task<bool> DeleteTaskAsync (int TaskId, int UserId)
        {
            var task = await _context.Tasks
                .Include(t => t.Project) // when deleting a task, delete it also from project's tasks
                .FirstOrDefaultAsync(t => (t.TaskId == TaskId) && (t.Project.UserId == UserId));
            
            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
