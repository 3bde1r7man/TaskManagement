using Domain.Entities.ProjectModels;

namespace Application.Common.Interfaces
{
    public interface IProjectRepository
    {
        Task<Project>? FindByIdAsync(int id);
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<IEnumerable<Project>> GetProjectsByUserIdAsync(int userId);
        Task AddProjectAsync(Project project);
        Task UpdateProjectAsync(Project project);
        Task DeleteProjectAsync(Project project);
        Task DeleteProjectAsync(int id);
        Task SaveChangesAsync();
        IEnumerable<Project> SearchProjects(string query);

    }
}
