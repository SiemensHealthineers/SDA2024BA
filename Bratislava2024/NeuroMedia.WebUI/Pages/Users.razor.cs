using Microsoft.AspNetCore.Components;

using System.Net.Http.Json;

using static System.Net.Mime.MediaTypeNames;

using NeuroMedia.Application.Features.UserInfos.Queries.GetAllUserInfos;
using NeuroMedia.Application.Features.Users.Queries.GetAllUsers;
using NeuroMedia.WebUI.Services;
using NeuroMedia.WebUI.Services.Interfaces;

namespace NeuroMedia.WebUI.Pages
{
    public partial class Users : ComponentBase
    {
        [Inject]
        private IHttpClientService HttpClientService { get; set; } = default!;

        private List<GetAllUserInfosDto> UsersList;

        protected override async Task OnInitializedAsync()
        {
            UsersList = await HttpClientService.GetListAsync<GetAllUserInfosDto>("/api/userinfos");

        }
    }
}
