using System.ComponentModel.DataAnnotations;

namespace Workflow.Domain.Enums;

public enum TypeSeance
{
    [Display(Name = "Collège")]
    College,

    [Display(Name = "Conseil")]
    Conseil
}
