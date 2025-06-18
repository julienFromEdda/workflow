using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Workflow.Domain.Entities;
using Workflow.Domain.Interfaces;
using Workflow.Persistence;

namespace Workflow.Application.Services;

public class ActionDossierService(WFContext context, ICurrentUserService userManager) : IActionDossierService
{
    public async Task<ActionDossier> EnregistrerActionAsync(ActionDossier action)
    {
        context.ActionsDossiers.Add(action);
        await context.SaveChangesAsync();
        return action;
    }

    public async Task<IEnumerable<ActionDossier>> GetHistoriqueByDossierAsync(int dossierId)
    {
        return await context.ActionsDossiers
            .Where(a => a.DossierId == dossierId)
            .ToListAsync();
    }

    public async Task AddToHistory(int dossierId, string actionName, string description)
    {
        var user = await userManager.GetUserAsync();
        var action = new ActionDossier
        {
            DossierId = dossierId,
            UtilisateurId = user!.Id,
            Date = DateTime.Now,
            Action = actionName,
            Description = description
        };
        await EnregistrerActionAsync(action);
    }
}
