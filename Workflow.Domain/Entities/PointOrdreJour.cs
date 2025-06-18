using Workflow.Domain.Enums;

namespace Workflow.Domain.Entities;

public class PointOrdreJour
{
    public int Id { get; set; }
    public string? Titre { get; set; }
    public string? Description { get; set; }
    public string? Decision { get; set; }
    public StatutPOJ Statut { get; set; }

    public int? DossierId { get; set; }
    public Dossier? Dossier { get; set; }

    public int SeanceId { get; set; }
    public Seance? Seance { get; set; }

    public ICollection<Document>? Documents { get; set; }
    public ICollection<Vote>? Votes { get; set; }
}
