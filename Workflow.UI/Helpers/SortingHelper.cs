using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Text.Encodings.Web;

namespace Workflow.UI.Helpers;

public static class SortingHelper
{
    public static IHtmlContent SortableColumn(
        this IHtmlHelper html,
        string label,
        string champ,
        ViewContext viewContext)
    {
        var currentTri = viewContext.HttpContext.Request.Query["TriPar"];
        var currentDesc = viewContext.HttpContext.Request.Query["TriDesc"] == "true";
        var recherche = viewContext.HttpContext.Request.Query["Recherche"];
        var page = viewContext.HttpContext.Request.Query["Page"];

        var isCurrent = currentTri == champ;
        var newTriDesc = isCurrent ? (!currentDesc).ToString().ToLower() : "false";

        var urlHelperFactory = viewContext.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();
        var urlHelper = urlHelperFactory.GetUrlHelper(viewContext);
        var url = urlHelper.Action("Index", new RouteValueDictionary {
            { "Recherche", recherche },
            { "TriPar", champ },
            { "TriDesc", newTriDesc },
            { "Page", 1 }
        });

        var span = new TagBuilder("span");
        span.Attributes["role"] = "button";
        span.Attributes["data-url"] = url!;
        span.Attributes["title"] = $"Trier par {label.ToLower()}";
        span.Attributes["style"] = "cursor:pointer;";
        span.AddCssClass("sortable");

        var icon = "";
        if (isCurrent)
        {
            icon = currentDesc
                ? "<i class='bi bi-arrow-down ms-1'></i>"
                : "<i class='bi bi-arrow-up ms-1'></i>";
        }

        span.InnerHtml.AppendHtml($"{label} {icon}");

        var writer = new StringWriter();
        span.WriteTo(writer, HtmlEncoder.Default);
        return new HtmlString(writer.ToString());
    }
}
