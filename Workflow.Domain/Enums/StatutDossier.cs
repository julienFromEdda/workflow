using System.ComponentModel.DataAnnotations;

namespace Workflow.Domain.Enums;

public enum StatutDossier
{
    [Display(Name = "Créé")]
    Cree,

    [Display(Name = "En attente d'approbation")]
    EnAttenteApprobation,

    [Display(Name = "En cours de traitement")]
    EnCoursTraitement,

    [Display(Name = "En attente de décision")]
    EnAttenteDecision,

    [Display(Name = "En attente de délibération")]
    EnAttenteDeliberation,

    [Display(Name = "Clôturé")]
    Cloture,

    [Display(Name = "Archivé")]
    Archive,

    [Display(Name = "Supprimé")]
    Supprime
}
