using Application.Common.Interfaces;
using Application.Common.Results;
using Application.DTOs;
using Domain.Entities.TaskModels;
using Microsoft.EntityFrameworkCore;

namespace Application.Service
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        public TaskService(
            ITaskRepository taskRepository,
            IUserRepository userRepository,
            IProjectRepository projectRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
        }

        public async Task<ServiceResult<bool>> AssignUserToTaskAsync(int taskId, int userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);
            if (user == null)
            {
                return ServiceResult<bool>.Failure("User not found.");
            }
            var task = await _taskRepository.Include([t => t.AssigneUsers]).FirstOrDefaultAsync(t => t.Id == taskId)
            ;
            
            if (task == null)
            {
                return ServiceResult<bool>.Failure("Task not found.");
            }
            // Check if the user is already assigned to the task
            var usersForTask = await _taskRepository.GetUsersForTask(taskId);
            if (usersForTask.Any(u => u.Id == userId))
            {
                return ServiceResult<bool>.Failure("User is already assigned to this task.");
            }
            // Assign user to task
            task.AssigneUsers.Add(new UserTask
            {
                UserId = userId,
                TaskId = taskId,
                User = user,
                Task = task
            });
            _taskRepository.Update(task);
            await _taskRepository.SaveChangesAsync();
            return ServiceResult<bool>.Success(true, $"User {user.Name} assigned to task {task.Title} successfully.");
        }

        public async Task<ServiceResult<bool>> ChangeTaskStatusAsync(int taskId, string status)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null)
            {
                return ServiceResult<bool>.Failure("Task not found.");
            }
            if (!Enum.TryParse<Domain.Entities.TaskModels.TaskStatus>(status, true, out var newStatus))
            {
                return ServiceResult<bool>.Failure("Invalid status value.");
            }
            task.Status = newStatus;
            _taskRepository.Update(task);
            await _taskRepository.SaveChangesAsync();
            return ServiceResult<bool>.Success(true, "Task status updated successfully.");
        }

        public async Task<ServiceResult<bool>> CreateTaskAsync(CreateTaskRequest request)
        {
            var project =  await _projectRepository.GetByIdAsync(request.ProjectId);
            if (project == null)
            {
                return ServiceResult<bool>.Failure("Project not found.");
            }

            var task = new Domain.Entities.TaskModels.Task
            {
                Title = request.Title,
                Description = request.Description,
                DueDate = request.DueDate,
                Status = request.Status,
                ProjectId = request.ProjectId,
                StartDate = request.StartDate ?? DateOnly.FromDateTime(DateTime.UtcNow)
            };
            await _taskRepository.AddAsync(task);
            await _taskRepository.SaveChangesAsync();
            return ServiceResult<bool>.Success(true, "Task created successfully.");
        }

        public async Task<ServiceResult<bool>> DeleteTaskAsync(int taskId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null)
            {
                return ServiceResult<bool>.Failure("Task not found.");
            }
            _taskRepository.RemoveAsync(task.Id);
            await _taskRepository.SaveChangesAsync();
            return ServiceResult<bool>.Success(true, $"The Task {task.Title} deleted successfully.");
        }

        public async Task<ServiceResult<IEnumerable<TaskResponse>>> GetAllTasksAsync()
        {
            var tasks = await _taskRepository.GetAllAsync();
            var taskResponses = tasks.Select(t => new TaskResponse
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                StartDate = t.StartDate,
                DueDate = t.DueDate,
                Status = t.Status
            });
            return ServiceResult<IEnumerable<TaskResponse>>.Success(taskResponses, "Tasks retrieved successfully.");

        }

        public async Task<ServiceResult<TaskResponse>>? GetTaskByIdAsync(int taskId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null)
            {
                return ServiceResult<TaskResponse>.Failure("Task not found.");
            }
            var taskResponse = new TaskResponse
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                StartDate = task.StartDate,
                DueDate = task.DueDate,
                Status = task.Status
            };
            return ServiceResult<TaskResponse>.Success(taskResponse, "Task retrieved successfully.");
        }

        public async Task<ServiceResult<IEnumerable<TaskResponse>>> GetTasksByProjectIdAsync(int projectId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
            {
                return ServiceResult<IEnumerable<TaskResponse>>.Failure("Project not found.");
            }
            var tasks = await _taskRepository.FindAsync(t => t.ProjectId == projectId);
            var taskResponses = tasks.Select(t => new TaskResponse
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                StartDate = t.StartDate,
                DueDate = t.DueDate,
                Status = t.Status
            });
            return ServiceResult<IEnumerable<TaskResponse>>.Success(taskResponses, "Tasks retrieved successfully.");
        }

        public async Task<ServiceResult<IEnumerable<TaskResponse>>> GetTasksByStatusAsync(Domain.Entities.TaskModels.TaskStatus status)
        {
            var tasks = await _taskRepository.FindAsync(t => t.Status == status);
            var taskResponses = tasks.Select(t => new TaskResponse
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                StartDate = t.StartDate,
                DueDate = t.DueDate,
                Status = t.Status
            });
            return ServiceResult<IEnumerable<TaskResponse>>.Success(taskResponses, "Tasks retrieved successfully.");

        }

        public async Task<ServiceResult<IEnumerable<TaskResponse>>> GetTasksByUserIdAsync(int userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);
            if (user == null)
            {
                return ServiceResult<IEnumerable<TaskResponse>>.Failure("User not found.");
            }
            var tasks = await _taskRepository.GetTasksByUserId(userId);
            var taskResponses = tasks.Select(t => new TaskResponse
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                StartDate = t.StartDate,
                DueDate = t.DueDate,
                Status = t.Status
            });
            return ServiceResult<IEnumerable<TaskResponse>>.Success(taskResponses, "Tasks retrieved successfully.");
        }

        public async Task<ServiceResult<IEnumerable<TaskResponse>>> GetTasksDueOnDateAsync(DateOnly date)
        {
            var tasks = await _taskRepository.FindAsync(t => t.DueDate == date);
            var taskResponses = tasks.Select(t => new TaskResponse
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                StartDate = t.StartDate,
                DueDate = t.DueDate,
                Status = t.Status
            });
            return ServiceResult<IEnumerable<TaskResponse>>.Success(taskResponses, "Tasks retrieved successfully.");
        }

        public async Task<ServiceResult<IEnumerable<TaskResponse>>> GetUpcomingTasksAsync(int daysAhead)
        {
            var targetDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(daysAhead));
            var tasks = await _taskRepository.FindAsync(t => t.DueDate <= targetDate);
            var taskResponses = tasks.Select(t => new TaskResponse
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                StartDate = t.StartDate,
                DueDate = t.DueDate,
                Status = t.Status
            });
            return ServiceResult<IEnumerable<TaskResponse>>.Success(taskResponses, "Upcoming tasks retrieved successfully.");
        }

        public async Task<ServiceResult<IEnumerable<UserResponse>>> GetUsersForTaskAsync(int taskId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null)
            {
                return ServiceResult<IEnumerable<UserResponse>>.Failure("Task not found.");
            }
            var users = await _taskRepository.GetUsersForTask(taskId);
            var userResponses = users.Select(u => new UserResponse
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email
            });
            return ServiceResult<IEnumerable<UserResponse>>.Success(userResponses, "Users for task retrieved successfully.");
        }

        public async Task<ServiceResult<bool>> RemoveUserFromTaskAsync(int taskId, int userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);
            if (user == null)
            {
                return ServiceResult<bool>.Failure("User not found.");
            }
            var task = await _taskRepository.Include([t=> t.AssigneUsers]).FirstOrDefaultAsync(t => t.Id == taskId);
            if (task == null)
            {
                return ServiceResult<bool>.Failure("Task not found.");
            }
            if (!task.AssigneUsers.Any(ut => ut.UserId == userId))
            {
                return ServiceResult<bool>.Failure("User is not assigned to this task.");
            }

            task.AssigneUsers.Remove(task.AssigneUsers.First(ut => ut.UserId == userId));
            _taskRepository.Update(task);
            await _taskRepository.SaveChangesAsync();

            return ServiceResult<bool>.Success(true, $"User {user.Name} removed from task {task.Title} successfully.");
        }

        public async Task<ServiceResult<IEnumerable<TaskResponse>>> SearchTasksAsync(string query)
        {
            var tasks = _taskRepository.SearchTasks(query);
            var taskResponses = tasks.Select(t => new TaskResponse
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                StartDate = t.StartDate,
                DueDate = t.DueDate,
                Status = t.Status
            });
            return ServiceResult<IEnumerable<TaskResponse>>.Success(taskResponses, "Tasks retrieved successfully.");
        }

        public async Task<ServiceResult<bool>> UpdateTaskAsync(UpdateTaskRequest request)
        {
            var task = await _taskRepository.GetByIdAsync(request.Id);
            if (task == null)
            {
                return ServiceResult<bool>.Failure("Task not found.");
            }
            task.Title = request.Title ?? task.Title;
            task.Description = request.Description ?? task.Description;
            task.StartDate = request.StartDate ?? task.StartDate;
            task.DueDate = request.DueDate ?? task.DueDate;
            task.Status = request.Status ?? task.Status;

            _taskRepository.Update(task);
            await _taskRepository.SaveChangesAsync();
            return ServiceResult<bool>.Success(true, "Task updated successfully.");

        }
    }
}
