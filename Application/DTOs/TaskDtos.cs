using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class CreateTaskRequest
    {
        [Required]
        [MaxLength(100)]
        public required string Title { get; set; }
        public string? Description { get; set; }
        public DateOnly DueDate { get; set; }
        [Required]
        public required int ProjectId { get; set; }
        [Required]
        [EnumDataType(typeof(Domain.Entities.TaskModels.TaskStatus), ErrorMessage = "Invalid status specified")]
        public required Domain.Entities.TaskModels.TaskStatus Status { get; set; }
    }

    public class UpdateTaskRequest
    {
        [MaxLength(100)]
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateOnly? DueDate { get; set; }
        public int? ProjectId { get; set; }

        [EnumDataType(typeof(Domain.Entities.TaskModels.TaskStatus), ErrorMessage = "Invalid status specified")]
        public Domain.Entities.TaskModels.TaskStatus? Status { get; set; }
    }

    public class TaskResponse
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public DateOnly DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [EnumDataType(typeof(Domain.Entities.TaskModels.TaskStatus))]
        public required Domain.Entities.TaskModels.TaskStatus Status { get; set; }
    }

}
