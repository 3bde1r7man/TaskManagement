using Application.Common.Interfaces;
using Domain.Entities.ProjectModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class ProjectRepository : GenericRepository<Project>,  IProjectRepository
    {

        public ProjectRepository(AppDbContext context) : base(context)
        { }

        public async Task<IEnumerable<Project>> GetProjectsByUserIdAsync(int userId)
        {
            return await _context.UserProjects.Where(up => up.UserId == userId).Include( up => up.Project).
                Select(up => up.Project).ToListAsync();
        }

        public IEnumerable<Project> SearchProjects(string query)
        {
            return _context.Projects
                .Where(p => p.Title.Contains(query) || p.Description.Contains(query))
                .ToList();
        }
    }
}
