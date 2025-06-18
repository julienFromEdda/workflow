using Workflow.Domain.Entities;
using Workflow.Domain.Enums;

namespace Workflow.Domain.Interfaces;

public interface ISeanceService
{
    Task<Seance> CreerSeanceAsync(Seance seance);
    Task<Seance> ModifierSeanceAsync(Seance seance);
    Task<bool> SupprimerSeanceAsync(int id);
    Task<Seance?> GetByIdAsync(int id);
    Task<IEnumerable<Seance>> GetByTypeAsync(TypeSeance type);
}
