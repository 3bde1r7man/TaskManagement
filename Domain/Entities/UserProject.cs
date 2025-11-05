using Domain.Entities.ProjectModels;

namespace Domain.Entities
{
    public class UserProject
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int ProjectId { get; set; }
        public required string ProjectName { get; set; }
        public Project Project { get; set; } = null!;
    }
}
