using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.NotificationModels
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Title { get; set; }

        public string? Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public NotificationStatus Status { get; set; } = NotificationStatus.Unread;
    }
}
