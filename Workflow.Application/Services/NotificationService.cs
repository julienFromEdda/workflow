using Microsoft.EntityFrameworkCore;
using Workflow.Domain.Entities;
using Workflow.Domain.Interfaces;
using Workflow.Persistence;

namespace Workflow.Application.Services
{
    public class NotificationService(WFContext context, ICurrentUserService currentUser) : INotificationService
    {
        public async Task<Notification> CreerNotificationAsync(Notification notification)
        {
            context.Notifications.Add(notification);
            await context.SaveChangesAsync();
            return notification;
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserAsync(string utilisateurId)
        {
            return await context.Notifications
                .Where(n => n.UtilisateurId == utilisateurId)
                .ToListAsync();
        }

        public async Task MarquerCommeLuAsync(int notificationId)
        {
            var notif = await context.Notifications.FindAsync(notificationId);
            if (notif != null)
            {
                notif.Lu = true;
                await context.SaveChangesAsync();
            }
        }

        public async Task NotifierUtilisateurAsync(string? ancienEmployeId, string message)
        {
            var user = await currentUser.GetUserAsync();

            if (user?.Id == ancienEmployeId) 
                return;

            var notification = new Notification
            {
                UtilisateurId = ancienEmployeId,
                Message = message
            };

            await CreerNotificationAsync(notification);
        }
    }
}
