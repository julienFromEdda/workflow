using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Workflow.Domain.Entities;
using Workflow.Domain.Security;

namespace Workflow.UI.Controllers;

[Authorize(Policy = Permissions.Utilisateur.Gerer)]
public class RoleController(RoleManager<Role> roleManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var roles = roleManager.Roles.ToList();
        var allPermissions = Permissions.GetRolePermissions().SelectMany(kv => kv.Value).Distinct().ToList();

        var matrix = new Dictionary<string, Dictionary<string, bool>>();

        foreach (var role in roles)
        {
            var claims = await roleManager.GetClaimsAsync(role);
            var permMap = new Dictionary<string, bool>();

            foreach (var perm in allPermissions)
            {
                permMap[perm] = claims.Any(c => c.Type == "permission" && c.Value == perm);
            }

            matrix[role.Name!] = permMap;
        }

        ViewBag.Matrix = matrix;

        return View();
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
            //await service.CreerDossierAsync(dossier);

            return RedirectToAction(nameof(Index));
        }
        return PartialView("Partials/Create", role);
    }

    //[HttpGet("{roleName}/permissions")]
    //public async Task<IActionResult> GetPermissions(string roleName)
    //{
    //    var role = await roleManager.FindByNameAsync(roleName);
    //    if (role == null) return NotFound();

    //    var claims = await roleManager.GetClaimsAsync(role);
    //    var allPermissions = Permissions.GetRolePermissions().SelectMany(kv => kv.Value).Distinct();
    //    var result = allPermissions.ToDictionary(
    //        perm => perm,
    //        perm => claims.Any(c => c.Type == "permission" && c.Value == perm));

    //    return Ok(result);
    //}

    //[HttpPost("{roleName}/permissions")]
    //public async Task<IActionResult> UpdatePermissions(string roleName, [FromBody] Dictionary<string, bool> permissions)
    //{
    //    var role = await roleManager.FindByNameAsync(roleName);
    //    if (role == null) return NotFound();

    //    var existingClaims = await roleManager.GetClaimsAsync(role);

    //    foreach (var perm in permissions)
    //    {
    //        var hasClaim = existingClaims.Any(c => c.Type == "permission" && c.Value == perm.Key);
    //        if (perm.Value && !hasClaim)
    //            await roleManager.AddClaimAsync(role, new Claim("permission", perm.Key));
    //        else if (!perm.Value && hasClaim)
    //            await roleManager.RemoveClaimAsync(role, new Claim("permission", perm.Key));
    //    }

    //    return NoContent();
    //}
}
