using Application.Common.Ineterfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddTaskAsync(Domain.Entities.TaskModels.Task task)
        {
            await _context.Tasks.AddAsync(task);
        }

        public async Task DeleteTaskAsync(Domain.Entities.TaskModels.Task task)
        {
            _context.Tasks.Remove(task);
        }

        public async Task DeleteTaskAsync(int id)
        {
            var task = await FindByIdAsync(id);
            if (task != null)
                _context.Tasks.Remove(task);
        }

        public async Task<Domain.Entities.TaskModels.Task>? FindByIdAsync(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }


        public async Task<IEnumerable<Domain.Entities.TaskModels.Task>> GetAllTasksAsync()
        {
            return await _context.Tasks.ToListAsync();
        }


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
                .Select(ut => ut.Task)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersForTask(int taskId)
        {
            return await _context.UserTasks
                .Where(ut => ut.TaskId == taskId)
                .Select(ut => ut.User)
                .ToListAsync();
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public async Task UpdateTaskAsync(Domain.Entities.TaskModels.Task task)
        {
            _context.Tasks.Update(task);
        }
    }
}
