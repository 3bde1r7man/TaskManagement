using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.TaskModels
{
    public class Task
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public TaskStatus Status { get; set; } = TaskStatus.Pending;

        public ICollection<Category>? Categories { get; set; }
        public ICollection<UserTask>? AssigneUsers { get; set; }

    }
}
