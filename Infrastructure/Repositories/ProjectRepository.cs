using Application.Common.Ineterfaces.Repositories;
using Domain.Entities.ProjectModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;

        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddProjectAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
        }

        public async Task DeleteProjectAsync(Project project)
        {
            _context.Projects.Remove(project);
        }

        public async Task DeleteProjectAsync(int id)
        {
            var project = await FindByIdAsync(id);
            if (project != null)
                _context.Projects.Remove(project);

        }

        public async Task<Project>? FindByIdAsync(int id)
        {
            return await _context.Projects.FindAsync(id);
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            return await _context.Projects.ToListAsync();
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public async Task UpdateProjectAsync(Project project)
        {
            _context.Projects.Update(project);
        }

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
