using Application.Common.Authorization;
using Application.Common.Interfaces;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet("{projectId}")]
        [AuthorizeRoles]
        [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProjectById(int projectId)
        {
            var result = await _projectService.GetProjectByIdAsync(projectId);
            
            if (!result.IsSuccess)
            {
                return NotFound(new ErrorResponse
                {
                    Message = result.Message,
                    Errors = result.Errors
                });
            }

            return Ok(result.Data);
        }

        [HttpGet]
        [AuthorizeAdmin]
        [ProducesResponseType(typeof(IEnumerable<ProjectResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllProjects()
        {
            var result = await _projectService.GetAllProjectsAsync();
            
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
                {
                    Message = result.Message,
                    Errors = result.Errors
                });
            }

            return Ok(result);
        }

        [HttpPost]
        [AuthorizeAdmin]
        [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Invalid input data",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            var result = await _projectService.CreateProjectAsync(request);
            
            if (!result.IsSuccess)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = result.Message,
                    Errors = result.Errors
                });
            }

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut]
        [AuthorizeAdmin]
        [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProject([FromBody] UpdateProjectRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Invalid input data",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            var result = await _projectService.UpdateProjectAsync(request);
            
            if (!result.IsSuccess)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = result.Message,
                    Errors = result.Errors
                });
            }

            return Ok(result);
        }

        [HttpDelete("{projectId}")]
        [AuthorizeAdmin]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProject(int projectId)
        {
            var result = await _projectService.DeleteProjectAsync(projectId);
            
            if (!result.IsSuccess)
            {
                return NotFound(new ErrorResponse
                {
                    Message = result.Message,
                    Errors = result.Errors
                });
            }

            return Ok(result);
        }

        [HttpPost("{projectId}/users/{userId}")]
        [AuthorizeAdmin]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AssignUserToProject(int projectId, int userId)
        {
            var result = await _projectService.AssignUserToProjectAsync(projectId, userId);
            
            if (!result.IsSuccess)
            {
                return NotFound(new ErrorResponse
                {
                    Message = result.Message,
                    Errors = result.Errors
                });
            }

            return Ok(result);
        }

        [HttpDelete("{projectId}/users/{userId}")]
        [AuthorizeAdmin]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveUserFromProject(int projectId, int userId)
        {
            var result = await _projectService.RemoveUserFromProjectAsync(projectId, userId);
            
            if (!result.IsSuccess)
            {
                return NotFound(new ErrorResponse
                {
                    Message = result.Message,
                    Errors = result.Errors
                });
            }

            return Ok(result);
        }

        [HttpGet("search")]
        [AuthorizeAdmin]
        [ProducesResponseType(typeof(IEnumerable<ProjectResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchProjects([FromQuery] string query)
        {
            var result = await _projectService.SearchProjectsAsync(query);
            
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
                {
                    Message = result.Message,
                    Errors = result.Errors
                });
            }

            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        [AuthorizeRoles]
        [ProducesResponseType(typeof(IEnumerable<ProjectResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProjectsByUserId(int userId)
        {
            var result = await _projectService.GetProjectsByUserIdAsync(userId);
            
            if (!result.IsSuccess)
            {
                return NotFound(new ErrorResponse
                {
                    Message = result.Message,
                    Errors = result.Errors
                });
            }

            return Ok(result);
        }
    }
}
