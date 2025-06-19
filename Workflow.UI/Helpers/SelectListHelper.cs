using Microsoft.AspNetCore.Mvc.Rendering;
using Workflow.Domain.Extensions;

namespace Workflow.UI.Helpers;

public static class SelectListHelper
{
    public static IEnumerable<SelectListItem> EnumToSelectList<TEnum>(TEnum? selected = null) where TEnum : struct, Enum
    {
        return Enum.GetValues(typeof(TEnum))
            .Cast<TEnum>()
            .Select(e => new SelectListItem
            {
                Text = (e as Enum).GetDisplayName(),
                Value = e.ToString(),
                Selected = selected.HasValue && EqualityComparer<TEnum>.Default.Equals(selected.Value, e)
            });
    }

    public static SelectList EnumToSelectListExcluding<TEnum>(TEnum selected, params TEnum[] exclude) where TEnum : Enum
    {
        var items = Enum.GetValues(typeof(TEnum))
            .Cast<TEnum>()
            .Where(e => !exclude.Contains(e))
            .Select(e => new SelectListItem
            {
                Value = e.ToString(),
                Text = (e as Enum).GetDisplayName(),
                Selected = e.Equals(selected)
            });

        return new SelectList(items, "Value", "Text", selected);
    }
}
