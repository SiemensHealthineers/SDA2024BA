using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using NeuroMedia.API.Policies;
using NeuroMedia.Application.Features.Users.Commands.CreateUser;
using NeuroMedia.Application.Features.Users.Commands.DeleteUser;
using NeuroMedia.Application.Features.Users.Commands.UpdateUser;
using NeuroMedia.Application.Features.Users.Queries.GetAllUsers;
using NeuroMedia.Application.Features.Users.Queries.GetUserById;
using NeuroMedia.Application.Logging;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.API.Controllers
{
    [Route("api/[controller]")]
    public class UsersController(IMediator mediator, IConfiguration configuration, ILogger<UsersController> logger) : ApiController
    {
        private readonly IMediator _mediator = mediator;
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<UsersController> _logger = logger;

        [HttpGet]
        [CustomRoleAuthorize(Roles.UserManagementGroup)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _mediator.Send(new GetAllUsersQuery(GetUserWithCustomClaims(Request, _configuration, _logger))));
        }

        [HttpGet("{id:int}")]
        [CustomRoleAuthorize(Roles.UserManagementGroup)]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await _mediator.Send(new GetUserByIdQuery(id, new LogRow().UpdateCallerProperties())));
        }

        [HttpPost]
        [CustomRoleAuthorize(Roles.UserManagementGroup)]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            return Ok(await _mediator.Send(new CreateUserCommand(dto, GetUserWithCustomClaims(Request, _configuration, _logger), new LogRow().UpdateCallerProperties())));
        }

        [HttpPut("{id:int}")]
        [CustomRoleAuthorize(Roles.UserManagementGroup)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
        {
            return Ok(await _mediator.Send(new UpdateUserCommand(id, dto, GetUserWithCustomClaims(Request, _configuration, _logger), new LogRow().UpdateCallerProperties())));
        }

        [HttpDelete("{id:int}")]
        [CustomRoleAuthorize(Roles.UserManagementGroup)]
        public async Task<IActionResult> Delete(string id)
        {
            await _mediator.Send(new DeleteUserCommand(id, new LogRow().UpdateCallerProperties()));

            return NoContent();
        }
    }
}

