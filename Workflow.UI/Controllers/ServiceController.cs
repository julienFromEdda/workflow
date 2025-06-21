using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workflow.Domain.Entities;
using Workflow.Domain.Exceptions;
using Workflow.Domain.Interfaces;
using Workflow.Domain.Security;

namespace Workflow.UI.Controllers;

[Authorize(Policy = Permissions.Utilisateur.Gerer)]
public class ServiceController(IServiceService svc) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await svc.GetAllAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var service = await svc.GetByIdAsync(id);

            return View(service);
        }
        catch (ApiException ex)
        {
            return NotFound(ex.Message);
        }
    }

    public IActionResult Create()
    {
        return PartialView("Partials/Create", new Service());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Service service)
    {
        if (ModelState.IsValid)
        {
            await svc.CreateAsync(service);

            return RedirectToAction(nameof(Index));
        }

        return PartialView("Partials/Create", service);
    }

    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            return PartialView("Partials/Edit", await svc.GetByIdAsync(id));
        }
        catch (ApiException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Service service)
    {
        try
        {
            if (id != service.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                await svc.UpdateAsync(service);

                return RedirectToAction(nameof(Details), new { id });
            }

            return PartialView("Partials/Edit", service);
        }
        catch (ApiException ex)
        {
            return NotFound(ex.Message);
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            return PartialView("Partials/Delete", await svc.GetByIdAsync(id));
        }
        catch (ApiException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await svc.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
        catch (ApiException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
