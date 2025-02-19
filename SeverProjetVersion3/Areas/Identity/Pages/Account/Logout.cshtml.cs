﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Threading.Tasks;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SeverProjetVersion3.Models;

namespace SeverProjetVersion3.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<UtilisateurDaplication> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly IIdentityServerInteractionService _interaction;

        public LogoutModel(SignInManager<UtilisateurDaplication> signInManager,
            ILogger<LogoutModel> logger,
            IIdentityServerInteractionService interactionS)
        {
            _signInManager = signInManager;
            _logger = logger;
            _interaction = interactionS;
        }

        public async Task<IActionResult> OnGet(string returnUrl = null)
        {
            return await OnPost(returnUrl);
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");


            //permet de recuperer l'id de la page de logout
            string logoutId = Request.Query["logoutId"].ToString();

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }

            if (!string.IsNullOrEmpty(logoutId))
            {
                // Récupère l'url vers laquelle rediriger après déconnexion
                var req = await _interaction.GetLogoutContextAsync(logoutId);
                returnUrl = req.PostLogoutRedirectUri;

                // Redirige vers cette url si elle existe
                if (!string.IsNullOrEmpty(returnUrl)) return Redirect(returnUrl);
            }

         
            return Page();
            
        }
    }
}
