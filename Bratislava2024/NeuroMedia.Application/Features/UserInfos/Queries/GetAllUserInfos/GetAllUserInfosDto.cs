using NeuroMedia.Application.Common.Mappings;
using NeuroMedia.Domain.Entities;

namespace NeuroMedia.Application.Features.UserInfos.Queries.GetAllUserInfos
{
    public class GetAllUserInfosDto : IMapFrom<UserInfo>
    {
        public int Id { get; init; }
        public string UserOid { get; init; } = default!;
        public string TenantId { get; init; } = default!;
        public string Name { get; init; } = default!;
        public string Surname { get; init; } = default!;
        public string Email { get; init; } = default!;
        public string Institution { get; init; } = default!;
        public string Roles { get; init; } = default!;
    }
}
