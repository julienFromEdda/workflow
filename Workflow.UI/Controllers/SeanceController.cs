using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workflow.Domain.Entities;
using Workflow.Domain.Enums;
using Workflow.Domain.Interfaces;
using Workflow.Domain.Security;

namespace Workflow.UI.Controllers;

[Authorize]
public class SeanceController(ISeanceService service) : Controller
{
    [Authorize(Policy = Permissions.Seance.Lire)]
    public async Task<IActionResult> Index(TypeSeance type)
    {
        var seances = await service.GetByTypeAsync(type);

        return View(seances);
    }

    [Authorize(Policy = Permissions.Seance.Modifier)]
    public IActionResult Create(TypeSeance type)
    {
        return PartialView("Partials/Create", new Seance 
        { 
            Type = type,
            Date = DateTime.UtcNow
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permissions.Seance.Modifier)]
    public async Task<IActionResult> Create(Seance seance)
    {
        if (ModelState.IsValid)
        {
            await service.CreerSeanceAsync(seance);

            return RedirectToAction(nameof(Index), new { type = seance.Type });
        }
        ViewBag.TypeSeance = seance.Type;
        return PartialView("Partials/Create", seance);
    }

    [Authorize(Policy = Permissions.Seance.Lire)]
    public async Task<IActionResult> Details(int id)
    {
        var seance = await service.GetByIdAsync(id);
        if (seance == null) return NotFound();

        return View(seance);
    }

    [Authorize(Policy = Permissions.Seance.Modifier)]
    public async Task<IActionResult> Edit(int id)
    {
        var seance = await service.GetByIdAsync(id);
        if (seance == null) return NotFound();

        return PartialView("Partials/Edit", seance);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permissions.Seance.Modifier)]
    public async Task<IActionResult> Edit(int id, Seance seance)
    {
        if (id != seance.Id) return BadRequest();
        if (ModelState.IsValid)
        {
            await service.ModifierSeanceAsync(seance);
            
            return RedirectToAction(nameof(Details), new { id });
        }

        return PartialView("Partials/Edit", seance);
    }

    [Authorize(Policy = Permissions.Seance.Modifier)]
    public async Task<IActionResult> Delete(int id)
    {
        var dossier = await service.GetByIdAsync(id);
        if (dossier == null) return NotFound();

        return PartialView("Partials/Delete", dossier);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Policy = Permissions.Seance.Modifier)]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var seance = await service.GetByIdAsync(id);
        if (seance == null) return NotFound();

        await service.SupprimerSeanceAsync(id);

        return RedirectToAction(nameof(Index), new { type = seance.Type });
    }
}