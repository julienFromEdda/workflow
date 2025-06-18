using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Workflow.Domain.Entities;
using Workflow.Domain.Exceptions;
using Workflow.Domain.Interfaces;
using Workflow.Domain.Security;
using Workflow.UI.Filters;

namespace Workflow.UI.Controllers;

[Authorize(Policy = Permissions.Dossier.Acces)]
public class DossierController(IDossierService service) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await service.GetAllAsync());
    }


    public IActionResult Create()
    {
        return PartialView("Partials/Create", new Dossier());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Dossier dossier)
    {
        if (ModelState.IsValid)
        {
            await service.CreerDossierAsync(dossier);

            return RedirectToAction(nameof(Index));
        }
        return PartialView("Partials/Create", dossier);
    }

    [HasAccessToDossier]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            return View(await service.GetByIdAsync(id));
        }
        catch (ApiException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HasAccessToDossier]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var dossier = await service.GetByIdAsync(id);

            return PartialView("Partials/Edit", dossier);
        }
        catch (ApiException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [HasAccessToDossier]
    public async Task<IActionResult> Edit(int id, Dossier dossier)
    {
        if (id != dossier.Id) return BadRequest();

        if (ModelState.IsValid)
        {
            try
            {
                await service.ModifierDossierAsync(dossier);

                return RedirectToAction(nameof(Details), new { id = dossier.Id });
            }
            catch (ApiException ex)
            {
                return NotFound(ex.Message);
            }
        }
        return PartialView("Partials/Edit", dossier);
    }

    [HasAccessToDossier]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            return PartialView("Partials/Delete", await service.GetByIdAsync(id));
        }
        catch (ApiException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [HasAccessToDossier]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await service.SupprimerDossierAsync(id);

            return RedirectToAction(nameof(Index));
        }
        catch (ApiException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HasAccessToDossier]
    public async Task<IActionResult> Transfert(int id)
    {
        try
        {
            var services = await service.GetAvailableServiceForTransfert(id);

            ViewBag.dossierId = id;
            ViewBag.Services = services
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Nom })
                .ToList();

            return PartialView("Partials/Transfert");
        }
        catch (ApiException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [HasAccessToDossier]
    public async Task<IActionResult> Transfert(int id, int nouveauServiceId)
    {
        try
        {
            await service.Transfert(id, nouveauServiceId);

            return RedirectToAction(nameof(Index));
        }
        catch (ApiException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    [HasAccessToDossier]
    public async Task<IActionResult> Archiver(int id)
    {
        try
        {
            return PartialView("Partials/Archiver", await service.GetByIdAsync(id));
        }
        catch (ApiException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [HasAccessToDossier]
    public async Task<IActionResult> Archiver(int id, object? _ = null)
    {
        try
        {
            await service.Archive(id);

            return RedirectToAction(nameof(Details), new { id });
        }
        catch (ApiException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    [HasAccessToDossier]
    public async Task<IActionResult> Assigner(int id)
    {
        try
        {
            var utilisateurs = await service.GetAvailableUserForAssign(id);

            ViewBag.DossierId = id;
            ViewBag.Utilisateurs = utilisateurs
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.Nom + " " + u.Prenom
                })
                .ToList();

            return PartialView("Partials/Assigner");
        }
        catch (ApiException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [HasAccessToDossier]
    public async Task<IActionResult> Assigner(int id, string nouvelEmployeId)
    {
        try
        {
            await service.Assign(id, nouvelEmployeId);

            return RedirectToAction("Details", new { id });
        }
        catch (ApiException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
