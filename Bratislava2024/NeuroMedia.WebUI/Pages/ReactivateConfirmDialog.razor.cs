using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using NeuroMedia.WebUI.Models;
using NeuroMedia.WebUI.Services.Interfaces;

namespace NeuroMedia.WebUI.Pages
{
    public partial class ReactivateConfirmDialog
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
        public EventCallback<int> OnPatientReactivated { get; set; }


        private async Task OnReactivate()
        {
            var response = await HttpClientService.PutEmptyAsync($"/api/Patients/reactivate/{PatientId}");

            if (response)
            {
                await JSRuntime.InvokeVoidAsync("closeModal", "reactivateModal");
                await OnPatientReactivated.InvokeAsync(PatientId);
            }
            else
            {
                Console.WriteLine($"Failed to reactivate patient.");
            }

            await JSRuntime.InvokeVoidAsync("closeModal", "reactivateModal");
        }

        private async Task OnCancel()
        {
            await JSRuntime.InvokeVoidAsync("closeModal", "reactivateModal");
        }
    }
}
