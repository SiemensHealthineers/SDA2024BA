using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using NeuroMedia.WebUI.Models;
using NeuroMedia.WebUI.Services.Interfaces;

namespace NeuroMedia.WebUI.Pages
{
    public partial class DeleteConfirmDialog
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
        public EventCallback<int> OnPatientDeleted { get; set; }


        private async Task OnDelete()
        {
            var response = await HttpClientService.DeleteAsync($"/api/patients/{PatientId}");

            if (response)
            {
                await JSRuntime.InvokeVoidAsync("closeModal", "deleteModal");
                await OnPatientDeleted.InvokeAsync(PatientId);
            }
            else
            {
                Console.WriteLine($"Failed to delete patient.");
            }

            await JSRuntime.InvokeVoidAsync("closeModal", "deleteModal");
        }

        private async Task OnCancel()
        {
            await JSRuntime.InvokeVoidAsync("closeModal", "deleteModal");
        }
    }
}
