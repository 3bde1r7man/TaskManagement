using Domain.Entities.ProjectModels;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class CreateProjectRequest
    {
        [Required]
        [MaxLength(100)]
        public required string Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public required DateOnly StartDate { get; set; }
        [Required]
        public required DateOnly DueDate { get; set; }

        [Required]
        [EnumDataType(typeof(ProjectStatus), ErrorMessage = "Invalid status specified")]
        public required ProjectStatus Status { get; set; }

    }

    public class UpdateProjectRequest
    {
        [Required]
        public required int Id { get; set; }
        [MaxLength(100)]
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? DueDate { get; set; }
        [EnumDataType(typeof(ProjectStatus), ErrorMessage = "Invalid status specified")]
        public ProjectStatus? Status { get; set; }
    }

    public class ProjectResponse
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly DueDate { get; set; }

        [EnumDataType(typeof(ProjectStatus))]
        public required ProjectStatus Status { get; set; }
    }

    public class ProjectTasksResponse
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public IEnumerable<TaskResponse>? tasks { get; set; }
    }

    public class ProjectUsersResponse
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public IEnumerable<UserResponse>? Users { get; set; }
    }
}
