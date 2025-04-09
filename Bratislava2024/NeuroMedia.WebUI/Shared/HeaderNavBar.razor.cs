using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Graph.Admin.Edge.InternetExplorerMode.SiteLists.Item.Publish;
using Microsoft.Graph.Solutions.VirtualEvents.Webinars.GetByUserIdAndRoleWithUserIdWithRole;
using Microsoft.JSInterop;
using NeuroMedia.Application.Features.Patients.Queries.GetPatientById;
using NeuroMedia.WebUI.Services.Interfaces;

using System.Net.Http.Json;

namespace NeuroMedia.WebUI.Shared
{
    public partial class HeaderNavBar : ComponentBase
    {
        [Inject]
        private IHttpClientService HttpClientService { get; set; } = default!;

        [Inject]
        private SignOutSessionStateManager SignOutManager { get; set; } = default!;

        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        public string Role { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            await LoadUserRoleAsync();

            AuthenticationStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;
        }

        private async Task LoadUserRoleAsync()
        {
            Role = await HttpClientService.GetUserRole();
            StateHasChanged(); 
        }

        private async void OnAuthenticationStateChanged(Task<AuthenticationState> task)
        {
            await LoadUserRoleAsync();
        }

        protected void BeginSignIn()
        {
            Navigation.NavigateTo("authentication/login");
        }

        protected async Task BeginSignOut(MouseEventArgs args)
        {
            await SignOutManager.SetSignOutState();
            Navigation.NavigateTo("authentication/logout");
        }

        public void Dispose()
        {
            AuthenticationStateProvider.AuthenticationStateChanged -= OnAuthenticationStateChanged;
        }
    }
}
