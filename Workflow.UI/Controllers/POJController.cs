using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Workflow.Application.Helpers;
using Workflow.Domain.Entities;
using Workflow.Domain.Enums;
using Workflow.Domain.Interfaces;
using Workflow.Domain.Security;
using Workflow.Persistence;

namespace Workflow.UI.Controllers;

[Authorize]
public class POJController(IPOJService pojService, WFContext context) : Controller
{
    [HttpGet]
    [Authorize(Policy = Permissions.POJ.Creer)]

    public async Task<IActionResult> Create(int seanceId)
    {
        ViewBag.SeanceId = seanceId;
        ViewBag.Dossiers = await context.Dossiers
            .Where(d => d.Statut != StatutDossier.Supprime && d.Statut != StatutDossier.Archive)
            .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Titre })
            .ToListAsync();
        return PartialView("Partials/Create", new PointOrdreJour { SeanceId = seanceId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permissions.POJ.Creer)]

    public async Task<IActionResult> Create(PointOrdreJour poj)
    {
        if (ModelState.IsValid)
        {
            await pojService.CreerPOJAsync(poj);
            return RedirectToAction("Details", "Seance", new { id = poj.SeanceId });
        }

        ViewBag.SeanceId = poj.SeanceId;
        ViewBag.Dossiers = await context.Dossiers.Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Titre }).ToListAsync();
        return PartialView("Partials/Create", poj);
    }

    [HttpGet]
    [Authorize(Policy = Permissions.POJ.Modifier)]
    public async Task<IActionResult> Edit(int id)
    {
        var poj = await pojService.GetByIdAsync(id);
        if (poj == null) return NotFound();

        ViewBag.Dossiers = await context.Dossiers
            .Where(d => d.Statut != StatutDossier.Supprime && d.Statut != StatutDossier.Archive)
            .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Titre })
            .ToListAsync();

        return PartialView("Partials/Edit", poj);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permissions.POJ.Modifier)]
    public async Task<IActionResult> Edit(int id, PointOrdreJour poj)
    {
        if (id != poj.Id) return BadRequest();
        if (ModelState.IsValid)
        {
            await pojService.ModifierPOJAsync(poj);
            return RedirectToAction("Details", "Seance", new { id = poj.SeanceId });
        }
        ViewBag.Dossiers = await context.Dossiers.Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Titre }).ToListAsync();
        return PartialView("Partials/Edit", poj);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var poj = await pojService.GetByIdAsync(id);
        return PartialView("Partials/Delete", poj);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permissions.POJ.Supprimer)]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var poj = await pojService.GetByIdAsync(id);
        var seanceId = poj!.SeanceId;
        await pojService.SupprimerPOJAsync(id);
        return RedirectToAction("Details", "Seance", new { id = seanceId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permissions.POJ.Modifier)]
    public async Task<IActionResult> UploadDocument(int pojId, IFormFile fichier)
    {
        if (fichier == null || fichier.Length == 0)
            return RedirectToAction("Details", "Seance", new { id = pojId });

        var document = new Document
        {
            NomFichier = fichier.FileName,
            Url = DocumentHelper.SaveUploadedFile(fichier),
            ObjetId = pojId,
            TypeObjet = "POJ"
        };
        context.Documents.Add(document);
        await context.SaveChangesAsync();

        var poj = await pojService.GetByIdAsync(pojId);
        return PartialView("DetailsModal", poj);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permissions.POJ.Modifier)]
    public async Task<IActionResult> DeleteDocument(int id, int pojId)
    {
        var doc = await context.Documents.FindAsync(id);
        if (doc == null || doc.ObjetId != pojId || doc.TypeObjet != "POJ") return NotFound();

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", doc.Url!.TrimStart('/'));
        if (System.IO.File.Exists(filePath))
            System.IO.File.Delete(filePath);

        context.Documents.Remove(doc);
        await context.SaveChangesAsync();

        var poj = await pojService.GetByIdAsync(pojId);
        return PartialView("DetailsModal", poj);
    }
}