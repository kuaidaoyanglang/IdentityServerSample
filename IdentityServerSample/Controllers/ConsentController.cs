using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServerSample.Models;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServerSample.Controllers
{
    public class ConsentController : Controller
    {
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;
        public ConsentController(IClientStore clientStore, IResourceStore resourceStore,IIdentityServerInteractionService identityServerInteractionService)
        {
            _clientStore = clientStore;
            _resourceStore = resourceStore;
            _identityServerInteractionService = identityServerInteractionService;
        }

        private async Task<ConsentViewModel> BuildConcentViewModel(string returnUrl)
        {
            var request = await _identityServerInteractionService.GetAuthorizationContextAsync(returnUrl);
            if (request == null)
            {
                return null;
            }

            var client = await _clientStore.FindEnabledClientByIdAsync(request.ClientId);
            var resource = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);
            return CreateConcentViewModel(request, client, resource);
        }

        private ConsentViewModel CreateConcentViewModel(AuthorizationRequest request,Client client,Resources resources)
        {
            var vm = new ConsentViewModel();
            vm.ClientName = client.ClientName;
            vm.ClientLogoUrl = client.LogoUri;
            vm.ClientUrl = client.ClientUri;
            vm.AllowRememberConsent = client.AllowRememberConsent;

            vm.IdentityResources = resources.IdentityResources.Select(i=>CreateScopeViewModel(i));

            vm.ApiResources = resources.ApiResources.SelectMany(i=>i.Scopes).Select(i=>CreateScopeViewModel(i));

            return vm;
        }

        private ScopeViewModel CreateScopeViewModel(IdentityResource identityResource)
        {
            return new ScopeViewModel
            {
                Name = identityResource.Name,
                DisplayName = identityResource.DisplayName,
                Description = identityResource.Description,
                Required = identityResource.Required,
                Checked = identityResource.Required,
                Emphasize = identityResource.Emphasize
            };
        }

        private ScopeViewModel CreateScopeViewModel(Scope scope)
        {
            return new ScopeViewModel
            {
                Name = scope.Name,
                DisplayName = scope.DisplayName,
                Description = scope.Description,
                Required = scope.Required,
                Checked = scope.Required,
                Emphasize = scope.Emphasize
            };
        }

        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl)
        {
            var model = await BuildConcentViewModel(returnUrl);

            if (model==null)
            {
                
            }
            
            return View(model);
        }
    }
}