using Domain.Entities;


namespace Application.Common.Ineterfaces.Repositories
{
    public interface ITaskRepository
    {
        Task<Domain.Entities.TaskModels.Task>? FindByIdAsync(int id);
        Task<IEnumerable<Domain.Entities.TaskModels.Task>> GetAllTasksAsync();
        Task<IEnumerable<Domain.Entities.TaskModels.Task>> GetTasksByProjectId(int projectId);
        Task<IEnumerable<User>> GetUsersForTask(int taskId);
        Task<IEnumerable<Domain.Entities.TaskModels.Task>> GetTasksByUserId(int userId);
        Task AddTaskAsync(Domain.Entities.TaskModels.Task task);
        Task UpdateTaskAsync(Domain.Entities.TaskModels.Task task);
        Task DeleteTaskAsync(Domain.Entities.TaskModels.Task task);
        Task DeleteTaskAsync(int id);
        Task SaveChangesAsync();
    }
}
