using Workflow.Domain.DTOs;
using Workflow.Domain.Entities;

namespace Workflow.Domain.Interfaces;

public interface IDossierService
{
    Task<Dossier> CreerDossierAsync(Dossier dossier);
    Task<Dossier> ModifierDossierAsync(Dossier dossier);
    Task<bool> SupprimerDossierAsync(int id);
    Task<Dossier> GetByIdAsync(int id);
    Task<PagedResult<Dossier>> GetFilteredAsync(ListFilterDTO filtre);
    Task<List<Service>> GetAvailableServiceForTransfert(int id);
    Task Transfert(int id, int nouveauServiceId);
    Task Archive(int id);
    Task<List<Utilisateur>> GetAvailableUserForAssign(int id);
    Task Assign(int id, string nouvelEmployeId);
}
