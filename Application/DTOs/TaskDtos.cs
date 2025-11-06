using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class CreateTaskRequest
    {
        [Required]
        [MaxLength(100)]
        public required string Title { get; set; }
        public string? Description { get; set; }
        
        public DateOnly? StartDate { get; set; }
        [Required]
        public required DateOnly DueDate { get; set; }
        [Required]
        public required int ProjectId { get; set; }
        [Required]
        [EnumDataType(typeof(Domain.Entities.TaskModels.TaskStatus), ErrorMessage = "Invalid status specified")]
        public required Domain.Entities.TaskModels.TaskStatus Status { get; set; }
    }

    public class UpdateTaskRequest
    {
        [Required]
        public required int Id { get; set; }
        [MaxLength(100)]
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? DueDate { get; set; }

        [EnumDataType(typeof(Domain.Entities.TaskModels.TaskStatus), ErrorMessage = "Invalid status specified")]
        public Domain.Entities.TaskModels.TaskStatus? Status { get; set; }
    }

    public class TaskResponse
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required DateOnly StartDate { get; set; }
        public required DateOnly DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [EnumDataType(typeof(Domain.Entities.TaskModels.TaskStatus))]
        public required Domain.Entities.TaskModels.TaskStatus Status { get; set; }
    }

}
