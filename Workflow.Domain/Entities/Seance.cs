using Workflow.Domain.Enums;

namespace Workflow.Domain.Entities;

public class Seance
{
    public int Id { get; set; }
    public TypeSeance Type { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Heure { get; set; }
    public StatutSeance Statut { get; set; }

    public ICollection<PointOrdreJour>? POJs { get; set; }
}
