namespace Workflow.Domain.Entities;

public class Service
{
    public int Id { get; set; }
    public string? Nom { get; set; }

    public ICollection<Utilisateur> Employes { get; set; } = [];
}
