using System.ComponentModel.DataAnnotations;


namespace Domain.Entities.ProjectModels
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public required DateOnly StartDate { get; set; }
        [Required]
        public required DateOnly DueDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public required ProjectStatus Status { get; set; } = ProjectStatus.NotStarted;

        // Navigation property
        public ICollection<TaskModels.Task> Tasks { get; set; } = null!;
        public ICollection<UserProject> Users { get; set; } = null!;
    }
}
