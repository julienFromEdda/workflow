using Microsoft.AspNetCore.Authorization;
using Workflow.Domain.Security;

namespace Workflow.UI.Security;

public static class PermissionPolicies
{
    public static void AddPermissionPolicies(this AuthorizationOptions options)
    {
        var allPermissions = Permissions.GetRolePermissions()
            .SelectMany(kv => kv.Value)
            .Distinct();

        foreach (var permission in allPermissions)
        {
            options.AddPolicy(permission, policy =>
                policy.RequireClaim("permission", permission));
        }
    }
}