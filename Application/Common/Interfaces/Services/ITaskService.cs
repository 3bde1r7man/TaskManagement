using Application.Common.Results;
using Application.DTOs;

namespace Application.Common.Interfaces
{
    public interface ITaskService
    {
        Task<ServiceResult<TaskResponse>>? GetTaskByIdAsync(int taskId);
        Task<ServiceResult<bool>> CreateTaskAsync(CreateTaskRequest request);
        Task<ServiceResult<bool>> UpdateTaskAsync(UpdateTaskRequest request);
        Task<ServiceResult<bool>> DeleteTaskAsync(int taskId);
        Task<ServiceResult<IEnumerable<TaskResponse>>> GetAllTasksAsync();
        Task<ServiceResult<IEnumerable<TaskResponse>>> GetTasksByProjectIdAsync(int projectId);
        Task<ServiceResult<IEnumerable<TaskResponse>>> GetTasksByUserIdAsync(int userId);
        Task<ServiceResult<bool>> AssignUserToTaskAsync(int taskId, int userId);
        Task<ServiceResult<bool>> RemoveUserFromTaskAsync(int taskId, int userId);
        Task<ServiceResult<IEnumerable<UserResponse>>> GetUsersForTaskAsync(int taskId);
        Task<ServiceResult<IEnumerable<TaskResponse>>> SearchTasksAsync(string query);
        Task<ServiceResult<bool>> ChangeTaskStatusAsync(int taskId, Domain.Entities.TaskModels.TaskStatus status);
        Task<ServiceResult<IEnumerable<TaskResponse>>> GetUpcomingTasksAsync(int daysAhead);
        Task<ServiceResult<IEnumerable<TaskResponse>>> GetTasksDueOnDateAsync(DateOnly date);
        Task<ServiceResult<IEnumerable<TaskResponse>>> GetTasksByStatusAsync(Domain.Entities.TaskModels.TaskStatus status);
    }
}
