using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Workflow.Domain.Entities;
using Workflow.Domain.Security;

namespace Workflow.UI.Controllers
{
    [Authorize(Policy = Permissions.Utilisateur.Gerer)]
    public class UserManagementController(UserManager<Utilisateur> userManager, RoleManager<Role> roleManager) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var users = await userManager.Users
                .Include(u => u.Service)
                .ToListAsync();
            
            // Get roles for each user
            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                ViewData[$"Roles_{user.Id}"] = roles;
            }

            return View(users);
        }

        [HttpGet("/UserManagement/Edit/{userId}")]
        public async Task<IActionResult> Edit(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var model = new
            {
                UserId = user.Id,
                SelectedRoles = await userManager.GetRolesAsync(user),
                AvailableRoles = (await roleManager.Roles.ToListAsync())
                    .Select(r => r.Name)
                    .ToList()
            };

            return PartialView("Partials/Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string userId, string[] selectedRoles)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Get current roles
            var currentRoles = await userManager.GetRolesAsync(user);
            var allRoles = await roleManager.Roles.ToListAsync();

            // Check if user is trying to remove admin role from another admin
            if (currentRoles.Contains("SuperAdmin") && selectedRoles != null && !selectedRoles.Contains("SuperAdmin") && 
                allRoles.Any(r => r.Name == "SuperAdmin"))
            {
                ModelState.AddModelError("", "Vous ne pouvez pas retirer le rôle d'administrateur à un autre administrateur.");
                return PartialView("Partials/Edit", new
                {
                    UserId = userId,
                    SelectedRoles = currentRoles,
                    AvailableRoles = allRoles.Select(r => r.Name).ToList()
                });
            }

            // Remove all current roles
            var removeResult = await userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Erreur lors de la suppression des rôles actuels.");
                return PartialView("Partials/Edit", new
                {
                    UserId = userId,
                    SelectedRoles = currentRoles,
                    AvailableRoles = allRoles.Select(r => r.Name).ToList()
                });
            }

            // Add selected roles
            var addResult = selectedRoles != null ? await userManager.AddToRolesAsync(user, selectedRoles) : IdentityResult.Success;
            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Erreur lors de l'ajout des nouveaux rôles.");
                return PartialView("Partials/Edit", new
                {
                    UserId = userId,
                    SelectedRoles = currentRoles,
                    AvailableRoles = allRoles.Select(r => r.Name).ToList()
                });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
