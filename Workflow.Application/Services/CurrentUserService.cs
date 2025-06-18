using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Workflow.Domain.Entities;
using Workflow.Domain.Interfaces;

namespace Workflow.Application.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor, UserManager<Utilisateur> userManager) : ICurrentUserService
{
    public bool HasPermission(string permission) => httpContextAccessor.HttpContext?.User?.HasClaim("permission", permission) ?? false;
    public async Task<Utilisateur?> GetUserAsync()
    {
        var principal = httpContextAccessor.HttpContext?.User;
        return principal == null ? null : await userManager.GetUserAsync(principal);
    }
}