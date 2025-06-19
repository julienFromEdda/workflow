using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Workflow.Domain.Entities;
using Workflow.Domain.Security;
using Workflow.UI.Models;

namespace Workflow.UI.Controllers;

[Authorize(Policy = Permissions.Utilisateur.Gerer)]
public class RoleController(RoleManager<Role> roleManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var roles = roleManager.Roles.ToList();
        var allPerms = Permissions.GetRolePermissions().SelectMany(kv => kv.Value).Distinct().ToList();

        var matrix = new Dictionary<string, Dictionary<string, bool>>();

        foreach (var role in roles)
        {
            var claims = await roleManager.GetClaimsAsync(role);
            matrix[role.Name!] = allPerms.ToDictionary(
                perm => perm,
                perm => claims.Any(c => c.Type == "permission" && c.Value == perm));
        }

        var grouped = allPerms
            .GroupBy(p => p.Split('.')[1])
            .ToDictionary(g => g.Key, g => g.OrderBy(p => p).ToList());

        var vm = new RolePermissionsViewModel
        {
            Matrix = matrix,
            GroupedPermissions = grouped
        };

        return View(vm);
    }

    public IActionResult Create()
    {
        return PartialView("Partials/Create", new Role());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Role role)
    {
        if (ModelState.IsValid)
        {
            await roleManager.CreateAsync(role);

            return RedirectToAction(nameof(Index));
        }
        return PartialView("Partials/Create", role);
    }

    [HttpPost]
    public async Task<IActionResult> UpdatePermission(string roleName, string permission, bool value)
    {
        var role = await roleManager.FindByNameAsync(roleName);
        if (role == null) return NotFound();

        if (role.Name == "SuperAdmin" && permission == Permissions.Utilisateur.Gerer && !value)
        {
            TempData["Error"] = "Impossible de retirer la gestion des utilisateurs au rôle SuperAdmin.";
            return RedirectToAction(nameof(Index));
        }

        var claims = await roleManager.GetClaimsAsync(role);
        var hasClaim = claims.Any(c => c.Type == "permission" && c.Value == permission);

        if (value && !hasClaim)
            await roleManager.AddClaimAsync(role, new Claim("permission", permission));
        else if (!value && hasClaim)
            await roleManager.RemoveClaimAsync(role, new Claim("permission", permission));

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string roleName)
    {
        if (roleName == "SuperAdmin")
        {
            TempData["Error"] = "Le rôle SuperAdmin ne peut pas être supprimé.";
            return RedirectToAction(nameof(Index));
        }

        var role = await roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            TempData["Error"] = "Rôle introuvable.";
            return RedirectToAction(nameof(Index));
        }

        await roleManager.DeleteAsync(role);
        TempData["Success"] = $"Le rôle {roleName} a été supprimé.";
        return RedirectToAction(nameof(Index));
    }
}
