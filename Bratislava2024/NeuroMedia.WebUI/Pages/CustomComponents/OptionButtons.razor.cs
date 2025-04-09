using Microsoft.AspNetCore.Components;

using System.Net.Http.Json;

using NeuroMedia.Application.Features.Patients.Queries.GetPatientById;
using Microsoft.JSInterop;
using NeuroMedia.WebUI.Services.Interfaces;

namespace NeuroMedia.WebUI.Pages.CustomComponents
{

    public partial class OptionButtons : ComponentBase
    {
        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        [Parameter]
        public int PatientId { get; set; }
        [Parameter]
        public bool IsActive { get; set; }

        private async Task ShowDeactivateConfirmationModal()
        {
            await JSRuntime.InvokeVoidAsync("ShowModal", "deactivateModal");
        }

        private async Task ShowReactivateConfirmationModal()
        {
            await JSRuntime.InvokeVoidAsync("ShowModal", "reactivateModal");
        }

        private async Task ShowDeleteConfirmationModal()
        {
            await JSRuntime.InvokeVoidAsync("ShowModal", "deleteModal");
        }

        private async Task CreateEditPatientModal()
        {
            if (JSRuntime == null)
            {
                throw new InvalidOperationException("JSRuntime is not initialized.");
            }
            await JSRuntime.InvokeVoidAsync("ShowModal", "editPatientModal");
        }
    }
}
