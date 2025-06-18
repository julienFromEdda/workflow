using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workflow.Application.Services;
using Workflow.Domain.Entities;
using Workflow.Domain.Enums;
using Workflow.Domain.Interfaces;

namespace Workflow.UI.Controllers;

[Authorize(Roles = "Greffe")]
public class SeanceController(ISeanceService service) : Controller
{
    public async Task<IActionResult> Index(TypeSeance type)
    {
        var seances = await service.GetByTypeAsync(type);

        return View(seances);
    }

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

    public async Task<IActionResult> Details(int id)
    {
        var seance = await service.GetByIdAsync(id);
        if (seance == null) return NotFound();

        return View(seance);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var seance = await service.GetByIdAsync(id);
        if (seance == null) return NotFound();

        return PartialView("Partials/Edit", seance);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
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

    public async Task<IActionResult> Delete(int id)
    {
        var dossier = await service.GetByIdAsync(id);
        if (dossier == null) return NotFound();

        return PartialView("Partials/Delete", dossier);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var seance = await service.GetByIdAsync(id);
        if (seance == null) return NotFound();

        await service.SupprimerSeanceAsync(id);

        return RedirectToAction(nameof(Index), new { type = seance.Type });
    }
}