using Application.Common.Results;
using Application.DTOs;

namespace Application.Common.Interfaces
{
    public interface IProjectService
    {
        Task<ServiceResult<ProjectResponse>>? GetProjectByIdAsync(int projectId);
        Task<ServiceResult<bool>> CreateProjectAsync(CreateProjectRequest request);
        Task<ServiceResult<bool>> UpdateProjectAsync(UpdateProjectRequest request);
        Task<ServiceResult<bool>> DeleteProjectAsync(int projectId);
        Task<ServiceResult<IEnumerable<ProjectResponse>>> GetAllProjectsAsync();
        Task<ServiceResult<IEnumerable<ProjectResponse>>> GetProjectsByUserIdAsync(int userId);
        Task<ServiceResult<IEnumerable<ProjectResponse>>> SearchProjectsAsync(string query);
        Task<ServiceResult<bool>> AssignUserToProjectAsync(int projectId, int userId);
        Task<ServiceResult<bool>> RemoveUserFromProjectAsync(int projectId, int userId);
        Task<ServiceResult<ProjectTasksResponse>> GetProjectWithTasksAsync(int projectId);
        Task<ServiceResult<ProjectUsersResponse>> GetProjectWithUsersAsync(int projectId);
    }
}
