using System.ComponentModel.DataAnnotations;

namespace Workflow.Domain.Extensions;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum value)
    {
        return value.GetType()
                    .GetMember(value.ToString())
                    .First()
                    .GetCustomAttributes(false)
                    .OfType<DisplayAttribute>()
                    .FirstOrDefault()?.Name ?? value.ToString();
    }
}
