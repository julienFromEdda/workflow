using System.ComponentModel.DataAnnotations;

namespace Workflow.Domain.Enums;

public enum StatutSeance
{
    [Display(Name = "Prévu")]
    Prevu,

    [Display(Name = "Terminé")]
    Termine,

    [Display(Name = "Reporté")]
    Reporte
}
