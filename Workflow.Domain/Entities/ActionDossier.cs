namespace Workflow.Domain.Entities;

public class ActionDossier
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string? Action { get; set; }
    public string? Description { get; set; }

    public string? UtilisateurId { get; set; }
    public  Utilisateur? Utilisateur { get; set; }

    public int DossierId { get; set; }
    public Dossier? Dossier { get; set; }
}
