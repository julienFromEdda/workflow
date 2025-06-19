using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Workflow.Domain.Entities;
using Workflow.Domain.Enums;
using Workflow.Domain.Interfaces;
using Workflow.Domain.Security;

namespace Workflow.UI.Controllers;

[Authorize(Policy = Permissions.Vote.Voter)]
public class VoteController(IVoteService voteService, IPOJService pojService, UserManager<Utilisateur> userManager) : Controller
{
    public async Task<IActionResult> Vote(int id)
    {
        var user = await userManager.GetUserAsync(User);
        var poj = await pojService.GetByIdAsync(id);
        if (poj == null)
            return NotFound();

        if (poj.Seance!.Type != TypeSeance.Conseil || poj.Seance.Statut != StatutSeance.Prevu)
        {
            TempData["VoteRefuse"] = "Il n'est plus possible de voter sur ce point (séance clôturée ou non valide).";
            return RedirectToAction("Details", "Seance", new { id = poj.SeanceId });
        }

        var existingVote = poj.Votes?.FirstOrDefault(v => v.UtilisateurId == user!.Id);

        ViewBag.PojId = poj.Id;

        return PartialView("Partials/Vote", existingVote ?? new Vote());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitVote(int pojId, TypeVote valeur)
    {
        var user = await userManager.GetUserAsync(User);

        var poj = await pojService.GetByIdAsync(pojId);

        if (poj == null || poj.Seance!.Type != TypeSeance.Conseil || poj.Seance.Statut != StatutSeance.Prevu)
            return Forbid();

        var existingVotes = await voteService.GetVotesByPOJAsync(pojId);
        var vote = existingVotes.FirstOrDefault(v => v.UtilisateurId == user!.Id);

        if (vote == null)
        {
            vote = new Vote { PointOrdreJourId = pojId, UtilisateurId = user!.Id, Valeur = valeur };
            await voteService.AjouterVoteAsync(vote);
        }
        else
        {
            vote.Valeur = valeur;
            await voteService.ModifierVoteAsync(vote);
        }

        return RedirectToAction("Details", "Seance", new { id = poj.SeanceId });
    }
}
