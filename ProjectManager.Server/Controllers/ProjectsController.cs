using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Server.Dtos;
using ProjectManager.Server.Services;

namespace ProjectManager.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly ProjectService _projectService;

        public ProjectsController(ProjectService projectService)
        {
            _projectService = projectService;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(userIdClaim!.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var userId = GetUserId();
            var projects = await _projectService.GetUserProjectsAsync(userId);
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(int id)
        {
            var userId = GetUserId();
            var project = await _projectService.GetProjectByIdAsync(id, userId);

            if (project == null)
                return NotFound(new { message = "The project doesn't exist" });

            return Ok(project);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto createProjectDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();
            var project = await _projectService.CreateProjectAsync(createProjectDto, userId);

            if (project == null)
                return BadRequest(new { message = "Error in project creation" });

            return CreatedAtAction(nameof(GetProject), new { id = project.ProjectId }, project); // returns 201 Created with the location of the new project
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var userId = GetUserId();
            var deleted = await _projectService.DeleteProjectAsync(id, userId);

            if (!deleted)
                return NotFound(new { message = "The project is not found" });

            return NoContent();
        }

        [HttpPost("{projectId}/tasks")]
        public async Task<IActionResult> CreateTask(int projectId, [FromBody] CreateTaskDto createTaskDto, [FromServices] TaskService taskService)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();
            var task = await taskService.CreateTaskAsync(projectId, createTaskDto, userId);

            if (task == null)
                return NotFound(new { message = "The project doesn't exist" });

            return CreatedAtAction("GetProject", new { id = projectId }, task); // returns 201 Created with the location of the new task
        }

    }
}
