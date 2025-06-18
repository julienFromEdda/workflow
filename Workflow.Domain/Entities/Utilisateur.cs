using Microsoft.AspNetCore.Identity;

namespace Workflow.Domain.Entities;

public class Utilisateur : IdentityUser
{
    public required string Nom { get; set; }
    public required string Prenom { get; set; }

    public int ServiceId { get; set; }
    public Service? Service { get; set; }

    public ICollection<Vote>? Votes { get; set; }
    public ICollection<Notification>? Notifications { get; set; }
}
