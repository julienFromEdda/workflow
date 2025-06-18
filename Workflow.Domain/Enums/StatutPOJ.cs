using System.ComponentModel.DataAnnotations;

namespace Workflow.Domain.Enums;

public enum StatutPOJ
{
    [Display(Name = "À analyser")]
    AAnalyser,

    [Display(Name = "En discussion")]
    EnDiscussion,

    [Display(Name = "Approuvé")]
    Approuve,

    [Display(Name = "Refusé")]
    Refuse
}
