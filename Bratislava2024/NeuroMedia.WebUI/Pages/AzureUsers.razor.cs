using Microsoft.AspNetCore.Components;

using System.Net.Http.Json;

using static System.Net.Mime.MediaTypeNames;

using NeuroMedia.Application.Features.Users.Queries.GetAllUsers;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using NeuroMedia.WebUI.Services.Interfaces;

namespace NeuroMedia.WebUI.Pages
{
    public partial class AzureUsers : ComponentBase
    {
        [Inject]
        private IHttpClientService HttpClientService { get; set; } = default!;

        public List<GetAllUsersDto> UsersList { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            UsersList = await HttpClientService.GetListAsync<GetAllUsersDto>("/api/users");
        }
    }
}
