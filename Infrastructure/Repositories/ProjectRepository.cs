using Application.Common.Ineterfaces.Repositories;
using Domain.Entities.ProjectModels;


namespace Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        public Task AddProjectAsync(Project project)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProjectAsync(Project project)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProjectAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Project>? FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateProjectAsync(Project project)
        {
            throw new NotImplementedException();
        }
    }
}
