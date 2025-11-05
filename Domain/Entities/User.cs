using Domain.Entities.TaskModels;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser<int>
    {

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Name => $"{FirstName} {LastName}".Trim();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public required UserRole Role { get; set; }

        // Refresh Token properties
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        // Navigation property
        public ICollection<UserProject> Projects { get; set; } = new List<UserProject>();
        public ICollection<UserTask> Tasks { get; set; } = new List<UserTask>();
    }
}
