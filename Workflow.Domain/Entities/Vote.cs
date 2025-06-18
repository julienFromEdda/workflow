using Workflow.Domain.Enums;

namespace Workflow.Domain.Entities;

public class Vote
{
    public int Id { get; set; }
    public string? UtilisateurId { get; set; }
    public Utilisateur? Utilisateur { get; set; }

    public int PointOrdreJourId { get; set; }
    public PointOrdreJour? PointOrdreJour { get; set; }

    public TypeVote Valeur { get; set; }
}
