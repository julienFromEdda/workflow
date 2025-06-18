namespace Workflow.Domain.Entities;

public class Notification
{
    public int Id { get; set; }
    public string? UtilisateurId { get; set; }
    public Utilisateur? Utilisateur { get; set; }

    public string? Message { get; set; }
    public string? LienObjet { get; set; }
    public bool Lu { get; set; }
}
