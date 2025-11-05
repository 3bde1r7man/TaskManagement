using Application.Common.Interfaces;
using Application.Common.Results;
using Application.DTOs;
using Domain.Entities;
using Domain.Entities.ProjectModels;

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
            var project = await projectRepository.FindByIdAsync(projectId);
            if (project == null)
            {
                return ServiceResult<bool>.Failure("Project not found.");
            }
            var user = await userRepository.FindByIdAsync(userId);
            if (user == null)
            {
                return ServiceResult<bool>.Failure("User not found.");
            }

            project.Users.Add(new UserProject
            {
                ProjectId = projectId,
                UserId = userId,
                ProjectName = project.Title,
                User = user,
                Project = project
            });
            await projectRepository.UpdateProjectAsync(project);
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
            await projectRepository.AddProjectAsync(project);
            await projectRepository.SaveChangesAsync();
            return ServiceResult<bool>.Success(true, "Project created successfully.");

        }

        public async Task<ServiceResult<bool>> DeleteProjectAsync(int projectId)
        {
            var project = await projectRepository.FindByIdAsync(projectId);
            if (project == null)
            {
                return ServiceResult<bool>.Failure("Project not found.");
            }
            await projectRepository.DeleteProjectAsync(project);
            await projectRepository.SaveChangesAsync();
            return ServiceResult<bool>.Success(true, "Project deleted successfully.");

        }

        public async Task<ServiceResult<IEnumerable<ProjectResponse>>> GetAllProjectsAsync()
        {
            var projects = await projectRepository.GetAllProjectsAsync();
            
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
            var project = await projectRepository.FindByIdAsync(projectId);
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
            var project = await projectRepository.FindByIdAsync(projectId);
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
            await projectRepository.UpdateProjectAsync(project);
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
            var project = await projectRepository.FindByIdAsync(request.Id);
            if (project == null)
            {
                return ServiceResult<bool>.Failure("Project not found.");
            }
            if (request.Title != null)
            {
                project.Title = request.Title;
            }
            if (request.Description != null)
            {
                project.Description = request.Description;
            }
            if (request.StartDate != null)
            {
                project.StartDate = request.StartDate ?? project.StartDate;
            }
            if (request.DueDate != null)
            {
                project.DueDate = request.DueDate ?? project.DueDate;
            }
            if (request.Status != null)
            {
                project.Status = request.Status ?? project.Status;
            }
            await projectRepository.UpdateProjectAsync(project);
            await projectRepository.SaveChangesAsync();
            return ServiceResult<bool>.Success(true, "Project updated successfully.");
        }
    }
}
