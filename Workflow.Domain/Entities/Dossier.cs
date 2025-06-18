using Workflow.Domain.Enums;

namespace Workflow.Domain.Entities;

public class Dossier
{
    public int Id { get; set; }
    public string? Titre { get; set; }
    public string? Description { get; set; }
    public DateTime DateCreation { get; set; }
    public DateTime DateModification { get; set; }
    public StatutDossier Statut { get; set; }
    public string? Intervenant { get; set; }

    public string? EmployeTraitantId { get; set; }
    public Utilisateur? EmployeTraitant { get; set; }

    public int ServiceTraitantId { get; set; }
    public Service? ServiceTraitant { get; set; }

    public ICollection<ActionDossier>? Historique { get; set; }
    public ICollection<Document>? Documents { get; set; }
    public ICollection<PointOrdreJour>? POJs { get; set; }
}
