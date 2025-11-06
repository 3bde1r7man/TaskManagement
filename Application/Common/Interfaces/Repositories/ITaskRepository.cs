using Domain.Entities;


namespace Application.Common.Interfaces
{
    public interface ITaskRepository : IGenericRepository<Domain.Entities.TaskModels.Task>
    {
        Task<IEnumerable<Domain.Entities.TaskModels.Task>> GetTasksByProjectId(int projectId);
        Task<IEnumerable<User>> GetUsersForTask(int taskId);
        Task<IEnumerable<Domain.Entities.TaskModels.Task>> GetTasksByUserId(int userId);
        IEnumerable<Domain.Entities.TaskModels.Task> SearchTasks(string query);
    }
}
