// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Workflow.Domain.Entities;

namespace Workflow.UI.Areas.Identity.Pages.Account;

public class LogoutModel(SignInManager<Utilisateur> signInManager) : PageModel
{
    public async Task<IActionResult> OnPost(string returnUrl = null)
    {
        await signInManager.SignOutAsync();

        return LocalRedirect("/");
    }
}
