using Workflow.Domain.Entities;

namespace Workflow.Domain.Interfaces;

public interface IPOJService
{
    Task<PointOrdreJour> CreerPOJAsync(PointOrdreJour poj);
    Task<PointOrdreJour> ModifierPOJAsync(PointOrdreJour poj);
    Task<bool> SupprimerPOJAsync(int id);
    Task<PointOrdreJour?> GetByIdAsync(int id);
    Task<IEnumerable<PointOrdreJour>> GetBySeanceAsync(int seanceId);
}
