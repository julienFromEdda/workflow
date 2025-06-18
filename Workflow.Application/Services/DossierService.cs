using Microsoft.EntityFrameworkCore;
using Workflow.Domain.Entities;
using Workflow.Domain.Enums;
using Workflow.Domain.Exceptions;
using Workflow.Domain.Interfaces;
using Workflow.Persistence;

namespace Workflow.Application.Services;

public class DossierService(WFContext context, IActionDossierService actionDossierService, ICurrentUserService currentUser, INotificationService notificationService) : IDossierService
{
    public async Task<Dossier> CreerDossierAsync(Dossier dossier)
    {
        var user = await currentUser.GetUserAsync();

        dossier.EmployeTraitantId = user!.Id;
        dossier.ServiceTraitantId = user!.ServiceId;
        dossier.Statut = StatutDossier.Cree;
        dossier.DateCreation = dossier.DateModification = System.DateTime.Now;
        context.Dossiers.Add(dossier);
        await context.SaveChangesAsync();

        await actionDossierService.AddToHistory(dossier.Id, "Création", "Création du dossier");

        return dossier;
    }

    public async Task<List<Service>> GetAvailableServiceForTransfert(int id)
    {
        var dossier = await GetByIdAsync(id);

        return await context.Services
            .Where(s => s.Id != dossier.ServiceTraitantId)
            .ToListAsync();
    }

    public async Task Transfert(int id, int nouveauServiceId)
    {
        var dossier = await GetByIdAsync(id);

        var nouveauService = await context.Services.FindAsync(nouveauServiceId);
        if (nouveauService == null)
            throw new ApiException("Service non trouvé", 404);

        var ancienEmployeId = dossier.EmployeTraitantId;
        dossier.ServiceTraitantId = nouveauService.Id;
        dossier.EmployeTraitantId = null;
        await ModifierDossierAsync(dossier);

        await actionDossierService.AddToHistory(dossier!.Id, "Transfert", $"Dossier transféré vers le service '{nouveauService.Nom}'");

        await notificationService.NotifierUtilisateurAsync(ancienEmployeId!, "Votre dossier a été transféré vers un autre service.");
    }

    public async Task<Dossier> ModifierDossierAsync(Dossier toUpdate)
    {
        var dossier = await GetByIdAsync(toUpdate.Id);

        dossier.Titre = toUpdate.Titre;
        dossier.Statut = toUpdate.Statut;
        dossier.Description = toUpdate.Description;
        dossier.Intervenant = toUpdate.Intervenant;
        dossier.DateModification = DateTime.Now;

        context.Dossiers.Update(dossier);
        await context.SaveChangesAsync();

        await actionDossierService.AddToHistory(dossier.Id, "Modification", "Modification du signalétique");

        return dossier;
    }

    public async Task<bool> SupprimerDossierAsync(int id)
    {
        var dossier = await GetByIdAsync(id);

        dossier.Statut = StatutDossier.Supprime;

        context.Dossiers.Update(dossier);
        await context.SaveChangesAsync();

        await actionDossierService.AddToHistory(dossier.Id, "Suppression", "Dossier marqué comme supprimé");

        return true;
    }

    public async Task<Dossier> GetByIdAsync(int id)
    {
        var dossier = await context.Dossiers
            .Include(d => d.ServiceTraitant)
            .Include(d => d.EmployeTraitant)
            .Include(d => d.Documents)
            .Include(d => d.Historique)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (dossier == null)
            throw new ApiException("Dossier non trouvé", 404);

        dossier.Documents = await context.Documents
            .Where(d => d.TypeObjet == "Dossier" && d.ObjetId == dossier.Id)
            .ToListAsync();

        return dossier;
    }

    public async Task<IEnumerable<Dossier>> GetAllAsync()
    {
        var user = await currentUser.GetUserAsync();
        return await context.Dossiers
            .Include(d => d.ServiceTraitant)
            .Where(d => d.ServiceTraitantId == user!.ServiceId)
            .Where(d => d.Statut != StatutDossier.Supprime)
            .ToListAsync();
    }

    public async Task Archive(int id)
    {
        var dossier = await GetByIdAsync(id);

        dossier!.Statut = StatutDossier.Archive;
        await ModifierDossierAsync(dossier);

        await actionDossierService.AddToHistory(dossier!.Id, "Archivage", "Dossier archivé");
    }

    public async Task<List<Utilisateur>> GetAvailableUserForAssign(int id)
    {
        var user = await currentUser.GetUserAsync();
        var dossier = await GetByIdAsync(id);

        return await context.Users
            .Where(u => u.ServiceId == user!.ServiceId && u.Id != null && u.Id != user.Id)
            .ToListAsync();
    }

    public async Task Assign(int id, string nouvelEmployeId)
    {
        var dossier = await GetByIdAsync(id);

        dossier.EmployeTraitantId = nouvelEmployeId;
        await ModifierDossierAsync(dossier);

        dossier = await GetByIdAsync(dossier.Id);

        await actionDossierService.AddToHistory(dossier!.Id, "Assignation", $"Dossier assigné à {dossier.EmployeTraitant?.Prenom} {dossier.EmployeTraitant?.Nom}");
        await notificationService.NotifierUtilisateurAsync(nouvelEmployeId, "Un dossier vous a été assigné.");

    }
}