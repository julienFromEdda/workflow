using Workflow.Domain.Entities;

namespace Workflow.Domain.Interfaces;

public interface IActionDossierService
{
    Task<ActionDossier> EnregistrerActionAsync(ActionDossier action);
    Task<IEnumerable<ActionDossier>> GetHistoriqueByDossierAsync(int dossierId);
    Task AddToHistory(int dossierId, string actionName, string description);
}
