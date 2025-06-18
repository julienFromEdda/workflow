using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Workflow.Domain.Entities;
using Workflow.Domain.Interfaces;

namespace Workflow.UI.Controllers;

[Authorize]
public class NotificationController(INotificationService notificationService, UserManager<Utilisateur> userManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var user = await userManager.GetUserAsync(User);
        var notifications = await notificationService.GetNotificationsByUserAsync(user!.Id);
        return View(notifications.OrderByDescending(n => n.Id));
    }

    [HttpPost]
    public async Task<IActionResult> MarquerCommeLu(int id)
    {
        await notificationService.MarquerCommeLuAsync(id);
        return RedirectToAction(nameof(Index));
    }
}