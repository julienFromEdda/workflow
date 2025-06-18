using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Workflow.Domain.Entities;
using Workflow.Domain.Enums;
using Workflow.Domain.Interfaces;

namespace Workflow.UI.Filters;

public class HasAccessToDossierAttribute : TypeFilterAttribute
{
    public HasAccessToDossierAttribute() : base(typeof(HasAccessToDossierFilter)) { }

    private class HasAccessToDossierFilter(IDossierService dossierService, UserManager<Utilisateur> userManager) : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ActionArguments.TryGetValue("id", out var idObj) || idObj is not int id)
            {
                context.Result = new BadRequestResult();
                return;
            }

            var user = await userManager.GetUserAsync(context.HttpContext.User);
            if (user == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            var dossier = await dossierService.GetByIdAsync(id);
            if (dossier == null || dossier.ServiceTraitantId != user.ServiceId || dossier.Statut == StatutDossier.Supprime)
            {
                context.Result = new ForbidResult();
                return;
            }

            await next();
        }
    }
}
