using Domain.Entities.ProjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Ineterfaces.Repositories
{
    public interface IProjectRepository
    {
        Task<Project>? FindByIdAsync(int id);
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task AddProjectAsync(Project project);
        Task UpdateProjectAsync(Project project);
        Task DeleteProjectAsync(Project project);
        Task DeleteProjectAsync(int id);
        Task SaveChangesAsync();

    }
}
