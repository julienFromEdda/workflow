namespace Workflow.UI.Models;

public class RolePermissionsViewModel
{
    public Dictionary<string, Dictionary<string, bool>> Matrix { get; set; } = new();
    public Dictionary<string, List<string>> GroupedPermissions { get; set; } = new();
}
