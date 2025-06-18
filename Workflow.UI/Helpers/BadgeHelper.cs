using Workflow.Domain.Enums;

namespace Workflow.UI.Helpers;

public static class BadgeHelper
{
    public static string GetBadgeClass(Enum value)
    {
        return value switch
        {
            StatutPOJ.AAnalyser => "bg-secondary",
            StatutPOJ.EnDiscussion => "bg-info",
            StatutPOJ.Approuve => "bg-success",
            StatutPOJ.Refuse => "bg-danger",

            StatutSeance.Prevu => "bg-primary",
            StatutSeance.Termine => "bg-success",
            StatutSeance.Reporte => "bg-warning text-dark",

            TypeSeance.College => "bg-secondary",
            TypeSeance.Conseil => "bg-dark",

            TypeVote.Pour => "bg-success",
            TypeVote.Contre => "bg-danger",
            TypeVote.Abstention => "bg-warning text-dark",

            StatutDossier.Cree => "bg-secondary",
            StatutDossier.EnAttenteApprobation => "bg-info",
            StatutDossier.EnCoursTraitement => "bg-primary",
            StatutDossier.EnAttenteDecision => "bg-warning text-dark",
            StatutDossier.EnAttenteDeliberation => "bg-warning",
            StatutDossier.Cloture => "bg-success",
            StatutDossier.Archive => "bg-dark",
            StatutDossier.Supprime => "bg-danger",

            _ => "bg-light text-dark"
        };
    }
}
