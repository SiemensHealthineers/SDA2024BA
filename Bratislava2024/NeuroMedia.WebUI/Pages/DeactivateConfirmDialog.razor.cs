using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using NeuroMedia.WebUI.Models;
using NeuroMedia.WebUI.Services.Interfaces;

namespace NeuroMedia.WebUI.Pages
{
    public partial class DeactivateConfirmDialog
    {
        [Inject]
        private IHttpClientService HttpClientService { get; set; } = default!;
        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;
        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;
        [Parameter]
        public int PatientId { get; set; }
        [Parameter]
        public EventCallback<int> OnPatientDeactivated { get; set; }


        private async Task OnDeactivate()
        {
            var response = await HttpClientService.PutEmptyAsync($"/api/Patients/deactivate/{PatientId}");

            if (response)
            {
                await JSRuntime.InvokeVoidAsync("closeModal", "deactivateModal");
                await OnPatientDeactivated.InvokeAsync(PatientId);
            }
            else
            {
                Console.WriteLine($"Failed to deactivate patient.");
            }

            await JSRuntime.InvokeVoidAsync("closeModal", "deactivateModal");
        }

        private async Task OnCancel()
        {
            await JSRuntime.InvokeVoidAsync("closeModal", "deactivateModal");
        }
    }
}
