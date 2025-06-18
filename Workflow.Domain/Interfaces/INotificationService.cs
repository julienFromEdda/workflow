using Workflow.Domain.Entities;

namespace Workflow.Domain.Interfaces;

public interface INotificationService
{
    Task<Notification> CreerNotificationAsync(Notification notification);
    Task<IEnumerable<Notification>> GetNotificationsByUserAsync(string utilisateurId);
    Task MarquerCommeLuAsync(int notificationId);
    Task NotifierUtilisateurAsync(string? ancienEmployeId, string message);
}
