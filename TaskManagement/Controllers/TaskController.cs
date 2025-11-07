using Application.Common.Authorization;
using Application.Common.Interfaces;
using Application.DTOs;
using Domain.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        [AuthorizeRoles(RoleConstants.Admin, RoleConstants.User)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllTasks()
        {
            var result = await _taskService.GetAllTasksAsync();
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
        [AuthorizeRoles(RoleConstants.Admin, RoleConstants.User)]
        [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTask(CreateTaskRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Invalid input data",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }
            var result = await _taskService.CreateTaskAsync(request);
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
        [AuthorizeRoles(RoleConstants.Admin, RoleConstants.User)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTask(UpdateTaskRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Invalid input data",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }
            var result = await _taskService.UpdateTaskAsync(request);
            if (!result.IsSuccess)
            {
                if (result.Errors != null && result.Errors.Contains("not found"))
                {
                    return NotFound(new ErrorResponse
                    {
                        Message = result.Message,
                        Errors = result.Errors
                    });
                }
                return BadRequest(new ErrorResponse
                {
                    Message = result.Message,
                    Errors = result.Errors
                });
            }
            return Ok();
        }

        [HttpDelete("{taskId}")]
        [AuthorizeRoles(RoleConstants.Admin, RoleConstants.User)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var result = await _taskService.DeleteTaskAsync(taskId);
            if (!result.IsSuccess)
            {
                if (result.Errors != null && result.Errors.Contains("not found"))
                {
                    return NotFound(new ErrorResponse
                    {
                        Message = result.Message,
                        Errors = result.Errors
                    });
                }
                return BadRequest(new ErrorResponse
                {
                    Message = result.Message,
                    Errors = result.Errors
                });
            }
            return Ok();
        }

        [HttpGet("{taskId}/users")]
        [AuthorizeAdmin]
        [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsersForTask(int taskId)
        {
            var result = await _taskService.GetUsersForTaskAsync(taskId);
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

        [HttpGet("{taskId}")]
        [AuthorizeRoles(RoleConstants.Admin, RoleConstants.User)]
        [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTask(int taskId)
        {
            var result = await _taskService.GetTaskByIdAsync(taskId);
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

        [HttpGet("user/{userId}")]
        [AuthorizeRoles(RoleConstants.Admin, RoleConstants.User)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTasksByUserId(int userId)
        {
            var result = await _taskService.GetTasksByUserIdAsync(userId);
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

        [HttpGet("project/{projectId}")]
        [AuthorizeRoles(RoleConstants.Admin, RoleConstants.User)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTasksByProjectId(int projectId)
        {
            var result = await _taskService.GetTasksByProjectIdAsync(projectId);
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


        [HttpPost("{taskId}/users/{userId}")]
        [AuthorizeAdmin]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AssignUserToTask(int taskId, int userId)
        {
            var result = await _taskService.AssignUserToTaskAsync(taskId, userId);
            if (!result.IsSuccess)
            {
                if (result.Errors != null && result.Errors.Contains("not found"))
                {
                    return NotFound(new ErrorResponse
                    {
                        Message = result.Message,
                        Errors = result.Errors
                    });
                }
                return BadRequest(new ErrorResponse
                {
                    Message = result.Message,
                    Errors = result.Errors
                });
            }
            return Ok();
        }

        [HttpDelete("{taskId}/users/{userId}")]
        [AuthorizeAdmin]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveUserFromTask(int taskId, int userId)
        {
            var result = await _taskService.RemoveUserFromTaskAsync(taskId, userId);
            if (!result.IsSuccess)
            {
                if (result.Errors != null && result.Errors.Contains("not found"))
                {
                    return NotFound(new ErrorResponse
                    {
                        Message = result.Message,
                        Errors = result.Errors
                    });
                }
                return BadRequest(new ErrorResponse
                {
                    Message = result.Message,
                    Errors = result.Errors
                });
            }
            return Ok();
        }

        // change task status
        [HttpPatch("{taskId}/status/{status}")]
        [AuthorizeRoles(RoleConstants.Admin, RoleConstants.User)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangeTaskStatus(int taskId, Domain.Entities.TaskModels.TaskStatus status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Invalid input data",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }
            var result = await _taskService.ChangeTaskStatusAsync(taskId, status);
            if (!result.IsSuccess)
            {
                if (result.Errors != null && result.Errors.Contains("not found"))
                {
                    return NotFound(new ErrorResponse
                    {
                        Message = result.Message,
                        Errors = result.Errors
                    });
                }
                return BadRequest(new ErrorResponse
                {
                    Message = result.Message,
                    Errors = result.Errors
                });
            }
            return Ok();

        }

        [HttpGet("search")]
        [AuthorizeAdmin]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchTasks([FromQuery] string query)
        {
            var result = await _taskService.SearchTasksAsync(query);
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

        [HttpGet("upcoming/{daysAhead}")]
        [AuthorizeAdmin]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUpcomingTasks(int daysAhead)
        {
            var result = await _taskService.GetUpcomingTasksAsync(daysAhead);
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

        [HttpGet("due/{date}")]
        [AuthorizeAdmin]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTasksDueOnDate(DateOnly date)
        {
            var result = await _taskService.GetTasksDueOnDateAsync(date);
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

        [HttpGet("status/{status}")]
        [AuthorizeAdmin]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTasksByStatus(Domain.Entities.TaskModels.TaskStatus status)
        {
            var result = await _taskService.GetTasksByStatusAsync(status);
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
    }
}
