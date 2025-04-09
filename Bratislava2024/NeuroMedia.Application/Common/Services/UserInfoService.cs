using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using NeuroMedia.Application.Common.Helpers;
using NeuroMedia.Application.Interfaces.Services;
using NeuroMedia.Domain.Entities;
using NeuroMedia.Application.Logging;
using NeuroMedia.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace NeuroMedia.Application.Common.Services
{
    public class UserInfoService(IUnitOfWork unitOfWork, ILogger<UserInfoService> logger) : IUserInfoService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<UserInfoService> _logger = logger;

        private static readonly Dictionary<string, UserInfo> s_dictionary = [];

        public async Task<UserInfo?> GetUserInfoAsync(ClaimsPrincipal userClaimsPrincipal)
        {
            var oid = userClaimsPrincipal.GetUserId();

            if (!s_dictionary.TryGetValue(oid, out var userInfo))
            {
                userInfo = await _unitOfWork.Repository<UserInfo>().Entities
                                .FirstOrDefaultAsync(x => x.UserOid == oid);
                userInfo ??= await _unitOfWork.Repository<UserInfo>().Entities
                                .FirstOrDefaultAsync(x => x.Email == userClaimsPrincipal.GetEmail());
            }

            return userInfo;
        }

        public async Task SetUserInfoAsync(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken = default)
        {
            var oid = userClaimsPrincipal.GetUserId();

            if (string.IsNullOrEmpty(oid))
            {
                _logger.LogWarningAndThrow404(new LogRow().UpdateCallerProperties(), "No oid in JWT token claims found");
            }

            var actualUserInfo = MapToUserInfo(userClaimsPrincipal);
            actualUserInfo = await ActualizeDbData(actualUserInfo, userClaimsPrincipal, cancellationToken);
            UpdateDictionary(oid, actualUserInfo);
        }

        private async Task<UserInfo> ActualizeDbData(UserInfo actualUserInfo, ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken = default)
        {
            var userInfo = await GetUserInfoAsync(userClaimsPrincipal);

            if (userInfo == null)
            {
                actualUserInfo = await _unitOfWork.Repository<UserInfo>().AddAsync(actualUserInfo);
                await _unitOfWork.SaveAsync(cancellationToken);
            }
            else if (!actualUserInfo.Equals(userInfo))
            {
                actualUserInfo.Id = userInfo!.Id;
                await _unitOfWork.Repository<UserInfo>().UpdateAsync(actualUserInfo);
                await _unitOfWork.SaveAsync(cancellationToken);
            }

            return actualUserInfo;
        }

        private static void UpdateDictionary(string oid, UserInfo actualUserInfo)
        {
            if (!s_dictionary.ContainsKey(oid))
            {
                s_dictionary.TryAdd(oid, actualUserInfo);
            }
            else
            {
                s_dictionary[oid] = actualUserInfo;
            }
        }

        private static UserInfo MapToUserInfo(ClaimsPrincipal userClaimsPrincipal)
        {
            return new UserInfo
            {
                UserOid = userClaimsPrincipal.GetUserId(),
                TenantId = userClaimsPrincipal.GetTenantId(),
                Name = userClaimsPrincipal.GetUserName().Split(" ").First(),
                Surname = userClaimsPrincipal.GetUserName().Split(" ").Last(),
                Email = userClaimsPrincipal.GetEmail(),
                Institution = userClaimsPrincipal.GetTenantId(),
                Roles = userClaimsPrincipal.GetRoles(),
            };
        }
    }
}
