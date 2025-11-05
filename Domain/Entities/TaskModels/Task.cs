using Domain.Entities.ProjectModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Required]
        public required DateOnly StartDate { get; set; }
        [Required]
        public required DateOnly DueDate { get; set; }
        [Required]
        public required TaskStatus Status { get; set; } = TaskStatus.Pending;
        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;
        public ICollection<Category>? Categories { get; set; } = null!;
        public ICollection<UserTask>? AssigneUsers { get; set; } = null!;
    }
}
