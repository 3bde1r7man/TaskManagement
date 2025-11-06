using Domain.Entities.ProjectModels;

namespace Application.Common.Interfaces
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        Task<IEnumerable<Project>> GetProjectsByUserIdAsync(int userId);
        IEnumerable<Project> SearchProjects(string query);
        Task<Project?> GetByIdWithUsersAsync(int id);
        Task<Project?> GetByIdWithTasksAsync(int id);

    }
}
