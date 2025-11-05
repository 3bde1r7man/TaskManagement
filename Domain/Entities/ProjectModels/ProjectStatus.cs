

using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.ProjectModels
{
    public enum ProjectStatus
    {
        [Display(Name = "Not Started")]
        NotStarted,
        [Display(Name = "In Progress")]
        InProgress,
        Completed,
        [Display(Name = "On Hold")]
        OnHold,
        Cancelled
    }
}
