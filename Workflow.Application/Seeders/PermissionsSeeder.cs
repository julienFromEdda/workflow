using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Workflow.Domain.Entities;
using Workflow.Domain.Security;

namespace Workflow.Application.Seeders;

public static class PermissionSeeder
{
    public static async Task SeedRolePermissionsAsync(RoleManager<Role> roleManager)
    {
        var rolePermissions = Permissions.GetRolePermissions();

        foreach (var rolePair in rolePermissions)
        {
            var roleName = rolePair.Key;
            var permissions = rolePair.Value;

            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null) continue;

            var existingClaims = await roleManager.GetClaimsAsync(role);
            foreach (var permission in permissions)
            {
                if (!existingClaims.Any(c => c.Type == "permission" && c.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim("permission", permission));
                }
            }
        }
    }
}
