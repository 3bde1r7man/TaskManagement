using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.TaskModels
{
    public enum TaskStatus
    {
        [Display(Name = "Pending")]
        Pending,
        [Display(Name = "In Progress")]
        InProgress,
        [Display(Name = "Completed")]
        Completed,
        [Display(Name = "On Hold")]
        OnHold,
        [Display(Name = "Cancelled")]
        Cancelled,
        [Display(Name = "Overdue")]
        Overdue
    }
}
