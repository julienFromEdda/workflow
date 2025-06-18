using System.ComponentModel.DataAnnotations;

namespace Workflow.Domain.Enums;

public enum TypeVote
{
    [Display(Name = "Pour")]
    Pour,

    [Display(Name = "Contre")]
    Contre,

    [Display(Name = "Abstention")]
    Abstention
}
