using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using NeuroMedia.Application.Logging;
using NeuroMedia.Domain.Entities;

namespace NeuroMedia.Application.Interfaces.Services
{
    public interface IUserInfoService
    {
        public Task SetUserInfoAsync(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken = default);
        public Task<UserInfo?> GetUserInfoAsync(ClaimsPrincipal userClaimsPrincipal);
    }
}
