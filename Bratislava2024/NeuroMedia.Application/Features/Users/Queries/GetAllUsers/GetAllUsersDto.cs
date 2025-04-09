using AutoMapper;

using NeuroMedia.Application.Common.Mappings;
using NeuroMedia.Domain.Entities;

namespace NeuroMedia.Application.Features.Users.Queries.GetAllUsers
{
    public class GetAllUsersDto
    {
        public string Id { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string Institution { get; set; } = default!;
        public string Roles { get; set; } = default!;
    }
}
