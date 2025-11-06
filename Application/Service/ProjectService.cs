using Application.Common.Interfaces;
using Application.Common.Results;
using Application.DTOs;
using Domain.Entities;
using Domain.Entities.ProjectModels;
using Microsoft.EntityFrameworkCore;

namespace Application.Service
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository projectRepository;
        private readonly IUserRepository userRepository;
        public ProjectService(IProjectRepository projectRepository, IUserRepository userRepository)
        {
            this.projectRepository = projectRepository;
            this.userRepository = userRepository;
        }
        public async Task<ServiceResult<bool>> AssignUserToProjectAsync(int projectId, int userId)
        {
            var project = await projectRepository.GetByIdWithUsersAsync(projectId);
            if (project == null)
            {
                return ServiceResult<bool>.Failure("Project not found.");
            }
            var user = await userRepository.FindByIdAsync(userId);
            if (user == null)
            {
                return ServiceResult<bool>.Failure("User not found.");
            }
            if (project.Users.Any(up => up.UserId == userId))
            {
                return ServiceResult<bool>.Failure("User is already assigned to this project.");
            }

            project.Users.Add(new UserProject
            {
                ProjectId = projectId,
                UserId = userId,
                User = user,
                Project = project
            });
            projectRepository.Update(project);
            await projectRepository.SaveChangesAsync();
            return ServiceResult<bool>.Success(true, $"User {user.Name} added to the project {project.Title} Successfully.");

        }

        public async Task<ServiceResult<bool>> CreateProjectAsync(CreateProjectRequest request)
        {
            var project = new Project
            {
                Title = request.Title,
                Description = request.Description,
                StartDate = request.StartDate,
                DueDate = request.DueDate,
                Status = request.Status
            };
            await projectRepository.AddAsync(project);
            await projectRepository.SaveChangesAsync();
            return ServiceResult<bool>.Success(true, "Project created successfully.");

        }

        public async Task<ServiceResult<bool>> DeleteProjectAsync(int projectId)
        {
            var project = await projectRepository.GetByIdAsync(projectId);
            if (project == null)
            {
                return ServiceResult<bool>.Failure("Project not found.");
            }
            projectRepository.RemoveAsync(project.Id);
            await projectRepository.SaveChangesAsync();
            return ServiceResult<bool>.Success(true, "Project deleted successfully.");

        }

        public async Task<ServiceResult<IEnumerable<ProjectResponse>>> GetAllProjectsAsync()
        {
            var projects = await projectRepository.GetAllAsync();

            var projectResponses = projects.Select(p => new ProjectResponse
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                StartDate = p.StartDate,
                DueDate = p.DueDate,
                Status = p.Status
            });

            return ServiceResult<IEnumerable<ProjectResponse>>.Success(projectResponses);
        }

        public async Task<ServiceResult<ProjectResponse>>? GetProjectByIdAsync(int projectId)
        {
            var project = await projectRepository.GetByIdAsync(projectId);
            if (project == null)
            {
                return ServiceResult<ProjectResponse>.Failure("Project not found.");
            }
            var projectResponse = new ProjectResponse
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                StartDate = project.StartDate,
                DueDate = project.DueDate,
                Status = project.Status
            };
            return ServiceResult<ProjectResponse>.Success(projectResponse);
        }

        public async Task<ServiceResult<IEnumerable<ProjectResponse>>> GetProjectsByUserIdAsync(int userId)
        {
            var user = await userRepository.FindByIdAsync(userId);
            if (user == null)
            {
                return ServiceResult<IEnumerable<ProjectResponse>>.Failure("User not found.");
            }
            var projects = await projectRepository.GetProjectsByUserIdAsync(userId);
            var projectResponses = projects.Select(p => new ProjectResponse
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                StartDate = p.StartDate,
                DueDate = p.DueDate,
                Status = p.Status
            });
            return ServiceResult<IEnumerable<ProjectResponse>>.Success(projectResponses);
        }

        public async Task<ServiceResult<bool>> RemoveUserFromProjectAsync(int projectId, int userId)
        {
            var project = await projectRepository.GetByIdWithUsersAsync(projectId);
            if (project == null)
            {
                return ServiceResult<bool>.Failure("Project not found.");
            }
            var user = await userRepository.FindByIdAsync(userId);
            if (user == null)
            {
                return ServiceResult<bool>.Failure("User not found.");
            }
            var userProject = project.Users.FirstOrDefault(up => up.UserId == userId);
            if (userProject == null)
            {
                return ServiceResult<bool>.Failure("User is not assigned to this project.");
            }
            project.Users.Remove(userProject);
            projectRepository.Update(project);
            await projectRepository.SaveChangesAsync();
            return ServiceResult<bool>.Success(true, $"User {user.Name} removed from the project {project.Title} Successfully.");
        }

        public Task<ServiceResult<IEnumerable<ProjectResponse>>> SearchProjectsAsync(string query)
        {
            var projects = projectRepository.SearchProjects(query);
            var projectResponses = projects.Select(p => new ProjectResponse
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                StartDate = p.StartDate,
                DueDate = p.DueDate,
                Status = p.Status
            });
            return Task.FromResult(ServiceResult<IEnumerable<ProjectResponse>>.Success(projectResponses));
        }

        public async Task<ServiceResult<bool>> UpdateProjectAsync(UpdateProjectRequest request)
        {
            var project = await projectRepository.GetByIdAsync(request.Id);
            if (project == null)
            {
                return ServiceResult<bool>.Failure("Project not found.");
            }
            project.Title = request.Title ?? project.Title;
            project.Description = request.Description ?? project.Description;
            project.StartDate = request.StartDate ?? project.StartDate;
            project.DueDate = request.DueDate ?? project.DueDate;
            project.Status = request.Status ?? project.Status;

            projectRepository.Update(project);
            await projectRepository.SaveChangesAsync();
            return ServiceResult<bool>.Success(true, "Project updated successfully.");
        }

        public async Task<ServiceResult<ProjectTasksResponse>> GetProjectWithTasksAsync(int projectId)
        {
            var project = await projectRepository.GetByIdWithTasksAsync(projectId);
            if (project == null)
            {
                return ServiceResult<ProjectTasksResponse>.Failure("Project not found.");
            }
            var projectTasksResponse = new ProjectTasksResponse
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                tasks = project.Tasks?.Select(t => new TaskResponse
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Status = t.Status,
                    StartDate = t.StartDate,
                    DueDate = t.DueDate

                })
            };
            return ServiceResult<ProjectTasksResponse>.Success(projectTasksResponse);
        }

        public async Task<ServiceResult<ProjectUsersResponse>> GetProjectWithUsersAsync(int projectId)
        {
            var project = await projectRepository.GetByIdWithUsersAsync(projectId);
            if (project == null)
            {
                return ServiceResult<ProjectUsersResponse>.Failure("Project not found.");
            }
            var projectUsersResponse = new ProjectUsersResponse
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                Users = project.Users?.Select(up => new UserResponse
                {
                    Id = up.User.Id,
                    Name = up.User.Name,
                    Email = up.User.Email
                })
            };
            return ServiceResult<ProjectUsersResponse>.Success(projectUsersResponse);
        }
    }
}
