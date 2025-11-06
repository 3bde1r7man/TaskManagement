using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TaskRepository : GenericRepository<Domain.Entities.TaskModels.Task>, ITaskRepository
    {

        public TaskRepository(AppDbContext context) : base(context)
        { }

        public async Task<IEnumerable<Domain.Entities.TaskModels.Task>> GetTasksByProjectId(int projectId)
        {
            return await _context.Tasks
                .Where(t => t.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Domain.Entities.TaskModels.Task>> GetTasksByUserId(int userId)
        {
            return await _context.UserTasks
                .Where(ut => ut.UserId == userId)
                .Include(ut => ut.Task)
                .Select(ut => ut.Task)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersForTask(int taskId)
        {
            return await _context.UserTasks
                .Where(ut => ut.TaskId == taskId)
                .Include(ut => ut.User)
                .Select(ut => ut.User)
                .ToListAsync();
        }

        public IEnumerable<Domain.Entities.TaskModels.Task> SearchTasks(string query)
        {
            return _context.Tasks
                .Where(t => t.Title.Contains(query) || t.Description.Contains(query))
                .ToList();
        }
    }
}
