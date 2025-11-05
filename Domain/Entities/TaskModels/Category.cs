using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.TaskModels
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
    }
}
