using AutoMapper;

using NeuroMedia.Application.Common.Mappings;
using NeuroMedia.Application.Features.Users.Commands.UpdateUser;
using NeuroMedia.Domain.Entities;

namespace NeuroMedia.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserDto
    {
        public string GivenName { get; set; } = default!;
        public string Surname { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string Password { get; set; } = default!;
        public bool AccountEnabled { get; set; } = default!;
        public string Institution { get; set; } = default!;
        public string Roles { get; set; } = default!;
        public string Scopes { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}
