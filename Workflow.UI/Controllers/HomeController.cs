using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Workflow.Domain.Entities;
using Workflow.Persistence;

namespace Workflow.UI.Controllers;

[Authorize]
public class HomeController(UserManager<Utilisateur> userManager, WFContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var user = await userManager.GetUserAsync(User);

        var notifications = await context.Notifications
            .Where(n => n.UtilisateurId == user.Id)
            .OrderByDescending(n => n.Lu)
            .Take(5)
            .ToListAsync();

        var dossiers = await context.Dossiers
            .Where(d => d.ServiceTraitantId == user.ServiceId && d.Statut != Domain.Enums.StatutDossier.Supprime)
            .OrderByDescending(d => d.DateCreation)
            .Take(5)
            .ToListAsync();

        var prochaineSeance = await context.Seances
            .Where(s => s.Date >= DateTime.Today)
            .OrderBy(s => s.Date)
            .FirstOrDefaultAsync();

        var pojsAVoter = Enumerable.Empty<PointOrdreJour>();
        if (await userManager.IsInRoleAsync(user, "MembreConseil"))
        {
            pojsAVoter = await context.PointsOrdreJour
                .Include(p => p.Seance)
                .Where(p => p.Seance.Type == Domain.Enums.TypeSeance.Conseil
                         && p.Seance.Statut == Domain.Enums.StatutSeance.Prevu
                         && !p.Votes.Any(v => v.UtilisateurId == user.Id))
                .Take(5)
                .ToListAsync();
        }

        ViewBag.Notifications = notifications;
        ViewBag.Dossiers = dossiers;
        ViewBag.Seance = prochaineSeance;
        ViewBag.POJs = pojsAVoter;

        return View();
    }
}