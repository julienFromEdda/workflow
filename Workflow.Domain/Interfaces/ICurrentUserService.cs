using Workflow.Domain.Entities;

namespace Workflow.Domain.Interfaces;

public interface ICurrentUserService
{
    Task<Utilisateur?> GetUserAsync();
    bool HasPermission(string permission);
}