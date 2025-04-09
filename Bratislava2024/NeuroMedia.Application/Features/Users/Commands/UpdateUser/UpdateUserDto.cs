using AutoMapper;

using NeuroMedia.Application.Common.Mappings;
using NeuroMedia.Domain.Entities;

namespace NeuroMedia.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserDto
    {
        public string Id { get; set; } = default!;
        public string GivenName { get; set; } = default!;
        public string Surname { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string Institution { get; set; } = default!;
        public string Roles { get; set; } = default!;
        public string Scopes { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}
